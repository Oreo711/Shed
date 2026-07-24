using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.UI;
using _Project.Develop.Runtime.UI.Core;
using _Project.Develop.Runtime.Utilities.AssetManagment;
using _Project.Develop.Runtime.Utilities.ConfigManagement;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagment;
using _Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using _Project.Develop.Runtime.Utilities.DataManagment.DataRepository;
using _Project.Develop.Runtime.Utilities.DataManagment.KeysStorage;
using _Project.Develop.Runtime.Utilities.DataManagment.Serializers;
using _Project.Develop.Runtime.Utilities.LoadingScreen;
using _Project.Develop.Runtime.Utilities.SceneManagment;
using _Project.Develop.Runtime.Utilities.LoadingScreen;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Develop.Runtime.Infrastructure.EntryPoint
{
    public static class ProjectContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            container.Register(CreateResourcesAssetLoader);

            container.Register(CreateSceneLoaderService);

            container.Register(CreateSceneSwitcherService);

            container.Register<ILoadingScreen>(CreateLoadingScreen);

            container.Register<ICoroutinePerformer>(CreateCoroutinePerformer);

            container.Register(CreatePlayerDataProvider);

            container.Register<ISaveLoadService>(CreateSaveLoadService);

            container.Register(CreateConfigProviderService);

            container.Register(CreateProjectPresentersFactory);

            container.Register(CreateViewFactory);
        }

        private static ViewFactory CreateViewFactory(DIContainer c)
            => new(c.Resolve<ResourcesAssetLoader>());

        private static ProjectPresentersFactory CreateProjectPresentersFactory(DIContainer c)
            => new ProjectPresentersFactory(c);

        private static ConfigProviderService CreateConfigProviderService(DIContainer c)
        {
            ResourcesAssetLoader resourcesAssetLoader = c.Resolve<ResourcesAssetLoader>();

            ResourcesConfigsLoader resourcesConfigsLoader = new ResourcesConfigsLoader(resourcesAssetLoader);

            return new ConfigProviderService(resourcesConfigsLoader);
        }

        private static PlayerDataProvider CreatePlayerDataProvider(DIContainer c)
            => new PlayerDataProvider(c.Resolve<ISaveLoadService>());

        private static SaveLoadService CreateSaveLoadService(DIContainer c)
        {
            IDataSerializer  dataSerializer  = new JsonSerializer();
            IDataKeysStorage dataKeysStorage = new MapDataKeysStorage();

            string saveFolderPath = Application.isEditor ? Application.dataPath : Application.persistentDataPath;

            IDataRepository dataRepository = new LocalFileDataRepository(saveFolderPath, "json");

            return new SaveLoadService(dataSerializer, dataKeysStorage, dataRepository);
        }

        private static SceneSwitcherService CreateSceneSwitcherService(DIContainer c)
            => new SceneSwitcherService(
                c.Resolve<SceneLoaderService>(),
                c.Resolve<ILoadingScreen>(),
                c);

        private static SceneLoaderService CreateSceneLoaderService(DIContainer c)
            => new SceneLoaderService();

        private static ResourcesAssetLoader CreateResourcesAssetLoader(DIContainer c)
            => new ResourcesAssetLoader();

        private static CoroutinePerformer CreateCoroutinePerformer(DIContainer c)
        {
            ResourcesAssetLoader resourcesAssetLoader = c.Resolve<ResourcesAssetLoader>();

            CoroutinePerformer coroutinePerformerPrefab = resourcesAssetLoader
                .Load<CoroutinePerformer>("Utilities/Coroutine Performer");

            return Object.Instantiate(coroutinePerformerPrefab);
        }

        private static StandardLoadingScreen CreateLoadingScreen(DIContainer c)
        {
            ResourcesAssetLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetLoader>();

            StandardLoadingScreen standardLoadingScreenPrefab = resourcesAssetsLoader
                .Load<StandardLoadingScreen>("Utilities/Standard Loading Screen");

            return Object.Instantiate(standardLoadingScreenPrefab);
        }
    }
}
