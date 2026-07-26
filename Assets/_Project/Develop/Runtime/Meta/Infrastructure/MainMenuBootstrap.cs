using System.Collections;
using _Project.Develop.Runtime.Infrastructure;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.UI.MainMenu;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;
using _Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using _Project.Develop.Runtime.Utilities.SceneManagment;
using UnityEngine;


namespace _Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer          _container;
        private PlayerDataProvider   _playerDataProvider;
        private SceneSwitcherService _sceneSwitcherService;
        private ICoroutinePerformer  _coroutinePerformer;

        private MainMenuScreenPresenter _screen;

        public override void ProcessRegistrations(DIContainer container)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            _playerDataProvider   = _container.Resolve<PlayerDataProvider>();
            _coroutinePerformer   = _container.Resolve<ICoroutinePerformer>();
            _sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            _screen               = _container.Resolve<MainMenuScreenPresenter>();

            yield break;
        }

        public override void Run()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _coroutinePerformer.StartCoroutine(_playerDataProvider.SaveAsync());
                Debug.Log("Saved!");
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                _coroutinePerformer.StartCoroutine(_sceneSwitcherService.SwitchToAsync(Scenes.Gameplay));
            }
        }
    }
}
