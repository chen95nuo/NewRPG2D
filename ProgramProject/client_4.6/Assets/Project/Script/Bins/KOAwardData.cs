using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KOAwardData : PropertyReader {
	public int id {get;set;}
	public int number{get;set;}
	public int numName{get;set;}
	public string discretion{get;set;}
	public int rewardtyp1{get;set;}
	public string reward1{get;set;}
	public string numNum{get;set;}
	
	private static Hashtable data = new Hashtable();
	public static List<KOAwardData> dataList = new List<KOAwardData>();
	
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
	
	public static KOAwardData getData(int id)
    {
        return (KOAwardData)data[id];
    }
	
	public static List<KOAwardData> getList()
	{
		return dataList;
	}
}
