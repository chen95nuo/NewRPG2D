using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SweepResultJson : ErrorJson {

	/**
	 * 点击扫荡按钮， 服->客
	 */
	//站前//
	public int lv0;				//军团等级//
	public List<string> cs0;	//卡组信息 格式： cardId-level-curExp-maxExp-hp-atk-def//
	public int ce0;				//军团经验值//
	
	
	//战后//
	public int lv1;
	public List<string> cs1;
	public int ce1;				//军团经验值//
	
	//扫荡信息//
	public List<SweepCardJson> sweepInfo;		//扫荡之后的军团，人物以及掉落物品//
	public int power;			//扫荡一次消耗的体力值//
	public int entryTimes;		//可挑战次数//
	public int itemNum;			//扫荡券个数//
	public int sweepTimes;		//可连续挑战次数//
	
	public int power0;			//升级前军团体力值//
	public int power1;			//升级后军团体力值//
	
	//模块解锁id-是否解锁（0未解锁， 1解锁）//
	public string[] s;
	
}
