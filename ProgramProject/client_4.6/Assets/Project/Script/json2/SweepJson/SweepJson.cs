using UnityEngine;
using System.Collections;

public class SweepJson : BasicJson {
	/**
	 * 点击扫荡按钮,客->服
	 */
	public int sweepNum;		//扫荡次数//
	public int bNum;			//场次//
	public int md;				//关卡id//
	
	public SweepJson(int md, int sweepN, int bN)
	{
		this.md = md;
		this.bNum = bN;
		this.sweepNum = sweepN;
	}
}
