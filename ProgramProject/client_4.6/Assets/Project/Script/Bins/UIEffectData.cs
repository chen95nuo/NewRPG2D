using UnityEngine;
using System.Collections;

public class UIEffectData : PropertyReader {
	
	public int id{get;set;}
	public string name{get;set;}
	public int postion{get;set;}
	public string path{get;set;}
	public float keepTime{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{}
	
	public static UIEffectData getData(int id)
	{
		return (UIEffectData)data[id];
	}
}
