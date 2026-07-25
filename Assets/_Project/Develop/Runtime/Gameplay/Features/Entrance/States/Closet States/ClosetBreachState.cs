using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Closet_States
{
	public class ClosetBreachState : IState<Closet, EntranceTriggers>
	{
		private IStateMachine<Closet, EntranceTriggers> _machine;

		public void OnEnter (IStateMachine<Closet, EntranceTriggers> machine)
		{
			Closet closet = machine.Context;

			closet.Breach();
			closet.Door.rotation = Quaternion.Euler(0, 120, 0);

			_machine = machine;

			machine.Context.Player.HitGunshot += OnHitGunshot;
		}

		private void OnHitGunshot ()
		{
			_machine.Fire(EntranceTriggers.Idle);
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{
			machine.Context.transform.rotation = Quaternion.Euler(0, 0, 0);

			machine.Context.Player.HitGunshot -= OnHitGunshot;
		}

		public void Tick (IStateMachine<Closet, EntranceTriggers> machine, float deltaTime) {}
	}
}
