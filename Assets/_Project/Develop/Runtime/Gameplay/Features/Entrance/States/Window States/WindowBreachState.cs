using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;

namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Window_States
{
	public class WindowBreachState : IState<Window, EntranceTriggers>
	{
		private IStateMachine<Window, EntranceTriggers> _machine;

		public void OnEnter (IStateMachine<Window, EntranceTriggers> machine)
		{
			machine.Context.Breach();
			machine.Context.transform.rotation = Quaternion.Euler(0, -80, 0);

			_machine = machine;

			machine.Context.Player.HitGunshot += OnHitGunshot;
		}

		private void OnHitGunshot ()
		{
			_machine.Fire(EntranceTriggers.Idle);
		}

		public void OnExit (IStateMachine<Window, EntranceTriggers> machine)
		{
			machine.Context.transform.rotation = Quaternion.Euler(0, 0, 0);

			machine.Context.Player.HitGunshot -= OnHitGunshot;
		}

		public void Tick (IStateMachine<Window, EntranceTriggers> machine, float deltaTime)
		{

		}
	}
}
