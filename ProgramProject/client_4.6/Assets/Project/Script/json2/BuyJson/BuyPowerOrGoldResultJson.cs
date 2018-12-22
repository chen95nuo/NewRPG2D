using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuyPowerOrGoldResultJson : ErrorJson {

	public int crystal;			//水晶数（请求界面信息时，表示购买需要花费的水晶数， 购买后，表示玩家剩余的水晶数）//
	public int num;				//界面请求表示购买的金币或体力的个数， 若是够买完成后，表示玩家剩余的金币或体力//
	public int times;			//当天剩余的购买次数//
	public int md;				//关卡id  商城购买时表示商城购买物品id//
    public string goodsInfo;    //购买的商品//
	public List<string> ids;		//可以购买的vip礼包ids//
}
