using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Door_States
{
	public class DoorBreachState : IState<Door, EntranceTriggers>
	{
		private IStateMachine<Door, EntranceTriggers> _machine;

		public void OnEnter (IStateMachine<Door, EntranceTriggers> machine)
		{
			machine.Context.Breach();

			machine.Context.transform.rotation = Quaternion.Euler(0, 80, 0);

			_machine = machine;

			machine.Context.Player.HitGunshot += OnHitGunshot;
		}

		private void OnHitGunshot ()
		{
			_machine.Fire(EntranceTriggers.Idle);
		}

		public void OnExit (IStateMachine<Door, EntranceTriggers> machine)
		{
			machine.Context.transform.rotation = Quaternion.Euler(0, 0, 0);

			machine.Context.Player.HitGunshot -= OnHitGunshot;
		}

		public void Tick (IStateMachine<Door, EntranceTriggers> machine, float deltaTime)
		{

		}
	}
}
