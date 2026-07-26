using System;
using LitMotion;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features
{
	public class Player : MonoBehaviour
	{
		[SerializeField] private Flashlight            _flashlight;
		[SerializeField] private FirstPersonController _controller;
		[SerializeField] private Camera                _camera;
		[SerializeField] private LayerMask             _interactableLayerMask;
		[SerializeField] private AudioSource           _footsteps;
		[SerializeField] private AudioSource           _clockSound;

		[Space]
		[SerializeField] private Collider _batteryBox;
		[SerializeField] private Collider _clock;
		[SerializeField] private Gun      _gun;

		[HideInInspector] public bool HeardKnocking;
		[HideInInspector] public bool EncounteredDoor;
		[HideInInspector] public bool EncounteredWindow;
		[HideInInspector] public bool EncounteredCloset;
		[HideInInspector] public bool PacifiedCloset;
		[HideInInspector] public bool IsLowOnBattery;
		[HideInInspector] public bool Shot;

		private bool  _isRecharging;
		private float _remainingRechargeTime;
		private bool  _isFacingClock;
		private bool  _wasWalking;
		private bool  _isActive = true;

		private readonly MonologueMilestones _monologueMilestones = new();

		public bool                IsWalking             => _controller.IsWalking;
		public bool                IsUsingFlashlight     => _flashlight.IsOn;
		public bool                IsRecharging          => _isRecharging;
		public bool                IsFacingClock         => _isFacingClock;
		public float               RemainingRechargeTime => _remainingRechargeTime;
		public Camera              Camera                => _camera;
		public Flashlight          Flashlight            => _flashlight;
		public Gun                 Gun                   => _gun;
		public MonologueMilestones MonologueMilestones   => _monologueMilestones;
		public bool                CanNotAct             {get; set;}
		public bool                CanUseBatteryBox      {get; private set;}
		public bool                CanUseCloset          {get; set;}

		public event Action Caught;

		private void Awake ()
		{
			_remainingRechargeTime = _flashlight.RechargeTime;
		}

		private void Update ()
		{
			if (!_isActive)
			{
				_controller.playerCanMove = false;
				return;
			}

			_controller.playerCanMove = true;

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

			if (_flashlight.BatteryPercentage <= 30)
			{
				IsLowOnBattery = true;
			}

			if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, 50, _interactableLayerMask))
			{
				if (hit.collider == _clock && (_clock.transform.position - transform.position).magnitude <= 7f)
				{
					_isFacingClock = true;
				} else
				{
					_isFacingClock = false;
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

		public void DeactivateControls ()
		{
			_controller.cameraCanMove = false;
			_controller.playerCanMove = false;
		}

		public void ActivateControls ()
		{
			_controller.cameraCanMove = true;
			_controller.playerCanMove = true;
		}

		public void StopClockSound ()
		{
			_clockSound.Stop();
		}
	}
}
