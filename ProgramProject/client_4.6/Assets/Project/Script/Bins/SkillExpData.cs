using UnityEngine;
using System.Collections;

public class SkillExpData : PropertyReader {

	//==等级==//
	public int level{get;set;}
	//==1星经验值==//
	public int starexp1{get;set;}					
	//==2星经验值==//
	public int starexp2{get;set;}
	//==3星经验值==//
	public int starexp3{get;set;}
	//==4星经验值==//
	public int starexp4{get;set;}
	//==5星经验值==//
	public int starexp5{get;set;}
	//==6星经验值==//
	public int starexp6{get;set;}
	//==消耗金币==//
	public int cost{get;set;}
	
	public int[] starexps;
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		starexps=new int[6];
		starexps[0]=starexp1;
		starexps[1]=starexp2;
		starexps[2]=starexp3;
		starexps[3]=starexp4;
		starexps[4]=starexp5;
		starexps[5]=starexp6;
		
		data.Add(level,this);
	}
	
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static SkillExpData getData(int level)
	{
		if(!data.ContainsKey(level))
			return null;
		return (SkillExpData)data[level];
	}
	
	public static int getExp(int level,int star)
	{
		if(!data.ContainsKey(level))
			return 0;
		return ((SkillExpData)data[level]).starexps[star-1];
	}
	
	public static int getCost(int level)
	{
		if(!data.ContainsKey(level))
			return 0;
		return ((SkillExpData)data[level]).cost;
	}
}
