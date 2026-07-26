using System.Collections;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Infrastructure.GameplayStates
{
	public class EndSessionState : IState<GameplayContext, GameplayTriggers>
	{
		private IStateMachine<GameplayContext, GameplayTriggers> _machine;

		public void OnEnter (IStateMachine<GameplayContext, GameplayTriggers> machine)
		{
			_machine = machine;

			machine.Context.StartCoroutine(WaitBeforeSwitch());
			machine.Context.StopClockSound();
		}

		public void OnExit (IStateMachine<GameplayContext, GameplayTriggers> machine) {}

		public void Tick (IStateMachine<GameplayContext, GameplayTriggers> machine, float deltaTime)
		{

		}

		private IEnumerator WaitBeforeSwitch ()
		{
			yield return new WaitForSeconds(6f);

			_machine.Context.SwitchToMenu();
		}
	}
}
