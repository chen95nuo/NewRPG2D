using UnityEngine;
//using System.Collections;
using System.Collections.Generic;

public class EventResultJson : ErrorJson {

	//进入择副本界面：id-mark(0未开启，1开启)-time(剩余时间)//
	//进入副本选择关卡界面： id(关卡id)-num(完成次数)//
	public List<string> s;
	//当前关卡对应的副本的id//
	public int id;
    //活动副本-死亡洞窟进入的次数//
    public int num;
	//冷却时间--请求某个副本选择关卡时发送给客户端//
	public int cdtime;	
}
