using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.UI.Core;
using UnityEngine;


namespace _Project.Develop.Runtime.UI.Gameplay
{
	public class GameplayScreenPresenter : IPresenter
	{
		private readonly GameplayScreenView  _screen;
		private readonly PlayerHolderService _playerHolderService;
		private readonly GameTimer           _gameTimer;

		public GameplayScreenPresenter (
			GameplayScreenView screen,
			PlayerHolderService playerHolderService,
			GameTimer gameTimer
		)
		{
			_screen              = screen;
			_playerHolderService = playerHolderService;
			_gameTimer           = gameTimer;
		}

		public void Initialize ()
		{
			_screen.DisableRechargeBar();
			_screen.DisableClock();
			_screen.DisableHoldInteractGuide();
			// _screen.DisablePressInteractGuide();

			_screen.QueueMonologueText("Phew.. this shed will protect me from them. I have to make sure they don't get in.");
		}

		public void Tick ()
		{
			Player player = _playerHolderService.Player;

			ProcessMonologue();
			_screen.SetClock(_gameTimer.Hours, _gameTimer.Minutes);
			_screen.SetMonologueText();

			if (player)
			{
				_screen.SetBatteryPercentage(_playerHolderService.Player.Flashlight.BatteryPercentage);
			}

			if (player.IsFacingClock)
			{
				_screen.EnableClock();
			} else
			{
				_screen.DisableClock();
			}

			if (player.Flashlight.IsOn)
			{
				_screen.DisableFlashlightGuide();
			}

			if (player.IsRecharging)
			{
				_screen.EnableRechargeBar();

				_screen.SetRechargeBarPercentage((player.Flashlight.RechargeTime - player.RemainingRechargeTime) / player.Flashlight.RechargeTime);
			} else
			{
				_screen.DisableRechargeBar();
			}

			if (player.CanUseBatteryBox || player.CanUseCloset)
			{
				_screen.EnableHoldInteractGuide();
			}
			else
			{
				_screen.DisableHoldInteractGuide();
			}
		}

		private void ProcessMonologue ()
		{
			Player player = _playerHolderService.Player;

			if (player.HeardKnocking && !player.MonologueMilestones.HasHeardKnocking)
			{
				_screen.QueueMonologueText("One of them is knocking on the door.. If it opens it, I'll have to turn off the flashlight and stay quiet, it might leave if I do.");
				player.MonologueMilestones.HasHeardKnocking = true;
			}

			if (player.EncounteredDoor && !player.MonologueMilestones.HasEncounteredDoor)
			{
				_screen.QueueMonologueText("...");
				player.MonologueMilestones.HasEncounteredDoor = true;
			}

			if (player.EncounteredWindow && !player.MonologueMilestones.HasEncounteredWindow)
			{
				_screen.QueueMonologueText("Something is at the window. Maybe the flashlight could scare it away?");
				player.MonologueMilestones.HasEncounteredWindow = true;
			}

			if (player.EncounteredCloset && !player.MonologueMilestones.HasEncounteredCloset)
			{
				_screen.QueueMonologueText("Something is rummaging in the closet, I can't let it get out.");
				player.MonologueMilestones.HasEncounteredCloset = true;
			}

			if (player.IsLowOnBattery && !player.MonologueMilestones.HasBeenLowOnBattery)
			{
				_screen.QueueMonologueText("The batteries are dying... Good thing I have a box of them. I think i put it on the table.");
				player.MonologueMilestones.HasBeenLowOnBattery = true;
			}

			if (player.Shot && !player.MonologueMilestones.HasShot)
			{
				_screen.QueueMonologueText("I'm out of bullets.. if I let one of them in again I'll die for sure.");
				player.MonologueMilestones.HasShot = true;
			}
		}

		public void Dispose () {}
	}
}
