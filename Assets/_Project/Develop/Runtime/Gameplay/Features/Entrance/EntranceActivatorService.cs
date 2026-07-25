using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public class EntranceActivatorService
	{
		private readonly Timer                          _timer;
		private readonly ICoroutinePerformer            _coroutinePerformer;
		private readonly Dictionary<IBreachSource, int> _aggressionValues = new();

		private List<IBreachSource> _breachSources;

		private const float IntervalScale = 0.85f;

		private bool  _isRunning;
		private float _minInterval = 10;
		private float _maxInterval = 15;

		private Coroutine _aggressionLoop;
		private Coroutine _activationLoop;

		public event Action<IBreachSource> EntranceBreached;

		public EntranceActivatorService (Timer timer, ICoroutinePerformer coroutinePerformer)
		{
			_timer              = timer;
			_coroutinePerformer = coroutinePerformer;
		}

		public void Initialize (IReadOnlyList<IBreachSource> breachSources)
		{
			foreach (IBreachSource breachSource in breachSources)
			{
				_aggressionValues.Add(breachSource, 0);
				breachSource.Breached += HandleBreach;
			}

			_breachSources = _aggressionValues.Keys.ToList();
		}

		public void Run ()
		{
			_isRunning = true;

			_aggressionLoop = _coroutinePerformer.StartCoroutine(IncreaseAggressionLoop());
			_activationLoop = _coroutinePerformer.StartCoroutine(BreachActivationLoop());

			_timer.HourPassed += ScaleInterval;
		}

		private void ScaleInterval ()
		{
			_minInterval *= IntervalScale;
			_maxInterval *= IntervalScale;
		}

		private IEnumerator BreachActivationLoop ()
		{
			yield return new WaitForSeconds(_maxInterval);

			while (_isRunning)
			{
				IBreachSource breachSource = GetSourceWithHighestAggression();
				ActivateBreachSource(breachSource);

				float interval = Random.Range(_minInterval, _maxInterval);
				yield return new WaitForSeconds(interval);
			}
		}

		private void ActivateBreachSource (IBreachSource breachSource)
		{
			_aggressionValues[breachSource] = 0;
			breachSource.Activate();
		}

		private IEnumerator IncreaseAggressionLoop ()
		{
			while (_isRunning)
			{
				yield return new WaitForSeconds(1);

				int aggression = Random.Range(1, 101);

				int           index        = Random.Range(0, _breachSources.Count);
				IBreachSource breachSource = _breachSources[index];

				_aggressionValues[breachSource] += aggression;
			}
		}

		public void Stop ()
		{
			if (_aggressionLoop != null)
			{
				_coroutinePerformer.StopCoroutine(_aggressionLoop);
			}

			if (_activationLoop != null)
			{
				_coroutinePerformer.StopCoroutine(_activationLoop);
			}

			_isRunning = false;

			Teardown();
		}

		private void Teardown ()
		{
			_timer.HourPassed -= ScaleInterval;

			foreach (IBreachSource breachSource in _aggressionValues.Keys)
			{
				breachSource.Breached -= HandleBreach;
			}

			_aggressionValues.Clear();
		}

		private void HandleBreach (IBreachSource breachSource)
		{
			EntranceBreached?.Invoke(breachSource);
		}

		private IBreachSource GetSourceWithHighestAggression ()
		{
			IBreachSource breachSource = null; // not possible to get null if aggression meters are initialized correctly

			int aggression = -1;

			foreach (KeyValuePair<IBreachSource, int> aggressionMeter in _aggressionValues)
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
