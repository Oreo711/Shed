using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;


namespace Shed.Runtime.Gameplay.Infrastructure.GameplayStates
{
	public class GameplayState : IState<GameplayContext, GameplayTriggers>
	{
		public void OnEnter (IStateMachine<GameplayContext, GameplayTriggers> machine)
		{
			machine.Context.ResumeEntranceActivator();
		}

		public void OnExit (IStateMachine<GameplayContext, GameplayTriggers> machine)
		{
			machine.Context.PauseEntranceActivator();
		}

		public void Tick (IStateMachine<GameplayContext, GameplayTriggers> machine, float deltaTime) {}
	}
}
