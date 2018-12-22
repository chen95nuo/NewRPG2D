using UnityEngine;
using System.Collections;

public class UsableItemData : PropertyReader {
	public int id{get;set;}
	public string name{get;set;}
	public int cost{get;set;}
	public int uselevel{get;set;}
	public int viprequest{get;set;}
	public int rewardtype1{get;set;}
	public string reward1{get;set;}
	public int rewardtype2{get;set;}
	public string reward2{get;set;}
	public int rewardtype3{get;set;}
	public string reward3{get;set;}
	public int rewardtype4{get;set;}
	public string reward4{get;set;}
	public int rewardtype5{get;set;}
	public string reward5{get;set;}
	public int rewardtype6{get;set;}
	public string reward6{get;set;}
	public int rewardtype7{get;set;}
	public string reward7{get;set;}
	public int rewardtype8{get;set;}
	public string reward8{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss){}
	
	public static UsableItemData getData(int id)
	{
		return (UsableItemData)data[id];
	}
	
}
