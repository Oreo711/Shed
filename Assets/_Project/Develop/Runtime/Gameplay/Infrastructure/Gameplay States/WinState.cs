using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure.Gameplay_States
{
	public class WinState : IState<GameplayBootstrap, GameplayTriggers>
	{
		public void OnEnter (IStateMachine<GameplayBootstrap, GameplayTriggers> machine)
		{
			machine.Context.Player.SetActive(false);
			Debug.Log("You Win!");
		}

		public void OnExit (IStateMachine<GameplayBootstrap, GameplayTriggers> machine)
		{

		}

		public void Tick (IStateMachine<GameplayBootstrap, GameplayTriggers> machine, float deltaTime)
		{

		}
	}
}
