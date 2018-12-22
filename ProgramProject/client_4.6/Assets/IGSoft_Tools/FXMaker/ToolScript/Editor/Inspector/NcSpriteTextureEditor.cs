// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

[CustomEditor(typeof(NcSpriteTexture))]

public class NcSpriteTextureEditor : FXMakerEditor
{
	// Attribute ------------------------------------------------------------------------
	protected	NcSpriteTexture		m_Sel;
	protected	FxmPopupManager		m_FxmPopupManager;

	// Property -------------------------------------------------------------------------
	// Event Function -------------------------------------------------------------------
    void OnEnable()
    {
 		m_Sel = target as NcSpriteTexture;
 		m_UndoManager	= new FXMakerUndoManager(m_Sel, "NcSpriteTexture");
   }

    void OnDisable()
    {
		if (m_FxmPopupManager != null && m_FxmPopupManager.IsShowByInspector())
			m_FxmPopupManager.CloseNcPrefabPopup();
    }

	public override void OnInspectorGUI()
	{
		AddScriptNameField(m_Sel);
		m_UndoManager.CheckUndo();

		Rect	rect;

		m_FxmPopupManager = GetFxmPopupManager();

		// --------------------------------------------------------------
		bool bClickButton = false;
		EditorGUI.BeginChangeCheck();
		{
//			DrawDefaultInspector();
			m_Sel.m_fUserTag = EditorGUILayout.FloatField(GetCommonContent("m_fUserTag"), m_Sel.m_fUserTag);

			m_Sel.m_NcSpriteFactoryPrefab	= (GameObject)EditorGUILayout.ObjectField(GetHelpContent("m_NcSpriteFactoryPrefab"), m_Sel.m_NcSpriteFactoryPrefab, typeof(GameObject), false, null);
			// --------------------------------------------------------------
			EditorGUILayout.Space();
			rect = EditorGUILayout.BeginHorizontal(GUILayout.Height(m_fButtonHeight));
			{
				if (FXMakerLayout.GUIButton(FXMakerLayout.GetInnerHorizontalRect(rect, 2, 0, 1), GetHelpContent("Select SpriteFactory"), (m_FxmPopupManager != null)))
					m_FxmPopupManager.ShowSelectPrefabPopup(m_Sel, true, 0);
				if (FXMakerLayout.GUIButton(FXMakerLayout.GetInnerHorizontalRect(rect, 2, 1, 1), GetHelpContent("Clear SpriteFactory"), (m_Sel.m_NcSpriteFactoryPrefab != null)))
				{
					bClickButton = true;
					m_Sel.m_NcSpriteFactoryPrefab = null;
				}
				GUILayout.Label("");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			// --------------------------------------------------------------

			NcSpriteFactory ncSpriteFactory	= (m_Sel.m_NcSpriteFactoryPrefab == null ? null : m_Sel.m_NcSpriteFactoryPrefab.GetComponent<NcSpriteFactory>());
			if (ncSpriteFactory != null)
			{
				int nSelIndex	= EditorGUILayout.IntSlider(GetHelpContent("m_nSpriteFactoryIndex")	, m_Sel.m_nSpriteFactoryIndex, 0, ncSpriteFactory.GetSpriteNodeCount()-1);
				if (m_Sel.m_nSpriteFactoryIndex != nSelIndex)
					m_Sel.SetSpriteFactoryIndex(nSelIndex, m_Sel.m_nFrameIndex, false);
			}

			// --------------------------------------------------------------
			if (m_Sel.m_NcSpriteFactoryPrefab != null && m_Sel.m_NcSpriteFactoryPrefab.renderer != null && m_Sel.renderer)
				if (m_Sel.m_NcSpriteFactoryPrefab.renderer.sharedMaterial != m_Sel.renderer.sharedMaterial)
					m_Sel.UpdateSpriteMaterial();

			// --------------------------------------------------------------
			m_Sel.m_MeshType	= (NcSpriteFactory.MESH_TYPE)EditorGUILayout.EnumPopup(GetHelpContent("m_MeshType"), m_Sel.m_MeshType);
			m_Sel.m_AlignType	= (NcSpriteFactory.ALIGN_TYPE)EditorGUILayout.EnumPopup(GetHelpContent("m_AlignType"), m_Sel.m_AlignType);

			// --------------------------------------------------------------
			if (ncSpriteFactory != null && ncSpriteFactory.IsValidFactory())
			{
				// Texture --------------------------------------------------------------
				rect = EditorGUILayout.BeginHorizontal(GUILayout.Height(200));
				{
					GUI.Box(rect, "");
					GUILayout.Label("");

					Rect subRect = rect;

					// draw texture
					if (0 < rect.width && m_Sel.renderer != null && m_Sel.renderer.sharedMaterial != null && m_Sel.renderer.sharedMaterial.mainTexture != null)
					{
						int nClickFactoryIndex;
						int nClickFrameIndex;
						bClickButton = DrawTrimTexture(subRect, true, m_Sel.renderer.sharedMaterial, ncSpriteFactory, m_Sel.m_nSpriteFactoryIndex, m_Sel.m_nFrameIndex, true, out nClickFactoryIndex, out nClickFrameIndex);
						if (bClickButton)
							m_Sel.SetSpriteFactoryIndex(nClickFactoryIndex, nClickFrameIndex, false);
					}
				}
				EditorGUILayout.EndHorizontal();
			} else {
				m_Sel.m_nSpriteFactoryIndex	= -1;
			}
		}
		m_UndoManager.CheckDirty();
		// --------------------------------------------------------------
		if ((EditorGUI.EndChangeCheck() || bClickButton) && GetFXMakerMain())
			OnEditComponent();
		// ---------------------------------------------------------------------
		if (GUI.tooltip != "")
			m_LastTooltip	= GUI.tooltip;
		HelpBox(m_LastTooltip);
	}

	// ----------------------------------------------------------------------------------
	// ----------------------------------------------------------------------------------
	protected GUIContent GetHelpContent(string tooltip)
	{
		string caption	= tooltip;
		string text		= FXMakerTooltip.GetHsEditor_NcSpriteTexture(tooltip);
		return GetHelpContent(caption, text);
	}

	protected override void HelpBox(string caption)
	{
		string	str	= caption;
		if (caption == "" || caption == "Script")
			str = FXMakerTooltip.GetHsEditor_NcSpriteTexture("");
		base.HelpBox(str);
	}
}
