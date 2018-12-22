using UnityEngine;
using System.Collections;

public class IconUnlockData : PropertyReader {
	//==编号==//
	public int ID{get;set;}
	//==解锁要求==//
	public int unlock{get;set;}
	//==头像1==//
	public string headIcon1{get;set;}
	//==头像2==//
	public string headIcon2{get;set;}
	//==头像3==//
	public string headIcon3{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(ID,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static IconUnlockData getData(int id)
	{
		return (IconUnlockData)data[id];
		
	}
	
	public static int GetHashLength()
	{
		
		return data.Count;
	}
}
