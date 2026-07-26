using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenView : MonoBehaviour, IView
    {
        [SerializeField] private Slider   _battery;
        [SerializeField] private TMP_Text _clock;
        [SerializeField] private Slider   _rechargeBar;
        [SerializeField] private TMP_Text _flashlightGuide;
        [SerializeField] private TMP_Text _pressInteractGuide;
        [SerializeField] private TMP_Text _holdInteractGuide;
        [SerializeField] private TMP_Text _monologue;

        private readonly Queue<string> _monologueRequests        = new();
        private          bool          _currentMonologueFinished = true;

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

        // public void EnablePressInteractGuide() => _pressInteractGuide.gameObject.SetActive(true);
        //
        // public void DisablePressInteractGuide() => _pressInteractGuide.gameObject.SetActive(false);

        public void EnableHoldInteractGuide() => _holdInteractGuide.gameObject.SetActive(true);

        public void DisableHoldInteractGuide() => _holdInteractGuide.gameObject.SetActive(false);

        public void EnableFlashlightGuide() => _flashlightGuide.gameObject.SetActive(true);

        public void DisableFlashlightGuide() => _flashlightGuide.gameObject.SetActive(false);

        public void QueueMonologueText (string text)
        {
            _monologueRequests.Enqueue(text);
        }

        public void SetMonologueText ()
        {
            if (_monologueRequests.Count == 0 || !_currentMonologueFinished)
                return;

            _currentMonologueFinished = false;
            string text = _monologueRequests.Dequeue();
            _monologue.text = "";
            StartCoroutine(TypeLetterByLetter(_monologue, text));
        }

        private IEnumerator TypeLetterByLetter (TMP_Text textObject, string text)
        {
            char[] chars = text.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                textObject.text += chars[i];
                yield return new WaitForSeconds(0.03f);
            }

            yield return new WaitForSeconds(3f);

            textObject.text           = "";
            _currentMonologueFinished = true;
        }
    }
}
