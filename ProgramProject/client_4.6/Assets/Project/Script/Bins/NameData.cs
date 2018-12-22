using UnityEngine;
using System.Collections;

public class NameData : PropertyReader {

	//==编号==//
	public int id{get;set;}
	//==前缀==//
	public string front{get;set;}
	//==后缀==//
	public string back{get;set;}
		
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static NameData getData(int id)
	{
		return (NameData)data[id];
	}
}
