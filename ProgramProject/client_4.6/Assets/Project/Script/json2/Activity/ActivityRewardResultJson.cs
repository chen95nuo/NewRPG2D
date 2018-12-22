using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityRewardResultJson : ErrorJson {
	
	public List<TaskElement> tes;	//任务列表//
	public int active;					//活跃度//
	public string activeState;		//活跃度礼包领取状态,格式：活跃度－领取状态（0-未开启，1-可领取，2-已领取）,活跃度－领取状态，活跃度－领取状态，活跃度－领取状态//
}
