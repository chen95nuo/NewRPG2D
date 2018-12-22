using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PkBattleResultJson : ErrorJson {
	public int bNum;			//场次信息//

	public CardJson[] cs0;
	public CardJson[] cs1;
	public int[] us0;
	public int[] us1;
	public int pkId;
	public int[] mes;//==怒气上限==//
	public int[] bps;		//==双方战力,战力大的一方先出手==//
	public List<string> ds;	//掉落物品id-个数//
	public string[] ns;//==双方名字==//
	public int[] lvs;//==双方等级==//
	public string[] runes;
	public string[] raceAtts1;//==己方种族加成属性==//
	public string[] raceAtts2;//==对方种族加成属性==//
	public int[] initEs;//==初始怒气==//
}
