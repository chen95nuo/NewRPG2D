using UnityEngine;
using System.Collections;

public class SpriteWroldItem {
	
	/**
	 * 冥想获得的物品的数据的存储类
	 */
	
	//物品的类型 1 废品， 2 被动技能， 3 item//
	public int type;
	//物品在表中的id//
	public int id;
	//物品的数量//
	public int num;
	
	
	public SpriteWroldItem(int iType, int itemId, int inum){
		this.type = iType;
		this.id = itemId;
		this.num = inum;
	}
}
