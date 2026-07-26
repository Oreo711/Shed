using System;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using Shed.Runtime.Gameplay.Features.Entrance.States.Closet_States;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance
{
	public class Closet : BreachSource<Closet>, IStateContext
	{
		[SerializeField] private Player      _player;
		[SerializeField] private float       _breachTime;
		[SerializeField] private float       _pacifyTime;
		[SerializeField] private float       _doorOpenDuration = 1.0f;
		[SerializeField] private Transform   _doorTransform;
		[SerializeField] private float       _openDoorAngle = 20f;
		[SerializeField] private AudioSource _pacifiedSound;

		private bool _isBeingPacified;

		public Player    Player           => _player;
		public float     BreachTime       => _breachTime;
		public float     PacifyTime       => _pacifyTime;
		public bool      IsBeingPacified  => _isBeingPacified;
		public float     DoorOpenDuration => _doorOpenDuration;
		public Transform DoorTransform    => _doorTransform;
		public float     OpenDoorAngle    => _openDoorAngle;

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
					}

					if (Input.GetKeyUp(KeyCode.E))
					{
						_isBeingPacified = false;
					}

					_player.CanUseCloset = !_isBeingPacified;
				} else
				{
					_player.CanUseCloset = false;
				}
			} else
			{
				_isBeingPacified     = false;
				_player.CanUseCloset = false;
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

		public void PlayPacifiedSound ()
		{
			_pacifiedSound.Play();
		}

		public void Breach ()
		{
			Breached?.Invoke(this);
		}
	}
}
