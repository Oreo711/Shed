using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Door_States
{
	public class DoorBreachState : IState<Door, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Door, EntranceTriggers> machine)
		{
			Door door = machine.Context;
			door.Breach();
			door.transform.rotation = Quaternion.Euler(0, 80, 0);
		}

		public void OnExit (IStateMachine<Door, EntranceTriggers> machine)
		{
			Door door = machine.Context;
			door.transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		public void Tick (IStateMachine<Door, EntranceTriggers> machine, float deltaTime) {}
	}
}
