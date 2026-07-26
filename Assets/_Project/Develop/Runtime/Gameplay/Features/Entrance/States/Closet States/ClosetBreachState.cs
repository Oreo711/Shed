using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Closet_States
{
	public class ClosetBreachState : IState<Closet, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Closet, EntranceTriggers> machine)
		{
			Closet closet = machine.Context;

			closet.Breach();
			closet.Door.localRotation = Quaternion.Euler(0, 120, 0);
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{
			Closet closet = machine.Context;
			closet.transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		public void Tick (IStateMachine<Closet, EntranceTriggers> machine, float deltaTime) {}
	}
}
