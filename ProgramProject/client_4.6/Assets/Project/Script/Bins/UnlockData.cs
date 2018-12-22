using UnityEngine;
using System.Collections;

public class UnlockData : PropertyReader{

	//==编号==//
	public int id{get;set;}		
	//==模块名称==//
	public string name{get;set;}
	//==模块==//
	public int mode{get;set;}
	//==描述==//
	public string description{get;set;}
	//==解锁类型==//
	public int type{get;set;}
	//==解锁条件==//
	public int method{get;set;}
	//==是否弹窗==//
	public int showup{get;set;}
	//==解锁图标==//
	public string icon{get;set;}
	//==解锁描述==//
	public string unlockdes{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static UnlockData getData(int modelId){
		return (UnlockData)data[modelId];
	}
}
