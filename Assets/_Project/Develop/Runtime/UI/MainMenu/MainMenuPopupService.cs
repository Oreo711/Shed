using Shed.Runtime.UI.Core;
using UnityEngine;


namespace Shed.Runtime.UI.MainMenu
{
	public class MainMenuPopupService : PopupService
	{
		private readonly MainMenuUIRoot            _uiRoot;
		private readonly MainMenuPresentersFactory _gameplayPresentersFactory;

		public MainMenuPopupService(
			ViewFactory viewsFactory,
			ProjectPresentersFactory presentersFactory,
			MainMenuUIRoot uiRoot,
			MainMenuPresentersFactory gameplayPresentersFactory)
			: base(viewsFactory, presentersFactory)
		{
			_uiRoot                    = uiRoot;
			_gameplayPresentersFactory = gameplayPresentersFactory;
		}

		protected override Transform PopupLayer => _uiRoot.PopupsLayer;
	}
}
