using Modules.StateMachineLite;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Closet_States
{
	public class ClosetBreachState : IState<Closet, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Closet, EntranceTriggers> machine)
		{
			machine.Context.Breach();

			machine.Context.Door.rotation = Quaternion.Euler(0, 120, 0);

			Debug.Log("Died to closet");
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Closet, EntranceTriggers> machine, float deltaTime)
		{

		}
	}
}
