using System;
using _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Door_States;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public class Door : BreachSource, IStateContext
	{
		[SerializeField] private Player _player;
		[SerializeField] private float  _preparationTime;
		[SerializeField] private float  _pacifyTime;

		private StateMachine<Door, EntranceTriggers> _stateMachine;

		public override event Action<IBreachSource> Breached;

		public Player Player          => _player;
		public float  PreparationTime => _preparationTime;
		public float  PacifyTime      => _pacifyTime;

		private void Awake ()
		{
			_stateMachine = BuildStateMachine();
		}

		private void Update ()
		{
			_stateMachine?.Tick(Time.deltaTime);
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

			stateMachine.AddState<DoorBreachState>();

			stateMachine.Start();

			return stateMachine;
		}

		public override void Activate ()
		{
			_stateMachine.Fire(EntranceTriggers.Warning);
		}

		public void Breach ()
		{
			Breached?.Invoke(this);
		}
	}
}
