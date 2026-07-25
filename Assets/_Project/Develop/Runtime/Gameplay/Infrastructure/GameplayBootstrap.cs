using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.Gameplay.Features.Entrance;
using _Project.Develop.Runtime.Gameplay.Infrastructure.Gameplay_States;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.UI.Gameplay;
using _Project.Develop.Runtime.Utilities.SceneManagment;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
	public class GameplayBootstrap : SceneBootstrap, IStateContext
	{
		[SerializeField] private List<BreachSource> _breachSources;
		[SerializeField] private Player             _player;

		private DIContainer              _container;
		private Timer                    _timer;
		private EntranceActivatorService _entranceActivatorService;
		private PlayerHolderService      _playerHolderService;
		private SceneSwitcherService     _sceneSwitcherService;

		private GameplayScreenPresenter _screen;

		private StateMachine<GameplayBootstrap, GameplayTriggers> _stateMachine;

		private bool _isRunning;

		public Player Player => _player;

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
			_playerHolderService      = _container.Resolve<PlayerHolderService>();
			_screen                   = _container.Resolve<GameplayScreenPresenter>();

			_entranceActivatorService.Initialize(_breachSources);
			_playerHolderService.Initialize(_player);

			_entranceActivatorService.EntranceBreached += OnBreached;

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
			_screen.Update(Time.deltaTime);

			if (_timer.Hours >= 7)
			{
				OnNightPassed();
			}
		}

		private StateMachine<GameplayBootstrap, GameplayTriggers> BuildStateMachine ()
		{
			StateMachine<GameplayBootstrap, GameplayTriggers> stateMachine = new(this);

			stateMachine.AddState<GameplayState>()
			            .AllowTransition<WinState>(GameplayTriggers.Win)
			            .AllowTransition<GunshotState>(GameplayTriggers.Gunshot)
			            .AllowTransition<LossState>(GameplayTriggers.Loss)
			            .SetAsInitial();

			stateMachine.AddState<WinState>();

			stateMachine.AddState<GunshotState>()
			            .AllowTransition<GameplayState>(GameplayTriggers.Gameplay)
			            .AllowTransition<LossState>(GameplayTriggers.Loss);

			stateMachine.AddState<LossState>();

			stateMachine.Start();

			return stateMachine;
		}

		private void OnNightPassed ()
		{
			HandleSessionEnded();
			_stateMachine.Fire(GameplayTriggers.Win);
		}

		public void OnSuccessfulGunshot ()
		{
			_player.OnSuccessfulGunshot();
		}

		private void OnBreached (IBreachSource _)
		{
			if (_player.Gun.Bullets > 0)
			{
				_stateMachine.Fire(GameplayTriggers.Gunshot);

				return;
			}

			HandleSessionEnded();
			_stateMachine.Fire(GameplayTriggers.Loss);
		}

		public void HandleSessionEnded ()
		{
			_entranceActivatorService.EntranceBreached -= OnBreached;

			_timer.Stop();
			_entranceActivatorService.Stop();
			_screen.Dispose();
		}

		private void OnDestroy ()
		{
			Teardown();
		}

		private void Teardown ()
		{
			_timer.Stop();
			_entranceActivatorService.Stop();
			_screen.Dispose();
			_stateMachine?.Stop();
			_isRunning = false;

			_entranceActivatorService.EntranceBreached -= OnBreached;
		}
	}
}
