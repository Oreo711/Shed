using System;
using _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Closet_States;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public class Closet : BreachSource<Closet>, IStateContext
	{
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

		public override event Action<BreachSourceBase> Breached;

		private void Awake ()
		{
			StateMachine = BuildStateMachine();
		}

		private void Update ()
		{
			StateMachine.Tick(Time.deltaTime);

			if (Physics.Raycast(_player.Camera.transform.position, _player.Camera.transform.forward, out RaycastHit hit, 2.5f, LayerMask.GetMask("Entrance")))
			{
				if (hit.collider.gameObject == gameObject)
				{
					if (Input.GetKeyDown(KeyCode.E) || _isBeingPacified)
					{
						if (!_isBeingPacified)
						{
							_isBeingPacified = true;
						}

						_player.IsFacingCloset = false;
					} else
					{
						_player.IsFacingCloset = true;
					}

					if (Input.GetKeyUp(KeyCode.E))
					{
						_isBeingPacified = false;
					}
				}
			} else
			{
				_isBeingPacified = false;
			}
		}

		public override void Activate ()
		{
			StateMachine.Fire(EntranceTriggers.Warning);
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

		public void Breach ()
		{
			Breached?.Invoke(this);
		}
	}
}
