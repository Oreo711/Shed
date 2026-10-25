using UnityEngine;


namespace Shed.Runtime.Gameplay.Features
{
	public class Bed : MonoBehaviour
	{
		[SerializeField] private Transform _lyingPoint;
		[SerializeField] private Transform _getupPoint;

		public Transform LyingPoint => _lyingPoint;
		public Transform GetupPoint => _getupPoint;
	}
}
