using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Closet_States
{
	public class ClosetWarningState : IState<Closet, EntranceTriggers>
	{
		private float _breachTimer;
		private float _pacifyTimer;

		public void OnEnter (IStateMachine<Closet, EntranceTriggers> machine)
		{
			_breachTimer = machine.Context.BreachTime;
			_pacifyTimer = machine.Context.PacifyTime;

			machine.Context.Player.EncounteredCloset = true;

			machine.Context.Door.localRotation = Quaternion.Euler(0, 20, 0);
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{
			machine.Context.Door.rotation = Quaternion.Euler(0, 0, 0);
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
				_pacifyTimer =  machine.Context.PacifyTime;
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
