using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Window_States
{
	public class WindowWarningState : IState<Window, EntranceTriggers>
	{
		private float _breachTimer;
		private float _pacifyTimer;

		public void OnEnter (IStateMachine<Window, EntranceTriggers> machine)
		{
			Window window = machine.Context;

			_breachTimer = window.BreachTime;
			_pacifyTimer = window.PacifyTime;
			window.Player.MonologueService.ShowWindowEncounterMonologue();

			window.ShowHint();
		}

		public void OnExit (IStateMachine<Window, EntranceTriggers> machine)
		{
			Window window = machine.Context;
			window.HideHint();
		}

		public void Tick (IStateMachine<Window, EntranceTriggers> machine, float deltaTime)
		{
			Camera camera = machine.Context.Player.Camera;

			if (
				Physics.Raycast(camera.transform.position,
					camera.transform.forward,
					out RaycastHit hit, 20f,
					LayerMask.GetMask("Entrance")))
			{
				Window window = machine.Context;

				if (hit.collider.gameObject == window.gameObject && window.Player.IsUsingFlashlight)
				{
					_pacifyTimer -= deltaTime;
				} else
				{
					if (!machine.Context.Player.CanNotAct)
					{
						_breachTimer -= deltaTime;
					}

					_pacifyTimer = machine.Context.PacifyTime;
				}
			} else
			{
				_breachTimer -= deltaTime;
				_pacifyTimer =  machine.Context.PacifyTime;
			}

			if (_pacifyTimer <= 0)
			{
				machine.Fire(EntranceTriggers.Idle);
			}

			if (_breachTimer <= 0)
			{
				machine.Fire(EntranceTriggers.Breach);
			}
		}
	}
}
