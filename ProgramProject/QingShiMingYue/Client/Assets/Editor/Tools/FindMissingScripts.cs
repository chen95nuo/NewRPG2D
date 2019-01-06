using UnityEngine;
using UnityEditor;

public class FindMissingScripts : EditorWindow
{
	[MenuItem("Tools/FindMissingScripts")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FindMissingScripts));
	}

	public void OnGUI()
	{
		if (GUILayout.Button("Find Missing Scripts in selected objects"))
			FindInSelected(false);

		if (GUILayout.Button("Find Missing Scripts in selected objects recursively"))
			FindInSelected(true);
	}

	private static void FindInSelected(bool recursively)
	{
		if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
		{
			EditorUtility.DisplayDialog("FindMissingScripts", "No selection", "OK");
			return;
		}

		int go_count = 0;
		int components_count = 0;
		int missing_count = 0;

		foreach (GameObject g in Selection.gameObjects)
			FindInGO(g, recursively, ref go_count, ref components_count, ref missing_count);

		Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
	}

	private static void FindInGO(GameObject g, bool recursively, ref int go_count, ref int components_count, ref int missing_count)
	{
		go_count++;

		Component[] components = g.GetComponents<Component>();
		for (int i = 0; i < components.Length; i++)
		{
			components_count++;
			if (components[i] == null)
			{
				missing_count++;
				Debug.Log(g.name + " has an empty script attached in position: " + i);
			}
		}

		// Now recurse through each child GO (if there are any):
		if (recursively)
		{
			foreach (Transform childT in g.transform)
			{
				//Debug.Log("Searching " + childT.name  + " " );
				FindInGO(childT.gameObject, recursively, ref go_count, ref components_count, ref missing_count);
			}
		}
	}
}