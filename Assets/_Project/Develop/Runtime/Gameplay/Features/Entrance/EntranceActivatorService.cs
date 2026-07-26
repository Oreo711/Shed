using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;


namespace Shed.Runtime.Gameplay.Features.Entrance
{
	public class EntranceActivatorService
	{
		private readonly GameTimer                         _gameTimer;
		private readonly Dictionary<BreachSourceBase, int> _aggressionValues = new();

		private List<BreachSourceBase> _breachSources;

		private const float IntervalScale = 0.9f;

		private bool  _isRunning;
		private float _minInterval = 10;
		private float _maxInterval = 16;

		public event Action<BreachSourceBase> EntranceBreached;

		private Ticker _aggressionTicker = new Ticker(1.0f);
		private Ticker _breachActivationTicker;

		public EntranceActivatorService (GameTimer gameTimer)
		{
			_gameTimer              = gameTimer;
			_breachActivationTicker = new Ticker(20f);
		}

		public void Initialize (IReadOnlyList<BreachSourceBase> breachSources)
		{
			foreach (BreachSourceBase breachSource in breachSources)
			{
				_aggressionValues.Add(breachSource, 0);
				breachSource.Breached += HandleBreach;
			}

			_breachSources = _aggressionValues.Keys.ToList();
		}

		public void Run ()
		{
			_isRunning = true;

			_gameTimer.HourPassed += ScaleInterval;
		}

		public void Tick (float deltaTime)
		{
			if (!_isRunning)
				return;

			_aggressionTicker.DoNonAlloc(
				this,
				static service => service.AdvanceAggression(),
				deltaTime
			);

			if (_breachActivationTicker.Tick(deltaTime) > 0)
			{
				ActivateBreach();
				float interval = Random.Range(_minInterval, _maxInterval);
				_breachActivationTicker = new Ticker(interval);
			}
		}

		public void Pause ()
		{
			_isRunning = false;
		}

		public void Resume ()
		{
			_isRunning = true;
		}

		private void ScaleInterval ()
		{
			_minInterval *= IntervalScale;
			_maxInterval *= IntervalScale;
		}

		private void ActivateBreachSource (BreachSourceBase breachSource)
		{
			_aggressionValues[breachSource] = 0;
			breachSource.Activate();
		}

		private void AdvanceAggression ()
		{
			int aggression = Random.Range(1, 101);
			int index      = Random.Range(0, _breachSources.Count);

			BreachSourceBase breachSource = _breachSources[index];

			_aggressionValues[breachSource] += aggression;
		}

		private void ActivateBreach ()
		{
			BreachSourceBase breachSource = GetSourceWithHighestAggression();
			ActivateBreachSource(breachSource);
		}

		public void Stop ()
		{
			_isRunning = false;

			_gameTimer.HourPassed -= ScaleInterval;

			foreach (BreachSourceBase breachSource in _aggressionValues.Keys)
			{
				breachSource.Breached -= HandleBreach;
			}

			_aggressionValues.Clear();
		}

		private void HandleBreach (BreachSourceBase breachSource)
		{
			EntranceBreached?.Invoke(breachSource);
		}

		private BreachSourceBase GetSourceWithHighestAggression ()
		{
			BreachSourceBase breachSource = null; // not possible to get null if aggression meters are initialized correctly

			int aggression = -1;

			foreach (KeyValuePair<BreachSourceBase, int> aggressionMeter in _aggressionValues)
			{
				if (aggressionMeter.Value > aggression)
				{
					aggression   = aggressionMeter.Value;
					breachSource = aggressionMeter.Key;
				}
			}

			return breachSource;
		}
	}
}
