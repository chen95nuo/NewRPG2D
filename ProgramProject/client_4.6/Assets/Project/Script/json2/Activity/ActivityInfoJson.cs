using UnityEngine;
using System.Collections;

public class ActivityInfoJson : BasicJson {

	public int id{get;set;}	//所属活动id//
	public int aid{get;set;}		//活动id//
	public int type{get;set;}	//活动类型//
	
	//点击活动左侧按钮请求的构造函数//
	public ActivityInfoJson(int id,int aid,int type)
	{
		this.id = id;
		this.aid = aid;
		this.type = type;
	}
}
