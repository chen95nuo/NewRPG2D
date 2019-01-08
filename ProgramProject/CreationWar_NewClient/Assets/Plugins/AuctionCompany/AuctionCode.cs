using System.Collections;

public enum AuctionCompanyType : short
{
    /**
	 * 一口价拍卖
	 */
	FixedPriceAuction = 0,
	/**
	 * 拍卖搜索
	 */
	AuctionSearch = 1,
	/**
	 * 购买拍卖品
	 */
	BuyAuctions = 2,
    /**
	 * 玩家的拍卖信息
	 */
	PlayerAuctionInfo = 3,
    /**
	 * 购买拍卖次数
	 */
	BuyAuctionSlot = 5,
}

public enum AuctionCompanyParams : short
{
    /// <summary>
    /// 拍卖行子协议
    /// </summary>
	AuctionCompanyType = 0,
    /// <summary>
    /// 物品ID和数量
    /// </summary>
	ItemIDAndCount = 1,
    /// <summary>
    /// 一口价的固定价格
    /// </summary>
	FixedPrice = 2,
    /// <summary>
    /// 竞拍时长
    /// </summary>
	AuctionTime = 3,
    /// <summary>
    /// 物品最小等级
    /// </summary>
	MinLvl = 4,
    /// <summary>
    /// 物品最大等级
    /// </summary>
	MaxLvl = 5,
    /// <summary>
    /// 物品品质
    /// </summary>
	ItemQuality = 6,
    /// <summary>
    /// 装备类型
    /// </summary>
	EquipType = 7,
    /// <summary>
    /// 材料类型
    /// </summary>
	MatType = 8,
    /// <summary>
    /// 购买的拍卖品的id,即表的主键
    /// </summary>
	AuctionID = 9,
    /// <summary>
    /// 一次性可拍卖的次数
    /// </summary>
	AuctionCount = 10,
    /// <summary>
    /// 剩余拍卖次数
    /// </summary>
	UsedAuctionCount = 11,
    /// <summary>
    /// 一次所购买的拍卖位数量
    /// </summary>
	AuctionSlotCount = 12,
}