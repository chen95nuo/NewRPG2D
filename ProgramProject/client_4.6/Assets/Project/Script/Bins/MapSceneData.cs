using UnityEngine;
using System.Collections;

public class MapSceneData : PropertyReader {
	
	public int id{get;set;}
	public string scene{get;set;}
	public string fog{get;set;}
	public float[] fogs;

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		string[] ss=fog.Split(',');
		fogs=new float[ss.Length];
		for(int k=0;k<fogs.Length;k++)
		{
			fogs[k]=StringUtil.getFloat(ss[k]);
		}
		
		data.Add(id,this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{
		
	}
	
	public static MapSceneData getData(string sceneName)
	{
		foreach(DictionaryEntry de in data)
		{
			if(((MapSceneData)de.Value).scene == sceneName)
			{
				return (MapSceneData)de.Value;
			}
		}
		return null;
	}
	
}
