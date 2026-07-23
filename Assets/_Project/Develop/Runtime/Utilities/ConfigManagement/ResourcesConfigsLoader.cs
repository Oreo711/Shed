using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Develop.Runtime.Utilities.AssetManagment;
using UnityEngine;


namespace _Project.Develop.Runtime.Utilities.ConfigManagement
{
    public class ResourcesConfigsLoader : IConfigsLoader
    {
        private readonly ResourcesAssetLoader _resources;

        private readonly Dictionary<Type, string> _configsResourcesPaths = new()
        {

        };

        public ResourcesConfigsLoader(ResourcesAssetLoader resources)
        {
            _resources = resources;
        }

        public IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded)
        {
            Dictionary<Type, object> loadedConfigs = new();

            foreach (KeyValuePair<Type, string> configResourcesPath in _configsResourcesPaths)
            {
                ScriptableObject config = _resources.Load<ScriptableObject>(configResourcesPath.Value);
                loadedConfigs.Add(configResourcesPath.Key, config);
                yield return null;
            }

            onConfigsLoaded?.Invoke(loadedConfigs);
        }
    }
}
