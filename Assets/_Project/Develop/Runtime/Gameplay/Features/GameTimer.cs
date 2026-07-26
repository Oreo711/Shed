using System;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features
{
	public class GameTimer
	{
		public int Minutes {get; private set;}
		public int Hours   {get; private set;} = -1;

		private readonly CountdownTimer _countdownTimer;

		private bool _isRunning;

		public event Action HourPassed;

		public event Action Expired
		{
			add => _countdownTimer.Expired += value;
			remove => _countdownTimer.Expired -= value;
		}

		public GameTimer ()
		{
			const float sessionHours = 60 * 7;
			_countdownTimer = new CountdownTimer(sessionHours);
		}

		private int CalculateHours ()
		{
			return Mathf.FloorToInt(_countdownTimer.RemainingTime / 60);
		}

		public void Start ()
		{
			_isRunning = true;
		}

		public void Stop ()
		{
			_isRunning = false;
		}

		public void Tick (float deltaTime)
		{
			if (!_isRunning)
				return;

			_countdownTimer.Tick(deltaTime);

			Minutes = Mathf.FloorToInt(_countdownTimer.RemainingTime) % 60;

			int currentHours = CalculateHours();

			int hours = Hours == -1 ? currentHours : Hours;

			Hours = currentHours;

			if (hours != Hours)
				HourPassed?.Invoke();
		}
	}
}
