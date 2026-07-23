using System.Collections;
using UnityEngine;


namespace _Project.Develop.Runtime.Utilities.CoroutineManagement
{
    public interface ICoroutinePerformer
    {
        Coroutine StartCoroutine(IEnumerator coroutineFunction);
        void StopCoroutine(Coroutine coroutine);
    }
}
