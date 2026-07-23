using System;
using _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Door_States;
using _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Window_States;
using Modules.StateMachineLite;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public class Door : MonoBehaviour, IStateContext
	{
		public event Action Breached;

		[SerializeField] private Player _player;
		[SerializeField] private float  _preparationTime;
		[SerializeField] private float  _pacifyTime;

		public Player Player          => _player;
		public float  PreparationTime => _preparationTime;
		public float  PacifyTime      => _pacifyTime;

		private StateMachine<Door, EntranceTriggers> _stateMachine;

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
			            .AllowTransition<DoorWarningState>(EntranceTriggers.warning)
			            .SetAsInitial();

			stateMachine.AddState<DoorWarningState>()
			            .AllowTransition<DoorIdleState>(EntranceTriggers.idle)
			            .AllowTransition<DoorBreachState>(EntranceTriggers.breach);

			stateMachine.AddState<DoorBreachState>();

			stateMachine.Start();

			return stateMachine;
		}

		public void Breach ()
		{
			Breached?.Invoke();
		}
	}
}
