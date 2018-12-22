using UnityEngine;
using System.Collections;

public class ActiveJson : BasicJson {

	public int active{get;set;}		//当前活跃度值//
	
	public ActiveJson(int active)
	{
		this.active = active;
	}
}
