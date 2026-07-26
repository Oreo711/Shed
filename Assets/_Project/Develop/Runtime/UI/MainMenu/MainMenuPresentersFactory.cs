using Shed.Runtime.Infrastructure.DI;
using Shed.Runtime.Utilities.CoroutineManagement;
using Shed.Runtime.Utilities.SceneManagment;


namespace Shed.Runtime.UI.MainMenu
{
	public class MainMenuPresentersFactory
	{
		private readonly DIContainer _container;

		public MainMenuPresentersFactory (DIContainer container)
		{
			_container = container;
		}

		public MainMenuScreenPresenter CreateMainMenuScreenPresenter (MainMenuScreenView view)
		{
			return new MainMenuScreenPresenter(
				view,
					_container.Resolve<ICoroutinePerformer>(),
				_container.Resolve<SceneSwitcherService>());
		}
	}
}
