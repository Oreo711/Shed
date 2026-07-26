using System.Collections;
using UnityEngine;


namespace Shed.Runtime.Utilities.CoroutineManagement
{
    public interface ICoroutinePerformer
    {
        Coroutine StartCoroutine(IEnumerator coroutineFunction);
        void StopCoroutine(Coroutine coroutine);
    }
}
