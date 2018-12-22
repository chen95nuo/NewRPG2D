using UnityEngine;
using System.Collections;

public class PowerData : PropertyReader {

	//==编号==//
	public int id{get;set;}
	//==系数==//
	public float number{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static float getMul(int id)
	{
		return ((PowerData)data[id]).number;
	}
}
