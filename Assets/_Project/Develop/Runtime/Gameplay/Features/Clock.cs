using UnityEngine;


namespace Shed.Runtime.Gameplay.Features
{
	public class Clock : MonoBehaviour
	{
		[SerializeField] private AudioSource _clockSound;

		public void StopClockSound ()
		{
			_clockSound.Stop();
		}
	}
}
