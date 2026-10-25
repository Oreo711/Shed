using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Door_States
{
	public class DoorBreachState : IState<Door, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Door, EntranceTriggers> machine)
		{
			Door door = machine.Context;
			door.Breach();
			// door.transform.rotation = Quaternion.Euler(0, 170, 90);
		}

		public void OnExit (IStateMachine<Door, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Door, EntranceTriggers> machine, float deltaTime)
		{

		}
	}
}
