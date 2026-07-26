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
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{
			Closet closet = machine.Context;
		}

		public void Tick (IStateMachine<Closet, EntranceTriggers> machine, float deltaTime) {}
	}
}
