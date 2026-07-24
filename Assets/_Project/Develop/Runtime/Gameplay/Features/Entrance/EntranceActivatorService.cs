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
		public event Action<Entrances> EntranceActivated;

		private readonly Timer               _timer;
		private readonly ICoroutinePerformer _coroutinePerformer;

		private readonly Dictionary<Entrances, int> _aggressionValues = new()
		{
			{ Entrances.door, 0 },
			{ Entrances.window, 0 },
			{ Entrances.otherWindow, 0 },
			{ Entrances.closet, 0 },
		};

		private          float _minInterval   = 8;
		private          float _maxInterval   = 12;
		private readonly float _intervalScale = 0.8f;

		private Coroutine _aggressionProcess;
		private Coroutine _activationProcess;

		public EntranceActivatorService (Timer timer, ICoroutinePerformer coroutinePerformer)
		{
			_timer              = timer;
			_coroutinePerformer = coroutinePerformer;
		}

		public void Run ()
		{
			_aggressionProcess = _coroutinePerformer.StartCoroutine(GiveAggression());
			_activationProcess = _coroutinePerformer.StartCoroutine(ActivateEntrances());

			_timer.HourPassed += ScaleInterval;
		}

		private void ScaleInterval ()
		{
			_minInterval *= _intervalScale;
			_maxInterval *= _intervalScale;
		}

		private IEnumerator ActivateEntrances ()
		{
			yield return new WaitForSeconds(_maxInterval);

			while (true)
			{
				Entrances entrance = CalculateHighestAggression();

				switch (entrance)
				{
					case Entrances.door:
						EntranceActivated?.Invoke(Entrances.door);
						break;
					case Entrances.window:
						EntranceActivated?.Invoke(Entrances.window);
						break;
					case Entrances.otherWindow:
						EntranceActivated?.Invoke(Entrances.otherWindow);
						break;
					case Entrances.closet:
						EntranceActivated?.Invoke(Entrances.closet);
						break;
				}

				_aggressionValues[entrance] = 0;
				float interval = Random.Range(_minInterval, _maxInterval);
				yield return new WaitForSeconds(interval);
			}
		}

		private IEnumerator GiveAggression ()
		{
			while (true)
			{
				yield return new WaitForSeconds(1);

				int             aggression = Random.Range(1, 101);
				int             index      = Random.Range(0, 4);
				List<Entrances> entrances  = Enum.GetValues(typeof(Entrances)).Cast<Entrances>().ToList();
				Entrances       key        = entrances[index];

				_aggressionValues[key] += aggression;
			}
		}

		public void Stop ()
		{
			_coroutinePerformer.StopCoroutine(_aggressionProcess);
			_coroutinePerformer.StopCoroutine(_activationProcess);
			_timer.HourPassed -= ScaleInterval;
		}

		private Entrances CalculateHighestAggression ()
		{
			int       highestAggression = _aggressionValues[Entrances.door];
			Entrances entrance          = Entrances.door;

			if (highestAggression < _aggressionValues[Entrances.window])
			{
				highestAggression = _aggressionValues[Entrances.window];
				entrance          = Entrances.window;
			}

			if (highestAggression < _aggressionValues[Entrances.otherWindow])
			{
				highestAggression = _aggressionValues[Entrances.otherWindow];
				entrance          = Entrances.otherWindow;
			}

			if (highestAggression < _aggressionValues[Entrances.closet])
			{
				entrance = Entrances.closet;
			}

			return entrance;
		}
	}
}
