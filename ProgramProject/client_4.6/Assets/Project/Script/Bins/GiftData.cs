using UnityEngine;
using System.Collections;

public class GiftData : PropertyReader {

	public int id{get;set;}
	public int time{get;set;}
	public int rewardtyp1{get;set;}
	public string reward1{get;set;}
	public int rewardtyp2{get;set;}
	public string reward2{get;set;}
	public int rewardtyp3{get;set;}
	public string reward3{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static GiftData getData(int id)
	{
		return (GiftData)data[id];
	}
}
