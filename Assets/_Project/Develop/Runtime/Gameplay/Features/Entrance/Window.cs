using System;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using Shed.Runtime.Gameplay.Features.Entrance.States.Window_States;
using Shed.Runtime.Gameplay.Hints;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance
{
	public class Window : BreachSource<Window>, IStateContext
	{
		[SerializeField] private Player           _player;
		[SerializeField] private float            _breachTime;
		[SerializeField] private float            _pacifyTime;
		[SerializeField] private BreachSourceHint _breachSourceHint;

		public Player Player     => _player;
		public float  BreachTime => _breachTime;
		public float  PacifyTime => _pacifyTime;

		public override event Action<BreachSourceBase> Breached;

		private void Awake ()
		{
			StateMachine = BuildStateMachine();
		}

		private void Update ()
		{
			StateMachine.Tick(Time.deltaTime);
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
			StateMachine.Fire(EntranceTriggers.Warning);
		}

		public void ShowHint ()
		{
			_breachSourceHint.Show();
		}

		public void HideHint ()
		{
			_breachSourceHint.Hide();
		}

		public void Breach ()
		{
			Breached?.Invoke(this);
		}
	}
}
