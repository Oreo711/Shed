using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;


namespace Modules.TimeMachine
{
	internal class TimeMachine : EditorWindow
	{
		private const string TimeScaleKey = "forced_time_scale";

		private static Texture2D _windowIcon;
		private static Texture2D _plusIcon;
		private static Texture2D _minusIcon;

		private const float Increment    = 0.1f;
		private const float MinTimeScale = 0.1f;
		private const float MaxTimeScale = 4.0f;

		private readonly Vector2 _buttonSize = new(30, EditorGUIUtility.singleLineHeight);

		private static float TimeScale
		{
			get => GetForcedTimeScale();
			set => SetForcedTimeScale(value);
		}

		private void OnEnable ()
		{
			_windowIcon = LoadIcon("SpeedScale");
			_plusIcon   = LoadIcon("d_Toolbar Plus");
			_minusIcon  = LoadIcon("d_Toolbar Minus");

			titleContent = new GUIContent("Time Machine") { image = _windowIcon };
		}

		private void OnGUI ()
		{
			using (new GUILayout.HorizontalScope())
			{
				TimeScale = EditorGUILayout.Slider(TimeScale, MinTimeScale, MaxTimeScale);

				if (GUILayout.Button(_minusIcon, GUILayout.Width(_buttonSize.x), GUILayout.Height(_buttonSize.y)))
				{
					TimeScale -= Increment;
				}

				if (GUILayout.Button("1", GUILayout.Width(_buttonSize.x), GUILayout.Height(_buttonSize.y)))
				{
					TimeScale = 1;
				}

				if (GUILayout.Button(_plusIcon, GUILayout.Width(_buttonSize.x), GUILayout.Height(_buttonSize.y)))
				{
					TimeScale += Increment;
				}
			}
		}

		[MenuItem("Window/Time Machine")]
		public static void ShowWindow ()
		{
			GetWindow<TimeMachine>().Show();
		}

		[InitializeOnEnterPlayMode]
		private static void OnEnterPlayMode ()
		{
			ApplyForcedTimeScale();
		}

		private static void ApplyForcedTimeScale ()
		{
			float forcedTimeScale = GetForcedTimeScale();
			bool  shouldApply     = !Mathf.Approximately(forcedTimeScale, Time.timeScale);

			if (shouldApply)
			{
				Time.timeScale = forcedTimeScale;
				Debug.LogWarning($"Time Scale forced to {forcedTimeScale.ToString("0.0", CultureInfo.InvariantCulture)} by Time Machine");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float GetForcedTimeScale ()
		{
			float defaultValue = Time.timeScale;

			return SessionState.GetFloat(TimeScaleKey, defaultValue);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SetForcedTimeScale (float value)
		{
			value = Mathf.Clamp(value, MinTimeScale, MaxTimeScale);
			value = (float)Math.Round(value, 1);

			SessionState.SetFloat(TimeScaleKey, value);

			if (Application.isPlaying)
			{
				ApplyForcedTimeScale();
			}
		}

		private static Texture2D LoadIcon (string iconName)
		{
			Texture2D icon = (Texture2D)EditorGUIUtility.Load(iconName);
			icon.filterMode = FilterMode.Bilinear;

			return icon;
		}
	}
}
