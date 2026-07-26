using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Closet_States
{
	public class ClosetWarningState : IState<Closet, EntranceTriggers>
	{
		private float _breachTimer;
		private float _pacifyTimer;

		public void OnEnter (IStateMachine<Closet, EntranceTriggers> machine)
		{
			Closet closet = machine.Context;
			_breachTimer = closet.BreachTime;
			_pacifyTimer = closet.PacifyTime;

			closet.Player.EncounteredCloset = true;
			closet.Door.localRotation       = Quaternion.Euler(0, 20, 0);
			closet.ShowHint();
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{
			Closet closet = machine.Context;
			closet.Door.rotation = Quaternion.Euler(0, 0, 0);
			closet.HideHint();
		}

		public void Tick (IStateMachine<Closet, EntranceTriggers> machine, float deltaTime)
		{
			if (machine.Context.IsBeingPacified)
			{
				_pacifyTimer -= deltaTime;
			} else
			{
				if (!machine.Context.Player.CanNotAct)
				{
					_breachTimer -= deltaTime;
				}

				_pacifyTimer = machine.Context.PacifyTime;
			}

			if (_pacifyTimer <= 0)
			{
				machine.Fire(EntranceTriggers.Idle);
			}

			if (_breachTimer <= 0)
			{
				machine.Fire(EntranceTriggers.Breach);
			}
		}
	}
}
