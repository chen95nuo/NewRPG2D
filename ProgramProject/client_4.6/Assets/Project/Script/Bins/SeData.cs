using UnityEngine;
using System.Collections;

public class SeData : PropertyReader {

	//==编号==//
	public int id{get;set;}
	//==音乐==//
	public string music{get;set;}
	public int loop{get;set;}
	//==说明==//
	public string des{get;set;}

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static SeData getData(int id)
	{
		return (SeData)data[id];
	}
}
