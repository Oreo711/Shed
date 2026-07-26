using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.UI.Gameplay;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.SceneManagment;


namespace _Project.Develop.Runtime.UI.MainMenu
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
