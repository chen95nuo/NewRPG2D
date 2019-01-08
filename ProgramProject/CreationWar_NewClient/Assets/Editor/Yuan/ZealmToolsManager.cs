//----------------------------------------------
//           ZealmTools
// Copyright © 2010-2014 Zealm
// Copyright © 2010-2014 FernYuan
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ZealmToolsManager : Editor
{

	[MenuItem("ZealmTools/SDKOpenPack(一键出包)",false,9)]
	public static void CreateSDKOpenPack()
	{
		EditorWindow.GetWindow<SDKOpenPackEditor>(false,"SDKOpenTool(一键出包)",true).Show();
	}

	[MenuItem("ZealmTools/ReplaceFont(一键替换字体)",false,9)]
	public static void CreateReplaceFont()
	{
		EditorWindow.GetWindow<ReplaceFont>(false,"ReplaceFont",true).Show();
	}

	[MenuItem("Assets/Create/C# Script(ZealmServer)", false, 1)]
	public static void CreateServerCSharp()
	{

	}



	/// <summary>
	/// 寻找所有类型的预制体
	/// </summary>
	/// <param name="mType">M type.</param>
	public static Object[] Search (params System.Type[] mType)
	{
		
		string[]  mExtensions =new string[] {".prefab"};
		
		string[] paths = AssetDatabase.GetAllAssetPaths();
		
		//			bool isComponent = mType.IsSubclassOf(typeof(Component));
		
		BetterList<Object> list = new BetterList<Object>();
		
		
		
		for (int i = 0; i < paths.Length; ++i)
		{
			string path = paths[i];
			
			bool valid = false;
			
			for (int b = 0; b < mExtensions.Length; ++b)
			{
				if (path.EndsWith(mExtensions[b], System.StringComparison.OrdinalIgnoreCase))
				{
					valid = true;
					break;
				}
			}
			
			if (!valid) continue;
			
			//				EditorUtility.DisplayProgressBar("资源找寻中", "机器正在使出吃奶的劲搜索预制体，切勿浮躁，耐心等候……", (float)i / paths.Length);
			Object obj = AssetDatabase.LoadMainAssetAtPath(path);
			if (obj == null || list.Contains(obj)) continue;
			
			
			for(int numType=0;numType<mType.Length;numType++)
			{
				bool isComponent = mType[numType] .IsSubclassOf(typeof(Component));
				if (!isComponent)
				{
					System.Type t = obj.GetType();
					if (t == mType[numType] || t.IsSubclassOf(mType[numType]) && !list.Contains(obj))
						list.Add(obj);
				}
				else if (PrefabUtility.GetPrefabType(obj) == PrefabType.Prefab)
				{
					GameObject gameobj=obj as GameObject;
					Object t =gameobj.GetComponent(mType[numType]);
					if (t != null && !list.Contains(t)) list.Add(t);
					
					
					Object[] listPre = gameobj.GetComponentsInChildren(mType[numType],true);
					for(int num=0;num<listPre.Length;num++)
					{
						if(!list.Contains(listPre[num]))
						{
							list.Add (listPre[num]);
						}
					}
				}
			}
			EditorUtility.DisplayProgressBar("资源找寻中", "机器正在使出吃奶的劲搜索预制体，切勿浮躁，耐心等候……", (float)i / paths.Length);
			
		}
		//list.Sort(delegate(Object a, Object b) { return a.name.CompareTo(b.name); });
		EditorUtility.ClearProgressBar();
		return list.ToArray();
		
	}
	
	/// <summary>
	/// 搜索场景中所有的type类型资源
	/// </summary>
	/// <returns>The all.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	static public List<T> FindAll<T> () where T : Component
	{
		T[] comps = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
		
		List<T> list = new List<T>();
		
		foreach (T comp in comps)
		{
			if (comp.gameObject.hideFlags == 0)
			{
				string path = AssetDatabase.GetAssetPath(comp.gameObject);
				if (string.IsNullOrEmpty(path)) list.Add(comp);
			}
		}
		return list;
	}

}
