using System;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using Shed.Runtime.Gameplay.Features.Entrance.States.Door_States;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance
{
	public class Door : BreachSource<Door>, IStateContext
	{
		[SerializeField] private Player      _player;
		[SerializeField] private float       _preparationTime;
		[SerializeField] private float       _pacifyTime;
		[SerializeField] private float       _timeBeforeKnock;
		[SerializeField] private AudioSource _openSound;
		[SerializeField] private AudioSource _closeSound;
		[SerializeField] private AudioSource _knockingSound;

		public override event Action<BreachSourceBase> Breached;

		public Player Player          => _player;
		public float  PreparationTime => _preparationTime;
		public float  PacifyTime      => _pacifyTime;

		private float _remainingTimeBeforeKnock;
		private bool  _hasKnocked;

		private void Awake ()
		{
			StateMachine              = BuildStateMachine();
			_remainingTimeBeforeKnock = _timeBeforeKnock;
		}

		private void Update ()
		{
			StateMachine.Tick(Time.deltaTime);

			if (_remainingTimeBeforeKnock > 0)
			{
				_remainingTimeBeforeKnock -= Time.deltaTime;
			}

			if (_remainingTimeBeforeKnock <= 0 && !_hasKnocked)
			{
				_hasKnocked           = true;
				_player.HeardKnocking = true;
				_knockingSound.Play();
			}
		}

		private StateMachine<Door, EntranceTriggers> BuildStateMachine ()
		{
			StateMachine<Door, EntranceTriggers> stateMachine = new(this);

			stateMachine.AddState<DoorIdleState>()
			            .AllowTransition<DoorWarningState>(EntranceTriggers.Warning)
			            .SetAsInitial();

			stateMachine.AddState<DoorWarningState>()
			            .AllowTransition<DoorIdleState>(EntranceTriggers.Idle)
			            .AllowTransition<DoorBreachState>(EntranceTriggers.Breach);

			stateMachine.AddState<DoorBreachState>()
			            .AllowTransition<DoorIdleState>(EntranceTriggers.Idle);

			stateMachine.Start();

			return stateMachine;
		}

		public override void Activate ()
		{
			StateMachine.Fire(EntranceTriggers.Warning);
		}

		public void PlayOpenSound ()
		{
			_openSound.Play();
		}

		public void PlayCloseSound ()
		{
			_closeSound.Play();
		}

		public void Breach ()
		{
			Breached?.Invoke(this);
		}
	}
}
