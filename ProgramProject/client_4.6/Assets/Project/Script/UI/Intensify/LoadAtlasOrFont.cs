using UnityEngine;
using System.Collections;

public class LoadAtlasOrFont {
	//hero atlas path//
	static string atlasPath = "UI/UIAtlas/";
	//font path//
	static string fontPath = "UI/UIFont/Num_Texture/";
	
	public static UIAtlas LoadHeroAtlasByName(string name)
	{
		GameObject go = null;
		UIAtlas atlas = null;
		if(name != null)
		{
			go = (GameObject) Resources.Load(atlasPath + name);
			if(go != null)
			{
				atlas = go.GetComponent<UIAtlas>();
			}
		}
		return atlas;
	}
	
	public static UIAtlas LoadAtlasByName(string name)
	{
		GameObject go = null;
		UIAtlas atlas = null;
		if(name != null)
		{
			go = (GameObject) Resources.Load(atlasPath + name);
			if(go != null)
			{
				atlas = go.GetComponent<UIAtlas>();
			}
		}
		return atlas;
	}
	
	public static UIAtlas LoadAtlas(string path)
	{
		UIAtlas atlas= null;
		if(path != "")
		{
			GameObject go = (GameObject) Resources.Load(path );
			if(go != null)
			{
				atlas = go.GetComponent<UIAtlas>();
			}
		}
		return atlas;
	}
	
	public static UIFont LoadFont(string name)
	{
		UIFont font = null;
		if(name != "")
		{
			GameObject go = (GameObject) Resources.Load(fontPath + name );
			if(go != null)
			{
				font = go.GetComponent<UIFont>();
			}
		}
		return font;
	}
}
