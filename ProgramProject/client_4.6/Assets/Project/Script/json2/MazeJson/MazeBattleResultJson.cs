using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeBattleResultJson : ErrorJson {
	public int bNum;			//场次信息//
	
	public int md;				//战斗关卡id//
	public int td;				//迷宫编号//
	public int state;			//当前迷宫位置信息//
	public int type ;			//战斗类型，1 普通战斗， 2boss战//
	public int aF;				//addFriendValue//
	public CardJson[] cs0;		//cards0//
	public CardJson[] cs1;		//cards1//
	public int[] us0;			//unitSkillsId0//
	public int[] us1;			//unitSkillsId1//
	public string[] s;			//tape - drops//
	public int[] mes;//==怒气上限==//
	public string[] raceAtts;//==己方种族加成属性==//
	public int initE;//==初始怒气==//
}
