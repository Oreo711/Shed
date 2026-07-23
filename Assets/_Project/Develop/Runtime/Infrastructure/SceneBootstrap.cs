using System.Collections;
using _Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;


namespace _Project.Develop.Runtime.Infrastructure
{
    public abstract class SceneBootstrap : MonoBehaviour
    {
        public abstract void ProcessRegistrations(DIContainer container);

        public abstract IEnumerator Initialize();

        public abstract void Run();
    }
}
