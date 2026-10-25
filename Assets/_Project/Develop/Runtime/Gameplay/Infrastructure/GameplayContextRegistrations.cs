using Shed.Runtime.Gameplay.Features;
using Shed.Runtime.Gameplay.Features.Entrance;
using Shed.Runtime.Infrastructure.DI;
using Shed.Runtime.UI;
using Shed.Runtime.UI.Core;
using Shed.Runtime.UI.Gameplay;
using Shed.Runtime.Utilities.AssetManagment;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Infrastructure
{
	public static class GameplayContextRegistrations
	{
		public static void Process (DIContainer container)
		{
			container.Register(CreateTimer);

			container.Register(CreateEntranceActivatorService);

			container.Register(CreateGameplayUIRoot);

			container.Register(CreateGameplayScreenPresenter);

			container.Register(CreateGameplayPresentersFactory);

			container.Register(CreateGameplayPopupService);

			container.Register(CreatePlayerHolderService);

			container.Register(CreateMonologueService);
		}

		private static MonologueService CreateMonologueService (DIContainer c)
		{
			MonologueService monologueService = new MonologueService(c.Resolve<GameplayScreenPresenter>());
			monologueService.Initialize();

			return monologueService;
		}

		private static PlayerHolderService CreatePlayerHolderService (DIContainer c)
		{
			return new PlayerHolderService();
		}

		private static GameplayPopupService CreateGameplayPopupService (DIContainer c)
		{
			return new GameplayPopupService(
				c.Resolve<ViewFactory>(),
				c.Resolve<ProjectPresentersFactory>(),
				c.Resolve<GameplayUIRoot>(),
				c.Resolve<GameplayPresentersFactory>());
		}

		private static GameplayUIRoot CreateGameplayUIRoot (DIContainer c)
		{
			ResourcesAssetLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetLoader>();

			GameplayUIRoot gameplayUIRootPrefab = resourcesAssetsLoader
				.Load<GameplayUIRoot>("UI/Gameplay/Gameplay UI Root");

			return Object.Instantiate(gameplayUIRootPrefab);
		}

		private static GameplayScreenPresenter CreateGameplayScreenPresenter (DIContainer c)
		{
			GameplayUIRoot uiRoot = c.Resolve<GameplayUIRoot>();

			GameplayScreenView view = c
			                          .Resolve<ViewFactory>()
			                          .Create<GameplayScreenView>(ViewIDs.GameplayScreen, uiRoot.HUDLayer);

			GameplayScreenPresenter presenter = c
			                                    .Resolve<GameplayPresentersFactory>()
			                                    .CreateGameplayScreenPresenter(view);

			presenter.Initialize();

			return presenter;
		}

		private static GameplayPresentersFactory CreateGameplayPresentersFactory (DIContainer c)
		{
			return new GameplayPresentersFactory(c);
		}

		private static GameTimer CreateTimer (DIContainer c)
		{
			return new GameTimer();
		}

		private static EntranceActivatorService CreateEntranceActivatorService (DIContainer c)
		{
			return new EntranceActivatorService(c.Resolve<GameTimer>());
		}
	}
}
