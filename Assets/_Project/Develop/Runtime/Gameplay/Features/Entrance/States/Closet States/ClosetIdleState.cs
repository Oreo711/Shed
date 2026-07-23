using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Closet_States
{
	public class ClosetIdleState : IState<Closet, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Closet, EntranceTriggers> machine)
		{
			Debug.Log("The closet is empty.");
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Closet, EntranceTriggers> machine, float deltaTime)
		{
			if (Input.GetKeyDown(KeyCode.C))
			{
				machine.Fire(EntranceTriggers.warning);
			}
		}
	}
}
