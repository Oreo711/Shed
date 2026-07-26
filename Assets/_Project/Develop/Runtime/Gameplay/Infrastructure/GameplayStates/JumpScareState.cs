using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Infrastructure.GameplayStates
{
	public class JumpScareState : IState<GameplayContext, GameplayTriggers>
	{
		private const float TimeToShoot = 1f;

		private float _timeout;

		public void OnEnter (IStateMachine<GameplayContext, GameplayTriggers> machine)
		{
			GameplayContext context = machine.Context;
			context.AnimateJumpScare();

			_timeout = TimeToShoot;
			Debug.Log("PRESS X TO SHOOT!");
		}

		public void OnExit (IStateMachine<GameplayContext, GameplayTriggers> machine) {}

		public void Tick (IStateMachine<GameplayContext, GameplayTriggers> machine, float deltaTime)
		{
			if (Input.GetKeyDown(KeyCode.X) && machine.Context.TryShoot())
			{
				machine.Fire(GameplayTriggers.Gameplay);
				machine.Context.Player.Shot = true;
				return;
			}

			_timeout -= deltaTime;

			if (_timeout <= 0)
			{
				machine.Context.Lose();
			}
		}
	}
}
