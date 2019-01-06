//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


using UnityEditor;
using UnityEngine;
using System.Collections;


// Only compile if not using Unity iPhone
#if !UNITY_IPHONE || (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
[CustomEditor(typeof(GUIDynamicFont))]
#endif
public class GUIDynamicFontInspector : Editor
{
	// Working vars:
	protected GUIDynamicFont spriteFont;
	protected bool isDirty = false;

	// Begins watching the GUI for changes
	protected void BeginMonitorChanges()
	{
		GUI.changed = false;
	}

	// Determines if something changed
	protected void EndMonitorChanges()
	{
		if (GUI.changed)
			isDirty = true;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		isDirty = false;

		spriteFont = target as GUIDynamicFont;

		GUILayout.BeginVertical();

		//-----------------------------------------
		// Draw our transition stuff:
		//-----------------------------------------
		BeginMonitorChanges();
		DoTransitionStuff();
		EndMonitorChanges();

		GUILayout.Space(10f);

		GUILayout.EndVertical();

		// Set dirty if anything changed:
		if (isDirty)
			EditorUtility.SetDirty(spriteFont);
	}

	// Draws all our transition-related UI stuff:
	void DoTransitionStuff()
	{
		if (FontManager.instance != null)
		{
			spriteFont.guiStyle.font = (Font)EditorGUILayout.ObjectField("Font", spriteFont.guiStyle.font, typeof(Font), false);
			spriteFont.guiStyle.normal.textColor = EditorGUILayout.ColorField("Text color", spriteFont.guiStyle.normal.textColor);
			spriteFont.guiStyle.fontSize = EditorGUILayout.IntField("Font size", spriteFont.guiStyle.fontSize);
			spriteFont.guiStyle.fontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font style", spriteFont.guiStyle.fontStyle);

			//string[] fontNames = FontManager.instance.GetFontNames();
			//int curFontIndex = System.Array.IndexOf(fontNames, spriteText.fontName);
			//int newFontIndex = EditorGUILayout.Popup("Font Name", curFontIndex, fontNames);
			//if (newFontIndex != curFontIndex)
			//{
			//    spriteText.SetFont(newFontIndex != -1 ? fontNames[newFontIndex] : "");
			//}
		}
	}

	//---------------------------------------
	// IGUIHelper interface stuff:
	//---------------------------------------
	public System.Enum EnumField(string label, System.Enum selected)
	{
		return EditorGUILayout.EnumPopup(label, selected);
	}

	public Color ColorField(string label, Color color)
	{
		return EditorGUILayout.ColorField(label, color);
	}

	public Vector3 Vector3Field(string label, Vector3 val)
	{
		EditorGUIUtility.LookLikeControls();
		Vector3 v = EditorGUILayout.Vector3Field(label, val);
		EditorGUIUtility.LookLikeInspector();
		return v;
	}

	public float FloatField(string label, float val)
	{
		return EditorGUILayout.FloatField(label, val);
	}

	public string TextField(string label, string val)
	{
		return EditorGUILayout.TextField(label, val);
	}

	public Object ObjectField(string label, System.Type type, Object obj)
	{
		return EditorGUILayout.ObjectField(label, obj, type, false, GUILayout.Width(200f));
	}
}
