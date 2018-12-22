using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SweepCardJson {
	public int playerExp;		//军团经验//
	public int cardExp;			//人物经验//
	
	public List<string> ds;		//掉落物品 type-id,num 当掉落物品为卡牌时，num表示等级，当掉落物品为item时表示数量，其他的没有num//
}
