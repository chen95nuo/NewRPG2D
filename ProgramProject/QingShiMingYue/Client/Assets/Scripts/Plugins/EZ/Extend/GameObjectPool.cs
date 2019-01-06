using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// A Pool for scroll list item, alloc and release via this class can greatly improve the performance 
/// when initializing a scroll list with a lot of items.
/// </summary>
public class GameObjectPool : MonoBehaviour
{
	public int initializeCount;
	public GameObject sceneObject;

	private List<GameObject> itemContainer = new List<GameObject>();

	protected virtual void Awake()
	{
		// Add to pool manager
		GameObjectPoolManager.Singleton.AddPool(this);

		CreatePool(sceneObject, initializeCount);
	}

	void OnEnable()
	{
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
		gameObject.SetActive(false);
#else
		gameObject.SetActiveRecursively(false);
#endif
	}

	void OnDestroy()
	{
		// Delete all pooled item
		DestroyPooledItems();

		// Remove from pool manager
		GameObjectPoolManager.Singleton.RemovePool(this);
	}

	public int Size
	{
		get { return itemContainer.Count; }
	}

	private void CreatePool(GameObject go, int count)
	{
		if (go == null)
			return;

		// Make scene object as child
		sceneObject = go;

		// 为了实例化之后默认是inactve, 将模板强制为false, 
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
		sceneObject.SetActive(false);
#else
		sceneObject.SetActiveRecursively(false);
#endif
		ObjectUtility.AttachToParentAndResetLocalTrans(this.gameObject, go);

		// Pre create items
		List<GameObject> objs = new List<GameObject>();
		for (int i = itemContainer.Count; i < count; ++i)
			objs.Add(AllocateItem(false));

		foreach (GameObject o in objs)
			ReleaseItem(o);
	}

	public void DestroyPooledItems()
	{
		foreach (var item in itemContainer)
			Destroy(item);

		itemContainer.Clear();
	}

	public GameObject AllocateItem()
	{
		return AllocateItem(true);
	}

	public GameObject AllocateItem(bool autoActive)
	{
		GameObject go = null;
		if (itemContainer.Count != 0)
		{
			go = itemContainer[itemContainer.Count - 1];
			itemContainer.RemoveAt(itemContainer.Count - 1);
		}
		else
			go = GameObject.Instantiate(sceneObject) as GameObject;

		if (autoActive)
		{
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
			go.SetActive(true);
#else
			go.SetActiveRecursively(true);
#endif
			FXController fXController = go.GetComponent<FXController>();
			if (fXController != null)
				fXController.Start();
		}

		return go;
	}

	public void ReleaseItem(GameObject go)
	{
		go.transform.parent = this.gameObject.transform;

#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
		go.SetActive(false);
#else
		go.SetActiveRecursively(false);
#endif
		itemContainer.Add(go);
	}

	public void ClearScrollListAndReleaseItemsToPool(UIScrollList scrollList)
	{
		// Save all items in scroll list
		List<GameObject> goList = new List<GameObject>();
		for (int i = 0; i < scrollList.Count; ++i)
		{
			UIListItemContainer item = scrollList.GetItem(i) as UIListItemContainer;
			goList.Add(item.gameObject);
		}

		// Clear scroll list
		scrollList.ClearList(false);

		// Release items to pool
		foreach (var go in goList)
			ReleaseItem(go);
	}

	public void ClearAndReleaseItemsToPoolByType(UIScrollList scrollList, string name)
	{
		// Save all items in scroll list
		List<GameObject> goList = new List<GameObject>();
		for (int i = 0; i < scrollList.Count; ++i)
		{
			UIListItemContainer item = scrollList.GetItem(i) as UIListItemContainer;
			if (item.name == name)
			{
				goList.Add(item.gameObject);
				scrollList.RemoveItem(i, false);
				i--;
			}
		}

		// Release items to pool
		foreach (var go in goList)
			ReleaseItem(go);
	}
}

public class GameObjectPoolManager
{
	private static GameObjectPoolManager singleton;
	public static GameObjectPoolManager Singleton
	{
		get
		{
			if (singleton == null)
				singleton = new GameObjectPoolManager();

			return singleton;
		}
	}

	public void AddPool(GameObjectPool pool)
	{
		if (poolList.Contains(pool))
			return;

		poolList.AddLast(pool);
	}

	public void RemovePool(GameObjectPool pool)
	{
		poolList.Remove(pool);
	}

	public void ActivePool(GameObjectPool pool)
	{
		if (poolList.First != null && poolList.First.Value == pool)
			return;

		LinkedListNode<GameObjectPool> node = poolList.Find(pool);
		poolList.Remove(node);
		poolList.AddFirst(node);
	}

	public void FreeMemory()
	{
		int totalPoolSize = 0;
		foreach (var node in poolList)
		{
			totalPoolSize += node.Size;
			node.DestroyPooledItems();
		}
	}

	private LinkedList<GameObjectPool> poolList = new LinkedList<GameObjectPool>();
}