using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardInfoResultJson : ErrorJson
{
	// int value : 0 - no tip, 1 - N tip, 2 - UP tip
	public int ps1; // passiveSkill
	public int ps2;
	public int ps3;
	public int equip1;
	public int equip2;
	public int equip3;

    public int diamond;//突破需要的金罡心数量//
    public int pd; //玩家当前拥有的金罡心数量//
    public int cardN;//突破需要的卡牌数量//
    public int pCardN;//玩家拥有此卡牌的数量//
    public int multCard;//同星级的万能突破卡//

    public List<PackElement> pes; //突破卡牌索引/
	
}

