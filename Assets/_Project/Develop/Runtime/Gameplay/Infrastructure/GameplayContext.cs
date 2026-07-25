using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.Gameplay.Features.Entrance;
using _Project.Develop.Runtime.Gameplay.Infrastructure.GameplayStates;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.UI.Gameplay;
using _Project.Develop.Runtime.Utilities.SceneManagment;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
	public class GameplayContext : SceneBootstrap, IStateContext
	{
		[SerializeField] private List<BreachSourceBase> _breachSources;
		[SerializeField] private Player                 _player;
		[SerializeField] private Gun                    _gun;

		private DIContainer              _container;
		private GameTimer                _gameTimer;
		private EntranceActivatorService _entranceActivatorService;
		private PlayerHolderService      _playerHolderService;
		private SceneSwitcherService     _sceneSwitcherService;

		private GameplayScreenPresenter _screen;

		private StateMachine<GameplayContext, GameplayTriggers> _stateMachine;

		private bool _isRunning;

		public override void ProcessRegistrations (DIContainer container)
		{
			_container = container;

			GameplayContextRegistrations.Process(_container);
		}

		public override IEnumerator Initialize ()
		{
			_gameTimer                = _container.Resolve<GameTimer>();
			_sceneSwitcherService     = _container.Resolve<SceneSwitcherService>();
			_entranceActivatorService = _container.Resolve<EntranceActivatorService>();
			_playerHolderService      = _container.Resolve<PlayerHolderService>();
			_screen                   = _container.Resolve<GameplayScreenPresenter>();

			_entranceActivatorService.Initialize(_breachSources);
			_playerHolderService.Initialize(_player);

			_gameTimer.Expired                         += HandleNightPassed;
			_entranceActivatorService.EntranceBreached += HandleBreached;

			yield break;
		}

		public override void Run ()
		{
			_stateMachine = BuildStateMachine();
			_gameTimer.Start();
			_entranceActivatorService.Run();

			_isRunning = true;
		}

		private void Update ()
		{
			if (!_isRunning)
				return;

			float deltaTime = Time.deltaTime;

			_stateMachine.Tick(deltaTime);

			if (!_isRunning)
				return;

			_gameTimer.Tick(deltaTime);
			_entranceActivatorService.Tick(deltaTime);
		}

		private void LateUpdate ()
		{
			if (!_isRunning)
				return;

			_screen.Tick();
		}

		private StateMachine<GameplayContext, GameplayTriggers> BuildStateMachine ()
		{
			StateMachine<GameplayContext, GameplayTriggers> stateMachine = new(this);

			stateMachine.AddState<GameplayState>()
			            .AllowTransition<JumpScareState>(GameplayTriggers.JumpScare)
			            .AllowTransition<EndSessionState>(GameplayTriggers.EndSession)
			            .SetAsInitial();

			stateMachine.AddState<JumpScareState>()
			            .AllowTransition<GameplayState>(GameplayTriggers.Gameplay)
			            .AllowTransition<EndSessionState>(GameplayTriggers.EndSession);

			stateMachine.AddState<EndSessionState>();

			stateMachine.Start();

			return stateMachine;
		}

		public bool TryShoot ()
		{
			if (!_gun.TryPerformShot())
				return false;

			foreach (BreachSourceBase source in _breachSources)
				source.HandleGunshot();

			return true;
		}

		public void Lose ()
		{
			EndSession(SessionResult.Loss);
		}

		public void PauseEntranceActivator ()
		{
			_entranceActivatorService.Pause();
		}

		public void ResumeEntranceActivator ()
		{
			_entranceActivatorService.Resume();
		}

		private void HandleNightPassed ()
		{
			EndSession(SessionResult.Win);
		}

		private void HandleBreached (BreachSourceBase _)
		{
			if (_gun.Bullets > 0)
			{
				_stateMachine.Fire(GameplayTriggers.JumpScare);
				return;
			}

			EndSession(SessionResult.Loss);
		}

		private void EndSession (SessionResult result)
		{
			TeardownSession();
			ShowEndSessionScreen(result);

			_stateMachine.Fire(GameplayTriggers.EndSession);
		}

		private void TeardownSession ()
		{
			_gameTimer.Expired                         -= HandleNightPassed;
			_entranceActivatorService.EntranceBreached -= HandleBreached;

			_player.SetActive(false);
			_gameTimer.Stop();
			_entranceActivatorService.Stop();
			_screen.Dispose();

			_isRunning = false;
		}

		private void Teardown ()
		{
			TeardownSession();
			_stateMachine?.Stop();
		}

		private static void ShowEndSessionScreen (SessionResult result)
		{
			// TODO: Implement end session screen logic
			switch (result)
			{
				case SessionResult.Loss:
					Debug.Log("You Lose.");
					break;
				case SessionResult.Win:
					Debug.Log("You Win!");
					break;
				default:
					Debug.LogError($"Unknown session result: {result}");
					break;
			}
		}

		private void OnDestroy ()
		{
			Teardown();
		}
	}
}
