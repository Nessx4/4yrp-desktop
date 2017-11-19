/* Credit for this code:
 * https://answers.unity.com/questions/460727/how-to-serialize-dictionary-with-unity-serializati.html
 * 
 * Dictionaries are not natively serializable by Unity, so this is a workaround
 * to ensure they are. Note: both the key and value data types must themselves 
 * be serializable.
 * 
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField]
	private List<TKey> keys = new List<TKey>();

	[SerializeField]
	private List<TValue> values = new List<TValue>();

	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();
		foreach (KeyValuePair<TKey, TValue> pair in this)
		{
			keys.Add(pair.Key);
			values.Add(pair.Value);
		}
	}

	public void OnAfterDeserialize()
	{
		this.Clear();

		if (keys.Count != values.Count)
			throw new System.Exception(string.Format("There are {0} keys and {1} "
				+ "values after deserialization. "
				+ "Make sure that both key and value types are serializable."));

		for (int i = 0; i < keys.Count; i++)
			this.Add(keys[i], values[i]);
	}
}
