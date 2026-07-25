using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.UI.Core;


namespace _Project.Develop.Runtime.UI.Gameplay
{
	public class GameplayScreenPresenter : IPresenter
	{
		private readonly GameplayScreenView  _screen;
		private readonly PlayerHolderService _playerHolderService;
		private readonly Timer               _timer;

		public GameplayScreenPresenter(
			GameplayScreenView screen,
			PlayerHolderService playerHolderService,
			Timer timer
		)
		{
			_screen              = screen;
			_playerHolderService = playerHolderService;
			_timer               = timer;
		}

		public void Initialize()
		{
			_screen.DisableRechargeBar();
			_screen.DisableClock();
		}

		public void Update (float deltaTime)
		{
			Player player = _playerHolderService.Player;

			_screen.SetClock(_timer.Hours, _timer.Minutes);

			if (player)
			{
				_screen.SetBatteryPercentage(_playerHolderService.Player.Flashlight.BatteryPercentage);
			}

			if (player.IsFacingClock)
			{
				_screen.EnableClock();
			}
			else
			{
				_screen.DisableClock();
			}

			if (player.IsRecharging)
			{
				_screen.EnableRechargeBar();

				_screen.SetRechargeBarPercentage((player.Flashlight.RechargeTime - player.RemainingRechargeTime) / player.Flashlight.RechargeTime);
			}
			else
			{
				_screen.DisableRechargeBar();
			}


		}

		public void Dispose()
		{

		}
	}
}
