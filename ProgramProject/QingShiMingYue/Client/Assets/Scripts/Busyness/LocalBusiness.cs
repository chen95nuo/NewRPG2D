#define SERVER_BUSSINESS_ENABLE_LOG

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ClientServerCommon;
using KodGames.ClientClass;
using KodGames.ClientHelper;
using UnityEngine;

public class LocalBusiness : IBusiness
{
	private Dictionary<int, string> prtEnum = new Dictionary<int, string>(); // Protocol enum values.

	#region IBusiness Members

	public void Initialze()
	{
		SysPrefs pref = SysModuleManager.Instance.GetSysModule<SysPrefs>();
		pref.AutoLogin = true;
		pref.QuickLogin = false;
		pref.Account = "Player";
		pref.SavePassword(string.Empty, false, 0);
		pref.Save();
	}

	public void Dispose()
	{
	}

	public void Update()
	{
	}

	public bool DoesSupprotReconnect()
	{
		return false;
	}

	public string PtrErrStr(int prtVal)
	{
		string errKey = "";

		if (!prtEnum.ContainsKey(prtVal))
			errKey = string.Format("UndefinedErrorCode_{0:X8}", prtVal);
		else
			errKey = prtEnum[prtVal];

		ClientServerCommon.StringsConfig strCfg = ClientServerCommon.ConfigDatabase.DefaultCfg.StringsConfig;

		if (strCfg.HasString(GameDefines.strSrvErr, errKey))
			return strCfg.GetString(GameDefines.strSrvErr, errKey);
		else
			return string.Format("Error Code: {0}", prtVal);
	}

	public bool Logout(int callback)
	{
		return true;
	}

	public bool DisconnectAS()
	{
		return true;
	}

	public int GetNetStatus()
	{
		return com.kodgames.corgi.protocol.Protocols.E_GAME_NETSTATUS_CONNECTED;
	}

	public bool DisconnectIS()
	{
		return true;
	}

	public bool SendTimeout()
	{
		return true;
	}

	public bool ConnectIS(string hostname, int port, KodGames.ClientHelper.NetType netType, int areaID, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("ConnectIS");
#endif
		RequestMgr.Inst.Response(new ConnectISRes(callback));
		return true;
	}

	public bool CreateAccount(string authServerHostName, int port, string email, string password, int channelId, string mobile, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("CreateAccount");
#endif
		RequestMgr.Inst.Response(new CreateAccountRes(callback));
		return true;
	}

	public bool QueryManifest(string authServerHostName, int port, int resourceVersion, int version, int channelID, KodGames.ClientClass.DeviceInfo deviceInfo, int callback, int subChannelID)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("QueryManifest");
#endif
		RequestMgr.Inst.Response(new QueryManifestRes(callback, 0, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, false));
		return true;
	}

	public bool AuthActivityCode(string authServerHostName, int port, int accountId, string activityCode, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("AuthActivityCode");
#endif
		return true;
	}

	public bool Login(string authServerHostName, int port, String email, String password, string version, int channelID, KodGames.ClientClass.DeviceInfo deviceInfo, string channelUniqueId, string token, AccountChannel accountChannel, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("Login");
#endif
		List<Area> areas = new List<Area>();
		var area = new Area();
		area.AreaID = 1;
		area.AreaName = GameUtility.GetUIString("Area_Name");
		area.AreaStatus = ClientServerCommon._ServerAreaStatus.New;
		areas.Add(area);

		RequestMgr.Inst.Response(new LoginRes(callback, 1, areas, string.Empty, 1, false, false, false, null));
		return true;
	}

	public bool QuickLogin(string authServerHostName, int port, int version, int channelID, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, string bindedAccount, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("QuickLogin");
#endif
		RequestMgr.Inst.Response(new LoginRes(callback, com.kodgames.corgi.protocol.Protocols.E_AUTH_QUERY_MANIFEST_SUCCESS, GameUtility.GetUIString("FUNC_CLOSE")));
		return true;
	}

	public bool ResetPassword(string authServerHostName, int port, string email, string oldPassword, string newPassword, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("ResetPassword");
#endif
		return true;
	}

	public bool BindAccount(string authServerHostName, int port, string email, string password, string mobile, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("BindAccount");
#endif
		return true;
	}

	public bool QueryInitInfo(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("QueryInitInfo");
#endif
		Player player = new Player();

		player.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
		player.Avatars = new List<KodGames.ClientClass.Avatar>();
		player.Skills = new List<KodGames.ClientClass.Skill>();
		player.Equipments = new List<KodGames.ClientClass.Equipment>();
		player.Consumables = new List<KodGames.ClientClass.Consumable>();
		player.PlayerId = 1;

		InitPlayerConfig.Player cfgPlayer = ConfigDatabase.DefaultCfg.InitPlayerConfig.player;
		player.Name = "秦时明月";
		player.LevelAttrib.Level = cfgPlayer.player_level;
		player.LevelAttrib.Experience = cfgPlayer.player_experience;
		player.CurrentPickedLevel = cfgPlayer.player_level;
		player.VipLevel = cfgPlayer.vip_level;
		player.Badge = cfgPlayer.badge;
		player.CurrentChatCount = cfgPlayer.chatCount;
		player.GameMoney = cfgPlayer.game_money;
		player.RealMoney = cfgPlayer.real_money;

		player.QuestData.QuestQuick = new KodGames.ClientClass.QuestQuick();
		player.QuestData.Quests = new List<KodGames.ClientClass.Quest>();

		player.Stamina.Point = new TimerIncreaseValue(cfgPlayer.strength_point, System.DateTime.Now.Millisecond);

		// Daily Sign.
		player.SignData = new KodGames.ClientClass.SignData();
		player.SignData.RemedySignCount = 1;
		player.SignData.SignDetails = 29;
		player.SignData.SignCount = 1;

		player.BattleSpeed = 1.0f;

		// Avatar , Equipment, Skill
		foreach (var avatar in cfgPlayer.avatars)
		{
			var kd_avatar = new KodGames.ClientClass.Avatar();
			kd_avatar.Guid = System.Guid.NewGuid().ToString();
			kd_avatar.ResourceId = avatar.resource_id;
			//kd_avatar.Position.BattlePositions.Add(GetBattlePosition(avatar.battle_position));
			kd_avatar.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
			kd_avatar.LevelAttrib.Level = avatar.level;
			kd_avatar.LevelAttrib.Experience = avatar.experience;

			kd_avatar.BreakthoughtLevel = 3;
			KodGames.ClientClass.Domineer domineer1 = new KodGames.ClientClass.Domineer();
			domineer1.DomineerId = 0x32041018;
			domineer1.Level = 1;
			KodGames.ClientClass.Domineer domineer2 = new KodGames.ClientClass.Domineer();
			domineer2.DomineerId = 0x32041017;
			domineer2.Level = 1;

			kd_avatar.Domineer.Domineers.Add(domineer1);
			kd_avatar.Domineer.Domineers.Add(domineer2);

			MeridianData meridianData = new MeridianData();
			meridianData.Id = 0x22000001;
			kd_avatar.MeridianDatas.Add(meridianData);

			foreach (var equip in avatar.equipments)
				player.Equipments.Add(CreateEquipFromClientServerCommon(equip, kd_avatar.Guid));

			foreach (var skill in avatar.skills)
				player.Skills.Add(CreateSkillFromClientServerCommon(skill, kd_avatar.Guid));

			player.Avatars.Add(kd_avatar);
		}

		foreach (var equip in cfgPlayer.equipments)
			player.Equipments.Add(CreateEquipFromClientServerCommon(equip, string.Empty));

		foreach (var skill in cfgPlayer.skills)
			player.Skills.Add(CreateSkillFromClientServerCommon(skill, string.Empty));

		foreach (var consumable in cfgPlayer.consumables)
		{
			var kd_comsumable = new KodGames.ClientClass.Consumable();
			kd_comsumable.Id = consumable.consumable_id;
			kd_comsumable.Amount = consumable.amount;

			player.Consumables.Add(kd_comsumable);
		}

		RequestMgr.Inst.Response(new QueryInitInfoRes(callback, player, new List<com.kodgames.corgi.protocol.ActivityData>(), 0, null, false, null, 0));

		return true;
	}

	private int GetBattlePosition(int battlePos)
	{
		if (battlePos < 0)
			return -1;
		else
		{
			int row = battlePos / 3;
			int column = battlePos % 3;
			return ((row & 0x0000FFFF) << 16) | (column & 0x0000FFFF);
		}
	}

	private KodGames.ClientClass.Equipment CreateEquipFromClientServerCommon(InitPlayerConfig.Equipment equip, string avatarGuid)
	{
		var kd_equip = new KodGames.ClientClass.Equipment();
		kd_equip.ResourceId = equip.resource_id;
		kd_equip.Guid = System.Guid.NewGuid().ToString();
		kd_equip.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
		kd_equip.LevelAttrib.Level = equip.level;
		kd_equip.LevelAttrib.Experience = equip.experience;
		kd_equip.BreakthoughtLevel = equip.breakthought_level;

		return kd_equip;
	}

	private KodGames.ClientClass.Skill CreateSkillFromClientServerCommon(InitPlayerConfig.Skill skill, string avatarGuid)
	{
		var kd_skill = new KodGames.ClientClass.Skill();
		kd_skill.ResourceId = skill.resource_id;
		kd_skill.Guid = System.Guid.NewGuid().ToString();
		kd_skill.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
		kd_skill.LevelAttrib.Level = skill.level;
		kd_skill.LevelAttrib.Experience = skill.experience;

		return kd_skill;
	}

	public bool AvatarLevelUp(int callback, string avatarGUID, bool levelUpType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("EatAvatar");
#endif
		return true;
	}

	public bool AvatarBreakthought(int callback, string avatarGUID, List<string> destroyAvatarGUIDs)
	{
		return true;
	}

	public bool avatarChange(int callback, int battlePosition, string avatarOffGUID, string avatarOnGUID)
	{
		return true;
	}

	public bool ChangeMeridian(int callback, int meridianId, string avatarGuid)
	{
		return true;
	}

	public bool SaveMeridian(int callback, string avatarGuid, bool saveOrNot, int meridianId)
	{
		return true;
	}

	public bool EquipBreakthought(int callback, string equipGUID, List<string> destroyEquipGUIDs)
	{
		return true;
	}

	public bool EquipLevelUp(int callback, String equipGUID, bool strengthenType)
	{
		return true;
	}

	public bool QueryGoodsList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("QueryGoodsList");
#endif
		return true;
	}

	public bool BuyGoods(int callback, KodGames.ClientClass.GoodRecord goodRecord, int statusIndex)
	{
		return true;
	}

	public bool ConsumeItem(int callback, int itemId, int amount, int groupIndex, string phoneNumber)
	{
		return true;
	}

	public bool SellItem(int callback, List<KodGames.ClientClass.Cost> items)
	{
		return true;
	}

	public bool QueryDungeonList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("QueryDungeonList");
#endif

		List<KodGames.ClientClass.Zone> zones = new List<KodGames.ClientClass.Zone>();

		foreach (var zone in ConfigDatabase.DefaultCfg.CampaignConfig.secretZones)
		{
			KodGames.ClientClass.Zone zoneRecord = new KodGames.ClientClass.Zone();
			zoneRecord.ZoneId = zone.zoneId;
			zoneRecord.Status = _ZoneStatus.PlotDialogue;
			zoneRecord.DungeonDifficulties = new List<KodGames.ClientClass.DungeonDifficulty>();

			foreach (var diff in zone.dungeonDifficulties)
			{
				KodGames.ClientClass.DungeonDifficulty diffRecord = new KodGames.ClientClass.DungeonDifficulty();
				diffRecord.DifficultyType = diff.difficultyType;
				diffRecord.Dungeons = new List<KodGames.ClientClass.Dungeon>();
				zoneRecord.DungeonDifficulties.Add(diffRecord);

				foreach (var dungeon in diff.dungeons)
				{
					KodGames.ClientClass.Dungeon dungeonRecord = new KodGames.ClientClass.Dungeon();
					dungeonRecord.TodayCompleteTimes = 0;
					dungeonRecord.DungeonId = dungeon.dungeonId;

					diffRecord.Dungeons.Add(dungeonRecord);
				}
			}

			zones.Add(zoneRecord);
		}

		RequestMgr.Inst.Response(new QueryDungeonListRes(callback, zones, null, 0, 0, 0, 0, 0, 0));

		return true;
	}

	public bool SetZoneStatus(int callback, int zoneId, int status)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("SetZoneStatus");
#endif
		var zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(zoneId);

		if (zoneRecord == null)
		{
			zoneRecord = new KodGames.ClientClass.Zone();
			zoneRecord.ZoneId = zoneId;
			zoneRecord.DungeonDifficulties = new List<KodGames.ClientClass.DungeonDifficulty>();
			SysLocalDataBase.Inst.LocalPlayer.CampaignData.Zones.Add(zoneRecord.ZoneId, zoneRecord);
		}
		zoneRecord.Status = status;

		RequestMgr.Inst.Response(new SetZoneStatusRes(callback));

		return true;
	}

	public bool SetDungeonStatus(int callback, int dungeonId, int status)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("SetDungeonStatus");
#endif
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		var zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(dungeonCfg.ZoneId);

		if (zoneRecord == null)
		{
			zoneRecord = new KodGames.ClientClass.Zone();
			zoneRecord.ZoneId = dungeonCfg.ZoneId;
			zoneRecord.DungeonDifficulties = new List<KodGames.ClientClass.DungeonDifficulty>();
			SysLocalDataBase.Inst.LocalPlayer.CampaignData.Zones.Add(zoneRecord.ZoneId, zoneRecord);
		}

		KodGames.ClientClass.DungeonDifficulty dungeonDiff = zoneRecord.GetDungeonDiffcultyByDiffcultyType(ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId).DungeonDifficulty);
		if (dungeonDiff == null)
		{
			dungeonDiff = new KodGames.ClientClass.DungeonDifficulty();
			dungeonDiff.DifficultyType = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId).DungeonDifficulty;
			dungeonDiff.Dungeons = new List<KodGames.ClientClass.Dungeon>();
			zoneRecord.DungeonDifficulties.Add(dungeonDiff);
		}

		KodGames.ClientClass.Dungeon dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfg.ZoneId, dungeonId);
		if (dungeonRecord == null)
		{
			dungeonRecord = new KodGames.ClientClass.Dungeon();
			dungeonRecord.DungeonId = dungeonId;
			dungeonRecord.TodayCompleteTimes = 1;
			dungeonRecord.BestRecord = 3;
			dungeonDiff.Dungeons.Add(dungeonRecord);
		}

		dungeonRecord.DungeonStatus = status;

		RequestMgr.Inst.Response(new SetDungeonStatusRes(callback));

		return true;
	}

	public bool Combat(int callback, int dungeonId, KodGames.ClientClass.Position position, int npcId)
	{
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);

		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastBattleZoneID = dungeonCfg.ZoneId;
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastBattleDungeonId = dungeonId;

		KodGames.ClientClass.Dungeon dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfg.ZoneId, dungeonId);
		if (dungeonRecord == null)
		{
			dungeonRecord = new KodGames.ClientClass.Dungeon();
			dungeonRecord.DungeonId = dungeonId;
			dungeonRecord.TodayCompleteTimes = 1;
			dungeonRecord.BestRecord = 3;
		}
		else
		{
			dungeonRecord.BestRecord = 3;
			dungeonRecord.TodayCompleteTimes++;
		}

		KodGames.ClientClass.CombatResultAndReward combatResultAndReward = new KodGames.ClientClass.CombatResultAndReward();

		CampaignConfig.Dungeon dungeonConfig = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		CreateReward(combatResultAndReward.DungeonReward_ExpSilver, dungeonConfig.fixedRewards);
		CreateReward(combatResultAndReward.DungeonReward, dungeonConfig.displayRewards);

		combatResultAndReward.BattleRecords = new List<KodGames.ClientClass.BattleRecord>();

		String battleRecordFile = "Texts/Configs/Battle_" + dungeonId.ToString("X8");
		TextAsset asset = ResourceManager.Instance.LoadAsset<TextAsset>(battleRecordFile);
		if (asset == null)
			Debug.Log(battleRecordFile + " is not found.");
		else
		{
			com.kodgames.corgi.protocol.BattleRecord protoData = new KodGames.ClientHelper.TypeModelProtobufSerializer(new Protocols_c()).Deserialize(new MemoryStream(asset.bytes), typeof(com.kodgames.corgi.protocol.BattleRecord)) as com.kodgames.corgi.protocol.BattleRecord;
			KodGames.ClientClass.BattleRecord battleRecord = new KodGames.ClientClass.BattleRecord().FromProtobuf(protoData);
			//				new KodGames.ClientClass.BattleRecord().FromProtoBuffData(asset.bytes);
			battleRecord.SceneId = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId).sceneId;
			combatResultAndReward.BattleRecords.Add(battleRecord);
		}

		RequestMgr.Inst.Response(new OnCombatRes(callback, combatResultAndReward, new CostAndRewardAndSync(), _ZoneStatus.ZoneProceed, dungeonRecord, null));

		return true;
	}

	private void CreateReward(KodGames.ClientClass.Reward reward, List<ClientServerCommon.Reward> rewardConfigs)
	{
		foreach (var rewardConfig in rewardConfigs)
		{
			switch (IDSeg.ToAssetType(rewardConfig.id))
			{
				case IDSeg._AssetType.Item:
					Consumable consumable = new Consumable();
					consumable.Id = rewardConfig.id;
					consumable.Amount = rewardConfig.count;
					reward.Consumable.Add(consumable);

					break;
				case IDSeg._AssetType.Avatar:
					KodGames.ClientClass.Avatar avatar = new KodGames.ClientClass.Avatar();
					avatar.ResourceId = rewardConfig.id;
					reward.Avatar.Add(avatar);
					break;
				case IDSeg._AssetType.Equipment:
					KodGames.ClientClass.Equipment equip = new KodGames.ClientClass.Equipment();
					equip.ResourceId = rewardConfig.id;
					reward.Equip.Add(equip);
					break;
				case IDSeg._AssetType.CombatTurn:
					KodGames.ClientClass.Skill skill = new KodGames.ClientClass.Skill();
					skill.ResourceId = rewardConfig.id;
					reward.Skill.Add(skill);
					break;
			}
		}
	}

	public bool ResetDungeonCompleteTimes(int callback, int dungeonId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("ResetDungeonCompleteTimes");
#endif
		return true;
	}

	public bool DungeonGetReward(int callback, int zoneId, int dungeonDifficulty, int boxIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("DungeonGetReward");
#endif
		return true;
	}

	public bool SetDungeonDialogState(int callback, int dungeonId, int state)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("SetDungeonDialogState");
#endif
		return true;
	}

	public bool ContinueCombat(int callback, int zoneId, int dungeonId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("ContinueCombat");
#endif
		return true;
	}

	public bool QueryDungeonGuide(int callback, int dungeonId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("QueryDungeonGuide");
#endif
		return true;
	}

	public bool QueryTravel(int callback, int dungeonId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("QueryTravel");
#endif
		return true;
	}

	public bool BuyTravel(int callback, int dungeonId, int goodId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("BuyTravel");
#endif
		return true;
	}

	public bool QueryRecruiteNpc(int callback, int dungeonId)
	{
		return true;
	}

	public bool QueryEmailListInfo(int callback, int emailType)
	{
		return true;
	}

	public bool GetAttachments(int callback, long emailId)
	{
		return true;
	}

	public bool DeleteEmail(int callback, List<long> deleteEmailIds)
	{
		return true;
	}

	public bool MergeIllustration(int callback, int id, int count)
	{
		return true;
	}

	public bool QueryIllustration(int callback)
	{
		return true;
	}

	public bool SignIn(int callback, int signType)
	{
		return true;
	}

	public bool ApplePurchase(byte[] paymentTransactionReceipt, int callback)
	{
		return true;
	}

	public bool ApplePurchaseTest(int goodsID, int count, int callback, string additionalData)
	{
		return true;
	}

	public bool SkillLevelUp(int callback, string skillGUID, List<string> destroySkillGUIDs)
	{
		return true;
	}

	public bool QueryInitRobInfo(int callback, int robType)
	{
		return true;
	}

	public bool RobSelectType(int callback, int robType, int skillId)
	{
		return true;
	}

	public bool Rob(int callback, int robType, int playerId, int fragmentId)
	{
		return true;
	}

	public bool MixType(int callback, int robType, int skillId)
	{
		return true;
	}

	public bool Chat(int callback, com.kodgames.corgi.protocol.ChatMessage chatMessage)
	{
		return true;
	}

	public bool QueryArenaRank(int callback)
	{
		return true;
	}

	public bool ArenaCombat(int callback, int rank, KodGames.ClientClass.Position position)
	{
		return true;
	}

	public bool QueryArenaPlayerInfo(int callback, int rank, int arenaGradeId)
	{
		return true;
	}

	public bool QueryRankToFew(int callback)
	{
		return true;
	}

	public bool QueryPlayerInfo(int callback, int playerId)
	{
		return true;
	}

	public bool AnswerFriend(int callback, int activePlayerId, long passiveEmailId, bool agree)
	{
		return true;
	}

	public bool QueryInitFriend(int callback)
	{
		return true;
	}

	public bool InviteFriend(int callback, int passivePlayerId, string passivePlayerName)
	{
		return true;
	}

	public bool RandomFriend(int callback)
	{
		return true;
	}

	public bool QueryFriendPlayerInfo(int callback, int playerId)
	{
		return true;
	}

	public bool RemoveFriend(int callback, int removePlayerId)
	{
		return true;
	}

	public bool CombatFriend(int callback, int friendId, KodGames.ClientClass.Position position)
	{
		return true;
	}

	public bool QueryPlayerName(int callback, string playerName)
	{
		return true;
	}

	public bool BuyConsignment(int callback, long consignmentIdx, List<KodGames.ClientClass.Cost> costs)
	{
		return true;
	}

	public bool Consignment(int callback, string guid, List<KodGames.ClientClass.Cost> costs)
	{
		return true;
	}

	public bool Downshelf(int callback, long consignmentIdx)
	{
		return true;
	}

	public bool QueryConsignmentList(int callback, int type, int subType, int isPrevNotNext, long consignIdx)
	{
		return true;
	}

	public bool QuerySelfConsignment(int callback)
	{
		return true;
	}

	public bool QueryAvatarList(int callback, int type)
	{
		return true;
	}

	public bool QueryChatMessageList(int callback)
	{
		return true;
	}

	public bool CloseChatMessageDialog(int callback)
	{
		return true;
	}

	public bool ExchangeCode(int callback, string strRewardKey)
	{
		return true;
	}

	public bool SettingFeedback(int callback, int type, string strInfo)
	{
		return true;
	}

	public bool SignInContinueActivity(int callback)
	{
		return true;
	}

	public bool QueryFixedTimeActivityReward(int callback)
	{
		return true;
	}

	public bool GetFixedTimeActivityReward(int callback)
	{
		return true;
	}

	public bool QueryTreasureBowlInfo(int callback)
	{
		return true;
	}

	public bool PayAndGetRealMoney(int callback)
	{
		return true;
	}

	public bool GetLotteryList(int callback)
	{
		return true;
	}

	public bool DrawLottery(int callback, int activityId)
	{
		return true;
	}

	public bool QueryExchangeList(int callback)
	{
		return true;
	}

	public bool ExchangeReq(int callback, int exchangeId, List<KodGames.ClientClass.Cost> costs, int groupId)
	{
		return true;
	}

	public bool QueryFirstGetList(int callback)
	{
		return true;
	}

	public bool FirstGetReward(int callback, int firstGetId)
	{
		return true;
	}

	public bool QueryProcessRechargeInfo(int callback)
	{
		return true;
	}

	public bool QueryProcessSpendInfo(int callback)
	{
		return true;
	}

	public bool GetProcessRechargeReward(int callback, int accumulateId)
	{
		return true;
	}

	public bool GetProcessSpendReward(int callback, int accumulateId)
	{
		return true;
	}

	public bool PickStartServerReward(int callback, int pickId)
	{
		return true;
	}

	public bool KongMingQueryInfo(int callback)
	{
		return true;
	}

	public bool KongMingAnswer(int callback, int chooseIndex)
	{
		return true;
	}

	public bool KongMingFetch(int callback)
	{
		return true;
	}

	public bool CycleQueryInfo(int callback)
	{
		return true;
	}

	public bool CycleRandomAndMove(int callback)
	{
		return true;
	}

	public bool TavernQuery(int callback)
	{
		return true;
	}

	public bool TavernBuy(int callback, int tavernId, int tavernType)
	{
		return true;
	}

	public bool QueryRankInfo(int callback)
	{
		return true;
	}

	public bool LevelRewardGetReward(int callback, int levelRewardId)
	{
		return true;
	}

	public bool QueryLevelReward(int callback)
	{
		return true;
	}

	public bool QueryGradePoint(int callback)
	{
		return true;
	}

	public bool BuyAndUse(int callback, int goodsId, int statusIndex)
	{
		return true;
	}

	public bool BuySpecialGoods(int callback, int goodsId)
	{
		return true;
	}

	public bool GetTutorialAvatarAndSetPlayerName(int callback, int resourceId, string playerName, int tutorialId)
	{
		return true;
	}

	public bool CompleteTutorial(int callback, int tutorialId)
	{
		return true;
	}

	public bool NoviceCombat(int callback)
	{
		return true;
	}

	public bool FetchRandomPlayerNames(int callback)
	{
		return true;
	}

	public bool QueryQuestInfo(int callback)
	{
		return true;
	}

	public bool PickQuestReward(int callback, int questId)
	{
		return true;
	}

	public bool GetLevelUpReward(int callback, int wantPickLevel)
	{
		return true;
	}

	public bool WorldBoss_Query(int callback)
	{
		return true;
	}

	public bool WorldBoss_StopAutoBattle(int callback, bool wantAuto)
	{
		return true;
	}

	public bool WorldBoss_EnterView(int callback)
	{
		return true;
	}

	public bool WorldBoss_LeaveView(int callback)
	{
		return true;
	}

	public bool WorldBoss_ViewBattleRank(int callback)
	{
		return true;
	}

	public bool WorldBoss_Excitation(int callback, int type)
	{
		return true;
	}

	public bool WorldBoss_Battle(int callback)
	{
		return true;
	}

	public bool WorldBoss_Respawn(int callback)
	{
		return true;
	}

	public bool WorldBoss_EnterOrLeaveMainView(int callback, bool isEnter)
	{
		return true;
	}

	public bool GiveMeFive(int callback, bool isEvaluate)
	{
		return true;
	}

	public bool QueryMysteryShopInfo(int callback, int shopType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[LocalBusiness] [Test] QueryMysteryShopInfo.");
#endif
		List<MysteryGoodInfo> goods = new List<MysteryGoodInfo>();

		var goodscfg = ConfigDatabase.DefaultCfg.MysteryShopConfig.GetShopByType(MysteryShopConfig._ShopType.Normal).goodsSet;
		int goodMaxCount = goodscfg.Count > 6 ? 6 : goodscfg.Count;

		for (int i = 0; i < goodMaxCount; i++)
		{
			MysteryGoodInfo tempGoodsInfo = new KodGames.ClientClass.MysteryGoodInfo();
			if (i == 1)
				tempGoodsInfo.GoodId = goodscfg[0].goodsId;
			else
				tempGoodsInfo.GoodId = goodscfg[i].goodsId;
			tempGoodsInfo.GoodIndex = i;
			tempGoodsInfo.BuyOrNot = false;
			tempGoodsInfo.Cost = new KodGames.ClientClass.Cost()
			{
				Id = goodscfg[i].cost.id,
				Count = goodscfg[i].cost.count,
				Guid = System.Guid.NewGuid().ToString(),
			};

			tempGoodsInfo.DiscountCost = new KodGames.ClientClass.Cost()
			{
				Id = goodscfg[i].discountCost.id,
				Count = goodscfg[i].discountCost.count,
				Guid = System.Guid.NewGuid().ToString(),
			};
			goods.Add(tempGoodsInfo);
		}

		MysteryShopInfo mysteryShopInfo = new MysteryShopInfo()
		{
			Goods = goods,
			NextRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime + 10000,
			LastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime,
			PlayerRefreshTimes = 0,
		};

		RequestMgr.Inst.Response(new QueryMysteryShopRes(callback, mysteryShopInfo));

		//RequestMgr.Inst.Response(new QueryMysteryShopRes(callback, 0, "Error"));

		return true;
	}

	public bool ChangeMysteryShopInfo(int callback, int shopType, int refreshId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[LocalBusiness] [Test] ChangeMysterShopInfo refreshId=" + refreshId);
#endif

		List<MysteryGoodInfo> goods = new List<MysteryGoodInfo>();

		var goodscfg = ConfigDatabase.DefaultCfg.MysteryShopConfig.GetShopByType(MysteryShopConfig._ShopType.Normal).goodsSet;
		int goodMaxCount = goodscfg.Count > 6 ? 6 : goodscfg.Count;

		for (int i = 0; i < goodMaxCount; i++)
		{
			MysteryGoodInfo tempGoodsInfo = new KodGames.ClientClass.MysteryGoodInfo();

			if (i == 1)
				tempGoodsInfo.GoodId = goodscfg[0].goodsId;
			else
				tempGoodsInfo.GoodId = goodscfg[i].goodsId;
			tempGoodsInfo.BuyOrNot = false;

			tempGoodsInfo.GoodIndex = i;
			tempGoodsInfo.Cost = new KodGames.ClientClass.Cost()
			{
				Id = goodscfg[i].cost.id,
				Count = goodscfg[i].cost.count,
				Guid = System.Guid.NewGuid().ToString(),
			};

			tempGoodsInfo.DiscountCost = new KodGames.ClientClass.Cost()
			{
				Id = goodscfg[i].discountCost.id,
				Count = goodscfg[i].discountCost.count,
				Guid = System.Guid.NewGuid().ToString(),
			};
			goods.Add(tempGoodsInfo);
		}

		MysteryShopInfo mysteryShopInfo = new MysteryShopInfo()
		{
			Goods = goods,
			NextRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime + 200000,
			LastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime,
			PlayerRefreshTimes = 0,
		};

		RequestMgr.Inst.Response(new ChangeMysteryShopRes(callback, mysteryShopInfo, new CostAndRewardAndSync()));

		//RequestMgr.Inst.Response(new ChangeMysteryShopRes(callback, 0, "Error", new CostAndRewardAndSync()));

		return true;
	}

	public bool BuyMysteryGoods(int callback, int shopType, int goodsId, int goodsIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[LocalBusiness] [Test] BuyMysteryGood GoodId=" + goodsId + "GoodsIndex=" + goodsIndex);
#endif
		RequestMgr.Inst.Response(new BuyMysteryGoodRes(callback,
		new MysteryGoodInfo()
		{
			GoodId = goodsId,
			GoodIndex = goodsIndex
		},
		 new CostAndRewardAndSync()));
		return true;
	}

	public bool ChangeDomineer(int callback, string avatarGuid, List<string> destroyAvatarGuids)
	{
		KodGames.ClientClass.Avatar avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(avatarGuid);
		avatar.Domineer.UnsaveDomineers.AddRange(avatar.Domineer.Domineers);

		RequestMgr.Inst.Response(new ChangeDomineerRes(callback, avatar, new CostAndRewardAndSync()));

		return true;
	}

	public bool SaveDomineer(int callback, string avatarGuid, bool isSave)
	{
		KodGames.ClientClass.Avatar avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(avatarGuid);
		if (isSave)
		{
			avatar.Domineer.Domineers.Clear();
			avatar.Domineer.Domineers.AddRange(avatar.Domineer.UnsaveDomineers);
			avatar.Domineer.UnsaveDomineers.Clear();
		}
		else
		{
			avatar.Domineer.UnsaveDomineers.Clear();
		}

		RequestMgr.Inst.Response(new SaveDomineerRes(callback, avatar));

		return true;
	}

	public bool QueryPositionList(int callback)
	{
		throw new NotImplementedException();
	}

	public bool OpenPosition(int callback, int positionId)
	{
		throw new NotImplementedException();
	}

	public bool SetMasterPosition(int callback, int positionId)
	{
		throw new NotImplementedException();
	}

	public bool EmBattle(int callback, int positionId, int locationId1, int locationId2)
	{
		throw new NotImplementedException();
	}

	public bool ChangeLocation(int callback, string guid, int resourceId, string offGuid, int positionId, int location, int index)
	{
		throw new NotImplementedException();
	}

	public bool OneClickPositionOff(int callback, int positionId)
	{
		throw new NotImplementedException();
	}

	public bool QueryQinInfo(int callback)
	{
		return true;
	}

	public bool AnswerQinInfo(int callback, int questionId, int answerNum)
	{
		return true;
	}

	public bool GetQinInfoContinueReward(int callback)
	{
		return true;
	}

	public bool QueryMonthCardInfo(int callback)
	{
		return true;
	}

	public bool MonthCardPickReward(int callback, int monthCardId, int rewardType)
	{
		return true;
	}

	public bool PartnerOpen(int callback, int partnerId) { return true; }

	public bool PartnerSetup(int callback, int positionId, int partnerId, string avatarGuid) { return true; }

	public bool QueryDinerList(int callback) { return true; }

	public bool HireDiner(int callback, int qualityType, int dinerId) { return true; }

	public bool RenewDiner(int callback, int qualityType, int dinerId) { return true; }

	public bool FireDiner(int callback, int qualityType, int dinerId) { return true; }

	public bool RefreshDinerList(int callback, int bagId) { return true; }

	public bool QueryTaskList(int callback) { return true; }

	public bool TaskCondition(int callback, int gotoUI) { return true; }

	public bool QueryMelaleucaFloorPlayerInfo(int callback) { return true; }

	public bool QueryMelaleucaFloorInfo(int callback, int layers) { return true; }

	public bool MelaleucaFloorCombat(int callback, int layers, KodGames.ClientClass.Position position) { return true; }

	public bool MelaleucaFloorConsequentCombat(int callback, int layers, int combatCount, KodGames.ClientClass.Position position) { return true; }

	public bool MelaleucaFloorThisWeekRank(int callback) { return true; }

	public bool MelaleucaFloorLastWeekRank(int callback) { return true; }

	public bool MelaleucaFloorGetReward(int callback) { return true; }

	public bool MelaleucaFloorWeekRewardInfo(int callback) { return true; }

	//WolfSmoke
	public bool QueryBuyWolfSmokeShop(int callback, int goodsId, int goodsIndex) { return true; }

	public bool QueryCombatWolfSmoke(int callback, int additionId, KodGames.ClientClass.Position position) { return true; }

	public bool QueryJoinWolfSmoke(int callback, int positionId) { return true; }

	public bool QueryWolfSmoke(int callback) { return true; }

	public bool QueryWolfSmokeEnemy(int callback) { return true; }

	public bool QueryWolfSmokePosition(int callback) { return true; }

	public bool QueryWolfSmokeShop(int callback) { return true; }

	public bool QueryRefreshWolfSmokeShop(int callback) { return true; }

	public bool QueryResetWolfSmoke(int callback) { return true; }

	public bool SendAPNToken(int callback, byte[] token) { return true; }

	//Friend
	public bool QueryFriendList(int callback)
	{
		var friends = new List<FriendInfo>();

		for (int index = 0; index < 10; index++)
		{
			var friend = new FriendInfo();
			friend.PlayerId = 10000 + index;
			friend.Name = "Name" + index;
			friends.Add(friend);
		}

		RequestMgr.Inst.Response(new OnQueryFriendListRes(callback, friends));
		return true;
	}

	// activity
	public bool OperationActivityQueryReq(int callback) { return true; }

	public bool OperationActivityPickRewardReq(int callback, int operationId) { return true; }

	// FriendCombatSystem
	public bool QueryFriendCampaignReq(int callback) { return true; }

	public bool ResetFriendCampaignReq(int callback) { return true; }

	public bool JoinFriendCampaignReq(int callback, int positionId, List<int> friendIds) { return true; }

	public bool CombatFriendCampaignReq(int callback, int playerId, KodGames.ClientClass.Position position) { return true; }

	public bool QueryFriendCampaignHelpFriendInfoReq(int callback, List<int> playerIds) { return true; }

	public bool QueryFCRankReq(int callback, int rankType) { return true; }

	public bool QueryFCPointDetailReq(int callback, int rankType) { return true; }

	public bool QueryFCRankRewarReq(int callback) { return true; }

	public bool FCRankGetRewardReq(int callback) { return true; }


	//Illusion
	public bool QueryIllusionReq(int callback) { return true; }

	public bool ActivateIllusionReq(int callback, int avatarId, int illusionId, int activateType) { return true; }

	public bool IllusionReq(int callback, int avatarId, int illusionId, int type, int useStatusType) { return true; }

	public bool ActivateAndIllusionReq(int callback, int avatarId, int illusionId, int useStatus) { return true; }

	//Adventure
	public bool QueryMarvellousNextMarvellousReq(int callback, int nextEventId, int nextZone, KodGames.ClientClass.Position position) { return true; }

	public bool QueryMarvellousPickDelayRewardReq(int callback, int eventId, long couldPickTime) { return true; }

	public bool QueryMarvellousQueryDelayRewardReq(int callback) { return true; }

	public bool QueryMarvellousQueryReq(int callback) { return true; }

	//东海寻仙
	//主查询
	public bool QueryZentiaReq(int callback) { return true; }
	//东海寻仙玩家获得特殊道具跑马灯信息
	public bool QueryZentiaFlowMessageReq(int callback) { return true; }
	//兑换东海寻仙道具
	public bool ExchangeZentiaItemReq(int callback, int exchangeId, int index, List<KodGames.ClientClass.Cost> costs) { return true; }
	//刷新东海寻仙道具兑换
	public bool RefreshZentiaReq(int callback) { return true; }
	//查询仙缘兑换商品
	public bool QueryZentiaGoodReq(int callback) { return true; }
	//仙缘兑换下的商品购买
	public bool BuyZentiaGoodReq(int callback, int goodsId) { return true; }
	//查询全服奖励
	public bool QueryServerZentiaRewardReq(int callback) { return true; }
	//领取全服奖励
	public bool GetServerZentiaRewardReq(int callback, int rewardLevelId) { return true; }
	//排行榜查询
	public bool QueryZentiaRankReq(int callback) { return true; }



	#endregion IBusiness Members

	public bool GiveFiveStarsEvaluate(int callback, bool isEvaluate)
	{
		return true;
	}

	//新神秘商店
	public bool QueryMysteryerReq(int callback) { return true; }
	public bool RefreshMysteryerReq(int callback, int type) { return true; }
	public bool BuyMysteryerReq(int callback, int goodId, int place) { return true; }

	//海外兑换码
	public bool QueryInviteCodeInfoReq(int callback) { return true; }
	public bool VerifyInviteCodeAndPickRewardReq(int callback, string code) { return true; }
	public bool PickInviteCodeRewardReq(int callback, int rewardId) { return true; }
	public bool FacebookShareReq(int callback) { return true; }

	//711活动
	public bool QuerySevenElevenGiftReq(int callback, string deviceId) { return true; }
	public bool TurnNumberReq(int callback, string deviceId, int position) { return true; }
	public bool NumberConvertReq(int callback, int contertType) { return true; }
	//内丹系统
	public bool QueryAlchemy(int callback)
	{
		//今日炼丹次数虚拟
		int todayAlchemyCount = 21;

		long nextTime = 0;

		//普通炼丹消耗虚拟
		List<KodGames.ClientClass.Cost> alchemyCosts = new List<KodGames.ClientClass.Cost>();
		KodGames.ClientClass.Cost alchemyCost = new KodGames.ClientClass.Cost();
		alchemyCost.Id = -16777200;
		alchemyCost.Count = 8000;
		alchemyCosts.Add(alchemyCost);

		//批量炼丹消耗虚拟
		List<KodGames.ClientClass.Cost> batchAlchemyCosts = new List<KodGames.ClientClass.Cost>();
		KodGames.ClientClass.Cost batchAlchemyCost1 = new KodGames.ClientClass.Cost();
		batchAlchemyCost1.Id = -16777200;
		batchAlchemyCost1.Count = 9500;

		KodGames.ClientClass.Cost batchAlchemyCost2 = new KodGames.ClientClass.Cost();
		batchAlchemyCost2.Id = -16777200 + 1;
		batchAlchemyCost2.Count = 2000;
		batchAlchemyCosts.Add(batchAlchemyCost1);
		batchAlchemyCosts.Add(batchAlchemyCost2);

		List<com.kodgames.corgi.protocol.BoxReward> boxRewards = new List<com.kodgames.corgi.protocol.BoxReward>();

		//宝箱虚拟
		for (int i = 0; i < 10; i++)
		{
			com.kodgames.corgi.protocol.BoxReward boxReward = new com.kodgames.corgi.protocol.BoxReward();

			if (i == 2 || i == 6 || i == 9)
				boxReward.hasActivityIcon = true;
			else
				boxReward.hasActivityIcon = false;
			boxReward.hasPicked = false;
			if (i < 3)
				boxReward.hasPicked = true;
			boxReward.id = 0x56000001 + i;
			boxReward.iconId = 0x15000253;
			boxReward.openIconId = 0x15000254;
			boxReward.alchemyCount = i * 5;

			//固定奖励虚拟
			List<com.kodgames.corgi.protocol.ShowReward> fixRewards = new List<com.kodgames.corgi.protocol.ShowReward>();

			for (int j = 0; j < 8; j++)
			{
				com.kodgames.corgi.protocol.ShowReward fixReward = new com.kodgames.corgi.protocol.ShowReward();
				fixReward.id = 0x03000010 + j;
				fixReward.count = j * 10;
				fixRewards.Add(fixReward);
			}

			//随即奖励虚拟
			List<com.kodgames.corgi.protocol.ShowReward> randomRewards = new List<com.kodgames.corgi.protocol.ShowReward>();

			for (int j = 0; j < 5; j++)
			{
				com.kodgames.corgi.protocol.ShowReward randomReward = new com.kodgames.corgi.protocol.ShowReward();
				randomReward.id = 0x03000010 + j;
				randomReward.count = j + 2;
				randomRewards.Add(randomReward);
			}

			boxReward.randomRewards.AddRange(randomRewards);
			boxReward.rewards.AddRange(fixRewards);

			boxRewards.Add(boxReward);
		}

		com.kodgames.corgi.protocol.ShowCounter showCounter = new com.kodgames.corgi.protocol.ShowCounter();
		com.kodgames.corgi.protocol.AlchemyClientIcon alchemyClientIcon = new com.kodgames.corgi.protocol.AlchemyClientIcon();
		//alchemyClientIcon.activityText = "光棍节烧烧烧";
		alchemyClientIcon.alchemyDesc = "活动时间：2014.11.11-2014.12.24";
		alchemyClientIcon.NoActivityText = "无活动无活动无活动无活动无活动";

		RequestMgr.Inst.Response(new QueryAlchemyRes(callback, todayAlchemyCount, alchemyCosts, batchAlchemyCosts, boxRewards, showCounter, alchemyClientIcon, nextTime, new com.kodgames.corgi.protocol.DecomposeInfo()));
		return true;
	}

	public bool PickAlchemyBox(int callback, int countRewardId) { return true; }
	public bool QueryDanActivity(int callback, int activityType) { return true; }
	public bool Alchemy(int callback, int chatType, List<KodGames.ClientClass.Cost> cost)
	{
		CostAndRewardAndSync costAndRewardAndSync = new CostAndRewardAndSync();
		KodGames.ClientClass.Reward reward = new KodGames.ClientClass.Reward();

		KodGames.ClientClass.Reward extraReward = new KodGames.ClientClass.Reward();
		KodGames.ClientClass.Consumable consumable = new KodGames.ClientClass.Consumable();
		consumable.Id = 0x07020119;
		consumable.Amount = 2;
		//KodGames.ClientClass.Consumable consumable2 = new KodGames.ClientClass.Consumable();
		//consumable2.Id = 0x07020120;
		//consumable2.Amount = 1;
		extraReward.Consumable.Add(consumable);
		//extraReward.Consumable.Add(consumable2);

		int todayAlchemyCount = 32;

		com.kodgames.corgi.protocol.ShowCounter showCounter = new com.kodgames.corgi.protocol.ShowCounter();

		for (int i = 0; i < 10; i++)
		{
			KodGames.ClientClass.Dan dan = new KodGames.ClientClass.Dan();
			dan.ResourceId = 0x57000001;
			if (i == 0 || i == 7 || i == 9)
				dan.BreakthoughtLevel = 4;
			else if (i == 2 || i == 8)
				dan.BreakthoughtLevel = 5;
			else dan.BreakthoughtLevel = 1;
			dan.LevelAttrib.Level = 1 + i;
			reward.Dan.Add(dan);
		}

		//普通炼丹消耗虚拟
		List<KodGames.ClientClass.Cost> alchemyCosts = new List<KodGames.ClientClass.Cost>();
		KodGames.ClientClass.Cost alchemyCost = new KodGames.ClientClass.Cost();
		alchemyCost.Id = -16777200;
		alchemyCost.Count = 8000;
		alchemyCosts.Add(alchemyCost);

		//批量炼丹消耗虚拟
		List<KodGames.ClientClass.Cost> batchAlchemyCosts = new List<KodGames.ClientClass.Cost>();
		KodGames.ClientClass.Cost batchAlchemyCost1 = new KodGames.ClientClass.Cost();
		batchAlchemyCost1.Id = -16777200;
		batchAlchemyCost1.Count = 9500;

		KodGames.ClientClass.Cost batchAlchemyCost2 = new KodGames.ClientClass.Cost();
		batchAlchemyCost2.Id = -16777200 + 1;
		batchAlchemyCost2.Count = 2000;
		batchAlchemyCosts.Add(batchAlchemyCost1);
		batchAlchemyCosts.Add(batchAlchemyCost2);

		bool isNeedRefresh = false;

		RequestMgr.Inst.Response(new AlchemyRes(callback, costAndRewardAndSync, reward, extraReward, showCounter, todayAlchemyCount, alchemyCosts, batchAlchemyCosts, isNeedRefresh, new com.kodgames.corgi.protocol.DecomposeInfo()));

		return true;
	}
	public bool QueryDanHome(int callback)
	{
		return true;
	}

	public bool QueryLockDan(int callback, int type, string guid)
	{
		return true;
	}

	public bool QueryDanStore(int callback, int type)
	{
		return true;
	}

	public bool DanDecompose(int callback, int type, List<string> guids, KodGames.ClientClass.Cost cost)
	{
		return true;
	}

	public bool QueryDanDecompose(int callback)
	{
		return true;
	}

	public bool DanLevelUp(int callback, string guid) { return true; }
	public bool DanBreakthought(int callback, string guid) { return true; }
	public bool DanAttributeRefresh(int callback, string guid, List<int> attributeGroupIds) { return true; }

	public bool GuildQuery(int callback)
	{
		return true;
	}

	public bool GuildSetAnnouncement(int callback, string announcement)
	{
		return true;
	}

	public bool GuildCreate(int callback, string guildName, bool allowAutoEnter)
	{
		return true;
	}

	public bool GuildQueryGuildList(int callback, string keyWord)
	{
		return true;
	}

	public bool GuildApply(int callback, int guildId)
	{
		return true;
	}

	public bool GuildQuickJoin(int callback)
	{
		return true;
	}

	public bool GuildViewSimple(int callback, int guildId)
	{
		return true;
	}

	public bool GuildQueryRankList(int callback)
	{
		return true;
	}

	public bool GuildQueryMsg(int callback)
	{
		return true;
	}

	public bool GuildAddMsg(int callback, string msg)
	{
		return true;
	}

	public bool GuildQueryNews(int callback)
	{
		return true;
	}

	public bool GuildSetDeclaration(int callback, string declaration)
	{
		return true;
	}

	public bool GuildQueryTransferMember(int callback)
	{
		return true;
	}

	public bool GuildTransfer(int callback, int destPlayer)
	{
		return true;
	}

	public bool GuildQuit(int callback)
	{
		return true;
	}

	public bool GuildQueryMember(int callback)
	{
		return true;
	}

	public bool GuildQueryApplyList(int callback)
	{
		return true;
	}

	public bool GuildReviewApply(int callback, int playerId, bool refuse)
	{
		return true;
	}

	public bool GuildOneKeyRefuse(int callback, List<int> playerIds)
	{
		return true;
	}

	public bool GuildKickPlayer(int callback, int playerId)
	{
		return true;
	}

	public bool GuildSetPlayerRole(int callback, int playerId, int roleId)
	{
		return true;
	}

	public bool GuildSetAutoEnter(int callback, bool allowAutoEnter)
	{
		return true;
	}

	public bool QueryConstructionTask(int callback)
	{
		return true;
	}

	public bool AcceptConstructionTask(int callback, int taskId, int taskIndex)
	{
		return true;
	}

	public bool GiveUpConstructionTask(int callback, int taskId, int taskIndex)
	{
		return true;
	}

	public bool CompleteConstructionTask(int callback, int taskId, List<KodGames.ClientClass.Cost> costs, int taskIndex)
	{
		return true;
	}

	public bool RefreshConstructionTask(int callback)
	{
		return true;
	}

	public bool QueryGuildPublicShop(int callback)
	{
		return true;
	}

	public bool BuyGuildPublicGoods(int callback, int goodsId)
	{
		return true;
	}

	public bool QueryGuildPrivateShop(int callback)
	{
		return true;
	}

	public bool BuyGuildPrivateGoods(int callback, int goodsId, int goodsCount)
	{
		return true;
	}

	public bool QueryGuildExchangeShop(int callback)
	{
		return true;
	}

	public bool ExchangeGuildExchangeGoods(int callback, int exchangeId, List<KodGames.ClientClass.Cost> costs)
	{
		return true;
	}

	public bool QueryGuildTask(int callback)
	{
		return true;
	}

	public bool GuildTaskDice(int callback)
	{
		return true;
	}

	public bool RefreshGuildTask(int callback)
	{
		return true;
	}

	public bool OpenGuildStage(int callback)
	{
		return true;
	}
	public bool QueryGuildStage(int callback, int type)
	{
		return true;
	}
	public bool GuildStageExplore(int callback, int moveIndex, int exploreIndex)
	{
		return true;
	}
	public bool GuildStageCombatBoss(int callback, int exploreIndex, int type)
	{
		return true;
	}
	public bool GuildStageGiveBox(int callback, int playerId)
	{
		return true;
	}
	public bool GuildStageReset(int callback)
	{
		return true;
	}
	public bool GuildStageQueryMsg(int callback, int type)
	{
		return true;
	}
	public bool GuildStageQueryBossRank(int callback)
	{
		return true;
	}
	public bool GuildStageQueryBossRankDetail(int callback, int mapNum, int num)
	{
		return true;
	}
	public bool GuildStageQueryExploreRank(int callback, int type)
	{
		return true;
	}
	public bool GuildStageQueryRank(int callback, int rankType)
	{
		return true;
	}
	public bool GuildStageQueryTalent(int callback, int type)
	{
		return true;
	}
	public bool GuildStageTalentReset(int callback)
	{
		return true;
	}
	public bool GuildStageTalentAdd(int callback, int type, int talentId)
	{
		return true;
	}

	public bool QueryFacebookReq(int callback)
	{
		return true;
	}

	public bool FacebookRewardReq(int callback)
	{
		return true;
	}

	public bool SetPlayerNameReq(int callback, string playerName) { return true; }
	public bool SetGuildNameReq(int callback, string guildName) { return true; }

	public bool ActiveBeast(int callback, int id) { return true; }
	public bool EquipBeastPart(int callback, string guid, int index) { return true; }
	public bool BeastBreakthought(int callback, string guid) { return true; }
	public bool BeastLevelUp(int callback, string guid) { return true; }
	public bool QueryBeastExchangeShop(int callback) { return true; }
	public bool BeastExchangeShop(int callback, int exchangeId, int index, List<KodGames.ClientClass.Cost> costs) { return true; }
	public bool RefreshBeastExchangeShop(int callback) { return true; }
}