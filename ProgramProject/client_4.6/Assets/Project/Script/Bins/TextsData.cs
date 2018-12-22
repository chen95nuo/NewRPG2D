using UnityEngine;
using System.Collections;

public class TextsData : PropertyReader {
	
	public int id{get;set;}
	public string chinese{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData(){
		data.Clear();
	}
	public void parse(string[] ss){}
	
	public static TextsData getData(int id)
	{
		return (TextsData)data[id];
	}
	
}
