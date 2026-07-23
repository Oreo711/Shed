using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Light                 _pointLight;
        [SerializeField] private Light                 _spotLight;
        [SerializeField] private FirstPersonController _controller;
        [SerializeField] private Camera                _camera;

        public bool   IsWalking         => _controller.IsWalking;
        public bool   IsUsingFlashlight => _spotLight.gameObject.activeSelf;
        public Camera Camera            => _camera;

        private void Awake ()
        {
            _pointLight.gameObject.SetActive(false);
            _spotLight.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _spotLight.gameObject.SetActive(!_spotLight.gameObject.activeSelf);
                _pointLight.gameObject.SetActive(!_pointLight.gameObject.activeSelf);
            }
        }
    }
}
