using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[AddComponentMenu("Tools")]
public class TowerScenePathEditor
{
	[MenuItem("Tools/TowerScene/SetUnitPathNodes")]
	public static void setTowerUnitPathNode()
	{
		if (Selection.activeGameObject == null)
			return;

		Transform[] nodes = Selection.activeGameObject.GetComponentsInChildren<Transform>();
		TowerUnit unit = Selection.activeGameObject.GetComponent<TowerUnit>();
		unit.pathNodes = new List<Transform>();
		unit.pathNodes.AddRange(nodes);

		int indexToRemove = -1;
		for (int i = 0; i < unit.pathNodes.Count; i++)
		{
			var temp = unit.pathNodes[i];
			if (temp.Equals(Selection.activeGameObject.transform))
			{
				indexToRemove = i;
				break;
			}
		}

		if (indexToRemove >= 0)
			unit.pathNodes.RemoveAt(indexToRemove);
	}
}
