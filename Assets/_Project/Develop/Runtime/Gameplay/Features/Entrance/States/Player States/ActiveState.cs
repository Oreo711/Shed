using DG.Tweening;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Player_States
{
	public class ActiveState : IState<Player, PlayerStateTriggers>
	{
		public void OnEnter (IStateMachine<Player, PlayerStateTriggers> machine)
		{
			Player player = machine.Context;

			player.Rigidbody.isKinematic = false;

			machine.Context.ActivateControls();
		}

		public void OnExit (IStateMachine<Player, PlayerStateTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Player, PlayerStateTriggers> machine, float deltaTime)
		{
			if (machine.Context.Energy >= 0)
			{
				machine.Context.SpendEnergy(deltaTime);
			}
		}
	}
}
