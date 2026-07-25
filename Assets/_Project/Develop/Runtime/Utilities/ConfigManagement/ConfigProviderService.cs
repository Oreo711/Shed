using System;
using System.Collections;
using System.Collections.Generic;


namespace _Project.Develop.Runtime.Utilities.ConfigManagement
{
	public class ConfigProviderService
	{
		private readonly Dictionary<Type, object> _configs = new();

		private readonly IConfigsLoader[] _loaders;

		public ConfigProviderService (params IConfigsLoader[] loaders)
		{
			_loaders = loaders;
		}

		public IEnumerator LoadAsync ()
		{
			_configs.Clear();

			foreach (IConfigsLoader loader in _loaders)
			{
				yield return loader.LoadAsync(loadedConfigs => _configs.AddRangeStrict(loadedConfigs));
			}
		}

		public T GetConfig<T> () where T : class
		{
			if (!_configs.ContainsKey(typeof(T)))
				throw new InvalidOperationException($"Not found config by {typeof(T)}");

			return (T)_configs[typeof(T)];
		}
	}
}
