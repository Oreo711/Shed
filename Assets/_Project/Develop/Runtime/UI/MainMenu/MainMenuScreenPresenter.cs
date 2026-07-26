using Shed.Runtime.UI.Core;
using Shed.Runtime.Utilities.CoroutineManagement;
using Shed.Runtime.Utilities.SceneManagment;
using UnityEngine;


namespace Shed.Runtime.UI.MainMenu
{
	public class MainMenuScreenPresenter : IPresenter
	{
		private readonly MainMenuScreenView   _screen;
		private readonly ICoroutinePerformer  _coroutinePerformer;
		private readonly SceneSwitcherService _sceneSwitcherService;

		public MainMenuScreenPresenter (MainMenuScreenView screen, ICoroutinePerformer coroutinePerformer, SceneSwitcherService sceneSwitcherService)
		{
			_screen               = screen;
			_coroutinePerformer            = coroutinePerformer;
			_sceneSwitcherService = sceneSwitcherService;
		}

		public void Initialize ()
		{
			_screen.PlayButtonPressed += HandlePlayButtonPressed;
			_screen.QuitButtonPressed += HandleQuitButtonPressed;
		}

		private void HandlePlayButtonPressed ()
		{
			_coroutinePerformer.StartCoroutine(_sceneSwitcherService.SwitchToAsync(Scenes.Gameplay));
		}

		private void HandleQuitButtonPressed ()
		{
			Application.Quit();
		}

		public void Dispose ()
		{
			_screen.PlayButtonPressed -= HandlePlayButtonPressed;
			_screen.QuitButtonPressed -= HandleQuitButtonPressed;
		}
	}
}
