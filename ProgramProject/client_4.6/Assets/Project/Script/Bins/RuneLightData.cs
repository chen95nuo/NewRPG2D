using UnityEngine;
using System.Collections;

public class RuneLightData : PropertyReader {
	
	public int page{get;set;}
	public int runeSequence{get;set;}
	public string runeName{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(page+"-"+runeSequence,runeName);
	}
	public void resetData()
	{
		
	}
	public void parse(string[] ss)
	{
		
	}
	
	public static string getRuneName(int page,int runeSequence)
	{
		return (string)data[page+"-"+runeSequence];
	}
}
