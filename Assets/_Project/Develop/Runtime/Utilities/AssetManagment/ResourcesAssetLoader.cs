using UnityEngine;


namespace Shed.Runtime.Utilities.AssetManagment
{
    public class ResourcesAssetLoader
    {
        public T Load<T>(string resourcePath) where T : Object
            => Resources.Load<T>(resourcePath);
    }
}
