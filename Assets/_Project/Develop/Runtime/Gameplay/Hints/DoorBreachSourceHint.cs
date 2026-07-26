using LitMotion;
using Shed.Runtime.Utilities;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Hints
{
	public class DoorBreachSourceHint : BreachSourceHint
	{
		[SerializeField] private Light _light;
		[SerializeField] private float _lightFadeDuration = 1.0f;

		private MotionHandle _lightFadeHandle;
		private float        _maxLightIntensity;

		private void Awake ()
		{
			_maxLightIntensity = _light.intensity;

			_lightFadeHandle = LMotion.Create(0f, 1f, _lightFadeDuration)
			                          .WithEase(Ease.OutQuad)
			                          .Bind(SetLightIntensity)
			                          .Preserve()
			                          .Pause()
			                          .AddTo(this);
		}

		private void SetLightIntensity (float normalizedProgress)
		{
			_light.intensity = Mathf.Lerp(0f, _maxLightIntensity, normalizedProgress);
		}

		public override void Show ()
		{
			_lightFadeHandle.PlayForward();
		}

		public override void Hide ()
		{
			_lightFadeHandle.PlayBackwards();
		}

		private void OnDestroy ()
		{
			_lightFadeHandle.TryCancel();
		}
	}
}