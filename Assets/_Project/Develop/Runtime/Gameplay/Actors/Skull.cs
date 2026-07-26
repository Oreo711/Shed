using LitMotion;
using LitMotion.Extensions;
using Shed.Runtime.Utilities.LitMotion;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Actors
{
	public class Skull : MonoBehaviour
	{
		[SerializeField] private Vector3   _startPosition;
		[SerializeField] private float     _showDuration;
		[SerializeField] private Transform _bobbingTransform;
		[SerializeField] private float     _bobbingAmplitude = 0.1f;

		private MotionHandle _motionHandle;

		private Vector3 _initialPosition;
		private float   _xSeed;
		private float   _ySeed;

		private void Awake ()
		{
			_initialPosition =  transform.position;
			_startPosition   += transform.position;

			_motionHandle = LMotion.Create(_startPosition, _initialPosition, 1f)
			                       .WithEase(Ease.OutQuad)
			                       .BindToPosition(transform)
			                       .Preserve()
			                       .Pause()
			                       .AddTo(this);

			_xSeed = Random.Range(1f, 1000f);
			_ySeed = Random.Range(1f, 1000f);
		}

		private void Update ()
		{
			float x = Mathf.PerlinNoise(Time.time + _xSeed, 0) * 2f - 1f;
			float y = Mathf.PerlinNoise(Time.time + _ySeed, 0) * 2f - 1f;
			_bobbingTransform.localPosition = new Vector3(x, y, 0f) * _bobbingAmplitude;
		}

		public void Show ()
		{
			_motionHandle.PlayForward();
		}

		public void Hide ()
		{
			_motionHandle.PlayBackwards();
		}

		private void OnDestroy ()
		{
			_motionHandle.TryCancel();
		}

		#if UNITY_EDITOR
		private void OnDrawGizmos ()
		{
			Gizmos.color = Color.dodgerBlue;
			Gizmos.DrawSphere(_startPosition + transform.position, 0.2f);
		}
		#endif
	}
}
