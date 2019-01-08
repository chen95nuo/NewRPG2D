using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(YuanPicManager))]
public class PicEdite : Editor
{

    YuanPicManager picManager;

    void OnEnable()
    {
        picManager = (YuanPicManager)this.target;
    }


    void RegisterUndo()
    {
        NGUIEditorTools.RegisterUndo("YuanPic Change", this);
    }

    void OnSelectAtlas(Object obj)
    {
        cbAtlas.atlas = obj as UIAtlas;
        UnityEditor.EditorUtility.SetDirty(this);
    }

	void OnSelectAtlasBK(Object obj)
    {
        atlasBK = obj as UIAtlas;
        UnityEditor.EditorUtility.SetDirty(this);
    }
	void OnSelectAtlasMR(Object obj)
    {
        atlasMR = obj as UIAtlas;
        UnityEditor.EditorUtility.SetDirty(this);
    }
	void OnSelectAtlasMS(Object obj)
    {
        atlasMS = obj as UIAtlas;
        UnityEditor.EditorUtility.SetDirty(this);
    }
	void OnSelectAtlasPlayer(Object obj)
    {
        atlasPlayer = obj as UIAtlas;
        UnityEditor.EditorUtility.SetDirty(this);
    }
	
	void OnSelectAtlasExpression(Object obj)
    {
        atlasExpression = obj as UIAtlas;
        UnityEditor.EditorUtility.SetDirty(this);
    }

    void OnSelectSprite(string spriteName)
    {     

        cbSpriteName.spriteName = spriteName;
    }

    private bool selectBKout;
    private Dictionary<string, bool> dicOut = new Dictionary<string, bool>();
    private bool selectBkSize;
    private UIAtlas atlasBK;
    private UIAtlas atlasMR;
    private UIAtlas atlasMS;
	private UIAtlas atlasPlayer;
	private UIAtlas atlasExpression;
    private YuanPic cbSpriteName;
    private YuanPic cbAtlas;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        NGUIEditorTools.DrawSeparator();
        this.DrewGUI(this.picManager.picSelectBackground, "BackGround",OnSelectAtlasBK,atlasBK);
        NGUIEditorTools.DrawSeparator();
        this.DrewGUI(this.picManager.picSelectMark, "Mark",OnSelectAtlasMR,atlasMR);
        NGUIEditorTools.DrawSeparator();
        this.DrewGUI(this.picManager.picSelectMask, "Mask",OnSelectAtlasMS,atlasMS);
        NGUIEditorTools.DrawSeparator();
		   this.DrewGUI(this.picManager.picPlayer, "Player",OnSelectAtlasPlayer,atlasPlayer);
        NGUIEditorTools.DrawSeparator();
		 this.DrewGUI(this.picManager.picExpression, "Expression",OnSelectAtlasExpression,atlasExpression);
        NGUIEditorTools.DrawSeparator();
        SceneView.RepaintAll();
    }

    private void DrewGUI(List<YuanPic> yuanPic,string listName,ComponentSelector.OnSelectionCallback callback,UIAtlas atlasCB)
    {
		if (NGUIEditorTools.DrawHeader(listName))
		{
	        if (!dicOut.ContainsKey(listName))
	        {
	            dicOut.Add(listName, false);
	        }
	        atlasCB = (UIAtlas)EditorGUILayout.ObjectField(atlasCB, typeof(UIAtlas));
	        EditorGUILayout.BeginHorizontal();
	        if (GUILayout.Button(string.Format("{0}Atlas",listName)))
	        {
	            ComponentSelector.Show<UIAtlas>(callback);

	        }
	        GUILayout.Label(atlasCB != null ? atlasCB.name : "No Selected");
	        EditorGUILayout.EndHorizontal();
	        dicOut[listName] = EditorGUILayout.Foldout(dicOut[listName], listName);
	        if (dicOut[listName])
	        {
	            int count = yuanPic.Count;

	            count = EditorGUILayout.IntField("Size:", count);



	            if (yuanPic.Count < count)
	            {
	                int num = count - yuanPic.Count;
	                for (int i = 0; i < num; i++)
	                {
	                    YuanPic tempYuanPic = new YuanPic();
	                    yuanPic.Add(tempYuanPic);
	                    tempYuanPic.atlas = atlasCB != null ? atlasCB : null;
	                }
	            }
	            else
	            {
	                int num = yuanPic.Count - count;
	                for (int i = 0; i < num; i++)
	                {
	                    yuanPic.RemoveAt(yuanPic.Count - 1);
	                }
	            }

	            int imageNum = 0;
	            foreach (YuanPic item in yuanPic)
	            {
	                GUILayout.BeginHorizontal();
	                GUILayout.Label("Image" + imageNum.ToString() + ":");

	                if (GUILayout.Button(item.atlas != null ? item.atlas.name : "Select Atlas"))
	                {
	                    cbAtlas = item;
	                    ComponentSelector.Show<UIAtlas>(OnSelectAtlas);

	                }
	                if (item.atlas != null)
	                {
	                    if (GUILayout.Button(item.spriteName != null ? item.spriteName : "Select Sprite"))
	                    {
							NGUISettings.atlas = item.atlas;
							NGUISettings.selectedSprite = item.spriteName;
							SpriteSelector.Show(OnSelectSprite);
	//                        SpriteSelector.Show(item.atlas, item.spriteName, OnSelectSprite);
	                        cbSpriteName = item;
	                    }
	                }
	                GUILayout.EndHorizontal();
	                imageNum++;
	            }

	        }
	}
    }
}
