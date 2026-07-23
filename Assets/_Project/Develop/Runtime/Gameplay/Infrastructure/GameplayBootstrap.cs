using System;
using System.Collections;
using _Project.Develop.Runtime.Gameplay.Features.Entrance;
using _Project.Develop.Runtime.Gameplay.Infrastructure.Gameplay_States;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.AssetManagment;
using _Project.Develop.Runtime.Utilities.ConfigManagement;
using Modules.StateMachineLite;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using Unity.VisualScripting;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap, IStateContext
    {
        [SerializeField] private Window _window;
        [SerializeField] private Door   _door;

        private DIContainer           _container;
        private ConfigProviderService _configProviderService;
        private ResourcesAssetLoader  _resourcesAssetLoader;

        private StateMachine<GameplayBootstrap, GameplayTriggers> _stateMachine;

        public override void ProcessRegistrations(DIContainer container)
        {
            _container = container;

            GameplayContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            _configProviderService = _container.Resolve<ConfigProviderService>();
            _resourcesAssetLoader  = _container.Resolve<ResourcesAssetLoader>();

            _window.Breached += OnBreached;

            yield break;
        }

        public override void Run()
        {
            _stateMachine = BuildStateMachine();
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

        private void OnBreached ()
        {
            _stateMachine.Fire(GameplayTriggers.loss);
        }
    }
}
