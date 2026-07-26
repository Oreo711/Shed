using LitMotion;
using Shed.Runtime.Utilities.LitMotion;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Hints
{
	public class ClosetBreachSourceHint : BreachSourceHint
	{
		[SerializeField] private AudioSource _rummagingSound;
		[SerializeField] private Light       _light;
		[SerializeField] private float       _lightFadeDuration = 1.0f;
		[SerializeField] private GameObject  _monster;

		private MotionHandle _lightFadeHandle;
		private float        _maxLightIntensity;
		private Vector3      _doorInitialRotation;

		private void Awake ()
		{
			_maxLightIntensity   = _light.intensity;

			_lightFadeHandle = LMotion.Create(0f, 1f, _lightFadeDuration)
			                          .WithEase(Ease.OutQuad)
			                          .Bind(Tick)
			                          .Preserve()
			                          .Pause()
			                          .AddTo(this);
		}

		private void Tick (float normalizedProgress)
		{
			SetLightIntensity(normalizedProgress);
		}

		private void SetLightIntensity (float normalizedProgress)
		{
			_light.intensity = Mathf.Lerp(0f, _maxLightIntensity, normalizedProgress);
		}

		public override void Show ()
		{
			_lightFadeHandle.PlayForward();
			_rummagingSound.Play();
			_monster.gameObject.SetActive(true);
		}

		public override void Hide ()
		{
			_lightFadeHandle.PlayBackwards();
			_monster.gameObject.SetActive(false);
		}

		private void OnDestroy ()
		{
			_lightFadeHandle.TryCancel();
		}
	}
}
