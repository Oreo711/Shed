using System.Collections;
using UnityEngine;


namespace Shed.Runtime.Utilities.CoroutineManagement
{
    public class CoroutinePerformer : MonoBehaviour, ICoroutinePerformer
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public Coroutine StartCoroutine(IEnumerator coroutineFunction)
            => base.StartCoroutine(coroutineFunction);

        public void StopCoroutine(Coroutine coroutine)
            => base.StopCoroutine(coroutine);
    }
}
