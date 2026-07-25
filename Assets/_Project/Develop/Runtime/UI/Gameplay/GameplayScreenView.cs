using System;
using _Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenView : MonoBehaviour, IView
    {
        [SerializeField] private Slider     _battery;
        [SerializeField] private TMP_Text   _clock;
        [SerializeField] private Slider     _rechargeBar;
        [SerializeField] private GameObject _interactGuide;

        public void SetClock (int hours, int minutes)
        {
            _clock.text = hours + ":" + minutes.ToString("00");
        }

        public void SetBatteryPercentage (float percentage)
        {
            _battery.value = percentage / 100;
        }

        public void SetRechargeBarPercentage (float percentage)
        {
            _rechargeBar.value = percentage;
        }

        public void EnableRechargeBar() => _rechargeBar.gameObject.SetActive(true);

        public void DisableRechargeBar() => _rechargeBar.gameObject.SetActive(false);

        public void EnableClock() => _clock.gameObject.SetActive(true);

        public void DisableClock() => _clock.gameObject.SetActive(false);

        public void EnableInteractGuide() => _interactGuide.gameObject.SetActive(true);

        public void DisableInteractGuide() => _interactGuide.gameObject.SetActive(false);
    }
}
