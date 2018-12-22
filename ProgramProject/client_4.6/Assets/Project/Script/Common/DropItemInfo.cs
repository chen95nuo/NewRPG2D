using UnityEngine;
using System.Collections;

public class DropItemInfo {
//用来存储掉落物品的信息//
	
	public int itemType;				//物品的类型 1:item  2:equip  3:card  4:skill  5:passiveSkill//
	public int itemId;					//物品在表中的id//
	public int itemNum;					//该物品的数量//
	public int itemLevel;				//掉落的物品的等级//
	public int itemStar;				//掉落物品的星级//
	
	public DropItemInfo(int type, int id, int num, int lv){
		this.itemType = type;
		this.itemId = id;
		this.itemNum = num;
		this.itemLevel = lv;
		switch(type){
		case STATE.DROPS_TYPE_ITEM:
			itemStar = ItemsData.getData(id).star;
			break;
		case STATE.DROPS_TYPE_EQUIP:
			itemStar = EquipData.getData(id).star;
			break;
		case STATE.DROPS_TYPE_CARD:
			itemStar = CardData.getData(id).star;
			break;
		case STATE.DROPS_TYPE_SKILL:
			itemStar = SkillData.getData(id).star;
			break;
		case STATE.DROPS_TYPE_PASSIVESKILL:
			itemStar = PassiveSkillData.getData(id).star;
			break;
		}
	
		
	}

}
