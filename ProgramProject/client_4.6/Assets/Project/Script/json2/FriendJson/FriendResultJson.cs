using UnityEngine;
using System.Collections.Generic;

public class FriendResultJson : ErrorJson
{
	public List<FriendElement> list;
	public int power;
	public int times;//==今日已经使用的邀请好友次数==//
	public int cost;//==下一次邀请需要消耗的金币==//
	public int buyNum;//==购买次数==//
}
