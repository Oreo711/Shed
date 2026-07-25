using System;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public abstract class BreachSourceBase : MonoBehaviour
	{
		public abstract event Action<BreachSourceBase> Breached;

		public abstract void Activate ();
		public abstract void HandleGunshot ();
	}

	public abstract class BreachSource<TDerived> : BreachSourceBase
		where TDerived : BreachSource<TDerived>, IStateContext
	{
		protected StateMachine<TDerived, EntranceTriggers> StateMachine;

		public override void HandleGunshot ()
		{
			StateMachine.Fire(EntranceTriggers.Idle);
		}
	}
}
