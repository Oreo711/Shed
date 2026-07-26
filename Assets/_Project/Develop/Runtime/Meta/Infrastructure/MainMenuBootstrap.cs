using System.Collections;
using Shed.Runtime.Infrastructure;
using Shed.Runtime.Infrastructure.DI;
using Shed.Runtime.UI.MainMenu;
using Shed.Runtime.Utilities.CoroutineManagement;
using Shed.Runtime.Utilities.DataManagment.DataProviders;
using Shed.Runtime.Utilities.SceneManagment;
using UnityEngine;


namespace Shed.Runtime.Meta.Infrastructure
{
	public class MainMenuBootstrap : SceneBootstrap
	{
		private DIContainer          _container;
		private PlayerDataProvider   _playerDataProvider;
		private SceneSwitcherService _sceneSwitcherService;
		private ICoroutinePerformer  _coroutinePerformer;

		private MainMenuScreenPresenter _screen;

		public override void ProcessRegistrations (DIContainer container)
		{
			_container = container;

			MainMenuContextRegistrations.Process(_container);
		}

		public override IEnumerator Initialize ()
		{
			_playerDataProvider   = _container.Resolve<PlayerDataProvider>();
			_coroutinePerformer   = _container.Resolve<ICoroutinePerformer>();
			_sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
			_screen               = _container.Resolve<MainMenuScreenPresenter>();

			Cursor.lockState = CursorLockMode.None;

			yield break;
		}

		public override void Run () {}
	}
}
