using System;
using _Project.Develop.Runtime.Gameplay.Features.Entrance.States.Window_States;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Entrance
{
	public class Window : BreachSource, IStateContext
	{
		[SerializeField] private Player _player;
		[SerializeField] private float  _breachTime;
		[SerializeField] private float  _pacifyTime;

		public Player Player     => _player;
		public float  BreachTime => _breachTime;
		public float  PacifyTime => _pacifyTime;

		private StateMachine<Window, EntranceTriggers> _stateMachine;

		public override event Action<IBreachSource> Breached;

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
			            .AllowTransition<WindowWarningState>(EntranceTriggers.Warning)
			            .SetAsInitial();

			stateMachine.AddState<WindowWarningState>()
			            .AllowTransition<WindowIdleState>(EntranceTriggers.Idle)
			            .AllowTransition<WindowBreachState>(EntranceTriggers.Breach);

			stateMachine.AddState<WindowBreachState>()
			            .AllowTransition<WindowIdleState>(EntranceTriggers.Idle);

			stateMachine.Start();

			return stateMachine;
		}

		public override void Activate ()
		{
			_stateMachine.Fire(EntranceTriggers.Warning);
		}

		public override void Reset ()
		{
			_stateMachine.Fire(EntranceTriggers.Idle);
		}

		public void Breach ()
		{
			Breached?.Invoke(this);
		}
	}
}
