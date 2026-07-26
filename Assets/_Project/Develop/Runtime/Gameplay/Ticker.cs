using System;
using JetBrains.Annotations;


namespace Shed.Runtime.Gameplay
{
	[PublicAPI]
	public struct Ticker
	{
		private readonly float _interval;

		private float _timeLeft;

		public Ticker (float seconds)
		{
			if (seconds <= 0f)
				throw new ArgumentOutOfRangeException(nameof(seconds), "Ticker interval must be greater than zero.");

			_interval = seconds;
			_timeLeft = _interval;
		}

		public Ticker (TimeSpan interval)
		{
			_interval = (float)interval.TotalSeconds;

			if (_interval <= 0f)
				throw new ArgumentOutOfRangeException(nameof(interval), "Ticker interval must be greater than zero.");

			_timeLeft = _interval;
		}

		public int Tick (float deltaTime)
		{
			_timeLeft -= deltaTime;

			if (_timeLeft > 0f)
				return 0;

			int ticks = (int)(-_timeLeft / _interval) + 1;
			_timeLeft += _interval * ticks;

			return ticks;
		}

		public void Reset ()
		{
			_timeLeft = _interval;
		}
	}
}
