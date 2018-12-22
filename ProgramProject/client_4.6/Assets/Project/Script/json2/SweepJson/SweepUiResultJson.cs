using UnityEngine;
using System.Collections;

public class SweepUiResultJson : ErrorJson {
	/**
	 *请求扫荡界面信息  服->客 
	 */
	public int power;		//扫荡一次消耗的体力值//
	public int entryTimes;	//可挑战次数//
	public int itemNum;		//扫荡券个数//
	public int sweepTimes;	//可连续挑战次数//
	public int bNum;		//场次//
	public int md;			//关卡id//
}
