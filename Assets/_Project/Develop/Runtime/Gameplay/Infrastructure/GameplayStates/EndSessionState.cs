using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure.GameplayStates
{
	public class EndSessionState : IState<GameplayContext, GameplayTriggers>
	{
		public void OnEnter (IStateMachine<GameplayContext, GameplayTriggers> machine) {}

		public void OnExit (IStateMachine<GameplayContext, GameplayTriggers> machine) {}

		public void Tick (IStateMachine<GameplayContext, GameplayTriggers> machine, float deltaTime) {}
	}
}
