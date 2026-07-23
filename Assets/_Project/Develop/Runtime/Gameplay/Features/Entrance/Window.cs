using System;
using _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Window_States;
using Modules.StateMachineLite;
using Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public class Window : MonoBehaviour, IStateContext
	{
		public event Action Breached;

		[SerializeField] private Player _player;
		[SerializeField] private float  _breachTime;
		[SerializeField] private float  _pacifyTime;

		public Player Player     => _player;
		public float  BreachTime => _breachTime;
		public float  PacifyTime => _pacifyTime;

		private StateMachine<Window, EntranceTriggers> _stateMachine;

		private void Awake ()
		{
			_stateMachine = BuildStateMachine();
		}

		private void Update ()
		{
			_stateMachine?.Tick(Time.deltaTime);
		}

		private StateMachine<Window, EntranceTriggers> BuildStateMachine ()
		{
			StateMachine<Window, EntranceTriggers> stateMachine = new(this);

			stateMachine.AddState<WindowIdleState>()
			            .AllowTransition<WindowWarningState>(EntranceTriggers.warning)
			            .SetAsInitial();

			stateMachine.AddState<WindowWarningState>()
			            .AllowTransition<WindowIdleState>(EntranceTriggers.idle)
			            .AllowTransition<WindowBreachState>(EntranceTriggers.breach);

			stateMachine.AddState<WindowBreachState>();

			stateMachine.Start();

			return stateMachine;
		}

		public void Breach ()
		{
			Breached?.Invoke();
		}
	}
}
