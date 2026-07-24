using System;
using System.Collections;
using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.Gameplay.Features.Entrance;
using _Project.Develop.Runtime.Gameplay.Infrastructure.Gameplay_States;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.SceneManagment;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap, IStateContext
    {
        [SerializeField] private Door   _door;
        [SerializeField] private Window _window;
        [SerializeField] private Window _otherWindow;
        [SerializeField] private Closet _closet;

        private DIContainer              _container;
        private Timer                    _timer;
        private EntranceActivatorService _entranceActivatorService;
        private SceneSwitcherService     _sceneSwitcherService;

        private StateMachine<GameplayBootstrap, GameplayTriggers> _stateMachine;

        public override void ProcessRegistrations(DIContainer container)
        {
            _container = container;

            GameplayContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            _timer                    = _container.Resolve<Timer>();
            _sceneSwitcherService     = _container.Resolve<SceneSwitcherService>();
            _entranceActivatorService = _container.Resolve<EntranceActivatorService>();

            _entranceActivatorService.EntranceActivated += ActivateEntrance;
            _window.Breached                            += OnBreached;
            _timer.NightPassed                          += OnNightPassed;

            yield break;
        }

        private void ActivateEntrance (Entrances entrance)
        {
            switch (entrance)
            {
                case Entrances.door:
                    _door.Activate();
                    break;
                case Entrances.window:
                    _window.Activate();
                    break;
                case Entrances.otherWindow:
                    _otherWindow.Activate();
                    break;
                case Entrances.closet:
                    _closet.Activate();
                    break;
            }
        }

        public override void Run()
        {
            _stateMachine = BuildStateMachine();
            _timer.Launch();
            _entranceActivatorService.Run();
        }

        private void Update ()
        {
            _stateMachine?.Tick(Time.deltaTime);
        }

        private StateMachine<GameplayBootstrap, GameplayTriggers> BuildStateMachine ()
        {
            StateMachine<GameplayBootstrap, GameplayTriggers> stateMachine = new(this);

            stateMachine.AddState<GameplayState>()
                        .AllowTransition<WinState>(GameplayTriggers.win)
                        .AllowTransition<LossState>(GameplayTriggers.loss)
                        .SetAsInitial();

            stateMachine.AddState<WinState>();

            stateMachine.AddState<LossState>();

            stateMachine.Start();

            return stateMachine;
        }

        private void OnNightPassed ()
        {
            _stateMachine.Fire(GameplayTriggers.win);
            _timer.Stop();
            _entranceActivatorService.Stop();
        }

        private void OnBreached ()
        {
            _stateMachine.Fire(GameplayTriggers.loss);
            _timer.Stop();
            _entranceActivatorService.Stop();
        }
    }
}
