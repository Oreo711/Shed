using Shed.Runtime.UI.Core;
using UnityEngine;


namespace Shed.Runtime.UI.Gameplay
{
	public class GameplayPopupService : PopupService
	{
		private readonly GameplayUIRoot            _uiRoot;
		private readonly GameplayPresentersFactory _gameplayPresentersFactory;

		public GameplayPopupService(
			ViewFactory viewsFactory,
			ProjectPresentersFactory presentersFactory,
			GameplayUIRoot uiRoot,
			GameplayPresentersFactory gameplayPresentersFactory)
			: base(viewsFactory, presentersFactory)
		{
			_uiRoot                    = uiRoot;
			_gameplayPresentersFactory = gameplayPresentersFactory;
		}

		protected override Transform PopupLayer => _uiRoot.PopupsLayer;
	}
}
