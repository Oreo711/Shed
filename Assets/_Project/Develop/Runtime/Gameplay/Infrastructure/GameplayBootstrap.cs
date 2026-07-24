using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.Gameplay.Features.Entrance;
using _Project.Develop.Runtime.Gameplay.Infrastructure.Gameplay_States;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.SceneManagment;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
	public class GameplayBootstrap : SceneBootstrap, IStateContext
	{
		[SerializeField] private List<BreachSource> _breachSources;

		private DIContainer              _container;
		private Timer                    _timer;
		private EntranceActivatorService _entranceActivatorService;
		private SceneSwitcherService     _sceneSwitcherService;

		private StateMachine<GameplayBootstrap, GameplayTriggers> _stateMachine;

		private bool _isRunning;

		public override void ProcessRegistrations (DIContainer container)
		{
			_container = container;

			GameplayContextRegistrations.Process(_container);
		}

		public override IEnumerator Initialize ()
		{
			_timer                    = _container.Resolve<Timer>();
			_sceneSwitcherService     = _container.Resolve<SceneSwitcherService>();
			_entranceActivatorService = _container.Resolve<EntranceActivatorService>();
			_entranceActivatorService.Initialize(_breachSources);

			_entranceActivatorService.EntranceBreached += OnBreached;
			_timer.NightPassed                         += OnNightPassed;

			yield break;
		}

		public override void Run ()
		{
			_stateMachine = BuildStateMachine();
			_timer.Start();
			_entranceActivatorService.Run();

			_isRunning = true;
		}

		private void Update ()
		{
			if (!_isRunning)
				return;

			_stateMachine.Tick(Time.deltaTime);
		}

		private StateMachine<GameplayBootstrap, GameplayTriggers> BuildStateMachine ()
		{
			StateMachine<GameplayBootstrap, GameplayTriggers> stateMachine = new(this);

			stateMachine.AddState<GameplayState>()
			            .AllowTransition<WinState>(GameplayTriggers.Win)
			            .AllowTransition<LossState>(GameplayTriggers.Loss)
			            .SetAsInitial();

			stateMachine.AddState<WinState>();

			stateMachine.AddState<LossState>();

			stateMachine.Start();

			return stateMachine;
		}

		private void OnNightPassed ()
		{
			HandleSessionEnded();
			_stateMachine.Fire(GameplayTriggers.Win);
		}

		private void OnBreached (IBreachSource _)
		{
			HandleSessionEnded();
			_stateMachine.Fire(GameplayTriggers.Loss);
		}

		private void HandleSessionEnded ()
		{
			_entranceActivatorService.EntranceBreached -= OnBreached;

			_timer.Stop();
			_entranceActivatorService.Stop();
		}

		private void OnDestroy ()
		{
			Teardown();
		}

		private void Teardown ()
		{
			_timer.Stop();
			_entranceActivatorService.Stop();
			_stateMachine?.Stop();
			_isRunning = false;

			_entranceActivatorService.EntranceBreached -= OnBreached;
			_timer.NightPassed                         -= OnNightPassed;
		}
	}
}
