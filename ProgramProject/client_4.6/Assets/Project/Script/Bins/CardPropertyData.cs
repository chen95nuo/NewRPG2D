using UnityEngine;
using System.Collections;

public class CardPropertyData : PropertyReader {
	
	public int level{get;set;}
	public int atk{get;set;}
	public int def{get;set;}
	public int hp{get;set;}

	private static ArrayList data=new ArrayList();
	
	public void addData()
	{
		data.Add(this);
//		Debug.Log("size:"+data.Count);
	}
	public void resetData()
	{
		data.Clear();	
	}
	public void parse(string[] ss)
	{
		
	}
	
	public static CardPropertyData getData(int level)
	{
		foreach(CardPropertyData cpd in data)
		{
			if(cpd.level==level)
			{
				return cpd;
			}
		}
		return null;
	}
	
}
