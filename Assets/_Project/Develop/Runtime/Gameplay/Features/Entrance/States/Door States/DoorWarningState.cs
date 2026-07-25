using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Door_States
{
	public class DoorWarningState : IState<Door, EntranceTriggers>
	{
		private float _preparationTimer;
		private float _pacifyTimer;
		private bool  _isPrepared;
		private bool  _playedCloseSound;

		public void OnEnter (IStateMachine<Door, EntranceTriggers> machine)
		{
			_preparationTimer = machine.Context.PreparationTime;
			_pacifyTimer      = machine.Context.PacifyTime;
			machine.Context.PlayOpenSound();

			machine.Context.Player.CanNotAct = true;

			machine.Context.transform.rotation = Quaternion.Euler(0, 25, 0);
		}

		public void OnExit (IStateMachine<Door, EntranceTriggers> machine)
		{
			_isPrepared       = false;
			_playedCloseSound = false;

			machine.Context.Player.CanNotAct = false;

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

			if (_pacifyTimer <= 1 && !_playedCloseSound)
			{
				machine.Context.PlayCloseSound();
				_playedCloseSound = true;
			}

			if (_pacifyTimer <= 0)
			{
				machine.Fire(EntranceTriggers.Idle);
			}

			Player player = machine.Context.Player;

			if (player.IsWalking || player.IsUsingFlashlight || player.IsRecharging)
			{
				machine.Fire(EntranceTriggers.Breach);
			}
		}
	}
}
