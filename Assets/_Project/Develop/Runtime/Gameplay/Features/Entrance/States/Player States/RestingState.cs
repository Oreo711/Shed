using DG.Tweening;
using Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine;
using UnityEngine;


namespace Shed.Runtime.Gameplay.Features.Entrance.States.Player_States
{
	public class RestingState : IState<Player, PlayerStateTriggers>
	{
		public void OnEnter (IStateMachine<Player, PlayerStateTriggers> machine)
		{
			Player player = machine.Context;
			player.DeactivateControls();

			player.CanUseBed             = false;
			player.CanStopUsingBed       = true;
			player.IsResting             = true;
			player.Rigidbody.isKinematic = true;

			player.transform.DOMove(player.Bed.LyingPoint.transform.position, 1.25f)
			      .SetEase(Ease.InOutQuad);
			player.transform.DORotateQuaternion(player.Bed.LyingPoint.transform.rotation, 1.25f)
			      .SetEase(Ease.InOutQuad);
		}
		public void OnExit (IStateMachine<Player, PlayerStateTriggers> machine)
		{
			Player player = machine.Context;

			player.CanStopUsingBed = false;
			player.IsResting       = false;

			player.transform.DOMove(player.Bed.GetupPoint.transform.position, 1.25f)
			      .SetEase(Ease.InOutQuad);
			player.transform.DORotateQuaternion(player.Bed.GetupPoint.transform.rotation, 1.25f)
			      .SetEase(Ease.InOutQuad);

		}
		public void Tick (IStateMachine<Player, PlayerStateTriggers> machine, float deltaTime)
		{
			Player player = machine.Context;

			player.RegainEnergy(deltaTime * 6);

			if (Input.GetKeyDown(KeyCode.Space))
			{
				machine.Fire(PlayerStateTriggers.active);
			}
		}
	}
}
