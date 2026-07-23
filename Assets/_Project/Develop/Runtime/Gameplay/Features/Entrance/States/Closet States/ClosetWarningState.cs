using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
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

			Debug.Log("Something is in the closet.");
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Closet, EntranceTriggers> machine, float deltaTime)
		{
			if (machine.Context.IsBeingPacified)
			{
				_pacifyTimer -= deltaTime;
			} else
			{
				_breachTimer -= deltaTime;
				_pacifyTimer = machine.Context.PacifyTime;
			}

			if (_pacifyTimer <= 0)
			{
				machine.Fire(EntranceTriggers.idle);
			}

			if (_breachTimer <= 0)
			{
				machine.Fire(EntranceTriggers.breach);
			}
		}
	}
}
