using UnityEngine;
using System.Collections;

public class BloodBuffData : PropertyReader {

	public int num{get;set;}
	public int type{get;set;}
	public int target{get;set;}
	public float effect{get;set;}
	public int probability{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(num,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static BloodBuffData getData(int num)
	{
		return (BloodBuffData)data[num];
	}
}
