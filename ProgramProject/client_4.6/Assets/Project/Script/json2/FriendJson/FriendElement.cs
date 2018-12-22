using UnityEngine;
using System.Collections;

public class FriendElement
{
	/**好友等级**/
	public int level;
	/**好友名字**/
	public string name;
	/**合体技Id**/
	public int unit;
	/**上次登录时间,yyyy-MM-dd HH:mm:ss**/
	public string login;
	/**好友状态:好友状态:0未赠不可收,1已删除,2未赠可收,3未赠已收,4已赠不可收,5已赠可收,6已赠已收
	 * 申请状态:0已经是好友,1未申请,2已申请**/
	public int t;
	
	//==玩家id==//
	public int pid;
	//==玩家icon==//
	public string icon;
	//==玩家战力==//
	public int bp;
}
