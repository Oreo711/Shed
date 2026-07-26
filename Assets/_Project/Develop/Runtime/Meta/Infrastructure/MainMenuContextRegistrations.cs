using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.UI;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.UI.MainMenu;
using _Project.Develop.Runtime.Utilities.AssetManagment;
using UnityEngine;


namespace _Project.Develop.Runtime.Meta.Infrastructure
{
    public static class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            container.Register(CreateMainMenuUIRoot);

            container.Register(CreateMainMenuPresentersFactory);

            container.Register(CreateMainMenuScreenPresenter);

            container.Register(CreateMainMenuPopupService);
        }

        private static MainMenuPopupService CreateMainMenuPopupService (DIContainer c)
        {
            return new MainMenuPopupService(
                c.Resolve<ViewFactory>(),
                c.Resolve<ProjectPresentersFactory>(),
                c.Resolve<MainMenuUIRoot>(),
                c.Resolve<MainMenuPresentersFactory>());
        }

        private static MainMenuUIRoot CreateMainMenuUIRoot (DIContainer c)
        {
            ResourcesAssetLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetLoader>();

            MainMenuUIRoot gameplayUIRootPrefab = resourcesAssetsLoader
                .Load<MainMenuUIRoot>("UI/Meta/Main Menu UI Root");

            return Object.Instantiate(gameplayUIRootPrefab);
        }

        private static MainMenuScreenPresenter CreateMainMenuScreenPresenter (DIContainer c)
        {
            MainMenuUIRoot uiRoot = c.Resolve<MainMenuUIRoot>();

            MainMenuScreenView view = c
                                      .Resolve<ViewFactory>()
                                      .Create<MainMenuScreenView>(ViewIDs.MainMenuScreen, uiRoot.HUDLayer);

            MainMenuScreenPresenter presenter = c
                                                .Resolve<MainMenuPresentersFactory>()
                                                .CreateMainMenuScreenPresenter(view);

            presenter.Initialize();

            return presenter;
        }

        private static MainMenuPresentersFactory CreateMainMenuPresentersFactory (DIContainer c)
        {
            return new MainMenuPresentersFactory(c);
        }
    }
}
