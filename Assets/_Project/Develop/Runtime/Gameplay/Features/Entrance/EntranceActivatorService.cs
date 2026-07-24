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
		private readonly Dictionary<IBreachSource, int> _aggressionMeters = new();

		private const float IntervalScale = 0.8f;

		private bool  _isRunning;
		private float _minInterval = 8;
		private float _maxInterval = 12;

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
				_aggressionMeters.Add(breachSource, 0);
				breachSource.Breached += HandleBreach;
			}
		}

		public void Run ()
		{
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
			_aggressionMeters[breachSource] = 0;
			breachSource.Activate();
		}

		private IEnumerator IncreaseAggressionLoop ()
		{
			while (_isRunning)
			{
				yield return new WaitForSeconds(1);

				int aggression = Random.Range(1, 101);

				List<IBreachSource> breachSources = _aggressionMeters.Keys.ToList();
				int                 index         = Random.Range(0, breachSources.Count);
				IBreachSource       breachSource  = breachSources[index];

				_aggressionMeters[breachSource] += aggression;
			}
		}

		public void Stop ()
		{
			_coroutinePerformer.StopCoroutine(_aggressionLoop);
			_coroutinePerformer.StopCoroutine(_activationLoop);
			_isRunning = false;

			Teardown();
		}

		private void Teardown ()
		{
			_timer.HourPassed -= ScaleInterval;

			foreach (IBreachSource breachSource in _aggressionMeters.Keys)
			{
				breachSource.Breached -= HandleBreach;
			}

			_aggressionMeters.Clear();
		}

		private void HandleBreach (IBreachSource breachSource)
		{
			EntranceBreached?.Invoke(breachSource);
		}

		private IBreachSource GetSourceWithHighestAggression ()
		{
			IBreachSource breachSource = null; // not possible to get null if aggression meters are initialized correctly

			int aggression = -1;

			foreach (KeyValuePair<IBreachSource, int> aggressionMeter in _aggressionMeters)
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
