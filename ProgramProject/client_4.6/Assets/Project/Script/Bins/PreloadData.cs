using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreloadData : PropertyReader {
	public int id {get;set;}
	public string path{get;set;}
	private static Hashtable data = new Hashtable();
	public static List<PreloadData> dataList = new List<PreloadData>();

	public void addData()
    {
        data.Add(id, this);
		dataList.Add(this);
    }
	
	public void resetData()
    {
        data.Clear();
    }
	
	public void parse(string[] ss) 
	{
	
	}
	
	
	
}
