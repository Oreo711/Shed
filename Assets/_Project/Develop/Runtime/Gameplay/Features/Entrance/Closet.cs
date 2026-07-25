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

			if (Physics.Raycast(_player.Camera.transform.position, _player.Camera.transform.forward, out RaycastHit hit, 2.5f, LayerMask.GetMask("Entrance")))
			{
				_isBeingPacified = hit.collider.gameObject == gameObject;
			}
			else
			{
				_isBeingPacified = false;
			}
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

			stateMachine.AddState<ClosetBreachState>()
			            .AllowTransition<ClosetIdleState>(EntranceTriggers.Idle);

			stateMachine.Start();

			return stateMachine;
		}

		public override void Reset ()
		{
			_stateMachine.Fire(EntranceTriggers.Idle);
		}

		public void Breach ()
		{
			Breached?.Invoke(this);
		}
	}
}
