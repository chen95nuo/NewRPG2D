using UnityEngine;
using System.Collections;

public class AllThreeStarData : PropertyReader
{
	//==编号==//	
	public int id{get;set;}
	//==关卡类型==//
	public int type{get;set;}
	//==地图==//
	public int map{get;set;}
	//==区域==//
	public int zone{get;set;}
	//==区域描述==//
	public string description{get;set;}
	//==奖励类别==//
	public int rewardtype{get;set;}
	//==奖励==//
	public string reward{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(map+"-"+zone+"-"+type,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static AllThreeStarData getData(string key)
	{
		return (AllThreeStarData)data[key];
	}
}
