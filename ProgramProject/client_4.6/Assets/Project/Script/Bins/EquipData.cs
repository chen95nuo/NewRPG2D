using UnityEngine;
using System.Collections;

public class EquipData :PropertyReader {
	
	public int index{get;set;}
	public int number{get;set;}
	public string name{get;set;}
	public int type{get;set;}
	public int star{get;set;}
	public int level{get;set;}
	public string resource{get;set;}
	public string description{get;set;}
	public int sell{get;set;}
	public string icon{get;set;}

	private static ArrayList data=new ArrayList();
	
	public void addData()
	{
		data.Add(this);
	}
	
	public void resetData()
	{
		data.Clear();
	}
	
	public void parse(string[] ss)
	{
		
	}
	
	
	public static EquipData getData(int equipId)
	{
		foreach(EquipData ed in data)
		{
			if(ed.index==equipId)
			{
				return ed;
			}
		}
		return null;
	}
	
}
