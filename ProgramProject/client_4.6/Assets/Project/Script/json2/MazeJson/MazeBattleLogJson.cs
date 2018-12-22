using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeBattleLogJson : BasicJson {
	public int bNum;			//场次信息
	
	public List<string> bs;		//battlelogs元素格式:动作方-合体技ID(0表示不是合体技)-站位-技能ID-目标列表(站位%血量增加值%暴击闪避标识%死亡标识,两个元素之间用&分割)//
	public int r;				//result:1胜利,2失败//
	public string gm;			//goldMul金币倍数//
	public int type;			//迷宫中战斗类型，1 普通战， 2 Boss战斗//
	public int map;				//迷宫id;//
	public string cb; //各个卡牌的血量 格式cardid-hp&cardid-hp//
//	public int state;			//迷宫要到达的位置//

}
