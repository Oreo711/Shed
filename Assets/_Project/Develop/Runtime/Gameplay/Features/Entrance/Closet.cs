using System;
using _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Closet_States;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public class Closet : MonoBehaviour, IStateContext
	{
		public event Action Breached;

		[SerializeField] private Player    _player;
		[SerializeField] private Transform _door;
		[SerializeField] private float     _breachTime;
		[SerializeField] private float     _pacifyTime;

		private bool _isBeingPacified;

		public Player    Player          => _player;
		public Transform Door            => _door;
		public float     BreachTime      => _breachTime;
		public float     PacifyTime      => _pacifyTime;
		public bool      IsBeingPacified => _isBeingPacified;

		private StateMachine<Closet, EntranceTriggers> _stateMachine;

		private void Awake ()
		{
			_stateMachine = BuildStateMachine();
		}

		private void Update ()
		{
			_stateMachine?.Tick(Time.deltaTime);
		}

		private StateMachine<Closet, EntranceTriggers> BuildStateMachine ()
		{
			StateMachine<Closet, EntranceTriggers> stateMachine = new(this);

			stateMachine.AddState<ClosetIdleState>()
			            .AllowTransition<ClosetWarningState>(EntranceTriggers.warning)
			            .SetAsInitial();

			stateMachine.AddState<ClosetWarningState>()
			            .AllowTransition<ClosetIdleState>(EntranceTriggers.idle)
			            .AllowTransition<ClosetBreachState>(EntranceTriggers.breach);

			stateMachine.AddState<ClosetBreachState>();

			stateMachine.Start();

			return stateMachine;
		}

		public void Activate ()
		{
			_stateMachine.Fire(EntranceTriggers.warning);
		}

		private void OnMouseDown()
		{
			_isBeingPacified = true;
		}

		private void OnMouseUp()
		{
			_isBeingPacified = false;
		}

		public void Breach ()
		{
			Breached?.Invoke();
		}
	}
}
