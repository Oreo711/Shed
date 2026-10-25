using UnityEngine;


namespace Shed.Runtime.Gameplay.Hints
{
	public class FireplaceBreachSourceHint : BreachSourceHint
	{
		[SerializeField] private GameObject _fire;

		public override void Show ()
		{
			_fire.SetActive(false);
		}

		public override void Hide ()
		{
			_fire.SetActive(true);
		}
	}
}
