using System.Collections;
using System.Collections.Generic;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using Shed.Runtime.Gameplay.Features;
using Shed.Runtime.Gameplay.Features.Entrance;
using Shed.Runtime.Gameplay.Infrastructure.GameplayStates;
using Shed.Runtime.Infrastructure;
using Shed.Runtime.Infrastructure.DI;
using Shed.Runtime.UI.Gameplay;
using Shed.Runtime.Utilities.CoroutineManagement;
using Shed.Runtime.Utilities.SceneManagment;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Infrastructure
{
	public class GameplayContext : SceneBootstrap, IStateContext
	{
		[SerializeField] private List<BreachSourceBase> _mainCycleBreachSources;
		[SerializeField] private List<BreachSourceBase> _additionalBreachSources;
		[SerializeField] private Player                 _player;
		[SerializeField] private Clock                  _clock;

		private DIContainer              _container;
		private GameTimer                _gameTimer;
		private EntranceActivatorService _entranceActivatorService;
		private PlayerHolderService      _playerHolderService;
		private SceneSwitcherService     _sceneSwitcherService;
		private ICoroutinePerformer      _coroutinePerformer;

		private GameplayScreenPresenter _screen;

		private StateMachine<GameplayContext, GameplayTriggers> _stateMachine;

		private bool _isRunning;

		public Player Player => _player;

		private Vector3 _lastBreachedSourcePosition;


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
			_coroutinePerformer       = _container.Resolve<ICoroutinePerformer>();


			_entranceActivatorService.Initialize(_mainCycleBreachSources, _additionalBreachSources);
			_playerHolderService.Initialize(_player);

			_screen = _container.Resolve<GameplayScreenPresenter>();

			MonologueService monologueService = _container.Resolve<MonologueService>();
			_player.Initialize(monologueService);

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
			_clock.SetTime(_gameTimer.Hours, _gameTimer.Minutes);

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
			if (!_player.Gun.TryPerformShot())
				return false;

			foreach (BreachSourceBase source in _mainCycleBreachSources)
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

		private void HandleBreached (BreachSourceBase breachSource)
		{
			_lastBreachedSourcePosition = breachSource.LookAtSpot;

			_stateMachine.Fire(GameplayTriggers.JumpScare);
		}

		public void SwitchToMenu ()
		{
			_coroutinePerformer.StartCoroutine(_sceneSwitcherService.SwitchToAsync(Scenes.MainMenu));
		}

		private void EndSession (SessionResult result)
		{
			_stateMachine.Fire(GameplayTriggers.EndSession);
			ShowEndSessionScreen(result);

			TeardownSession();
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

		private void ShowEndSessionScreen (SessionResult result)
		{
			switch (result)
			{
				case SessionResult.Loss:
					_screen.ShowLossScreen();
					break;
				case SessionResult.Win:
					_screen.ShowWinScreen();
					break;
				default:
					Debug.LogError($"Unknown session result: {result}");
					break;
			}
		}

		public void AnimateJumpScare ()
		{
			_player.LookAt(_lastBreachedSourcePosition);
		}

		public void CancelJumpScare ()
		{
			_screen.CancelJumpScare();
		}

		public void StopClockSound ()
		{
			_player.StopClockSound();
		}

		private void OnDestroy ()
		{
			Teardown();
		}
	}
}
