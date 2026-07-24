using Modules.StateMachineLite;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Window_States
{
	public class WindowBreachState : IState<Window, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Window, EntranceTriggers> machine)
		{
			machine.Context.Breach();
			machine.Context.transform.rotation = Quaternion.Euler(0, -80, 0);

			Debug.Log("Died to window");
		}

		public void OnExit (IStateMachine<Window, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Window, EntranceTriggers> machine, float deltaTime)
		{

		}
	}
}
