using System;
using _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Closet_States;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public class Closet : BreachSource, IStateContext
	{
		[SerializeField] private Player    _player;
		[SerializeField] private Transform _door;
		[SerializeField] private float     _breachTime;
		[SerializeField] private float     _pacifyTime;

		private StateMachine<Closet, EntranceTriggers> _stateMachine;

		private bool _isBeingPacified;

		public Player    Player          => _player;
		public Transform Door            => _door;
		public float     BreachTime      => _breachTime;
		public float     PacifyTime      => _pacifyTime;
		public bool      IsBeingPacified => _isBeingPacified;

		public override event Action<IBreachSource> Breached;

		private void Awake ()
		{
			_stateMachine = BuildStateMachine();
		}

		private void Update ()
		{
			_stateMachine.Tick(Time.deltaTime);
		}

		public override void Activate ()
		{
			_stateMachine.Fire(EntranceTriggers.Warning);
		}

		private StateMachine<Closet, EntranceTriggers> BuildStateMachine ()
		{
			StateMachine<Closet, EntranceTriggers> stateMachine = new(this);

			stateMachine.AddState<ClosetIdleState>()
			            .AllowTransition<ClosetWarningState>(EntranceTriggers.Warning)
			            .SetAsInitial();

			stateMachine.AddState<ClosetWarningState>()
			            .AllowTransition<ClosetIdleState>(EntranceTriggers.Idle)
			            .AllowTransition<ClosetBreachState>(EntranceTriggers.Breach);

			stateMachine.AddState<ClosetBreachState>();

			stateMachine.Start();

			return stateMachine;
		}

		private void OnMouseDown ()
		{
			_isBeingPacified = true;
		}

		private void OnMouseUp ()
		{
			// TODO: убедиться что работает корректно, если мышь отпущена за пределами коллайдера
			_isBeingPacified = false;
		}

		public void Breach ()
		{
			Breached?.Invoke(this);
		}
	}
}
