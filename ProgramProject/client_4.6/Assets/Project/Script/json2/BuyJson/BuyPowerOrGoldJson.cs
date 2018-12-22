using UnityEngine;
using System.Collections;

public class BuyPowerOrGoldJson : BasicJson {

    public int type;		//购买的物品的类型 ·1 金币， 2 体力,  3 购买扫荡券， 4 购买战斗次数, 5 购买冷却时间 ,6pvp进入次数,7购买vip礼包 8商城黑市荣誉商店购买商品 9购买位置//
	public int jsonType;	//购买请求的类型 1 界面信息， 2 购买//
	public int costType;	//花费类型 1 水晶， 2 金币//
	public int sweepTimes;	//点击扫荡的次数（两个按钮，上面的按钮是扫n次，下面的是1）//
	public int md;			//关卡id，请求挑战次数时用//
	
	//购买冷却时间数据//
	public int cdType;		//冷却类型： 1 pk, 2 maze, 3 event(异世界，活动副本)//
	public int eventId;		// 如果请求购买冷却的类型为副本， 则需要副本id//
	
	public int giftId;		//礼包id//

    public int shopType;    //1商城 2黑市 3荣誉商城//
    public int goodsId;     //id
    public int number;      //商城购买数量//

    public BuyPowerOrGoldJson() { }

	public BuyPowerOrGoldJson(int jsonT, int t, int costT)
	{
		this.jsonType = jsonT;
		this.type = t;
		this.costType = costT;
	}
	
	public BuyPowerOrGoldJson(int jsonT, int t, int costT, int sweepT, int missionId = 0, int cdT = 0, int copyId = 0)
	{
		this.jsonType = jsonT;
		this.type = t;
		this.costType = costT;
		this.sweepTimes = sweepT;
		this.md = missionId;
		this.cdType = cdT;
		this.eventId = copyId;
		
	} 
	
	//花费水晶购买vip礼包的构造函数//
	public BuyPowerOrGoldJson(string temp,int t, int jsonT, int costT, int gId)
	{
		this.type = t;
		this.jsonType = jsonT;
		this.costType = costT;
		this.giftId = gId;
	}

    public void PackageShopJson(int type, int jsonType, int shopType, int goodsId, int num = 1)
    {
        this.type = type;
        this.jsonType = jsonType;
        this.shopType = shopType;
        this.goodsId = goodsId;
        this.number = num;
    }

}
