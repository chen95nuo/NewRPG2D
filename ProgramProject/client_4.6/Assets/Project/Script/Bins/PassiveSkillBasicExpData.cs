using UnityEngine;
using System.Collections;

public class PassiveSkillBasicExpData : PropertyReader {

	public int star{get;set;}
	public int basicexp{get;set;}
	
	private static Hashtable data = new Hashtable();
	
	public void addData()
	{
		data.Add(star, this);
	}
	
	public void resetData()
	{
		data.Clear();
	}
	
	public void parse(string[] ss)
	{
		
	}

	public static PassiveSkillBasicExpData getData(int star)
	{
		return (PassiveSkillBasicExpData)data[star];
	}
}
