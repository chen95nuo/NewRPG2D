using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityInfoResultJson : ErrorJson {
	
	public string name{get;set;}		//公告名称//
	public string content{get;set;}	//公告内容//
	
	public List<ActivityInfoExchangeElement> exActs;	//兑换型活动内容//
}

public class ActivityInfoExchangeElement
{
	public int id	{get;set;}	//兑换列表id//
	
	public string exchangeContext{get;set;}		//兑换描述//
	public int exchangeId{get;set;}		//目标资源id//
	public int exchangeType {get;set;}	//目标资源类型id:1材料，2装备，3英雄卡，4主动技能，5被动技能，6金币，7钻石，8符文，9体力//
	public int exchangeNum{get;set;}		//目标资源的数量//
	
	public int sole{get;set;}		//兑换类型：0任意兑换，1只能兑换一次//
	public int pSole{get;set;}		//玩家兑换次数//
	public int sell{get;set;}		//是否可以兑换(0不能兑换,1兑换,2已过期,3物品不足)//
	
	public List<ActivityDElement> ade;
}

public class ActivityDElement
{
	public int needId{get;set;}		//玩家已有资源id//
	public int needType{get;set;}	//玩家已有资源类型//
	public int needNum{get;set;}		//需要玩家资源的数量//
	public int curNeedNum{get;set;}	//当前玩家拥有的资源数量
}


