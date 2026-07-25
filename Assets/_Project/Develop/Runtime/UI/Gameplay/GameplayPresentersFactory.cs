using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.Infrastructure.DI;


namespace _Project.Develop.Runtime.UI.Gameplay
{
	public class GameplayPresentersFactory
	{
		private readonly DIContainer _container;

		public GameplayPresentersFactory (DIContainer container)
		{
			_container = container;
		}

		public GameplayScreenPresenter CreateGameplayScreenPresenter (GameplayScreenView view)
		{
			return new GameplayScreenPresenter(
				view,
				_container.Resolve<PlayerHolderService>(),
				_container.Resolve<GameTimer>());
		}
	}
}
