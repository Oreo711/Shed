using System;
using System.Collections;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features
{
	public class Timer
	{
		public event Action HourPassed;

		public int Minutes {get; private set;}
		public int Hours   {get; private set;}

		private readonly ICoroutinePerformer _coroutinePerformer;

		private bool _isRunning;

		public Timer (ICoroutinePerformer coroutinePerformer)
		{
			_coroutinePerformer = coroutinePerformer;
		}

		public void Start ()
		{
			_isRunning = true;
			_coroutinePerformer.StartCoroutine(Count());
		}

		private IEnumerator Count ()
		{
			while (_isRunning)
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
		}

		public void Stop ()
		{
			_isRunning = false;
		}
	}
}
