using UnityEngine;
using System.Collections.Generic;

public class UserData : PropertyReader {

	public string username{get;set;}
	public string password{get;set;}
	
	private static List<UserData> data=new List<UserData>();
	
	public void addData()
	{
		data.Add(this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static List<UserData> getDatas()
	{
		return data;
	}
	
}
