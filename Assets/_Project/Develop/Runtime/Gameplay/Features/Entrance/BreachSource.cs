using System;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public abstract class BreachSource : MonoBehaviour, IBreachSource
	{
		public abstract event Action<IBreachSource> Breached;

		public abstract void Activate ();
	}

	public interface IBreachSource
	{
		event Action<IBreachSource> Breached;

		void Activate ();
	}
}
