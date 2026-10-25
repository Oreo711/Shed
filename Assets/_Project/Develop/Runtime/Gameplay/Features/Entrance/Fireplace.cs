using System;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using Shed.Runtime.Gameplay.Features.Entrance.States.Fireplace_States;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance
{
	public class Fireplace : BreachSource<Fireplace>, IStateContext
	{
		[SerializeField] private Player _player;
		[SerializeField] private float  _maxFuel;

		private bool  _outOfFuel;
		private float _fuel;

		public float Fuel => _fuel;

		public override event Action<BreachSourceBase> Breached;

		public override void Activate ()
		{
			StateMachine.Fire(EntranceTriggers.Warning);
		}

		public void Awake ()
		{
			StateMachine = BuildStateMachine();
			_fuel        = _maxFuel;
		}

		private void Update ()
		{
			if (!_outOfFuel)
			{
				if (_fuel > 0)
				{
					_fuel -= Time.deltaTime;
				}

				if (_fuel < 0)
				{
					_fuel = 0;
				}

				if (_fuel <= 0)
				{
					_outOfFuel = true;
					Activate();
				}
			}
		}

		public void AddFuel (float fuel)
		{
			if (_fuel + fuel > _maxFuel)
			{
				_fuel = _maxFuel;
			}
			else
			{
				_fuel += fuel;
			}
		}

		private StateMachine<Fireplace, EntranceTriggers> BuildStateMachine ()
		{
			StateMachine<Fireplace, EntranceTriggers> stateMachine = new(this);

			stateMachine.AddState<FireplaceIdleState>()
			            .AllowTransition<FireplaceBurnoutState>(EntranceTriggers.Warning)
			            .SetAsInitial();

			stateMachine.AddState<FireplaceBurnoutState>()
			            .AllowTransition<FireplaceBreachState>(EntranceTriggers.Breach);

			stateMachine.AddState<FireplaceBreachState>()
			            .AllowTransition<FireplaceIdleState>(EntranceTriggers.Idle);

			stateMachine.Start();

			return stateMachine;
		}

		public void Breach ()
		{
			Breached?.Invoke(this);
		}

	}
}
