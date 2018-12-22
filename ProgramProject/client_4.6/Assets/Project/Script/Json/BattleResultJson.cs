using UnityEngine;
using System.Collections;

public class BattleResultJson : ErrorJson {
	
	public int bNum;//场次信息
	
	public int md;			//missionId//
	public int aF;			//addFriendValue//
	public CardJson[] cs0;	//cards0//
	public CardJson[] cs1;	//cards1//
	public string[] ds;		//drops//
	public int[] us0;		//unitSkillsId0//
	public int[] us1;		//unitSkillsId1//
	public int bs;			//bouns奖励标识（0未领取过， 1领取过）//
	public int t;//==1有剧情,0无剧情==//
	public CardJson[] fs;//==好友战斗卡组角色==//
	public string fr;//==好友符文Id==//
	public int[] mes;//==怒气上限==//
	public string[] raceAtts;//==己方种族加成属性==//
	public int initE;//==初始怒气==//
}
