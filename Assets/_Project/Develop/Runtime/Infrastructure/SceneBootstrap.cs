using System.Collections;
using Shed.Runtime.Infrastructure.DI;
using UnityEngine;


namespace Shed.Runtime.Infrastructure
{
    public abstract class SceneBootstrap : MonoBehaviour
    {
        public abstract void ProcessRegistrations(DIContainer container);

        public abstract IEnumerator Initialize();

        public abstract void Run();
    }
}
