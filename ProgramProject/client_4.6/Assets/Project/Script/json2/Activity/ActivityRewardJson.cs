using UnityEngine;
using System.Collections;

public class ActivityRewardJson : BasicJson {

	public int taskId{get;set;}	//任务id//
	
	public ActivityRewardJson(int taskid)
	{
		this.taskId = taskid;
	}
}
