using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;
using KodGames.ClientClass;
using KodGames.ClientHelper;
using UnityEngine;

internal class Request
{
	public const int InvalidID = IDAllocator.InvalidID;	// 无效的ID标记

	private static IDAllocator idAllc = new IDAllocator();	// ID分配器

	public static int GetAnNewId()
	{
		return idAllc.NewID();
	}

	public Request()
	{
		id = idAllc.NewID();
	}

	// ID.
	private int id;

	public int ID { get { return id; } }

	// Information.
	public override string ToString()
	{
		return string.Format("{0} ID:{1:d} Executed:{2} ExecResult:{3} IsResponded:{4} Combined:{5} HasResponse:{6}",
			this.GetType(), ID, IsExecuted, ExecResult, IsResponded, IsCombined, HasResponse);
	}

	public virtual bool CheckTimeout
	{
		get { return true; }
	}

	public virtual bool MutuallyExclusive
	{
		get { return true; }
	}

	// If has response.
	public virtual bool HasResponse
	{
		get { return true; }
	}

	// Waiting response flag.
	public virtual bool WaitingResponse
	{
		get { return true; }
	}

	public virtual bool Combinable
	{
		get { return false; }
	}

	private bool combined = false;

	public bool IsCombined
	{
		get { return combined; }
		set { combined = value; }
	}

	public virtual bool CombineWithPrevRequest(Request request)
	{
		return this.GetType() == request.GetType();
	}

	public virtual bool Delayable
	{
		get { return false; }
	}

	public virtual bool CanResend
	{
		get { return true; }
	}

	// Is discarded flag.
	private bool isDiscarded = false;

	public bool IsDiscarded
	{
		get { return isDiscarded; }
		set { isDiscarded = value; }
	}

	// Is executed.
	private bool executed = false;

	public bool IsExecuted { get { return executed; } }

	private bool execResult = false;

	public bool ExecResult
	{
		get { return execResult; }
		set { execResult = value; }
	}

	// Get responded state, only valid for request that waiting for response.
	// The flag to mark response state to this request.
	private bool responded;

	public bool IsResponded
	{
		get { return responded; }
		set { responded = value; }
	}

	// Execute.
	public virtual bool Execute(IBusiness bsn)
	{
		if (IsExecuted)
			Debug.LogError("Execute executed request " + this.ToString());
		if (IsDiscarded)
			Debug.LogError("Execute discarded request " + this.ToString());
		if (IsCombined)
			Debug.LogError("Execute combined request " + this.ToString());

		executed = true;
		return true;
	}
}

/// <summary>
/// Connect IS req.
/// </summary>
internal class ConnectISReq : Request
{
	public delegate void OnIsConnectedSuccess();
	public delegate void OnIsConnectedFailed(int errCode, string errMsg);

	private OnIsConnectedSuccess onIsConnectedSuccessDel;

	public OnIsConnectedSuccess OnIsConnectedSuccessDel { get { return onIsConnectedSuccessDel; } }

	private OnIsConnectedFailed onIsConnectedFailedDel;

	public OnIsConnectedFailed OnIsConnectedFailedDel { get { return onIsConnectedFailedDel; } }

	private string hostname;
	private int port;
	private NetType netType;
	private int areaID;
	private int newAreaID;

	public int AreaID { get { return this.areaID; } }
	// 不支持自动重发
	public override bool CanResend { get { return false; } }

	public ConnectISReq(string hostname, int port, NetType netType, int areaID, int newAreaID, OnIsConnectedSuccess onIsConnectedSuccessDel, OnIsConnectedFailed onIsConnectedFailedDel)
	{
		this.hostname = hostname;
		this.port = port;
		this.netType = netType;
		this.areaID = areaID;
		this.newAreaID = newAreaID;
		this.onIsConnectedSuccessDel = onIsConnectedSuccessDel;
		this.onIsConnectedFailedDel = onIsConnectedFailedDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ConnectIS(hostname, port, netType, newAreaID, ID);
	}
}

/// <summary>
/// Query init info req.
/// </summary>
internal class QueryInitInfoReq : Request
{
	public override bool WaitingResponse { get { return false; } }

	// 不支持自动重发
	public override bool CanResend { get { return false; } }

	public QueryInitInfoReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		// 埋点
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.QueryInitInfo);

		return bsn.QueryInitInfo(ID);
	}
}

#region Position

internal class QueryPositionListReq : Request
{
	public QueryPositionListReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryPositionList(ID);
	}
}

//开启阵容
internal class OpenPositionReq : Request
{
	private int positionId;

	public OpenPositionReq(int positionId)
	{
		this.positionId = positionId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.OpenPosition(ID, positionId);
	}
}

internal class SetMasterPositionReq : Request
{
	private int positionId;

	public int PositionId { get { return positionId; } }

	public SetMasterPositionReq(int positionId)
	{
		this.positionId = positionId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SetMasterPosition(ID, positionId);
	}
}

internal class EmBattleReq : Request
{
	private int positionId;
	private int locationId1;
	private int locationId2;

	public override bool WaitingResponse { get { return false; } }

	public EmBattleReq(int positionId, int locationId1, int locationId2)
	{
		this.positionId = positionId;
		this.locationId1 = locationId1;
		this.locationId2 = locationId2;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(positionId);
		var avatarLocation1 = PlayerDataUtility.GetAvatarLocationByLocationId(SysLocalDataBase.Inst.LocalPlayer, positionId, locationId1);
		var avatarLocation2 = PlayerDataUtility.GetAvatarLocationByLocationId(SysLocalDataBase.Inst.LocalPlayer, positionId, locationId2);

		var locationPair1 = position.GetPairByLocationId(locationId1);
		var locationPair2 = position.GetPairByLocationId(locationId2);

		locationPair1.LocationId = locationId2;
		locationPair2.LocationId = locationId1;

		// Change LocationId.
		if (avatarLocation1 != null)
			avatarLocation1.LocationId = locationId2;
		if (avatarLocation2 != null)
			avatarLocation2.LocationId = locationId1;

		if (position.EmployLocationId == locationId1)
			position.EmployLocationId = locationId2;
		else if (position.EmployLocationId == locationId2)
			position.EmployLocationId = locationId1;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().OnEmBattleSuccess();

		return bsn.EmBattle(ID, positionId, locationId1, locationId2);
	}
}

internal class ChangeLocationReq : Request
{
	private string guid;

	public string Guid { get { return guid; } }

	private int resourceId;

	public int ResourceId { get { return resourceId; } }

	private int positionId;

	public int PostitionId { get { return positionId; } }

	private int location;

	public int Location { get { return location; } }

	private string offGuid;

	public string OffGuild { get { return offGuid; } }

	private int index;

	public ChangeLocationReq(string guid, int resourceId, string offGuid, int positionId, int location)
		: this(guid, resourceId, offGuid, positionId, location, 0)
	{
	}

	public ChangeLocationReq(string guid, int resourceId, string offGuid, int positionId, int location, int index)
	{
		this.guid = guid;
		this.resourceId = resourceId;
		this.offGuid = offGuid;
		this.positionId = positionId;
		this.location = location;
		this.index = index;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ChangeLocation(ID, guid, resourceId, offGuid, positionId, location, index);
	}
}

internal class OneClickPositionOffReq : Request
{
	private int positionId;

	public int PostitionId { get { return positionId; } }

	public OneClickPositionOffReq(int positionId)
	{
		this.positionId = positionId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.OneClickPositionOff(ID, positionId);
	}
}


#endregion Position

#region Partner

//开启助阵小伙伴
internal class PartnerOpenReq : Request
{
	private int partnerId;

	public int PartnerId { get { return partnerId; } }

	public PartnerOpenReq(int partnerId)
	{
		this.partnerId = partnerId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.PartnerOpen(ID, partnerId);
	}
}

internal class PartnerSetupReq : Request
{
	private int positionId;

	public int PositionId
	{
		get { return positionId; }
	}

	private int partnerId;

	public int PartnerId
	{
		get { return partnerId; }
	}

	private string avatarOffGuid;

	public string AvatarOffGuid
	{
		get { return avatarOffGuid; }
	}

	private string avatarOnGuid;

	public string AvatarOnGuid
	{
		get { return avatarOnGuid; }
	}

	public PartnerSetupReq(int positionId, int partnerId, string avatarOffGuid, string avatarOnGuid)
	{
		this.positionId = positionId;
		this.partnerId = partnerId;
		this.avatarOffGuid = avatarOffGuid;
		this.avatarOnGuid = avatarOnGuid;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.PartnerSetup(ID, positionId, partnerId, avatarOnGuid);
	}
}

#endregion Partner

#region Hire

internal class QueryDinerListReq : Request
{
	public QueryDinerListReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryDinerList(ID);
	}
}

//门客馆雇佣
internal class HireDinerReq : Request
{
	private int dinerId;

	public int DinerId { get { return dinerId; } }

	private int qualityType;

	public int QualityType { get { return qualityType; } }

	public HireDinerReq(int qualityType, int dinerId)
	{
		this.dinerId = dinerId;
		this.qualityType = qualityType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.HireDiner(ID, qualityType, dinerId);
	}
}

internal class RenewDinerReq : Request
{
	private int dinerId;

	public int DinerId { get { return dinerId; } }

	private int qualityType;

	public int QualityType { get { return qualityType; } }

	public RenewDinerReq(int qualityType, int dinerId)
	{
		this.dinerId = dinerId;
		this.qualityType = qualityType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.RenewDiner(ID, qualityType, dinerId);
	}
}

internal class FireDinerReq : Request
{
	private int dinerId;

	public int DinerId { get { return dinerId; } }

	private int qualityType;

	public int QualityType { get { return qualityType; } }

	public FireDinerReq(int qualityType, int dinerId)
	{
		this.dinerId = dinerId;
		this.qualityType = qualityType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.FireDiner(ID, qualityType, dinerId);
	}
}

//门客馆刷新
internal class RefreshDinerListReq : Request
{
	private int bagId;

	public int BagId { get { return bagId; } }

	public RefreshDinerListReq(int bagId)
	{
		this.bagId = bagId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.RefreshDinerList(ID, bagId);
	}
}

#endregion Hire

#region Avatar

// Avatar Power Up.
internal class AvatarLevelUpReq : Request
{
	public AvatarLevelUpReq(string avatarGUID, bool levelUpType)
	{
		this.avatarGUID = avatarGUID;
		this.levelUpType = levelUpType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.AvatarLevelUp(ID, avatarGUID, levelUpType);
	}

	private string avatarGUID;

	public string AvatarGUID
	{
		get { return this.avatarGUID; }
	}

	private bool levelUpType;

	public bool LevelUpType
	{
		get { return levelUpType; }
	}
}

// Avatar BreakThought.
internal class AvatarBreakthoughtReq : Request
{
	private string avatarGUID;
	private List<string> destroyAvatarGUIDs;

	public string AvatarGUID
	{
		get { return this.avatarGUID; }
	}

	public AvatarBreakthoughtReq(string avatarGUID, List<string> destroyAvatarGUIDs)
	{
		this.avatarGUID = avatarGUID;
		this.destroyAvatarGUIDs = destroyAvatarGUIDs;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.AvatarBreakthought(ID, avatarGUID, destroyAvatarGUIDs);
	}
}

// Avatar Meridian.
internal class ChangeMeridianReq : Request
{
	private int meridianId;

	public int MeridianId
	{
		get { return meridianId; }
	}

	private string avatarGuid;

	public string AvatarGuid
	{
		get { return avatarGuid; }
	}

	public ChangeMeridianReq(int meridianId, string avatarGuid)
	{
		this.meridianId = meridianId;
		this.avatarGuid = avatarGuid;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ChangeMeridian(ID, meridianId, avatarGuid);
	}
}

internal class SaveMeridianReq : Request
{
	private string avatarGuid;

	public string AvatarGuid
	{
		get { return avatarGuid; }
	}

	private bool saveOrNot;

	public bool SaveOrNot
	{
		get { return saveOrNot; }
	}

	private int meridianId;

	public int MeridianId
	{
		get { return meridianId; }
	}

	public SaveMeridianReq(string avatarGuid, bool saveOrNot, int meridianId)
	{
		this.avatarGuid = avatarGuid;
		this.saveOrNot = saveOrNot;
		this.meridianId = meridianId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SaveMeridian(ID, avatarGuid, saveOrNot, meridianId);
	}
}

// Avatar Domineer.
internal class ChangeDomineerReq : Request
{
	private string avatarGuid;
	private List<string> destroyAvatarGuids;

	public ChangeDomineerReq(string avatarGuid, List<string> destroyAvatarGuids)
	{
		this.avatarGuid = avatarGuid;
		this.destroyAvatarGuids = destroyAvatarGuids;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ChangeDomineer(ID, avatarGuid, destroyAvatarGuids);
	}
}

internal class SaveDomineerReq : Request
{
	private string avatarGuid;
	private bool isSave;

	public SaveDomineerReq(string avatarGuid, bool isSave)
	{
		this.avatarGuid = avatarGuid;
		this.isSave = isSave;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SaveDomineer(ID, avatarGuid, isSave);
	}
}

#endregion Avatar

#region Equipment

internal class EquipmentBreakthoutReq : Request
{
	public EquipmentBreakthoutReq(string equipGUID, List<string> destroyEquipGUIDs)
	{
		this.equipGUID = equipGUID;
		this.destroyEquipGUIDs = destroyEquipGUIDs;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.EquipBreakthought(ID, equipGUID, destroyEquipGUIDs);
	}

	private string equipGUID;

	public string EquipGUID
	{
		get { return equipGUID; }
	}

	private List<string> destroyEquipGUIDs;

	public List<string> DestroyEquipGUIDs
	{
		get { return destroyEquipGUIDs; }
	}
}

internal class EquipmentLevelUpReq : Request
{
	private string equipGuid;

	public string EquipGuid
	{
		get { return equipGuid; }
	}

	private bool levelType;

	public bool LevelType
	{
		get { return levelType; }
	}

	public EquipmentLevelUpReq(string equipGuid, bool levelType)
	{
		this.equipGuid = equipGuid;
		this.levelType = levelType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.EquipLevelUp(ID, equipGuid, levelType);
	}
}

#endregion Equipment

#region Skill

internal class SkillLevelUpReq : Request
{
	private string skillGUID;
	private List<string> destroySkillGUIDs;

	public SkillLevelUpReq(string skillGUID, List<string> destroySkillGUIDs)
	{
		this.skillGUID = skillGUID;
		this.destroySkillGUIDs = destroySkillGUIDs;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SkillLevelUp(ID, skillGUID, destroySkillGUIDs);
	}
}

#endregion Skill

#region Shop

//QueryGoods
internal class QueryGoodsReq : Request
{
	public delegate void OnQueryGoodsSuccessDelegate();

	private OnQueryGoodsSuccessDelegate queryGoodsSuccessDel;

	public OnQueryGoodsSuccessDelegate QueryGoodsSuccessDel
	{
		get { return queryGoodsSuccessDel; }
	}

	public QueryGoodsReq(OnQueryGoodsSuccessDelegate queryGoodsSuccessDel)
	{
		this.queryGoodsSuccessDel = queryGoodsSuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryGoodsList(ID);
	}
}

//BuyGoods
//购买物品 * 比武场兑换物品
internal class BuyGoodsReq : Request
{
	public delegate void OnBuyGachaKeySuccessDelegate(int goodsId, int amount, CostAndRewardAndSync costAndRewardAndSync);
	public delegate void OnBuyGoodsSuccessDelegate(int goodsId, int amount, KodGames.ClientClass.Reward reward, List<KodGames.ClientClass.Cost> costs);
	public delegate void OnBuyGoodsFailed_UpdateStatus(int goodsId);

	private OnBuyGachaKeySuccessDelegate buyGachaKeySuccessDel;

	public OnBuyGachaKeySuccessDelegate BuyGachaKeySuccessDel { get { return buyGachaKeySuccessDel; } }

	private OnBuyGoodsSuccessDelegate buyGoodsSuccessDel;

	public OnBuyGoodsSuccessDelegate BuyGoodsSuccessDel { get { return buyGoodsSuccessDel; } }

	private OnBuyGoodsFailed_UpdateStatus buyGoodsFailed_UpdateStatusDel;

	public OnBuyGoodsFailed_UpdateStatus BuyGoodsFailed_UpdateStatusDel { get { return buyGoodsFailed_UpdateStatusDel; } }

	private int goodsId;

	public int GoodsId { get { return goodsId; } }

	private int amount;

	public int Amount { get { return amount; } }

	private int statusIndex;

	public BuyGoodsReq(int goodsId, int amount, int statusIndex, OnBuyGachaKeySuccessDelegate buyGachaKeySuccessDel)
	{
		this.goodsId = goodsId;
		this.amount = amount;
		this.statusIndex = statusIndex;
		this.buyGachaKeySuccessDel = buyGachaKeySuccessDel;
	}

	public BuyGoodsReq(int goodsId, int amount, int statusIndex, OnBuyGoodsSuccessDelegate buyGoodsSuccessDel, OnBuyGoodsFailed_UpdateStatus buyGoodsFailed_UpdateStatusDel)
	{
		this.goodsId = goodsId;
		this.amount = amount;
		this.statusIndex = statusIndex;
		this.buyGoodsSuccessDel = buyGoodsSuccessDel;
		this.buyGoodsFailed_UpdateStatusDel = buyGoodsFailed_UpdateStatusDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		var record = new GoodRecord();
		record.GoodsID = goodsId;
		record.Amount = amount;

		return bsn.BuyGoods(ID, record, statusIndex);
	}
}

internal class BuyAndUseReq : Request
{
	private int goodsId;

	public int GoodsId { get { return goodsId; } }

	private System.Action<KodGames.ClientClass.Reward> buyAndUseCallBack;

	public System.Action<KodGames.ClientClass.Reward> BuyAndUseCallBack { get { return buyAndUseCallBack; } }

	private int statusIndex;

	public BuyAndUseReq(int goodsId, int statusIndex, System.Action<KodGames.ClientClass.Reward> callback)
	{
		this.goodsId = goodsId;
		this.statusIndex = statusIndex;
		this.buyAndUseCallBack = callback;
	}

	public BuyAndUseReq(int goodsId, int statusIndex)
	{
		this.goodsId = goodsId;
		this.statusIndex = statusIndex;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BuyAndUse(ID, goodsId, statusIndex);
	}
}

internal class BuySpecialGoodsReq : Request
{
	private int goodsId;

	public int GoodsId { get { return goodsId; } }

	private int goodsCount = 0;

	public int GoodsCount { get { return goodsCount; } }

	private System.Action<int> onBuySuccessDel;

	public System.Action<int> OnBuySuccessDel { get { return this.onBuySuccessDel; } }

	public BuySpecialGoodsReq(int goodsId, int goodsCount, System.Action<int> onBuySuccessDel)
	{
		this.goodsId = goodsId;
		this.goodsCount = goodsCount;
		this.onBuySuccessDel = onBuySuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BuySpecialGoods(ID, goodsId);
	}
}

#endregion Shop

#region bag

internal class ConsumeItemReq : Request
{
	private int itemId;
	private int amount;
	private int groupIndex;
	private string phoneNumber;
	private bool isQuickUse = false;

	public int ItemId { get { return itemId; } }

	public int Amount { get { return amount; } }

	public bool IsQuickUse { get { return isQuickUse; } }

	public ConsumeItemReq(int itemId, int amount)
		: this(itemId, amount, 0, "")
	{
	}

	public ConsumeItemReq(int itemId, int amount, int groupIndex)
		: this(itemId, amount, groupIndex, "", false)
	{
	}

	public ConsumeItemReq(int itemId, int amount, int groupIndex, string phoneNumber)
		: this(itemId, amount, groupIndex, phoneNumber, false) { }

	public ConsumeItemReq(int itemId, int amount, int groupIndex, string phoneNumber, bool isQuickUse)
	{
		this.itemId = itemId;
		this.amount = amount;
		this.groupIndex = groupIndex;
		this.phoneNumber = phoneNumber;
		this.isQuickUse = isQuickUse;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ConsumeItem(ID, itemId, amount, groupIndex, phoneNumber);
	}
}

internal class SellItemReq : Request
{
	private List<KodGames.ClientClass.Cost> items;

	public List<KodGames.ClientClass.Cost> Items { get { return items; } }

	public SellItemReq(List<KodGames.ClientClass.Cost> items)
	{
		this.items = items;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SellItem(ID, items);
	}
}

#endregion bag

#region Dungeon

internal class QueryDungeonListReq : Request
{
	public delegate void OnQuerySuccessDel();

	private OnQuerySuccessDel successDel;

	public OnQuerySuccessDel SuccessDel
	{
		get { return successDel; }
	}

	public QueryDungeonListReq() : this(null) { }

	public QueryDungeonListReq(OnQuerySuccessDel successDel)
	{
		this.successDel = successDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryDungeonList(ID);
	}
}

internal class OnCombatReq : Request
{
	private int dungeonId;

	public int DungeonId
	{
		get { return dungeonId; }
	}

	private int npcId;

	private KodGames.ClientClass.Position position;

	public KodGames.ClientClass.Position Position { get { return position; } }

	private float uiDungeonMapPosition;

	public float UiDungeonMapPosition
	{
		get { return uiDungeonMapPosition; }
	}

	public OnCombatReq(int dungeonId, int npcId, KodGames.ClientClass.Position position, float uiDungeonMapPosition)
	{
		this.dungeonId = dungeonId;
		this.npcId = npcId;
		this.position = position;
		this.uiDungeonMapPosition = uiDungeonMapPosition;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.Combat(ID, dungeonId, position, npcId);
	}
}

internal class QueryRecruiteNpcReq : Request
{
	private int dungeonId;

	public int DungeonId
	{
		get { return dungeonId; }
	}

	public QueryRecruiteNpcReq(int dungeonId)
	{
		this.dungeonId = dungeonId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryRecruiteNpc(ID, dungeonId);
	}
}

internal class SetZoneStatusReq : Request
{
	private int zoneId;

	public int ZoneId { get { return zoneId; } }

	private int status;

	public int Status { get { return status; } }

	public delegate void OnResponseSuccessDel();
	private OnResponseSuccessDel del;

	public OnResponseSuccessDel Del { get { return del; } }

	public SetZoneStatusReq(int zoneId, int status)
	{
		this.zoneId = zoneId;
		this.status = status;
		this.del = null;
	}

	public SetZoneStatusReq(int zoneId, int status, OnResponseSuccessDel del)
	{
		this.zoneId = zoneId;
		this.status = status;
		this.del = del;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SetZoneStatus(ID, zoneId, status);
	}
}

internal class SetDungeonStatusReq : Request
{
	private int dungeonId;

	public int DungeonId { get { return dungeonId; } }

	private int status;

	public int Status { get { return status; } }

	public SetDungeonStatusReq(int dungeonId, int status)
	{
		this.dungeonId = dungeonId;
		this.status = status;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SetDungeonStatus(ID, dungeonId, status);
	}
}

internal class ResetDungeonCompleteTimesReq : Request
{
	private int dungeonID;

	public int DungeonID { get { return dungeonID; } }

	public ResetDungeonCompleteTimesReq(int dungeonID)
	{
		this.dungeonID = dungeonID;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ResetDungeonCompleteTimes(ID, dungeonID);
	}
}

internal class GetDungeonRewardReq : Request
{
	private int zoneId;

	public int ZoneId { get { return zoneId; } }

	private int dungeonDifficulty;

	public int DungeonDifficulty { get { return dungeonDifficulty; } }

	private int boxIndex;

	public GetDungeonRewardReq(int zoneId, int dungeonDifficulty, int boxIndex)
	{
		this.zoneId = zoneId;
		this.dungeonDifficulty = dungeonDifficulty;
		this.boxIndex = boxIndex;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.DungeonGetReward(ID, zoneId, dungeonDifficulty, boxIndex);
	}
}

internal class SetDungeonDialogStateReq : Request
{
	private int dungeonId;
	private int state;

	public override bool WaitingResponse { get { return false; } }

	public SetDungeonDialogStateReq(int zoneId, int dungeonId, int state)
	{
		this.dungeonId = dungeonId;
		this.state = state;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SetDungeonDialogState(ID, dungeonId, state);
	}
}

//副本扫荡
internal class ContinueCombatReq : Request
{
	private int zoneId;

	public int ZoneId
	{
		get { return zoneId; }
	}

	private int dungeonId;

	public int DungeonId
	{
		get { return dungeonId; }
	}

	public ContinueCombatReq(int zoneId, int dungeonId)
	{
		this.zoneId = zoneId;
		this.dungeonId = dungeonId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ContinueCombat(ID, zoneId, dungeonId);
	}
}

internal class QueryDungeonGuideReq : Request
{
	private int dungeonId;

	public int DungeonId { get { return dungeonId; } }

	public QueryDungeonGuideReq(int dungeonId)
	{
		this.dungeonId = dungeonId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryDungeonGuide(ID, dungeonId);
	}
}

internal class QueryTravelReq : Request
{
	private int dungeonId;

	public int DungeonId { get { return dungeonId; } }

	public QueryTravelReq(int dungeonId)
	{
		this.dungeonId = dungeonId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryTravel(ID, dungeonId);
	}
}

internal class BuyTravelReq : Request
{
	private int dungeonId;

	public int DungeonId { get { return dungeonId; } }

	private int goodId;

	public int GoodId { get { return goodId; } }

	public BuyTravelReq(int dungeonId, int goodId)
	{
		this.dungeonId = dungeonId;
		this.goodId = goodId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BuyTravel(ID, dungeonId, goodId);
	}
}

#endregion Dungeon

#region Chat

internal class ChatReq : Request
{
	private com.kodgames.corgi.protocol.ChatMessage chatMessage;

	public override bool HasResponse
	{
		get { return false; }
	}

	public ChatReq(com.kodgames.corgi.protocol.ChatMessage chatMessage)
	{
		this.chatMessage = chatMessage;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		chatMessage.time = SysLocalDataBase.Inst.LoginInfo.NowTime;

		return bsn.Chat(ID, chatMessage);
	}

	private int Compare(com.kodgames.corgi.protocol.ChatMessage msg1, com.kodgames.corgi.protocol.ChatMessage msg2)
	{
		return msg1.time.CompareTo(msg2.time);
	}
}

internal class QueryPlayerInfoReq : Request
{
	public QueryPlayerInfoReq(int playerId)
	{
		this.playerId = playerId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryPlayerInfo(ID, playerId);
	}

	private int playerId;

	public int PlayerId
	{
		set { this.playerId = value; }
		get { return this.playerId; }
	}
}

internal class QueryChatMessageListReq : Request
{
	public QueryChatMessageListReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		int id = ID;
		return bsn.QueryChatMessageList(id);
	}
}

internal class CloseChatMessageDialog : Request
{
	public CloseChatMessageDialog()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		int id = ID;
		return bsn.CloseChatMessageDialog(id);
	}
}

#endregion Chat

#region Account

internal class QueryEmailListsReq : Request
{
	private int emailType;

	public int EmailType
	{
		get { return emailType; }
	}

	public QueryEmailListsReq(int displayType)
	{
		this.emailType = displayType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryEmailListInfo(ID, emailType);
	}
}

//可能要改
internal class GetAttachmentsReq : Request
{
	private long emailId;

	public GetAttachmentsReq(long emailId)
	{
		this.emailId = emailId;
	}

	public long EmailID
	{
		get { return emailId; }
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GetAttachments(ID, emailId);
	}
}

#endregion Account

#region Daily SignIn

internal class SignInReq : Request
{
	public int signType;

	public SignInReq(int signType)
	{
		this.signType = signType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SignIn(ID, signType);
	}
}

#endregion Daily SignIn

#region Activity

internal class QueryFixedTimeActivityRewardReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryFixedTimeActivityReward(ID);
	}
}

internal class GetFixedTimeActivityRewardReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GetFixedTimeActivityReward(ID);
	}
}

#endregion Activity

#region Arena

internal class QueryArenaRankReq : Request
{
	public QueryArenaRankReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryArenaRank(ID);
	}
}

internal class QueryHonorPointReq : Request
{
	public QueryHonorPointReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryGradePoint(ID);
	}
}

internal class ArenaCombatReq : Request
{
	public ArenaCombatReq(int rank, KodGames.ClientClass.Position position)
	{
		this.rank = rank;
		this.positon = position;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ArenaCombat(ID, rank, positon);
	}

	private int rank;
	private KodGames.ClientClass.Position positon;
}

internal class QueryArenaPlayerInfoReq : Request
{
	private int rank;

	public int Rank { get { return rank; } }

	private int arenaGradeId;

	public QueryArenaPlayerInfoReq(int rank, int arenaGradeId)
	{
		this.rank = rank;
		this.arenaGradeId = arenaGradeId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryArenaPlayerInfo(ID, rank, arenaGradeId);
	}
}

internal class QueryRankToFewReq : Request
{
	public QueryRankToFewReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryRankToFew(ID);
	}
}

#endregion Arena

#region Setting Request

internal class ExchangeCodeReq : Request
{
	private string strRewardKey;

	public ExchangeCodeReq(string strRewardKey)
	{
		this.strRewardKey = strRewardKey;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ExchangeCode(ID, strRewardKey);
	}
}

internal class SettingFeedbackReq : Request
{
	private int type;
	private string strInfo;

	public SettingFeedbackReq(int type, string strInfo)
	{
		this.type = type;
		this.strInfo = strInfo;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SettingFeedback(ID, type, strInfo);
	}
}

#endregion Setting Request

#region Tavern

internal class QueryTavernInfoReq : Request
{
	public QueryTavernInfoReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.TavernQuery(ID);
	}
}

//酒馆
internal class TavernBuyReq : Request
{
	private int tavernId;
	private int tavernType;

	public TavernBuyReq(int tavernId, int tavernType)
	{
		this.tavernId = tavernId;
		this.tavernType = tavernType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.TavernBuy(ID, tavernId, tavernType);
	}
}

#endregion Tavern

#region Tutorial

internal class GetTutorialAvatarAndSetPlayerNameReq : Request
{
	private int resourceId;
	private string playerName;

	public string PlayerName { get { return playerName; } }

	private int tutorialId;

	public GetTutorialAvatarAndSetPlayerNameReq(int resourceId, string playerName, int tutorialId)
	{
		this.resourceId = resourceId;
		this.playerName = playerName;
		this.tutorialId = tutorialId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GetTutorialAvatarAndSetPlayerName(ID, resourceId, playerName, tutorialId);
	}
}

internal class CompleteTutorialReq : Request
{
	public override bool WaitingResponse { get { return false; } }

	private int tutorialId;

	public int TutorialId { get { return tutorialId; } }

	public CompleteTutorialReq(int tutorialId)
	{
		this.tutorialId = tutorialId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.CompleteTutorial(ID, tutorialId);
	}
}

internal class NoviceCombatReq : Request
{
	public NoviceCombatReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.NoviceCombat(ID);
	}
}

internal class FetchRandomPlayerNameReq : Request
{
	public FetchRandomPlayerNameReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.FetchRandomPlayerNames(ID);
	}
}

#endregion Tutorial

#region Quest : DailyGuid

internal class QueryQuestInfoReq : Request
{
	public delegate void OnQueryQuestInfoSuccess();

	private OnQueryQuestInfoSuccess del;

	public OnQueryQuestInfoSuccess Del { get { return del; } }

	public QueryQuestInfoReq(OnQueryQuestInfoSuccess del)
	{
		this.del = del;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryQuestInfo(ID);
	}
}

internal class PickQuestRewardReq : Request
{
	private int questId;

	public delegate void OnPickQuestRewardSuccessDel(List<KodGames.ClientClass.Quest> changedQuests, KodGames.ClientClass.Reward reward);

	private OnPickQuestRewardSuccessDel pickQuestRewardSuccessDel;

	public OnPickQuestRewardSuccessDel PickQuestRewardSuccessDel
	{
		get { return pickQuestRewardSuccessDel; }
	}

	public PickQuestRewardReq(int questId, OnPickQuestRewardSuccessDel onPickQuestRewardSuccessDel)
	{
		this.questId = questId;
		this.pickQuestRewardSuccessDel = onPickQuestRewardSuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.PickQuestReward(ID, questId);
	}
}

#endregion Quest : DailyGuid

#region Remote Notifycation

internal class SendAPNTokenRequest : Request
{
	public override bool WaitingResponse { get { return false; } }

	public SendAPNTokenRequest(byte[] token)
	{
		this.token = token;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SendAPNToken(ID, token);
	}

	private byte[] token;
}

#endregion Remote Notifycation

internal class LevelRewardGetRewardReq : Request
{
	private int levelRewardId;

	public LevelRewardGetRewardReq(int levelRewardId)
	{
		this.levelRewardId = levelRewardId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.LevelRewardGetReward(ID, levelRewardId);
	}
}

internal class QueryLevelRewardReq : Request
{
	public QueryLevelRewardReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryLevelReward(ID);
	}
}

internal class GetLevelUpRewardReq : Request
{
	private int wantPickLevel;

	private SysUIEnv.UIModuleDelayShowEvent delayShowEvent;

	public SysUIEnv.UIModuleDelayShowEvent DelayShowEvent { get { return delayShowEvent; } }

	public GetLevelUpRewardReq(int wantPickLevel)
	{
		this.wantPickLevel = wantPickLevel;

		if ((SysGameStateMachine.Instance.CurrentState is GameState_Battle) == false)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level <= ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel)
				this.delayShowEvent = new SysUIEnv.UIModuleDelayShowEvent(typeof(UIDlgPlayerLevelUp));
			SysUIEnv.Instance.AddShowEvent(this.delayShowEvent);
		}
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GetLevelUpReward(ID, wantPickLevel);
	}
}

#region InterfaceServer

/*
/// <summary>
/// Auth token req.
/// </summary>
class CI_AuthTokenReq : Request
{
	public CI_AuthTokenReq(int team_id, string token)
	{
		this.team_id = team_id;
		this.token = token;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.CI_AuthToken(team_id, token, ID);
	}

	private int team_id;
	public int Team_id
	{
		set{ this.team_id = value; }
		get{ return this.team_id; }
	}

	private string token;
	public string Token
	{
		set{ this.token = value; }
		get{ return this.token; }
	}
}*/

#endregion InterfaceServer

#region ActivityExchange

internal class QueryExchangeList : Request
{
	public QueryExchangeList()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryExchangeList(ID);
	}
}

internal class ExchangeReq : Request
{
	private int exchangeId;
	private List<KodGames.ClientClass.Cost> costs;
	private int groupId;

	public ExchangeReq(int exchangeId, List<KodGames.ClientClass.Cost> costs, int groupId)
	{
		this.exchangeId = exchangeId;
		this.costs = costs;
		this.groupId = groupId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ExchangeReq(ID, exchangeId, costs, groupId);
	}
}

#endregion ActivityExchange

#region StartServerReward

internal class PickStartServerRewardReq : Request
{
	private int pickID;

	public int PickID
	{
		get { return pickID; }
	}

	public PickStartServerRewardReq(int pickID)
	{
		this.pickID = pickID;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.PickStartServerReward(ID, pickID);
	}
}

#endregion StartServerReward

#region HandBookReward

//图鉴合成
internal class MergeIllustrationReq : Request
{
	private int illustrationId;
	private int count;

	public int Count
	{
		get
		{
			return count;
		}
	}

	public int IllustrationId
	{
		get { return illustrationId; }
	}

	public MergeIllustrationReq(int illustrationId, int count)
	{
		this.illustrationId = illustrationId;
		this.count = count;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.MergeIllustration(ID, illustrationId, count);
	}
}

#endregion HandBookReward

#region QueryIllustrationReq

internal class QueryIllustrationReq : Request
{
	public QueryIllustrationReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryIllustration(ID);
	}
}

#endregion QueryIllustrationReq

#region MysteryShop

internal class QueryMysteryShopInfoReq : Request
{
	private int shopType;

	public int ShopType { get { return shopType; } }

	public QueryMysteryShopInfoReq(int shopType)
	{
		this.shopType = shopType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryMysteryShopInfo(ID, shopType);
	}
}

//千机阁购买物品
internal class ChangeMysteryShopInfoReq : Request
{
	private int refreshId;

	public int RefreshId { get { return refreshId; } }

	private int shopType;

	public int ShopType { get { return shopType; } }

	public ChangeMysteryShopInfoReq(int shopType, int refreshId)
	{
		this.refreshId = refreshId;
		this.shopType = shopType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ChangeMysteryShopInfo(ID, shopType, refreshId);
	}
}

internal class BuyMysteryGoodReq : Request
{
	private int goodsIndex;
	private int goodsId;

	public int GoodsID { get { return goodsId; } }

	public int GoodsIndex { get { return goodsIndex; } }

	private int shopType;

	public int ShopType { get { return shopType; } }

	public BuyMysteryGoodReq(int shopType, int goodsId, int goodsIndex)
	{
		this.shopType = shopType;
		this.goodsId = goodsId;
		this.goodsIndex = goodsIndex;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BuyMysteryGoods(ID, shopType, goodsId, goodsIndex);
	}
}

#endregion MysteryShop

#region Assistant

internal class QueryTaskListReq : Request
{
	public QueryTaskListReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryTaskList(ID);
	}
}

internal class TaskConditionReq : Request
{
	private int gotoUI;

	public TaskConditionReq(int gotoUI)
	{
		this.gotoUI = gotoUI;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.TaskCondition(ID, gotoUI);
	}
}

#endregion Assistant

#region Tower

internal class QueryMelaleucaFloorPlayerInfoReq : Request
{
	public QueryMelaleucaFloorPlayerInfoReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryMelaleucaFloorPlayerInfo(ID);
	}
}

internal class QueryMelaleucaFloorInfoReq : Request
{
	private int layers;

	public QueryMelaleucaFloorInfoReq(int layers)
	{
		this.layers = layers;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryMelaleucaFloorInfo(ID, layers);
	}
}

internal class MelaleucaFloorCombatReq : Request
{
	private int layers;
	private KodGames.ClientClass.Position position;

	public MelaleucaFloorCombatReq(int layers, KodGames.ClientClass.Position position)
	{
		this.layers = layers;
		this.position = position;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.MelaleucaFloorCombat(ID, layers, position);
	}
}

internal class MelaleucaFloorConsequentCombatReq : Request
{
	private int layers;
	private int combatCount;
	private KodGames.ClientClass.Position position;

	public MelaleucaFloorConsequentCombatReq(int layers, int combatCount, KodGames.ClientClass.Position position)
	{
		this.layers = layers;
		this.combatCount = combatCount;
		this.position = position;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.MelaleucaFloorConsequentCombat(ID, layers, combatCount, position);
	}
}

internal class MelaleucaFloorThisWeekRankReq : Request
{
	public MelaleucaFloorThisWeekRankReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.MelaleucaFloorThisWeekRank(ID);
	}
}

internal class MelaleucaFloorLastWeekReq : Request
{
	public MelaleucaFloorLastWeekReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.MelaleucaFloorLastWeekRank(ID);
	}
}

internal class MelaleucaFloorGetRewardReq : Request
{
	public MelaleucaFloorGetRewardReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.MelaleucaFloorGetReward(ID);
	}
}

internal class MelaleucaFloorWeekRewardInfoReq : Request
{
	public MelaleucaFloorWeekRewardInfoReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.MelaleucaFloorWeekRewardInfo(ID);
	}
}

#endregion Tower

#region WolfSmoke

internal class QueryBuyWolfSmokeShop : Request
{
	private int goodsId;

	public int GoodId { get { return goodsId; } }

	private int goodsIndex;

	public QueryBuyWolfSmokeShop(int goodsId, int goodsIndex)
	{
		this.goodsId = goodsId;
		this.goodsIndex = goodsIndex;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryBuyWolfSmokeShop(ID, goodsId, goodsIndex);
	}
}

internal class QueryCombatWolfSmoke : Request
{
	private int additionId;

	public int AdditionId { get { return additionId; } }

	private KodGames.ClientClass.Position position;

	public QueryCombatWolfSmoke(int additionId, KodGames.ClientClass.Position position)
	{
		this.additionId = additionId;
		this.position = position;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryCombatWolfSmoke(ID, additionId, position);
	}
}

internal class QueryJoinWolfSmoke : Request
{
	private int positionId;

	public QueryJoinWolfSmoke(int positionId)
	{
		this.positionId = positionId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryJoinWolfSmoke(ID, positionId);
	}
}

internal class QueryWolfSmoke : Request
{
	private int positionId;

	public delegate void OnQueryfWolfSuccessDel();
	private OnQueryfWolfSuccessDel del;

	public OnQueryfWolfSuccessDel Del
	{
		get { return del; }
	}

	public QueryWolfSmoke(OnQueryfWolfSuccessDel del)
	{
		this.del = del;
	}

	public QueryWolfSmoke() : this(null) { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryWolfSmoke(ID);
	}
}

internal class QueryWolfSmokeEnemy : Request
{
	public QueryWolfSmokeEnemy() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryWolfSmokeEnemy(ID);
	}
}

internal class QueryWolfSmokePosition : Request
{
	public QueryWolfSmokePosition() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryWolfSmokePosition(ID);
	}
}

internal class QueryWolfSmokeShop : Request
{
	public QueryWolfSmokeShop() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryWolfSmokeShop(ID);
	}
}

internal class QueryRefreshWolfSmokeShop : Request
{
	private int positionId;

	public QueryRefreshWolfSmokeShop() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryRefreshWolfSmokeShop(ID);
	}
}

internal class QueryResetWolfSmoke : Request
{
	private int positionId;

	public QueryResetWolfSmoke() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;
		return bsn.QueryResetWolfSmoke(ID);
	}
}

#endregion WolfSmoke

#region QinInfo

internal class QueryQinInfoReq : Request
{
	public QueryQinInfoReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryQinInfo(ID);
	}
}

internal class AnswerQinInfoReq : Request
{
	private int questionId;

	public int QuestionId
	{
		get { return questionId; }
	}

	private int answerNum;

	public int AnswerNum
	{
		get { return answerNum; }
	}

	public AnswerQinInfoReq(int questionId, int answerNum)
	{
		this.questionId = questionId;
		this.answerNum = answerNum;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.AnswerQinInfo(ID, questionId, answerNum);
	}
}

internal class GetQinInfoContinueRewardReq : Request
{
	public GetQinInfoContinueRewardReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GetQinInfoContinueReward(ID);
	}
}

#endregion QinInfo

#region MonthCard

internal class QueryMonthCardInfoReq : Request
{
	public QueryMonthCardInfoReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryMonthCardInfo(ID);
	}
}

internal class MonthCardPickRewardReq : Request
{
	private int monthCardId;
	private int rewardType;

	public MonthCardPickRewardReq(int monthCardId, int rewardType)
	{
		this.monthCardId = monthCardId;
		this.rewardType = rewardType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.MonthCardPickReward(ID, monthCardId, rewardType);
	}
}

#endregion MonthCard

#region GiveMeFive

internal class GiveFiveStarsEvaluateReq : Request
{
	private bool isEvaluate = false;

	public bool IsEvaluate { get { return isEvaluate; } }

	public GiveFiveStarsEvaluateReq(bool isEvaluate)
	{
		this.isEvaluate = isEvaluate;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GiveFiveStarsEvaluate(ID, IsEvaluate);
	}
}

#endregion GiveMeFive

#region Friend

internal class QueryFriendListReq : Request
{
	private Func<List<KodGames.ClientClass.FriendInfo>, bool> querySuccessDel;
	public Func<List<KodGames.ClientClass.FriendInfo>, bool> QuerySuccessDel
	{
		get { return querySuccessDel; }
	}

	public QueryFriendListReq() : this(null) { }

	public QueryFriendListReq(Func<List<KodGames.ClientClass.FriendInfo>, bool> querySuccessDel)
	{
		this.querySuccessDel = querySuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryFriendList(ID);
	}
}

internal class RandomFriendReq : Request
{
	public RandomFriendReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.RandomFriend(ID);
	}
}

internal class QueryPlayerNameReq : Request
{
	private string playerName;

	public QueryPlayerNameReq(string playerName)
	{
		this.playerName = playerName;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryPlayerName(ID, playerName);
	}
}

internal class InviteFriendReq : Request
{
	private int invitedPlayerId;

	public int InvitedPlayerId
	{
		get { return this.invitedPlayerId; }
	}

	private string invitedPlayerName;

	public InviteFriendReq(int invitedPlayerId, string invitedPlayerName)
	{
		this.invitedPlayerId = invitedPlayerId;
		this.invitedPlayerName = invitedPlayerName;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.InviteFriend(ID, invitedPlayerId, invitedPlayerName);
	}
}

internal class AnswerFriendReq : Request
{
	private int invitorPlayerId;
	private long passiveEmailId;
	private bool agree;

	public AnswerFriendReq(int invitorPlayerId, long passiveEmailId, bool agree)
	{
		this.invitorPlayerId = invitorPlayerId;
		this.passiveEmailId = passiveEmailId;
		this.agree = agree;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.AnswerFriend(ID, invitorPlayerId, passiveEmailId, agree);
	}
}

internal class QueryFriendPlayerInfoReq : Request
{
	private int friendPlayerId;

	public QueryFriendPlayerInfoReq(int friendPlayerId)
	{
		this.friendPlayerId = friendPlayerId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryFriendPlayerInfo(ID, friendPlayerId);
	}
}

internal class RemoveFriendReq : Request
{
	private FriendInfo removePlayer;

	public FriendInfo RemovePlayer
	{
		get { return removePlayer; }
	}

	public RemoveFriendReq(FriendInfo removePlayer)
	{
		this.removePlayer = removePlayer;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.RemoveFriend(ID, removePlayer.PlayerId);
	}
}

internal class CombatFriendReq : Request
{
	private int friendId;

	public int FriendId { get { return friendId; } }

	private KodGames.ClientClass.Position position;

	public CombatFriendReq(int friendId, KodGames.ClientClass.Position position)
	{
		this.friendId = friendId;
		this.position = position;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.CombatFriend(ID, friendId, position);
	}
}

#endregion Friend

#region RunAcitvity

//主查询协议
internal class OperationActivityQueryReq : Request
{
	private System.Func<bool> del;
	public System.Func<bool> Del { get { return del; } }

	public OperationActivityQueryReq()
		: this(null)
	{
	}

	public OperationActivityQueryReq(System.Func<bool> del)
	{
		this.del = del;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.OperationActivityQueryReq(ID);
	}
}

//领奖协议
internal class OperationActivityPickRewardReq : Request
{
	private int activityId;

	public int ActivityId { get { return activityId; } }

	private int operationId;

	public OperationActivityPickRewardReq(int activityId, int operationId)
	{
		this.activityId = activityId;
		this.operationId = operationId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.OperationActivityPickRewardReq(ID, this.operationId);
	}
}

#endregion RunAcitvity

#region Adventure

//下一步
internal class MarvellousNextMarvellousReq : Request
{
	private int selectType;
	private int nextZone;

	public int SelectType { get { return selectType; } }

	private KodGames.ClientClass.Position position;

	public MarvellousNextMarvellousReq(int selectType, int nextZone, KodGames.ClientClass.Position position)
	{
		this.selectType = selectType;
		this.position = position;
		this.nextZone = nextZone;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryMarvellousNextMarvellousReq(ID, selectType, nextZone, position);
	}
}

//领取临时奖励
internal class MarvellousPickDelayRewardReq : Request
{
	private int eventId;
	private long couldPickTime;

	public int EventId { get { return eventId; } }

	public long CouldPickTime { get { return couldPickTime; } }

	public MarvellousPickDelayRewardReq(int eventId, long couldPickTime)
	{
		this.eventId = eventId;
		this.couldPickTime = couldPickTime;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryMarvellousPickDelayRewardReq(ID, eventId, couldPickTime);
	}
}

//查询延时奖励
internal class MarvellousQueryDelayRewardReq : Request
{
	public MarvellousQueryDelayRewardReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryMarvellousQueryDelayRewardReq(ID);
	}
}

//查询奇遇
internal class MarvellousQueryReq : Request
{
	public MarvellousQueryReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryMarvellousQueryReq(ID);
	}
}

#endregion Adventure

#region FriendCombatSystem

//主查询协议
internal class QueryFriendCampaignReq : Request
{
	public QueryFriendCampaignReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryFriendCampaignReq(ID);
	}
}

//重置协议
internal class ResetFriendCampaignReq : Request
{
	public ResetFriendCampaignReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ResetFriendCampaignReq(ID);
	}
}

//参战协议
internal class JoinFriendCampaignReq : Request
{
	private int positionId;
	private List<int> friendIds;

	public JoinFriendCampaignReq(int positionId, List<int> friendIds)
	{
		this.positionId = positionId;
		this.friendIds = friendIds;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.JoinFriendCampaignReq(ID, this.positionId, this.friendIds);
	}
}

//战斗协议
internal class CombatFriendCampaignReq : Request
{
	private int playerId;
	private KodGames.ClientClass.Position position;

	public CombatFriendCampaignReq(int playerId, KodGames.ClientClass.Position position)
	{
		this.playerId = playerId;
		this.position = position;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.CombatFriendCampaignReq(ID, this.playerId, this.position);
	}
}

internal class QueryFriendCampaignHelpFriendInfoReq : Request
{
	private List<int> friendIds;
	private int positionId;

	public int PositionId
	{
		get { return positionId; }
	}

	public QueryFriendCampaignHelpFriendInfoReq(int positionId, List<int> friendIds)
	{
		this.friendIds = friendIds;
		this.positionId = positionId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryFriendCampaignHelpFriendInfoReq(ID, friendIds);
	}
}

//查询排行榜数据
internal class QueryFCRankReq : Request
{
	private int rankType;

	public QueryFCRankReq(int rankType)
	{
		this.rankType = rankType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryFCRankReq(ID, rankType);
	}
}

//查询好友贡献值的具体信息
internal class QueryFCPointDetailReq : Request
{
	private int rankType;

	public QueryFCPointDetailReq(int rankType) { this.rankType = rankType; }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryFCPointDetailReq(ID, rankType);
	}
}

//排行榜奖励查询
internal class QueryFCRankRewarReq : Request
{
	public QueryFCRankRewarReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryFCRankRewarReq(ID);
	}
}

//领取排行榜奖励协议
internal class FCRankGetRewardReq : Request
{
	public FCRankGetRewardReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.FCRankGetRewardReq(ID);
	}
}

#endregion FriendCombatSystem

#region Illusion

internal class QueryIllusionReq : Request
{
	public delegate void OnQueryIllusionResDel(bool success);
	private OnQueryIllusionResDel onQueryIllusionRes;

	public OnQueryIllusionResDel OnQueryIllusionRes
	{
		get { return onQueryIllusionRes; }
	}

	public QueryIllusionReq(OnQueryIllusionResDel onQueryIllusionSuccess)
	{
		this.onQueryIllusionRes = onQueryIllusionSuccess;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryIllusionReq(ID);
	}
}

internal class ActivateIllusionReq : Request
{
	public delegate void OnActivateIllusionResDel(bool success);

	private OnActivateIllusionResDel onActivateIllusionRes;

	public OnActivateIllusionResDel OnActivateIllusionRes
	{
		get { return onActivateIllusionRes; }
	}

	private int avatarId;

	public int AvatarId { get { return avatarId; } }

	private int illusionId;

	public int IllusionId { get { return illusionId; } }

	private int activeType;

	public int ActiveType { get { return activeType; } }

	public ActivateIllusionReq(int avatarId, int illusionId, int activeType, OnActivateIllusionResDel onActivateIllusionSuccess)
	{
		this.onActivateIllusionRes = onActivateIllusionSuccess;
		this.avatarId = avatarId;
		this.illusionId = illusionId;
		this.activeType = activeType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ActivateIllusionReq(ID, avatarId, illusionId, activeType);
	}
}

internal class IllusionReq : Request
{
	public delegate void OnIllusionResDel(bool success);

	private OnIllusionResDel onIllusionRes;

	public OnIllusionResDel OnIllusionRes
	{
		get { return onIllusionRes; }
	}

	private int avatarId;

	public int AvatarId { get { return avatarId; } }

	private int illusionId;

	public int IllusionId { get { return illusionId; } }

	private int useStatusType;

	public int UseStatusType { get { return useStatusType; } }

	private int type;

	public IllusionReq(int avatarId, int illusionId, int useStatusType, int type, OnIllusionResDel onIllusionReqSuccess)
	{
		this.avatarId = avatarId;
		this.illusionId = illusionId;
		this.useStatusType = useStatusType;
		this.onIllusionRes = onIllusionReqSuccess;
		this.type = type;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.IllusionReq(ID, avatarId, illusionId, type, useStatusType);
	}
}

internal class ActivateAndIllusionReq : Request
{
	public delegate void OnActivateAndIllusionResDel(bool success);

	private OnActivateAndIllusionResDel onActivateAndIllusionRes;

	public OnActivateAndIllusionResDel OnActivateAndIllusionRes
	{
		get { return onActivateAndIllusionRes; }
	}

	private int avatarId;

	public int AvatarId { get { return avatarId; } }

	private int illusionId;

	public int IllusionId { get { return illusionId; } }

	private int useStatus;

	public ActivateAndIllusionReq(int avatarId, int illusionId, int useStatus, OnActivateAndIllusionResDel onActivateAndIllusionRes)
	{
		this.avatarId = avatarId;
		this.illusionId = illusionId;
		this.onActivateAndIllusionRes = onActivateAndIllusionRes;
		this.useStatus = useStatus;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ActivateAndIllusionReq(ID, avatarId, illusionId, useStatus);
	}
}

#endregion Illusion

#region 新神秘商店

//主查询协议
internal class QueryMysteryerReq : Request
{
	public QueryMysteryerReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryMysteryerReq(ID);
	}
}

//刷新协议
internal class RefreshMysteryerReq : Request
{
	private int type;

	public RefreshMysteryerReq(int type)
	{
		this.type = type;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.RefreshMysteryerReq(ID, type);
	}
}

//购买协议
internal class BuyMysteryerReq : Request
{
	private int place;
	private int goodId;

	public BuyMysteryerReq(int place, int goodId)
	{
		this.place = place;
		this.goodId = goodId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BuyMysteryerReq(ID, goodId, place);
	}
}

#endregion


#region Invite
//海外兑换码主查询
internal class QueryInviteCodeInfoReq : Request
{
	public QueryInviteCodeInfoReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryInviteCodeInfoReq(ID);
	}
}

//海外兑换码领取奖励
internal class VerifyInviteCodeAndPickRewardReq : Request
{
	private string code;

	public VerifyInviteCodeAndPickRewardReq(string code)
	{
		this.code = code;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.VerifyInviteCodeAndPickRewardReq(ID, code);
	}
}

//海外兑换码领取奖励
internal class PickInviteCodeRewardReq : Request
{
	private int rewardId;
	public int RewardId { get { return rewardId; } }

	public PickInviteCodeRewardReq() { }

	public PickInviteCodeRewardReq(int rewardId)
	{
		this.rewardId = rewardId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.PickInviteCodeRewardReq(ID, rewardId);
	}
}

//分享成功后通知服务器修改状态
internal class FacebookShareReq : Request
{
	public FacebookShareReq() { }

	public override bool WaitingResponse { get { return false; } }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.FacebookShareReq(ID);
	}
}
#endregion

#region 711活动

//主查询协议
internal class QuerySevenElevenGiftReq : Request
{
	private string deviceId;

	public QuerySevenElevenGiftReq(string deviceId)
	{
		this.deviceId = deviceId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QuerySevenElevenGiftReq(ID, deviceId);
	}
}

//摇数
internal class TurnNumberReq : Request
{
	private string deviceId;
	private int position;
	public int Postion { get { return position; } }

	public TurnNumberReq(string deviceId, int position)
	{
		this.deviceId = deviceId;
		this.position = position;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.TurnNumberReq(ID, deviceId, position);
	}
}

//领取奖励
internal class NumberConvertReq : Request
{
	private int contertType;

	public NumberConvertReq(int contertType)
	{
		this.contertType = contertType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.NumberConvertReq(ID, contertType);
	}
}
#endregion

#region 东海寻仙

//主查询
internal class EastSeaQueryZentiaReq : Request
{
	private Func<bool, bool> queryDel;
	public Func<bool, bool> QueryDel { get { return queryDel; } }

	public EastSeaQueryZentiaReq(Func<bool, bool> queryDel)
	{
		this.queryDel = queryDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryZentiaReq(ID);
	}
}

//东海寻仙玩家获得特殊道具跑马灯信息
internal class EastSeaQueryZentiaFlowMessageReq : Request
{
	public EastSeaQueryZentiaFlowMessageReq()
	{
	}
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryZentiaFlowMessageReq(ID);
	}
}

//兑换东海寻仙道具
internal class EastSeaExchangeZentiaItemReq : Request
{
	private int exchangeId;
	private int index;
	private List<KodGames.ClientClass.Cost> costs;
	public EastSeaExchangeZentiaItemReq(int exchangeId, int index, List<KodGames.ClientClass.Cost> costs)
	{
		this.exchangeId = exchangeId;
		this.index = index;
		this.costs = costs;
	}
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ExchangeZentiaItemReq(ID, exchangeId, index, costs);
	}
}

//刷新东海寻仙道具兑换
internal class EastSeaRefreshZentiaReq : Request
{
	public EastSeaRefreshZentiaReq()
	{
	}
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.RefreshZentiaReq(ID);
	}
}

//查询仙缘兑换商品
internal class EastSeaQueryZentiaGoodReq : Request
{
	public EastSeaQueryZentiaGoodReq()
	{
	}
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryZentiaGoodReq(ID);
	}
}

//仙缘兑换下的商品购买
internal class EastSeaBuyZentiaGoodReq : Request
{
	private int goodsId;
	public EastSeaBuyZentiaGoodReq(int goodsId)
	{
		this.goodsId = goodsId;
	}
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BuyZentiaGoodReq(ID, goodsId);
	}
}

//查询全服奖励
internal class EastSeaQueryServerZentiaRewardReq : Request
{
	public EastSeaQueryServerZentiaRewardReq()
	{
	}
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryServerZentiaRewardReq(ID);
	}
}

//领取全服奖励
internal class EastSeaGetServerZentiaRewardReq : Request
{
	private int rewardLevelId;

	public EastSeaGetServerZentiaRewardReq(int rewardLevelId)
	{
		this.rewardLevelId = rewardLevelId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GetServerZentiaRewardReq(ID, rewardLevelId);
	}
}

//排行榜查询
internal class EastSeaQueryZentiaRankReq : Request
{
	public EastSeaQueryZentiaRankReq()
	{
	}
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryZentiaRankReq(ID);
	}
}


#endregion

#region 内丹系统

internal class QueryAlchemyReq : Request
{
	public QueryAlchemyReq() { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryAlchemy(ID);
	}
}

internal class PickAlchemyBoxReq : Request
{
	private int countRewardId;

	public PickAlchemyBoxReq(int countRewardId)
	{
		this.countRewardId = countRewardId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.PickAlchemyBox(ID, countRewardId);
	}
}

internal class QueryDanActivityReq : Request
{
	private int activityType;

	public QueryDanActivityReq(int activityType)
	{
		this.activityType = activityType;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryDanActivity(ID, activityType);
	}
}

internal class QueryDanHomeReq : Request
{
	public QueryDanHomeReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryDanHome(ID);
	}
}

internal class AlchemyReq : Request
{
	private int chatType;
	public int ChatType
	{
		get { return chatType; }
	}

	private List<KodGames.ClientClass.Cost> cost;

	public AlchemyReq(int chatType, List<KodGames.ClientClass.Cost> cost)
	{
		this.chatType = chatType;
		this.cost = cost;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.Alchemy(ID, chatType, cost);
	}
}

internal class QueryDanDecomposeReq : Request
{
	private List<KodGames.ClientClass.Dan> dans;
	public List<KodGames.ClientClass.Dan> Dans
	{
		get { return dans; }
	}

	public QueryDanDecomposeReq()
	{

	}

	public QueryDanDecomposeReq(List<KodGames.ClientClass.Dan> dans)
	{
		this.dans = dans;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryDanDecompose(ID);
	}
}

internal class DanDecomposeReq : Request
{
	private int type;
	private List<string> guids;
	private KodGames.ClientClass.Cost cost;

	public DanDecomposeReq(int type, List<string> guids, KodGames.ClientClass.Cost cost)
	{
		this.type = type;
		this.guids = guids;
		this.cost = cost;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.DanDecompose(ID, type, guids, cost);
	}
}

internal class LockDanReq : Request
{
	private int type;
	public int Type
	{
		get { return type; }
	}

	private string guid;

	private int danPackage;
	public int DanPackage
	{
		get { return danPackage; }
	}

	public LockDanReq(int type, string guid, int danPackage)
	{
		this.type = type;
		this.guid = guid;
		this.danPackage = danPackage;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryLockDan(ID, type, guid);
	}
}

internal class QueryDanStoreReq : Request
{
	private int type;

	public QueryDanStoreReq(int type)
	{
		this.type = type;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryDanStore(ID, type);
	}
}

internal class DanLevelUpReq : Request
{
	private string guid;

	public DanLevelUpReq(string guid)
	{
		this.guid = guid;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.DanLevelUp(ID, guid);
	}
}

internal class DanBreakthoughtReq : Request
{
	private string guid;

	public DanBreakthoughtReq(string guid)
	{
		this.guid = guid;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.DanBreakthought(ID, guid);
	}
}

internal class DanAttributeRefreshReq : Request
{
	private string guid;
	private List<int> attributeGroupIds;
	public DanAttributeRefreshReq(string guid, List<int> attributeGroupIds)
	{
		this.attributeGroupIds = attributeGroupIds;
		this.guid = guid;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.DanAttributeRefresh(ID, guid, attributeGroupIds);
	}
}

#endregion

#region 门派
// 门派信息查询
internal class GuildQueryReq : Request
{
	private Func<bool> querySuccessDel;
	public Func<bool> QuerySuccessDel
	{
		get { return querySuccessDel; }
	}

	public GuildQueryReq() : this(null) { }

	public GuildQueryReq(Func<bool> querySuccessDel)
	{
		this.querySuccessDel = querySuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQuery(ID);
	}
}

// 设置公告
internal class GuildSetAnnouncementReq : Request
{
	private string announcement;

	public GuildSetAnnouncementReq(string announcement)
	{
		this.announcement = announcement;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildSetAnnouncement(ID, announcement);
	}
}

// 创建门派
internal class GuildCreateReq : Request
{
	private string guildName;
	private bool allowAutoEnter;

	public GuildCreateReq(string guildName, bool allowAutoEnter)
	{
		this.guildName = guildName;
		this.allowAutoEnter = allowAutoEnter;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildCreate(ID, guildName, allowAutoEnter);
	}
}

// 查询门派列表
internal class GuildQueryGuildListReq : Request
{
	private string keyWord;
	public string KeyWord
	{
		get { return keyWord; }
	}

	public GuildQueryGuildListReq(string keyWord)
	{
		this.keyWord = keyWord;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQueryGuildList(ID, keyWord);
	}
}

// 申请进入门派
internal class GuildApplyReq : Request
{
	private int guildId;

	public GuildApplyReq(int guildId)
	{
		this.guildId = guildId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildApply(ID, guildId);
	}
}

// 快速加入门派
internal class GuildQuickJoinReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQuickJoin(ID);
	}
}

// 查看门派简略信息
internal class GuildViewSimpleReq : Request
{
	private int guildId;
	private Func<object, bool> querySuccessDel;
	public Func<object, bool> QuerySuccessDel
	{
		get { return querySuccessDel; }
	}


	public GuildViewSimpleReq(int guildId, Func<object, bool> querySuccessDel)
	{
		this.guildId = guildId;
		this.querySuccessDel = querySuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildViewSimple(ID, guildId);
	}
}

// 查看门派排行列表
internal class GuildQueryRankList : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQueryRankList(ID);
	}
}

// 查看门派留言
internal class GuildQueryMsgReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQueryMsg(ID);
	}
}

// 门派留言
internal class GuildAddMsgReq : Request
{
	private string msg;

	public GuildAddMsgReq(string msg)
	{
		this.msg = msg;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildAddMsg(ID, msg);
	}
}

// 查看门派动态
internal class GuildQueryNewsReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQueryNews(ID);
	}
}

// 修改门派宣言
internal class GuildSetDeclarationReq : Request
{
	private string declaration;

	public GuildSetDeclarationReq(string declaration)
	{
		this.declaration = declaration;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildSetDeclaration(ID, declaration);
	}
}

// 转让门派成员列表查询
internal class GuildQueryTransferMemberReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQueryTransferMember(ID);
	}
}

// 转让门派
internal class GuildTransferReq : Request
{
	private Func<bool> transSuccessDel;
	public Func<bool> TransSuccessDel
	{
		get { return transSuccessDel; }
	}

	private int destPlayer;

	public GuildTransferReq(int destPlayer, Func<bool> transSuccessDel)
	{
		this.destPlayer = destPlayer;
		this.transSuccessDel = transSuccessDel;
	}

	public GuildTransferReq(int destPlayer) : this(destPlayer, null) { }

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildTransfer(ID, destPlayer);
	}
}

// 离开门派
internal class GuildQuitReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQuit(ID);
	}
}

// 查询门派成员
internal class GuildQueryMemberReq : Request
{
	private Func<bool> querySuccessDel;
	public Func<bool> QuerySuccessDel
	{
		get { return querySuccessDel; }
	}

	public GuildQueryMemberReq() : this(null) { }

	public GuildQueryMemberReq(Func<bool> querySuccessDel)
	{
		this.querySuccessDel = querySuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQueryMember(ID);
	}
}

// 查询申请列表
internal class GuildQueryApplyListReq : Request
{
	private Func<object, bool> querySuccessDel;
	public Func<object, bool> QuerySuccessDel
	{
		get { return querySuccessDel; }
	}

	public GuildQueryApplyListReq() : this(null) { }

	public GuildQueryApplyListReq(Func<object, bool> querySuccessDel)
	{
		this.querySuccessDel = querySuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildQueryApplyList(ID);
	}
}

// 审核申请
internal class GuildReviewApplyReq : Request
{
	private Func<List<int>, bool> reviewSuccessDel;
	public Func<List<int>, bool> ReviewSuccessDel
	{
		get { return reviewSuccessDel; }
	}

	private int playerId;
	private bool refuse;

	public GuildReviewApplyReq(int playerId, bool refuse) : this(playerId, refuse, null) { }

	public GuildReviewApplyReq(int playerId, bool refuse, Func<List<int>, bool> reviewSuccessDel)
	{
		this.playerId = playerId;
		this.refuse = refuse;
		this.reviewSuccessDel = reviewSuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildReviewApply(ID, playerId, refuse);
	}
}

// 一键拒绝
internal class GuildOneKeyRefuseReq : Request
{
	private Func<List<int>, bool> reviewSuccessDel;
	public Func<List<int>, bool> ReviewSuccessDel
	{
		get { return reviewSuccessDel; }
	}

	private List<int> playerIds;

	public GuildOneKeyRefuseReq(List<int> playerIds) : this(playerIds, null) { }

	public GuildOneKeyRefuseReq(List<int> playerIds, Func<List<int>, bool> reviewSuccessDel)
	{
		this.playerIds = playerIds;
		this.reviewSuccessDel = reviewSuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildOneKeyRefuse(ID, playerIds);
	}
}

// 踢出门派
internal class GuildKickPlayerReq : Request
{
	private Func<bool> kickSuccessDel;
	public Func<bool> KickSuccessDel
	{
		get { return kickSuccessDel; }
	}

	private int playerId;
	public int PlayerId
	{
		get { return playerId; }
	}

	public GuildKickPlayerReq(int playerId) : this(playerId, null) { }

	public GuildKickPlayerReq(int playerId, Func<bool> kickSuccessDel)
	{
		this.playerId = playerId;
		this.kickSuccessDel = kickSuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildKickPlayer(ID, playerId);
	}
}

// 变更职位
internal class GuildSetPlayerRoleReq : Request
{
	private Func<bool> setRoleSuccessDel;
	public Func<bool> SetRoleSuccessDel
	{
		get { return setRoleSuccessDel; }
	}

	private int playerId;
	private int roleId;

	public GuildSetPlayerRoleReq(int playerId, int roleId) : this(playerId, roleId, null) { }

	public GuildSetPlayerRoleReq(int playerId, int roleId, Func<bool> setRoleSuccessDel)
	{
		this.playerId = playerId;
		this.roleId = roleId;
		this.setRoleSuccessDel = setRoleSuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildSetPlayerRole(ID, playerId, roleId);
	}
}

// 更改自动批准
internal class GuildSetAutoEnterReq : Request
{
	private bool allowAutoEnter;

	private Func<bool> querySuccessDel;
	public Func<bool> QuerySuccessDel
	{
		get { return querySuccessDel; }
	}

	public GuildSetAutoEnterReq(bool allowAutoEnter) : this(allowAutoEnter, null) { }

	public GuildSetAutoEnterReq(bool allowAutoEnter, Func<bool> querySuccessDel)
	{
		this.allowAutoEnter = allowAutoEnter;
		this.querySuccessDel = querySuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildSetAutoEnter(ID, allowAutoEnter);
	}
}

// 门派建设请求
internal class QueryConstructionTaskReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryConstructionTask(ID);
	}
}

// 接受一个门派建设
internal class AcceptConstructionTaskReq : Request
{
	private int taskId;
	private int taskIndex;

	public AcceptConstructionTaskReq(int taskId, int taskIndex)
	{
		this.taskId = taskId;
		this.taskIndex = taskIndex;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.AcceptConstructionTask(ID, taskId, taskIndex);
	}
}

// 放弃一个门派建设
internal class GiveUpConstructionTaskReq : Request
{
	private int taskId;
	private int taskIndex;

	public GiveUpConstructionTaskReq(int taskId, int taskIndex)
	{
		this.taskId = taskId;
		this.taskIndex = taskIndex;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GiveUpConstructionTask(ID, taskId, taskIndex);
	}
}

// 完成一个门派建设
internal class CompleteConstructionTaskReq : Request
{
	private int taskId;
	private List<KodGames.ClientClass.Cost> costs;
	private int taskIndex;

	public CompleteConstructionTaskReq(int taskId, List<KodGames.ClientClass.Cost> costs, int taskIndex)
	{
		this.taskId = taskId;
		this.costs = costs;
		this.taskIndex = taskIndex;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.CompleteConstructionTask(ID, taskId, costs, taskIndex);
	}
}

// 手动刷新
internal class RefreshConstructionTaskReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.RefreshConstructionTask(ID);
	}
}

// 门派公共商品查询
internal class QueryGuildPublicShopReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryGuildPublicShop(ID);
	}
}

// 门派公共商品购买
internal class BuyGuildPublicGoodsReq : Request
{
	private int goodsId;

	public BuyGuildPublicGoodsReq(int goodsId)
	{
		this.goodsId = goodsId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BuyGuildPublicGoods(ID, goodsId);
	}
}

// 门派玩家商品查询
internal class QueryGuildPrivateShopReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryGuildPrivateShop(ID);
	}
}

// 门派玩家商品购买
internal class BuyGuildPrivateGoodsReq : Request
{
	private int goodsId;
	private int cout;

	public BuyGuildPrivateGoodsReq(int goodsId, int cout)
	{
		this.goodsId = goodsId;
		this.cout = cout;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BuyGuildPrivateGoods(ID, goodsId, cout);
	}
}

// 门派活动商品查询
internal class QueryGuildExchangeShopReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryGuildExchangeShop(ID);
	}
}

// 门派活动商品兑换
internal class ExchangeGuildExchangeGoodsReq : Request
{
	private int exchangeId;
	private List<KodGames.ClientClass.Cost> costs;

	public ExchangeGuildExchangeGoodsReq(int exchangeId, List<KodGames.ClientClass.Cost> costs)
	{
		this.exchangeId = exchangeId;
		this.costs = costs;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ExchangeGuildExchangeGoods(ID, exchangeId, costs);
	}
}

// 门派任务查询
internal class QueryGuildTaskReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryGuildTask(ID);
	}
}

// 门派任务投掷
internal class GuildTaskDiceReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildTaskDice(ID);
	}
}

// 门派任务刷新
internal class RefreshGuildTaskReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.RefreshGuildTask(ID);
	}
}
#endregion

#region 门派关卡
//开启门派关卡
internal class OpenGuildStageReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.OpenGuildStage(ID);
	}
}
//门派关卡主查询
internal class QueryGuildStageReq : Request
{
	private int type;
	public int Type
	{
		get { return type; }
	}

	public QueryGuildStageReq(int type)
	{
		this.type = type;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryGuildStage(ID, type);
	}
}
//移动并探索
internal class GuildStageExploreReq : Request
{
	private int moveIndex;
	private int exploreIndex;

	public GuildStageExploreReq(int moveIndex, int exploreIndex)
	{
		this.moveIndex = moveIndex;
		this.exploreIndex = exploreIndex;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageExplore(ID, moveIndex, exploreIndex);
	}
}
//挑战boss
internal class GuildStageCombatBossReq : Request
{
	private int exploreIndex;
	private int type;

	public GuildStageCombatBossReq(int exploreIndex, int type)
	{
		this.exploreIndex = exploreIndex;
		this.type = type;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageCombatBoss(ID, exploreIndex, type);
	}
}
//宝箱赠送
internal class GuildStageGiveBoxReq : Request
{
	private int playerId;

	private Func<bool> querySuccess;
	public Func<bool> QuerySuccess
	{
		get { return querySuccess; }
	}

	public GuildStageGiveBoxReq(int playerId, Func<bool> querySuccess)
	{
		this.playerId = playerId;
		this.querySuccess = querySuccess;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageGiveBox(ID, playerId);
	}
}
//手动重置协议
internal class GuildStageResetReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageReset(ID);
	}
}
//查询门派关卡奖励消息
internal class GuildStageQueryMsgReq : Request
{
	private int type;

	public GuildStageQueryMsgReq(int type)
	{
		this.type = type;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageQueryMsg(ID, type);
	}
}
//查询门派boss伤害排行
internal class GuildStageQueryBossRankReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageQueryBossRank(ID);
	}
}
//查询门派探索排行
internal class GuildStageQueryExploreRankReq : Request
{
	private int type;

	public GuildStageQueryExploreRankReq(int type)
	{
		this.type = type;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageQueryExploreRank(ID, type);
	}
}
//查询门派boss伤害排行
internal class GuildStageQueryBossRankDetailReq : Request
{
	private int type;
	private int num;

	public GuildStageQueryBossRankDetailReq(int type, int num)
	{
		this.type = type;
		this.num = num;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageQueryBossRankDetail(ID, type, num);
	}
}
//查询门派间排行
internal class GuildStageQueryRankReq : Request
{
	private int rankType;
	private Func<com.kodgames.corgi.protocol.Rank, List<com.kodgames.corgi.protocol.Rank>, bool> querySuccess;
	public Func<com.kodgames.corgi.protocol.Rank, List<com.kodgames.corgi.protocol.Rank>, bool> QuerySuccess
	{
		get { return querySuccess; }
	}

	public GuildStageQueryRankReq(int rankType, Func<com.kodgames.corgi.protocol.Rank, List<com.kodgames.corgi.protocol.Rank>, bool> querySuccess)
	{
		this.rankType = rankType;
		this.querySuccess = querySuccess;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageQueryRank(ID, rankType);
	}

}
//查询天赋信息
internal class GuildStageQueryTalentReq : Request
{
	private int type;

	public GuildStageQueryTalentReq(int type)
	{
		this.type = type;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageQueryTalent(ID, type);
	}
}
//重置天赋
internal class GuildStageTalentResetReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageTalentReset(ID);
	}
}
//天赋加点
internal class GuildStageTalentAddReq : Request
{
	private int type;
	private int talentId;

	public GuildStageTalentAddReq(int type, int talentId)
	{
		this.type = type;
		this.talentId = talentId;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.GuildStageTalentAdd(ID, type, talentId);
	}
}
#endregion

#region 新马版新功能协议

internal class QueryFacebookReq : Request
{
	public QueryFacebookReq()
	{

	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryFacebookReq(ID);
	}
}

internal class FacebookRewardReq : Request
{
	public FacebookRewardReq()
	{
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.FacebookRewardReq(ID);
	}
}


#endregion

#region 修改玩家名称 和 门派名称

internal class SetPlayerNameReq : Request
{
	private string playerName;

	public SetPlayerNameReq() { }

	public SetPlayerNameReq(string playerName)
	{
		this.playerName = playerName;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SetPlayerNameReq(ID, playerName);
	}
}

internal class SetGuildNameReq : Request
{
	private string guildName;

	public SetGuildNameReq() { }

	public SetGuildNameReq(string guildName)
	{
		this.guildName = guildName;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.SetGuildNameReq(ID, guildName);
	}
}

#endregion

#region 机关兽

internal class ActiveBeastReq : Request
{
	private int id;
	public int BeastId
	{
		get { return id; }
	}

	public ActiveBeastReq(int id)
	{
		this.id = id;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ActiveBeast(ID, id);
	}
}

internal class EquipBeastPartReq : Request
{
	private string guid;
	private int index;
	public int PartType
	{
		get { return index; }
	}

	public EquipBeastPartReq(string guid, int index)
	{
		this.guid = guid;
		this.index = index;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.EquipBeastPart(ID, guid, index);
	}
}

internal class BeastBreakthoughtReq : Request
{
	private string guid;

	private KodGames.ClientClass.Beast oldBeast;
	public KodGames.ClientClass.Beast OldBeast
	{
		get { return oldBeast; }
	}

	public BeastBreakthoughtReq(string guid, KodGames.ClientClass.Beast oldBeast)
	{
		this.guid = guid;
		this.oldBeast = oldBeast;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BeastBreakthought(ID, guid);
	}
}

internal class BeastLevelUpReq : Request
{
	private string guid;

	private KodGames.ClientClass.Beast oldBeast;
	public KodGames.ClientClass.Beast OldBeast
	{
		get { return oldBeast; }
	}

	public BeastLevelUpReq(string guid, KodGames.ClientClass.Beast oldBeast)
	{
		this.guid = guid;
		this.oldBeast = oldBeast;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BeastLevelUp(ID, guid);
	}
}

internal class QueryBeastExchangeShopReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryBeastExchangeShop(ID);
	}
}

internal class BeastExchangeShopReq : Request
{
	private int exchangeId;
	private int index;
	private List<KodGames.ClientClass.Cost> costs;

	private Func<KodGames.ClientClass.Reward, bool> queryDel;
	public Func<KodGames.ClientClass.Reward, bool> QueryDel { get { return queryDel; } }

	public BeastExchangeShopReq(int exchangeId, int index, List<KodGames.ClientClass.Cost> costs, Func<KodGames.ClientClass.Reward, bool> queryDel)
	{
		this.exchangeId = exchangeId;
		this.index = index;
		this.costs = costs;
		this.queryDel = queryDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BeastExchangeShop(ID, exchangeId, index, costs);
	}
}

internal class RefreshBeastExchangeShopReq : Request
{
	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.RefreshBeastExchangeShop(ID);
	}
}
#endregion