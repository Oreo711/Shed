using System;
using System.Collections;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features
{
	public class Timer
	{
		public event Action HourPassed;
		public event Action NightPassed;

		public int Minutes {get; private set;}
		public int Hours   {get; private set;}

		private readonly ICoroutinePerformer _coroutinePerformer;

		private Coroutine _countProcess;

		public Timer (ICoroutinePerformer coroutinePerformer)
		{
			_coroutinePerformer = coroutinePerformer;
		}

		public void Start ()
		{
			_countProcess = _coroutinePerformer.StartCoroutine(Count());
		}

		private IEnumerator Count ()
		{
			while (true)
			{
				yield return new WaitForSeconds(1);

				Minutes++;

				if (Minutes >= 60)
				{
					Minutes = 0;
					Hours++;
					HourPassed?.Invoke();

					if (Hours >= 7)
						break;
				}
			}

			NightPassed?.Invoke();
		}

		public void Stop ()
		{
			if (_countProcess != null)
				return;

			_coroutinePerformer.StopCoroutine(_countProcess);
			_countProcess = null;
		}
	}
}
