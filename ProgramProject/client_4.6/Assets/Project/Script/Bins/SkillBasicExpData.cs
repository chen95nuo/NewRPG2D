using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillBasicExpData : PropertyReader {

	public int star{get;set;}
	public int basicexp{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(star,this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss){}
	
	public static SkillBasicExpData getData(int star)
	{
		return (SkillBasicExpData)data[star];
	}
}
