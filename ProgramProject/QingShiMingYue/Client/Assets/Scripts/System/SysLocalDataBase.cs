using UnityEngine;
using System.Collections.Generic;
using KodGames.ClientClass;
using System;
using ClientServerCommon;
using KodGames;

public class LocalSet<T> : IEnumerable<T>
{
	private List<T> datas = new List<T>();

	public bool Add(T t)
	{
		if (!datas.Contains(t))
		{
			datas.Add(t);
			return true;
		}

		return false;
	}

	public void Clear()
	{
		this.datas.Clear();
	}

	public List<T> ToList()
	{
		return datas;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return datas.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return datas.GetEnumerator();
	}
}

public class SysLocalDataBase : SysModule
{
	public delegate void LocalDataChangedDel();

	private Dictionary<int, LocalDataChangedDel> dic_dataDel = new Dictionary<int, LocalDataChangedDel>();

	public void RegisterDataChangedDel(int type, LocalDataChangedDel del)
	{
		if (!dic_dataDel.ContainsKey(type))
			dic_dataDel.Add(type, null);

		dic_dataDel[type] += del;
	}

	public void DeleteDataChangedDel(int type, LocalDataChangedDel del)
	{
		if (dic_dataDel.ContainsKey(type))
			dic_dataDel[type] -= del;
	}

	public void Notify(int dataChangedType)
	{
		var dataLists = new List<int>();
		dataLists.Add(dataChangedType);

		Notify(dataLists);
	}

	public void Notify(List<int> dataChangedTypes)
	{
		foreach (var dataType in dataChangedTypes)
		{
			foreach (var listener in dic_dataDel)
			{
				if (listener.Key == dataType && listener.Value != null)
					listener.Value();
			}
		}
	}

	private static SysLocalDataBase sInst;
	public static SysLocalDataBase Inst
	{
		get { return SysModuleManager.Instance.GetSysModule<SysLocalDataBase>(); }
	}

	private LoginInfo loginInfo;
	public LoginInfo LoginInfo
	{
		get
		{
			if (loginInfo == null)
				loginInfo = new LoginInfo();

			return loginInfo;
		}
	}

	private Player localPlayer;
	public Player LocalPlayer
	{
		get { return localPlayer; }
		set { localPlayer = value; }
	}

	private Dictionary<int, List<Consumable>> lotteryRewardLists;
	public Dictionary<int, List<Consumable>> LotteryRewardLists
	{
		get { return lotteryRewardLists; }
		set { lotteryRewardLists = value; }
	}

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		this.RegisterDataChangedDel(IDSeg._AssetType.Special, RegistNotifyStamina);
		this.RegisterDataChangedDel(IDSeg._AssetType.Activity, RegistNotifyFixTimeActivity);

		return true;
	}

	public override void Dispose()
	{
		base.Dispose();

		ClearAllData(false);

		dic_dataDel.Clear();
	}

	public float GetBattleSpeed()
	{
		return LocalPlayer.BattleSpeed;
	}

	public void SetBattleSpeed(float speed)
	{
		LocalPlayer.BattleSpeed = speed;
	}

	public void ClearAllData(bool clearLogin)
	{
		if (clearLogin)
			loginInfo = null;

		localPlayer = null;
	}

	private float lastUpdateTime = 0;
	public override void OnUpdate()
	{
		if (localPlayer == null)
			return;

		// Update point
		localPlayer.Stamina.UpdatePoint(loginInfo.NowTime);
		localPlayer.QinInfoAnswerCount.UpdatePoint(loginInfo.NowTime);
		localPlayer.Energy.UpdatePoint(loginInfo.NowTime);

		if (Time.realtimeSinceStartup - lastUpdateTime < 1)
			return;

		lastUpdateTime = Time.realtimeSinceStartup;

		// Update reset times
		// Platform reset times
		Platform.Instance.UpdateResetTime();

		// Add stamina item
		if (TimeEx.GetTimeAfterTime(
			TimeEx.ToUTCDateTime(ConfigDatabase.DefaultCfg.GameConfig.restoreUseStaminaItemTime),
			loginInfo.ToServerDateTime(localPlayer.StaminaBuyCountLastResetTime),
			_TimeDurationType.Day) <= this.LoginInfo.NowDateTime)
		{
			localPlayer.StaminaBuyCountLastResetTime = TimeEx.DateTimeToInt64(this.LoginInfo.NowDateTime);
			localPlayer.StaminaBuyCount = 0;

			// Regist LocalNotification.
			RegistNotifyStamina();

			// Notice UI
		}

		//// Update Tavern free SumCount.
		//if (LocalPlayer.TavernQuick.MinTavernFreeLastStarTime > 0 && LocalPlayer.TavernQuick.MinTavernId != IDSeg.InvalidId)
		//{
		//    long nowTime = this.LoginInfo.NowTime;
		//    long endTime = LocalPlayer.TavernQuick.MinTavernFreeLastStarTime + 1000 * ConfigDatabase.DefaultCfg.TavernConfig.GetTavernById(LocalPlayer.TavernQuick.MinTavernId).freeRestoreTimeSecond;

		//    if (nowTime > endTime)
		//    {
		//        LocalPlayer.TavernQuick.MinTavernId = 0; ;
		//        LocalPlayer.TavernQuick.MinTavernFreeLastStarTime = 0;
		//        LocalPlayer.TavernQuick.SumTavernFreeCount++;
		//    }
		//}

		// Update QuestData.
		if (TimeEx.GetTimeAfterTime(
				TimeEx.ToUTCDateTime(ConfigDatabase.DefaultCfg.QuestConfig.dailyResetTime),
				loginInfo.ToServerDateTime(localPlayer.QuestData.QuestQuick.LastResetTime),
				_TimeDurationType.Day) <= this.LoginInfo.NowDateTime)
		{
			if (LocalPlayer.QuestData.Quests != null)
			{
				for (int i = 0; i < localPlayer.QuestData.Quests.Count; ++i)
				{
					var quest = localPlayer.QuestData.Quests[i];
					if (ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(quest.QuestId).resetType != _TimeDurationType.Day ||
						quest.Phase < QuestConfig._PhaseType.Active ||
						quest.Phase > QuestConfig._PhaseType.FinishedAndGotReward)
						continue;

					quest.CurrentStep = 0;
					quest.Phase = QuestConfig._PhaseType.Active;
				}
			}

			localPlayer.QuestData.QuestQuick.LastResetTime = TimeEx.DateTimeToInt64(this.LoginInfo.NowDateTime);
			localPlayer.QuestData.QuestQuick.CanPickDailyQuestsCount = 0;

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAssistantDailyTask)))
				SysUIEnv.Instance.GetUIModule<UIPnlAssistantDailyTask>().OnSyncUI(localPlayer.QuestData.Quests, null);

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAssistantFixedTask)))
				SysUIEnv.Instance.GetUIModule<UIPnlAssistantFixedTask>().OnSyncUI(localPlayer.QuestData.Quests, null);
		}

		// Update ServerReawrd DayCount.
		if (localPlayer.StartServerRewardInfo != null && localPlayer.StartServerRewardInfo.UnPickIds.Count > 0)
		{
			System.DateTime loginTime = this.loginInfo.ToServerDateTime(this.localPlayer.StartServerRewardInfo.LoginTime);
			System.DateTime endTime = TimeEx.GetTimeAfterTime(TimeEx.ToUTCDateTime(ConfigDatabase.DefaultCfg.StartServerRewardConfig.resetTime), loginTime, _TimeDurationType.Day);

			if (this.LoginInfo.NowDateTime > endTime)
			{
				this.localPlayer.StartServerRewardInfo.LoginTime = TimeEx.DateTimeToInt64(this.LoginInfo.NowDateTime);
				this.localPlayer.StartServerRewardInfo.DayCount++;
			}
		}

		//千机楼计时器
		if (SysGameStateMachine.Instance.CurrentState is GameState_Tower && SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().IsInTower)
		{
			System.DateTime loginTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
			System.DateTime endTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(SysLocalDataBase.Inst.localPlayer.MelaleucaFloorData.NextResetTime);

			if (loginTime > endTime && SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.NextResetTime != 0 &&
				!SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlTowerSweepBattle))
				RequestMgr.Inst.Request(new QueryMelaleucaFloorPlayerInfoReq());
		}

		// Update Diner Npc.
		bool hasDinerChanged = false;
		for (int i = localPlayer.HireDinerData.HireDiners.Count - 1; i >= 0; i--)
		{
			var hiredDiner = localPlayer.HireDinerData.HireDiners[i];

			if (hiredDiner.EndTime < this.loginInfo.NowTime)
			{
				hasDinerChanged = true;

				localPlayer.RemoveAvatar(hiredDiner.AvatarGuid);

				for (int j = 0; j < localPlayer.PositionData.Positions.Count; j++)
				{
					var position = localPlayer.PositionData.Positions[j];

					for (int k = 0; k < position.AvatarLocations.Count; k++)
					{
						if (position.AvatarLocations[k].Guid.Equals(hiredDiner.AvatarGuid))
						{
							position.AvatarLocations.RemoveAt(k);
							break;
						}
					}
				}

				localPlayer.HireDinerData.HireDiners.RemoveAt(i);
			}
		}

		if (hasDinerChanged)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
				SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().OnDinerAvatarFiredByTimes();
			else if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarDiner)))
			{
				var avatarDiner = SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>();

				if (avatarDiner.GetCurrentTabEnum() == UIPnlAvatarDiner.TabEnum.Dinered)
					SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>().OnFireNpcSuccess();
			}
		}
	}

	public void UpdateNotifyData(int assisantNum, List<com.kodgames.corgi.protocol.ActivityData> activityData, List<com.kodgames.corgi.protocol.State> functionStates, List<KodGames.ClientClass.Quest> changedQuests, KodGames.ClientClass.QuestQuick questQuick)
	{
		if (localPlayer == null)
			return;

		// 小助手数目
		if (assisantNum >= 0)
		{
			LocalPlayer.TaskData.NewTaskAmount = assisantNum;

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAssistantTask)))
				SysUIEnv.Instance.GetUIModule<UIPnlAssistantTask>().OnTaskChanged();
		}

		// 活动
		if (activityData != null && activityData.Count > 0)
		{
			// 活动信息修改
			ActivityManager.Instance.ResetData(activityData);

			// 活动数据改变,本地通知
			this.Notify(IDSeg._AssetType.Activity);
		}

		// 绿点推送
		if (functionStates != null && functionStates.Count > 0)
		{
			for (int i = 0; i < functionStates.Count; i++)
			{
				bool find = false;
				for (int j = 0; j < localPlayer.FunctionStates.Count; j++)
				{
					if (functionStates[i].id == localPlayer.FunctionStates[j].id)
					{
						find = true;
						localPlayer.FunctionStates[j].isOpen = functionStates[i].isOpen;
						break;
					}
				}

				if (!find)
				{
					var state = new com.kodgames.corgi.protocol.State();
					state.id = functionStates[i].id;
					state.isOpen = functionStates[i].isOpen;
					localPlayer.FunctionStates.Add(state);
				}
			}

			List<int> dataChanged = new List<int>();
			dataChanged.Add(IDSeg._AssetType.Unknown);

			SysLocalDataBase.Inst.Notify(dataChanged);
		}

		// 任务
		if (changedQuests != null && changedQuests.Count > 0)
		{
			// Update Quest Data.
			UpdateQuestData(changedQuests, true);

			//如果小助手的某个任务界面正在显示，更新任务进度。
			if (localPlayer.QuestData.Quests != null)
			{
				if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAssistantDailyTask))
					SysUIEnv.Instance.GetUIModule<UIPnlAssistantDailyTask>().OnSyncUI(changedQuests, null);

				if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAssistantFixedTask))
					SysUIEnv.Instance.GetUIModule<UIPnlAssistantFixedTask>().OnSyncUI(changedQuests, null);
			}
		}

		if (questQuick != null)
			// Update QuestQuick.
			SysLocalDataBase.Inst.LocalPlayer.QuestData.QuestQuick = questQuick;
	}

	public bool GetPlayerFunctionStatus(int type)
	{
		if (localPlayer == null)
			return false;

		foreach (var f in localPlayer.FunctionStates)
			if (f.id == type)
				return f.isOpen;

		return false;
	}

	#region LocalNotifications
	public void RegistrLocalNotify()
	{
		RegistNotifyStamina();
		RegistNotifyRecallPlayer();
		RegisterNotifyTravern();
	}

	private void RegistNotifyFixTimeActivity()
	{
		// 客栈本地推送
		SysNotification.Instance.CancelScheduleLocalNotification(LocalNotificationConfig._NotificationType.FixedTimeActivity);
		var activityFix = ActivityManager.Instance.GetActivity<ActivityFixedTime>();
		if (activityFix != null && activityFix.IsOpen)
		{
			foreach (var notifCfg in ConfigDatabase.DefaultCfg.LocalNotificationConfig.GetNotificationsByType(LocalNotificationConfig._NotificationType.FixedTimeActivity))
			{
				if (notifCfg.isOpen == false)
					continue;

				var startTimes = activityFix.ActivityInfo.GetTimersByStatus(_ActivityTimerStatus.Start);

				for (int index = 0; index < startTimes.Count; index++)
				{
					SysNotification.Instance.ScheduleLocalNotification(
						KodGames.TimeEx.ToLocalDataTime(startTimes[index].Timer).AddMilliseconds(notifCfg.delayTime),
						notifCfg.messageBody,
						notifCfg.appIconBadageNumber,
						notifCfg.hasAction,
						notifCfg.actionTitle,
						notifCfg.type,
						0,
						activityFix.RestTimeType);
				}
			}
		}
	}

	private void RegistNotifyStamina()
	{
		// 体力本地推送
		SysNotification.Instance.CancelScheduleLocalNotification(LocalNotificationConfig._NotificationType.Stamina);
		long staminaFullGenerateTime = localPlayer.Stamina.GetFullGenerationLeftTime(loginInfo.NowTime);
		if (staminaFullGenerateTime > 0)
		{
			foreach (var notifCfg in ConfigDatabase.DefaultCfg.LocalNotificationConfig.GetNotificationsByType(LocalNotificationConfig._NotificationType.Stamina))
			{
				if (notifCfg.isOpen == false)
					continue;

				SysNotification.Instance.ScheduleLocalNotification(
								KodGames.TimeEx.ToLocalDataTime(loginInfo.NowTime).AddMilliseconds(staminaFullGenerateTime).AddMilliseconds(notifCfg.delayTime),
								notifCfg.messageBody,
								notifCfg.appIconBadageNumber,
								notifCfg.hasAction,
								notifCfg.actionTitle,
								notifCfg.type,
								0,
								_TimeDurationType.Day);
			}
		}
	}

	private void RegistNotifyRecallPlayer()
	{
		// 玩家召回本地推送
		SysNotification.Instance.CancelScheduleLocalNotification(LocalNotificationConfig._NotificationType.RecallPlayer);
		foreach (var notifCfg in ConfigDatabase.DefaultCfg.LocalNotificationConfig.GetNotificationsByType(LocalNotificationConfig._NotificationType.RecallPlayer))
		{
			if (notifCfg.isOpen == false)
				continue;

			SysNotification.Instance.ScheduleLocalNotification(
							KodGames.TimeEx.ToLocalDataTime(loginInfo.NowTime).AddMilliseconds(notifCfg.delayTime),
							notifCfg.messageBody,
							notifCfg.appIconBadageNumber,
							notifCfg.hasAction,
							notifCfg.actionTitle,
							notifCfg.type,
							0,
							_TimeDurationType.Day);
		}
	}

	private void RegisterNotifyTravern()
	{

	}
	#endregion

	public int GetGoodsPriceAfterDiscount(int goodsId)
	{
		KodGames.ClientClass.Goods goods = localPlayer.ShopData.GetGoodsById(goodsId);
		if (goods == null)
			return 0;

		GoodConfig.Good goodsCfg = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goodsId);
		if (goodsCfg == null)
			return 0;

		return goods.Discount != 0 ? goods.Discount : goodsCfg.costs[0].count;
	}

	public void ProcessCostRewardSync(CostAndRewardAndSync costAndRewardAndSync, string srcReqName)
	{
		AddReward(costAndRewardAndSync.Reward, srcReqName);
		UseCost(costAndRewardAndSync.Costs, srcReqName);
	}

	public void RemoveSuperSkillInRewardForShow(KodGames.ClientClass.Reward reward)
	{
		if (reward == null || reward.Skill == null)
			return;

		for (int index = reward.Skill.Count - 1; index >= 0; index--)
		{
			if (ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(reward.Skill[index].ResourceId).type != CombatTurn._Type.PassiveSkill)
				reward.Skill.RemoveAt(index);
		}
	}

	#region Cost
	/// <summary>
	/// Reduce cost from local player
	/// </summary>	
	private void UseCost(List<KodGames.ClientClass.Cost> costs, string srcReqName)
	{
		if (localPlayer == null || costs == null)
			return;

		LocalSet<int> dataChangeds = new LocalSet<int>();

		foreach (var cost in costs)
		{
			switch (IDSeg.ToAssetType(cost.Id))
			{
				case IDSeg._AssetType.Avatar:
					localPlayer.RemoveAvatar(cost.Guid);

					// Add To Data Changed.
					dataChangeds.Add(IDSeg._AssetType.Avatar);
					break;

				case IDSeg._AssetType.Equipment:
					localPlayer.RemoveEquipment(cost.Guid);

					// Add To Data Changed.
					dataChangeds.Add(IDSeg._AssetType.Equipment);
					break;
				case IDSeg._AssetType.CombatTurn:
					localPlayer.RemoveSkill(cost.Guid);

					// Add To Data Changed.
					dataChangeds.Add(IDSeg._AssetType.CombatTurn);
					break;

				case IDSeg._AssetType.Dan:
					localPlayer.RemoveDan(cost.Guid);

					// Add To Data Change.
					dataChangeds.Add(IDSeg._AssetType.Dan);
					break;

				case IDSeg._AssetType.Item:
					localPlayer.RemoveConsumable(cost.Id, cost.Count);

					// Add To Data Changed.
					dataChangeds.Add(IDSeg._AssetType.Item);
					break;

				case IDSeg._AssetType.Tavern:
					if (SysLocalDataBase.Inst.LocalPlayer.ShopData.TavernInfos != null)
					{
						var tavernInfo = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetTavernInfoById(cost.Id);
						if (tavernInfo != null)
						{
							tavernInfo.leftFreeCount -= cost.Count;

							// Add To Data Changed.
							dataChangeds.Add(IDSeg._AssetType.Tavern);
						}
					}
					break;

				case IDSeg._AssetType.Special:
					// Add To Data Changed.
					dataChangeds.Add(IDSeg._AssetType.Special);

					switch (cost.Id)
					{
						case IDSeg._SpecialId.GameMoney:
							localPlayer.GameMoney -= cost.Count;
							break;
						case IDSeg._SpecialId.RealMoney:
							localPlayer.RealMoney -= cost.Count;

							// Talking Game RealMoney Used.
							GameAnalyticsUtility.OnPurchase(srcReqName, 1, cost.Count);

							break;
						case IDSeg._SpecialId.Experience: break;
						case IDSeg._SpecialId.Stamina:
							localPlayer.Stamina.ModifyPoint(-cost.Count, loginInfo.NowTime);
							break;
						case IDSeg._SpecialId.Energy:
							localPlayer.Energy.ModifyPoint(-cost.Count, loginInfo.NowTime);
							break;
						case IDSeg._SpecialId.WorldChatTimes:
							localPlayer.CurrentChatCount += cost.Count;
							break;
						case IDSeg._SpecialId.ArenaHonorPoint:
							if (localPlayer.ArenaData != null)
								localPlayer.ArenaData.HonorPoint -= cost.Count;
							break;
						case IDSeg._SpecialId.ArenaChallengeTimes:
							if (localPlayer.ArenaData != null)
								localPlayer.ArenaData.ChallengePoint += cost.Count;
							break; // Add						
						case IDSeg._SpecialId.Badge:
							localPlayer.Badge -= cost.Count;
							break;
						case IDSeg._SpecialId.Soul:
							localPlayer.Soul -= cost.Count;
							break;
						case IDSeg._SpecialId.VipLevel: break;
						//case IDSeg._SpecialId.UseItemCount_AddStamina:
						case IDSeg._SpecialId.StaminaBuyCount:
							localPlayer.StaminaBuyCount += cost.Count;
							break; // Add 
						case IDSeg._SpecialId.Spirit:
							localPlayer.Spirit -= cost.Count;
							break;
						case IDSeg._SpecialId.Iron:
							localPlayer.Iron -= cost.Count;
							break;
						case IDSeg._SpecialId.TrialStamp:
							localPlayer.TrialStamp -= cost.Count;
							break;
						case IDSeg._SpecialId.QinInfoAnswerCount:
							localPlayer.QinInfoAnswerCount.ModifyPoint(-cost.Count, loginInfo.NowTime);
							break;
						case IDSeg._SpecialId.Medals:
							localPlayer.Medals -= cost.Count;
							break;
						case IDSeg._SpecialId.WineSoul:
							localPlayer.WineSoul -= cost.Count;
							break;
						case IDSeg._SpecialId.GuildMoney:
							localPlayer.GuildMoney -= cost.Count;
							break;
						case IDSeg._SpecialId.GuildBossCount:
							localPlayer.GuildBossCount -= cost.Count;
							break;
					}
					break;
			}
		}

		// Notify DataChanged.
		Notify(dataChangeds.ToList());
	}

	public static List<Pair<int, int>> ConvertIdCountList(List<KodGames.ClientClass.Cost> costs)
	{
		var retList = new List<Pair<int, int>>();
		foreach (KodGames.ClientClass.Cost cost in costs)
			retList.Add(new Pair<int, int>(cost.Id, cost.Count));

		return retList;
	}

	public static List<Pair<int, int>> ConvertIdCountList(List<ClientServerCommon.Cost> costs)
	{
		var retList = new List<Pair<int, int>>();
		foreach (var cost in costs)
			retList.Add(new Pair<int, int>(cost.id, cost.count));

		return retList;
	}

	public static string GetCostsDesc(List<Pair<int, int>> costColls, bool hasPreDesc, bool combineNameAndCount)
	{
		if (costColls.Count == 0)
			return "";

		var rewardDesc = new System.Text.StringBuilder();

		if (hasPreDesc)
			rewardDesc.Append(GameUtility.GetUIString("UI_Costs"));

		foreach (var kvp in costColls)
		{
			if (combineNameAndCount && kvp.second > 0)
				rewardDesc.Append(string.Format("{0}{1}", ItemInfoUtility.GetAssetName(kvp.first), kvp.second));
			else if (kvp.second > 0)
				rewardDesc.Append(GameUtility.FormatUIString("UI_Costs_Item", ItemInfoUtility.GetAssetName(kvp.first), kvp.second));
		}

		return rewardDesc.ToString();
	}

	public static string GetCostsDesc(List<ClientServerCommon.Cost> costs)
	{
		return GetCostsDesc(ConvertIdCountList(costs), true, false);
	}

	public static string GetCostsDesc(List<KodGames.ClientClass.Cost> costs)
	{
		return GetCostsDesc(ConvertIdCountList(costs), true, false);
	}
	#endregion

	#region Reward
	/// <summary>
	/// Add reward to local player
	/// </summary>	
	private void AddReward(KodGames.ClientClass.Reward reward, string srcReqName)
	{
		if (localPlayer == null || reward == null)
			return;

		LocalSet<int> dataChangeds = new LocalSet<int>();

		foreach (KodGames.ClientClass.Avatar avatar in reward.Avatar)
		{
			localPlayer.AddAvatar(avatar);

			// Add To Data Changed.
			dataChangeds.Add(IDSeg._AssetType.Avatar);
		}

		foreach (KodGames.ClientClass.Equipment equip in reward.Equip)
		{
			localPlayer.AddEquipment(equip);

			// Add To Data Changed.
			dataChangeds.Add(IDSeg._AssetType.Equipment);
		}

		foreach (KodGames.ClientClass.Skill skill in reward.Skill)
		{
			localPlayer.AddSkill(skill);

			// Add To Data Changed.
			dataChangeds.Add(IDSeg._AssetType.CombatTurn);
		}

		foreach (KodGames.ClientClass.Dan dan in reward.Dan)
		{
			localPlayer.AddDan(dan);

			dataChangeds.Add(IDSeg._AssetType.Dan);
		}

		foreach (KodGames.ClientClass.Beast beast in reward.Beast)
		{
			localPlayer.AddBeast(beast);
			dataChangeds.Add(IDSeg._AssetType.Beast);
		}		

		foreach (KodGames.ClientClass.Consumable consumable in reward.Consumable)
		{
			switch (IDSeg.ToAssetType(consumable.Id))
			{
				case IDSeg._AssetType.Item:

					//if (ItemConfig._Type.ToItemType(consumable.Id) == ItemConfig._Type.BeastScroll)
					//{
					//    BeastConfig.BaseInfo baseInfo= ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastFragmentId(consumable.Id);
					//    SysUIEnv.Instance.ShowUIModule(typeof(UIDlgOrganGet), baseInfo, reward.IsBeastDecomposed);
					//}

					localPlayer.AddConsumable(consumable.Id, consumable.Amount);
					// Add To Data Changed.
					dataChangeds.Add(IDSeg._AssetType.Item);
					break;

				case IDSeg._AssetType.Tavern:
					com.kodgames.corgi.protocol.TavernInfo tavernInfo = localPlayer.ShopData.GetTavernInfoById(consumable.Id);
					if (tavernInfo != null)
					{
						tavernInfo.leftFreeCount += consumable.Amount;

						// Add To Data Changed.
						dataChangeds.Add(IDSeg._AssetType.Tavern);
					}

					break;				

				case IDSeg._AssetType.Special:

					// Add To Data Changed.
					dataChangeds.Add(IDSeg._AssetType.Special);

					switch (consumable.Id)
					{
						case IDSeg._SpecialId.GameMoney:
							localPlayer.GameMoney += consumable.Amount;
							break;
						case IDSeg._SpecialId.RealMoney:
							{
								localPlayer.RealMoney += consumable.Amount;

								// Talking Game Get RealMoney.
								GameAnalyticsUtility.OnReward(consumable.Amount, srcReqName);
							}
							break;
						case IDSeg._SpecialId.Experience:
							AddExpriences(localPlayer, consumable.Amount);
							break;
						case IDSeg._SpecialId.Stamina:
							localPlayer.Stamina.ModifyPoint(consumable.Amount, this.LoginInfo.NowTime);
							break;
						case IDSeg._SpecialId.Energy:
							localPlayer.Energy.ModifyPoint(consumable.Amount, this.LoginInfo.NowTime);
							break;
						case IDSeg._SpecialId.WorldChatTimes:
							localPlayer.CurrentChatCount += consumable.Amount;
							break;
						case IDSeg._SpecialId.ArenaHonorPoint:
							if (localPlayer.ArenaData != null)
								localPlayer.ArenaData.HonorPoint += consumable.Amount;
							break;
						case IDSeg._SpecialId.ArenaChallengeTimes:
							if (localPlayer.ArenaData != null)
								localPlayer.ArenaData.ChallengePoint -= consumable.Amount;
							break; // Sub
						case IDSeg._SpecialId.Badge:
							localPlayer.Badge += consumable.Amount;
							break;
						case IDSeg._SpecialId.Soul:
							localPlayer.Soul += consumable.Amount;
							break;
						case IDSeg._SpecialId.VipLevel:
							localPlayer.VipLevel = Mathf.Max(localPlayer.VipLevel, consumable.Amount);
							UpdateIncreaseDataMaxValue();
							break;
						case IDSeg._SpecialId.UseItemCount_AddStamina:
							localPlayer.StaminaBuyCount += consumable.Amount;
							break; // Sub
						case IDSeg._SpecialId.Spirit:
							localPlayer.Spirit += consumable.Amount;
							break;
						case IDSeg._SpecialId.Iron:
							localPlayer.Iron += consumable.Amount;
							break;
						case IDSeg._SpecialId.TrialStamp:
							localPlayer.TrialStamp += consumable.Amount;
							break;
						case IDSeg._SpecialId.Medals:
							localPlayer.Medals += consumable.Amount;
							break;
						case IDSeg._SpecialId.QinInfoAnswerCount:
							localPlayer.QinInfoAnswerCount.ModifyPoint(consumable.Amount, this.LoginInfo.NowTime);
							break;
						case IDSeg._SpecialId.WineSoul:
							localPlayer.WineSoul += consumable.Amount;
							break;
						case IDSeg._SpecialId.Zentia:
							localPlayer.Zentia += consumable.Amount;
							break;
						case IDSeg._SpecialId.GuildMoney:
							localPlayer.GuildMoney += consumable.Amount;
							break;
						case IDSeg._SpecialId.GuildBossCount:
							localPlayer.GuildBossCount += consumable.Amount;
							break;						
					}
					break;
			}
		}

		// Skip Super skill in Reward for Show.
		RemoveSuperSkillInRewardForShow(reward);

		// Notify DataChanged.
		Notify(dataChangeds.ToList());
	}

	private static void AddExpriences(KodGames.ClientClass.Player player, int expriences)
	{
		player.LevelAttrib.Experience += expriences;

		for (int index = player.LevelAttrib.Level; index <= ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel; index++)
		{
			int upgrateExp = ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(index).playerExp;
			if (player.LevelAttrib.Experience >= upgrateExp)
			{
				if (player.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel)
				{
					break;
				}
				player.LevelAttrib.Level += 1;
				player.LevelAttrib.Experience -= upgrateExp;

				if (SysLocalDataBase.Inst != null && SysLocalDataBase.Inst.LoginInfo != null && SysLocalDataBase.Inst.LocalPlayer != null)
				{
					GameAnalyticsUtility.SetTDGAAccount(SysLocalDataBase.Inst.LoginInfo, player.LevelAttrib.Level);
				}

				// 等级变化埋点
				SysGameAnalytics.Instance.RecordGameData(GameRecordType.SetPlayerLevel);
			}
			else
			{
				break;
			}
		}

		SysLocalDataBase.Inst.CheckQueryLevelUpReward();
	}

	public void UpdateIncreaseDataMaxValue()
	{
		if (localPlayer == null)
			return;

		localPlayer.Stamina.UpdateMaxPoint(ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(localPlayer.VipLevel, VipConfig._VipLimitType.MaxStamina),
			LoginInfo.NowTime);
	}

	public static void CombineReward(ref List<KodGames.Pair<int, int>> rewards, KodGames.ClientClass.Reward reward)
	{
		if (reward == null)
			return;

		foreach (var keyValue in SysLocalDataBase.ConvertIdCountList(reward))
		{
			if (IDSeg.ToAssetType(keyValue.first) == IDSeg._AssetType.Item || IDSeg.ToAssetType(keyValue.first) == IDSeg._AssetType.Special)
			{
				int i = 0;
				for (; i < rewards.Count; ++i)
					if (rewards[i].first == keyValue.first)
					{
						var item = rewards[i];
						item.second += keyValue.second;
						rewards[i] = item;
						break;
					}

				if (i >= rewards.Count)
				{
					rewards.Add(new KodGames.Pair<int, int>(keyValue.first, keyValue.second));
				}
			}
			else
				rewards.Add(new KodGames.Pair<int, int>(keyValue.first, keyValue.second));
		}
	}

	public static List<Pair<int, int>> ConvertIdCountList(KodGames.ClientClass.Reward reward)
	{
		return ConvertIdCountList(reward, false);
	}

	public static List<Pair<int, int>> ConvertIdCountList(KodGames.ClientClass.Reward reward, bool combineRepeated)
	{
		var retList = new List<Pair<int, int>>();
		if (reward == null)
			return retList;

		foreach (var avatar in reward.Avatar)
			AddToCountList(retList, avatar.ResourceId, 1, combineRepeated);

		foreach (var equip in reward.Equip)
			AddToCountList(retList, equip.ResourceId, 1, combineRepeated);

		foreach (var skill in reward.Skill)
			if (ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).type == CombatTurn._Type.PassiveSkill)
				AddToCountList(retList, skill.ResourceId, 1, combineRepeated);

		foreach (var consumable in reward.Consumable)
			AddToCountList(retList, consumable.Id, consumable.Amount, combineRepeated);

		foreach (var dan in reward.Dan)
			AddToCountList(retList, dan.ResourceId, 1, combineRepeated);

		return retList;
	}

	private static void AddToCountList(List<Pair<int, int>> list, int id, int count, bool combineRepeated)
	{
		if (combineRepeated)
			for (int i = 0; i < list.Count; ++i)
				if (list[i].first == id)
				{
					var item = list[i];
					item.second += count;
					list[i] = item;
					return;
				}

		list.Add(new Pair<int, int>(id, count));
	}

	public static List<Pair<object, int>> ConvertObjCountList(KodGames.ClientClass.Reward reward)
	{
		var retList = new List<Pair<object, int>>();
		if (reward == null)
			return retList;

		foreach (var avatar in reward.Avatar)
			retList.Add(new Pair<object, int>(avatar, 1));

		foreach (var equip in reward.Equip)
			retList.Add(new Pair<object, int>(equip, 1));

		foreach (var skill in reward.Skill)
			if (ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).type == CombatTurn._Type.PassiveSkill)
				retList.Add(new Pair<object, int>(skill, 1));

		foreach (var consumable in reward.Consumable)
			retList.Add(new Pair<object, int>(consumable, consumable.Amount));

		foreach (var dan in reward.Dan)
			retList.Add(new Pair<object, int>(dan, 1));

		return retList;
	}

	public static string GetRewardDesc(KodGames.ClientClass.Reward reward, bool hasRewardTitle)
	{
		return GetRewardDesc(reward, hasRewardTitle, false, false);
	}

	public static string GetRewardDesc(KodGames.ClientClass.Reward reward, bool hasRewardTitle, bool hideCountLessThanOne)
	{
		return GetRewardDesc(reward, hasRewardTitle, hideCountLessThanOne, false);
	}

	public static string GetRewardDesc(KodGames.ClientClass.Reward reward, bool hasRewardTitle, bool hideCountLessThanOne, bool oneLineForOneReward)
	{
		return GetRewardDesc(CCRewardToCSCReward(reward, true), hasRewardTitle, hideCountLessThanOne, oneLineForOneReward);
	}

	public static string GetRewardDesc(List<ClientServerCommon.Reward> rewardColls, bool hasRewardTitle, bool hideCountLessThanOne, bool oneLineForOneReward)
	{
		if (rewardColls == null && rewardColls.Count <= 0)
			return "";

		var rewardDesc = new System.Text.StringBuilder();

		if (hasRewardTitle)
		{
			if (oneLineForOneReward)
				rewardDesc.Append(GameUtility.GetUIString("UI_Reward") + "\n");
			else
				rewardDesc.Append(GameUtility.GetUIString("UI_Reward"));
		}

		foreach (var kvp in rewardColls)
		{
			string strToAdd = "";

			if (kvp.id == IDSeg._SpecialId.VipLevel)
				strToAdd = GameUtility.FormatUIString("UI_Reward_Item_VIP", kvp.count);
			else if (hideCountLessThanOne && kvp.count <= 1)
				strToAdd = GameUtility.FormatUIString("UI_Reward_Item_NoCount", ItemInfoUtility.GetAssetName(kvp.id, kvp.breakthoughtLevel));
			else
				strToAdd = GameUtility.FormatUIString("UI_Reward_Item", ItemInfoUtility.GetAssetName(kvp.id), kvp.count);

			switch (IDSeg.ToAssetType(kvp.id))
			{
				case IDSeg._AssetType.Avatar:
					if (kvp.breakthoughtLevel > 0)
						strToAdd = GameUtility.FormatUIString("UI_Reward_Item_BreakLvAvatar", kvp.breakthoughtLevel, strToAdd);
					break;

				case IDSeg._AssetType.Equipment:
					if (kvp.breakthoughtLevel > 0)
						strToAdd = GameUtility.FormatUIString("UI_Reward_Item_BreakLvEquip", kvp.breakthoughtLevel, strToAdd);
					break;
			}

			if (oneLineForOneReward && rewardColls.IndexOf(kvp) != rewardColls.Count - 1)
				strToAdd += "\n";

			rewardDesc.Append(strToAdd);
		}

		return rewardDesc.ToString();
	}

	public static string GetRewardFormatDesc(KodGames.ClientClass.Reward reward)
	{
		List<Pair<int, int>> rewardColls = ConvertIdCountList(reward, true);

		if (rewardColls.Count == 0)
			return "";

		System.Text.StringBuilder rewardDesc = new System.Text.StringBuilder();
		rewardDesc.Append(GameUtility.GetUIString("UI_Reward") + "\n");
		foreach (var kvp in rewardColls)
		{
			rewardDesc.AppendFormat(GameUtility.GetUIString("UI_Reward_Item"), ItemInfoUtility.GetAssetName(kvp.first), kvp.second);
			rewardDesc.Append("\n");
		}

		return rewardDesc.ToString();
	}

	public static int GetFirstAssetId(KodGames.ClientClass.Reward reward)
	{
		if (reward.Avatar.Count != 0)
			return reward.Avatar[0].ResourceId;

		if (reward.Equip.Count != 0)
			return reward.Equip[0].ResourceId;

		if (reward.Skill.Count != 0)
			return reward.Skill[0].ResourceId;

		if (reward.Consumable.Count != 0)
			return reward.Consumable[0].Id;

		if (reward.Dan.Count != 0)
			return reward.Dan[0].ResourceId;

		return IDSeg.InvalidId;
	}


	public static List<ClientServerCommon.Reward> CCRewardToCSCReward(KodGames.ClientClass.Reward reward)
	{
		return CCRewardToCSCReward(reward, false);
	}

	public static List<ClientServerCommon.Reward> CCRewardToCSCReward(KodGames.ClientClass.Reward reward, bool mergeSame)
	{
		List<ClientServerCommon.Reward> result = new List<ClientServerCommon.Reward>();

		if (reward == null)
			return result;

		Dictionary<int, int> id_index = new Dictionary<int, int>();
		foreach (var a in reward.Avatar)
		{
			ClientServerCommon.Reward rwd = new ClientServerCommon.Reward();
			rwd.count = 1;
			rwd.breakthoughtLevel = a.BreakthoughtLevel;
			rwd.level = a.LevelAttrib.Level;
			rwd.id = a.ResourceId;

			if (mergeSame)
			{
				if (id_index.ContainsKey(rwd.id))
					result[id_index[rwd.id]].count += 1;
				else
				{
					id_index.Add(rwd.id, result.Count);
					result.Add(rwd);
				}
			}
			else
				result.Add(rwd);
		}

		foreach (var e in reward.Equip)
		{
			ClientServerCommon.Reward rwd = new ClientServerCommon.Reward();
			rwd.count = 1;
			rwd.breakthoughtLevel = e.BreakthoughtLevel;
			rwd.level = e.LevelAttrib.Level;
			rwd.id = e.ResourceId;

			if (mergeSame)
			{
				if (id_index.ContainsKey(rwd.id))
					result[id_index[rwd.id]].count += 1;
				else
				{
					id_index.Add(rwd.id, result.Count);
					result.Add(rwd);
				}
			}
			else
				result.Add(rwd);
		}

		foreach (var s in reward.Skill)
		{
			if (ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s.ResourceId).type == CombatTurn._Type.PassiveSkill)
			{
				ClientServerCommon.Reward rwd = new ClientServerCommon.Reward();
				rwd.count = 1;
				rwd.level = s.LevelAttrib.Level;
				rwd.id = s.ResourceId;

				if (mergeSame)
				{
					if (id_index.ContainsKey(rwd.id))
						result[id_index[rwd.id]].count += 1;
					else
					{
						id_index.Add(rwd.id, result.Count);
						result.Add(rwd);
					}
				}
				else
					result.Add(rwd);
			}
		}

		foreach (var c in reward.Consumable)
		{
			if (ItemConfig._Type.ToItemType(c.Id) == ItemConfig._Type.TelFee)
				continue;

			ClientServerCommon.Reward rwd = new ClientServerCommon.Reward();
			rwd.count = c.Amount;
			rwd.id = c.Id;

			if (mergeSame)
			{
				if (id_index.ContainsKey(rwd.id))
					result[id_index[rwd.id]].count += 1;
				else
				{
					id_index.Add(rwd.id, result.Count);
					result.Add(rwd);
				}
			}
			else
				result.Add(rwd);
		}

		foreach (var d in reward.Dan)
		{
			ClientServerCommon.Reward rwd = new ClientServerCommon.Reward();

			rwd.count = 1;
			rwd.breakthoughtLevel = d.BreakthoughtLevel;
			rwd.level = d.LevelAttrib.Level;
			rwd.id = d.ResourceId;

			if (mergeSame)
			{
				if (id_index.ContainsKey(rwd.id))
					result[id_index[rwd.id]].count += 1;
				else
				{
					id_index.Add(rwd.id, result.Count);
					result.Add(rwd);
				}
			}
			else
				result.Add(rwd);
		}

		return result;
	}

	public static List<com.kodgames.corgi.protocol.ShowReward> CCRewardListToShowReward(KodGames.ClientClass.Reward reward)
	{
		List<com.kodgames.corgi.protocol.ShowReward> result = new List<com.kodgames.corgi.protocol.ShowReward>();

		if (reward == null)
			return result;

		foreach (var a in reward.Avatar)
		{
			com.kodgames.corgi.protocol.ShowReward rwd = new com.kodgames.corgi.protocol.ShowReward();
			rwd.count = 1;
			rwd.breakthought = a.BreakthoughtLevel;
			rwd.level = a.LevelAttrib.Level;
			rwd.id = a.ResourceId;

			result.Add(rwd);
		}

		foreach (var e in reward.Equip)
		{
			com.kodgames.corgi.protocol.ShowReward rwd = new com.kodgames.corgi.protocol.ShowReward();
			rwd.count = 1;
			rwd.breakthought = e.BreakthoughtLevel;
			rwd.level = e.LevelAttrib.Level;
			rwd.id = e.ResourceId;

			result.Add(rwd);
		}

		foreach (var s in reward.Skill)
		{
			if (ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(s.ResourceId).type == CombatTurn._Type.PassiveSkill)
			{
				com.kodgames.corgi.protocol.ShowReward rwd = new com.kodgames.corgi.protocol.ShowReward();
				rwd.count = 1;
				rwd.level = s.LevelAttrib.Level;
				rwd.id = s.ResourceId;

				result.Add(rwd);
			}
		}

		foreach (var d in reward.Dan)
		{
			com.kodgames.corgi.protocol.ShowReward rwd = new com.kodgames.corgi.protocol.ShowReward();

			rwd.count = 1;
			rwd.breakthought = d.BreakthoughtLevel;
			rwd.level = d.LevelAttrib.Level;
			rwd.attributeIds.AddRange(d.AttributeIds);

			//ClinetClass To Proto Struct
			foreach (var attributeGroup in d.DanAttributeGroups)
			{
				var danAttributeGroup = new com.kodgames.corgi.protocol.DanAttributeGroup();
				danAttributeGroup.attributeDesc = attributeGroup.AttributeDesc;

				danAttributeGroup.id = attributeGroup.Id;

				foreach (var attribute in attributeGroup.DanAttributes)
				{
					var danAttribute = new com.kodgames.corgi.protocol.DanAttribute();
					danAttribute.id = attribute.Id;

					foreach (var p in attribute.PropertyModifierSets)
					{
						var danProperty = new com.kodgames.corgi.protocol.PropertyModifierSet();
						danProperty.levelFilter = p.Level;

						foreach (var m in p.Modifiers)
						{
							var danModifier = new com.kodgames.corgi.protocol.PropertyModifier();

							danModifier.modifyType = m.ModifyType;
							danModifier.type = m.Type;
							danModifier.attributeType = m.AttributeType;
							danModifier.attributeValue = m.AttributeValue;

							danProperty.modifiers.Add(danModifier);
						}

						danAttribute.propertyModifierSets.Add(danProperty);
					}

					danAttributeGroup.danAttributes.Add(danAttribute);
				}

				rwd.danAttributeGroups.Add(danAttributeGroup);
			}

			//rwd.danAttributeGroups.AddRange(d.DanAttributeGroups);
			rwd.id = d.ResourceId;

			result.Add(rwd);
		}

		foreach (var c in reward.Consumable)
		{
			if (ItemConfig._Type.ToItemType(c.Id) == ItemConfig._Type.TelFee)
				continue;

			com.kodgames.corgi.protocol.ShowReward rwd = new com.kodgames.corgi.protocol.ShowReward();
			rwd.count = c.Amount;
			rwd.id = c.Id;

			result.Add(rwd);
		}

		return result;
	}

	public static List<ClientServerCommon.Reward> CCRewardListToCSCReward(List<KodGames.ClientClass.Reward> rewards)
	{
		return CCRewardListToCSCReward(rewards, false);
	}

	public static List<ClientServerCommon.Reward> CCRewardListToCSCReward(List<KodGames.ClientClass.Reward> rewards, bool mergeSame)
	{
		KodGames.ClientClass.Reward tmpReward = new KodGames.ClientClass.Reward();

		foreach (var rwd in rewards)
		{
			foreach (var a in rwd.Avatar)
				tmpReward.Avatar.Add(a);

			foreach (var e in rwd.Equip)
				tmpReward.Equip.Add(e);

			foreach (var s in rwd.Skill)
				tmpReward.Skill.Add(s);

			foreach (var c in rwd.Consumable)
				tmpReward.Consumable.Add(c);

			foreach (var d in rwd.Dan)
				tmpReward.Dan.Add(d);
		}

		return CCRewardToCSCReward(tmpReward, mergeSame);
	}

	#endregion

	public void CheckQueryLevelUpReward()
	{
		if (localPlayer.CurrentPickedLevel > localPlayer.LevelAttrib.Level)
			Debug.LogError("invalid picked level");
		else if (localPlayer.CurrentPickedLevel < localPlayer.LevelAttrib.Level)
			RequestMgr.Inst.Request(new GetLevelUpRewardReq(localPlayer.LevelAttrib.Level));
	}

	public void UpdateMassages(List<com.kodgames.corgi.protocol.ChatMessage> msgs)
	{
		if (msgs.Count <= 0)
			return;

		msgs.Sort(DataCompare.CompareChageMessage);

		KodGames.ClientClass.MsgFlowData msgFlowData = SysLocalDataBase.Inst.LocalPlayer.MsgFlowData;
		msgFlowData.showMessage = msgs[msgs.Count - 1];
		msgFlowData.messages = msgs;
		msgFlowData.msg_ShowTimeMap = new List<com.kodgames.corgi.protocol.ChatMessage>();
	}

	public void UpdateQuestData(List<KodGames.ClientClass.Quest> quests, bool syncData)
	{
		if (quests == null)
			return;

		if (syncData && localPlayer.QuestData.Quests == null)
			return;

		if (localPlayer.QuestData.Quests == null)
			localPlayer.QuestData.Quests = quests;
		else
		{
			List<KodGames.ClientClass.Quest> deleteQuestDatas = new List<KodGames.ClientClass.Quest>();

			foreach (var quest in quests)
			{
				KodGames.ClientClass.Quest tempQuest = localPlayer.QuestData.GetQuestByQuestID(quest.QuestId);

				bool isQuestOpened = quest.Phase >= QuestConfig._PhaseType.Active && quest.Phase <= QuestConfig._PhaseType.FinishedAndGotReward;

				if (tempQuest == null)
				{
					if (isQuestOpened)
						localPlayer.QuestData.Quests.Add(quest);
				}
				else
				{
					if (isQuestOpened)
					{
						tempQuest.Phase = quest.Phase;
						tempQuest.CurrentStep = quest.CurrentStep;
					}
					else
						deleteQuestDatas.Add(tempQuest);
				}
			}

			foreach (var deleteQuestData in deleteQuestDatas)
				localPlayer.QuestData.Quests.Remove(deleteQuestData);
		}

		localPlayer.QuestData.Quests.Sort(DataCompare.CompareQuestData);

		// Talking Game Quest.
		GameAnalyticsUtility.OnMission(quests);
	}
}