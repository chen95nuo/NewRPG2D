using UnityEngine;
using System.Collections;

public class GameBoxElement  {

	public int goodsType;			//物品的类型 1 item, 2 equip, 3 card, 4 skill, 5 passiveSkill, 6 gold, 7 exp, 8 crystal, 9 rune, 10 体力值, 11 友情值//
	public int goodsId;				//物品的id//
	public int num;					//数量//
	public int dropType;			//掉落类型， 1 固定掉落，0 随机掉落//
}
