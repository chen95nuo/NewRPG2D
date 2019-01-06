using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(BattleScene))]
public class BattleSceneEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Place Camera"))
		{
			BattleScene.GetBattleScene().PlaceCamera();
		}
	}
}