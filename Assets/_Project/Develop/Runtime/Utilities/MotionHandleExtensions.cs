using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LitMotion;
using UnityEngine;
using UnityEngine.UI;


namespace Shed.Runtime.Utilities
{
	[PublicAPI]
	public static class MotionHandleExtensions
	{
		private readonly struct RotationBindState
		{
			public readonly Transform  Transform;
			public readonly Quaternion BaseRotation;
			public readonly Vector3    Axis;

			public RotationBindState (Transform transform, Quaternion baseRotation, Vector3 axis)
			{
				Transform    = transform;
				BaseRotation = baseRotation;
				Axis         = axis;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MotionHandle Pause (this MotionHandle handle)
		{
			handle.PlaybackSpeed = 0;
			return handle;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MotionHandle ClampTime (this MotionHandle handle)
		{
			handle.Time = Math.Clamp(handle.Time, 0, handle.Duration);
			return handle;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MotionHandle PlayForward (this MotionHandle handle, float speed = 1)
		{
			handle.ClampTime();
			handle.PlaybackSpeed = speed;
			return handle;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MotionHandle PlayBackwards (this MotionHandle handle, float speed = 1)
		{
			handle.ClampTime();
			handle.PlaybackSpeed = -speed;
			return handle;
		}

		public static MotionHandle BindToRotation<TOptions, TAdapter> (
			this MotionBuilder<Quaternion, TOptions, TAdapter> builder,
			TransformHandle transformHandle
		)
			where TOptions : unmanaged, IMotionOptions
			where TAdapter : unmanaged, IMotionAdapter<Quaternion, TOptions>
		{
			return builder.Bind(transformHandle, static (value, transformHandle) => { transformHandle.rotation = value; });
		}

		public static MotionHandle BindToEulerAngles<TOptions, TAdapter> (
			this MotionBuilder<Vector3, TOptions, TAdapter> builder,
			TransformHandle transformHandle
		)
			where TOptions : unmanaged, IMotionOptions
			where TAdapter : unmanaged, IMotionAdapter<Vector3, TOptions>
		{
			return builder.Bind(transformHandle, static (value, transformHandle) => { transformHandle.eulerAngles = value; });
		}

		public static MotionHandle BindToLocalScale<TOptions, TAdapter> (
			this MotionBuilder<Vector3, TOptions, TAdapter> builder,
			TransformHandle transformHandle
		)
			where TOptions : unmanaged, IMotionOptions
			where TAdapter : unmanaged, IMotionAdapter<Vector3, TOptions>
		{
			return builder.Bind(transformHandle, static (value, handle) => { handle.localScale = value; });
		}

		public static MotionHandle BindToSizeDelta<TOptions, TAdapter> (
			this MotionBuilder<Vector2, TOptions, TAdapter> builder,
			RectTransform rectTransform
		)
			where TOptions : unmanaged, IMotionOptions
			where TAdapter : unmanaged, IMotionAdapter<Vector2, TOptions>
		{
			return builder.Bind(rectTransform, static (value, transform) => { transform.sizeDelta = value; });
		}

		public static MotionHandle BindToAlpha<TOptions, TAdapter> (
			this MotionBuilder<float, TOptions, TAdapter> builder,
			Graphic graphic
		)
			where TOptions : unmanaged, IMotionOptions
			where TAdapter : unmanaged, IMotionAdapter<float, TOptions>
		{
			return builder.Bind(graphic, static (value, graphic) =>
			{
				Color color = graphic.color;
				color.a       = value;
				graphic.color = color;
			});
		}

		public static MotionHandle BindToLocalRotationX<TOptions, TAdapter> (
			this MotionBuilder<float, TOptions, TAdapter> builder,
			Transform transform
		)
			where TOptions : unmanaged, IMotionOptions
			where TAdapter : unmanaged, IMotionAdapter<float, TOptions>
		{
			Quaternion        baseRotation = transform.localRotation;
			RotationBindState state        = new(transform, baseRotation, Vector3.right);
			return builder.Bind(state, static (value, s) => { s.Transform.localRotation = s.BaseRotation * Quaternion.AngleAxis(value, s.Axis); });
		}

		public static MotionHandle BindToLocalRotationY<TOptions, TAdapter> (
			this MotionBuilder<float, TOptions, TAdapter> builder,
			Transform transform
		)
			where TOptions : unmanaged, IMotionOptions
			where TAdapter : unmanaged, IMotionAdapter<float, TOptions>
		{
			Quaternion        baseRotation = transform.localRotation;
			RotationBindState state        = new(transform, baseRotation, Vector3.up);
			return builder.Bind(state, static (value, s) => { s.Transform.localRotation = s.BaseRotation * Quaternion.AngleAxis(value, s.Axis); });
		}

		public static MotionHandle BindToLocalRotationZ<TOptions, TAdapter> (
			this MotionBuilder<float, TOptions, TAdapter> builder,
			Transform transform
		)
			where TOptions : unmanaged, IMotionOptions
			where TAdapter : unmanaged, IMotionAdapter<float, TOptions>
		{
			Quaternion        baseRotation = transform.localRotation;
			RotationBindState state        = new(transform, baseRotation, Vector3.forward);
			return builder.Bind(state, static (value, s) => { s.Transform.localRotation = s.BaseRotation * Quaternion.AngleAxis(value, s.Axis); });
		}
	}
}
