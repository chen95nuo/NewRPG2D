using UnityEngine;
using System.Collections;

public class FriendApplyJson : BasicJson {
	/**索引**/
	public int i;
	//==0列表申请,1搜索申请,2支援玩家申请==//
	public int s;
	
	public FriendApplyJson(int i,int isSearch)
	{
		this.i=i;
		this.s=isSearch;
	}
}
