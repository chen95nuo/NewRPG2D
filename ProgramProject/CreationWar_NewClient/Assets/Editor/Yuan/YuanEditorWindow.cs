//----------------------------------------------
//           ZealmTools
// Copyright © 2010-2014 Zealm
// Copyright © 2010-2014 FernYuan
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;

public class YuanEditorWindow : EditorWindow
{

	/// <summary>
	/// 绘制具体行为
	/// </summary>
	protected virtual void DarwInfo()
	{
		 
	}

	Vector2 mPos = Vector2.zero;
	void OnGUI()
	{
		mPos = GUILayout.BeginScrollView (mPos);
		{
			DarwInfo ();
		}
		GUILayout.EndScrollView ();
		
		GUI.color=Color.green;
		GUILayout.Label ("Copyright © 2010-2014 Zealm\nCopyright © 2010-2014 FernYuan");
	}

}
