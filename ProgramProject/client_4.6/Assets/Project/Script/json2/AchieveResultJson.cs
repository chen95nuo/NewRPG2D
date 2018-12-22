using UnityEngine;
using System.Collections;

public class AchieveResultJson : ErrorJson {
	public string ac;//==当前成就,格式:10101%1-10102%0-10103%0-10104%0,"-"分割多个成就,"%"分割成就id和是否已领取,0表示未领取,1表示已领取==//
}
