using LitMotion;
using Shed.Runtime.Utilities.LitMotion;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Hints
{
	public class DoorBreachSourceHint : BreachSourceHint
	{
		[SerializeField] private Light _light;
		[SerializeField] private float _lightFadeDuration = 1.0f;

		[SerializeField] private Transform _doorTransform;
		[SerializeField] private float     _openDoorAngle = 20f;

		private MotionHandle _lightFadeHandle;
		private float        _maxLightIntensity;
		private Vector3        _doorInitialRotation;

		private void Awake ()
		{
			_maxLightIntensity = _light.intensity;
			_doorInitialRotation    = _doorTransform.localEulerAngles;

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
			SetDoorRotation(normalizedProgress);
		}

		private void SetLightIntensity (float normalizedProgress)
		{
			_light.intensity = Mathf.Lerp(0f, _maxLightIntensity, normalizedProgress);
		}

		private void SetDoorRotation (float normalizedProgress)
		{

			float targetAngle = Mathf.Lerp(0f, _openDoorAngle, normalizedProgress);
			Vector3 newRotation = _doorInitialRotation + new Vector3(0f, targetAngle, 0f);
			_doorTransform.localEulerAngles = newRotation;
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
