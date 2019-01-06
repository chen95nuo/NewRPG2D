using UnityEngine;

public static class ResourcesWrapper
{
	public static T Load<T>(string path) where T : Object
	{
		var t = Resources.Load(path, typeof(T)) as T;
		if (t == null)
			Debug.LogWarning("Missing asset : " + path);

		return t;
	}

	public static Object Load(string path)
	{
		var obj = Resources.Load(path);
		if (obj == null)
			Debug.LogWarning("Missing asset : " + path);

		return obj;
	}

	public static T LoadAssetAtPath<T>(string path) where T : Object
	{
		var t = Resources.LoadAssetAtPath(path, typeof(T)) as T;
		if (t == null)
			Debug.LogWarning("Missing asset : " + path);

		return t;
	}
}
