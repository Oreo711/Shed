using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Door_States
{
	public class DoorIdleState : IState<Door, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Door, EntranceTriggers> machine)
		{
			machine.Context.transform.rotation = Quaternion.Euler(0, 90, 90);
		}

		public void OnExit (IStateMachine<Door, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Door, EntranceTriggers> machine, float deltaTime)
		{

		}
	}
}
