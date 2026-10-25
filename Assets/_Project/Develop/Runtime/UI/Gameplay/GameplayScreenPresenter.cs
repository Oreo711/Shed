using System;
using Shed.Runtime.Gameplay.Features;
using Shed.Runtime.UI.Core;


namespace Shed.Runtime.UI.Gameplay
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
			_screen.DisableClockWidget();
			_screen.DisableHoldInteractGuide();
			_screen.DisablePressInteractGuide();
			_screen.DisableBedGetupGuide();

			_playerHolderService.Player.Caught += ActivateJumpScare;
		}

		public void Tick ()
		{
			Player player = _playerHolderService.Player;

			_screen.SetClock(_gameTimer.Hours, _gameTimer.Minutes);
			_screen.SetMonologueText();

			if (player)
			{
				_screen.SetBatteryPercentage(_playerHolderService.Player.Flashlight.BatteryPercentage);
				_screen.SetEnergyPercentage(_playerHolderService.Player.Energy / _playerHolderService.Player.MaxEnergy);
			}

			ShowClock(player.IsFacingClock);

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
			} else
			{
				_screen.DisableHoldInteractGuide();
			}

			if (player.CanUseBed)
			{
				_screen.EnablePressInteractGuide();
			} else
			{
				_screen.DisablePressInteractGuide();
			}

			if (player.CanStopUsingBed)
			{
				_screen.EnableBedGetupGuide();
			} else
			{
				_screen.DisableBedGetupGuide();
			}
		}

		private void ActivateJumpScare ()
		{
				_screen.ShowJumpScare(_playerHolderService.Player.Gun.Bullets > 0);
		}

		public void ShowWinScreen ()
		{
			_screen.EnableWinScreen();
		}

		public void ShowLossScreen ()
		{
			_screen.EnableLossScreen();
		}

		private void ShowClock (bool isFacing)
		{
			if (isFacing)
			{
				_screen.EnableClockWidget();
			} else
			{
				_screen.DisableClockWidget();
			}
		}

		public void ProcessMonologue (string monologue)
		{
			_screen.QueueMonologueText(monologue);
		}

		public void CancelJumpScare ()
		{
			_screen.CancelJumpScare();
		}

		public void Dispose ()
		{
			_playerHolderService.Player.Caught -= ActivateJumpScare;
		}
	}
}
