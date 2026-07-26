using LitMotion;
using Shed.Runtime.Utilities.LitMotion;
using Unity.Jobs;
using UnityEngine;


[assembly: RegisterGenericJobType(typeof(MotionUpdateJob<Vector3, JumpMotionOptions, JumpMotionAdapter>))]

namespace Shed.Runtime.Utilities.LitMotion
{
	public readonly struct JumpMotionAdapter : IMotionAdapter<Vector3, JumpMotionOptions>
	{
		public Vector3 Evaluate (ref Vector3 startValue, ref Vector3 endValue, ref JumpMotionOptions options, in MotionEvaluationContext context)
		{
			float   heightOffset = Mathf.Sin(Mathf.PI * context.Progress) * options.Height;
			Vector3 flatPosition = Vector3.LerpUnclamped(startValue, endValue, context.Progress);

			return new Vector3(
				flatPosition.x,
				flatPosition.y + heightOffset,
				flatPosition.z
			);
		}
	}
}
