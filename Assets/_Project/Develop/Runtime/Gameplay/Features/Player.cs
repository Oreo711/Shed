using System;
using LitMotion;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using Shed.Runtime.Gameplay.Features.Entrance.States.Player_States;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features
{
	public class Player : MonoBehaviour, IStateContext
	{
		[SerializeField] private Flashlight            _flashlight;
		[SerializeField] private FirstPersonController _controller;
		[SerializeField] private Camera                _camera;
		[SerializeField] private LayerMask             _interactableLayerMask;
		[SerializeField] private AudioSource           _footsteps;
		[SerializeField] private Clock                 _clock;
		[SerializeField] private float                 _maxEnergy;
		[SerializeField] private Rigidbody             _rigidbody;

		[Space]
		[SerializeField] private Collider _batteryBox;
		[SerializeField] private Gun _gun;
		[SerializeField] private Bed _bed;

		private bool  _isRecharging;
		private float _remainingRechargeTime;
		private bool  _isFacingClock;
		private bool  _wasWalking;
		private float _energy;
		private bool  _isActive = true;

		private MonologueService                          _monologueService;
		private StateMachine<Player, PlayerStateTriggers> _stateMachine;

		public bool             IsWalking             => _controller.IsWalking;
		public bool             IsUsingFlashlight     => _flashlight.IsOn;
		public bool             IsRecharging          => _isRecharging;
		public bool             IsFacingClock         => _isFacingClock;
		public float            RemainingRechargeTime => _remainingRechargeTime;
		public float            Energy                => _energy;
		public float            MaxEnergy             => _maxEnergy;
		public Camera           Camera                => _camera;
		public Flashlight       Flashlight            => _flashlight;
		public Gun              Gun                   => _gun;
		public Bed              Bed                   => _bed;
		public MonologueService MonologueService      => _monologueService;
		public Rigidbody        Rigidbody             => _rigidbody;
		public bool             CanNotAct             {get; set;}
		public bool             CanUseBatteryBox      {get; private set;}
		public bool             CanUseCloset          {get; set;}
		public bool             CanUseBed             {get; set;}
		public bool             CanStopUsingBed       {get; set;}
		public bool             IsResting             {get; set;}

		public event Action Caught;

		public void Initialize (MonologueService monologueService)
		{
			_monologueService = monologueService;
		}

		private void Awake ()
		{
			_stateMachine          = BuildStateMachine();
			_remainingRechargeTime = _flashlight.RechargeTime;
			_energy                = _maxEnergy;
		}

		private void Update ()
		{
			if (!_isActive)
			{
				_controller.playerCanMove = false;
				return;
			}

			_stateMachine.Tick(Time.deltaTime);

			_controller.playerCanMove = true;

			if (_flashlight.BatteryPercentage <= 30f)
			{
				_monologueService.ShowLowBatteryMonologue();
			}

			if (_energy / _maxEnergy < 0.25f)
			{
				_monologueService.ShowLowEnergyMonologue();
			}

			if (Input.GetKeyDown(KeyCode.F))
			{
				_flashlight.Toggle();
			}

			if (_controller.IsWalking && !_wasWalking)
			{
				_footsteps.Play();
				_wasWalking = true;
			}

			if (!_controller.IsWalking)
			{
				_footsteps.Stop();
				_wasWalking = false;
			}

			if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, 50, _interactableLayerMask))
			{
				if (hit.collider.gameObject == _clock.gameObject && (_clock.transform.position - transform.position).magnitude <= 4f)
				{
					_isFacingClock = true;
				} else
				{
					_isFacingClock = false;
				}

				if (hit.collider.gameObject == _bed.gameObject && (_bed.transform.position - transform.position).magnitude <= 7f)
				{
					if (!IsResting)
					{
						CanUseBed = true;
					}

					if (Input.GetKeyDown(KeyCode.E))
					{
						_stateMachine.Fire(PlayerStateTriggers.resting);
					}
				} else
				{
					CanUseBed = false;
				}

				if (hit.collider == _batteryBox && (_batteryBox.transform.position - transform.position).magnitude <= 2.5f)
				{
					if (Input.GetKeyDown(KeyCode.E) || _isRecharging)
					{
						if (!_isRecharging)
						{
							_isRecharging = true;
							_flashlight.PlayRechargeSound();
						}

						_remainingRechargeTime -= Time.deltaTime;

						if (_remainingRechargeTime <= 0)
						{
							_flashlight.Recharge();
							_isRecharging = false;
						}
					}

					if (Input.GetKeyUp(KeyCode.E))
					{
						_isRecharging          = false;
						_remainingRechargeTime = _flashlight.RechargeTime;
					}

					CanUseBatteryBox = !_isRecharging;
				} else
				{
					CanUseBatteryBox       = false;
					_isRecharging          = false;
					_remainingRechargeTime = _flashlight.RechargeTime;
					_flashlight.StopRechargeSound();
				}
			} else
			{
				CanUseBatteryBox       = false;
				_isRecharging          = false;
				_isFacingClock         = false;
				_remainingRechargeTime = _flashlight.RechargeTime;
				_flashlight.StopRechargeSound();
			}
		}

		public void SetActive (bool value)
		{
			_isActive = value;
		}

		public void LookAt (Vector3 position)
		{
			DeactivateControls();

			LMotion.Create(0.0f, 1.0f, 0.25f)
			       .WithEase(Ease.InQuad)
			       .WithOnComplete(() => Caught?.Invoke())
			       .Bind(normalizedProgress =>
			       {
				       Vector3    direction      = (position - _camera.transform.position).normalized;
				       Quaternion targetRotation = Quaternion.LookRotation(direction);
				       _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, targetRotation, normalizedProgress);
			       });
		}

		private StateMachine<Player, PlayerStateTriggers> BuildStateMachine ()
		{
			StateMachine<Player, PlayerStateTriggers> stateMachine = new(this);

			stateMachine.AddState<ActiveState>()
			            .AllowTransition<RestingState>(PlayerStateTriggers.resting)
			            .SetAsInitial();

			stateMachine.AddState<RestingState>()
			            .AllowTransition<ActiveState>(PlayerStateTriggers.active);

			stateMachine.Start();

			return stateMachine;
		}

		public void DeactivateControls ()
		{
			_controller.cameraCanMove = false;
			_controller.playerCanMove = false;
			_controller.enableHeadBob = false;
		}

		public void ActivateControls ()
		{
			_controller.cameraCanMove = true;
			_controller.playerCanMove = true;
			_controller.enableHeadBob = true;
		}

		public void SpendEnergy (float value)
		{
			if (_energy - value < 0)
			{
				_energy = 0;
			} else
			{
				_energy -= value;
			}
		}

		public void RegainEnergy (float value)
		{
			if (_energy + value > _maxEnergy)
			{
				_energy = _maxEnergy;
			} else
			{
				_energy += value;
			}
		}

		public void StopClockSound ()
		{
			_clock.StopClockSound();
		}
	}
}
