using System;
using System.Collections;
using Shed.Runtime.Infrastructure;
using Shed.Runtime.Infrastructure.DI;
using Shed.Runtime.Utilities.LoadingScreen;
using Object = UnityEngine.Object;

namespace Shed.Runtime.Utilities.SceneManagment
{
    public class SceneSwitcherService
    {
        private readonly SceneLoaderService _sceneLoaderService;
        private readonly ILoadingScreen _loadingScreen;
        private readonly DIContainer _projectContainer;

        private DIContainer _currentSceneContainer;

        public SceneSwitcherService(
            SceneLoaderService sceneLoaderService,
            ILoadingScreen loadingScreen,
            DIContainer projectContainer
        )
        {
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen      = loadingScreen;
            _projectContainer   = projectContainer;
        }

        public IEnumerator SwitchToAsync(string sceneName, IInputSceneArgs sceneArgs = null)
        {
            _loadingScreen.Show();

            _currentSceneContainer?.Dispose();

            yield return _sceneLoaderService.LoadAsync(Scenes.Empty);
            yield return _sceneLoaderService.LoadAsync(sceneName);

            SceneBootstrap sceneBootstrap = Object.FindFirstObjectByType<SceneBootstrap>();

            if (!sceneBootstrap)
                throw new NullReferenceException(nameof(sceneBootstrap) + " not found");

            _currentSceneContainer = new DIContainer(_projectContainer);

            sceneBootstrap.ProcessRegistrations(_currentSceneContainer);

            _currentSceneContainer.Initialize();

            yield return sceneBootstrap.Initialize();

            _loadingScreen.Hide();

            sceneBootstrap.Run();
        }
    }
}
