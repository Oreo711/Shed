using UnityEngine;


namespace Shed.Runtime.Gameplay
{
	public class Clock : MonoBehaviour
	{
		[SerializeField] private Transform _minuteHand;
		[SerializeField] private Transform _hourHand;

		public void SetTime (int hours, int minutes)
		{
			hours   = 12 - hours % 12 + 6;
			minutes = 60 - minutes % 60;

			float minutesRotation = minutes * 6f;
			float hoursRotation   = hours * 30f;

			_minuteHand.localRotation = Quaternion.Euler(0, 0, minutesRotation);
			_hourHand.localRotation   = Quaternion.Euler(0, 0, hoursRotation);
		}
	}
}
