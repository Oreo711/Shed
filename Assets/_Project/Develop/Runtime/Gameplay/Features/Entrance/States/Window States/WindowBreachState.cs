using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Window_States
{
	public class WindowBreachState : IState<Window, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Window, EntranceTriggers> machine)
		{
			Window window = machine.Context;
			window.Breach();
			window.transform.rotation = Quaternion.Euler(0, -80, 0);
		}

		public void OnExit (IStateMachine<Window, EntranceTriggers> machine)
		{
			Window window = machine.Context;
			window.transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		public void Tick (IStateMachine<Window, EntranceTriggers> machine, float deltaTime) {}
	}
}
