using Modules.StateMachineLite;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Window_States
{
	public class WindowIdleState : IState<Window, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Window, EntranceTriggers> machine)
		{
			Debug.Log("The figure in the window disappears.");
		}

		public void OnExit (IStateMachine<Window, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Window, EntranceTriggers> machine, float deltaTime)
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				machine.Fire(EntranceTriggers.warning);
			}
		}
	}
}
