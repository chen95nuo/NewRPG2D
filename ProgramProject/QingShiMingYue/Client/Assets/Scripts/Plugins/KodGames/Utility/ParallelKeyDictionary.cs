using System;
using System.Collections.Generic;

namespace KodGames.Graphics
{
	// http://stackoverflow.com/questions/146204/duplicate-keys-in-net-dictionaries
	// KeyValuePair http://msdn.microsoft.com/en-us/library/3db765db.aspx
	/// <summary>
	/// Identical-Safe-Key Dictionary Class
	/// </summary>	
	/// <typeparam name="K">Key Type</typeparam>
	/// <typeparam name="V">Value Type</typeparam>
	public class ParallelKeyDictionary<K, V>
	{
		private List<KeyValuePair<K, V>> list;
		public List<V> values;

		public ParallelKeyDictionary()
		{
			list = new List<KeyValuePair<K, V>>();
			values = new List<V>();
		}

		/// <summary>
		/// Add a value with a key. the key doesn't have to be unique in that dictionary, that's the point.
		/// </summary>	
		/// <typeparam name="key">The key the value will be stored at.</typeparam>
		/// <typeparam name="value">The value that is to be stored.</typeparam>
		public void Add(K key, V value) { list.Add(new KeyValuePair<K, V>(key, value)); values.Add(value); }
		/// <summary>
		/// Remove the first occurence of key.
		/// </summary>	
		/// <typeparam name="key">The key to remove.</typeparam>
		public void RemoveFirst(K key)
		{
			for (int i = 0; i < list.Count; i++)
				if (list[i].Key.Equals(key))
				{
					values.RemoveAt(i);
					list.RemoveAt(i);
					return;
				}
		}
		/// <summary>
		/// Remove all occurences of key.
		/// </summary>	
		/// <typeparam name="key">The key to remove.</typeparam>
		public void RemoveAll(K key)
		{
			while (ContainsKey(key))
				RemoveFirst(key);
		}

		/// <summary>
		/// Get the value at the first occurence of key. Return the default value if key isn't found.
		/// </summary>	
		/// <typeparam name="key">This is the key you're looking for.</typeparam>
		/// <returns>The value if the key is found, default otherwise.</returns>
		public V GetFirst(K key)
		{
			for (int i = 0; i < list.Count; i++)
				if (list[i].Key.Equals(key))
					return list[i].Value;

			return default(V);
		}
		/// <summary>
		/// Get the value at the first occurence of key. Return the default value if key isn't found.
		/// </summary>	
		/// <typeparam name="key">This is the key you're looking for.</typeparam>
		/// <returns>The value if the key is found, default otherwise.</returns>
		public V this[K key] { get { return GetFirst(key); } }

		/// <summary>
		/// Get a list of values with that key. The returned list is never null.
		/// </summary>	
		/// <typeparam name="key">This is the key you're looking for.</typeparam>
		/// <returns>A list of values with that key. It is never null.</returns>
		public List<V> GetAll(K key)
		{
			List<V> result = new List<V>();
			for (int i = 0; i < list.Count; i++)
				if (list[i].Key.Equals(key))
					result.Add(list[i].Value);

			return result;
		}
		/// <summary>
		/// Count the number of occurences of that key.
		/// </summary>	
		/// <typeparam name="key">This is the key you're looking for.</typeparam>
		/// <returns>The number of occurences of that key.</returns>
		public int Count(K key) { return GetAll(key).Count; }
		/// <summary>
		/// The number of elements in the dictionary.
		/// </summary>	
		/// <returns>The number of elements in the dictionary.</returns>
		public int Count() { return list.Count; }

		/// <summary>
		/// Get the first value of that key. If it isn't found, return false.
		/// </summary>	
		/// <typeparam name="key">This is the key you're looking for.</typeparam>
		/// <typeparam name="val">The output val.</typeparam>
		/// <returns>Either it found the key or not.</returns>
		public bool TryGetFirstValue(K key, out V val)
		{
			val = GetFirst(key);
			try { return !val.Equals(default(V)); } // If default is null, exception.
			catch { return false; }
		}

		/// <summary>
		/// Check if the dictionary contains at least one such key.
		/// </summary>	
		/// <typeparam name="key">This is the key you're looking for.</typeparam>
		/// <returns>Either it contains the key or not.</returns>
		public bool ContainsKey(K key)
		{
			for (int i = 0; i < list.Count; i++)
				if (list[i].Key.Equals(key))
					return true;

			return false;
		}

		public override string ToString()
		{
			string acc = "";
			for (int i = 0; i < Count(); i++)
				acc += string.Format("[{0}] Key : {1}, Value : {2}\n", i, list[i].Key.ToString(), list[i].Value.ToString());

			return acc;
		}
	}
}
