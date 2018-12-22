using UnityEngine;
using System.Collections;

public class MazeItem : MonoBehaviour{

	public int id;			//迷宫的在表中的id//
	public int state;		//今天进入该迷宫的位置//
	public int num;			//今天进入迷宫的次数//
	public int payNum;		//今天进入迷宫的付费次数0 没有付费，1 已经付费//
	public int resetCost;	//重置进入次数花费的钻石数//
	public int mark;		//0未解锁，1已解锁//
	public UISprite Icon;
	public UISprite IconBlack;
	public UILabel Name;
	public UILabel Locked;
	public UIButtonMessage rewardInfo;//奖励信息按钮//
}
