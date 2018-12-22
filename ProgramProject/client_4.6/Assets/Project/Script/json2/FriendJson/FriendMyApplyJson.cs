using UnityEngine;
using System.Collections;

public class FriendMyApplyJson : BasicJson {
	//==0获取我的申请,1获取申请我的信息==//
	public int t;
	
	public FriendMyApplyJson(int t)
	{
		this.t=t;
	}
}
