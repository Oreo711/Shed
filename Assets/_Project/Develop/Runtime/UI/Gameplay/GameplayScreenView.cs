using System.Collections;
using System.Collections.Generic;
using LitMotion;
using Shed.Runtime.UI.Core;
using Shed.Runtime.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Shed.Runtime.UI.Gameplay
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

		private MotionHandle _clockFadeHandle;

		private void Awake ()
		{
			_clockFadeHandle = LMotion.Create(0f, 1f, 0.15f)
			                          .WithEase(Ease.OutQuad)
			                          .BindToAlpha(_clock)
			                          .Pause()
			                          .Preserve()
			                          .AddTo(this);
		}

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

		public void EnableRechargeBar () => _rechargeBar.gameObject.SetActive(true);

		public void DisableRechargeBar () => _rechargeBar.gameObject.SetActive(false);

		public void EnableClockWidget ()
		{
			_clockFadeHandle.PlayForward();
		}

		public void DisableClockWidget ()
		{
			_clockFadeHandle.PlayBackwards();
		}

		// public void EnablePressInteractGuide() => _pressInteractGuide.gameObject.SetActive(true);
		//
		// public void DisablePressInteractGuide() => _pressInteractGuide.gameObject.SetActive(false);

		public void EnableHoldInteractGuide () => _holdInteractGuide.gameObject.SetActive(true);

		public void DisableHoldInteractGuide () => _holdInteractGuide.gameObject.SetActive(false);

		public void EnableFlashlightGuide () => _flashlightGuide.gameObject.SetActive(true);

		public void DisableFlashlightGuide () => _flashlightGuide.gameObject.SetActive(false);

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

		private void OnDestroy ()
		{
			_clockFadeHandle.TryCancel();
		}
	}
}
