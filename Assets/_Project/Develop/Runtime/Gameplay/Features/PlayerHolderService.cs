namespace Shed.Runtime.Gameplay.Features
{
	public class PlayerHolderService
	{
		private Player _player;

		public Player Player => _player;

		public void Initialize (Player player)
		{
			_player = player;
		}
	}
}
