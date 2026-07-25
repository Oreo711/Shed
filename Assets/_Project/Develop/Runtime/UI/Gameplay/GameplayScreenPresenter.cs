using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.UI.Core;


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
			_screen.DisableInteractGuide();
		}

		public void Tick ()
		{
			Player player = _playerHolderService.Player;

			_screen.SetClock(_gameTimer.Hours, _gameTimer.Minutes);

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

			if (player.IsRecharging)
			{
				_screen.EnableRechargeBar();

				_screen.SetRechargeBarPercentage((player.Flashlight.RechargeTime - player.RemainingRechargeTime) / player.Flashlight.RechargeTime);
			} else
			{
				_screen.DisableRechargeBar();
			}

			if (player.IsFacingBatteryBox || player.IsFacingClock)
			{
				_screen.EnableInteractGuide();
			} else
			{
				_screen.DisableInteractGuide();
			}
		}

		public void Dispose () {}
	}
}
