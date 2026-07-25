using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features
{
	public class Flashlight : MonoBehaviour
	{
		[SerializeField] private Light       _spotLight;
		[SerializeField] private float       _rechargeTime;
		[SerializeField] private AudioSource _lowBatteryPing;
		[SerializeField] private AudioSource _toggleSound;
		[SerializeField] private AudioSource _batteryReplacementSound;

		private bool _isOn;
		private float _batteryPercentage;

		public bool  IsOn              => _isOn;
		public float BatteryPercentage => _batteryPercentage;
		public float RechargeTime => _rechargeTime;

		private void Awake ()
		{
			_spotLight.gameObject.SetActive(false);
			_batteryPercentage = 100;
		}

		private void FixedUpdate ()
		{
			if (IsOn)
			{
				if (_batteryPercentage <= 10)
				{
					_lowBatteryPing.Play();
				}

				if (_batteryPercentage <= 0)
				{
					_batteryPercentage = 0;
					TurnOff();
				}

				_batteryPercentage -= 0.1f;
			}
		}

		public void Toggle()
		{
			_toggleSound.Play();

			if (IsOn)
			{
				TurnOff();
			}
			else
			{
				TurnOn();
			}
		}

		public void Recharge ()
		{
			_batteryPercentage = 100;
		}

		public void PlayRechargeSound ()
		{
			_batteryReplacementSound.Play();
		}

		public void StopRechargeSound ()
		{
			_batteryReplacementSound.Stop();
		}

		private void TurnOff ()
		{
			_isOn = false;
			_spotLight.gameObject.SetActive(false);
		}

		private void TurnOn ()
		{
			if (_batteryPercentage <= 0)
			{
				return;
			}

			_isOn = true;
			_spotLight.gameObject.SetActive(true);
		}
	}
}
