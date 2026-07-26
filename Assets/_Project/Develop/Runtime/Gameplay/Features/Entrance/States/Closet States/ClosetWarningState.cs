using LitMotion;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using Shed.Runtime.Utilities.LitMotion;
using UnityEditorInternal;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Closet_States
{
	public class ClosetWarningState : IState<Closet, EntranceTriggers>
	{
		private float _breachTimer;
		private float _pacifyTimer;
		private IStateMachine<Closet, EntranceTriggers> _machine;

		private Vector3      _doorInitialRotation;
		private MotionHandle _doorOpenHandle;
		private bool         _wasBeingPacified;

		public void OnEnter (IStateMachine<Closet, EntranceTriggers> machine)
		{
			_machine = machine;
			Closet closet = machine.Context;
			_breachTimer         = closet.BreachTime;
			_pacifyTimer         = closet.PacifyTime;
			_doorInitialRotation = closet.DoorTransform.localEulerAngles;

			closet.Player.EncounteredCloset = true;
			closet.ShowHint();

			_doorOpenHandle = LMotion.Create(0f, 1f, machine.Context.DoorOpenDuration)
			                          .WithEase(Ease.OutQuad)
			                          .Bind(Tick)
			                          .Preserve()
			                          .Pause()
			                          .AddTo(closet);

			_doorOpenHandle.PlayForward();
		}

		private void Tick (float normalizedProgress)
		{
			SetDoorRotation(normalizedProgress);
		}

		private void SetDoorRotation (float normalizedProgress)
		{
			float   targetAngle = Mathf.Lerp(0f, _machine.Context.OpenDoorAngle, normalizedProgress);
			Vector3 newRotation = _doorInitialRotation + new Vector3(0f, targetAngle, 0f);
			_machine.Context.DoorTransform.localEulerAngles = newRotation;
		}

		public void OnExit (IStateMachine<Closet, EntranceTriggers> machine)
		{
			Closet closet = machine.Context;
			closet.HideHint();
			_doorOpenHandle.PlayBackwards();
		}

		public void Tick (IStateMachine<Closet, EntranceTriggers> machine, float deltaTime)
		{
			if (machine.Context.IsBeingPacified)
			{
				_pacifyTimer -= deltaTime;

				if (!_wasBeingPacified)
				{
					_doorOpenHandle.PlayBackwards();
					_wasBeingPacified = true;
				}
			}
			else
			{
				if (!machine.Context.Player.CanNotAct)
				{
					_breachTimer -= deltaTime;
				}

				if (_wasBeingPacified)
				{
					_doorOpenHandle.PlayForward();
					_wasBeingPacified = false;
				}

				_pacifyTimer = machine.Context.PacifyTime;
			}

			if (_pacifyTimer <= 0)
			{
				machine.Context.Player.PacifiedCloset = true;
				machine.Fire(EntranceTriggers.Idle);
				machine.Context.PlayPacifiedSound();
			}

			if (_breachTimer <= 0)
			{
				machine.Fire(EntranceTriggers.Breach);
			}
		}
	}
}
