using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Window_States
{
	public class WindowBreachState : IState<Window, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Window, EntranceTriggers> machine)
		{
			Window window = machine.Context;
			window.Breach();
		}

		public void OnExit (IStateMachine<Window, EntranceTriggers> machine)
		{
			Window window = machine.Context;
		}

		public void Tick (IStateMachine<Window, EntranceTriggers> machine, float deltaTime) {}
	}
}
