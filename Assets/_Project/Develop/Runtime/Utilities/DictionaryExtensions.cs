using System;
using System.Collections.Generic;


namespace _Project.Develop.Runtime.Utilities
{
	public static class DictionaryExtensions
	{
		public static void AddRangeStrict<TKey, TValue> (this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> other)
		{
			foreach (KeyValuePair<TKey, TValue> kvp in other)
			{
				if (dictionary.ContainsKey(kvp.Key))
					throw new ArgumentException($"Key {kvp.Key} already exists in the dictionary.");

				dictionary[kvp.Key] = kvp.Value;
			}
		}
	}
}
