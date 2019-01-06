using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using KodGames;
using KodGames.ClientHelper;
using KodGames.ClientClass;
using ClientServerCommon;

class Response
{
	private bool errOcc; // Error occurred.
	private int errCode;
	private string errMsg; // Error message.

	public override string ToString()
	{
		return string.Format("{0} (RequestID:{1}) {2}", this.GetType(), requestID, errMsg);
	}

	// The request id to this response.
	private int requestID;
	public int ReuqestID
	{
		get { return requestID; }
	}

	// IsExecuted state.
	private bool executed;
	public bool IsExecuted
	{
		get { return executed; }
	}

	public Response(int pRqstID)
	{
		requestID = pRqstID;
	}

	public Response(int pRqstID, int errCode, string errMsg)
	{
		requestID = pRqstID;

		errOcc = true;
		this.errCode = errCode;
		this.errMsg = errMsg;
	}

	public virtual bool Execute(Request request)
	{
		if (IsExecuted)
			Debug.LogError("Execute executed response " + this.ToString());

		executed = true;

		if (errOcc)
			PrcErr(request, errCode, errMsg);

		return !errOcc;
	}

	protected virtual void PrcErr(Request request, int errCode, string errMsg)
	{
		if (string.IsNullOrEmpty(errMsg) == false)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errMsg);
	}
}

#region Connect Response
class ConnectISRes : Response
{
	public ConnectISRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public ConnectISRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var srcReq = request as ConnectISReq;

		// Set last area
		SysLocalDataBase.Inst.LoginInfo.LastAreaId = srcReq.AreaID;
		SysLocalDataBase.Inst.LoginInfo.LoginArea = SysLocalDataBase.Inst.LoginInfo.SearchArea(srcReq.AreaID);

		if (srcReq.OnIsConnectedSuccessDel != null)
			srcReq.OnIsConnectedSuccessDel();

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		// 这条协议不会产生提示热更新的返回值
		switch (errCode)
		{
			case com.kodgames.corgi.protocol.Protocols.E_INTERFACE_AUTH_TOKEN_MAX_ONLINE_NUM:
			case com.kodgames.corgi.protocol.Protocols.E_INTERFACE_AUTH_TOKEN_MAX_DEVICE_NUM:
			case com.kodgames.corgi.protocol.Protocols.E_INTERFACE_AUTH_TOKEN_MAX_ACCOUNT_NUM:
				base.PrcErr(request, errCode, errMsg);
				break;

			default:
				var srcReq = request as ConnectISReq;

				if (srcReq.OnIsConnectedFailedDel != null)
					srcReq.OnIsConnectedFailedDel(errCode, errMsg);

				break;
		}
	}
}
#endregion

#region GameServer Response
class QueryInitInfoRes : Response
{
	private KodGames.ClientClass.Player player;
	private List<com.kodgames.corgi.protocol.Notice> notices;
	private bool autoShowNotice;
	private List<com.kodgames.corgi.protocol.ActivityData> activityData;
	private KodGames.ClientClass.Function functionOpen;
	private int serverType;
	private int assistantNum;

	public QueryInitInfoRes(int pRqstID, KodGames.ClientClass.Player player, List<com.kodgames.corgi.protocol.ActivityData> activityData, int serverType, List<com.kodgames.corgi.protocol.Notice> notices, bool autoShowNotice, KodGames.ClientClass.Function functionOpen, int assistantNum)
		: base(pRqstID)
	{
		this.player = player;
		this.activityData = activityData;
		this.serverType = serverType;
		this.notices = notices;
		this.autoShowNotice = autoShowNotice;
		this.functionOpen = functionOpen;
		this.assistantNum = assistantNum;
	}

	public QueryInitInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// 埋点
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.QueryInitInfoSuccess);

		Platform.Instance.ServerType = serverType;

		// Add ActivityManager
		SysModuleManager.Instance.AddSysModule<ActivityManager>(true);

		SysLocalDataBase localDB = SysLocalDataBase.Inst;

		// Save login time
		localDB.LoginInfo.ServerTimeZone = player.TimeZone;
		localDB.LoginInfo.LoginTime = player.LoginTime;

		// Set the local player data.
		player.Stamina.MaxPoint = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(player.VipLevel, VipConfig._VipLimitType.MaxStamina);
		player.Stamina.GenerateTime = ConfigDatabase.DefaultCfg.GameConfig.staminaGenerateTime;
		player.Energy.MaxPoint = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(player.VipLevel, VipConfig._VipLimitType.MaxEnergy);
		player.Energy.GenerateTime = ConfigDatabase.DefaultCfg.GameConfig.energyGenerateTime;
		player.Function = functionOpen;
		player.QinInfoAnswerCount.MaxPoint = ConfigDatabase.DefaultCfg.QinInfoConfig.MaxQinAnswerCount;
		player.QinInfoAnswerCount.GenerateTime = ConfigDatabase.DefaultCfg.QinInfoConfig.RecoverTime;
		localDB.LocalPlayer = player;

		// Save Login Time For StartServerReward Activity.
		if (localDB.LocalPlayer.StartServerRewardInfo != null)
			localDB.LocalPlayer.StartServerRewardInfo.LoginTime = player.LoginTime;

		// Remove Consumable if the count is 0.
		for (int i = localDB.LocalPlayer.Consumables.Count - 1; i >= 0; i--)
			if (localDB.LocalPlayer.Consumables[i].Amount == 0)
				localDB.LocalPlayer.Consumables.RemoveAt(i);

		// Email
		player.EmailData.SetNewEmailCount(_EmailDisplayType.System, player.NewSystemEmailCount);
		player.EmailData.SetNewEmailCount(_EmailDisplayType.Friend, player.NewFriendEmailCount);
		player.EmailData.SetNewEmailCount(_EmailDisplayType.Combat, player.NewCombatEmailCount);
		player.EmailData.SetNewEmailCount(_EmailDisplayType.Guild, player.NewGuildEmailCount);

		// Sort Tutorial.
		if (localDB.LocalPlayer.UnDoneTutorials != null && localDB.LocalPlayer.UnDoneTutorials.Count > 0)
			localDB.LocalPlayer.UnDoneTutorials.Sort();

		// CLose Tutorial by config.
#if !DISABLE_TUTORIAL
		bool openTutorial = ConfigDatabase.DefaultCfg.TutorialConfig.campaignGuid.openTutorial;
		if (openTutorial == false)
#endif
			localDB.LocalPlayer.UnDoneTutorials = new List<int>();

#if UNITY_EDITOR
		var sb = new System.Text.StringBuilder();
		sb.AppendLine("UnDoneTutorials");
		foreach (var id in player.UnDoneTutorials)
			sb.AppendLine(id.ToString("X"));
		Debug.Log(sb);
#endif

		ActivityManager.Instance.SetData(activityData);

		localDB.UpdateNotifyData(assistantNum, activityData, localDB.LocalPlayer.FunctionStates, null, localDB.LocalPlayer.QuestData.QuestQuick);
		localDB.RegistrLocalNotify();

		// Notice
		localDB.LocalPlayer.NoticeData.AutoShowNotice = autoShowNotice;
		localDB.LocalPlayer.NoticeData.Notices = notices;

		// Platform Upload GameData.
		if (!localDB.LocalPlayer.UnDoneTutorials.Contains(ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.selectPlayerAvatarTutorialId))
			Platform.Instance.UploadGameData(false);

		// Handler the success response.
		SysGameStateMachine.Instance.GetCurrentState<GameState_RetriveGameData>().OnQueryInitInfoSuccess(localDB.LocalPlayer);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		// Handler the failed response. 直接在GameState_RetriveGameData中处理, 不需要
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_INIT_INFO_ISFROZEN)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), errMsg);
			SysGameStateMachine.Instance.EnterState<GameState_Upgrading>();
		}
		else
			SysGameStateMachine.Instance.GetCurrentState<GameState_RetriveGameData>().OnQueryInitInfoFaild(errMsg);
	}
}
#endregion

#region  Position Response

class QueryPositionListRes : Response
{
	private KodGames.ClientClass.PositionData positonData;

	public QueryPositionListRes(int pRqstID, KodGames.ClientClass.PositionData positonData)
		: base(pRqstID)
	{
		this.positonData = positonData;
	}

	public QueryPositionListRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.PositionData = positonData;

		return true;
	}
}

class OpenPositionRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardSync;
	private KodGames.ClientClass.Position position;

	public OpenPositionRes(int pRqstID, KodGames.ClientClass.CostAndRewardAndSync costAndRewardSync, KodGames.ClientClass.Position position)
		: base(pRqstID)
	{
		this.costAndRewardSync = costAndRewardSync;
		this.position = position;
	}

	public OpenPositionRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public OpenPositionRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardSync = costAndRewardSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost and Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardSync, "OpenPosition");

		SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions.Add(position);

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().OnOpenPositionSuccess(position.PositionId);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_OPEN_POSITION_FAILED_CHANGE_COST_NOT_ENOUGH && costAndRewardSync != null && costAndRewardSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardSync.NotEnoughCost.Id, costAndRewardSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class ChangeLocationRes : Response
{
	private KodGames.ClientClass.Position position;
	public ChangeLocationRes(int pRqstID, KodGames.ClientClass.Position position)
		: base(pRqstID)
	{
		this.position = position;
	}

	public ChangeLocationRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var changeLocationReq = request as ChangeLocationReq;
		SysLocalDataBase.Inst.LocalPlayer.PositionData.PutPosition(position);

		// Create location.
		KodGames.ClientClass.Location location = null;
		List<KodGames.ClientClass.Location> locations = null;

		switch (IDSeg.ToAssetType(changeLocationReq.ResourceId))
		{
			case IDSeg._AssetType.Avatar:
				locations = position.AvatarLocations;
				break;

			case IDSeg._AssetType.Equipment:
				locations = position.EquipLocations;
				break;

			case IDSeg._AssetType.Dan:
				locations = position.DanLocations;
				break;

			case IDSeg._AssetType.CombatTurn:
				locations = position.SkillLocations;
				break;

			case IDSeg._AssetType.Beast:
				locations = position.BeastLocations;
				break;
		}

		for (int i = 0; i < locations.Count; i++)
		{
			if (locations[i].ResourceId == changeLocationReq.ResourceId &&
			   locations[i].ShowLocationId == changeLocationReq.Location)
			{
				location = locations[i];
				break;
			}
		}

		// Notify UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectEquipmentList)))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectEquipmentList>().OnChangeEquipmentSuccess(location);
		else if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectDanList)))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectDanList>().OnChangeDanSuccess(location);
		else if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectSkillList)))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectSkillList>().OnChangeSkillSuccess(location);
		else if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectAvatarList)))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectAvatarList>().OnChangeAvatarSuccess(location);
		else if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectOrganList)))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectOrganList>().OnChangeBeastSuccess(location);

		return true;
	}
}

class OneClickPositionOffRes : Response
{
	private KodGames.ClientClass.Position position;
	public OneClickPositionOffRes(int pRqstID, KodGames.ClientClass.Position position)
		: base(pRqstID)
	{
		this.position = position;
	}

	public OneClickPositionOffRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.PositionData.PutPosition(position);

		SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().OnOneKeyOffSucess();
		return true;
	}
}


class SetMasterPositionRes : Response
{
	public SetMasterPositionRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public SetMasterPositionRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().OnSetMasterSuccess(SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId = (request as SetMasterPositionReq).PositionId;

		return true;
	}
}

class EmBattleRes : Response
{
	public EmBattleRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public EmBattleRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		return true;
	}
}
#endregion

#region Partner
//开启助阵
class PartnerOpenRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public PartnerOpenRes(int pRqstID, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public PartnerOpenRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public PartnerOpenRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		PartnerOpenReq partnerReq = request as PartnerOpenReq;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "PartnerOpen");

		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions.Count; i++)
		{
			var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions[i];

			KodGames.ClientClass.Partner partner = new KodGames.ClientClass.Partner();
			partner.PartnerId = partnerReq.PartnerId;
			partner.AvatarGuid = string.Empty;

			position.Partners.Add(partner);
		}

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().OnActivePartnerSuccess(partnerReq.PartnerId);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_PARTNER_OPEN_CONSUME_NOT_ENOUGH_FAILED && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class PartnerSetupRes : Response
{
	private List<KodGames.ClientClass.Partner> partners;

	public PartnerSetupRes(int pRqstID, List<KodGames.ClientClass.Partner> partners)
		: base(pRqstID)
	{
		this.partners = partners;
	}

	public PartnerSetupRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		PartnerSetupReq partnerSetupReq = request as PartnerSetupReq;
		var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(partnerSetupReq.PositionId);

		for (int i = 0; i < partners.Count; i++)
		{
			var partner = position.GetPartnerById(partners[i].PartnerId);
			if (partner == null)
			{
				partner = new KodGames.ClientClass.Partner();
				position.Partners.Add(partner);
			}

			partner.ShallowCopy(partners[i]);
		}

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectAvatarList)))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectAvatarList>().OnChangeParterSuccess(partnerSetupReq.PartnerId, partnerSetupReq.AvatarOffGuid, partnerSetupReq.AvatarOnGuid, partners);

		return true;
	}
}
#endregion

#region  Hire
class QueryDinerListRes : Response
{
	private List<KodGames.ClientClass.DinerPackage> dinerPackages;

	public QueryDinerListRes(int pRqstID, List<KodGames.ClientClass.DinerPackage> dinerPackages)
		: base(pRqstID)
	{
		this.dinerPackages = dinerPackages;
	}

	public QueryDinerListRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Set DinerPackage datas.
		SysLocalDataBase.Inst.LocalPlayer.HireDinerData.SetDinerPackage(dinerPackages);

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDiner)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>().OnQueryDinerListSuccess();

		return true;
	}
}

//门客馆雇佣
class HireDinerRes : Response
{
	private KodGames.ClientClass.HiredDiner hiredDiner;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public HireDinerRes(int pRqstID, KodGames.ClientClass.HiredDiner hiredDiner, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.hiredDiner = hiredDiner;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public HireDinerRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost and Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "HireDiner");

		// Update Data.
		HireDinerReq hireReq = request as HireDinerReq;

		var diner = SysLocalDataBase.Inst.LocalPlayer.HireDinerData.GetHiredDiner(hireReq.QualityType, hireReq.DinerId);
		if (diner != null)
			diner.ShallowCopy(hiredDiner);
		else
			SysLocalDataBase.Inst.LocalPlayer.HireDinerData.HireDiners.Add(hiredDiner);

		// Update UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDiner)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>().OnHireDinerSuccess(hireReq.DinerId);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_HIRE_DINER_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class RenewDinerRes : Response
{
	private KodGames.ClientClass.HiredDiner hiredDiner;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public RenewDinerRes(int pRqstID, KodGames.ClientClass.HiredDiner hiredDiner, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.hiredDiner = hiredDiner;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public RenewDinerRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost and Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "RenewDiner");

		// Update Data.
		RenewDinerReq renewReq = request as RenewDinerReq;
		var diner = SysLocalDataBase.Inst.LocalPlayer.HireDinerData.GetHiredDiner(renewReq.QualityType, renewReq.DinerId);
		if (diner != null)
			diner.ShallowCopy(hiredDiner);
		else
			SysLocalDataBase.Inst.LocalPlayer.HireDinerData.HireDiners.Add(hiredDiner);

		// Update UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDiner)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>().OnRenewDinerSuccess(renewReq.DinerId);

		return true;
	}
}

class FireDinerRes : Response
{
	public FireDinerRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public FireDinerRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		FireDinerReq fireReq = request as FireDinerReq;

		// Delete Hired Diner.
		string avatarGuid = string.Empty;
		foreach (var diner in SysLocalDataBase.Inst.LocalPlayer.HireDinerData.HireDiners)
		{
			if (diner.DinerId == fireReq.DinerId && diner.QualityType == diner.QualityType)
			{
				avatarGuid = diner.AvatarGuid;
				SysLocalDataBase.Inst.LocalPlayer.HireDinerData.HireDiners.Remove(diner);
				break;
			}
		}

		// Delete Avatar.
		SysLocalDataBase.Inst.LocalPlayer.RemoveAvatar(avatarGuid);

		// Delete Position.
		foreach (var position in SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions)
		{
			foreach (var avatarLocation in position.AvatarLocations)
			{
				if (avatarLocation.Guid.Equals(avatarGuid))
				{
					position.AvatarLocations.Remove(avatarLocation);
					break;
				}
			}
		}

		// Update UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDiner)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>().OnFireNpcSuccess();

		return true;
	}
}

//门客馆刷新
class RefreshDinerListRes : Response
{
	private KodGames.ClientClass.DinerPackage dinerPackage;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public RefreshDinerListRes(int pRqstID, KodGames.ClientClass.DinerPackage dinerPackage, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.dinerPackage = dinerPackage;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public RefreshDinerListRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public RefreshDinerListRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost and Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "RefreshDinerList");

		// Update Data.
		RefreshDinerListReq refreshReq = request as RefreshDinerListReq;
		var bagCfg = ConfigDatabase.DefaultCfg.DinerConfig.GetDinerBagById(refreshReq.BagId);

		var package = SysLocalDataBase.Inst.LocalPlayer.HireDinerData.GetDinerPackageByQualityType(bagCfg.QualityType);

		if (package == null)
			SysLocalDataBase.Inst.LocalPlayer.HireDinerData.DinerPackages.Add(dinerPackage);
		else
			package.ShallowCopy(dinerPackage);

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDiner)) && !SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>().IsOverlayed)
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>().OnRefreshListSuccess(bagCfg.RefreshType, bagCfg.QualityType);
		else if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDinerRefresh)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarDinerRefresh>().OnRefreshListSuccess(bagCfg.RefreshType, bagCfg.QualityType);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_DINER_LIST_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

#endregion

#region Avatar

//角色升级
class AvatarLevelUpRes : Response
{
	private int levelAfter;
	private int critCount;
	private CostAndRewardAndSync costAndRewardAndSync;

	public AvatarLevelUpRes(int pRqstID, int levelAfter, CostAndRewardAndSync costAndRewardAndSync, int critCount)
		: base(pRqstID)
	{
		this.levelAfter = levelAfter;
		this.critCount = critCount;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public AvatarLevelUpRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Use cost.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "AvatarLevelUp");

		AvatarLevelUpReq req = request as AvatarLevelUpReq;

		// Handler the success response.
		KodGames.ClientClass.Avatar powerUpAvatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(req.AvatarGUID);

		int preLevel = powerUpAvatar.LevelAttrib.Level;

		// Change the power up avatar level attribute.
		powerUpAvatar.LevelAttrib.Level = levelAfter;

		// Notify UI.
		SysUIEnv.Instance.GetUIModule<UIPnlAvatarLevelUp>().OnAvatarLevelUpResponse(powerUpAvatar, req.LevelUpType, preLevel, critCount, costAndRewardAndSync);
		SysUIEnv.Instance.GetUIModule<UIPnlAvatarPowerUpTab>().SetNotifyState(_UIType.UIPnlAvatarLevelUp);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_AVATAR_LEVEL_UP_FAILED_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

//角色突破
class AvatarBreakthoughtRes : Response
{
	private int breakThoughLevel;
	private CostAndRewardAndSync costAndRewardAndSync;

	public AvatarBreakthoughtRes(int pRqstID, int breakThoughLevel, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.breakThoughLevel = breakThoughLevel;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public AvatarBreakthoughtRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		AvatarBreakthoughtReq req = request as AvatarBreakthoughtReq;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "AvatarBreakthought");

		// Get original data
		var avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(req.AvatarGUID);

		// Update avatar data.
		avatar.BreakthoughtLevel = breakThoughLevel;

		// Set breakthrough result controls before updating original avatar data.
		SysUIEnv.Instance.GetUIModule<UIPnlAvatarBreakThrough>().OnBreakThroughSuccess();
		SysUIEnv.Instance.GetUIModule<UIPnlAvatarPowerUpTab>().SetNotifyState(_UIType.UIPnlAvatarBreakThrough);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_AVATAR_BREAKTHOUGHT_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

//洗脉或易经
class ChangeMeridianRes : Response
{
	private List<KodGames.ClientClass.PropertyModifier> newModifiers;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private int meridianTimes;
	private int bufferId;

	public ChangeMeridianRes(int pRqstID, List<KodGames.ClientClass.PropertyModifier> newModifiers, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, int meridianTimes, int bufferId)
		: base(pRqstID)
	{
		this.newModifiers = newModifiers;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.meridianTimes = meridianTimes;
		this.bufferId = bufferId;
	}

	public ChangeMeridianRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		ChangeMeridianReq changeMeridianReq = request as ChangeMeridianReq;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "ChangeMeridian");

		// Refresh Data.
		if (SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(changeMeridianReq.AvatarGuid).GetMeridianByID(changeMeridianReq.MeridianId) == null)
		{
			SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(changeMeridianReq.AvatarGuid).SaveMeridianData(changeMeridianReq.MeridianId, newModifiers, bufferId);
		}

		SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(changeMeridianReq.AvatarGuid).GetMeridianByID(changeMeridianReq.MeridianId).Times = meridianTimes;

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIDlgAvatarMeridian))
			SysUIEnv.Instance.GetUIModule<UIDlgAvatarMeridian>().OnChangeMeridianSuccess(newModifiers);
		else
		{
			if (meridianTimes > 1)
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgAvatarMeridian, SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(changeMeridianReq.AvatarGuid), changeMeridianReq.MeridianId, true, false);
			else
			{
				SysUIEnv.Instance.GetUIModule<UIPnlAvatarMeridianTab>().UpDataMeridianUI(changeMeridianReq.MeridianId, true);
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgAvatarMeridian, SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(changeMeridianReq.AvatarGuid), changeMeridianReq.MeridianId, false, true);
			}
		}

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if ((errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_CHANGE_MEIRIDIAN_FAILED_COST_NOT_ENOUGH || errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_CHANGE_MEIRIDIAN_FAILED_CHANGE_COST_NOT_ENOUGH)
			&& costAndRewardAndSync != null
			&& costAndRewardAndSync.NotEnoughCost != null)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIDlgAvatarMeridian))
				SysUIEnv.Instance.HideUIModule(_UIType.UIDlgAvatarMeridian);

			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		}
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class SaveMeridianRes : Response
{
	private List<KodGames.ClientClass.PropertyModifier> newModifiers;
	private int bufferId;

	public SaveMeridianRes(int pRqstID, List<KodGames.ClientClass.PropertyModifier> newModifiers, int bufferId)
		: base(pRqstID)
	{
		this.newModifiers = newModifiers;
		this.bufferId = bufferId;
	}

	public SaveMeridianRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SaveMeridianReq meridianReq = request as SaveMeridianReq;

		// Refresh Data.
		if (meridianReq.SaveOrNot)
			SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(meridianReq.AvatarGuid).SaveMeridianData(meridianReq.MeridianId, newModifiers, bufferId);

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIDlgAvatarMeridian))
			SysUIEnv.Instance.GetUIModule<UIDlgAvatarMeridian>().OnSaveMeridianSuccess(meridianReq.SaveOrNot, SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(meridianReq.AvatarGuid).GetMeridianByID(meridianReq.MeridianId).Modifiers);

		return true;
	}
}

//洗炼霸气
class ChangeDomineerRes : Response
{
	private KodGames.ClientClass.Avatar changedAvatar;
	private CostAndRewardAndSync costSync;

	public ChangeDomineerRes(int pRqstID, KodGames.ClientClass.Avatar changedAvatar, CostAndRewardAndSync costSync)
		: base(pRqstID)
	{
		this.changedAvatar = changedAvatar;
		this.costSync = costSync;
	}

	public ChangeDomineerRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public ChangeDomineerRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costSync = costSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costSync, "ChangeDomineer");
		SysLocalDataBase.Inst.LocalPlayer.RemoveAvatar(changedAvatar.Guid);
		SysLocalDataBase.Inst.LocalPlayer.AddAvatar(changedAvatar);

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDomineerTab)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarDomineerTab>().OnChangeDomineerResponse(changedAvatar);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_CHANGE_DOMINEER_FAILED_CONSUMABLE_IS_NOT_ENOUGH && costSync != null && costSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costSync.NotEnoughCost.Id, costSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class SaveDomineerRes : Response
{
	private KodGames.ClientClass.Avatar changedAvatar;

	public SaveDomineerRes(int pRqstID, KodGames.ClientClass.Avatar changedAvatar)
		: base(pRqstID)
	{
		this.changedAvatar = changedAvatar;
	}

	public SaveDomineerRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.RemoveAvatar(changedAvatar.Guid);
		SysLocalDataBase.Inst.LocalPlayer.AddAvatar(changedAvatar);

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDomineerTab)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarDomineerTab>().OnSaveDomineerResponse(changedAvatar);

		return true;
	}
}

#endregion

#region Equipment response

//装备精炼
class EquipmentBreakthoutRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private Equipment equipment;

	public EquipmentBreakthoutRes(int pRqstID, Equipment equipment, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.equipment = equipment;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public EquipmentBreakthoutRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process CostAndReward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "EquipmentBreakthout");

		var equip = SysLocalDataBase.Inst.LocalPlayer.SearchEquipment((request as EquipmentBreakthoutReq).EquipGUID);
		equip.BreakthoughtLevel = equipment.BreakthoughtLevel;

		SysUIEnv.Instance.GetUIModule<UIPnlEquipmentRefine>().OnResponseEquipmentBreakthoutSuccess();
		SysUIEnv.Instance.GetUIModule<UIPnlEquipmentPowerUpTab>().SetNotifyState(_UIType.UIPnlEquipmentRefine);
		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_EQUIP_BREAKTHOUGHT_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

//装备升级
class EquipmentLevelUpRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private int levelAfter;
	private int critCount;

	public EquipmentLevelUpRes(int pRqstID, int levelAfter, CostAndRewardAndSync costAndRewardAndSync, int critCount)
		: base(pRqstID)
	{
		this.critCount = critCount;
		this.levelAfter = levelAfter;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public EquipmentLevelUpRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process CostAndReward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "EquipmentLevelUp");

		var equipLevelReq = request as EquipmentLevelUpReq;

		// Pre Level.
		int preLevel = SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(equipLevelReq.EquipGuid).LevelAttrib.Level;

		SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(equipLevelReq.EquipGuid).LevelAttrib.Level = levelAfter;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEquipmentLevelup))
		{
			SysUIEnv.Instance.GetUIModule<UIPnlEquipmentLevelup>().OnReponseEquipLevelUpSuccess(equipLevelReq.LevelType, preLevel, critCount, costAndRewardAndSync.Costs[0]);
			SysUIEnv.Instance.GetUIModule<UIPnlEquipmentPowerUpTab>().SetNotifyState(_UIType.UIPnlEquipmentLevelup);
		}

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_EQUIP_LEVEL_UP_FAILED_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

#endregion

#region Shop
class QueryGoodsRes : Response
{
	private List<KodGames.ClientClass.Goods> goods;
	private long nextRefreshTime;
	private bool isMelaleucaShopOpen;

	public QueryGoodsRes(int pRqstID, List<KodGames.ClientClass.Goods> goods, long nextRefreshTime, bool isMelaleucaShopOpen)
		: base(pRqstID)
	{
		this.goods = goods;
		this.nextRefreshTime = nextRefreshTime;
		this.isMelaleucaShopOpen = isMelaleucaShopOpen;
	}

	public QueryGoodsRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Set local data
		SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods = goods;
		SysLocalDataBase.Inst.LocalPlayer.ShopData.NextRefreshTime = nextRefreshTime;

		// CallBack.
		QueryGoodsReq queryGoodReq = request as QueryGoodsReq;
		if (queryGoodReq.QueryGoodsSuccessDel != null)
			queryGoodReq.QueryGoodsSuccessDel();

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlTowerShop))
			SysUIEnv.Instance.GetUIModule<UIPnlTowerShop>().OpenShop(isMelaleucaShopOpen);


		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		// Set empty goods list
		SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods = new List<KodGames.ClientClass.Goods>();

		base.PrcErr(request, errCode, errMsg);
	}
}

//购买物品 * 比武场兑换物品
class BuyGoodsRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.Goods good;
	private string notEnoughText;

	public BuyGoodsRes(int pRqstID, KodGames.ClientClass.Goods good, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, string notEnoughText)
		: base(pRqstID)
	{
		this.good = good;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.notEnoughText = notEnoughText;
	}

	public BuyGoodsRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.Goods good, CostAndRewardAndSync costAndRewardAndSync, string notEnoughText)
		: base(pRqstID, errCode, errMsg)
	{
		this.good = good;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.notEnoughText = notEnoughText;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		BuyGoodsReq buyGoodsReq = request as BuyGoodsReq;

		// Use the cost.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, string.Format("BuyGoods_{0}", buyGoodsReq.GoodsId.ToString("X")));

		// Refresh Good.
		SysLocalDataBase.Inst.LocalPlayer.ShopData.UpdateGoodData(good);

		// Callback
		if (buyGoodsReq.BuyGoodsSuccessDel != null)
			buyGoodsReq.BuyGoodsSuccessDel(buyGoodsReq.GoodsId, buyGoodsReq.Amount, costAndRewardAndSync.Reward, costAndRewardAndSync.Costs);
		else if (buyGoodsReq.BuyGachaKeySuccessDel != null)
			buyGoodsReq.BuyGachaKeySuccessDel(buyGoodsReq.GoodsId, buyGoodsReq.Amount, costAndRewardAndSync);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (!string.IsNullOrEmpty(notEnoughText))
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), notEnoughText);
		else if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GOODS_GOOD_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
		{
			if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GOODS_FAILED_REFRESH_GOODS && good != null)
			{
				// 更新商品状态
				SysLocalDataBase.Inst.LocalPlayer.ShopData.UpdateGoodData(good);

				BuyGoodsReq buyGoodsReq = request as BuyGoodsReq;
				if (buyGoodsReq.BuyGoodsFailed_UpdateStatusDel != null)
					buyGoodsReq.BuyGoodsFailed_UpdateStatusDel(good.GoodsID);
			}

			base.PrcErr(request, errCode, errMsg);
		}
	}
}

class BuyAndUseRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.Goods good;

	public BuyAndUseRes(int pRqstID, KodGames.ClientClass.Goods good, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.good = good;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public BuyAndUseRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		BuyAndUseReq rqst = request as BuyAndUseReq;

		//Use the cost
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, string.Format("BuyAndUse_{0}", rqst.GoodsId.ToString("X")));

		// Update good remain count;
		SysLocalDataBase.Inst.LocalPlayer.ShopData.UpdateGoodData(good);

		// 更新购买并使用UI 现在购买并使用不包括烤山鸡 体力购买走BuySpecialGoodsRes
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgAffordCost)))
		{
			KodGames.ClientClass.Reward reward = costAndRewardAndSync.Reward;

			if (rqst.BuyAndUseCallBack != null)
				rqst.BuyAndUseCallBack(reward);

			//foreach (GameConfig.RecoveryItem recoveryItem in ConfigDatabase.DefaultCfg.GameConfig.recoveryConfig.recoveryItems)
			//    if (good.GoodsID == recoveryItem.goodsId)
			//    {
			//        SysUIEnv.Instance.GetUIModule<UIDlgAffordCost>().SetUICtrls(recoveryItem.assetId);
			//        break;
			//    }
		}

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_AND_USE_CONSUMABLE_IS_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
		{
			if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_AND_USE_FAILED_REFRESH_GOODS && good != null)
				// 更新商品状态
				SysLocalDataBase.Inst.LocalPlayer.ShopData.UpdateGoodData(good);

			base.PrcErr(request, errCode, errMsg);
		}
	}
}

class BuySpecialGoodsRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public BuySpecialGoodsRes(int pRqstId, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public BuySpecialGoodsRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		BuySpecialGoodsReq rqst = request as BuySpecialGoodsReq;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, string.Format("BuySpecialGoods_{0}", rqst.GoodsId.ToString("X")));

		if (rqst.OnBuySuccessDel != null)
			rqst.OnBuySuccessDel(rqst.GoodsCount);

		return true;
	}

}
#endregion

#region Skill response

//书籍升级
class SkillLevelUpRes : Response
{
	private Skill skill;
	private CostAndRewardAndSync costAndRewardAndSync;
	public SkillLevelUpRes(int rqstID, Skill skill, CostAndRewardAndSync costAndRewardAndSync)
		: base(rqstID)
	{
		this.skill = skill;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public SkillLevelUpRes(int rqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(rqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request)) return false;

		// Process CostAndReward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "SkillLevelUp");

		// Find Origin Skill Data.
		var skillToChange = SysLocalDataBase.Inst.LocalPlayer.SearchSkill(skill.Guid);

		// Save Origin Level Data.
		LevelAttrib orgAttrib = new LevelAttrib();
		orgAttrib.Level = skillToChange.LevelAttrib.Level;
		orgAttrib.Experience = skillToChange.LevelAttrib.Experience;

		// Change Skill's Level.
		skillToChange.LevelAttrib = skill.LevelAttrib;

		// Notify UI.
		SysUIEnv.Instance.GetUIModule<UIPnlSkillPowerUp>().ShowRresultView(orgAttrib, skill);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_SKILL_LEVEL_UP_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}
#endregion

#region bag

class ConsumeItemRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public ConsumeItemRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public ConsumeItemRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		ConsumeItemReq consumReq = request as ConsumeItemReq;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "ConsumeItem");

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgPhoneNumberVerify)))
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgPhoneNumberVerify));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgOpenPackage)))
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgOpenPackage));

		if (consumReq.IsQuickUse)//如果是通过快捷使用发送的请求
		{
			if (consumReq.ItemId == ConfigDatabase.DefaultCfg.ItemConfig.GetItemByType(ItemConfig._Type.AddStamina).id)
			{
				//使用烤山鸡购买
				var affordUI = SysUIEnv.Instance.GetUIModule<UIDlgAffordCost>();
				if (affordUI != null)
					affordUI.OnUseItemRecoverStaminaSuccess();
			}
			else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIDlgAffordCost))
			{
				foreach (GameConfig.RecoveryItem recoveryItem in ConfigDatabase.DefaultCfg.GameConfig.recoveryConfig.recoveryItems)
				{
					GoodConfig.Good goods = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(recoveryItem.goodsId);

					foreach (KodGames.ClientClass.Cost cost in costAndRewardAndSync.Costs)
					{
						if (goods.rewards[0].id == cost.Id)
						{
							SysUIEnv.Instance.GetUIModule<UIDlgAffordCost>().SetUICtrls(recoveryItem.assetId);
							return true;
						}
					}
				}
			}
		}
		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageItemTab))
			SysUIEnv.Instance.GetUIModule<UIPnlPackageItemTab>().OnConsumeItemSuccess(consumReq.ItemId, consumReq.Amount, costAndRewardAndSync);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_CONSUME_ITEM_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class SellItemRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public SellItemRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public SellItemRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "SellItem");
		SysUIEnv.Instance.GetUIModule<UIPnlPackageSell>().OnSellSuccess(costAndRewardAndSync.Reward);
		return true;
	}
}

#endregion

#region Chat
class ChatRes : Response
{
	private com.kodgames.corgi.protocol.ChatMessage chatMessage;
	private int result;
	private CostAndRewardAndSync costAndRewardAndSync;

	public ChatRes(int pRqstID, int result, com.kodgames.corgi.protocol.ChatMessage chatMessage, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.chatMessage = chatMessage;
		this.result = result;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public ChatRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_WORLD_SUCCESS)
		{
			SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "Chat");

			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlChatTab))
				SysUIEnv.Instance.GetUIModule<UIPnlChatTab>().OnWorldMsgSent();
		}

		if ((result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_PRIVATE_SUCCESS_RECEIVER_NOT_ONLINE)
			|| (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_PRIVATE_SUCCESS))
		{
			if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_PRIVATE_SUCCESS_RECEIVER_NOT_ONLINE)
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgLeaveMsg_Label_LeaveMessage"));

			SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadPrivateChatMsgs = true;
			SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadPrivateChatMsgCount++;

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlChatTab)))
			{
				SysLocalDataBase.Inst.LocalPlayer.ChatData.WorldChatMsgs.Add(chatMessage);
				SysLocalDataBase.Inst.LocalPlayer.ChatData.WorldChatMsgs.Sort(Compare);

				SysLocalDataBase.Inst.LocalPlayer.ChatData.PrivateChatMsgs.Add(chatMessage);
				SysLocalDataBase.Inst.LocalPlayer.ChatData.PrivateChatMsgs.Sort(Compare);
			}

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlChatTab)))
				SysUIEnv.Instance.GetUIModule<UIPnlChatTab>().OnPrivateMsgReceived(chatMessage);
		}

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_GUILD_SUCCESS)
		{
			SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadGuildChatMsgs = true;
			SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadGuildChatCount++;

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlChatTab)))
			{
				SysLocalDataBase.Inst.LocalPlayer.ChatData.GuildChatMsgs.Add(chatMessage);
				SysLocalDataBase.Inst.LocalPlayer.ChatData.GuildChatMsgs.Sort(Compare);
			}

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlChatTab)))
				SysUIEnv.Instance.GetUIModule<UIPnlChatTab>().OnGuildMsgReceived(chatMessage);
		}

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_PRIVATE_SUCCESS && chatMessage.senderId != SysLocalDataBase.Inst.LocalPlayer.PlayerId)
		{
			List<com.kodgames.corgi.protocol.ChatMessage> msgs = new List<com.kodgames.corgi.protocol.ChatMessage>();

			msgs.Add(chatMessage);
			SysLocalDataBase.Inst.UpdateMassages(msgs);
		}

		return true;
	}

	private int Compare(com.kodgames.corgi.protocol.ChatMessage msg1, com.kodgames.corgi.protocol.ChatMessage msg2)
	{
		return msg1.time.CompareTo(msg2.time);
	}
}

class SyncWorldChatAndFlowMessagesRes : Response
{
	private List<com.kodgames.corgi.protocol.ChatMessage> messages;

	public SyncWorldChatAndFlowMessagesRes(List<com.kodgames.corgi.protocol.ChatMessage> messages)
		: base(Request.InvalidID)
	{
		this.messages = messages;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysLocalDataBase.Inst.LocalPlayer == null)
			return true;

		if (messages.Count <= 0)
			return true;

		List<com.kodgames.corgi.protocol.ChatMessage> flowMsgs = new List<com.kodgames.corgi.protocol.ChatMessage>();
		foreach (com.kodgames.corgi.protocol.ChatMessage msg in messages)
			flowMsgs.Add(msg);

		SysLocalDataBase.Inst.UpdateMassages(flowMsgs);

		// Add to chat tab
		List<com.kodgames.corgi.protocol.ChatMessage> msgs = new List<com.kodgames.corgi.protocol.ChatMessage>();
		foreach (var msg in messages)
		{
			//if (msg.messageType != _ChatType.FlowMessage)
			msgs.Add(msg);
		}

		if (msgs.Count > 0)
		{
			foreach (var msg in msgs)
			{
				SysLocalDataBase.Inst.LocalPlayer.ChatData.WorldChatMsgs.Add(msg);

				if (!SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlChatTab) && msg.messageType != _ChatType.FlowMessage)
					SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadWorldChatMsgCount++;
			}

			SysLocalDataBase.Inst.LocalPlayer.ChatData.WorldChatMsgs.Sort(Compare);

			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlChatTab))
				SysUIEnv.Instance.GetUIModule<UIPnlChatTab>().OnWorldMsgReceived(msgs);
		}

		return true;
	}

	private int Compare(com.kodgames.corgi.protocol.ChatMessage msg1, com.kodgames.corgi.protocol.ChatMessage msg2)
	{
		return msg1.time.CompareTo(msg2.time);
	}
}

class QueryPlayerInfoRes : Response
{
	private KodGames.ClientClass.Player player;

	public QueryPlayerInfoRes(int pRqstID, KodGames.ClientClass.Player player)
		: base(pRqstID)
	{
		this.player = player;
	}

	public QueryPlayerInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlTowerThisWeekRank)))
			SysUIEnv.Instance.GetUIModule<UIPnlTowerThisWeekRank>().ShowViewLineUp(player);
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlTowerLastWeekRank)))
			SysUIEnv.Instance.GetUIModule<UIPnlTowerLastWeekRank>().ShowViewLineUp(player);
		return true;
	}
}

class CloseChatMessageDialogRes : Response
{
	public CloseChatMessageDialogRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public CloseChatMessageDialogRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		return true;
	}
}

class OnQueryChatMessageList : Response
{
	private List<com.kodgames.corgi.protocol.ChatMessage> chatMessages;
	private int privateMessageCount;
	private int worldMsgCount;
	private int guildMsgCount;

	public OnQueryChatMessageList(int pRqstID, List<com.kodgames.corgi.protocol.ChatMessage> chatMessages, int privateMessageCount, int worldMsgCount, int guildMsgCount)
		: base(pRqstID)
	{
		this.chatMessages = chatMessages;
		this.privateMessageCount = privateMessageCount;
		this.worldMsgCount = worldMsgCount;
		this.guildMsgCount = guildMsgCount;
	}

	public OnQueryChatMessageList(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadWorldChatMsgCount = worldMsgCount;

		AddLeavePrivateMessageToList();

		SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadPrivateChatMsgs = (privateMessageCount > 0 ? true : false);
		SysLocalDataBase.Inst.LocalPlayer.ChatData.HasUnreadGuildChatMsgs = (guildMsgCount > 0 ? true : false);

		if (SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadPrivateChatMsgCount == 0)
			SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadPrivateChatMsgCount = privateMessageCount;
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlChatTab))
			SysUIEnv.Instance.GetUIModule<UIPnlChatTab>().OnRequestMsgListSuccess();

		return true;
	}

	private void AddLeavePrivateMessageToList()
	{
		//SysLocalDataBase.Inst.LocalPlayer.ChatData.PrivateChatMsgs.Clear();
		//SysLocalDataBase.Inst.LocalPlayer.ChatData.WorldChatMsgs.Clear();
		var chatData = SysLocalDataBase.Inst.LocalPlayer.ChatData;

		foreach (var msg in chatMessages)
		{
			if (msg.messageType != _ChatType.Guild)
				chatData.WorldChatMsgs.Add(msg);

			if (msg.messageType == _ChatType.Private)
				chatData.PrivateChatMsgs.Add(msg);

			if (msg.messageType == _ChatType.Guild)
				chatData.GuildChatMsgs.Add(msg);
		}

		chatData.WorldChatMsgs.Sort(Compare);
		chatData.PrivateChatMsgs.Sort(Compare);
		chatData.GuildChatMsgs.Sort(Compare);

		//不在聊天界面时不请求聊天
		chatData.OfflineMessageQueried = true;

		if (!SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlChatTab))
			SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadWorldChatMsgCount++;
	}

	private int Compare(com.kodgames.corgi.protocol.ChatMessage msg1, com.kodgames.corgi.protocol.ChatMessage msg2)
	{
		return msg1.time.CompareTo(msg2.time);
	}
}
#endregion

#region Dungeon
class QueryDungeonListRes : Response
{
	private List<KodGames.ClientClass.Zone> zones;
	private List<KodGames.ClientClass.Zone> secretZones;
	private int lastDungeonId;
	private int lastZoneID;
	private int lastSecretDungeonId;
	private int lastSecretZoneId;
	private int positionId;
	private long lastResetDungeonTime;

	public QueryDungeonListRes(int pRqstID, List<Zone> zones, List<Zone> secretZones, int lastDungeonId, int lastZoneId, int lastSecretDungeonId, int lastSecretZoneId, int positionId, long lastResetDungeonTime)
		: base(pRqstID)
	{
		this.zones = zones;
		this.secretZones = secretZones;
		this.lastDungeonId = lastDungeonId;
		this.lastZoneID = lastZoneId;
		this.lastSecretDungeonId = lastSecretDungeonId;
		this.lastSecretZoneId = lastSecretZoneId;
		this.positionId = positionId;
		this.lastResetDungeonTime = lastResetDungeonTime;
	}

	public QueryDungeonListRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.CampaignData.SetZones(zones);
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.SetZones(secretZones);

		for (int index = 0; index < zones.Count; index++)
		{
			for (int i = 0; i < zones[index].DungeonDifficulties.Count; i++)
			{
				for (int j = 0; j < zones[index].DungeonDifficulties[i].TravelDatas.Count; j++)
				{
					if (zones[index].DungeonDifficulties[i].TravelDatas[j] != null)
						SysLocalDataBase.Inst.LocalPlayer.CampaignData.SetDungeonTravelData(zones[index].DungeonDifficulties[i].TravelDatas[j].DungeonId, zones[index].DungeonDifficulties[i].TravelDatas[j]);
				}
			}
		}

		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastBattleZoneID = lastZoneID;
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastBattleDungeonId = lastDungeonId;

		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleZoneId = lastZoneID;
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleDungeonId = lastDungeonId;

		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastSecretBattleZoneId = lastSecretZoneId;
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastSecretBattleDungeonId = lastSecretDungeonId;

		SysLocalDataBase.Inst.LocalPlayer.CampaignData.PositionId = positionId;
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastResetDungeonTime = lastResetDungeonTime;

		var queryReq = request as QueryDungeonListReq;
		if (queryReq.SuccessDel != null)
			queryReq.SuccessDel();

		return true;
	}
}

class QueryRecruiteNpcRes : Response
{
	private List<RecruiteNpc> recruiteNpcs;

	public QueryRecruiteNpcRes(int pRqstID, List<RecruiteNpc> recruiteNpcs)
		: base(pRqstID)
	{
		this.recruiteNpcs = recruiteNpcs;
	}

	public QueryRecruiteNpcRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryReq = request as QueryRecruiteNpcReq;

		// Set Data.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.SetDungeonRecruiteNpcs(queryReq.DungeonId, recruiteNpcs);

		// Notify UI.
		SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.ShowBefroeCombatUI(queryReq.DungeonId);

		return true;
	}
}

//战斗触发
class OnCombatRes : Response
{
	private int zoneStatus;
	private KodGames.ClientClass.Dungeon newDungeonRecord;
	private KodGames.ClientClass.CombatResultAndReward combatResultAndReward;
	private CostAndRewardAndSync costAndRewardAndSync;
	private TravelData travelData;

	public OnCombatRes(int pRqstID, CombatResultAndReward combatResultAndReward, CostAndRewardAndSync costAndRewardAndSync, int zoneStatus, Dungeon dungeon, TravelData travelData)
		: base(pRqstID)
	{
		this.combatResultAndReward = combatResultAndReward;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.zoneStatus = zoneStatus;
		this.newDungeonRecord = dungeon;
		this.travelData = travelData;
	}

	public OnCombatRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		OnCombatReq combatReq = request as OnCombatReq;
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(combatReq.DungeonId);

		// Set positionId.
		if (combatReq.Position != null)
			SysLocalDataBase.Inst.LocalPlayer.CampaignData.PositionId = combatReq.Position.PositionId;

		// Set TravelData.
		if (travelData != null)
		{
			SysLocalDataBase.Inst.LocalPlayer.CampaignData.SetDungeonTravelData(combatReq.DungeonId, travelData);
			//if (travelData.DungeonId != IDSeg.InvalidId)
			//    SysLocalDataBase.Inst.LocalPlayer.CampaignData.UpdateTravelData(travelData);
		}

		// Record last battle id.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastBattleZoneID = dungeonCfg.ZoneId;
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastBattleDungeonId = dungeonCfg.dungeonId;

		// Record last NormalCombat.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleZoneId = dungeonCfg.ZoneId;
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleDungeonId = dungeonCfg.dungeonId;

		int preRecord = 0;

		KodGames.ClientClass.Zone zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(dungeonCfg.ZoneId);
		KodGames.ClientClass.Dungeon dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfg.ZoneId, dungeonCfg.dungeonId);

		if (zoneRecord == null)
		{
			zoneRecord = new KodGames.ClientClass.Zone();
			zoneRecord.ZoneId = dungeonCfg.ZoneId;
			zoneRecord.DungeonDifficulties = new List<KodGames.ClientClass.DungeonDifficulty>();
			DungeonDifficulty dungeonDiffcult = new DungeonDifficulty();
			dungeonDiffcult.DifficultyType = dungeonCfg.DungeonDifficulty;
			dungeonDiffcult.Dungeons = new List<Dungeon>();
			zoneRecord.DungeonDifficulties.Add(dungeonDiffcult);

			SysLocalDataBase.Inst.LocalPlayer.CampaignData.Zones.Add(dungeonCfg.ZoneId, zoneRecord);
		}
		zoneRecord.Status = zoneStatus;

		if (dungeonRecord == null)
		{
			dungeonRecord = new KodGames.ClientClass.Dungeon();

			DungeonDifficulty dungeonDiffcult = zoneRecord.GetDungeonDiffcultyByDiffcultyType(dungeonCfg.DungeonDifficulty);

			if (dungeonDiffcult == null)
			{
				dungeonDiffcult = new DungeonDifficulty();
				dungeonDiffcult.DifficultyType = dungeonCfg.DungeonDifficulty;
				dungeonDiffcult.Dungeons = new List<Dungeon>();
				zoneRecord.DungeonDifficulties.Add(dungeonDiffcult);
			}

			dungeonDiffcult.Dungeons.Add(dungeonRecord);
		}
		else
		{
			preRecord = dungeonRecord.BestRecord;
		}

		dungeonRecord.CopyDungeon(newDungeonRecord);

		UIPnlCampaignBattleResult.CampaignBattleResultData battleResultData = new UIPnlCampaignBattleResult.CampaignBattleResultData(combatResultAndReward, dungeonCfg.ZoneId, dungeonCfg.dungeonId, 0, preRecord, combatReq.UiDungeonMapPosition);

		// Go to combat state
		List<object> paramsters = new List<object>();
		paramsters.Add(combatResultAndReward);
		paramsters.Add(battleResultData);

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnCombat");

		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Battle>(paramsters);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		OnCombatReq combatReq = request as OnCombatReq;
		int maxEnterTimes = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(combatReq.DungeonId).resetCount;

		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_COMBAT_FAILED_DUNGEON_EXCEED_MAX_ENTER_TIMES && SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlCampaignSceneMid)) && maxEnterTimes > 0)
			SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.OnResponseCombatErrOfEnterTimes(combatReq.DungeonId);
		else if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_COMBAT_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class SetZoneStatusRes : Response
{
	public SetZoneStatusRes(int pRqstID) : base(pRqstID) { }

	public SetZoneStatusRes(int pRqstID, int errCode, string errMsg) : base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SetZoneStatusReq setStatusReq = request as SetZoneStatusReq;
		int preZoneStatus = -1;

		KodGames.ClientClass.Zone zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(setStatusReq.ZoneId);
		if (zoneRecord != null)
		{
			preZoneStatus = zoneRecord.Status;
			zoneRecord.Status = setStatusReq.Status;
		}

		if ((request as SetZoneStatusReq).Del == null)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlCampaignScene)))
				SysUIEnv.Instance.GetUIModule<UIPnlCampaignScene>().OnResponseSetZoneStatus(setStatusReq.ZoneId, preZoneStatus);
		}
		else
			(request as SetZoneStatusReq).Del();

		return true;
	}
}

class SetDungeonStatusRes : Response
{
	public SetDungeonStatusRes(int pRqstID) : base(pRqstID) { }
	public SetDungeonStatusRes(int pRqstID, int errCode, string errMsg) : base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var setDungeonReq = request as SetDungeonStatusReq;

		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(setDungeonReq.DungeonId);

		if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(dungeonCfg.ZoneId))
		{
			SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleZoneId = dungeonCfg.ZoneId;
			SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleDungeonId = dungeonCfg.dungeonId;
		}

		var dungeon = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfg.ZoneId, dungeonCfg.dungeonId);

		if (dungeon == null)
		{
			dungeon = new KodGames.ClientClass.Dungeon();
			dungeon.BestRecord = 0;
			dungeon.DungeonId = dungeonCfg.dungeonId;
			dungeon.TodayCompleteTimes = 0;

			KodGames.ClientClass.DungeonDifficulty diffcult = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(dungeonCfg.ZoneId).GetDungeonDiffcultyByDiffcultyType(dungeonCfg.DungeonDifficulty);

			if (diffcult == null)
			{
				diffcult = new KodGames.ClientClass.DungeonDifficulty();
				diffcult.BoxPickedIndexs = new List<int>();
				diffcult.DifficultyType = dungeonCfg.DungeonDifficulty;
				diffcult.Dungeons = new List<Dungeon>();

				SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(dungeonCfg.ZoneId).DungeonDifficulties.Add(diffcult);
			}

			diffcult.Dungeons.Add(dungeon);
		}

		dungeon.DungeonStatus = setDungeonReq.Status;

		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastBattleZoneID = dungeonCfg.ZoneId;
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastBattleDungeonId = dungeonCfg.dungeonId;

		return true;
	}
}

class ResetDungeonCompleteTimesRes : Response
{

	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private int todayCompleteTimes;
	private int alreadyResetTimes;

	public ResetDungeonCompleteTimesRes(int pRqstID, int todayCompleteTimes, int alreadyResetTimes, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.todayCompleteTimes = todayCompleteTimes;
		this.alreadyResetTimes = alreadyResetTimes;
	}

	public ResetDungeonCompleteTimesRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Dungeon Config.
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById((request as ResetDungeonCompleteTimesReq).DungeonID);

		// Use costs.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "ResetDungeonCompleteTimes");

		// Reset enterTimes.
		KodGames.ClientClass.Dungeon dungeon = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfg.ZoneId, dungeonCfg.dungeonId);

		if (dungeon != null)
		{
			dungeon.TodayCompleteTimes = todayCompleteTimes;
			dungeon.TodayAlreadyResetTimes = alreadyResetTimes;
		}

		// Refresh View.
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignSceneMid))
			SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.OnResponseRefreshDungeonInfo(dungeonCfg.dungeonId);

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlCampaign_DungeonCount_ResetSuccess"));

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_RESET_DUNGEON_COMPLETE_TIMES_CONSUME_NOT_ENOUGH_FAILD && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class GetDungeonRewardRes : Response
{
	private List<int> boxPickedIndexs;
	KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public GetDungeonRewardRes(int pRqstID, List<int> boxPickedIndexs, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.boxPickedIndexs = boxPickedIndexs;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public GetDungeonRewardRes(int pRqstID, int errCode, string errMsg) : base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Use costs.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "GetDungeonReward");

		// Set the DungeonDiffcult rewardState.
		var getDungeonReq = request as GetDungeonRewardReq;
		var zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(getDungeonReq.ZoneId);
		var dungeonDiffcult = zoneRecord.GetDungeonDiffcultyByDiffcultyType(getDungeonReq.DungeonDifficulty);

		if (dungeonDiffcult == null)
		{
			dungeonDiffcult = new KodGames.ClientClass.DungeonDifficulty();
			dungeonDiffcult.DifficultyType = getDungeonReq.DungeonDifficulty;
			dungeonDiffcult.BoxPickedIndexs = new List<int>();
			dungeonDiffcult.Dungeons = new List<Dungeon>();
			zoneRecord.DungeonDifficulties.Add(dungeonDiffcult);
		}

		dungeonDiffcult.BoxPickedIndexs.Clear();
		for (int index = 0; index < boxPickedIndexs.Count; index++)
			dungeonDiffcult.BoxPickedIndexs.Add(boxPickedIndexs[index]);

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgDungeonStarReward)))
			SysUIEnv.Instance.GetUIModule<UIDlgDungeonStarReward>().OnResponseFixRewardSuccess();

		return true;
	}
}

//副本扫荡
class ContinueCombatRes : Response
{
	private KodGames.ClientClass.Dungeon dungeon;
	private List<KodGames.ClientClass.CombatResultAndReward> rewards;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public ContinueCombatRes(int pRqstID, KodGames.ClientClass.Dungeon dungeon, List<CombatResultAndReward> rewards, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.dungeon = dungeon;
		this.rewards = rewards;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public ContinueCombatRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		ContinueCombatReq continueCombatReq = request as ContinueCombatReq;

		// Use costs.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "ContinueCombat");

		// Reset Dungeon Data.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(continueCombatReq.ZoneId, continueCombatReq.DungeonId).CopyDungeon(dungeon);
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleZoneId = continueCombatReq.ZoneId;
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleDungeonId = continueCombatReq.DungeonId;

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgCampaignContinue)))
			SysUIEnv.Instance.GetUIModule<UIDlgCampaignContinue>().OnContinueSuccess(continueCombatReq.DungeonId, rewards);

		// Talking Game Battle.
		GameAnalyticsUtility.OnEventBattleCampaign(continueCombatReq.DungeonId, rewards, true);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgCampaignContinue)))
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgCampaignContinue));

		ContinueCombatReq combatReq = request as ContinueCombatReq;

		int maxEnterTimes = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(combatReq.DungeonId).resetCount;

		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_CONTINUE_COMBAT_FAILED_DUNGEON_EXCEED_MAX_ENTER_TIMES && SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlCampaignSceneMid)) && maxEnterTimes > 0)
			SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.OnResponseCombatErrOfEnterTimes(combatReq.DungeonId);
		else if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_CONTINUE_COMBAT_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class QueryDungeonGuideRes : Response
{
	private List<KodGames.ClientClass.DungeonGuideNpc> dungeonGuideNpc;

	public QueryDungeonGuideRes(int pRqstID, List<KodGames.ClientClass.DungeonGuideNpc> dungeonGuideNpc)
		: base(pRqstID)
	{
		this.dungeonGuideNpc = dungeonGuideNpc;
	}

	public QueryDungeonGuideRes(int pRqstID, int errCode, string errMsg) : base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryRequest = request as QueryDungeonGuideReq;

		// Set DungeonGuidNpc Data.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.SetDungeonGuidNpcs(queryRequest.DungeonId, dungeonGuideNpc);

		// Show Guid UI.
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAvatarLineUpGuide), queryRequest.DungeonId);

		return true;
	}
}

//云游商人查询
class QueryTravelRes : Response
{
	private TravelData travelData;

	public QueryTravelRes(int pRqstID, TravelData travelData)
		: base(pRqstID)
	{
		this.travelData = travelData;
	}

	public QueryTravelRes(int pRqstID, int errCode, string errMsg) : base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryRequest = request as QueryTravelReq;

		// Set Travel Data.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.SetDungeonTravelData(queryRequest.DungeonId, travelData);
		//SysLocalDataBase.Inst.LocalPlayer.CampaignData.UpdateTravelData(travelData);

		// Show UI.
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDungeonTravelShop), queryRequest.DungeonId);

		return true;
	}
}

//云游商人商品购买
class BuyTravelRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private TravelData travelData;

	public BuyTravelRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync, TravelData travelData)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.travelData = travelData;
	}

	public BuyTravelRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process CostAndReward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "BuyTravel");

		// Reset Data.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.SetDungeonTravelData((request as BuyTravelReq).DungeonId, travelData);
		//if (travelData.DungeonId != IDSeg.InvalidId)
		//    SysLocalDataBase.Inst.LocalPlayer.CampaignData.UpdateTravelData(travelData);

		// Refresh View.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgDungeonTravelShop)))
			SysUIEnv.Instance.GetUIModule<UIDlgDungeonTravelShop>().OnResponseBuyTravel((request as BuyTravelReq).GoodId);

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlCampaignSceneMid)))
			SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.ChangeDungeonTab();//ChangeDungeonTab(travelData.DungeonId);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_TRAVEL_CONSUME_NOT_ENOUGH_FAILED && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
		{
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
			//if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgDungeonTravelShop)))
			//    SysUIEnv.Instance.GetUIModule(typeof(UIDlgDungeonTravelShop)).OnHide();
		}
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

#endregion

#region EmailRes
class QueryEmailListsRes : Response
{
	private long lastQueryTime;
	private List<KodGames.ClientClass.EmailPlayer> emailPlayers;

	public QueryEmailListsRes(int pRqstID, long lastQueryTime, List<KodGames.ClientClass.EmailPlayer> emailPlayers)
		: base(pRqstID)
	{
		this.lastQueryTime = lastQueryTime;
		this.emailPlayers = emailPlayers;
	}

	public QueryEmailListsRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryReq = request as QueryEmailListsReq;

		// Set local data
		//设置当前标签下的邮件数量为“0”【手动申请，重置按照申请的时候保留的标签】
		SysLocalDataBase.Inst.LocalPlayer.EmailData.SetNewEmailCount(queryReq.EmailType, 0);
		SysLocalDataBase.Inst.LocalPlayer.EmailData.SetEmails(queryReq.EmailType, emailPlayers);


		// Notice UI
		//将上次查询时间当成参数传入界面当中
		SysUIEnv.Instance.GetUIModule<UIPnlEmail>().QueryEmailListSuccess(queryReq.EmailType, lastQueryTime);

		return true;
	}
}

class GetAttachmetnsRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public GetAttachmetnsRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public GetAttachmetnsRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryReq = request as GetAttachmentsReq;

		KodGames.ClientClass.EmailPlayer email = null;
		List<KodGames.ClientClass.EmailPlayer> emailPlayers = SysLocalDataBase.Inst.LocalPlayer.EmailData.GetEmails(_EmailDisplayType.Guild);
		if (emailPlayers != null && emailPlayers.Count > 0)
			foreach (var ep in emailPlayers)
				if (ep.EmailId == queryReq.EmailID)
					email = ep;

		if (email == null)
			email = SysLocalDataBase.Inst.LocalPlayer.EmailData.GetEmailById(queryReq.EmailID);

		if (email != null)
			email.StatusDidPick = 1;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "GetAttachmetns");
		SysUIEnv.Instance.GetUIModule<UIPnlEmail>().OnGetAttachmentSuccess(costAndRewardAndSync.Reward);

		return true;
	}
}

class SyncNewEmailCountRes : Response
{
	private int count;
	private int emailType;

	public SyncNewEmailCountRes(int pRqstID, int count, int emailType)
		: base(pRqstID)
	{
		this.count = count;
		this.emailType = emailType;
	}

	public SyncNewEmailCountRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
		Debug.LogError(1);
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysLocalDataBase.Inst.LocalPlayer == null)
			return true;

		// Update local data
		//更新返回邮件标签数量【系统自动发送，不需要申请】
		SysLocalDataBase.Inst.LocalPlayer.EmailData.SetNewEmailCount(emailType, count);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEmail))
			SysUIEnv.Instance.GetUIModule<UIPnlEmail>().ChangeNewMailButtonState();

		return true;
	}
}
#endregion

#region Daily SignIn

class SignInRes : Response
{
	//private int signType;
	KodGames.ClientClass.SignData signData;
	private CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.Reward reward;
	private KodGames.ClientClass.Reward specialReward;

	public SignInRes(int pRqstID, int signType, KodGames.ClientClass.Reward reward, KodGames.ClientClass.Reward specialReward, CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.SignData signData)
		: base(pRqstID)
	{
		//this.signType = signType;
		this.signData = signData;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.reward = reward;
		this.specialReward = specialReward;
	}

	public SignInRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryReq = request as SignInReq;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "SignIn");

		// Get the Sign/Resign DayNum.
		int signDayData = SysLocalDataBase.Inst.LocalPlayer.SignData.SignDetails ^ signData.SignDetails;
		int signDay = 1;
		for (; signDay <= 31; signDay++)
		{
			if (signDayData >> (signDay - 1) == 1)
				break;
		}

		SysLocalDataBase.Inst.LocalPlayer.SignData = signData;

		SysUIEnv.Instance.GetUIModule<UIDlgDailyReward>().OnResponeSignInSuccess(reward, specialReward, signDay, queryReq.signType);
		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_SIGN_IN_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}
#endregion

#region Activity
// FixedTime
//活动客栈休息

class QueryFixedTimeActivityRewardRes : Response
{
	private List<KodGames.ClientClass.Reward> reward;
	private long lastGetTime;
	private int resetType;

	public QueryFixedTimeActivityRewardRes(int pRqstID, List<KodGames.ClientClass.Reward> reward, long lastGetTime, int resetType)
		: base(pRqstID)
	{
		this.reward = reward;
		this.lastGetTime = lastGetTime;
		this.resetType = resetType;
	}

	public QueryFixedTimeActivityRewardRes(int pRqstID, int result, string ErrMsg)
		: base(pRqstID, result, ErrMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		ActivityManager.Instance.GetActivity<ActivityFixedTime>().LastGetTime = this.lastGetTime;
		ActivityManager.Instance.GetActivity<ActivityFixedTime>().RestTimeType = this.resetType;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlActivityInnTab)))
			SysUIEnv.Instance.GetUIModule<UIPnlActivityInnTab>().OnResponseQueryFixedActivitySuccess(reward);

		return true;
	}
}

class GetFixedTimeActivityRewardRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private long lastGetTime;

	public GetFixedTimeActivityRewardRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync, long lastGetTime)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.lastGetTime = lastGetTime;
	}

	public GetFixedTimeActivityRewardRes(int pRqstID, int result, string ErrMsg)
		: base(pRqstID, result, ErrMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "GetFixedTimeActivityReward");

		ActivityManager.Instance.GetActivity<ActivityFixedTime>().LastGetTime = lastGetTime;

		SysUIEnv.Instance.GetUIModule<UIPnlActivityInnTab>().OnResponseFixedActivitySuccess(costAndRewardAndSync.Reward);

		return true;
	}
}
#endregion

#region Recharge
class OnApplePurchaseRes : Response
{
	private int commodityID;
	private int commodityCount;
	private string transactionIdentifier;
	private int realMoneyDelta;
	private int totalConsumedRMB;
	private int remainRMB;
	private int vipLevel;
	private bool isUpLevel;
	private string errMsg;
	private List<int> payStatus;
	public OnApplePurchaseRes(int pRqstID, int commodityID, int commodityCount, string transactionIdentifier, int realMoneyDelta, int totalConsumedRMB, int vipLevel, int remainRMB, string errMsg, List<int> payStatus)
		: base(pRqstID)
	{
		this.commodityID = commodityID;
		this.commodityCount = commodityCount;
		this.transactionIdentifier = transactionIdentifier;
		this.realMoneyDelta = realMoneyDelta;
		this.totalConsumedRMB = totalConsumedRMB;
		this.remainRMB = remainRMB;
		this.vipLevel = vipLevel;
		this.errMsg = errMsg;
		this.payStatus = payStatus;
	}

	public OnApplePurchaseRes(int pRqstID, int result, string errMsg)
		: base(pRqstID, result, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var sysLocalDataBase = SysLocalDataBase.Inst;

		sysLocalDataBase.LocalPlayer.RealMoney += realMoneyDelta;
		sysLocalDataBase.LocalPlayer.TotalCostRMB = totalConsumedRMB;
		sysLocalDataBase.LocalPlayer.VipLevel = vipLevel;
		sysLocalDataBase.LocalPlayer.RemainingCostRMB = remainRMB;

		// Update vip controlled max point
		sysLocalDataBase.UpdateIncreaseDataMaxValue();

		GameMain.Inst.GetIAPListener().OnVerifyPaymentTransaction(IAPListener.VerifyPaymentResult.Successed, transactionIdentifier, commodityID, commodityCount, errMsg, payStatus);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		// 特殊处理配置文件更新逻辑
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_COMMON_CONFIG_IS_NOT_LATEST)
			base.PrcErr(request, errCode, errMsg);
		else
		{
			GameMain.Inst.GetIAPListener().OnVerifyPaymentTransaction(IAPListener.VerifyPaymentResult.Payment_Failed, "", 0, 0, errMsg, payStatus);
			base.PrcErr(request, errCode, errMsg);
		}
	}
}
#endregion

#region Arena
class QueryArenaRankRes : Response
{
	private int challengeTimes;
	private int arenaGradeId;
	private long gradePoint;
	private int selfRank;
	private int speed;
	private long lastResetTime;
	private List<com.kodgames.corgi.protocol.PlayerRecord> challengeRecords;

	public QueryArenaRankRes(int pRqstID, int challengeTimes, int arenaGradeId, long gradePoint, long lastResetChallengeTime, int selfRank, int speed, List<com.kodgames.corgi.protocol.PlayerRecord> challengeRecords)
		: base(pRqstID)
	{
		this.challengeTimes = challengeTimes;
		this.arenaGradeId = arenaGradeId;
		this.gradePoint = gradePoint;
		this.lastResetTime = lastResetChallengeTime;
		this.selfRank = selfRank;
		this.speed = speed;
		this.challengeRecords = challengeRecords;
	}

	public QueryArenaRankRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Set local data
		KodGames.ClientClass.ArenaData arenaData = new ArenaData();
		arenaData.ChallengePoint = challengeTimes;
		arenaData.HonorPoint = gradePoint;
		arenaData.SelfRank = selfRank;
		SysLocalDataBase.Inst.LocalPlayer.ArenaData = arenaData;

		UIPnlArena.ArenaShowData arenaShowData = new UIPnlArena.ArenaShowData();
		arenaShowData.myGradeId = arenaGradeId;
		arenaShowData.mySpeed = speed;
		arenaShowData.challengeRecords = challengeRecords;

		// Notice UI
		SysUIEnv.Instance.GetUIModule<UIPnlArena>().OnQueryArenaRankSuccess(arenaShowData, lastResetTime);

		return true;
	}
}

class QueryHonorPointRes : Response
{
	private long gradePoint;

	public QueryHonorPointRes(int pRqstID, long gradePoint)
		: base(pRqstID)
	{
		this.gradePoint = gradePoint;
	}

	public QueryHonorPointRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysLocalDataBase.Inst.LocalPlayer.ArenaData != null)
			SysLocalDataBase.Inst.LocalPlayer.ArenaData.HonorPoint = gradePoint;

		SysUIEnv.Instance.GetUIModule<UIPnlArena>().OnUpdateMyGradePointSuccess();

		return true;
	}
}

class QueryArenaPlayerInfoRes : Response
{
	private KodGames.ClientClass.Player player;

	public QueryArenaPlayerInfoRes(int pRqstID, KodGames.ClientClass.Player player)
		: base(pRqstID)
	{
		this.player = player;
	}

	public QueryArenaPlayerInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		GameUtility.ShowViewAvatarUI(player, (request as QueryArenaPlayerInfoReq).Rank);

		return true;
	}
}


class ArenaCombatRes : Response
{
	private KodGames.ClientClass.CombatResultAndReward combatResultAndReward;
	private int selfRank;
	private CostAndRewardAndSync costAndRewardAndSync;

	public ArenaCombatRes(int pRqstID, KodGames.ClientClass.CombatResultAndReward combatResultAndReward, int selfRank, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.combatResultAndReward = combatResultAndReward;
		this.selfRank = selfRank;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public ArenaCombatRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Use cost.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "ArenaCombat");

		// Notice UI
		SysUIEnv.Instance.GetUIModule<UIPnlArena>().OnArenaCombatSuccess(combatResultAndReward, selfRank);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_ARENA_COMBAT_FAILED_NOT_POSITON_AVATAR)
			SysUIEnv.Instance.GetUIModule<UIPnlArena>().OnAreanCombatSuccessCaseNotAvatar(errMsg);
		else if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_ARENA_COMBAT_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class QueryRankToFewRes : Response
{
	private List<com.kodgames.corgi.protocol.PlayerRecord> topFew;

	public QueryRankToFewRes(int pRqstID, List<com.kodgames.corgi.protocol.PlayerRecord> topFew)
		: base(pRqstID)
	{
		this.topFew = topFew;
	}

	public QueryRankToFewRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Notice UI
		SysUIEnv.Instance.GetUIModule<UIPnlArena>().QueryRankToFewResSuccess(topFew);

		return true;
	}
}
#endregion

#region Tavern
class QueryTavernInfoRes : Response
{
	private List<com.kodgames.corgi.protocol.TavernInfo> tavernInfos;
	private List<int> mysteryerResourceIds;

	public QueryTavernInfoRes(int pRqstID, List<com.kodgames.corgi.protocol.TavernInfo> tavernInfos, List<int> mysteryerResourceIds)
		: base(pRqstID)
	{
		this.tavernInfos = tavernInfos;
		this.mysteryerResourceIds = mysteryerResourceIds;
	}

	public QueryTavernInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.ShopData.TavernInfos = tavernInfos;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlShopWine))
			SysUIEnv.Instance.GetUIModule<UIPnlShopWine>().OnQueryTavernInfoSuccess(this.mysteryerResourceIds);

		return true;
	}
}

//酒馆
class TavernBuyRes : Response
{
	private com.kodgames.corgi.protocol.TavernInfo tavernInfo;
	private CostAndRewardAndSync costAndRewardAndSync;

	public TavernBuyRes(int pRqstID, com.kodgames.corgi.protocol.TavernInfo tavernInfo, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.tavernInfo = tavernInfo;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public TavernBuyRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public TavernBuyRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Update local data
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "TavernBuy");

		var oldTavernInfo = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetTavernInfoById(tavernInfo.id);
		oldTavernInfo.leftFreeCount = tavernInfo.leftFreeCount;
		oldTavernInfo.nextFreeStartTime = tavernInfo.nextFreeStartTime;
		oldTavernInfo.isFirstMoneyBuy = tavernInfo.isFirstMoneyBuy;
		oldTavernInfo.alreadyTenTavern = tavernInfo.alreadyTenTavern;

		// Notice UI
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlShopWine))
			SysUIEnv.Instance.GetUIModule<UIPnlShopWine>().OnTavernBuySuccess(costAndRewardAndSync, tavernInfo);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_TAVERN_BUY_FAILED_CONSUMALBLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}
#endregion

#region Arena

#endregion

#region  Tutorial
class GetTutorialAvatarAndSetPlayerNameRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private PositionData positonData;

	public GetTutorialAvatarAndSetPlayerNameRes(int pRqstID, PositionData positonData, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.positonData = positonData;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public GetTutorialAvatarAndSetPlayerNameRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Set Position.
		SysLocalDataBase.Inst.LocalPlayer.PositionData = positonData;

		// Set the Player Name.
		GetTutorialAvatarAndSetPlayerNameReq tutorialReq = request as GetTutorialAvatarAndSetPlayerNameReq;
		SysLocalDataBase.Inst.LocalPlayer.Name = tutorialReq.PlayerName;

		// Get the Avatar.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "GetTutorialAvatarAndSetPlayerName");

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlSelectPlayerAvatar))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectPlayerAvatar>().OnResponseGetTutorialAvatarSuccess();

		// Platform Upload GameData.
		Platform.Instance.UploadGameData(true);

		return true;
	}
}

class CompleteTutorialRes : Response
{
	public CompleteTutorialRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public CompleteTutorialRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Talking Game Tutorial Finish.
		var req = request as CompleteTutorialReq;
		GameAnalyticsUtility.OnTutorialCompleted(req.TutorialId);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

class NoviceCombatRes : Response
{
	private KodGames.ClientClass.CombatResultAndReward combatResultAndReward;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public NoviceCombatRes(int pRqstID, KodGames.ClientClass.CombatResultAndReward combatResultAndReward, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.combatResultAndReward = combatResultAndReward;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public NoviceCombatRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		UIPnlTutorialBattleResult.TutorialBattleResultData battleResultData = new UIPnlTutorialBattleResult.TutorialBattleResultData();
		List<object> paramsters = new List<object>();
		paramsters.Add(combatResultAndReward);
		paramsters.Add(battleResultData);

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "NoviceCombat");
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Battle>(paramsters);

		return true;
	}
}

class FetchRandomPlayerNameRes : Response
{
	private List<string> playerNames;

	public FetchRandomPlayerNameRes(int pRqstID, List<string> playerNames)
		: base(pRqstID)
	{
		this.playerNames = playerNames;
	}

	public FetchRandomPlayerNameRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlSelectPlayerAvatar))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectPlayerAvatar>().OnResponseFetchRandomPlayerNameSuccess(playerNames);
		//if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectFirstAvatar)))
		//SysUIEnv.Instance.GetUIModule<UIPnlSelectFirstAvatar>().OnResponseFetchRandomPlayerNameSuccess(playerNames);

		return true;
	}
}

#endregion

#region  Quest : UIPnlAssistant

//申请任务
class QueryQuestInfoRes : Response
{
	private List<KodGames.ClientClass.Quest> quests;

	public QueryQuestInfoRes(int pRqstID, List<KodGames.ClientClass.Quest> quests, KodGames.ClientClass.QuestQuick questQuick)
		: base(pRqstID)
	{
		this.quests = quests;
	}

	public QueryQuestInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryQuestReq = request as QueryQuestInfoReq;

		// Update Quest Data.
		SysLocalDataBase.Inst.UpdateQuestData(quests, false);

		// QuerySuccess Del.
		if (queryQuestReq.Del != null)
			queryQuestReq.Del();

		return true;
	}
}

//领取任务奖励
class PickQuestRewardRes : Response
{
	//	private int questId;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private List<KodGames.ClientClass.Quest> changedQuests;
	private KodGames.ClientClass.QuestQuick questQuick;

	public PickQuestRewardRes(int pRqstID, int questId, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, List<KodGames.ClientClass.Quest> changedQuests, KodGames.ClientClass.QuestQuick questQuick)
		: base(pRqstID)
	{
		//		this.questId = questId;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.changedQuests = changedQuests;
		this.questQuick = questQuick;
	}

	public PickQuestRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Use Cost , Get Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "PickQuestReward");

		// Update Quest Data.
		SysLocalDataBase.Inst.UpdateQuestData(changedQuests, false);

		// Update QuestQuick.
		SysLocalDataBase.Inst.LocalPlayer.QuestData.QuestQuick = questQuick;

		PickQuestRewardReq rqst = request as PickQuestRewardReq;

		if (rqst.PickQuestRewardSuccessDel != null)
			rqst.PickQuestRewardSuccessDel(changedQuests, costAndRewardAndSync.Reward);

		return true;
	}
}
#endregion

#region  PlayerLevelUp
class GetLevelUpRewardRes : Response
{
	KodGames.ClientClass.CostAndRewardAndSync crs;

	public GetLevelUpRewardRes(int pReqId, KodGames.ClientClass.LevelAttrib levelAttri, KodGames.ClientClass.CostAndRewardAndSync crs)
		: base(pReqId)
	{
		this.crs = crs;
	}

	public GetLevelUpRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(crs, "GetLevelUpReward");
		SysLocalDataBase.Inst.LocalPlayer.PlayerLevelUpData = new PlayerLevelUpData(SysLocalDataBase.Inst.LocalPlayer.CurrentPickedLevel, crs);
		SysLocalDataBase.Inst.LocalPlayer.CurrentPickedLevel = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level;

		var srcReq = request as GetLevelUpRewardReq;
		if (srcReq.DelayShowEvent != null)
			srcReq.DelayShowEvent.Delay = false;

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		var srcReq = request as GetLevelUpRewardReq;
		if (srcReq.DelayShowEvent != null)
			srcReq.DelayShowEvent.Delay = false;

		base.PrcErr(request, errCode, errMsg);
	}
}
#endregion

class LevelRewardGetRewardRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public LevelRewardGetRewardRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public LevelRewardGetRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public LevelRewardGetRewardRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "LevelRewardGetReward");

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlLevelRewardTab))
			SysUIEnv.Instance.GetUIModule<UIPnlLevelRewardTab>().OnReqLevelRewardGetRewardSuccess(costAndRewardAndSync.Reward);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_LEVELREWARD_GET_REWARDS_FAILED_LEVELR_FAILED && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class QueryLevelRewardRes : Response
{
	private List<KodGames.ClientClass.LevelReward> levelRewards;

	public QueryLevelRewardRes(int pRqstID, List<KodGames.ClientClass.LevelReward> levelRewards)
		: base(pRqstID)
	{
		this.levelRewards = levelRewards;
	}

	public QueryLevelRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		ActivityManager.Instance.GetActivity<ActivityLevelReward>().LevelRewards = levelRewards;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlLevelRewardTab))
			SysUIEnv.Instance.GetUIModule<UIPnlLevelRewardTab>().OnReqRewardListSuccess();

		return true;
	}
}

//兑换码返回
class OnExchangeCodeRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private string strConver;
	private string strGetway;
	public OnExchangeCodeRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync, string strConver, string strGetway)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.strConver = strConver;
		this.strGetway = strGetway;
	}

	public OnExchangeCodeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public OnExchangeCodeRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync, string strConver, string strGetway)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.strConver = strConver;
		this.strGetway = strGetway;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnExchangeCode");
		SysUIEnv.Instance.GetUIModule<UIPnlSetting>().OnSettingExchangeRewardSuccess(costAndRewardAndSync, strConver, strGetway);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_EXCHANGECODE_FAILED_NO_EXCHANGE_CODE && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class SettingFeedbackRes : Response
{
	public SettingFeedbackRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public SettingFeedbackRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.GetUIModule<UIDlgFeedBack>().OnSettingFeedBackSuccess();

		return true;
	}
}

class ReturnExceptionCallbackRes : Response
{
	private int errCode;
	private string errMsg;

	public ReturnExceptionCallbackRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID)
	{
		this.errCode = errCode;
		this.errMsg = errMsg;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (errCode == com.kodgames.corgi.protocol.Protocols.E_COMMON_CONFIG_IS_NOT_LATEST)
			RequestMgr.Inst.ConfigOutOfSync(errMsg);
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errMsg);

		return true;
	}
}

#region ActivityExchange

class OnQueryExchangeListRes : Response
{
	private List<Exchange> exchanges;
	public OnQueryExchangeListRes(int pRqstID, List<Exchange> exchanges)
		: base(pRqstID)
	{
		this.exchanges = exchanges;
	}

	public OnQueryExchangeListRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		//ActivityManager.Instance.GetActivity<ActivityExchange>().ActivityInfo.Exchanges = exchanges;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityExchangeTab))
			SysUIEnv.Instance.GetUIModule<UIPnlActivityExchangeTab>().OnQueryExchangeListSuccess(exchanges);

		return true;
	}


}

class OnExchangeRes : Response
{
	private int exchangeId;
	private long nextOpenTime;
	private KodGames.ClientClass.CostAndRewardAndSync costAReward;

	public OnExchangeRes(int pRqstID, int exchangeId, long nextOpenTime, KodGames.ClientClass.CostAndRewardAndSync costAReward)
		: base(pRqstID)
	{
		this.exchangeId = exchangeId;
		this.nextOpenTime = nextOpenTime;
		this.costAReward = costAReward;
	}

	public OnExchangeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process reward and cost
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAReward, "OnExchange");

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityExchangeTab))
			SysUIEnv.Instance.GetUIModule<UIPnlActivityExchangeTab>().OnExchangeRes(exchangeId, nextOpenTime, costAReward.Reward);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_EXCHANGE_FAILED_EXCHANGE_OUT_OF_TIME)
			RequestMgr.Inst.Request(new QueryExchangeList());

		base.PrcErr(request, errCode, errMsg);
	}
}
#endregion

#region  StartServerReward
class PickStartServerRewardRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public PickStartServerRewardRes(int pRqstID, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public PickStartServerRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost and Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "PickStartServerReward");

		// Remove id.
		SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo.UnPickIds.Remove((request as PickStartServerRewardReq).PickID);

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlStartServerReward))
			SysUIEnv.Instance.GetUIModule<UIPnlStartServerReward>().OnPickRewardSuccess((request as PickStartServerRewardReq).PickID, costAndRewardAndSync.Reward);

		return true;
	}
}
#endregion

#region  HandBookReward
//图鉴合成
class MergeIllustrationRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public MergeIllustrationRes(int pRqstID, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public MergeIllustrationRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public MergeIllustrationRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var mergeReq = request as MergeIllustrationReq;

		// Save card data.
		if (!SysLocalDataBase.Inst.LocalPlayer.CardIds.Contains(mergeReq.IllustrationId))
			SysLocalDataBase.Inst.LocalPlayer.CardIds.Add(mergeReq.IllustrationId);

		// Process Cost and Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "MergeIllustration");

		// Notify Ui For Refresh.
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlHandBook))
			SysUIEnv.Instance.GetUIModule<UIPnlHandBook>().OnPickRewardSuccess(mergeReq.IllustrationId, mergeReq.Count);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_MERGE_ILLUSTRATION_FAILED_CONSUMALBLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}
#endregion

#region  QueryIllustrationRes

class QueryIllustrationRes : Response
{
	private List<int> cardIds;

	public QueryIllustrationRes(int pRqstID, List<int> cardIds)
		: base(pRqstID)
	{
		this.cardIds = cardIds;
	}

	public QueryIllustrationRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	//public QueryIllustrationRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	//    : base(pRqstID, errCode, errMsg)
	//{
	//    this.costAndRewardAndSync = costAndRewardAndSync;
	//}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.CardIds.Clear();
		SysLocalDataBase.Inst.LocalPlayer.CardIds.AddRange(cardIds);

		// Notify Ui For Refresh.
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlHandBook))
			SysUIEnv.Instance.GetUIModule<UIPnlHandBook>().Show();

		return true;
	}
}
#endregion

#region MysteryShop
class QueryMysteryShopRes : Response
{
	private MysteryShopInfo mysteryShopInfo;
	public QueryMysteryShopRes(int pRqstID, MysteryShopInfo mysteryShopInfo)
		: base(pRqstID)
	{
		this.mysteryShopInfo = mysteryShopInfo;
	}

	public QueryMysteryShopRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.AddMysteryShopInfo(mysteryShopInfo);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlTowerNormalShop))
			SysUIEnv.Instance.GetUIModule<UIPnlTowerNormalShop>().OnQuerynNormalShopInfoSuccess();

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlTowerNormalShop))
			SysUIEnv.Instance.GetUIModule<UIPnlTowerNormalShop>().OnQueryMysteryShopInfoFailed();
	}
}

class ChangeMysteryShopRes : Response
{
	private MysteryShopInfo mysteryShopInfo;
	private CostAndRewardAndSync costAndRewardAndSync;

	public ChangeMysteryShopRes(int pRqstID, MysteryShopInfo mysteryShopInfo, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.mysteryShopInfo = mysteryShopInfo;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public ChangeMysteryShopRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		//SysLocalDataBase.Inst.LocalPlayer.MysteryShopInfo = mysteryShopInfo;
		SysLocalDataBase.Inst.LocalPlayer.AddMysteryShopInfo(mysteryShopInfo);
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "ChangeMysteryShop");

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlTowerNormalShop)))
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlShopMystery_Tip_ChangeSuccess"));
			SysUIEnv.Instance.GetUIModule<UIPnlTowerNormalShop>().OnQuerynNormalShopInfoSuccess();
		}

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_MYSTERY_SHOP_REFRESH_FAILED_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

//神秘商店购买物品
class BuyMysteryGoodRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private MysteryGoodInfo goodsInfo;

	public BuyMysteryGoodRes(int pRqstID, MysteryGoodInfo goodsInfo, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.goodsInfo = goodsInfo;
	}

	public BuyMysteryGoodRes(int pRqstID, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var mysteryShopReq = request as BuyMysteryGoodReq;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, string.Format("BuyMysteryGood_{0}", mysteryShopReq.GoodsID.ToString("X")));

		//Set goodsInfo in LocalPlayer.MysterShopInfo
		if (SysGameStateMachine.Instance.CurrentState is GameState_Tower)
			for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).Goods.Count; i++)
			{
				if (SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).Goods[i].GoodId == goodsInfo.GoodId
				 && SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).Goods[i].GoodIndex == goodsInfo.GoodIndex)
				{
					SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).Goods[i].BuyOrNot = goodsInfo.BuyOrNot;
					SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).Goods[i].Consume = goodsInfo.Consume;
					SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).Goods[i].Cost = goodsInfo.Cost;
					SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).Goods[i].DiscountCost = goodsInfo.DiscountCost;
					break;
				}
			}
		else
			for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.Normal).Goods.Count; i++)
			{
				if (SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.Normal).Goods[i].GoodId == goodsInfo.GoodId
				 && SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.Normal).Goods[i].GoodIndex == goodsInfo.GoodIndex)
				{
					SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.Normal).Goods[i].BuyOrNot = goodsInfo.BuyOrNot;
					SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.Normal).Goods[i].Consume = goodsInfo.Consume;
					SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.Normal).Goods[i].Cost = goodsInfo.Cost;
					SysLocalDataBase.Inst.LocalPlayer.GetMysteryShopByType(MysteryShopConfig._ShopType.Normal).Goods[i].DiscountCost = goodsInfo.DiscountCost;
					break;
				}
			}

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlTowerNormalShop))
			SysUIEnv.Instance.GetUIModule<UIPnlTowerNormalShop>().OnBuyGoodSuccess(goodsInfo.GoodId, goodsInfo.GoodIndex);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_MYSTERY_SHOP_BUY_FAILED_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}
#endregion

#region  Assistant
class QueryTaskListRes : Response
{
	private List<com.kodgames.corgi.protocol.Task> tasks;

	public QueryTaskListRes(int pRqstID, List<com.kodgames.corgi.protocol.Task> tasks)
		: base(pRqstID)
	{
		this.tasks = tasks;
	}

	public QueryTaskListRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Set Task Data.
		SysLocalDataBase.Inst.LocalPlayer.TaskData.SetTasks(tasks);

		// Sort By Index.
		SysLocalDataBase.Inst.LocalPlayer.TaskData.Tasks.Sort((t1, t2) =>
			{
				int d1 = ConfigDatabase.DefaultCfg.TaskConfig.GetTaskById(t1.taskId).Priority;
				int d2 = ConfigDatabase.DefaultCfg.TaskConfig.GetTaskById(t2.taskId).Priority;

				return d1 - d2;
			});

		// Notify UI.
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAssistantTask))
			SysUIEnv.Instance.GetUIModule<UIPnlAssistantTask>().QueryTaskListReqSuccess();

		return true;
	}
}

class TaskConditionRes : Response
{
	public TaskConditionRes(int pRqstID)
		: base(pRqstID)
	{

	}

	public TaskConditionRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		return true;
	}
}

#endregion

#region  MelaTower
class QueryMelaleucaFloorPlayerInfoRes : Response
{
	private KodGames.ClientClass.MelaleucaFloorData mfInfo;
	public QueryMelaleucaFloorPlayerInfoRes(int pRqstID, KodGames.ClientClass.MelaleucaFloorData mfInfo)
		: base(pRqstID)
	{
		this.mfInfo = mfInfo;
	}

	public QueryMelaleucaFloorPlayerInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// 进入千层楼初始化玩家信息

		int gotoLayer = 0;
		bool isConnect = false;

		//战斗场景跨天判定 
		if (SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint > mfInfo.CurrentPoint)
			isConnect = true;

		//如果在千机楼中，并且跨天
		if (SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().IsInTower && SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.IsResetPlayerInfo != mfInfo.IsResetPlayerInfo)
			isConnect = true;

		if (SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint > 0)
			gotoLayer = mfInfo.CurrentLayer - SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentLayer;

		if (SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().IsSweepBattle)
		{
			gotoLayer = 0;
			SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().IsSweepBattle = false;
		}

		int limitLayer = SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().LimitLayer;

		//如果第一次上到扫荡层
		bool isSweepUnlock = false;
		if (SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.MaxPassLayer != 0 && SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.MaxPassLayer < limitLayer && mfInfo.CurrentLayer >= limitLayer)
			isSweepUnlock = true;

		SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData = mfInfo;
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlTowerPlayerInfo)))
			SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().OnPlayerInfoSuccess(gotoLayer, isConnect, isSweepUnlock);
		else if (isConnect)
			SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().TowerConnect();

		return true;
	}
}

class QueryMelaleucaFloorInfoRes : Response
{

	private List<com.kodgames.corgi.protocol.NpcInfo> npcInfos;

	public QueryMelaleucaFloorInfoRes(int pRqstID, List<com.kodgames.corgi.protocol.NpcInfo> npcInfos, List<KodGames.ClientClass.Reward> passRewards, List<KodGames.ClientClass.Reward> firstPassRewards)
		: base(pRqstID)
	{
		this.npcInfos = npcInfos;
	}

	public QueryMelaleucaFloorInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().QueryMelaleucaFloorInfoSuccess(npcInfos);

		return true;
	}
}

class OnMelaleucaFloorCombatRes : Response
{
	private com.kodgames.corgi.protocol.MelaleucaFloorInfo mfinfo;
	private KodGames.ClientClass.CostAndRewardAndSync perDayCrs;
	private List<KodGames.ClientClass.CostAndRewardAndSync> passCrs;
	private List<KodGames.ClientClass.CostAndRewardAndSync> fistPassCrs;
	private KodGames.ClientClass.CombatResultAndReward combatResult;
	private List<int> firstPassLayers;
	private int layers;

	public OnMelaleucaFloorCombatRes(int pRqstID, com.kodgames.corgi.protocol.MelaleucaFloorInfo mfInfo, KodGames.ClientClass.CostAndRewardAndSync perDayCrs, List<KodGames.ClientClass.CostAndRewardAndSync> passCrs, List<KodGames.ClientClass.CostAndRewardAndSync> firstPassCrs, KodGames.ClientClass.CombatResultAndReward combatResult, List<int> firstPassLayers, int layers)
		: base(pRqstID)
	{
		this.mfinfo = mfInfo;
		this.perDayCrs = perDayCrs;
		this.passCrs = passCrs;
		this.fistPassCrs = firstPassCrs;
		this.combatResult = combatResult;
		this.firstPassLayers = firstPassLayers;
		this.layers = layers;
	}

	public OnMelaleucaFloorCombatRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process CostAndReward.
		for (int i = 0; i < passCrs.Count; i++)
			SysLocalDataBase.Inst.ProcessCostRewardSync(passCrs[i], "OnMelaleucaFloorCombat");
		for (int i = 0; i < fistPassCrs.Count; i++)
			SysLocalDataBase.Inst.ProcessCostRewardSync(fistPassCrs[i], "OnMelaleucaFloorCombat");
		SysLocalDataBase.Inst.ProcessCostRewardSync(perDayCrs, "OnMelaleucaFloorCombat");

		List<object> paramsters = new List<object>();
		UIPnlTowerBattleResult.TowerBattleResultData battleData = new UIPnlTowerBattleResult.TowerBattleResultData(combatResult, mfinfo, perDayCrs, passCrs, fistPassCrs, firstPassLayers, layers);
		paramsters.Add(combatResult);
		paramsters.Add(battleData);
		SysGameStateMachine.Instance.EnterState<GameState_Battle>(paramsters);

		return true;
	}
}

class MelaleucaFloorConsequentCombatRes : Response
{
	private com.kodgames.corgi.protocol.MelaleucaFloorInfo mfinfo;
	private KodGames.ClientClass.CostAndRewardAndSync perDayCrs;
	private List<KodGames.ClientClass.CostAndRewardAndSync> passCrs;
	private List<KodGames.ClientClass.CostAndRewardAndSync> firstPassCrs;
	private KodGames.ClientClass.CombatResultAndReward combatResult;
	private List<int> firstPassLayers;
	private int layers;
	private int combatCount;


	public MelaleucaFloorConsequentCombatRes(int pRqstID, com.kodgames.corgi.protocol.MelaleucaFloorInfo mfInfo, KodGames.ClientClass.CostAndRewardAndSync perDayCrs, List<KodGames.ClientClass.CostAndRewardAndSync> passCrs, List<KodGames.ClientClass.CostAndRewardAndSync> firstPassCrs, KodGames.ClientClass.CombatResultAndReward combatResult, List<int> firstPassLayers, int layers, int combatCount)
		: base(pRqstID)
	{
		this.mfinfo = mfInfo;
		this.perDayCrs = perDayCrs;
		this.passCrs = passCrs;
		this.firstPassCrs = firstPassCrs;
		this.combatResult = combatResult;
		this.firstPassLayers = firstPassLayers;
		this.layers = layers;
		this.combatCount = combatCount;
	}

	public MelaleucaFloorConsequentCombatRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process CostAndReward.
		for (int i = 0; i < passCrs.Count; i++)
			SysLocalDataBase.Inst.ProcessCostRewardSync(passCrs[i], "MelaleucaFloorConsequentCombat");
		for (int i = 0; i < firstPassCrs.Count; i++)
			SysLocalDataBase.Inst.ProcessCostRewardSync(firstPassCrs[i], "MelaleucaFloorConsequentCombat");
		SysLocalDataBase.Inst.ProcessCostRewardSync(perDayCrs, "MelaleucaFloorConsequentCombat");

		UIPnlTowerSweepBattle.TowerSweepResultData sweepData = new UIPnlTowerSweepBattle.TowerSweepResultData(combatResult, mfinfo, perDayCrs, passCrs, firstPassCrs, firstPassLayers, layers, combatCount);

		// Talking Game Tower. First than show UI,.
		GameAnalyticsUtility.OnEventBattleTower(sweepData.IsWinner(), sweepData.MelaleucaFloorInfo.currentLayer, sweepData.MelaleucaFloorInfo.currentPoint - SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint);

		// Show UI.
		SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().QuerySweepBattleSuccess(sweepData);

		return true;
	}
}


class OnQueryMelaleucaFloorThisWeekRankRes : Response
{
	private int layer;
	private int point;
	private int maxPointWeek;
	private int predictRank;
	private List<com.kodgames.corgi.protocol.MfRankInfo> rankInfos;

	public OnQueryMelaleucaFloorThisWeekRankRes(int pRqstID, int layer, int point, int maxPointWeek, int predictRank, List<com.kodgames.corgi.protocol.MfRankInfo> rankInfos)
		: base(pRqstID)
	{
		this.layer = layer;
		this.point = point;
		this.maxPointWeek = maxPointWeek;
		this.predictRank = predictRank;
		this.rankInfos = rankInfos;
	}

	public OnQueryMelaleucaFloorThisWeekRankRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlTowerThisWeekRank>().OnQueryThisWeekInfoSuccess(layer, point, maxPointWeek, predictRank, rankInfos);

		return true;
	}
}

class OnQueryMelaleucaFloorLastWeekRankRes : Response
{
	private int rank;
	private int point;
	private int layer;
	private int predictRank;
	private List<com.kodgames.corgi.protocol.MfRankInfo> rankInfos;

	public OnQueryMelaleucaFloorLastWeekRankRes(int pRqstID, int rank, int point, int layer, List<com.kodgames.corgi.protocol.MfRankInfo> rankInfos)
		: base(pRqstID)
	{
		this.rank = rank;
		this.point = point;
		this.layer = layer;
		this.rankInfos = rankInfos;
	}

	public OnQueryMelaleucaFloorLastWeekRankRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.LastWeekRank = rank;
		SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.LastWeekPoint = point;
		SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.LastWeekLayer = layer;

		SysUIEnv.Instance.GetUIModule<UIPnlTowerLastWeekRank>().OnQueryLastWeekInfoSuccess(rank, point, layer, rankInfos);

		return true;
	}
}

class OnGetMelaleucaFloorWeekRewardRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public OnGetMelaleucaFloorWeekRewardRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync, bool isGetWeekReward)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnGetMelaleucaFloorWeekRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnGetMelaleucaFloorWeekReward");

		SysUIEnv.Instance.GetUIModule<UIPnlTowerWeekReward>().GetRewardSuccess(costAndRewardAndSync.Reward);
		return true;
	}
}

class OnQueryMelaleucaFloorWeekRewardInfoRes : Response
{
	private int weekRank;
	private bool isGetWeekReward;

	public OnQueryMelaleucaFloorWeekRewardInfoRes(int pRqstID, int weekRank, bool isGetWeekReward)
		: base(pRqstID)
	{
		this.weekRank = weekRank;
		this.isGetWeekReward = isGetWeekReward;
	}

	public OnQueryMelaleucaFloorWeekRewardInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlTowerWeekReward>().RequesetRewardSuccess(weekRank, isGetWeekReward);

		return true;
	}
}
#endregion

#region  WolfSmoke

//商店购买
class BuyWolfSmokeShopRes : Response
{
	private WolfSmokeGoodsInfo goodsInfo;
	private CostAndRewardAndSync costAndRewardAndSync;

	public BuyWolfSmokeShopRes(int pRqstID, bool isJoin, WolfSmokeGoodsInfo goodsInfo, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.goodsInfo = goodsInfo;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public BuyWolfSmokeShopRes(int pRqstID, int errCode, CostAndRewardAndSync costAndRewardAndSync, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var buyWolfSmokeReq = request as QueryBuyWolfSmokeShop;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, string.Format("BuyWolfSmokeShop_{0}", buyWolfSmokeReq.GoodId.ToString("X")));

		// Refresh UI.
		SysUIEnv.Instance.GetUIModule<UIPnlWolfNormalShop>().OnBuyGoodSuccess(goodsInfo.GoodsId, goodsInfo.GoodsIndex);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_WOLF_SMOKE_SHOP_MEDALS_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class OnCombatWolfSmokeRes : Response
{
	private CombatResultAndReward combatResultAndReward;
	private CostAndRewardAndSync costAndRewardAndSync;
	private int alreadyFailedTimes;
	private List<WolfEggs> wolfEggs;
	private CostAndRewardAndSync eggsCostAndRewardAndSync;
	private com.kodgames.corgi.protocol.Avatar showAvatar;
	private int alreadyResetTimes;
	private int stageId;

	public OnCombatWolfSmokeRes(int pRqstID, bool isJoin, CombatResultAndReward combatResultAndReward, CostAndRewardAndSync costAndRewardAndSync, int alreadyFailedTimes, int stageId, List<WolfEggs> wolfEggs, CostAndRewardAndSync eggsCostAndRewardAndSync, com.kodgames.corgi.protocol.Avatar showAvatar, int alreadyResetTimes)
		: base(pRqstID)
	{
		this.combatResultAndReward = combatResultAndReward;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.alreadyFailedTimes = alreadyFailedTimes;
		this.wolfEggs = wolfEggs;
		this.eggsCostAndRewardAndSync = eggsCostAndRewardAndSync;
		this.showAvatar = showAvatar;
		this.alreadyResetTimes = alreadyResetTimes;
		this.stageId = stageId;
	}

	public OnCombatWolfSmokeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var wolfBattleReq = request as QueryCombatWolfSmoke;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlWolfMyBattle)))
			SysUIEnv.Instance.GetUIModule<UIPnlWolfMyBattle>().Myhide();

		UIPnlWolfBattleResult.WolfBattleResultData battleResultData = new UIPnlWolfBattleResult.WolfBattleResultData(combatResultAndReward, wolfEggs, eggsCostAndRewardAndSync, showAvatar, alreadyFailedTimes, alreadyResetTimes, wolfBattleReq.AdditionId, stageId);

		// Go to combat state
		List<object> paramsters = new List<object>();
		paramsters.Add(combatResultAndReward);
		paramsters.Add(battleResultData);

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnCombatWolfSmoke");

		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Battle>(paramsters);

		return true;
	}
}

//参战协议
class QueryJoinWolfSmokeRes : Response
{
	private bool isJoin;
	public QueryJoinWolfSmokeRes(int pRqstID, bool isJoin)
		: base(pRqstID)
	{
		this.isJoin = isJoin;
	}

	public QueryJoinWolfSmokeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (isJoin)
			RequestMgr.Inst.Request(new QueryWolfSmoke());

		return true;
	}
}

//主查询协议
class QueryWolfSmokeRes : Response
{
	private bool isJoin;
	private WolfInfo wolfInfo;
	private Player wolfPlayer;
	private List<Location> locations;
	private Player enemyPlayer;
	private List<WolfSmokeAddition> wolfSmokeAdditions;
	private List<WolfSelectedAddition> wolfSelectedAdditions;
	private List<WolfAvatar> wolfAvatars;
	private int lastPositionId;

	public QueryWolfSmokeRes(int pRqstID, bool isJoin, WolfInfo wolfInfo, Player wolfPlayer, List<Location> locations, Player enemyPlayer, List<WolfSmokeAddition> wolfSmokeAdditions, List<WolfSelectedAddition> wolfSelectedAdditions, List<WolfAvatar> wolfAvatars, int lastPositionId)
		: base(pRqstID)
	{
		this.isJoin = isJoin;
		this.wolfInfo = wolfInfo;
		this.wolfPlayer = wolfPlayer;
		this.locations = locations;
		this.enemyPlayer = enemyPlayer;
		this.wolfSmokeAdditions = wolfSmokeAdditions;
		this.wolfSelectedAdditions = wolfSelectedAdditions;
		this.wolfAvatars = wolfAvatars;
		this.lastPositionId = lastPositionId;
	}

	public QueryWolfSmokeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		//如果没有参战，进入参战阵容选择界面
		//参战过，直接进入场景
		if (!isJoin)
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfExpedition), lastPositionId);
		else
		{
			UIPnlWolfInfo.WolfSmokeData wolfSmokeData = new UIPnlWolfInfo.WolfSmokeData(isJoin, wolfInfo, wolfPlayer, locations, enemyPlayer, wolfSmokeAdditions, wolfSelectedAdditions, wolfAvatars, lastPositionId);

			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlWolfBattleResult))
			{

				List<object> paramsters = new List<object>();
				UIPnlWolfBattleResult.WolfBattleResultData wolfBattleResultData = SysUIEnv.Instance.GetUIModule<UIPnlWolfBattleResult>().GetwolfBattleInfo();

				paramsters.Add(wolfSmokeData);
				paramsters.Add(wolfBattleResultData);
				if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgWolfStart)))
					paramsters.Add(true);
				else
					paramsters.Add(false);
				SysGameStateMachine.Instance.EnterState<GameState_WolfSmoke>(paramsters);

			}
			else
			{
				//从场景中请求
				List<object> paramsters = new List<object>();
				paramsters.Add(wolfSmokeData);
				SysGameStateMachine.Instance.EnterState<GameState_WolfSmoke>(paramsters);
			}
		}

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		string message = string.Empty;
		List<WolfSmokeConfig.OpenLimit> openLimits = ConfigDatabase.DefaultCfg.WolfSmokeConfig.OpenLimits;

		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_WOLF_SMOKE_AREA_OPEN_TIME)
			for (int i = 0; i < openLimits.Count; i++)
			{
				if (openLimits[i].Type == WolfSmokeConfig._WolfSmokeOpenLimit.OpenAreaTime)
					message = string.Format(openLimits[i].Desc, openLimits[i].Value);
			}

		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_WOLF_SMOKE_CTEATE_ACCOUNT_TIME)
			for (int i = 0; i < openLimits.Count; i++)
			{
				if (openLimits[i].Type == WolfSmokeConfig._WolfSmokeOpenLimit.CreateAccountTime)
					message = string.Format(openLimits[i].Desc, openLimits[i].Value);
			}

		if (string.IsNullOrEmpty(message))
			message = errMsg;

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), message);
	}
}

class QueryWolfSmokeEnemyRes : Response
{
	public QueryWolfSmokeEnemyRes(int pRqstID, bool isJoin, WolfSmokePlayer wolfsmokeplayer)
		: base(pRqstID)
	{
	}

	public QueryWolfSmokeEnemyRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		return true;
	}
}

class QueryWolfSmokePositionRes : Response
{
	public QueryWolfSmokePositionRes(int pRqstID, bool isJoin, WolfSmokePlayer WolfSmokePlayer, List<WolfSmokeAddition> wolfSmokeAdditions, List<WolfSelectedAddition> wolfSelectedAdditions)
		: base(pRqstID)
	{
	}

	public QueryWolfSmokePositionRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		return true;
	}
}

class QueryWolfSmokeShopRes : Response
{
	private List<WolfSmokeGoodsInfo> goodsInfos;

	public QueryWolfSmokeShopRes(int pRqstID, bool isJoin, List<WolfSmokeGoodsInfo> goodsInfos)
		: base(pRqstID)
	{
		this.goodsInfos = goodsInfos;
	}

	public QueryWolfSmokeShopRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlWolfNormalShop>().OnQuerynNormalShopInfoSuccess(goodsInfos);

		return true;
	}
}

//刷新商店
class QueryRefreshWolfSmokeShopRes : Response
{
	private List<WolfSmokeGoodsInfo> goodsInfos;
	private CostAndRewardAndSync costAndRewardAndSync;

	public QueryRefreshWolfSmokeShopRes(int pRqstID, bool isJoin, List<WolfSmokeGoodsInfo> goodsInfos, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.goodsInfos = goodsInfos;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public QueryRefreshWolfSmokeShopRes(int pRqstID, int errCode, CostAndRewardAndSync costAndRewardAndSync, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "QueryRefreshWolfSmokeShop");

		// Refresh UI.
		SysUIEnv.Instance.GetUIModule<UIPnlWolfNormalShop>().OnQuerynNormalShopInfoSuccess(goodsInfos);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_WOLF_SMOKE_SHOP_MEDALS_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class QueryResetWolfSmokeRes : Response
{
	public QueryResetWolfSmokeRes(int pRqstID, bool isJoin, WolfInfo wolfInfo)
		: base(pRqstID)
	{
	}

	public QueryResetWolfSmokeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgWolfStart)))
			SysUIEnv.Instance.GetUIModule<UIDlgWolfStart>().ResetWolfSmoke();

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgWolfFailyDeficiency)))
			SysUIEnv.Instance.GetUIModule<UIDlgWolfFailyDeficiency>().ResetWolfSmoke();

		return true;
	}
}

#endregion

#region Friend
class OnQueryFriendListRes : Response
{
	private List<KodGames.ClientClass.FriendInfo> friendInfos;

	public OnQueryFriendListRes(int pRqstID, List<KodGames.ClientClass.FriendInfo> friendInfos)
		: base(pRqstID)
	{
		this.friendInfos = friendInfos;
	}

	public OnQueryFriendListRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryReq = request as QueryFriendListReq;
		if (queryReq != null && queryReq.QuerySuccessDel != null)
			queryReq.QuerySuccessDel(friendInfos);
		else
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendTab)))
				SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnQueryFriendListSuccess(friendInfos);
			else if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendSelectFriends)))
				SysUIEnv.Instance.GetUIModule<UIPnlFriendSelectFriends>().OnQueryFriendListSuccess(friendInfos);
		}

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

class OnRandomFriendRes : Response
{
	private List<KodGames.ClientClass.FriendInfo> friendInfos;

	public OnRandomFriendRes(int pRqstID, List<KodGames.ClientClass.FriendInfo> friendInfos)
		: base(pRqstID)
	{
		this.friendInfos = friendInfos;
	}

	public OnRandomFriendRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnRandomFriendSuccess(friendInfos);
		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

class OnQueryPlayerNameRes : Response
{
	private List<FriendInfo> friendInfos;

	public OnQueryPlayerNameRes(int pRqstID, List<FriendInfo> friendInfos)
		: base(pRqstID)
	{
		this.friendInfos = friendInfos;
	}

	public OnQueryPlayerNameRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnQueryPlayerNameSuccess(friendInfos);
		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

class OnInviteFriendRes : Response
{
	public OnInviteFriendRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public OnInviteFriendRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		InviteFriendReq inviteFriend = request as InviteFriendReq;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlFriendTab))
			SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnInviteFriendSuccess(inviteFriend.InvitedPlayerId, true);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildApplyInfo))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildApplyInfo>().RequestInviteFriendSuccess();

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildIntroMember))
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgFriendMsg_Title_InviteFriendSuccess"));

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		//InviteFriendReq inviteFriend = request as InviteFriendReq;
		//SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnInviteFriendFailed(inviteFriend.InvitedPlayerId, errCode);

		base.PrcErr(request, errCode, errMsg);
	}
}

class OnAnswerFriendRes : Response
{
	private bool agree;

	public OnAnswerFriendRes(int pRqstID, bool agree)
		: base(pRqstID)
	{
		this.agree = agree;
	}

	public OnAnswerFriendRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public OnAnswerFriendRes(int pRqstID, bool agree, string errMsg)
		: base(pRqstID)
	{
		this.agree = agree;

		if (string.IsNullOrEmpty(errMsg) == false)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errMsg);
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlEmail>().OnAnswerFriendReqSuccess(agree);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

class OnQueryFriendPlayerInfoRes : Response
{
	private Player player;

	public OnQueryFriendPlayerInfoRes(int pRqstID, Player player)
		: base(pRqstID)
	{
		this.player = player;
	}

	public OnQueryFriendPlayerInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgFriendStartOne)))
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFriendAvatarView), player, IDSeg.InvalidId);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlViewAvatar), player);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

class OnRemoveFriendRes : Response
{
	public OnRemoveFriendRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public OnRemoveFriendRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		RemoveFriendReq removeFriend = request as RemoveFriendReq;

		SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnRemoveFriendSuccess(removeFriend.RemovePlayer.PlayerId);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{

		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_REMOVE_FRIEND_NOT_FRIEND_FAILED)
		{
			RemoveFriendReq removeFriend = request as RemoveFriendReq;
			SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnRemoveFriendAlready(removeFriend.RemovePlayer);
		}

		base.PrcErr(request, errCode, errMsg);
	}
}

class OnCombatFriendRes : Response
{
	private CombatResultAndReward combatResultAndReward;
	private CostAndRewardAndSync costAndRewardAndSync;

	public OnCombatFriendRes(int pRqstID, CombatResultAndReward combatResultAndReward, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.combatResultAndReward = combatResultAndReward;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnCombatFriendRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		CombatFriendReq combatReq = request as CombatFriendReq;

		UIPnlCombatFriendBattleResult.CombatFriendBattleResultData battleResultData = new UIPnlCombatFriendBattleResult.CombatFriendBattleResultData(combatResultAndReward, combatReq.FriendId);

		// Go to combat state
		List<object> paramsters = new List<object>();
		paramsters.Add(combatResultAndReward);
		paramsters.Add(battleResultData);

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnCombatFriend");
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Battle>(paramsters);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

class OnDelFriendRes : Response
{
	private int playerId;

	public OnDelFriendRes(int playerId)
		: base(Request.InvalidID)
	{
		this.playerId = playerId;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		//玩家在好友列表时，删除实时显示
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlFriendTab))
			SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnRemoveFriendSuccess(playerId);

		return true;
	}
}

class OnAddFriendRes : Response
{
	private FriendInfo friendInfo;

	public OnAddFriendRes(FriendInfo friendInfo)
		: base(Request.InvalidID)
	{
		this.friendInfo = friendInfo;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		//玩家在好友列表时，添加实时显示
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlFriendTab))
			SysUIEnv.Instance.GetUIModule<UIPnlFriendTab>().OnAddFriendSuccess(friendInfo);

		return true;
	}
}
#endregion
#region QinInfo

class QueryQinInfoRes : Response
{
	private KodGames.ClientClass.QinInfo qinInfo;

	public QueryQinInfoRes(int pRqstID, QinInfo qinInfo)
		: base(pRqstID)
	{
		this.qinInfo = qinInfo;
	}

	public QueryQinInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityQinInfo))
			SysUIEnv.Instance.GetUIModule<UIPnlActivityQinInfo>().OnResponseQueryQinInfoSuccess(qinInfo);

		return true;
	}
}

class AnswerQinInfoRes : Response
{
	private KodGames.ClientClass.QinInfo qinInfo;
	private bool rightfalse;
	private CostAndRewardAndSync costAndRewardAndSync;
	public AnswerQinInfoRes(int pRqstID, bool rightfalse, CostAndRewardAndSync costAndRewardAndSync, QinInfo qinInfo)
		: base(pRqstID)
	{
		this.rightfalse = rightfalse;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.qinInfo = qinInfo;
	}

	public AnswerQinInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "AnswerQinInfo");

		if (rightfalse)
		{
			string rewardText = GameUtility.GetUIString("UIPnlActivityQinInfo_AnswerRight");

			for (int i = 0; i < costAndRewardAndSync.Reward.Consumable.Count; i++)
			{
				rewardText += GameUtility.FormatUIString("UIElemShopPropItem_Goods_NameXCount_Str", ItemInfoUtility.GetAssetName(costAndRewardAndSync.Reward.Consumable[i].Id), costAndRewardAndSync.Reward.Consumable[i].Amount);
				rewardText += "";
			}

			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), rewardText);
		}
		else
		{
			QinInfoConfig.Question quesiton = ConfigDatabase.DefaultCfg.QinInfoConfig.GetQuestionById((request as AnswerQinInfoReq).QuestionId);

			string text = GameUtility.GetUIString("UIPnlActivityQinInfo_AnswerFalse");
			string answerText = GameUtility.FormatUIString("UIPnlActivityQinInfo_AnswerText", QinInfoConfig._AnswerNum.GetNameByType(quesiton.RightAnswer), quesiton.Answers[quesiton.RightAnswer - 1].Content);
			text += answerText;

			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), text);

		}

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityQinInfo))
			SysUIEnv.Instance.GetUIModule<UIPnlActivityQinInfo>().OnResponseAnswerSuccess(qinInfo, rightfalse);

		return true;
	}
}

class GetQinInfoContinueRewardRes : Response
{
	private KodGames.ClientClass.QinInfo qinInfo;
	private CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.Reward fixReward;
	private KodGames.ClientClass.Reward randomReward;

	public GetQinInfoContinueRewardRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.Reward fixReward, KodGames.ClientClass.Reward randomReward, QinInfo qinInfo)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.qinInfo = qinInfo;
		this.fixReward = fixReward;
		this.randomReward = randomReward;
	}

	public GetQinInfoContinueRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "GetQinInfoContinueReward");

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityQinInfo_TipFlow_RewardGot"));

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityQinInfo))
			SysUIEnv.Instance.GetUIModule<UIPnlActivityQinInfo>().OnResponseGetRewardSuccess(qinInfo, fixReward, randomReward);

		return true;
	}
}
#endregion

#region MonthCard

class QueryMonthCardInfoRes : Response
{
	private List<com.kodgames.corgi.protocol.OneMonthCardInfo> monthCardInfos;
	private long lastResetTime;

	public QueryMonthCardInfoRes(int pRqstID, List<com.kodgames.corgi.protocol.OneMonthCardInfo> monthCardInfos, long lastResetTime)
		: base(pRqstID)
	{
		this.monthCardInfos = monthCardInfos;
		this.lastResetTime = lastResetTime;
	}

	public QueryMonthCardInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		ActivityManager.Instance.GetActivity<ActivityMonthCard>().MonthCardInfos = monthCardInfos;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityMonthCardTab) && !SysUIEnv.Instance.GetUIModule(_UIType.UIPnlActivityMonthCardTab).IsOverlayed)
			SysUIEnv.Instance.GetUIModule<UIPnlActivityMonthCardTab>().OnResponseQuerySuccess(lastResetTime);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlMonthCardDetail))
			SysUIEnv.Instance.GetUIModule<UIPnlMonthCardDetail>().OnResponseQuerySuccess(monthCardInfos);

		return true;
	}
}

class MonthCardPickRewardRes : Response
{
	private int type;
	private KodGames.ClientClass.Reward fixReward;
	private KodGames.ClientClass.Reward randomReward;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private List<com.kodgames.corgi.protocol.OneMonthCardInfo> monthCardInfos;

	public MonthCardPickRewardRes(int pRqstID, int type, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.Reward fixReward, KodGames.ClientClass.Reward randomReward, List<com.kodgames.corgi.protocol.OneMonthCardInfo> monthCardInfos)
		: base(pRqstID)
	{
		this.type = type;
		this.fixReward = fixReward;
		this.randomReward = randomReward;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.monthCardInfos = monthCardInfos;
	}

	public MonthCardPickRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "MonthCardPickReward");

		ActivityManager.Instance.GetActivity<ActivityMonthCard>().MonthCardInfos = monthCardInfos;

		string tipsText = "";
		switch (type)
		{
			case MonthCardRewardType.BuyReward:
				tipsText = GameUtility.GetUIString("UIPnlActivityMonthCardInfo_BuyRewardSuccess");
				break;
			case MonthCardRewardType.DailyReward:
				tipsText = GameUtility.GetUIString("UIPnlActivityMonthCardInfo_DailyRewardSuccess");
				break;
			case MonthCardRewardType.TenTimesReward:
				tipsText = GameUtility.GetUIString("UIPnlActivityMonthCardInfo_TenRewardSuccess");
				break;
		}
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), tipsText);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlActivityMonthCardTab) && !SysUIEnv.Instance.GetUIModule(_UIType.UIPnlActivityMonthCardTab).IsOverlayed)
			SysUIEnv.Instance.GetUIModule<UIPnlActivityMonthCardTab>().OnResponseQuerySuccess();

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlMonthCardDetail))
			SysUIEnv.Instance.GetUIModule<UIPnlMonthCardDetail>().OnResponseQuerySuccess(monthCardInfos);

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgMonthCardView)))
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgMonthCardView));

		UIDlgShopGiftPreview.ShowData showdata = new UIDlgShopGiftPreview.ShowData();

		var rewardData = new UIDlgShopGiftPreview.RewardData();
		rewardData.rewards = SysLocalDataBase.CCRewardToCSCReward(fixReward);
		rewardData.title = GameUtility.GetUIString("UIPnlActivityQinInfo_MessageTitle");
		showdata.rewardDatas.Add(rewardData);
		showdata.btnText = GameUtility.GetUIString("UIPnlActivityMonthCardTab_GetRewardSuccessDlg_OkBtn");

		List<ClientServerCommon.Reward> rewards = SysLocalDataBase.CCRewardToCSCReward(randomReward);

		if (rewards != null && rewards.Count > 0)
		{
			rewardData = new UIDlgShopGiftPreview.RewardData();
			rewardData.rewards = rewards;
			rewardData.title = GameUtility.GetUIString("UIPnlActivityQinInfo_MessageRandom");
			showdata.rewardDatas.Add(rewardData);
		}

		switch (type)
		{
			case MonthCardRewardType.BuyReward:
				showdata.title = GameUtility.GetUIString("UIPnlActivityMonthCardInfo_GetBuyReward");
				break;
			case MonthCardRewardType.DailyReward:
				showdata.title = GameUtility.GetUIString("UIPnlActivityMonthCardInfo_GetDailyReward");
				break;
			case MonthCardRewardType.TenTimesReward:
				showdata.title = GameUtility.GetUIString("UIPnlActivityMonthCardInfo_GetTenReward");
				break;
		}

		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgShopGiftPreview), _UILayer.Top, showdata);

		return true;
	}
}

#endregion

#region 绿点推送
class QueryNotifyRes : Response
{
	private int assisantNum;
	private List<com.kodgames.corgi.protocol.ActivityData> activityData;
	private List<com.kodgames.corgi.protocol.State> functionStates;
	private List<KodGames.ClientClass.Quest> changedQuests;
	private KodGames.ClientClass.QuestQuick questQuick;

	public QueryNotifyRes(int pRqstID, int assisantNum, List<com.kodgames.corgi.protocol.ActivityData> activityData, List<com.kodgames.corgi.protocol.State> functionStates, List<KodGames.ClientClass.Quest> changedQuests, KodGames.ClientClass.QuestQuick questQuick)
		: base(pRqstID)
	{
		this.assisantNum = assisantNum;
		this.activityData = activityData;
		this.functionStates = functionStates;
		this.changedQuests = changedQuests;
		this.questQuick = questQuick;
	}

	public QueryNotifyRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.UpdateNotifyData(assisantNum, activityData, functionStates, changedQuests, questQuick);

		return true;
	}
}
#endregion

#region GiveMeFive

class GiveFiveStarsEvaluateRes : Response
{
	public GiveFiveStarsEvaluateRes(int pRqstId)
		: base(pRqstId) { }

	public GiveFiveStarsEvaluateRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

#endregion

#region RunActivity

//主查询协议
class OperationActivityQueryRes : Response
{
	private int activityId;
	private int index;
	private int money;
	private List<com.kodgames.corgi.protocol.OperationActivityItem> operationActivityItems;
	private long rechargeStart;
	private long rechargeEnd;
	private long rewardStart;
	private long rewardEnd;

	public OperationActivityQueryRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public OperationActivityQueryRes(int pRqstId, int activityId, int index, int money, List<com.kodgames.corgi.protocol.OperationActivityItem> operationActivityItems, long rechargeStart, long rechargeEnd, long rewardStart, long rewardEnd)
		: base(pRqstId)
	{
		this.activityId = activityId;
		this.index = index;
		this.money = money;
		this.operationActivityItems = operationActivityItems;
		this.rechargeStart = rechargeStart;
		this.rechargeEnd = rechargeEnd;
		this.rewardStart = rewardStart;
		this.rewardEnd = rewardEnd;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var activityData = ActivityManager.Instance.GetActivityInRunActivity(activityId) as ActivityRun;
		if (activityData != null)
		{
			activityData.AccumulateIndex = index;
			activityData.AccumulateMoney = money;
			activityData.OperationActivityItems = operationActivityItems;
			activityData.RechargeStart = rechargeStart;
			activityData.RechargeEnd = rechargeEnd;
			activityData.RewardStart = rewardStart;
			activityData.RewardEnd = rewardEnd;
		}

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlRunAccumulativeTab)))
			SysUIEnv.Instance.GetUIModule<UIPnlRunAccumulativeTab>().OnQueryInfoSuccess(this.activityId);

		var queryReq = request as OperationActivityQueryReq;
		if (queryReq.Del != null)
			queryReq.Del();

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlRunAccumulativeTab))
			SysUIEnv.Instance.GetUIModule<UIPnlRunAccumulativeTab>().OnQueryInfoFailed();
	}
}

//领取奖励协议
class OnOperationActivityPickRewardRes : Response
{
	private com.kodgames.corgi.protocol.OperationActivityItem operationActivityItem;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardSync;

	public OnOperationActivityPickRewardRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public OnOperationActivityPickRewardRes(int pRqstId, com.kodgames.corgi.protocol.OperationActivityItem operationActivityItem, KodGames.ClientClass.CostAndRewardAndSync costAndRewardSync)
		: base(pRqstId)
	{
		this.operationActivityItem = operationActivityItem;
		this.costAndRewardSync = costAndRewardSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost and Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardSync, "OnOperationActivityPickReward");

		// Reset Activity Data.
		var activityData = ActivityManager.Instance.GetActivityInRunActivity((request as OperationActivityPickRewardReq).ActivityId) as ActivityRun;
		if (activityData != null)
		{
			for (int index = 0; index < activityData.OperationActivityItems.Count; index++)
			{
				if (activityData.OperationActivityItems[index].itemId == operationActivityItem.itemId)
				{
					activityData.OperationActivityItems[index] = operationActivityItem;
					break;
				}
			}
		}

		// Notify UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlRunAccumulativeTab)))
			SysUIEnv.Instance.GetUIModule<UIPnlRunAccumulativeTab>().OnGetRewardSuccess(operationActivityItem, costAndRewardSync);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}
#endregion

#region Adventure

class MarvellousNextMarvellousRes : Response
{
	private com.kodgames.corgi.protocol.MarvellousProto marvellousProto;
	private KodGames.ClientClass.CombatResultAndReward combatResultAndReward;
	private List<com.kodgames.corgi.protocol.DelayReward> delayRewards;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.CostAndRewardAndSync fixRewardPackage;
	private KodGames.ClientClass.CostAndRewardAndSync randRewardPackage;
	private KodGames.ClientClass.CostAndRewardAndSync normalTipsReward;

	public MarvellousNextMarvellousRes(int pRqstID,
		com.kodgames.corgi.protocol.MarvellousProto marvellousProto,
		KodGames.ClientClass.CombatResultAndReward combatResultAndReward,
		List<com.kodgames.corgi.protocol.DelayReward> delayRewards,
		KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync,
		KodGames.ClientClass.CostAndRewardAndSync fixRewardPackage,
		KodGames.ClientClass.CostAndRewardAndSync randRewardPackage,
		KodGames.ClientClass.CostAndRewardAndSync normalTipsReward
		)
		: base(pRqstID)
	{
		this.marvellousProto = marvellousProto;
		this.combatResultAndReward = combatResultAndReward;
		this.delayRewards = delayRewards;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.fixRewardPackage = fixRewardPackage; //显示用宝箱奖励-固定
		this.randRewardPackage = randRewardPackage;//显示用宝箱奖励-额外
		this.normalTipsReward = normalTipsReward;
	}

	public MarvellousNextMarvellousRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "MarvellousNextMarvellousRes");

		if (AdventureSceneData.Instance.getDelayRewards != null && delayRewards != null)
			AdventureSceneData.Instance.getDelayRewards.AddRange(delayRewards);

		if (marvellousProto.currentEventId == 0 && marvellousProto.completedMarvellousScenarios.Count == 0)
		{
			List<object> parameters = new List<object>();
			parameters.Add(marvellousProto);
			parameters.Add(AdventureSceneData.Instance.getDelayRewards);
			SysGameStateMachine.Instance.EnterState<GameState_Adventure>(parameters, true);
		}

		//战斗
		if (combatResultAndReward != null && combatResultAndReward.BattleRecords.Count > 0)
		{
			UIPnlAdventureCombatResult.CombatFriendBattleResultData battleResultData = new UIPnlAdventureCombatResult.CombatFriendBattleResultData(combatResultAndReward);

			// Go to combat state
			List<object> paramsters = new List<object>();
			paramsters.Add(combatResultAndReward);
			paramsters.Add(battleResultData);
			List<Pair<int, int>> fixRewardPackagePars = SysLocalDataBase.ConvertIdCountList(fixRewardPackage.Reward);
			List<Pair<int, int>> randRewardPackagePars = SysLocalDataBase.ConvertIdCountList(randRewardPackage.Reward);
			AdventureSceneData.combatMarvellouseProto = marvellousProto;
			AdventureSceneData.delayRewardsConst = delayRewards;
			AdventureSceneData.fixRewardPackagePars = fixRewardPackagePars;
			AdventureSceneData.randRewardPackagePars = randRewardPackagePars;
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Battle>(paramsters);
		}
		//else
		{

			if (normalTipsReward != null && normalTipsReward.Reward != null)
			{
				List<Pair<int, int>> normalTipsRewardPars = SysLocalDataBase.ConvertIdCountList(normalTipsReward.Reward);
				foreach (Pair<int, int> pair in normalTipsRewardPars)
					SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UIDlgAdventureGetReward_Label_Reward_Tips", pair.second, ItemInfoUtility.GetAssetName(pair.first)), 0f, 0.5f);
			}
			if ((delayRewards != null && delayRewards.Count > 0) ||
				(fixRewardPackage != null && fixRewardPackage.Reward != null) ||
				(randRewardPackage != null && randRewardPackage.Reward != null)
				)
			{
				SysUIEnv.Instance.ShowUIModule<UIPnlAdventureGetReward>(marvellousProto, delayRewards, SysLocalDataBase.ConvertIdCountList(fixRewardPackage.Reward), SysLocalDataBase.ConvertIdCountList(randRewardPackage.Reward));
				SysUIEnv.Instance.HideUIModule<UIPnlAdventureScene>();
			}
			else
				AdventureSceneData.Instance.GetAdventureTypeByEventId(marvellousProto);
		}
		AdventureSceneData.isClick = true; //奇遇场景物体可点击
		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_MARVELLOUSNEXT_FAILED_CONSUMABLE_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class MarvellousPickDelayRewardRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private List<com.kodgames.corgi.protocol.DelayReward> delayRewards;

	public MarvellousPickDelayRewardRes(int pRqstID, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, List<com.kodgames.corgi.protocol.DelayReward> delayRewards)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.delayRewards = delayRewards;
	}

	public MarvellousPickDelayRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "Adventure");

		List<Pair<int, int>> showRewards = SysLocalDataBase.ConvertIdCountList(costAndRewardAndSync.Reward);

		foreach (var reward in showRewards)
		{
			string message = string.Format(GameUtility.GetUIString("UIPnlAdventureDelayReward_GetReward_Label"), ItemInfoUtility.GetAssetName(reward.first), reward.second);
			SysUIEnv.Instance.AddTip(message, 0f, 0.5f);
		}

		SysUIEnv.Instance.GetUIModule<UIPnlAdventureDelayReward>().OnQueryPickDelayRewardSuccess(delayRewards);

		return true;
	}
}

//查询延时奖励
class MarvellousQueryDelayRewardRes : Response
{
	private List<com.kodgames.corgi.protocol.DelayReward> delayRewards;

	public MarvellousQueryDelayRewardRes(int pRqstID, List<com.kodgames.corgi.protocol.DelayReward> delayRewards)
		: base(pRqstID)
	{
		this.delayRewards = delayRewards;
	}

	public MarvellousQueryDelayRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAdventureMain))
			SysUIEnv.Instance.GetUIModule<UIPnlAdventureMain>().OnQueryDelayRewardSuccess(delayRewards);
		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAdventureDelayReward))
			SysUIEnv.Instance.GetUIModule<UIPnlAdventureDelayReward>().QueryDelayRewardSucess(delayRewards);
		else if (SysGameStateMachine.Instance.CurrentState is GameState_Adventure)
			AdventureSceneData.Instance.getDelayRewards = delayRewards;

		return true;
	}
}

class MarvellousQueryRes : Response
{
	private com.kodgames.corgi.protocol.MarvellousProto marvellousProto;
	private List<com.kodgames.corgi.protocol.DelayReward> delayRewards;

	public MarvellousQueryRes(int pRqstID, com.kodgames.corgi.protocol.MarvellousProto marvellousProto, List<com.kodgames.corgi.protocol.DelayReward> delayRewards)
		: base(pRqstID)
	{

		this.marvellousProto = marvellousProto;
		this.delayRewards = delayRewards;
	}

	public MarvellousQueryRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		List<object> parameters = new List<object>();
		parameters.Add(marvellousProto);
		parameters.Add(delayRewards);
		SysGameStateMachine.Instance.EnterState<GameState_Adventure>(parameters);
		return true;
	}
}

#endregion

#region FriendCombatSystem

//主查询协议
class OnQueryFriendCampaignRes : Response
{
	private bool isJoin;
	private int passStageId;
	private int lastPositionId;
	private List<int> lastFriendIds;
	private int historyMaxDungeonId;
	private int alreadyResetCount;
	private KodGames.ClientClass.Player enemyPlayer;
	private List<KodGames.ClientClass.HpInfo> enemyHpInfos;
	private List<KodGames.ClientClass.FriendCampaignPosition> friendPositions;
	private int lastFriendPostitionId;
	private com.kodgames.corgi.protocol.RobotInfo robotInfo;

	public OnQueryFriendCampaignRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public OnQueryFriendCampaignRes(int callback, bool isJoin, int passStageId, int lastPositionId, List<int> lastFriendIds, int historyMaxDungeonId, int alreadyResetCount,
		KodGames.ClientClass.Player enemyPlayer, List<KodGames.ClientClass.HpInfo> enemyHpInfos, List<KodGames.ClientClass.FriendCampaignPosition> friendPositions, int lastFriendPositionId,
		com.kodgames.corgi.protocol.RobotInfo robotInfo)
		: base(callback)
	{
		this.isJoin = isJoin;
		this.passStageId = passStageId;
		this.lastPositionId = lastPositionId;
		this.lastFriendIds = lastFriendIds;
		this.historyMaxDungeonId = historyMaxDungeonId;
		this.alreadyResetCount = alreadyResetCount;
		this.enemyPlayer = enemyPlayer;
		this.enemyHpInfos = enemyHpInfos;
		this.friendPositions = friendPositions;
		this.lastFriendPostitionId = lastFriendPositionId;
		this.robotInfo = robotInfo;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (isJoin)
		{
			UIPnlFriendInfoTab.CombatData combatData = new UIPnlFriendInfoTab.CombatData(isJoin, passStageId, lastPositionId, lastFriendIds, historyMaxDungeonId, alreadyResetCount, enemyPlayer,
																							enemyHpInfos, friendPositions, lastFriendPostitionId, robotInfo);
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlFriendInfoTab), combatData);
		}
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlFriendStart), lastPositionId, lastFriendIds);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

//重置协议
class OnResetFriendCampaignRes : Response
{
	private bool isJoin;

	public OnResetFriendCampaignRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public OnResetFriendCampaignRes(int pRqstId, bool isJoin)
		: base(pRqstId)
	{
		this.isJoin = isJoin;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (isJoin && SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgFriendCampaignReset)))
			SysUIEnv.Instance.GetUIModule<UIDlgFriendCampaignReset>().ResetFriendCampaignSuccess();


		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

//战斗协议
class OnCombatFriendCampaignRes : Response
{
	private bool isJoin;
	private KodGames.ClientClass.CombatResultAndReward combatResultAndReward;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private int passStageId;
	private KodGames.ClientClass.Player enemyPlayer;
	private List<KodGames.ClientClass.HpInfo> enemyHpInfos;
	private com.kodgames.corgi.protocol.RobotInfo robotInfo;

	public OnCombatFriendCampaignRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public OnCombatFriendCampaignRes(int pRqstId, bool isJoin, KodGames.ClientClass.CombatResultAndReward combatResultAndReward,
		KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, int passStageId, KodGames.ClientClass.Player enemyPlayer, List<KodGames.ClientClass.HpInfo> enemyHpInfos,
		com.kodgames.corgi.protocol.RobotInfo robotInfo)
		: base(pRqstId)
	{
		this.isJoin = isJoin;
		this.combatResultAndReward = combatResultAndReward;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.passStageId = passStageId;
		this.enemyPlayer = enemyPlayer;
		this.enemyHpInfos = enemyHpInfos;
		this.robotInfo = robotInfo;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (this.isJoin)
		{
			UIPnlFriendCampaginBattleResult.FriendCampaginBattleResultData battleResultData = new UIPnlFriendCampaginBattleResult.FriendCampaginBattleResultData(passStageId, combatResultAndReward,
								enemyPlayer, enemyHpInfos, robotInfo);

			// Go to combat state
			List<object> paramsters = new List<object>();
			paramsters.Add(combatResultAndReward);
			paramsters.Add(battleResultData);

			SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnCombatFriendCampaignRes");

			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Battle>(paramsters);
		}
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTip), GameUtility.GetUIString("UIPnlFriendJoinFriendCampaignReqNotSuccess"));

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

//参战协议
class OnJoinFriendCampaignRes : Response
{
	private string deletedFriendName;

	public OnJoinFriendCampaignRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public OnJoinFriendCampaignRes(int pRqstId, string deletedFriendName)
		: base(pRqstId)
	{
		this.deletedFriendName = deletedFriendName;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (deletedFriendName.Equals(string.Empty))
			RequestMgr.Inst.Request(new QueryFriendCampaignReq());
		else
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgFriendStartOne)))
				SysUIEnv.Instance.GetUIModule<UIDlgFriendStartOne>().JoinFriendCampaignReqNotSuccess(deletedFriendName);

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendSelectFriends)) && !SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgFriendStartOne)))
				SysUIEnv.Instance.GetUIModule<UIPnlFriendSelectFriends>().JoinFriendCampaignReqNotSuccess(deletedFriendName);
		}

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

//查询好友协议
class OnQueryFriendCampaignHelpPlayerInfoRes : Response
{
	private List<KodGames.ClientClass.FriendInfo> friendInfos;

	public OnQueryFriendCampaignHelpPlayerInfoRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public OnQueryFriendCampaignHelpPlayerInfoRes(int pRqstId, List<KodGames.ClientClass.FriendInfo> friendInfos)
		: base(pRqstId)
	{
		for (int index = 0; index < friendInfos.Count; index++)
		{
			Debug.Log(friendInfos[index].PlayerId.ToString());
		}

		this.friendInfos = friendInfos;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		//show UI.
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFriendStartOne), (request as QueryFriendCampaignHelpFriendInfoReq).PositionId, friendInfos);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

//排行榜查询协议
class QueryFCRankRes : Response
{
	private int rankMaxSize;
	private string desc;
	private List<com.kodgames.corgi.protocol.FCRankInfo> rankInfos;
	private long nextResetTime;

	public QueryFCRankRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public QueryFCRankRes(int pRqstId, int rankMaxSize, List<com.kodgames.corgi.protocol.FCRankInfo> rankInfos, string desc, long nextResetTime)
		: base(pRqstId)
	{
		this.rankMaxSize = rankMaxSize;
		this.rankInfos = rankInfos;
		this.desc = desc;
		this.nextResetTime = nextResetTime;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendCampaginLastWeekRank)))
			SysUIEnv.Instance.GetUIModule<UIPnlFriendCampaginLastWeekRank>().OnQuerySuccess(rankMaxSize, rankInfos, desc, nextResetTime);
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendCampaginThisWeekRank)))
			SysUIEnv.Instance.GetUIModule<UIPnlFriendCampaginThisWeekRank>().OnQuerySuccess(rankMaxSize, rankInfos, desc, nextResetTime);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

//情义值详细查询协议
class QueryFCPointDetailRes : Response
{
	private int friendMaxCount;
	private int totalPoint;
	private string desc;
	private List<com.kodgames.corgi.protocol.FCPointInfo> pointInfos;

	public QueryFCPointDetailRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public QueryFCPointDetailRes(int pRqstId, int friendMaxCount, int totalPoint, List<com.kodgames.corgi.protocol.FCPointInfo> pointInfos, string desc)
		: base(pRqstId)
	{
		this.friendMaxCount = friendMaxCount;
		this.totalPoint = totalPoint;
		this.pointInfos = pointInfos;
		this.desc = desc;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFriendCampaginShips), friendMaxCount, totalPoint, desc, pointInfos);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

//查询奖励返回协议
class QueryFCRankRewarRes : Response
{
	private bool isGetReward;
	private string desc;
	private int rankNumber;
	private int maxRank;
	private List<com.kodgames.corgi.protocol.FCRewardInfo> rewardInfos;
	private long nextResetTime;

	public QueryFCRankRewarRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public QueryFCRankRewarRes(int pRqstId, bool isGetReward, int rankNumber, int maxRank, List<com.kodgames.corgi.protocol.FCRewardInfo> rewardInfos, string desc, long nextResetTime)
		: base(pRqstId)
	{
		this.isGetReward = isGetReward;
		this.rewardInfos = rewardInfos;
		this.rankNumber = rankNumber;
		this.maxRank = maxRank;
		this.desc = desc;
		this.nextResetTime = nextResetTime;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlFriendCampaginWeekReward>().OnQuerySuccess(isGetReward, rankNumber, maxRank, rewardInfos, desc, nextResetTime);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

//领取奖励返回协议
class FCRankGetRewardRes : Response
{
	private bool isGetReward;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public FCRankGetRewardRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public FCRankGetRewardRes(int pRqstId, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, bool isGetReward)
		: base(pRqstId)
	{
		this.isGetReward = isGetReward;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(this.costAndRewardAndSync, "FCRankGetRewardRes");
		SysUIEnv.Instance.GetUIModule<UIPnlFriendCampaginWeekReward>().OnGetRewardSuccess(isGetReward, this.costAndRewardAndSync);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		base.PrcErr(request, errCode, errMsg);
	}
}

#endregion

#region Illusion

class OnQueryIllusionRes : Response
{
	private com.kodgames.corgi.protocol.IllusionData illusionData;

	public OnQueryIllusionRes(int pRqstID, com.kodgames.corgi.protocol.IllusionData illusionData)
		: base(pRqstID)
	{
		this.illusionData = illusionData;
	}

	public OnQueryIllusionRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.IllusionData = illusionData;

		UIPnlAvatarIllusion.lastResponseTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

		//System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//foreach (var illuavatar in illusionData.illusionAvatars)
		//{
		//    sb.Append(string.Format("IllusionAvatar name={0}\n", ItemInfoUtility.GetAssetName(illuavatar.recourseId)));
		//    foreach (var illu in illuavatar.illusions)
		//        sb.Append(string.Format("\tIllusion {0} resId={1} useStatus={2} endTime=[{3}] nowTime=[{4}]\n", ItemInfoUtility.GetAssetName(illu.illusionId), illu.illusionId, ClientServerCommon.IllusionConfig._UseStatus.GetNameByType(illu.useStatus), illu.endTime, SysLocalDataBase.Inst.LoginInfo.NowTime));
		//    sb.Append("\n");
		//}

		//Debug.Log(sb.ToString());

		if ((request as QueryIllusionReq).OnQueryIllusionRes != null)
			(request as QueryIllusionReq).OnQueryIllusionRes(true);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		UIPnlAvatarIllusion.lastResponseTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

		if ((request as QueryIllusionReq).OnQueryIllusionRes != null)
			(request as QueryIllusionReq).OnQueryIllusionRes(false);

		base.PrcErr(request, errCode, errMsg);
	}
}

class OnActivateIllusionRes : Response
{
	com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar;
	CostAndRewardAndSync costAndRewardAndSync;
	public OnActivateIllusionRes(int pRqstID, com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.illusionAvatar = illusionAvatar;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnActivateIllusionRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(this.costAndRewardAndSync, "OnActivateIllusionRes");

		//UpdateData
		SysLocalDataBase.Inst.LocalPlayer.IllusionData.illusionAvatars
		[SysLocalDataBase.Inst.LocalPlayer.IllusionData.illusionAvatars.FindIndex(0, n => n.recourseId == illusionAvatar.recourseId)] = this.illusionAvatar;

		if ((request as ActivateIllusionReq).OnActivateIllusionRes != null)
			(request as ActivateIllusionReq).OnActivateIllusionRes(true);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if ((request as ActivateIllusionReq).OnActivateIllusionRes != null)
			(request as ActivateIllusionReq).OnActivateIllusionRes(false);

		base.PrcErr(request, errCode, errMsg);
	}
}

class OnActivateAndIllusionRes : Response
{
	com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar;
	CostAndRewardAndSync costAndRewardAndSync;

	public OnActivateAndIllusionRes(int pRqstID, com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.illusionAvatar = illusionAvatar;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnActivateAndIllusionRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(this.costAndRewardAndSync, "OnActivateAndIllusionRes");

		//UpdateData
		SysLocalDataBase.Inst.LocalPlayer.IllusionData.illusionAvatars
		[SysLocalDataBase.Inst.LocalPlayer.IllusionData.illusionAvatars.FindIndex(0, n => n.recourseId == illusionAvatar.recourseId)] = this.illusionAvatar;


		if ((request as ActivateAndIllusionReq).OnActivateAndIllusionRes != null)
			(request as ActivateAndIllusionReq).OnActivateAndIllusionRes(true);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if ((request as ActivateAndIllusionReq).OnActivateAndIllusionRes != null)
			(request as ActivateAndIllusionReq).OnActivateAndIllusionRes(false);

		base.PrcErr(request, errCode, errMsg);
	}
}

class OnIllusionRes : Response
{
	com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar;
	public OnIllusionRes(int pRqstID, com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar)
		: base(pRqstID)
	{
		this.illusionAvatar = illusionAvatar;
	}

	public OnIllusionRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		//UpdateData
		SysLocalDataBase.Inst.LocalPlayer.IllusionData.illusionAvatars
		[SysLocalDataBase.Inst.LocalPlayer.IllusionData.illusionAvatars.FindIndex(0, n => n.recourseId == illusionAvatar.recourseId)] = this.illusionAvatar;

		if ((request as IllusionReq).OnIllusionRes != null)
			(request as IllusionReq).OnIllusionRes(true);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if ((request as IllusionReq).OnIllusionRes != null)
			(request as IllusionReq).OnIllusionRes(false);

		base.PrcErr(request, errCode, errMsg);
	}
}

#endregion

#region 新神秘商店

//神秘商店主查询返回协议
class QueryMysteryerRes : Response
{
	private List<com.kodgames.corgi.protocol.MysteryerGood> goods;
	private List<com.kodgames.corgi.protocol.MysteryerRefresh> refresh;
	private int deleteItemId;

	public QueryMysteryerRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public QueryMysteryerRes(int pRqstId, List<com.kodgames.corgi.protocol.MysteryerGood> goods, List<com.kodgames.corgi.protocol.MysteryerRefresh> refresh, int deleteItemId)
		: base(pRqstId)
	{
		this.goods = goods;
		this.refresh = refresh;
		this.deleteItemId = deleteItemId;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlShopMystery)))
			SysUIEnv.Instance.GetUIModule<UIPnlShopMystery>().QueryAndRefreshMysteryerResSuccess(goods, refresh, deleteItemId);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlShopMystery)))
			SysUIEnv.Instance.GetUIModule<UIPnlShopMystery>().QueryRefreshMysterResFailed();
	}
}

//神秘商店刷新返回协议
class RefreshMysteryerRes : Response
{
	private List<com.kodgames.corgi.protocol.MysteryerGood> goods;                    //商品信息
	private List<com.kodgames.corgi.protocol.MysteryerRefresh> refresh;               //刷新信息
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;//消耗

	public RefreshMysteryerRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public RefreshMysteryerRes(int pRqstId, List<com.kodgames.corgi.protocol.MysteryerGood> goods, List<com.kodgames.corgi.protocol.MysteryerRefresh> refresh, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		this.goods = goods;
		this.refresh = refresh;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "RefreshMysteryerRes");

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlShopMystery)))
			SysUIEnv.Instance.GetUIModule<UIPnlShopMystery>().RefreshMysteryerResSuccess(goods, refresh);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_MYSTERYER_FAILED_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

//神秘商店购买商品协议
class BuyMysteryerRes : Response
{
	private com.kodgames.corgi.protocol.MysteryerGood goods;                    //商品信息
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;//消耗

	public BuyMysteryerRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public BuyMysteryerRes(int pRqstId, com.kodgames.corgi.protocol.MysteryerGood goods, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		this.goods = goods;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "BuyMysteryerRes");

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlShopMystery)))
			SysUIEnv.Instance.GetUIModule<UIPnlShopMystery>().BuyMysteryerReqSuccess(goods);


		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_MYSTERYER_FAILED_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class SyncMysteryerRes : Response
{
	private int count;

	public SyncMysteryerRes(int pRqstID, int count)
		: base(pRqstID)
	{
		this.count = count;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.WineSoul = count;

		return true;
	}
}

#endregion


#region Inviter
//海外兑换码主查询
class OnQueryInviteCodeInfoRes : Response
{
	private KodGames.ClientClass.Reward useCodeRewards;
	private string selfInviteCode;
	private string rewardDesc;
	private List<KodGames.ClientClass.InviteReward> inviteRewards;
	private bool useCodeRewardHasPick;
	private string codeOwnerName;
	private int rewardIconId;

	public OnQueryInviteCodeInfoRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public OnQueryInviteCodeInfoRes(int pRqstID, KodGames.ClientClass.Reward useCodeRewards, string selfInviteCode, string rewardDesc,
			List<KodGames.ClientClass.InviteReward> inviteRewards, bool useCodeRewardHasPick, string codeOwnerName, int rewardIconId)
		: base(pRqstID)
	{
		this.useCodeRewards = useCodeRewards;
		this.selfInviteCode = selfInviteCode;
		this.rewardDesc = rewardDesc;
		this.inviteRewards = inviteRewards;
		this.useCodeRewardHasPick = useCodeRewardHasPick;
		this.codeOwnerName = codeOwnerName;
		this.rewardIconId = rewardIconId;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlActivityInvite), useCodeRewards, selfInviteCode, rewardDesc, inviteRewards, useCodeRewardHasPick, codeOwnerName, rewardIconId);

		return true;
	}
}

//海外兑换码领取奖励
class OnVerifyInviteCodeAndPickRewardRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private string codeOwnername;

	public OnVerifyInviteCodeAndPickRewardRes(int pRqstID, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, string codeOwnerName)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.codeOwnername = codeOwnerName;
	}

	public OnVerifyInviteCodeAndPickRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnVerifyInviteCodeAndPickRewardRes");

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlActivityInvite)))
			SysUIEnv.Instance.GetUIModule<UIPnlActivityInvite>().OnVerifyInviteCodeAndPickRewardSuccess(costAndRewardAndSync, codeOwnername);

		return true;
	}
}

//海外兑换码Item领取奖励
class OnPickInviteCodeRewardRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public OnPickInviteCodeRewardRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnPickInviteCodeRewardRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnPickInviteCodeRewardRes");
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlActivityInvite)))
			SysUIEnv.Instance.GetUIModule<UIPnlActivityInvite>().OnPickInviteCodeRewardSuccess((request as PickInviteCodeRewardReq).RewardId);
		return true;
	}
}
#endregion

#region 711活动

//主查询
class OnQuerySevenElevenGiftRes : Response
{
	private int handredPos;
	private int decadePos;
	private int unitPos;
	private KodGames.ClientClass.SevenElevenGift sevenElevenGift;
	private bool isconverArea;
	private string areaName;
	private bool isConvert;

	public OnQuerySevenElevenGiftRes(int pRqstID, int handredPos, int decadePos, int unitPos, KodGames.ClientClass.SevenElevenGift sevenElevenGift, bool isConvertArea, string areaName, bool isConvert)
		: base(pRqstID)
	{
		this.handredPos = handredPos;
		this.decadePos = decadePos;
		this.unitPos = unitPos;
		this.sevenElevenGift = sevenElevenGift;
		this.isconverArea = isConvertArea;
		this.areaName = areaName;
		this.isConvert = isConvert;
	}

	public OnQuerySevenElevenGiftRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgConvertMain), unitPos, decadePos, handredPos, sevenElevenGift, isconverArea, areaName, isConvert);

		return true;
	}
}

//摇数
class OnTurnNumberRes : Response
{
	private int number;
	public OnTurnNumberRes(int pRqstID, int number)
		: base(pRqstID)
	{
		this.number = number;
	}

	public OnTurnNumberRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgConvertMain)))
			SysUIEnv.Instance.GetUIModule<UIDlgConvertMain>().TurnNumberReqSuccess((request as TurnNumberReq).Postion, number);

		return true;
	}
}

//领取奖励
class OnNumberconvertRes : Response
{
	private KodGames.ClientClass.SevenElevenGift sevenElevenGift;
	public OnNumberconvertRes(int pRqstID, KodGames.ClientClass.SevenElevenGift sevenElevenGift)
		: base(pRqstID)
	{
		this.sevenElevenGift = sevenElevenGift;
	}

	public OnNumberconvertRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(sevenElevenGift.CostAndRewardAndSync, "OnNumberconvertRes");

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgConvertMain)))
			SysUIEnv.Instance.GetUIModule<UIDlgConvertMain>().NumberConvertReqSuccess(sevenElevenGift);

		return true;
	}
}
#endregion

#region 东海求仙
//主查询
class EastSeaQueryZentiaRes : Response
{

	private List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges;	//兑换条目
	private KodGames.ClientClass.Cost refreshCost;		//刷新消耗
	private string refreshDesc;	//刷新描述
	private string zentiaDesc;		//东海寻仙描述
	private List<string> flowMessages;	//跑马灯信息(不带时间)

	public EastSeaQueryZentiaRes(int pRqstID, List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges
		, KodGames.ClientClass.Cost refreshCost, string refreshDesc, string zentiaDesc, List<string> flowMessages)
		: base(pRqstID)
	{
		this.zentiaExchanges = zentiaExchanges;
		this.refreshCost = refreshCost;
		this.refreshDesc = refreshDesc;
		this.zentiaDesc = zentiaDesc;
		this.flowMessages = flowMessages;
	}

	public EastSeaQueryZentiaRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastSeaFindFairyMain))
			SysUIEnv.Instance.GetUIModule<UIPnlEastSeaFindFairyMain>().OnQueryEastSeaZentiaSuccess(zentiaExchanges, refreshCost, refreshDesc, zentiaDesc, flowMessages);
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastSeaFindFairyMain))
			SysUIEnv.Instance.GetUIModule<UIPnlEastSeaCloseActivity>().OnQuerySuccess();

		var queryReq = request as EastSeaQueryZentiaReq;
		if (queryReq.QueryDel != null)
			queryReq.QueryDel(true);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ZENTIA_FAILED_AVTIVITY_CLOSED)
		{
			if (!SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlEastSeaCloseActivity)))
				SysUIEnv.Instance.ShowUIModule<UIPnlEastSeaCloseActivity>();
			SysUIEnv.Instance.GetUIModule<UIPnlEastSeaCloseActivity>().OnQuerySuccess();
		}
		else if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ZENTIA_FAILED_AVTIVITY_NOT_START ||
				errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ZENTIA_FAILED_FUNCTION_NOT_OPEN)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastSeaFindFairyMain))
				SysUIEnv.Instance.GetUIModule<UIPnlEastSeaFindFairyMain>().OnQueryEastSeaZentiaError();
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastSeaCloseActivity))
				SysUIEnv.Instance.GetUIModule<UIPnlEastSeaCloseActivity>().OnQueryEastSeaZentiaError();
		}
		else
			base.PrcErr(request, errCode, errMsg);

		var queryReq = request as EastSeaQueryZentiaReq;
		if (queryReq.QueryDel != null)
			queryReq.QueryDel(false);
	}
}

//东海寻仙玩家获得特殊道具跑马灯信息
class EastSeaQueryZentiaFlowMessageRes : Response
{

	private List<string> flowMessages;	//跑马灯信息(不带时间)

	public EastSeaQueryZentiaFlowMessageRes(int pRqstID, List<string> flowMessages)
		: base(pRqstID)
	{

		this.flowMessages = flowMessages;
	}

	public EastSeaQueryZentiaFlowMessageRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastSeaFindFairyMain))
			SysUIEnv.Instance.GetUIModule<UIPnlEastSeaFindFairyMain>().OnEastSeaQueryZentiaFlowMessageSuccess(flowMessages);
		return true;
	}
}

//兑换东海寻仙道具
class EastSeaExchangeZentiaItemRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public EastSeaExchangeZentiaItemRes(int pRqstID, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public EastSeaExchangeZentiaItemRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "EastSeaExchangeZentiaItemRes");

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastSeaFindFairyMain))
			SysUIEnv.Instance.GetUIModule<UIPnlEastSeaFindFairyMain>().OnEastSeaExchangeZentiaItemSuccess(SysLocalDataBase.ConvertIdCountList(costAndRewardAndSync.Reward));
		return true;
	}
}

//刷新东海寻仙道具兑换
class EastSeaRefreshZentiaRes : Response
{
	private List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges;
	private CostAndRewardAndSync costAndRewardAndSync;	//刷新消耗和奖励

	public EastSeaRefreshZentiaRes(int pRqstID, List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges
		, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.zentiaExchanges = zentiaExchanges;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}
	public EastSeaRefreshZentiaRes(int pRqstId, int errCode, CostAndRewardAndSync costAndRewardAndSync, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		//com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_ZENTIA_SUCCESS
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_ZENTIA_FAILED_COSTS_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "EastSeaRefreshZentiaRes");
		List<Pair<int, int>> pairs = SysLocalDataBase.ConvertIdCountList(costAndRewardAndSync.Reward);
		int count = 0;
		if (pairs != null && pairs.Count > 0)
			count = pairs[0].second;
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastSeaFindFairyMain))
			SysUIEnv.Instance.GetUIModule<UIPnlEastSeaFindFairyMain>().OnQueryEastSeaZentiaSuccess(zentiaExchanges, count);
		return true;
	}
}

//查询仙缘兑换商品
class EastSeaQueryZentiaGoodRes : Response
{
	private List<KodGames.ClientClass.ZentiaGood> zentiaGoods;	//兑换商品信息
	private bool isRankOpen;
	public EastSeaQueryZentiaGoodRes(int pRqstID, List<KodGames.ClientClass.ZentiaGood> zentiaGoods, bool isRankOpen)
		: base(pRqstID)
	{
		this.zentiaGoods = zentiaGoods;
		this.isRankOpen = isRankOpen;
	}
	public EastSeaQueryZentiaGoodRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }


	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastSeaElementItem))
			SysUIEnv.Instance.GetUIModule<UIPnlEastSeaElementItem>().OnQueryZentiaGoodSuccess(zentiaGoods, isRankOpen);
		return true;
	}
}

//仙缘兑换下的商品购买
class EastSeaBuyZentiaGoodRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	//private KodGames.ClientClass.ZentiaGood zentiaGood;	//兑换商品信息

	public EastSeaBuyZentiaGoodRes(int pRqstID, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		//this.zentiaGood = zentiaGood;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public EastSeaBuyZentiaGoodRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "EastSeaBuyZentiaGoodRes");
		if (costAndRewardAndSync.Costs != null && costAndRewardAndSync.Costs.Count > 0)
			SysLocalDataBase.Inst.LocalPlayer.Zentia -= costAndRewardAndSync.Costs[0].Count;

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), SysLocalDataBase.GetRewardDesc(costAndRewardAndSync.Reward, true, false));
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastSeaElementItem))
			SysUIEnv.Instance.GetUIModule<UIPnlEastSeaElementItem>().OnEastSeaBuyZentiaGoodRes();

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_ZENTIA_FAILED_COSTS_NOT_ENOUGH)
			GameUtility.ShowNotEnoughtAssetUI(IDSeg._SpecialId.Zentia, costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null ? costAndRewardAndSync.NotEnoughCost.Count : 0);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

//查询全服奖励
class EastSeaQueryServerZentiaRewardRes : Response
{
	private long serverZentiaPoint; //全服累计积分
	private int totalZentiaPoint; //个人累计仙缘值
	private string desc;		//全服奖励描述
	private List<KodGames.ClientClass.ZentiaServerReward> zentiaServerRewards;

	public EastSeaQueryServerZentiaRewardRes(int pRqstID, long serverZentiaPoint, int totalZentiaPoint, string desc, List<KodGames.ClientClass.ZentiaServerReward> zentiaServerRewards)
		: base(pRqstID)
	{
		this.totalZentiaPoint = totalZentiaPoint;
		this.serverZentiaPoint = serverZentiaPoint;
		this.desc = desc;
		this.zentiaServerRewards = zentiaServerRewards;
	}

	public EastSeaQueryServerZentiaRewardRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastElementAllServerReward))
			SysUIEnv.Instance.GetUIModule<UIPnlEastElementAllServerReward>().OnQueryServerZentiaRewardSuccess(totalZentiaPoint, serverZentiaPoint, zentiaServerRewards, desc);
		return true;
	}
}

//领取全服奖励
class EastSeaGetServerZentiaRewardRes : Response
{
	private long serverZentiaPoint; //全服累计积分
	private int totalZentiaPoint;  //个人累计仙缘值
	private List<KodGames.ClientClass.ZentiaServerReward> zentiaServerRewards;
	private CostAndRewardAndSync costAndRewardAndSync;

	public EastSeaGetServerZentiaRewardRes(int pRqstID, long serverZentiaPoint, int totalZentiaPoint, List<KodGames.ClientClass.ZentiaServerReward> zentiaServerRewards
		, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.totalZentiaPoint = totalZentiaPoint;
		this.serverZentiaPoint = serverZentiaPoint;
		this.zentiaServerRewards = zentiaServerRewards;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public EastSeaGetServerZentiaRewardRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "EastSeaGetServerZentiaRewardRes");
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), SysLocalDataBase.GetRewardDesc(costAndRewardAndSync.Reward, true, false));

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastElementAllServerReward))
			SysUIEnv.Instance.GetUIModule<UIPnlEastElementAllServerReward>().OnReceiveSuccess(totalZentiaPoint, serverZentiaPoint, zentiaServerRewards);
		return true;
	}
}

//排行榜查询
class EastSeaQueryZentiaRankRes : Response
{
	private long totalZentiaPoint;	//个人累计仙缘值
	private List<KodGames.ClientClass.ZentiaRank> zentiaRanks;	//排行榜信息及奖励
	private string desc;

	public EastSeaQueryZentiaRankRes(int pRqstID, long totalZentiaPoint, List<KodGames.ClientClass.ZentiaRank> zentiaRanks, string desc)
		: base(pRqstID)
	{
		this.totalZentiaPoint = totalZentiaPoint;
		this.zentiaRanks = zentiaRanks;
		this.desc = desc;
	}

	public EastSeaQueryZentiaRankRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEastElementRankingList))
			SysUIEnv.Instance.GetUIModule<UIPnlEastElementRankingList>().OnQueryZentiaRankSucces(totalZentiaPoint, zentiaRanks, desc);
		return true;
	}
}

#endregion

#region 炼丹房

class QueryAlchemyRes : Response
{
	private int todayAlchemyCount;
	private List<KodGames.ClientClass.Cost> alchemyCosts;
	private List<KodGames.ClientClass.Cost> batchAlchemyCosts;
	private List<com.kodgames.corgi.protocol.BoxReward> boxRewards;
	private com.kodgames.corgi.protocol.ShowCounter showCounter;
	private com.kodgames.corgi.protocol.AlchemyClientIcon alchemyClientIcon;
	private long nextRefreshTime;
	private com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo;

	public QueryAlchemyRes(int pRqstID, int todayAlchemyCount, List<KodGames.ClientClass.Cost> alchemyCosts, List<KodGames.ClientClass.Cost> batchAlchemyCosts, List<com.kodgames.corgi.protocol.BoxReward> boxRewards, com.kodgames.corgi.protocol.ShowCounter showCounter, com.kodgames.corgi.protocol.AlchemyClientIcon alchemyClientIcon, long nextRefreshTime, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
		: base(pRqstID)
	{
		this.todayAlchemyCount = todayAlchemyCount;
		this.alchemyCosts = alchemyCosts;
		this.batchAlchemyCosts = batchAlchemyCosts;
		this.boxRewards = boxRewards;
		this.showCounter = showCounter;
		this.alchemyClientIcon = alchemyClientIcon;
		this.nextRefreshTime = nextRefreshTime;
		this.decomposeInfo = decomposeInfo;
	}

	public QueryAlchemyRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		bool isActivity = false;
		if (ActivityManager.Instance.GetActivity<ActivityAlchemy>() != null)
			isActivity = ActivityManager.Instance.GetActivity<ActivityAlchemy>().IsOpen;

		if (!isActivity)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanFurnace)))
				SysUIEnv.Instance.GetUIModule<UIPnlDanFurnace>().OnQueryAlchemySuccess(todayAlchemyCount, boxRewards, alchemyCosts, batchAlchemyCosts, alchemyClientIcon, showCounter, nextRefreshTime, decomposeInfo);
			else
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlDanFurnace, todayAlchemyCount, boxRewards, alchemyCosts, batchAlchemyCosts, alchemyClientIcon, showCounter, nextRefreshTime, decomposeInfo);
		}
		else
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanFurnaceActivity)))
				SysUIEnv.Instance.GetUIModule<UIPnlDanFurnaceActivity>().OnQueryAlchemySuccess(todayAlchemyCount, boxRewards, alchemyCosts, batchAlchemyCosts, alchemyClientIcon, showCounter, nextRefreshTime, decomposeInfo);
			else
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlDanFurnaceActivity, todayAlchemyCount, boxRewards, alchemyCosts, batchAlchemyCosts, alchemyClientIcon, showCounter, nextRefreshTime, decomposeInfo);
		}

		return true;
	}
}

class PickAlchemyBoxRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public PickAlchemyBoxRes(int pRqstID, bool hasPicked, CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.Reward reward, KodGames.ClientClass.Reward randomReward, bool isNeedRefresh)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public PickAlchemyBoxRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "PickAlchemyCountRewardRes");

		var getRewards = SysLocalDataBase.CCRewardListToShowReward(costAndRewardAndSync.Reward);

		SysUIEnv.Instance.GetUIModule<UIDlgDanBoxRewardView>().QueryPickBoxRewardSuccess(getRewards);

		return true;
	}
}

class AlchemyRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private bool isNeedRefresh;
	private com.kodgames.corgi.protocol.DecomposeInfo demcomposeInfo;

	public AlchemyRes(int pRqstID, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.Reward reward, KodGames.ClientClass.Reward extraReward, com.kodgames.corgi.protocol.ShowCounter showCounter, int todayAlchemyCount, List<KodGames.ClientClass.Cost> alchemyCosts, List<KodGames.ClientClass.Cost> batchAlchemyCosts, bool isNeedRefresh, com.kodgames.corgi.protocol.DecomposeInfo demcomposeInfo)
		: base(pRqstID)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.isNeedRefresh = isNeedRefresh;
		this.demcomposeInfo = demcomposeInfo;
	}

	public AlchemyRes(int pRqstID, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanFurnace)))
			SysUIEnv.Instance.GetUIModule<UIPnlDanFurnace>().UpdateDecomposeInfo(demcomposeInfo);

		if (isNeedRefresh)
		{
			// Show message dlg and reconnect
			MainMenuItem okCallback = new MainMenuItem();
			okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK_Space");
			okCallback.Callback = (userData) =>
			{
				RequestMgr.Inst.Request(new QueryAlchemyReq());
				return true;
			};

			string alchemyMessage = GameUtility.GetUIString("UIDlgDanActivityInfo_Message");

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(
				GameUtility.GetUIString("UIDlgMessage_AlchemyTitle"),
				alchemyMessage,
				false,
				null,
				okCallback);

			SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
			SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
		}
		else
		{
			SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "PickAlchemyCountRewardRes");

			AlchemyReq alchemyReq = request as AlchemyReq;
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanAlchemy), costAndRewardAndSync.ViewFixReward, costAndRewardAndSync.ViewRandomReward, alchemyReq.ChatType, demcomposeInfo);
		}

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_ALCHEMY_FAILED_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class QueryDanActivityRes : Response
{
	private List<com.kodgames.corgi.protocol.DanActivityTap> danActivityTaps;

	public QueryDanActivityRes(int pRqstID, string acitvityName, List<com.kodgames.corgi.protocol.DanActivityTap> danActivityTaps, bool isNeedRefresh)
		: base(pRqstID)
	{
		this.danActivityTaps = danActivityTaps;
	}

	public QueryDanActivityRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanActivityInfo), danActivityTaps);

		return true;
	}
}

class QueryDanHomeRes : Response
{
	public QueryDanHomeRes(int pRqstID, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
		: base(pRqstID)
	{
	}

	public QueryDanHomeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanMain));
		return true;
	}
}

class OnQueryDanDecomposeRes : Response
{
	private com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo;
	private long nextRefreshTime;

	public OnQueryDanDecomposeRes(int pRqstID, string acitvityName, long activityStartTime, long activityEndTime, long nextRefreshTime, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
		: base(pRqstID)
	{
		this.decomposeInfo = decomposeInfo;
		this.nextRefreshTime = nextRefreshTime;
	}

	public OnQueryDanDecomposeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var decReq = request as QueryDanDecomposeReq;

		if (decReq.Dans != null)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanOneKeyDecompose), decReq.Dans, decomposeInfo);
		}
		else
		{

			bool isActivity = false;
			if (ActivityManager.Instance.GetActivity<ActivityDecompose>() != null)
				isActivity = ActivityManager.Instance.GetActivity<ActivityDecompose>().IsOpen;
			//decomposeInfo.danItemDecomposeCount

			if (!isActivity)
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanDecompose), decomposeInfo, nextRefreshTime);
			else
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanDecomposeActivity), decomposeInfo, nextRefreshTime);
		}

		return true;
	}
}

class OnDanDecomposeRes : Response
{
	private com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo;
	private CostAndRewardAndSync costAndRewardAndSync;


	public OnDanDecomposeRes(int pRqstID, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, bool isNeedRefresh, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
		: base(pRqstID)
	{
		this.decomposeInfo = decomposeInfo;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnDanDecomposeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnDanDecomposeRes");

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanOneKeyDecompose)) || SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanAlchemy)))
		{
			SysUIEnv.Instance.GetUIModule<UIDlgDanMaterialGet>().MaterialReward = costAndRewardAndSync;
			GameUtility.JumpUIPanel(_UIType.UIPnlDanFurnace);
		}
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanMaterialGet), SysLocalDataBase.CCRewardListToShowReward(costAndRewardAndSync.Reward));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanDecompose)))
			SysUIEnv.Instance.GetUIModule<UIPnlDanDecompose>().DecomposeSuccess(decomposeInfo);
		else if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanDecomposeActivity)))
			SysUIEnv.Instance.GetUIModule<UIPnlDanDecomposeActivity>().DecomposeSuccess(decomposeInfo);

		return true;
	}
}

class OnDanLockRes : Response
{
	public OnDanLockRes(int pRqstID, bool isNeedRefresh)
		: base(pRqstID)
	{

	}

	public OnDanLockRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var lockReq = request as LockDanReq;
		SysUIEnv.Instance.GetUIModule<UIPnlDanAttic>().ChangeLockStateSuccess(lockReq.Type, lockReq.DanPackage);

		return true;
	}
}

class OnQueryDanStoreRes : Response
{
	private List<com.kodgames.corgi.protocol.DanStoreQueryTime> danStoreQueryTimes;

	public OnQueryDanStoreRes(int pRqstID, List<com.kodgames.corgi.protocol.DanStoreQueryTime> danStoreQueryTimes)
		: base(pRqstID)
	{
		this.danStoreQueryTimes = danStoreQueryTimes;
	}

	public OnQueryDanStoreRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlDanAttic>().QueryDanStoreSuccess(danStoreQueryTimes);

		return true;
	}
}

class DanLevelUpRes : Response
{
	private Dan dan;
	private CostAndRewardAndSync costAndRewardAndSync;

	public DanLevelUpRes(int pRqstID, Dan dan, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.dan = dan;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public DanLevelUpRes(int pRqstID, int errCode, string errMsg) : base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "DanLevelUpRes");
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanCulture)))
			SysUIEnv.Instance.GetUIModule<UIPnlDanCulture>().OnDanLevelUpResSuccess(dan);

		return true;
	}
}

class DanBreakthoughtRes : Response
{
	private Dan dan;
	private CostAndRewardAndSync costAndRewardAndSync;

	public DanBreakthoughtRes(int pRqstID, Dan dan, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.dan = dan;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public DanBreakthoughtRes(int pRqstID, int errCode, string errMsg) : base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "DanBreakthoughtRes");
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanCulture)))
			SysUIEnv.Instance.GetUIModule<UIPnlDanCulture>().OnDanBreakthoughtResSuccess(dan);
		return true;
	}
}

class DanAttributeRefreshRes : Response
{
	private Dan dan;
	private CostAndRewardAndSync costAndRewardAndSync;

	public DanAttributeRefreshRes(int pRqstID, Dan dan, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstID)
	{
		this.dan = dan;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public DanAttributeRefreshRes(int pRqstID, int errCode, string errMsg) : base(pRqstID, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "DanAttributeRefreshRes");
		if (SysUIEnv.Instance.GetUIModule<UIPnlDanCulture>().IsShown)
			SysUIEnv.Instance.GetUIModule<UIPnlDanCulture>().OnDanAttributeRefreshResSuccess(dan);
		return true;
	}
}

#endregion

#region 门派

// 完成隐藏任务的通知
class OnAccomplishInvisibleTaskNotifyRes : Response
{
	private int taskId;
	private int taskStatus;

	public OnAccomplishInvisibleTaskNotifyRes(int taskId, int taskStatus)
		: base(Request.InvalidID)
	{
		this.taskId = taskId;
		this.taskStatus = taskStatus;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		Debug.Log(taskId.ToString() + taskStatus);

		return true;
	}
}

// 门派动态的通知
class OnGuildNewsNotifyRes : Response
{
	private KodGames.ClientClass.GuildNews news;

	public OnGuildNewsNotifyRes(KodGames.ClientClass.GuildNews news)
		: base(Request.InvalidID)
	{
		this.news = news;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildMessage))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildMessage>().AddOneNews(news);
		else if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo != null)
			SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.NewsLeft++;

		return true;
	}
}

// 被审核和一键拒绝的通知
class OnGuildApplyNotifyRes : Response
{
	private string guildName;
	private KodGames.ClientClass.GuildMiniInfo guildMiniInfo;

	public OnGuildApplyNotifyRes(string guildName, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
		: base(Request.InvalidID)
	{
		this.guildName = guildName;
		this.guildMiniInfo = guildMiniInfo;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (guildMiniInfo != null && guildMiniInfo.GuildId > 0)
		{
			SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo = guildMiniInfo;

			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildApplyList))
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);

			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlGuildApplyList_ApplySuccessName", guildName));
		}
		return true;
	}
}

// 被踢出门派的通知
class OnGuildKickNotifyRes : Response
{
	private string guildName;

	public OnGuildKickNotifyRes(string guildName)
		: base(Request.InvalidID)
	{
		this.guildName = guildName;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildMenuBot)))
		{
			SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildId = IDSeg.InvalidId;

			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);

			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIDlgGuildTab_KickSuccess", guildName));
		}
		else
			SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildId = IDSeg.InvalidId;

		return true;
	}
}

//门派留言的通知
class OnGuildMsgNotifyRes : Response
{
	private KodGames.ClientClass.GuildMsg guildMsg;

	public OnGuildMsgNotifyRes(KodGames.ClientClass.GuildMsg guildMsg)
		: base(Request.InvalidID)
	{
		this.guildMsg = guildMsg;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildChat))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildChat>().OnNewMsgSuccess(guildMsg);
		else if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo != null)
			SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.MsgLeft++;

		return true;
	}
}

// 门派信息查询
class GuildQueryRes : Response
{
	private KodGames.ClientClass.GuildMiniInfo guildMiniInfo;

	public GuildQueryRes(int pRqstID, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
		: base(pRqstID)
	{
		this.guildMiniInfo = guildMiniInfo;
	}

	public GuildQueryRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Set GuildMiniInfo.
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo = guildMiniInfo;

		var queryReq = request as GuildQueryReq;
		if (queryReq != null && queryReq.QuerySuccessDel != null)
			queryReq.QuerySuccessDel();

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUERY_FAILED_NOT_IN_GUILD)
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildApplyList);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

// 设置公告
class GuildSetAnnouncementRes : Response
{
	private string announcement;

	public GuildSetAnnouncementRes(int pRqstId, string announcement)
		: base(pRqstId)
	{
		this.announcement = announcement;
	}

	public GuildSetAnnouncementRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Reset Value.
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildAnnouncement = announcement;

		//Show Tips
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgGuildNotifyModify_AnnouncementSuccess"));

		// Notify UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgGuildNotifyModify)))
			SysUIEnv.Instance.GetUIModule<UIDlgGuildNotifyModify>().OnModifiyResSuccess();

		return true;
	}
}

// 创建门派
class GuildCreateRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.GuildMiniInfo guildMiniInfo;

	public GuildCreateRes(int pRqstId, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.guildMiniInfo = guildMiniInfo;
	}

	public GuildCreateRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "GuildCreate");

		// Reset Guild Data.
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo = guildMiniInfo;

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgGuildFound)))
			SysUIEnv.Instance.GetUIModule<UIDlgGuildFound>().OnResponseCreateGuild(true);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_CREATE_FAILED_CONSUMABLE_NOT_ENOUGH)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgGuildFound)))
				SysUIEnv.Instance.GetUIModule<UIDlgGuildFound>().OnResponseCreateGuild(false);

			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		}
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

// 查询门派列表
class GuildQueryGuildListRes : Response
{
	private List<KodGames.ClientClass.GuildRecord> guildRecords;
	public GuildQueryGuildListRes(int pRqstId, List<KodGames.ClientClass.GuildRecord> guildRecords)
		: base(pRqstId)
	{
		this.guildRecords = guildRecords;
	}

	public GuildQueryGuildListRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildApplyList))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildApplyList>().OnRequestMsgListSuccess(this.guildRecords, (request as GuildQueryGuildListReq).KeyWord);

		return true;
	}
}

// 申请进入门派
class GuildApplyRes : Response
{
	private KodGames.ClientClass.GuildRecord guildRecord;
	private int result;

	public GuildApplyRes(int pRqstId, int result, KodGames.ClientClass.GuildRecord guildRecord)
		: base(pRqstId)
	{
		this.guildRecord = guildRecord;
		this.result = result;
	}
	public GuildApplyRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_APPLY_SUCCESS_AUTO_ENTER)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildApplyList))
				SysUIEnv.Instance.GetUIModule<UIPnlGuildApplyList>().OnRequestApplyAutoSuccess(GameUtility.FormatUIString("UIPnlGuildApplyList_ApplySuccessName", guildRecord.GuildName));
		}
		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildApplyList))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildApplyList>().OnRequestApplySuccess(guildRecord);

		return true;
	}
}

// 快速加入门派
class GuildQuickJoinRes : Response
{
	private KodGames.ClientClass.GuildMiniInfo guildMiniInfo;
	public GuildQuickJoinRes(int pRqstId, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
		: base(pRqstId)
	{
		this.guildMiniInfo = guildMiniInfo;
	}

	public GuildQuickJoinRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (guildMiniInfo != null || guildMiniInfo.GuildId != IDSeg.InvalidId)
		{
			SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo = this.guildMiniInfo;

			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab, GameUtility.FormatUIString("UIPnlGuildApplyList_ApplySuccessName", guildMiniInfo.GuildName));
		}

		return true;
	}
}

// 查看门派简略信息
class GuildViewSimpleRes : Response
{
	private KodGames.ClientClass.GuildInfoSimple guildInfoSimple;
	public GuildViewSimpleRes(int pRqstId, KodGames.ClientClass.GuildInfoSimple guildInfoSimple)
		: base(pRqstId)
	{
		this.guildInfoSimple = guildInfoSimple;
	}

	public GuildViewSimpleRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryReq = request as GuildViewSimpleReq;
		if (queryReq != null && queryReq.QuerySuccessDel != null)
			queryReq.QuerySuccessDel(guildInfoSimple);

		return true;
	}
}

// 查看门派排行列表
class GuildQueryRankListRes : Response
{
	private List<KodGames.ClientClass.GuildRankRecord> guildRankRecords;
	public GuildQueryRankListRes(int pRqstId, List<KodGames.ClientClass.GuildRankRecord> guildRankRecords)
		: base(pRqstId)
	{
		this.guildRankRecords = guildRankRecords;
	}

	public GuildQueryRankListRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildRankList))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildRankList>().RequestQuerySuccess_GuildRank(guildRankRecords);

		return true;
	}
}

// 查看门派留言
class GuildQueryMsgRes : Response
{
	private List<KodGames.ClientClass.GuildMsg> guildMsgs;
	private int guildMsDay;
	private int guildMsgCount;

	public GuildQueryMsgRes(int pRqstId, List<KodGames.ClientClass.GuildMsg> guildMsgs, int guildMsDay, int guildMsgCount)
		: base(pRqstId)
	{
		this.guildMsgs = guildMsgs;
		this.guildMsDay = guildMsDay;
		this.guildMsgCount = guildMsgCount;
	}

	public GuildQueryMsgRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildMsgDay = guildMsDay;
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildMsgCount = guildMsgCount;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildChat))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildChat>().RequestQueryMsgSuccess(guildMsgs);

		return true;
	}
}

// 门派留言
class GuildAddMsgRes : Response
{
	private List<KodGames.ClientClass.GuildMsg> guildMsgs;
	private int guildMsDay;
	private int guildMsgCount;

	public GuildAddMsgRes(int pRqstId, List<KodGames.ClientClass.GuildMsg> guildMsgs, int guildMsDay, int guildMsgCount)
		: base(pRqstId)
	{
		this.guildMsgs = guildMsgs;
		this.guildMsDay = guildMsDay;
		this.guildMsgCount = guildMsgCount;
	}

	public GuildAddMsgRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildMsgDay = guildMsDay;
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildMsgCount = guildMsgCount;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildChat))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildChat>().RequestQueryMsgSuccess(guildMsgs);


		return true;
	}
}

// 查看门派动态
class GuildQueryNewsRes : Response
{
	private List<KodGames.ClientClass.GuildNews> guildNews;

	public GuildQueryNewsRes(int pRqstId, List<KodGames.ClientClass.GuildNews> guildNews)
		: base(pRqstId)
	{
		this.guildNews = guildNews;
	}

	public GuildQueryNewsRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildMessage))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildMessage>().RequestQueryNewsSuccess(guildNews);

		return true;
	}
}

// 修改门派宣言
class GuildSetDeclarationRes : Response
{
	private string declaration;
	public GuildSetDeclarationRes(int pRqstId, string declaration)
		: base(pRqstId)
	{
		this.declaration = declaration;
	}

	public GuildSetDeclarationRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Reset Value.
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildDeclaration = declaration;

		//Show Tips
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgGuildNotifyModify_DeclarationSuccess"));

		// Notify UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgGuildNotifyModify)))
			SysUIEnv.Instance.GetUIModule<UIDlgGuildNotifyModify>().OnModifiyResSuccess();

		return true;
	}
}

// 转让门派成员列表查询
class GuildQueryTransferMemberRes : Response
{
	private List<KodGames.ClientClass.GuildTransferMember> guildTransferMembers;

	public GuildQueryTransferMemberRes(int pRqstId, List<KodGames.ClientClass.GuildTransferMember> guildTransferMembers)
		: base(pRqstId)
	{
		this.guildTransferMembers = guildTransferMembers;
	}

	public GuildQueryTransferMemberRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgGuildAssignment)))
			SysUIEnv.Instance.GetUIModule<UIDlgGuildAssignment>().RequsetQuerySuccess(guildTransferMembers);

		return true;
	}
}

// 转让门派
class GuildTransferRes : Response
{
	private KodGames.ClientClass.GuildMiniInfo guildMiniInfo;

	public GuildTransferRes(int pRqstId, int playerId, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
		: base(pRqstId)
	{
		this.guildMiniInfo = guildMiniInfo;
	}

	public GuildTransferRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Reset GuildInfo.
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo = guildMiniInfo;

		var transReq = request as GuildTransferReq;
		if (transReq.TransSuccessDel != null)
			transReq.TransSuccessDel();

		return true;
	}
}

// 离开门派
class GuildQuitRes : Response
{
	private KodGames.ClientClass.GuildMiniInfo guildMiniInfo;

	public GuildQuitRes(int pRqstId, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
		: base(pRqstId)
	{
		this.guildMiniInfo = guildMiniInfo;
	}

	public GuildQuitRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Reset GuildMiniInfo.
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo = guildMiniInfo;

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgGuildMemberOperation_QuitGuildSuccess"));

		// TODO: Close All Panel and Show MainCity.
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildIntroMember))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlGuildIntroMember);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildIntroInfo))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlGuildIntroInfo);

		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);

		return true;
	}
}

// 查询门派成员
class GuildQueryMemberRes : Response
{
	private List<KodGames.ClientClass.GuildMemberInfo> guildMemberInfos;

	public GuildQueryMemberRes(int pRqstId, List<KodGames.ClientClass.GuildMemberInfo> guildMemberInfos)
		: base(pRqstId)
	{
		this.guildMemberInfos = guildMemberInfos;
	}

	public GuildQueryMemberRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Set GuildMembers.
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMembers = guildMemberInfos;

		var queryReq = request as GuildQueryMemberReq;
		if (queryReq != null && queryReq.QuerySuccessDel != null)
			queryReq.QuerySuccessDel();

		return true;
	}
}

// 查询申请列表
class GuildQueryApplyListRes : Response
{
	List<KodGames.ClientClass.GuildApplyInfo> guildApplyInfos;

	public GuildQueryApplyListRes(int pRqstId, List<KodGames.ClientClass.GuildApplyInfo> guildApplyInfos)
		: base(pRqstId)
	{
		this.guildApplyInfos = guildApplyInfos;
	}

	public GuildQueryApplyListRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var queryReq = request as GuildQueryApplyListReq;
		if (queryReq.QuerySuccessDel != null)
			queryReq.QuerySuccessDel(guildApplyInfos);

		return true;
	}

}

// 审核申请
class GuildReviewApplyRes : Response
{
	private int playerId;

	public GuildReviewApplyRes(int pRqstId, bool refuse, int playerId, int playerName)
		: base(pRqstId)
	{
		this.playerId = playerId;
	}

	public GuildReviewApplyRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var reviewReq = request as GuildReviewApplyReq;
		if (reviewReq.ReviewSuccessDel != null)
		{
			var lists = new List<int>();
			lists.Add(playerId);
			reviewReq.ReviewSuccessDel(lists);
		}

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildIntroReview))
			RequestMgr.Inst.Request(new GuildQueryApplyListReq(SysUIEnv.Instance.GetUIModule<UIPnlGuildIntroReview>().InitView));

		base.PrcErr(request, errCode, errMsg);
	}
}

// 一键拒绝
class GuildOneKeyRefuseRes : Response
{
	private List<int> playerIds;

	public GuildOneKeyRefuseRes(int pRqstId, List<int> playerIds)
		: base(pRqstId)
	{
		this.playerIds = playerIds;
	}

	public GuildOneKeyRefuseRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var reviewReq = request as GuildOneKeyRefuseReq;
		if (reviewReq.ReviewSuccessDel != null)
			reviewReq.ReviewSuccessDel(playerIds);

		return true;
	}
}

// 踢出门派
class GuildKickPlayerRes : Response
{
	public GuildKickPlayerRes(int pRqstId) : base(pRqstId) { }

	public GuildKickPlayerRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		var kickReq = request as GuildKickPlayerReq;

		// Reset Local Data.
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.RemoveGuildMember(kickReq.PlayerId);

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildIntroReview_ReviewOneMember"));

		if (kickReq.KickSuccessDel != null)
			kickReq.KickSuccessDel();

		return true;
	}
}

// 变更职位
class GuildSetPlayerRoleRes : Response
{
	private int playerId;
	private int roleId;

	public GuildSetPlayerRoleRes(int pRqstId, int playerId, int roleId)
		: base(pRqstId)
	{
		this.playerId = playerId;
		this.roleId = roleId;
	}

	public GuildSetPlayerRoleRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Set LocalData if exist.
		var memberInfo = SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GetGuildMemberByPlayerId(playerId);
		if (memberInfo != null)
			memberInfo.RoleId = roleId;

		var setRoleReq = request as GuildSetPlayerRoleReq;
		if (setRoleReq.SetRoleSuccessDel != null)
			setRoleReq.SetRoleSuccessDel();

		return true;
	}
}

// 更改自动批准
class GuildSetAutoEnterRes : Response
{
	private bool allowAutoEnter;
	public GuildSetAutoEnterRes(int pRqstId, bool allowAutoEnter)
		: base(pRqstId)
	{
		this.allowAutoEnter = allowAutoEnter;
	}

	public GuildSetAutoEnterRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Reset Data.
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildAllowAutoEnter = allowAutoEnter;

		if (allowAutoEnter)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildIntroInfo_AllowAutoEnter"));
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildIntroInfo_NoAllowAutoEnter"));

		var setAutoReq = request as GuildSetAutoEnterReq;
		if (setAutoReq != null && setAutoReq.QuerySuccessDel != null)
			setAutoReq.QuerySuccessDel();

		return true;
	}
}

// 门派建设请求
class QueryConstructionTaskRes : Response
{
	private KodGames.ClientClass.ConstructionInfo constructionInfo;
	public QueryConstructionTaskRes(int pRqstId, KodGames.ClientClass.ConstructionInfo constructionInfo)
		: base(pRqstId)
	{
		this.constructionInfo = constructionInfo;
	}

	public QueryConstructionTaskRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		KodGames.ClientClass.GuildMiniInfo miniInfo = SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo;
		SysLocalDataBase.Inst.LocalPlayer.GuildMoney = constructionInfo.GuildMoney;

		miniInfo.GuildLevel = constructionInfo.GuildLevel;
		miniInfo.GuildConstruct = constructionInfo.GuildConstruction;
		miniInfo.TotalContribution = constructionInfo.Contribution;

		// Refresh View.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildConstruct)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().IsOverlayed == false)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().RefreshConstructInfo(constructionInfo);

		return true;
	}
}

// 接受一个门派建设
class AcceptConstructionTaskRes : Response
{
	private KodGames.ClientClass.ConstructionInfo constructionInfo;

	public AcceptConstructionTaskRes(int pRqstId, KodGames.ClientClass.ConstructionInfo constructionInfo)
		: base(pRqstId)
	{
		this.constructionInfo = constructionInfo;
	}

	public AcceptConstructionTaskRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.ConstructionInfo = constructionInfo;

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildConstruct_TaskAccepSuccess"));

		// Refresh View.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildConstruct)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().IsOverlayed == false)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().RefreshConstructInfo(constructionInfo);

		return true;
	}
}

// 放弃一个门派建设
class GiveUpConstructionTaskRes : Response
{
	private KodGames.ClientClass.ConstructionInfo constructionInfo;

	public GiveUpConstructionTaskRes(int pRqstId, KodGames.ClientClass.ConstructionInfo constructionInfo)
		: base(pRqstId)
	{
		this.constructionInfo = constructionInfo;
	}

	public GiveUpConstructionTaskRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.ConstructionInfo = constructionInfo;

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildConstruct_TaskGiveUpSuccess"));

		// Refresh View.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildConstruct)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().IsOverlayed == false)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().RefreshConstructInfo(constructionInfo);

		return true;
	}
}

// 完成一个门派建设
class CompleteConstructionTaskRes : Response
{
	private KodGames.ClientClass.ConstructionInfo constructionInfo;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public CompleteConstructionTaskRes(int pRqstId, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.ConstructionInfo constructionInfo)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.constructionInfo = constructionInfo;
	}

	public CompleteConstructionTaskRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "CompleteConstructionTask");

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildConstruct_TaskSuccess"));

		KodGames.ClientClass.GuildMiniInfo miniInfo = SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo;
		SysLocalDataBase.Inst.LocalPlayer.GuildMoney = constructionInfo.GuildMoney;

		miniInfo.GuildLevel = constructionInfo.GuildLevel;
		miniInfo.GuildConstruct = constructionInfo.GuildConstruction;
		miniInfo.TotalContribution = constructionInfo.Contribution;

		// Refresh View.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildConstruct)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().IsOverlayed == false)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().OnCompleteConstructTask(constructionInfo, costAndRewardAndSync.Reward);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_COMPLETE_CONSTRUCTION_TASK_FAILED_NOT_ENOUGH_COST)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

// 手动刷新
class RefreshConstructionTaskRes : Response
{
	private KodGames.ClientClass.ConstructionInfo constructionInfo;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public RefreshConstructionTaskRes(int pRqstId, KodGames.ClientClass.ConstructionInfo constructionInfo, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.constructionInfo = constructionInfo;
	}

	public RefreshConstructionTaskRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync) :
		base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "RefreshConstructionTask");

		// Refresh View.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildConstruct)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().IsOverlayed == false)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildConstruct>().RefreshConstructInfo(constructionInfo);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_CONSTRUCTION_TASK_FAILED_NOT_ENOUGH_COST)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

// 门派公共商品查询
class QueryGuildPublicShopRes : Response
{
	private List<KodGames.ClientClass.GuildPublicGoods> guildPublicGoods;
	private long nextRefreshTime;

	public QueryGuildPublicShopRes(int pRqstId, List<KodGames.ClientClass.GuildPublicGoods> guildPublicGoods, long nextRefreshTime)
		: base(pRqstId)
	{
		this.guildPublicGoods = guildPublicGoods;
		this.nextRefreshTime = nextRefreshTime;
	}

	public QueryGuildPublicShopRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPublicShop)))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPublicShop>().OnQueryShopListSuccess(guildPublicGoods, nextRefreshTime);

		return true;
	}
}

// 门派公共商品购买
class BuyGuildPublicGoodsRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.GuildPublicGoods guildPublicGoods;

	public BuyGuildPublicGoodsRes(int pRqstId, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.GuildPublicGoods guildPublicGoods)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.guildPublicGoods = guildPublicGoods;
	}

	public BuyGuildPublicGoodsRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "BuyGuildPublicGoods");

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPublicShop)) && !SysUIEnv.Instance.GetUIModule<UIPnlGuildPublicShop>().IsOverlayed)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPublicShop>().OnBuyGoodsFinish(true, guildPublicGoods, costAndRewardAndSync.Reward);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GUILD_PUBLIC_GOODS_FAILED_COST_NOT_ENOUGH)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GUILD_PUBLIC_GOODS_FAILED_GOODS_NOT_ENOUGH)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPublicShop)))
				SysUIEnv.Instance.GetUIModule<UIPnlGuildPublicShop>().OnBuyGoodsFinish(false, guildPublicGoods, null);
		}
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

// 门派玩家商品查询
class QueryGuildPrivateShopRes : Response
{
	private List<KodGames.ClientClass.GuildPrivateGoods> guildPrivateGoods;

	public QueryGuildPrivateShopRes(int pRqstId, List<KodGames.ClientClass.GuildPrivateGoods> guildPrivateGoods)
		: base(pRqstId)
	{
		this.guildPrivateGoods = guildPrivateGoods;
	}

	public QueryGuildPrivateShopRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPrivateShop)))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPrivateShop>().OnQueryShopListSuccess(guildPrivateGoods);

		return true;
	}
}

// 门派玩家商品购买
class BuyGuildPrivateGoodsRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.GuildPrivateGoods goods;

	public BuyGuildPrivateGoodsRes(int pRqstId, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.GuildPrivateGoods goods)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.goods = goods;
	}

	public BuyGuildPrivateGoodsRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "BuyGuildPrivateGoods");

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPrivateShop)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildPrivateShop>().IsOverlayed == false)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPrivateShop>().OnBuyGoodsFinish(true, goods, costAndRewardAndSync.Reward);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GUILD_PRIVATE_GOODS_FAILED_COST_NOT_ENOUGH)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GUILD_PRIVATE_GOODS_FAILED_GOODS_NOT_ENOUGH)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPrivateShop)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildPrivateShop>().IsOverlayed == false)
				SysUIEnv.Instance.GetUIModule<UIPnlGuildPrivateShop>().OnBuyGoodsFinish(false, goods, null);
		}
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

// 门派活动商品查询
class QueryGuildExchangeShopRes : Response
{
	private List<KodGames.ClientClass.GuildExchangeGoods> guildExchangeGoods;

	public QueryGuildExchangeShopRes(int pRqstId, List<KodGames.ClientClass.GuildExchangeGoods> guildExchangeGoods)
		: base(pRqstId)
	{
		this.guildExchangeGoods = guildExchangeGoods;
	}

	public QueryGuildExchangeShopRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildShopActivity)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildShopActivity>().IsOverlayed == false)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildShopActivity>().OnQueryExchangeListSuccess(guildExchangeGoods);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildShopActivity)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildShopActivity>().IsOverlayed == false)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildShopActivity>().OnQueryExchangeListFail();

		base.PrcErr(request, errCode, errMsg);
	}
}

// 门派活动商品兑换
class ExchangeGuildExchangeGoodsRes : Response
{
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.GuildExchangeGoods goods;

	public ExchangeGuildExchangeGoodsRes(int pRqstId, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.GuildExchangeGoods goods)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.goods = goods;
	}

	public ExchangeGuildExchangeGoodsRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Process Cost And Reward.
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "ExchangeGuildExchangeGoods");

		// Refresh UI.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildShopActivity)) && SysUIEnv.Instance.GetUIModule<UIPnlGuildShopActivity>().IsOverlayed == false)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildShopActivity>().OnExchangeSuccess(goods, costAndRewardAndSync.Reward);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GUILD_PRIVATE_GOODS_FAILED_COST_NOT_ENOUGH)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

// 门派任务查询
class QueryGuildTaskRes : Response
{
	private KodGames.ClientClass.GuildTaskInfo guildTaskInfo;

	public QueryGuildTaskRes(int pRqstId, KodGames.ClientClass.GuildTaskInfo guildTaskInfo)
		: base(pRqstId)
	{
		this.guildTaskInfo = guildTaskInfo;
	}

	public QueryGuildTaskRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildTask))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildTask>().RequestQueryTaskSuccess(guildTaskInfo);

		return true;
	}
}

// 门派任务投掷
class GuildTaskDiceRes : Response
{
	private List<int> diceResults;
	private KodGames.ClientClass.GuildTaskInfo guildTaskInfo;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public GuildTaskDiceRes(int pRqstId, List<int> diceResults, KodGames.ClientClass.GuildTaskInfo guildTaskInfo, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		this.diceResults = diceResults;
		this.guildTaskInfo = guildTaskInfo;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public GuildTaskDiceRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "GuildTaskDice");

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildTask))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildTask>().RequestDiceSuccess(guildTaskInfo, diceResults);

		return true;
	}
}

// 门派任务刷新
class RefreshGuildTaskRes : Response
{
	private KodGames.ClientClass.GuildTaskInfo guildTaskInfo;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public RefreshGuildTaskRes(int pRqstId, KodGames.ClientClass.GuildTaskInfo guildTaskInfo, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		this.guildTaskInfo = guildTaskInfo;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public RefreshGuildTaskRes(int pRqstId, int errCode, string errMsg) : base(pRqstId, errCode, errMsg) { }

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "RefreshGuildTask");

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildTask))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildTask>().RequestQueryTaskSuccess(guildTaskInfo);

		return true;
	}
}

//开启门派关卡
class OpenGuildStageRes : Response
{
	public OpenGuildStageRes(int pRqstId)
		: base(pRqstId)
	{
	}

	public OpenGuildStageRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		RequestMgr.Inst.Request(new QueryGuildStageReq(GuildStageConfig._LayerType.Now));

		return true;
	}
}

//门派关卡主查询
class QueryGuildStageRes : Response
{
	private List<com.kodgames.corgi.protocol.Stage> stages;
	private int explorePoint;
	private int freeChallengeCount;
	private int itemChallengeCount;
	private int needPassBossCount;
	private KodGames.ClientClass.Cost costs;
	private int mapNum;
	private int index;
	private string preName;
	private string roadPreName;
	private bool isStageOpen;
	private long handResetTime;
	private int handResetStatus;
	private int needGuildLevel;
	private bool isLastMap;
	private int guildLevel;

	public QueryGuildStageRes(int pRqstId, List<com.kodgames.corgi.protocol.Stage> stages, int explorePoint, int freeChallengeCount, int itemChallengeCount, int needPassBossCount, KodGames.ClientClass.Cost costs, int mapNum, int index, string preName, string roadPreName, bool isStageOpen, long handResetTime, int handResetStatus, int needGuildLevel, bool isLastMap, int guildLevel)
		: base(pRqstId)
	{
		this.stages = stages;
		this.explorePoint = explorePoint;
		this.freeChallengeCount = freeChallengeCount;
		this.itemChallengeCount = itemChallengeCount;
		this.needPassBossCount = needPassBossCount;
		this.costs = costs;
		this.mapNum = mapNum;
		this.index = index;
		this.preName = preName;
		this.roadPreName = roadPreName;
		this.isStageOpen = isStageOpen;
		this.handResetTime = handResetTime;
		this.handResetStatus = handResetStatus;
		this.needGuildLevel = needGuildLevel;
		this.isLastMap = isLastMap;
		this.guildLevel = guildLevel;
	}

	public QueryGuildStageRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (isStageOpen)
		{
			List<object> paramsters = new List<object>();
			paramsters.Add(stages);
			paramsters.Add(explorePoint);
			paramsters.Add(freeChallengeCount);
			paramsters.Add(itemChallengeCount);
			paramsters.Add(needPassBossCount);
			paramsters.Add(costs);
			paramsters.Add(mapNum);
			paramsters.Add(index);
			paramsters.Add(preName);
			paramsters.Add(roadPreName);
			paramsters.Add(handResetTime);
			paramsters.Add(handResetStatus);
			paramsters.Add(needGuildLevel);
			paramsters.Add(isLastMap);
			paramsters.Add(guildLevel);

			SysGameStateMachine.Instance.EnterState<GameState_GuildPoint>(paramsters, true);
		}
		else
		{
			var guildRoleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.RoleId);

			if (guildRoleCfg.Rights.Contains(GuildConfig._RoleRightType.HandResetStage))
				RequestMgr.Inst.Request(new OpenGuildStageReq());
			else
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildPointMain_NoRole"));
		}

		return true;
	}
}

//移动并探索
class GuildStageExploreRes : Response
{
	private KodGames.ClientClass.StageInfo stageInfo;
	private KodGames.ClientClass.CombatResultAndReward combatResultAndReward;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private int operateType;

	public GuildStageExploreRes(int pRqstId, KodGames.ClientClass.StageInfo stageInfo, KodGames.ClientClass.CombatResultAndReward combatResultAndReward, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, int operateType)
		: base(pRqstId)
	{
		this.stageInfo = stageInfo;
		this.combatResultAndReward = combatResultAndReward;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.operateType = operateType;
	}

	public GuildStageExploreRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		bool isCost = true;
		if (costAndRewardAndSync.Costs == null || costAndRewardAndSync.Costs.Count == 0)
			isCost = false;
		else if (costAndRewardAndSync.Costs.Count > 0)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), string.Format(GameUtility.GetUIString("UIPnlGuildPointMain_ExploreStageCost"), costAndRewardAndSync.Costs[0].Count));

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "GuildStageExploreRes");

		GuildSceneData.Instance.QueryStageExploreSuccess(stageInfo, combatResultAndReward, operateType, isCost);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_EXPLORE_FAILED_ERROR_MAPNUM ||
			errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_TALENT_RESET_FAILED_NOT_OPEN)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().StageConnect();
		else if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_EXPLORE_FAILED_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

//挑战boss
class GuildStageCombatBossRes : Response
{
	private KodGames.ClientClass.CombatResultAndReward combatResultAndReward;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;
	private com.kodgames.corgi.protocol.Rank myRank;
	private com.kodgames.corgi.protocol.BossRank bossRank;
	private List<com.kodgames.corgi.protocol.ShowReward> commonRewards;
	private List<com.kodgames.corgi.protocol.ShowReward> extraRewards;
	private bool hasActivateGoods;
	private com.kodgames.corgi.protocol.Rank thisData;

	public GuildStageCombatBossRes(int pRqstId, KodGames.ClientClass.CombatResultAndReward combatResultAndReward, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, com.kodgames.corgi.protocol.Rank myRank, com.kodgames.corgi.protocol.BossRank bossRank, List<com.kodgames.corgi.protocol.ShowReward> commonRewards, List<com.kodgames.corgi.protocol.ShowReward> extraRewards, bool hasActivateGoods, com.kodgames.corgi.protocol.Rank thisData)
		: base(pRqstId)
	{
		this.combatResultAndReward = combatResultAndReward;
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.myRank = myRank;
		this.bossRank = bossRank;
		this.commonRewards = commonRewards;
		this.extraRewards = extraRewards;
		this.hasActivateGoods = hasActivateGoods;
		this.thisData = thisData;
	}

	public GuildStageCombatBossRes(int pRqstId, int errCode, string errMsg, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "GuildStageCombatBossRes");

		UIPnlGuildPointBossBattleResult.GuildBossBattleResultData bossBattleData = new UIPnlGuildPointBossBattleResult.GuildBossBattleResultData(combatResultAndReward, costAndRewardAndSync, myRank, bossRank, commonRewards, extraRewards, hasActivateGoods, thisData);

		// Go to combat state
		List<object> paramsters = new List<object>();
		paramsters.Add(combatResultAndReward);
		paramsters.Add(bossBattleData);

		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Battle>(paramsters);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_COMBAT_BOSS_FAILED_ERROR_MAP_NUM ||
			errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_TALENT_RESET_FAILED_NOT_OPEN)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().StageConnect();
		else if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_COMBAT_BOSS_FAILED_COST_NOT_ENOUGH && costAndRewardAndSync != null && costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

//宝箱赠送
class GuildStageGiveBoxRes : Response
{

	public GuildStageGiveBoxRes(int pRqstId)
		: base(pRqstId)
	{
	}

	public GuildStageGiveBoxRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		GuildStageGiveBoxReq req = request as GuildStageGiveBoxReq;
		if (req.QuerySuccess != null)
			req.QuerySuccess();
		return true;
	}
}

//手动重置协议
class GuildStageResetRes : Response
{
	public GuildStageResetRes(int pRqstId)
		: base(pRqstId)
	{
	}

	public GuildStageResetRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlGuildTab));

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_EXPLORE_FAILED_ERROR_MAPNUM)
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().StageConnect();
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

//查询门派关卡奖励消息
class GuildStageQueryMsgRes : Response
{
	private List<com.kodgames.corgi.protocol.GuildStageMsg> msgs;

	public GuildStageQueryMsgRes(int pRqstId, List<com.kodgames.corgi.protocol.GuildStageMsg> msgs)
		: base(pRqstId)
	{
		this.msgs = msgs;
	}

	public GuildStageQueryMsgRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPointPerson)))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointPerson>().OnGuildStageQueryMsgResSuccess(msgs);
		return true;
	}
}

//查询门派boss伤害排行
class GuildStageQueryBossRankRes : Response
{
	private List<com.kodgames.corgi.protocol.BossRank> bossRanks;

	public GuildStageQueryBossRankRes(int pRqstId, List<com.kodgames.corgi.protocol.BossRank> bossRanks)
		: base(pRqstId)
	{
		this.bossRanks = bossRanks;
	}

	public GuildStageQueryBossRankRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPointBossRank)))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointBossRank>().OnGuildStageQueryBossRankResSuccess(bossRanks);
		return true;
	}
}

//查询门派boss伤害排行
class GuildStageQueryBossRankDetailRes : Response
{
	private com.kodgames.corgi.protocol.BossRank bossRank;
	private com.kodgames.corgi.protocol.Rank rank;

	public GuildStageQueryBossRankDetailRes(int pRqstId, com.kodgames.corgi.protocol.Rank rank, com.kodgames.corgi.protocol.BossRank bossRank)
		: base(pRqstId)
	{
		this.bossRank = bossRank;
		this.rank = rank;
	}

	public GuildStageQueryBossRankDetailRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildPointDamageRank))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointDamageRank>().OnGuildStageQueryBossRankDetailSuccess(rank, bossRank);
		return true;
	}
}


//查询门派探索排行
class GuildStageQueryExploreRankRes : Response
{
	private com.kodgames.corgi.protocol.Rank myRank;
	private List<com.kodgames.corgi.protocol.Rank> ranks;

	public GuildStageQueryExploreRankRes(int pRqstId, com.kodgames.corgi.protocol.Rank myRank, List<com.kodgames.corgi.protocol.Rank> ranks)
		: base(pRqstId)
	{
		this.myRank = myRank;
		this.ranks = ranks;
	}

	public GuildStageQueryExploreRankRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPointExplorationRank)))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointExplorationRank>().OnGuildStageQueryExploreRankResSuccess(myRank, ranks);

		return true;
	}
}

//查询门派间排行
class GuildStageQueryRankRes : Response
{
	private com.kodgames.corgi.protocol.Rank myRank;
	private List<com.kodgames.corgi.protocol.Rank> ranks;

	public GuildStageQueryRankRes(int pRqstId, com.kodgames.corgi.protocol.Rank myRank, List<com.kodgames.corgi.protocol.Rank> ranks)
		: base(pRqstId)
	{
		this.myRank = myRank;
		this.ranks = ranks;
	}

	public GuildStageQueryRankRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		GuildStageQueryRankReq req = request as GuildStageQueryRankReq;
		if (req.QuerySuccess != null)
			req.QuerySuccess(myRank, ranks);

		return true;
	}
}

//查询天赋信息
class GuildStageQueryTalentRes : Response
{
	private int talentPoint;
	private List<com.kodgames.corgi.protocol.BossTalent> bossTalents;
	private int alreadyResetTimes;

	public GuildStageQueryTalentRes(int pRqstId, int talentPoint, List<com.kodgames.corgi.protocol.BossTalent> bossTalents, int alreadyResetTimes)
		: base(pRqstId)
	{
		this.talentPoint = talentPoint;
		this.bossTalents = bossTalents;
		this.alreadyResetTimes = alreadyResetTimes;
	}

	public GuildStageQueryTalentRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPointTalentTab)))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointTalentTab>().QueryTalentInfoSuccess(talentPoint, bossTalents, alreadyResetTimes);

		return true;
	}
}

//重置天赋
class GuildStageTalentResetRes : Response
{
	public GuildStageTalentResetRes(int pRqstId)
		: base(pRqstId)
	{
	}

	public GuildStageTalentResetRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPointTalentTab)))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointTalentTab>().SendQueryTalentReq();

		return true;
	}
}

//天赋加点
class GuildStageTalentAddRes : Response
{
	public GuildStageTalentAddRes(int pRqstId)
		: base(pRqstId)
	{
	}

	public GuildStageTalentAddRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPointTalentTab)))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildPointTalentTab>().SendQueryTalentReq();

		return true;
	}
}
#endregion

#region FaceBook
//天赋加点
class QueryFacebookRes : Response
{
	private bool isShowIcon = false;

	public QueryFacebookRes(int pRqstId, bool isShowIcon)
		: base(pRqstId)
	{
		this.isShowIcon = isShowIcon;
	}

	public QueryFacebookRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlActivityFaceBook)))
			SysUIEnv.Instance.GetUIModule<UIPnlActivityFaceBook>().SetFacebookSuccessBoxHide(!isShowIcon);

		return true;
	}
}

class FacebookRewardRes : Response
{
	public FacebookRewardRes(int pRqstId)
		: base(pRqstId)
	{
	}

	public FacebookRewardRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		//Application.OpenURL(GameUtility.GetUIString("UIPnlCentralCityPlayerInfo_WebPageUrl"));
		return true;
	}
}
#endregion

#region 修改玩家名称

class OnSetPlayerNameRes : Response
{
	private string playerName;
	private CostAndRewardAndSync costAndRewardAndSync;

	public OnSetPlayerNameRes(int pRqstId, string playerName, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		Debug.Log("playerName = " + playerName);
		this.playerName = playerName;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnSetPlayerNameRes(int pRqstId, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnSetPlayerNameRes");

		SysLocalDataBase.Inst.LocalPlayer.Name = playerName;
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIDlgSetName_Success", playerName));

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_PLAYERNAME_FAILED_PLAYERNAME_COST_NOT_ENOUGH &&
								costAndRewardAndSync != null &&
								costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

class OnSetGuildNameRes : Response
{
	private string guildName;
	private CostAndRewardAndSync costAndRewardAndSync;

	public OnSetGuildNameRes(int pRqstId, string guildName, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		Debug.Log("guildName = " + guildName);
		this.guildName = guildName;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnSetGuildNameRes(int pRqstId, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnSetGuildNameRes");
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildName = guildName;
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildIntroInfo)))
			SysUIEnv.Instance.GetUIModule<UIPnlGuildIntroInfo>().SetInitView();

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIDlgSetName_Success", guildName));

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_GUILDNAME_FAILED_GUILDNAME_COST_NOT_ENOUGH &&
								costAndRewardAndSync != null &&
								costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}

#endregion

#region 机关兽

class OnActiveBeastRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public OnActiveBeastRes(int pRqstId, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnActiveBeastRes(int pRqstId, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnActiveBeastRes");
		SysUIEnv.Instance.GetUIModule<UIPnlOrgansBeastTab>().OnActiveBeastSuccess();

		return true;
	}
}

class OnEquipBeastPartRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.Beast beast;

	public OnEquipBeastPartRes(int pRqstId, CostAndRewardAndSync costAndRewardAndSync, Beast beast)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.beast = beast;
	}

	public OnEquipBeastPartRes(int pRqstId, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		EquipBeastPartReq req = request as EquipBeastPartReq;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnEquipBeastPartRes");

		KodGames.ClientClass.Beast oldBeast = SysLocalDataBase.Inst.LocalPlayer.SearchBeast(beast.Guid);

		oldBeast.PartIndexs = beast.PartIndexs;
		oldBeast.LevelAttrib = beast.LevelAttrib;
		oldBeast.BreakthoughtLevel = beast.BreakthoughtLevel;

		SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().OnQueryBeastSuccess();
		//SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().OnQueryBeastLevelUpSuccess(req.PartType);

		if (req.PartType != BeastConfig._PartIndex.AllPut)
		{
			SysUIEnv.Instance.GetUIModule<UIDlgOrganChipSplit>().OnEquipSplitSuccess(beast, req.PartType);
		}

		return true;
	}

	//protected override void PrcErr(Request request, int errCode, string errMsg)
	//{
	//    if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_PLAYERNAME_FAILED_PLAYERNAME_COST_NOT_ENOUGH &&
	//                            costAndRewardAndSync != null &&
	//                            costAndRewardAndSync.NotEnoughCost != null)
	//        GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
	//    else
	//        base.PrcErr(request, errCode, errMsg);
	//}
}

class OnBeastLevelUpRes : Response
{
	private KodGames.ClientClass.Beast beast;

	public OnBeastLevelUpRes(int pRqstId, Beast beast)
		: base(pRqstId)
	{
		this.beast = beast;
	}

	public OnBeastLevelUpRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{

	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		//BeastLevelUpReq levelReq = request as BeastLevelUpReq;

		//SysUIEnv.Instance.ShowUIModule(typeof(UIDlgOrganUpShow), levelReq.OldBeast, beast);

		KodGames.ClientClass.Beast oldBeast = SysLocalDataBase.Inst.LocalPlayer.SearchBeast(beast.Guid);

		oldBeast.PartIndexs = beast.PartIndexs;
		oldBeast.LevelAttrib = beast.LevelAttrib;
		oldBeast.BreakthoughtLevel = beast.BreakthoughtLevel;

		SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().OnQueryBeastLevelUpSuccess();

		//SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().OnQueryBeastSuccess();
		return true;
	}
}

class OnBeastBreakthoughtRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;
	private KodGames.ClientClass.Beast beast;

	public OnBeastBreakthoughtRes(int pRqstId, CostAndRewardAndSync costAndRewardAndSync, Beast beast)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
		this.beast = beast;
	}

	public OnBeastBreakthoughtRes(int pRqstId, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnBeastStarUpRes");

		//BeastBreakthoughtReq breakReq = request as BeastBreakthoughtReq;
		//SysUIEnv.Instance.ShowUIModule(typeof(UIDlgOrganUpShow), breakReq.OldBeast, beast);

		KodGames.ClientClass.Beast oldBeast = SysLocalDataBase.Inst.LocalPlayer.SearchBeast(beast.Guid);
		oldBeast.LevelAttrib = beast.LevelAttrib;
		oldBeast.BreakthoughtLevel = beast.BreakthoughtLevel;

		//SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().OnQueryBeastSuccess();
		SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().OnQueryBeastStartUpSuccess();

		return true;
	}

	//protected override void PrcErr(Request request, int errCode, string errMsg)
	//{
	//    if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_PLAYERNAME_FAILED_PLAYERNAME_COST_NOT_ENOUGH &&
	//                            costAndRewardAndSync != null &&
	//                            costAndRewardAndSync.NotEnoughCost != null)
	//        GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
	//    else
	//        base.PrcErr(request, errCode, errMsg);
	//}
}


class OnQueryBeastExchangeShopRes : Response
{
	private List<ZentiaExchange> beastExchanges;
	private KodGames.ClientClass.Cost refreshCost;
	private long nextFreeStartTime;

	public OnQueryBeastExchangeShopRes(int pRqstId, List<ZentiaExchange> beastExchanges, KodGames.ClientClass.Cost refreshCost, long nextFreeStartTime)
		: base(pRqstId)
	{
		this.beastExchanges = beastExchanges;
		this.refreshCost = refreshCost;
		this.nextFreeStartTime = nextFreeStartTime;
	}

	public OnQueryBeastExchangeShopRes(int pRqstId, int errCode, string errMsg)
		: base(pRqstId, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlOrgansShopTab))
			SysUIEnv.Instance.GetUIModule<UIPnlOrgansShopTab>().OnQueryBeastExchangeShopSuccess(beastExchanges, refreshCost, nextFreeStartTime);

		return true;
	}
}

class OnBeastExchangeShopRes : Response
{
	private CostAndRewardAndSync costAndRewardAndSync;

	public OnBeastExchangeShopRes(int pRqstId, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnBeastExchangeShopRes(int pRqstId, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnBeastExchangeShopRes");

		var queryReq = request as BeastExchangeShopReq;
		if (queryReq.QueryDel != null)
			queryReq.QueryDel(costAndRewardAndSync.Reward);

		return true;
	}

	//protected override void PrcErr(Request request, int errCode, string errMsg)
	//{
	//    if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_PLAYERNAME_FAILED_PLAYERNAME_COST_NOT_ENOUGH &&
	//                            costAndRewardAndSync != null &&
	//                            costAndRewardAndSync.NotEnoughCost != null)
	//        GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
	//    else
	//        base.PrcErr(request, errCode, errMsg);
	//}
}

class OnRefreshBeastExchangeShopRes : Response
{
	private List<ZentiaExchange> beastExchanges;
	private CostAndRewardAndSync costAndRewardAndSync;

	public OnRefreshBeastExchangeShopRes(int pRqstId, List<ZentiaExchange> beastExchanges, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId)
	{
		this.beastExchanges = beastExchanges;
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public OnRefreshBeastExchangeShopRes(int pRqstId, int errCode, string errMsg, CostAndRewardAndSync costAndRewardAndSync)
		: base(pRqstId, errCode, errMsg)
	{
		this.costAndRewardAndSync = costAndRewardAndSync;
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;
		SysLocalDataBase.Inst.ProcessCostRewardSync(costAndRewardAndSync, "OnRefreshBeastExchangeShopRes");
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlOrgansShopTab))
			SysUIEnv.Instance.GetUIModule<UIPnlOrgansShopTab>().OnRefreshBeastExchangeShopSuccess(beastExchanges);
		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_BEAST_EXCHANGE_SHOP_FAILED_COSTS_NOT_ENOUGH &&
								costAndRewardAndSync != null &&
								costAndRewardAndSync.NotEnoughCost != null)
			GameUtility.ShowNotEnoughtAssetUI(costAndRewardAndSync.NotEnoughCost.Id, costAndRewardAndSync.NotEnoughCost.Count);
		else
			base.PrcErr(request, errCode, errMsg);
	}
}
#endregion