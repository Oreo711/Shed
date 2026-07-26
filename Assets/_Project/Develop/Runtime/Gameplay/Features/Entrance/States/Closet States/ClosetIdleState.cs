using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Closet_States
{
	public class ClosetIdleState : IState<Closet, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Closet, EntranceTriggers> machine)
		{
			machine.Context.Door.transform.localRotation = Quaternion.Euler(0, 0, 0);
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Closet, EntranceTriggers> machine, float deltaTime)
		{
			if (Input.GetKeyDown(KeyCode.C))
			{
				machine.Fire(EntranceTriggers.Warning);
			}
		}
	}
}
