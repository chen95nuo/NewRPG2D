//----------------------------------------------
//           ZealmTools
// Copyright © 2010-2014 Zealm
// Copyright © 2010-2014 FernYuan
//----------------------------------------------



using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ReplaceFont : YuanEditorWindow {

	protected virtual void Awake()
	{
		 listLable=new List<UILabel>();
		 listInput=new List<YuanInput>();
		 listCharBar=new List<CharBar>();
		 listPoplist=new List<UIPopupList>();
	}

	public UIFont myFont;
	public GameObject objFontParent;
	private List<UILabel> listLable=new List<UILabel>();
	private List<YuanInput> listInput=new List<YuanInput>();
	private List<CharBar> listCharBar=new List<CharBar>();
	private List<UIPopupList> listPoplist=new List<UIPopupList>();


	protected override void DarwInfo ()
	{
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
		if (GUILayout.Button(string.Format("在所有资源中搜索物体(预制体)")))
		{
			listLable.Clear ();
			listPoplist.Clear ();
			listInput.Clear ();
			listCharBar.Clear ();
			Object[] GetObjs=ZealmToolsManager.Search (typeof(UILabel),typeof(YuanInput),typeof(UIPopupList),typeof(CharBar));
			for(int i=0;i<GetObjs.Length;i++)
			{
				if(GetObjs[i] is UILabel)
				{
					listLable.Add (GetObjs[i] as UILabel);
				}
				else if(GetObjs[i] is UIPopupList)
				{
					listPoplist.Add (GetObjs[i] as UIPopupList);
				}
				else if(GetObjs[i] is YuanInput)
				{
					listInput.Add (GetObjs[i] as YuanInput);
				}
				else if(GetObjs[i] is CharBar)
				{
					listCharBar.Add (GetObjs[i] as CharBar);
				}
			}

			isFontComplate=false;
		}
		if (GUILayout.Button(string.Format("在当前场景中搜索物体(可以搜索未激活物体)")))
		{
			listLable.Clear ();
			listLable.AddRange (ZealmToolsManager.FindAll<UILabel>());
			listInput.Clear ();
			listInput.AddRange (ZealmToolsManager.FindAll<YuanInput>());
			listPoplist.Clear ();
			listPoplist.AddRange (ZealmToolsManager.FindAll<UIPopupList>());
			listCharBar.Clear ();
			listCharBar.AddRange (ZealmToolsManager.FindAll<CharBar>());
			isFontComplate=false;
		}
		GUI.color=Color.cyan;
		GUILayout.Label(string.Format ("已搜索到符合条件的物体{0}个",listLable.Count+listInput.Count+listPoplist.Count));
		GUI.color=Color.white;			
		if((listLable.Count+listInput.Count+listPoplist.Count)>0)
		{


			GUI.color=Color.green;
			GUILayout.Label(string.Format ("3.选择要替换成的UIFont"));
			GUI.color=Color.white;
			EditorGUILayout.BeginHorizontal ();
			setFont = (UIFont)EditorGUILayout.ObjectField(setFont, typeof(UIFont));
			if (GUILayout.Button(setFont==null?"选择UIFont":setFont.name))
			{
				//ComponentSelector.Show<Font>(OnSelectSetFont, new string[] { ".ttf", ".otf" });
				ComponentSelector.Show<UIFont>(OnSelectSetFont);

				isFontComplate=false;
			}
			EditorGUILayout.EndHorizontal ();
		}
		
		if(setFont!=null&&listLable.Count>0)
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
			EditorUtility.DisplayDialog ("祝贺！","锵！锵！锵！费尽九牛二虎之力终于替换完成啦，注意保存哟！","遵命");
			GUI.color=Color.green;
			GUILayout.Label(string.Format ("5.恭喜！批量替换Font成功！请注意保存"));
			GUI.color=Color.white;		
			isFontComplate=false;
		}
	}

	private bool isFontComplate=false;
	public void SetFont()
	{
		foreach(UILabel item in listLable)
		{
		
			item.ambigiousFont=setFont;
			item.fontSize+=1;
			item.fontSize-=1;
			UnityEditor.EditorUtility.SetDirty (item);
		}
//		foreach(YuanInput item in listInput)
//		{
//			item.font=setFont;
//			UnityEditor.EditorUtility.SetDirty (item);
//		}
//		foreach(UIPopupList item in listPoplist)
//		{
//			item.ambigiousFont=setFont;
//			UnityEditor.EditorUtility.SetDirty (item);
//		}
//		foreach(CharBar item in listCharBar)
//		{
//			item.font=setFont;
//			UnityEditor.EditorUtility.SetDirty (item);
//		}


		isFontComplate=true;
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



}
