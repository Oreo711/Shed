using System;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using Shed.Runtime.Gameplay.Hints;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance
{
	public abstract class BreachSourceBase : MonoBehaviour
	{
		[SerializeField] private BreachSourceHint _breachSourceHint;
		[SerializeField] private Transform        _lootAtSpot;

		public Vector3 LootAtSpot => _lootAtSpot.position;

		public abstract event Action<BreachSourceBase> Breached;

		public abstract void Activate ();
		public abstract void HandleGunshot ();

		public void ShowHint () => _breachSourceHint.Show();
		public void HideHint () => _breachSourceHint.Hide();
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
