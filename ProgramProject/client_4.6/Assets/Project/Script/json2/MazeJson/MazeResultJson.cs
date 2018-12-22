using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Maze result json.
/// cuixl
/// </summary>
public class MazeResultJson : ErrorJson {
	//获取的物品的类型：1 治疗， 2 号角， 3 炸弹， 4 金币，5精英战斗，6普通战斗，7boss//
	public int type;		
	//当类型是金币或经验时，表示数量，道具，表示id, 战斗 为0 //
	public int i;
	//迷宫编号//
	public int td;
	//迷宫进度//
	public int state;
	//今天进入迷宫的次数//
	public int num;
	//已经解锁的迷宫的id list id-type(0未解锁， 1解锁)//
	public List<string> s;
	//今天进入的迷宫，格式：迷宫id-进度-次数-付费次数，迷宫id-进度-次数-付费次数//
	public string maze;
	//掉落经验时。同时返回玩家的等级//
	public int lv;
	//体力表示，0 不扣，1扣除体力//
	public int t;
	
	//最新选中的迷宫的id，即进入后选中该迷宫//
	public int mId;
	
	//冷却时间--请求所有迷宫列表时服->客//
	public int cdtime;
	
	//如果该位置没有装备卡牌，则为null//
	//public List<PackElement> pes;
	
	//card-blood&card-blood&card-blood//
	public string cb;
	
	//最大血量card-maxHp&card-maxHp//
	public string mcb;
	
	//血瓶数量//
	public int number;
	
	public string mazeWish;//已经许愿的物品//
	
	public string mazeBossDrop;//可以许愿的物品//
}
