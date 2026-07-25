using System;
using _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Door_States;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public class Door : BreachSource<Door>, IStateContext
	{
		[SerializeField] private Player      _player;
		[SerializeField] private float       _preparationTime;
		[SerializeField] private float       _pacifyTime;
		[SerializeField] private AudioSource _openSound;
		[SerializeField] private AudioSource _closeSound;

		public override event Action<BreachSourceBase> Breached;

		public Player Player          => _player;
		public float  PreparationTime => _preparationTime;
		public float  PacifyTime      => _pacifyTime;

		private void Awake ()
		{
			StateMachine = BuildStateMachine();
		}

		private void Update ()
		{
			StateMachine.Tick(Time.deltaTime);
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
