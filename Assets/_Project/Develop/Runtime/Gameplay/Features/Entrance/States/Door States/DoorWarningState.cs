using Modules.StateMachineLite;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Door_States
{
	public class DoorWarningState : IState<Door, EntranceTriggers>
	{
		private float _preparationTimer;
		private float _pacifyTimer;
		private bool  _isPrepared;

		public void OnEnter (IStateMachine<Door, EntranceTriggers> machine)
		{
			_preparationTimer = machine.Context.PreparationTime;
			_pacifyTimer      = machine.Context.PacifyTime;

			Debug.Log("Something is at the door.");

			machine.Context.transform.rotation = Quaternion.Euler(0, 25, 0);
		}

		public void OnExit (IStateMachine<Door, EntranceTriggers> machine)
		{
			_isPrepared = false;

			machine.Context.transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		public void Tick (IStateMachine<Door, EntranceTriggers> machine, float deltaTime)
		{
			if (!_isPrepared)
			{
				_preparationTimer -= deltaTime;

				if (_preparationTimer <= 0)
				{
					_isPrepared = true;
				}

				return;
			}

			_pacifyTimer -= deltaTime;

			if (_pacifyTimer <= 0)
			{
				machine.Fire(EntranceTriggers.idle);
			}

			Player player = machine.Context.Player;

			if (player.IsWalking || player.IsUsingFlashlight)
			{
				machine.Fire(EntranceTriggers.breach);
			}
		}
	}
}
