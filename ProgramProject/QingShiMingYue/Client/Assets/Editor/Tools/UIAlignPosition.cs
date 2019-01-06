#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Simple script that aligns the position of several selected GameObjects with the first selected one.
/// </summary>
public class UIAlignPosition : EditorWindow 
{
	bool alignToX = true;
	bool alignToY = true; 
	bool alignToZ = true;
	string selected = "";
	string alignTo = "";
	
	[MenuItem("Tools/UI/Align position")]
	public static void ShowWindow() 
	{        
		var window = GetWindow(typeof(UIAlignPosition), true);
		window.Show();
	}

	void OnInspectorUpdate() 
	{
		// Call Repaint on OnInspectorUpdate as it repaints the windows
		// less times as if it was OnGUI/Update
		Repaint();
	}    

	void OnGUI() 
	{
		GUILayout.Label("Select various Objects in the Hierarchy view");
		selected = Selection.activeTransform ? Selection.activeTransform.name : "";
		foreach (var t in Selection.transforms)
			if(t.GetInstanceID() != Selection.activeTransform.GetInstanceID())
				alignTo += t.name + " ";    
		EditorGUILayout.LabelField("Align: ", alignTo);
		alignTo = "";
		EditorGUILayout.LabelField("With: ", selected);
		
		alignToX = EditorGUILayout.Toggle("X", alignToX);
		alignToY = EditorGUILayout.Toggle("Y", alignToY);
		alignToZ = EditorGUILayout.Toggle("Z", alignToZ);
		if(GUILayout.Button("Align"))
			Align();
	}

	void Align() 
	{
		if(selected == "" || alignTo == "")
			Debug.LogError("No objects selected to align");
		foreach (var t in Selection.transforms) {
			var alignementPosition = Selection.activeTransform.position;
			Vector3 newPosition;
			newPosition.x = alignToX ? alignementPosition.x : t.position.x;
			newPosition.y = alignToY ? alignementPosition.y : t.position.y;
			newPosition.z = alignToZ ? alignementPosition.z : t.position.z;
			t.position = newPosition;    
		}
	}
}
#endif