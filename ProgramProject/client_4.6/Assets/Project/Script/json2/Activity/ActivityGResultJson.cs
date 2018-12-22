using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityGResultJson : ErrorJson {
	
	public List<ActivityGElement> age;	//公告活动列表//
	
}

public class ActivityGElement
{
	public int id;		//所属活动id//
	public int aid;		//活动id//
	public string name;
	public int hot;		//1.hot显示，0.hot不显示//
	public int type;
}
