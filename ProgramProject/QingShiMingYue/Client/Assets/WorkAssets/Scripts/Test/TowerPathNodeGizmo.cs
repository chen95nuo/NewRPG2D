using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TowerPathNodeGizmo : MonoBehaviour
{
	public int color;

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Transform[] nodes = CachedTransform.GetComponentsInChildren<Transform>();

		switch(color)
		{
			case 1: Gizmos.color = Color.blue; break;
			case 2: Gizmos.color = Color.red; break;
			case 3: Gizmos.color = Color.green; break;
			default: Gizmos.color = Color.white; break;
		}		
		for (int i = 0; i < nodes.Length; i++)
			Gizmos.DrawSphere(nodes[i].position, 0.4f);
	}
#endif
}