using UnityEngine;


namespace Shed.Runtime.Gameplay.Features
{
	public class Gun : MonoBehaviour
	{
		[SerializeField] private int         _bullets;
		[SerializeField] private AudioSource _gunshotSound;

		public int Bullets => _bullets;

		public bool TryPerformShot ()
		{
			if (_bullets <= 0)
				return false;

			_bullets--;
			_gunshotSound.Play();

			return true;
		}
	}
}
