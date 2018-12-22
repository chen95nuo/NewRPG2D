using UnityEngine;
using System.Collections;

public class FriendJson : BasicJson {
	/**0查看,1收体力,2送体力**/
	public int t;
	/**索引**/
	public int i;
	
	public FriendJson(int type,int index)
	{
		t=type;
		i=index;
	}
}
