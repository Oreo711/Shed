using Modules.StateMachineLite;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Door_States
{
	public class DoorBreachState : IState<Door, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Door, EntranceTriggers> machine)
		{
			machine.Context.Breach();
			Debug.Log("Breached");
		}

		public void OnExit (IStateMachine<Door, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Door, EntranceTriggers> machine, float deltaTime)
		{

		}
	}
}
