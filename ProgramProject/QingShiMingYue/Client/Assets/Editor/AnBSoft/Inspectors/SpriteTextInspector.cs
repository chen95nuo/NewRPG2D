//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


// Only compile if not using Unity iPhone
#if !UNITY_IPHONE || (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
[CustomEditor(typeof(SpriteText))]
#endif
public class SpriteTextInspector : Editor
{
	// Working vars:
	protected SpriteText spriteText;
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

		spriteText = (SpriteText)target;

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
			EditorUtility.SetDirty(spriteText);
	}

	// Draws all our transition-related UI stuff:
	void DoTransitionStuff()
	{
		if (FontManager.instance != null)
		{
			string[] fontNames = FontManager.instance.GetFontNames();
			int curFontIndex = System.Array.IndexOf(fontNames, spriteText.fontName);
			int newFontIndex = EditorGUILayout.Popup("Font Name", curFontIndex, fontNames);
			if (newFontIndex != curFontIndex)
			{
				spriteText.SetFont(newFontIndex != -1 ? fontNames[newFontIndex] : "");
			}
		}

		ShowSpriteTextTemplateSetting();
	}

	void ShowSpriteTextTemplateSetting()
	{
		EditorGUILayout.BeginVertical();

		EditorGUILayout.BeginVertical("Toolbar");
		EditorGUILayout.LabelField("Template :");

		List<string> templateNames = UITemplateFormatter.GetTemplateNameList(typeof(SpriteText), true);
		int selectIndex = templateNames.FindIndex(
			delegate(string name)
			{
				return name.Equals(spriteText.templateName);
			});
		selectIndex = EditorGUILayout.Popup("Name", selectIndex, templateNames.ToArray());
		spriteText.templateName = selectIndex > 0 && selectIndex < templateNames.Count ? templateNames[selectIndex] : "";

		EditorGUILayout.EndVertical();

		GUILayout.Space(25f);
		EditorGUILayout.EndVertical();
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
