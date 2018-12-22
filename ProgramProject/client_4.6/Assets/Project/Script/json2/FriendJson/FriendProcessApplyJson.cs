using UnityEngine;
using System.Collections;

public class FriendProcessApplyJson : BasicJson {

	//==1同意,2拒绝==//
	public int t;
	public int i;
	
	public FriendProcessApplyJson(int t,int i)
	{
		this.t=t;
		this.i=i;
	}
}
