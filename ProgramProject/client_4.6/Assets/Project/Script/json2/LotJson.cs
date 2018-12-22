using UnityEngine;
using System.Collections;

public class LotJson : BasicJson {

	/**
	 * 抽卡方式:
	 * 1友情抽卡
	 * 2钻石抽卡
	 * 3钻石十连抽
	 * 4魔法抽卡
	 * 5女神卡
	 * 6北欧卡
	 * 7希腊卡
	 * 8东亚卡
	 * 9中国卡
	 * 10至尊包
	 * 11活动免费抽卡
	 * 12活动钻石抽卡
	 */
	public int t;
	
	//活动抽卡卡库//
	public int cs;
	
	public LotJson(int t)
	{
		this.t=t;
	}
	
	public LotJson(int t,int cs)
	{
		this.t = t;
		this.cs = cs;
	}
}
