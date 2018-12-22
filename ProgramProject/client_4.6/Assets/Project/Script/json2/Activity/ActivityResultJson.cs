using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityResultJson : ErrorJson {

	public List<ActivityElement> acts;
	
}

public class ActivityElement
{
	public int id	{get;set;}
	public string name	{get;set;}
	public int type {get;set;}	//面板类型:1-激活码 2-7天奖励 3-等级奖励 4-精彩兑换 5-vip礼包 6-聚宝盆 7-游戏公告 8-大风车 9-限时神将//
	public int t {get;set;}	//0没有可领的奖励，1有可领奖励//
	public int weight {get;set;}	//权值//
}


