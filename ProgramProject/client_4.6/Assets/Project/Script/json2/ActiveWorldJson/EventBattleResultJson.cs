using UnityEngine;
using System.Collections;

public class EventBattleResultJson : ErrorJson {
	
	public int bNum;//场次信息
	public int id;   	//fbeventId;
	public int eid;		//副本id//
	public CardJson[] cs0;	//cards0//
	public CardJson[] cs1;	//cards1//
	public int[] us0;		//unitSkillsId0//
	public int[] us1;		//unitSkillsId1//
	public string[] s;		//drops//
	public int[] mes;//==怒气上限==//
	public string[] raceAtts;//==己方种族加成属性==//
	public int initE;//==初始怒气==//
}
