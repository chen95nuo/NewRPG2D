using UnityEngine;
using System.Collections;

public class SkillPropertyData : PropertyReader {
	
	//==种类:1：攻击和回复类型主动技能,2：防御类型主动技能==//
	public int type{get;set;}
	//==等级==//
	public int level{get;set;}
	//==1星数值==//
	public int starNumber1{get;set;}
	//==2星数值==//
	public int starNumber2{get;set;}
	//==3星数值==//
	public int starNumber3{get;set;}
	//==4星数值==//
	public int starNumber4{get;set;}
	//==5星数值==//
	public int starNumber5{get;set;}
	//==6星数值==//
	public int starNumber6{get;set;}
	
	public int[] starNumbers;
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		starNumbers=new int[6];
		starNumbers[0]=starNumber1;
		starNumbers[1]=starNumber2;
		starNumbers[2]=starNumber3;
		starNumbers[3]=starNumber4;
		starNumbers[4]=starNumber5;
		starNumbers[5]=starNumber6;
		
		data.Add(type+"-"+level,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	//==skillType:1,2,3==//
	public static int getProperty(int skillType,int level,int star)
	{
		return ((SkillPropertyData)data[skillType+"-"+level]).starNumbers[star-1];
	}
}
