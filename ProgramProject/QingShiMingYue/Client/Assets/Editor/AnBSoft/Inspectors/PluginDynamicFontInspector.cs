//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------


using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Only compile if not using Unity iPhone
#if !UNITY_IPHONE || (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
[CustomEditor(typeof(PluginDynamicFont))]
#endif
public class PluginDynamicFontInspector : Editor
{
	// Working vars:
	protected PluginDynamicFont spriteFont;
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

		spriteFont = target as PluginDynamicFont;

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
		EditorGUILayout.Separator();
		SelectFont(FontPlugin.SupportedPlatform.IOS, "IOS", ref spriteFont.iosFont);
		spriteFont.fallbackIosFont.valid = EditorGUILayout.Toggle("Use fallback font", spriteFont.fallbackIosFont.valid);
		if (spriteFont.fallbackIosFont.valid)
			SelectFont(FontPlugin.SupportedPlatform.IOS, "IOS", ref spriteFont.fallbackIosFont);

		EditorGUILayout.Separator();
		SelectFont(FontPlugin.SupportedPlatform.ANDROID, "Android", ref spriteFont.androidFont);
		spriteFont.fallbackAndroidFont.valid = EditorGUILayout.Toggle("Use fallback font", spriteFont.fallbackAndroidFont.valid);
		if (spriteFont.fallbackAndroidFont.valid)
			SelectFont(FontPlugin.SupportedPlatform.ANDROID, "Android", ref spriteFont.fallbackAndroidFont);
		
		EditorGUILayout.Separator();
		spriteFont.fontColor = EditorGUILayout.ColorField("Font color", spriteFont.fontColor);
		spriteFont.bold = EditorGUILayout.Toggle("Bold", spriteFont.bold);
		spriteFont.italic = EditorGUILayout.Toggle("Italic", spriteFont.italic);
		spriteFont.outline = EditorGUILayout.Toggle("Outline", spriteFont.outline);
		if (spriteFont.outline)
		{
			spriteFont.outlineWidth = Mathf.Max(EditorGUILayout.IntField("Outline width", spriteFont.outlineWidth), 1);
			spriteFont.outlineColor = EditorGUILayout.ColorField("Outline color", spriteFont.outlineColor);
		}
	}

	static void SelectFont(FontPlugin.SupportedPlatform platform, string platformName, ref FontSetting fontSetting)
	{
		// Font name and face index.
		string[] faceNameList = new string[FontPlugin.supportedFontList[platform].Count];
		FontPlugin.supportedFontList[platform].Keys.CopyTo(faceNameList, 0);

		string curFaceName = FontPlugin.GetSupportedFontName(platform, fontSetting.fileName, fontSetting.familyName, fontSetting.styleName);
		int curFaceIndex = new List<string>(faceNameList).IndexOf(curFaceName);
		int newFaceIndex = EditorGUILayout.Popup(platformName + " Font", curFaceIndex, faceNameList);
		if (newFaceIndex != curFaceIndex)
		{
			if (newFaceIndex != -1)
			{
				fontSetting.fileName = FontPlugin.supportedFontList[platform][faceNameList[newFaceIndex]].fileName;
				fontSetting.familyName = FontPlugin.supportedFontList[platform][faceNameList[newFaceIndex]].familyName;
				fontSetting.styleName = FontPlugin.supportedFontList[platform][faceNameList[newFaceIndex]].styleName;
			}
			else
			{
				fontSetting.fileName = "";
				fontSetting.familyName = "";
				fontSetting.styleName = "";
			}
		}
		
		// Font size
		fontSetting.fontSize = EditorGUILayout.IntField(platformName + " Size", fontSetting.fontSize);

		// Character size
		fontSetting.characterSize = EditorGUILayout.FloatField(platformName + " Character Size", fontSetting.characterSize);
		fontSetting.characterSize = fontSetting.characterSize != 0 ? fontSetting.characterSize : 0;
		fontSetting.lineHeightFactor = EditorGUILayout.FloatField(platformName + " Line Height Factor", fontSetting.lineHeightFactor);
		fontSetting.lineHeightFactor = fontSetting.lineHeightFactor != 0 ? fontSetting.lineHeightFactor : 0;
		
		// Offset
		fontSetting.baseOffsetY = EditorGUILayout.IntField(platformName + " Base Offset Y", fontSetting.baseOffsetY);
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
