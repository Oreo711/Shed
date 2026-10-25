using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Door_States
{
	public class DoorWarningState : IState<Door, EntranceTriggers>
	{
		private float _preparationTimer;
		private float _pacifyTimer;
		private bool  _isPrepared;
		private bool  _playedCloseSound;

		public void OnEnter (IStateMachine<Door, EntranceTriggers> machine)
		{
			Door door = machine.Context;
			_preparationTimer = door.PreparationTime;
			_pacifyTimer      = door.PacifyTime;
			door.PlayOpenSound();
			door.Player.MonologueService.ShowDoorEncounterMonologue();

			door.Player.CanNotAct = true;
			door.ShowHint();
		}

		public void OnExit (IStateMachine<Door, EntranceTriggers> machine)
		{
			_isPrepared       = false;
			_playedCloseSound = false;

			Door door = machine.Context;
			door.Player.CanNotAct   = false;
			door.HideHint();
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

			if (_pacifyTimer <= 0.75 && !_playedCloseSound)
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
