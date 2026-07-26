using System.Collections;
using Shed.Runtime.Infrastructure.DI;
using Shed.Runtime.Utilities.ConfigManagement;
using Shed.Runtime.Utilities.CoroutineManagement;
using Shed.Runtime.Utilities.DataManagment.DataProviders;
using Shed.Runtime.Utilities.LoadingScreen;
using Shed.Runtime.Utilities.SceneManagment;
using UnityEngine;


namespace Shed.Runtime.Infrastructure.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            SetupAppSettings();

            DIContainer projectContainer = new DIContainer();

            ProjectContextRegistrations.Process(projectContainer);

            projectContainer.Initialize();

            projectContainer.Resolve<ICoroutinePerformer>().StartCoroutine(Initialize(projectContainer));
        }

        private void SetupAppSettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        private IEnumerator Initialize(DIContainer container)
        {
            ILoadingScreen       loadingScreen        = container.Resolve<ILoadingScreen>();
            SceneSwitcherService sceneSwitcherService = container.Resolve<SceneSwitcherService>();
            PlayerDataProvider   playerDataProvider   = container.Resolve<PlayerDataProvider>();

            loadingScreen.Show();

            yield return container.Resolve<ConfigProviderService>().LoadAsync();

            bool isPlayerDataSaveExists = false;

            yield return playerDataProvider.ExistsAsync(result => isPlayerDataSaveExists = result);

            if (isPlayerDataSaveExists)
                yield return playerDataProvider.LoadAsync();
            else
                playerDataProvider.Reset();

            loadingScreen.Hide();

            yield return sceneSwitcherService.SwitchToAsync(Scenes.MainMenu);
        }
    }
}
