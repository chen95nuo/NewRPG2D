using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskResultJson : ErrorJson {
	
	public List<TaskElement> tes;
	public int active;					//活跃度//
	public string activeState;		//活跃度礼包领取状态,格式：活跃度－领取状态（0-未开启，1-可领取，2-已领取）,活跃度－领取状态，活跃度－领取状态，活跃度－领取状态//
	
}

public class TaskElement
{
	public int id{get;set;}
	public string name{get;set;}		//名称//
	public string icon{get;set;}	//头像//
	public string description{get;set;}		//描述//
	public List<string> reward;	//奖励类型－id//
	public int type{get;set;}	//是否完成：0-未完成，1-完成未领取，2-完成已领取奖励//
	public int num{get;set;}	//已完成次数//
	public int sNum{get;set;}	//目标次数//
	public int unlockLevel;		//解锁等级//
	public string ulDesc;		//解锁描述//
	public int activeNum;		//活跃度//
}


