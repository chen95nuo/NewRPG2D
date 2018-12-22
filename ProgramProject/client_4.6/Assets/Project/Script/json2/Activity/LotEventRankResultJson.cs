using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LotEventRankResultJson : ErrorJson {
	//抽卡池id//
	public int cs;
	//积分排行榜//
	public List<string> sr;
	//积分//
	public int score;
	//排名//
	public int rank;
	//钻石//
	public int crystal;
	//活动开始时间//
	public string bt;
	//活动结束时间//
	public string et;
	//活动倒计时//
	public int ht;
	//抽卡冷却时间//
	public int lt;
	//使用钻石数量//
	public int uc;
	//距离下次抽到六星卡次数//
	public int n;
	
}