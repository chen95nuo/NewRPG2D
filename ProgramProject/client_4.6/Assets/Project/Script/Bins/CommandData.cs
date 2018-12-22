using UnityEngine;
using System.Collections;

public class CommandData : PropertyReader {
	
	public int id{get;set;}
	public string name{get;set;}
	public string urlPrefix{get;set;}
	public string urlSuffix{get;set;}
	public string param1{get;set;}
	public string param2{get;set;}
	
	private static ArrayList data=new ArrayList();

	public void addData()
	{
		data.Add(this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss){}
	
	public static CommandData getData(int commandId)
	{
		foreach(CommandData cd in data)
		{
			if(cd.id==commandId)
			{
				return cd;
			}
		}
		return null;
	}
	
}
