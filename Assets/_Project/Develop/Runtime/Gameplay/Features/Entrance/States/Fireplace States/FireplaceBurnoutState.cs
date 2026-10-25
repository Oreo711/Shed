using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Fireplace_States
{
	public class FireplaceBurnoutState : IState<Fireplace, EntranceTriggers>
	{
		private Ticker _ticker;

		public void OnEnter (IStateMachine<Fireplace, EntranceTriggers> machine)
		{
			_ticker = new Ticker(4f);
			machine.Context.ShowHint();
		}
		public void OnExit (IStateMachine<Fireplace, EntranceTriggers> machine)
		{

		}
		public void Tick (IStateMachine<Fireplace, EntranceTriggers> machine, float deltaTime)
		{
			if (_ticker.Tick(deltaTime) > 0)
			{
				machine.Fire(EntranceTriggers.Breach);
			}
		}
	}
}
