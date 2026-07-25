using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure.Gameplay_States
{
	public class GunshotState : IState<GameplayBootstrap, GameplayTriggers>
	{
		private const float TimeToShoot = 0.8f;
		private       float _remainingTimeToShoot;
		private       bool  _shot;

		public void OnEnter (IStateMachine<GameplayBootstrap, GameplayTriggers> machine)
		{
			_remainingTimeToShoot = TimeToShoot;

			Debug.Log("PRESS X TO SHOOT!");

			machine.Context.Player.Gun.Shot += OnShot;
		}

		public void OnExit (IStateMachine<GameplayBootstrap, GameplayTriggers> machine)
		{
			_remainingTimeToShoot           =  TimeToShoot;
			_shot                           =  false;
			machine.Context.Player.Gun.Shot -= OnShot;
		}

		public void Tick (IStateMachine<GameplayBootstrap, GameplayTriggers> machine, float deltaTime)
		{
			_remainingTimeToShoot -= deltaTime;

			if (_shot)
			{
				machine.Fire(GameplayTriggers.Gameplay);
				machine.Context.OnSuccessfulGunshot();
			}

			if (_remainingTimeToShoot <= 0)
			{
				machine.Context.HandleSessionEnded();
				machine.Fire(GameplayTriggers.Loss);
			}
		}

		private void OnShot ()
		{
			_shot = true;
		}
	}
}
