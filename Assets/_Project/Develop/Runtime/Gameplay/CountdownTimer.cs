using System;
using JetBrains.Annotations;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay
{
	[PublicAPI]
	public class CountdownTimer
	{
		private float _duration;
		private float _timeElapsed;

		public event Action Expired;

		public bool  IsRunning          => _timeElapsed < _duration;
		public float RemainingTime      => Mathf.Max(0f, _duration - _timeElapsed);
		public float NormalizedProgress => Mathf.Clamp01(_timeElapsed / _duration);

		public CountdownTimer (float seconds)
		{
			if (seconds <= 0f)
				throw new ArgumentOutOfRangeException(nameof(seconds), "Duration must be greater than zero.");

			_duration    = seconds;
			_timeElapsed = 0;
		}

		public CountdownTimer (TimeSpan duration)
		{
			if (duration.TotalSeconds <= 0.0)
				throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be greater than zero.");

			_duration    = (float)duration.TotalSeconds;
			_timeElapsed = 0;
		}

		public void IncreaseDuration (float seconds)
		{
			if (seconds <= 0f)
				throw new ArgumentOutOfRangeException(nameof(seconds), "Additional seconds must be greater than zero.");

			_duration += seconds;
		}

		public void Tick (float deltaTime)
		{
			if (!IsRunning)
				return;

			_timeElapsed += deltaTime;

			if (!IsRunning)
			{
				Expired?.Invoke();
			}
		}

		public void Tick (float deltaTime, Action<float> onTick)
		{
			if (!IsRunning)
				return;

			_timeElapsed += deltaTime;

			if (IsRunning)
			{
				onTick?.Invoke(NormalizedProgress);
			} else
			{
				Expired?.Invoke();
			}
		}

		public void Reset ()
		{
			_timeElapsed = 0;
		}
	}
}
