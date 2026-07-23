using Modules.StateMachineLite;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Window_States
{
	public class WindowWarningState : IState<Window, EntranceTriggers>
	{
		private float _breachTimer;
		private float _pacifyTimer;

		public void OnEnter (IStateMachine<Window, EntranceTriggers> machine)
		{
			_breachTimer = machine.Context.BreachTime;
			_pacifyTimer = machine.Context.PacifyTime;

			Debug.Log("Something is at the window");
		}

		public void OnExit (IStateMachine<Window, EntranceTriggers> machine)
		{

		}

		public void Tick (IStateMachine<Window, EntranceTriggers> machine, float deltaTime)
		{
			Camera camera = machine.Context.Player.Camera;

			if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit))
			{
				Window window = machine.Context;

				if (hit.collider.gameObject == window.gameObject && window.Player.IsUsingFlashlight)
				{
					_pacifyTimer -= deltaTime;
				}
				else
				{
					_breachTimer -= deltaTime;
					_pacifyTimer =  machine.Context.PacifyTime;
				}
			}
			else
			{
				_breachTimer -= deltaTime;
				_pacifyTimer =  machine.Context.PacifyTime;
			}

			if (_pacifyTimer <= 0)
			{
				machine.Fire(EntranceTriggers.idle);
			}

			if (_breachTimer <= 0)
			{
				machine.Fire(EntranceTriggers.breach);
			}
		}
	}
}
