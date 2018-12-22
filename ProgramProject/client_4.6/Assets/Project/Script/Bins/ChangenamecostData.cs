using UnityEngine;
using System.Collections;

public class ChangenamecostData : PropertyReader {
	
	//==改名次数:10次以后按照10次==//
	public int times{get;set;}
	//==花费==//
	public int cost{get;set;}

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(times,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static int getCost(int times)
	{
		if(times>10)
		{
			times=10;
		}
		return ((ChangenamecostData)data[times]).cost;
	}
}
