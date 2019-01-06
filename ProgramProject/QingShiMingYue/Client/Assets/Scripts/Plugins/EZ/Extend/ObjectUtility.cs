using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;

public static class ObjectUtility
{
	public static GameObject GetChild(GameObject root, string name)
	{
		return root.transform.Find(name).gameObject;
	}

	public static T GetComponentInChild<T>(GameObject root, string name) where T : MonoBehaviour
	{
		Transform child = root.transform.Find(name);
		return child != null ? child.GetComponent<T>() : null;
	}

	public static Transform FindChildObject(Transform parent, string name, bool excludeRoot)
	{
		if (name == "")
			return null;

		foreach (Transform child in parent.GetComponentsInChildren(typeof(Transform), true))
		{
			if (excludeRoot && child == parent)
				continue;

			if (String.Compare(child.gameObject.name, name, true) == 0)
				return child;
		}

		return null;
	}

	public static Transform FindChildObject(Transform parent, string name)
	{
		return FindChildObject(parent, name, false);
	}

	public static Transform FindChildObject(GameObject parent, string name, bool excludeRoot)
	{
		return FindChildObject(parent.transform, name, excludeRoot);
	}

	public static Transform FindChildObject(GameObject parent, string name)
	{
		return FindChildObject(parent.transform, name, false);
	}

	public static List<Transform> FindChildObjects(Transform parent, string name, bool excludeRoot)
	{
		var result = new List<Transform>();

		if (name == "")
			return result;

		foreach (Transform child in parent.GetComponentsInChildren(typeof(Transform), true))
		{
			if (excludeRoot && child == parent)
				continue;

			if (String.Compare(child.gameObject.name, name, true) == 0)
				result.Add(child);
		}

		return result;
	}

	public static List<Transform> FindChildObjects(Transform parent, string name)
	{
		return FindChildObjects(parent, name, false);
	}

	public static List<Transform> FindChildObjects(GameObject parent, string name, bool excludeRoot)
	{
		return FindChildObjects(parent.transform, name, false);
	}

	public static List<Transform> FindChildObjects(GameObject parent, string name)
	{
		return FindChildObjects(parent.transform, name, false);
	}

	public static T FindComponentInChildren<T>(GameObject parent, string gameObjectName, uint depth) where T : Component
	{
		foreach (var component in parent.GetComponentsInChildren<T>(true))
		{
			if (component.gameObject.name.Equals(gameObjectName) &&
				GetTransformDepth(component.gameObject.transform, parent.gameObject.transform) == depth)
				return component;
		}

		return null;
	}

	public static GameObject CreateChildGameObject(GameObject root, string name)
	{
		GameObject go = new GameObject();
		go.layer = root.layer;
		go.transform.parent = root.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		go.name = name;

		return go;
	}

	public static List<Transform> GetChildObjectsByDepth(Transform parent, uint depth)
	{
		List<Transform> rsList = new List<Transform>();

		uint prD = GetTransformDepth(parent.transform);

		foreach (Transform child in parent.GetComponentsInChildren(typeof(Transform), true))
		{
			if (GetTransformDepth(child) - prD == depth)
				rsList.Add(child);
		}

		return rsList;
	}

	public static uint GetTransformDepth(Transform trans)
	{
		uint depth = 0;

		Transform parent = trans.parent;

		while (parent != null)
		{
			depth++;
			parent = parent.parent;
		}

		return depth;
	}

	public static uint GetTransformDepth(Transform trans, Transform root)
	{
		uint depth = 0;

		Transform parent = trans;

		while (parent != root)
		{
			depth++;
			parent = parent.parent;
		}

		return depth;
	}

	public static string GetTransRootPath(Transform trans)
	{
		StringBuilder bld = new StringBuilder(trans.name);

		Transform parent = trans.parent;

		while (parent != null)
		{
			bld.Insert(0, "/");
			bld.Insert(0, parent.name);

			parent = parent.parent;
		}

		return bld.ToString();
	}

	private static Vector3 KeepSign(Vector3 a, Vector3 b)
	{
		if (a.x >= 0)
			b.x = Mathf.Abs(b.x);
		else
			b.x = -b.x;

		if (a.y >= 0)
			b.y = Mathf.Abs(b.y);
		else
			b.y = -b.y;

		if (a.z >= 0)
			b.z = Mathf.Abs(b.z);
		else
			b.z = -b.z;

		return b;
	}

	public static void AttachToParent(GameObject parent, GameObject child, Vector3 localPos, Quaternion localRot)
	{
		AttachToParent(parent != null ? parent.transform : null, child.transform, localPos, localRot);
	}

	public static void AttachToParent(Transform parent, Transform child, Vector3 localPos, Quaternion localRot)
	{
		child.parent = parent;
		child.localPosition = localPos;
		child.localRotation = localRot;
	}

	public static void AttachToParentAndResetLocalTrans(GameObject parent, GameObject child)
	{
		AttachToParentAndResetLocalTrans(parent != null ? parent.transform : null, child.transform);
	}

	public static void AttachToParentAndResetLocalTrans(Transform parent, Transform child)
	{
		child.parent = parent;
		child.localPosition = Vector3.zero;
		child.localRotation = Quaternion.identity;
		child.localScale = Vector3.one;
	}

	public static void AttachToParentAndResetLocalPosAndRotation(GameObject parent, GameObject child)
	{
		AttachToParentAndResetLocalPosAndRotation(parent != null ? parent.transform : null, child.transform);
	}

	public static void AttachToParentAndResetLocalPosAndRotation(Transform parent, Transform child)
	{
		child.parent = parent;
		child.localPosition = Vector3.zero;
		child.localRotation = Quaternion.identity;
	}

	public static void AttachToParentAndKeepWorldTrans(GameObject parent, GameObject child)
	{
		AttachToParentAndKeepWorldTrans(parent != null ? parent.transform : null, child.transform);
	}

	public static void AttachToParentAndKeepWorldTrans(Transform parent, Transform child)
	{
		Vector3 oldPos = child.position;
		Quaternion oldRot = child.rotation;
		child.parent = parent;
		child.position = oldPos;
		child.rotation = oldRot;
	}

	public static void AttachToParentAndKeepLocalTrans(GameObject parent, GameObject child)
	{
		AttachToParentAndKeepLocalTrans(parent != null ? parent.transform : null, child.transform);
	}

	public static void AttachToParentAndKeepLocalTrans(Transform parent, Transform child)
	{
		Vector3 oldLPos = child.localPosition;
		Quaternion oldLRot = child.localRotation;
		Vector3 oldScale = child.localScale;
		child.parent = parent;
		child.localPosition = oldLPos;
		child.localRotation = oldLRot;
		child.localScale = oldScale;
	}

	public static void DestroyChildObjects(GameObject parent)
	{
		Component[] children = parent.GetComponentsInChildren(typeof(Transform), true);

		foreach (Component child in children)
		{
			if (child.gameObject == parent)
				continue;

			GameObject.Destroy(child.gameObject);
		}
	}

	public static void SetObjectLayer(GameObject obj, int layer)
	{
		foreach (Component child in obj.GetComponentsInChildren(typeof(Transform), true))
		{
			child.gameObject.layer = layer;
		}
	}

	public static void UnifyWorldTrans(GameObject src, GameObject dst)
	{
		UnifyWorldTrans(src.transform, dst.transform);
	}

	public static void UnifyWorldTrans(Transform src, Transform dst)
	{
		dst.position = src.position;
		dst.rotation = src.rotation;
	}

	public static Bounds GetMaxBounds(GameObject obj)
	{
		Bounds bd = new Bounds(obj.transform.position, Vector3.zero);

		Vector3 min = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
		Vector3 max = new Vector3(-Mathf.Infinity, -Mathf.Infinity, -Mathf.Infinity);
		bool setBd = false;

		Component[] rnds = obj.GetComponentsInChildren(typeof(Renderer), true);

		foreach (Renderer rnd in rnds)
		{
			// Skip particle bounds.
			if (rnd is ParticleRenderer)
				continue;

			Bounds b = rnd.bounds;

			if (b.min.x < min.x)
				min.x = b.min.x;

			if (b.min.y < min.y)
				min.y = b.min.y;

			if (b.min.z < min.z)
				min.z = b.min.z;

			if (b.max.x > max.x)
				max.x = b.max.x;

			if (b.max.y > max.y)
				max.y = b.max.y;

			if (b.max.z > max.z)
				max.z = b.max.z;

			setBd = true;
		}

		if (setBd)
			bd.SetMinMax(min, max);

		return bd;
	}
}
