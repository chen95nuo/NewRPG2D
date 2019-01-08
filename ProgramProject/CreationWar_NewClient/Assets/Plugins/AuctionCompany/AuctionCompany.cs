using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuctionCompany : MonoBehaviour {
    public static AuctionCompany auctionCompany;
    
    [HideInInspector]
    public AuctionBuy auctionBuy;
    [HideInInspector]
    public AuctionSell auctionSell;
    [HideInInspector]
    public AuctionMy auctionMy;

    public void Awake()
    {
        auctionCompany = this;
    }

    [HideInInspector]
    public yuan.YuanMemoryDB.YuanTable auctionTable = new yuan.YuanMemoryDB.YuanTable("AuctionCompany", "id");
    [HideInInspector]
    public yuan.YuanMemoryDB.YuanTable myAuctionTable = new yuan.YuanMemoryDB.YuanTable("AuctionCompany", "id");
    /// <summary>
    /// 将服务器返回的id数组转换成列表(其他玩家的拍卖信息)
    /// </summary>
    /// <param name="ids">拍卖id数组</param>
    public void SetAuctionTable(Dictionary<short, object> auctionRows)
    {
        auctionTable.Clear();
        auctionTable.CopyToDictionary(auctionRows);
        auctionBuy.ShowAuctionItems(auctionTable);
    }

    /// <summary>
    /// 将服务器返回的id数组转换成列表（当前玩家的拍卖信息）
    /// </summary>
    /// <param name="ids">拍卖id数组</param>
    public void SetMyAuctionTable(Dictionary<short, object> auctionRows)
    {
        myAuctionTable.Clear();
        myAuctionTable.CopyToDictionary(auctionRows);
        auctionMy.ShowAuctionItems(myAuctionTable);
    }

    /// <summary>
    /// 拍卖物品
    /// </summary>
    /// <param name="itemID">物品id</param>
    /// <param name="price">拍卖价格</param>
    /// <param name="time">拍卖时长</param>
    public void SellOut(string itemID, int price, int time)
    {
//        Debug.Log("===================sell out=======" + itemID + "===" + price + "===" + time);

        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().FixedPriceAuction(itemID, price, time));
    }

    /// <summary>
    /// 刷新拍卖信息
    /// </summary>
    /// <param name="minLvl">物品最小等级</param>
    /// <param name="maxLvl">物品最小等级</param>
    /// <param name="quality">品质选项</param>
    /// <param name="equip">装备选项</param>
    /// <param name="mat">材料选项</param>
    public void RefreshAuctionInfo(int minLvl, int maxLvl, int[] quality, int[] equip, int[] mat)
    { 
        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().AuctionSearch(minLvl, maxLvl, quality, equip, mat));
    }

    /// <summary>
    /// 购买
    /// </summary>
    /// <param name="id"></param>
    public void BuyAuction(int id)
    {
        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().BuyAuctions(id));
    }

    /// <summary>
    /// 显示当前玩家的拍卖信息
    /// </summary>
    public void ShowAuctionInfo()
    {
        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().PlayerAuctionInfo());
    }

    /// <summary>
    /// 显示当前玩家的拍卖次数
    /// </summary>
    public void ShowAuctionCount()
    {
        InRoom.GetInRoomInstantiate().BuyAuctionSlot(0);
    }

    /// <summary>
    /// 购买拍卖位
    /// </summary>
    public void BuyAuctionSlot(int count)
    {
        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().BuyAuctionSlot(count));
    }
}
