using System;
using System.Collections.Generic;
using _Project.Develop.Runtime.Utilities.AssetManagment;
using UnityEngine;
using Object = UnityEngine.Object;


namespace _Project.Develop.Runtime.UI.Core
{
    public class ViewFactory
    {
        private readonly ResourcesAssetLoader _resourcesAssetsLoader;

        private readonly Dictionary<string, string> _viewIDToResourcesPath = new Dictionary<string, string>()
        {
            {ViewIDs.GameplayScreen, "UI/Gameplay/Gameplay Screen View"}
        };

        public ViewFactory(ResourcesAssetLoader resourcesAssetsLoader)
        {
            _resourcesAssetsLoader = resourcesAssetsLoader;
        }

        public TView Create<TView>(string viewID, Transform parent = null) where TView : MonoBehaviour, IView
        {
            if (_viewIDToResourcesPath.TryGetValue(viewID, out string resourcePath) == false)
                throw new ArgumentException($"Resource path for {typeof(TView)} not set, searched id: {viewID}");

            GameObject prefab   = _resourcesAssetsLoader.Load<GameObject>(resourcePath);
            GameObject instance = Object.Instantiate(prefab, parent);
            TView      view     = instance.GetComponent<TView>();

            if (view == null)
                throw new InvalidOperationException($"Not found {typeof(TView)} component on view instance");

            return view;
        }

        public void Release<TView>(TView view) where TView : MonoBehaviour, IView
        {
            Object.Destroy(view.gameObject);
        }
    }
}
