
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;

	private static bool _destroyed;

	public static T instance
	{
		get
		{
			return MonoSingleton<T>.GetInstance();
		}
	}

	public static T GetInstance()
	{
		if (MonoSingleton<T>._instance == null && !MonoSingleton<T>._destroyed)
		{
			System.Type typeFromHandle = typeof(T);
			MonoSingleton<T>._instance = (T)((object)Object.FindObjectOfType(typeFromHandle));
			if (MonoSingleton<T>._instance == null)
			{
				GameObject gameObject = new GameObject(typeof(T).Name);
				MonoSingleton<T>._instance = gameObject.AddComponent<T>();
                GameObject gameObject2 = GameObject.Find("GameManager");
				if (gameObject2 != null)
				{
					gameObject.transform.SetParent(gameObject2.transform);
				}
			}
		}
		return MonoSingleton<T>._instance;
	}

	public static void DestroyInstance()
	{
		if (MonoSingleton<T>._instance != null)
		{
			UnityEngine.Object.Destroy(MonoSingleton<T>._instance.gameObject);
		}
		MonoSingleton<T>._destroyed = true;
		MonoSingleton<T>._instance = (T)((object)null);
	}

	public static void ClearDestroy()
	{
		MonoSingleton<T>.DestroyInstance();
		MonoSingleton<T>._destroyed = false;
	}

	protected virtual void Awake()
	{
		if (MonoSingleton<T>._instance != null && MonoSingleton<T>._instance.gameObject != base.gameObject)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(base.gameObject);
			}
		}
		else if (MonoSingleton<T>._instance == null)
		{
			MonoSingleton<T>._instance = base.GetComponent<T>();
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.Init();
	}

	protected virtual void OnDestroy()
	{
		if (MonoSingleton<T>._instance != null && MonoSingleton<T>._instance.gameObject == base.gameObject)
		{
			MonoSingleton<T>._instance = (T)((object)null);
		}
	}

	public virtual void DestroySelf()
	{
		MonoSingleton<T>._instance = (T)((object)null);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public static bool HasInstance()
	{
		return MonoSingleton<T>._instance != null;
	}

	protected virtual void Init()
	{
	}
}
