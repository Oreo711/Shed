using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Fireplace_States
{
	public class FireplaceBreachState : IState<Fireplace, EntranceTriggers>
	{
		public void OnEnter (IStateMachine<Fireplace, EntranceTriggers> machine)
		{
			Fireplace fireplace = machine.Context;
			fireplace.Breach();
		}
		public void OnExit (IStateMachine<Fireplace, EntranceTriggers> machine)
		{

		}
		public void Tick (IStateMachine<Fireplace, EntranceTriggers> machine, float deltaTime)
		{

		}
	}
}
