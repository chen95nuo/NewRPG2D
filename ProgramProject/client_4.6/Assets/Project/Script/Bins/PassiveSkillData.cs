using UnityEngine;
using System.Collections;

public class PassiveSkillData:PropertyReader
{

	public int index{get;set;}
	public int number{get;set;}
	public string name{get;set;}
	public int type{get;set;}
	public int numbers{get;set;}
	public int star{get;set;}
	public int level{get;set;}
	public int exp{get;set;}
	public int action{get;set;}
	public string SE{get;set;}
	public float SETime{get;set;}
	public string icon{get;set;}
	public string describe{get;set;}
	public int sell{get;set;}

	private static Hashtable data=new Hashtable();
		
		
	public void addData()
	{
		data.Add(index,this);
	}
	
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{
		
	}
	
	public static PassiveSkillData getData(int passiveSkillId)
	{
		foreach(DictionaryEntry de in data)
		{
			if((int)(de.Key)==passiveSkillId)
			{
				return (PassiveSkillData)de.Value;
			}
		}
		return null;
	}
}
