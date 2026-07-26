using System;
using _Project.Develop.Runtime.UI.Core;
using UnityEngine;
using UnityEngine.UI;


namespace _Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;

        public event Action PlayButtonPressed;
        public event Action QuitButtonPressed;

        private void Start ()
        {
            _playButton.onClick.AddListener(HandlePlayButtonPressed);
            _quitButton.onClick.AddListener(HandleQuitButtonPressed);
        }

        private void HandlePlayButtonPressed ()
        {
            PlayButtonPressed?.Invoke();
        }

        private void HandleQuitButtonPressed ()
        {
            QuitButtonPressed?.Invoke();
        }

        private void OnDestroy ()
        {
            _playButton.onClick.RemoveListener(HandlePlayButtonPressed);
            _quitButton.onClick.RemoveListener(HandleQuitButtonPressed);
        }
    }
}
