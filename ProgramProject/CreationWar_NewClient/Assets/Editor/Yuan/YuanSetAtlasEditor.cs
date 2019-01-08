using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(YuanSetAtlas))]
public class YuanSetAtlasEditor : Editor {
	
	private YuanSetAtlas yuanSetAtlas;
	private bool getSame=true;
	private bool isComplate=false;
	public GameObject objParent;
	public UIFont myFont;
	public GameObject objFontParent;
	
	
	private void OnEnable()
	{
		yuanSetAtlas=(YuanSetAtlas)this.target;
	}
	
	
	public override void OnInspectorGUI ()
	{
		RunSet ();
		
		SceneView.RepaintAll ();
	}
	
	
	void RunSet()
	{
				EditorGUILayout.BeginHorizontal();
		GUI.color=Color.green;
		GUILayout.Label(string.Format ("使用本插件之前请务必先点击右上角的小锁，锁定本监视面板"));
		GUI.color=Color.white;
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal();
		GUI.color=Color.green;
		GUILayout.Label(string.Format ("1.请在资源面板中选择想要搜索的图片"));
		GUI.color=Color.white;
		EditorGUILayout.EndHorizontal ();
		base.OnInspectorGUI ();
		
		GetSelectedTextures ();
		
		if(yuanSetAtlas.listTextures.Count>0)
		{
			GUI.color=Color.green;
			GUILayout.Label(string.Format ("2.选择一种方式搜索符合条件的物体"));
			GUI.color=Color.white;
			if(GUILayout.Button ("在所有资源中搜索物体(包括预制体)"))
			{
				getSame= SearchAssetsObj ();
				isComplate=false;
			}
			if(GUILayout.Button ("在当前场景中搜索物体(无法搜索未激活物体)"))
			{
				getSame= SearchSenceObj();
				isComplate=false;
			}
			GUILayout.BeginHorizontal();
			GUILayout.Label(string.Format ("选择的父物体："));
			objParent=(GameObject)EditorGUILayout.ObjectField(objParent, typeof(GameObject));
			if(objParent!=null)
			{
				if(GUILayout.Button ("搜索场景中物体的子物体"))
				{
					getSame=SearchChildObj (objParent);
				}
			}
			GUILayout.EndHorizontal();
				
		}
		
		EditorGUILayout.BeginHorizontal ();
		GUI.color=Color.cyan;
		GUILayout.Label(string.Format ("已搜索到符合条件的物体{0}个",yuanSetAtlas.listSprite.Count));
		GUI.color=Color.white;
		EditorGUILayout.EndHorizontal ();
		if(yuanSetAtlas.listSprite.Count>0&&!getSame)
		{
			EditorGUILayout.BeginHorizontal ();
				GUI.color=Color.yellow;
				GUILayout.Label(string.Format ("注意：搜索到的物体中包含不同的Atlas图集，请确认是否符合您的要求"));
				GUI.color=Color.white;
			EditorGUILayout.EndHorizontal ();
		}
		
		if(yuanSetAtlas.listSprite.Count>0)
		{
			GUI.color=Color.green;
			GUILayout.Label(string.Format ("3.选择要替换成的Atlas图集"));
			GUI.color=Color.white;
				EditorGUILayout.BeginHorizontal ();
				myAtlas = (UIAtlas)EditorGUILayout.ObjectField(myAtlas, typeof(UIAtlas));
			    if (GUILayout.Button(string.Format("选择Atlas图集")))
        		{
            		ComponentSelector.Show<UIAtlas>(OnSelectAtlas);
       			}
				EditorGUILayout.EndHorizontal ();
				GUILayout.Label(myAtlas != null ? myAtlas.name : "还未选择Atlas图集");
		}
		
		if(myAtlas!=null)
		{
			GUI.color=Color.green;
			GUILayout.Label(string.Format ("4.确定批量替换Atlas图集"));
			GUI.color=Color.white;			
			  if (GUILayout.Button(string.Format("开始批量替换")))
        		{
            		SetAtlas ();
       			}
		}
		
		if(isComplate)
		{
			
			GUI.color=Color.green;
			GUILayout.Label(string.Format ("5.恭喜！批量替换Atlas图集成功！请注意保存"));
			GUI.color=Color.white;			
		}
		
		NGUIEditorTools.DrawSeparator ();
		GUI.color=Color.green;
			GUILayout.Label(string.Format ("批量替换字体小工具："));
			GUI.color=Color.white;	
		GUI.color=Color.green;
			GUILayout.Label(string.Format ("1.选择要搜索的目标字体"));
			GUI.color=Color.white;
				EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(string.Format ("目标字体 :"));
				myFont = (UIFont)EditorGUILayout.ObjectField(myFont, typeof(UIFont));
			    if (GUILayout.Button(string.Format(myFont==null?"选择目标字体":myFont.name)))
        		{
            		ComponentSelector.Show<UIFont>(OnSelectFont);
       			}
				EditorGUILayout.EndHorizontal ();	
				if(myFont==null)
				{
					GUI.color=Color.yellow;
					GUILayout.Label(string.Format ("注意：如果目标字体为空，将返回所有具有UILbale的物体"));
					GUI.color=Color.white;					
				}
				GUI.color=Color.green;
			GUILayout.Label(string.Format ("2.选择一种方式搜索符合条件的物体"));
			GUI.color=Color.white;
			  if (GUILayout.Button(string.Format("在所有资源中搜索物体(包括预制体)")))
        		{
            		SearchAssetsFontObj ();
					isFontComplate=false;
       			}
				  if (GUILayout.Button(string.Format("在当前场景中搜索物体(无法搜索未激活物体)")))
        		{
            		SearchSenceFontObj ();
				isFontComplate=false;
       			}
		
				GUILayout.BeginHorizontal();
			GUILayout.Label(string.Format ("选择的父物体："));
			objFontParent=(GameObject)EditorGUILayout.ObjectField(objFontParent, typeof(GameObject));
			if(objFontParent!=null)
			{
	 			if (GUILayout.Button(string.Format("搜索场景中物体的子物体")))
        		{
            		SearchChildFontObj (objFontParent);
					isFontComplate=false;
       			}
			}
			GUILayout.EndHorizontal();
		GUI.color=Color.cyan;
		GUILayout.Label(string.Format ("已搜索到符合条件的物体{0}个",yuanSetAtlas.listLable.Count));
		GUI.color=Color.white;			
		if(yuanSetAtlas.listLable.Count>0)
		{
			GUI.color=Color.green;
			GUILayout.Label(string.Format ("3.选择要替换成的UIFont"));
			GUI.color=Color.white;
				EditorGUILayout.BeginHorizontal ();
				setFont = (UIFont)EditorGUILayout.ObjectField(setFont, typeof(UIFont));
			    if (GUILayout.Button(setFont==null?"选择UIFont":setFont.name))
        		{
            		ComponentSelector.Show<UIFont>(OnSelectSetFont);
					isFontComplate=false;
       			}
				EditorGUILayout.EndHorizontal ();
		}
		
		if(setFont!=null)
		{
			GUI.color=Color.green;
			GUILayout.Label(string.Format ("4.确定批量替换Font"));
			GUI.color=Color.white;			
			  if (GUILayout.Button(string.Format("开始批量替换")))
        		{
            		SetFont ();
       			}
		}
		
		if(isFontComplate)
		{
			
			GUI.color=Color.green;
			GUILayout.Label(string.Format ("5.恭喜！批量替换Font成功！请注意保存"));
			GUI.color=Color.white;			
		}
	}
	
	private UIAtlas myAtlas;
	void OnSelectAtlas(Object obj)
    {
        myAtlas = obj as UIAtlas;
        UnityEditor.EditorUtility.SetDirty(this);
		isComplate=false;
    }
	
	void OnSelectFont(Object obj)
    {
        myFont = obj as UIFont;
        UnityEditor.EditorUtility.SetDirty(this);
		isFontComplate=false;
    }
	
	private UIFont setFont;
	void OnSelectSetFont(Object obj)
    {
        setFont = obj as UIFont;
        UnityEditor.EditorUtility.SetDirty(this);
		isFontComplate=false;
    }
	
	public	List<Texture> GetSelectedTextures ()
	{
		yuanSetAtlas.listTextures.Clear ();

		if (Selection.objects != null && Selection.objects.Length > 0)
		{
			Object[] objects = EditorUtility.CollectDependencies(Selection.objects);

			foreach (Object o in objects)
			{
				Texture tex = o as Texture;
				if (tex != null ) 
				{
					yuanSetAtlas.listTextures.Add(tex);
				}
			}
		}
		GUI.color=Color.cyan;
		GUILayout.Label(string.Format ("已选择{0}个图片",yuanSetAtlas.listTextures.Count));
		GUI.color=Color.white;
		

		
		return yuanSetAtlas.listTextures;
	}
	
	public bool SearchSenceObj()
	{
		yuanSetAtlas.listSprite.Clear ();
		UISprite[] getSp=(UISprite[])Object.FindObjectsOfType(typeof(UISprite));
//		Debug.Log ("-----------"+getSp);
		bool isSame=true;
		UIAtlas tempAtlas=null;
		foreach(UISprite item in getSp)
		{
			foreach(Texture mTex in yuanSetAtlas.listTextures)
			{
				if(mTex.name==item.spriteName)
				{
					
					yuanSetAtlas.listSprite.Add (item);
					
					if(isSame)
					{
						if(tempAtlas==null)
						{
							tempAtlas=item.atlas;
						}
						else if(item.atlas!=tempAtlas)
						{
							isSame=false;
						}
						else
						{
							tempAtlas=item.atlas;
						}
					}
					break;
				}
			}
		}
		return isSame;
	}
	
	public bool SearchAssetsObj()
	{
		yuanSetAtlas.listSprite.Clear ();
		UISprite[] getSp=(UISprite[])Resources.FindObjectsOfTypeAll (typeof(UISprite));
		bool isSame=true;
		UIAtlas tempAtlas=null;
		foreach(UISprite item in getSp)
		{
			foreach(Texture mTex in yuanSetAtlas.listTextures)
			{
				if(mTex.name==item.spriteName)
				{
					yuanSetAtlas.listSprite.Add (item);
					
					if(isSame)
					{
						if(tempAtlas==null)
						{
							tempAtlas=item.atlas;
						}
						else if(item.atlas!=tempAtlas)
						{
							isSame=false;
						}
						else
						{
							tempAtlas=item.atlas;
						}
					}
					break;
				}
			}
		}
		return isSame;
	}
	
	public bool SearchChildObj(GameObject mParent)
	{
		yuanSetAtlas.listSprite.Clear ();
		UISprite[] getSp=(UISprite[])mParent.GetComponentsInChildren<UISprite>(true);
		bool isSame=true;
		UIAtlas tempAtlas=null;
		foreach(UISprite item in getSp)
		{
			foreach(Texture mTex in yuanSetAtlas.listTextures)
			{
				if(mTex.name==item.spriteName)
				{
					yuanSetAtlas.listSprite.Add (item);
					
					if(isSame)
					{
						if(tempAtlas==null)
						{
							tempAtlas=item.atlas;
						}
						else if(item.atlas!=tempAtlas)
						{
							isSame=false;
						}
						else
						{
							tempAtlas=item.atlas;
						}
					}
					break;
				}
			}
		}
		return isSame;
	}
	
	public void SetAtlas()
	{
		foreach(UISprite item in yuanSetAtlas.listSprite)
		{
			item.atlas=myAtlas;
		}
		isComplate=true;
	}
	
	private bool isFontComplate=false;
	public void SetFont()
	{
		foreach(UILabel item in yuanSetAtlas.listLable)
		{
			item.font=setFont;
		}
		isFontComplate=true;
	}
	
	public void SearchAssetsFontObj()
	{
		yuanSetAtlas.listLable.Clear ();
		UILabel[] getSp=(UILabel[])Resources.FindObjectsOfTypeAll (typeof(UILabel));
		foreach(UILabel item in getSp)
		{
			if(myFont!=null)
			{
				if(item.font==myFont)
				{
					yuanSetAtlas.listLable.Add (item);
				}
			}
			else
			{
				yuanSetAtlas.listLable.Add (item);
			}
		}
	}
	
	public void SearchSenceFontObj()
	{
		yuanSetAtlas.listLable.Clear ();
		UILabel[] getSp=(UILabel[])Object.FindObjectsOfType (typeof(UILabel));
		foreach(UILabel item in getSp)
		{
			if(myFont!=null)
			{
				if(item.font==myFont)
				{
					yuanSetAtlas.listLable.Add (item);
				}
			}
			else
			{
				yuanSetAtlas.listLable.Add (item);
			}
		}
	}
	
	public void SearchChildFontObj(GameObject mParent)
	{
		yuanSetAtlas.listLable.Clear ();
		UILabel[] getSp=(UILabel[])mParent.GetComponentsInChildren<UILabel>(true);
		foreach(UILabel item in getSp)
		{
			if(myFont!=null)
			{
				if(item.font==myFont)
				{
					yuanSetAtlas.listLable.Add (item);
				}
			}
			else
			{
				yuanSetAtlas.listLable.Add (item);
			}
		}
	}
}
