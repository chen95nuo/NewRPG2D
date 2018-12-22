using UnityEngine;
using System.Collections;

public class ActivityGJson : BasicJson {

	public int id;		//活动id//
	public int type;	//活动类型//
	
	public ActivityGJson(int tId,int tType)
	{
		id = tId;
		type = tType;
	}
}
