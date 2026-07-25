using System;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features
{
	public class Gun : MonoBehaviour
	{
		[SerializeField] private int _bullets;
		[SerializeField] private AudioSource _gunshotSound;

		public int Bullets => _bullets;

		public event Action Shot;

		private void Shoot ()
		{
			if (_bullets <= 0)
				return;

			Shot?.Invoke();
			_bullets--;
			_gunshotSound.Play();
		}

		private void Update ()
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				Shoot();
			}
		}
	}
}
