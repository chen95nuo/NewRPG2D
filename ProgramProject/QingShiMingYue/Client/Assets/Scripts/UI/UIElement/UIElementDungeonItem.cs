using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElementDungeonItem : MonoBehaviour
{
	private class EasingData
	{
		public float timer;
		public float duration;
		public EZAnimation.Interpolator interpolator;
		public float distance;
	}

	protected enum ContinueCombatType
	{
		Combat,
		ClearCD,
	}

	public class CombatData
	{
		public int zoneID;
		public int dungeonID;
		public int positionId;
		public KodGames.ClientClass.RecruiteNpc recruiteNpc;
		public float uiDungeonMapPosition;
	}

	public AutoSpriteControlBase bgButton;
	public AutoSpriteControlBase diffChooseBtn;
	public AutoSpriteControlBase OperatorBase;
	public AutoSpriteControlBase continueCombatBase;
	public AutoSpriteControlBase combatBase;
	public UIChildLayoutControl operatorControll;
	public List<UIElemDungeonStarRewardItem> starRewardIcons;
	public GameObjectPool mapIconPool;
	public GameObjectPool mapShopPool;
	public GameObjectPool linePool;
	public GameObject mapAnimation;
	public float autoScrollDuration = 1f;
	public float animationDely = 0.5f;
	public float animationDuration = 1f;
	public EZAnimation.EASING_TYPE mapEasingType;
	public EZAnimation.EASING_TYPE animationEasingType;

	private int zoneId;

	public int ZoneId
	{
		get { return zoneId; }
	}

	private int currentDiffcultTab;
	public int CurrentDiffcultTab
	{
		get { return currentDiffcultTab; }
	}

	private List<UIElemDungeonMapIcon> mapIcons = new List<UIElemDungeonMapIcon>();
	private List<UIElemDungeonShopIcon> shopIcons = new List<UIElemDungeonShopIcon>();
	private List<UIElementDungeonLineItem> mapLines = new List<UIElementDungeonLineItem>();
	private EasingData taretDistanceEasingData;
	private float targetDistance = 0f;
	private int jumpDungeonId;
	private UIScrollList scrollList;
	private float prevScrollPosition = -1;
	private int currentIndex = -1;
	private bool isAnimationPlay = false;


	public void ClearData()
	{
		this.scrollList = null;
		this.currentIndex = 0;
		this.prevScrollPosition = -1;
		this.zoneId = 0;
		this.jumpDungeonId = 0;
		this.isAnimationPlay = false;
	}

	public void SetData(UIPnlCampaignBase.CampaignRecrod record, UIScrollList scrollList)
	{
		this.scrollList = scrollList;
		this.prevScrollPosition = record.dungeonScrollPosition;
		this.zoneId = record.jumpZoneId;
		this.jumpDungeonId = record.jumpDungeonId;

		// Hide difficult UI.
		diffChooseBtn.Hide(true);

		// InActive map operator button.
		OperatorBase.gameObject.SetActive(false);

		// InActive mapAnimation.
		mapAnimation.SetActive(false);

		// Init currentDiffcult : set the currentDiffcult base on the lastNormalBattleDungeonId , set the diffcultTab.
		currentDiffcultTab = record.GetDungeonDiffType() != _DungeonDifficulity.Unknow ? record.GetDungeonDiffType() : CampaignData.GetCampaignDiffTabState(zoneId);

		// Set the diffcultTab view.
		ChangeDungeonTab(currentDiffcultTab);
	}

	public void ChangeDungeonTab()
	{
		ChangeDungeonTab(currentDiffcultTab);
	}

	public void ChangeDungeonTab(int dungeonTab)
	{
		this.currentDiffcultTab = dungeonTab;

		// Set diffcultButton Text.
		SetDungeonDiffButtonText();

		// Set StarReward UI.
		SetDungeonStarRewardUI();

		// Save Tab Changed.
		CampaignData.SetCampaignDiffTabState(this.zoneId, this.currentDiffcultTab);

		// Reset View.
		SetDungeonDataView();
	}

	private void SetDungeonDiffButtonText()
	{
		var hardDiff = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(this.zoneId).GetDungeonDifficultyByDifficulty(_DungeonDifficulity.Hard);
		bool canShowDiff = hardDiff != null && hardDiff.dungeons != null && hardDiff.dungeons.Count > 0;

		diffChooseBtn.Hide(!canShowDiff);

		if (canShowDiff)
		{
			switch (this.currentDiffcultTab)
			{
				case _DungeonDifficulity.Common:
					diffChooseBtn.Text = GameUtility.GetUIString("UICampaign_NormalZone_DiffCommon");
					break;
				case _DungeonDifficulity.Hard:
					diffChooseBtn.Text = GameUtility.GetUIString("UICampaign_NormalZone_DiffHard");
					break;
				case _DungeonDifficulity.Nightmare:
					diffChooseBtn.Text = GameUtility.GetUIString("UICampaign_NormalZone_DiffNightmare");
					break;
			}
		}
	}

	//设置星级宝箱
	private void SetDungeonStarRewardUI()
	{
		// Init Star Reward UI.
		for (int index = 0; index < starRewardIcons.Count; index++)
			starRewardIcons[index].Init(this.zoneId, index);

		KodGames.ClientClass.Zone zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(zoneId);
		List<int> alreadGetRewardIndexs = new List<int>();
		int sumStar = 0;

		KodGames.ClientClass.DungeonDifficulty diffRecord = null;
		if (zoneRecord != null)
			diffRecord = zoneRecord.GetDungeonDiffcultyByDiffcultyType(this.currentDiffcultTab);

		if (diffRecord != null)
		{
			alreadGetRewardIndexs = diffRecord.BoxPickedIndexs;

			if (diffRecord.Dungeons != null)
				foreach (var dungeon in diffRecord.Dungeons)
					sumStar += dungeon.BestRecord;
		}

		for (int index = 0; index < starRewardIcons.Count; index++)
		{
			if (!alreadGetRewardIndexs.Contains(index) || index == starRewardIcons.Count - 1)
			{
				starRewardIcons[index].SetData(this.currentDiffcultTab, sumStar, alreadGetRewardIndexs);
				break;
			}
		}
	}

	//设置云游商人
	public int SetTravelData(int dungeonId, int mapShopIndex, SerializeDungeonData dungeonPositionData)
	{
		//判断本关有没有奖励
		var travelShop = ConfigDatabase.DefaultCfg.CampaignConfig.GetTravelTradeByDungeonId(dungeonId);
		if (travelShop == null)
			return mapShopIndex;

		//对云游商人进行渲染
		var uiElemShopItem = mapShopPool.AllocateItem().GetComponent<UIElemDungeonShopIcon>();
		shopIcons.Add(uiElemShopItem);
		uiElemShopItem.SetData(travelShop);

		//设置位置
		uiElemShopItem.CachedTransform.parent = this.bgButton.CachedTransform;
		uiElemShopItem.CachedTransform.localPosition = dungeonPositionData.shopLocations[mapShopIndex++].ConvertToVector3();

		return mapShopIndex;
	}

	private void SetDungeonDataView()
	{
		// Get DungeonConfigs.
		CampaignConfig.Zone zoneConfig = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneId);
		List<CampaignConfig.Dungeon> dungeonConfigs = zoneConfig.GetDungeonDifficultyByDifficulty(currentDiffcultTab).dungeons;

		if (dungeonConfigs == null)
			Debug.Log(string.Format("ZoneId {0} is Invalid .", this.zoneId.ToString("X8")));

		// Get the dungeon icon 's position.
		SerializeDungeonData dungeonPositionData = SerializeDungoenTools.GetSerializeDataFromFile(zoneConfig.GetDungeonDifficultyByDifficulty(currentDiffcultTab).positionName);

		// Set the dungeon icon 's position.
		for (int index = 0; index < mapIcons.Count; index++)
			mapIconPool.ReleaseItem(mapIcons[index].gameObject);

		for (int index = 0; index < mapLines.Count; index++)
			linePool.ReleaseItem(mapLines[index].gameObject);

		for (int index = 0; index < shopIcons.Count; index++)
			mapShopPool.ReleaseItem(shopIcons[index].gameObject);

		mapIcons.Clear();
		mapLines.Clear();
		shopIcons.Clear();

		int mapShopIndex = 0;
		for (int index = 0; index < dungeonConfigs.Count; index++)
		{
			UIElemDungeonMapIcon mapIcon = mapIconPool.AllocateItem().GetComponent<UIElemDungeonMapIcon>();
			mapIcons.Add(mapIcon);

			mapIcon.Init(index);
			mapIcon.CachedTransform.parent = this.bgButton.CachedTransform;
			mapIcon.transform.localPosition = dungeonPositionData.locations[index].ConvertToVector3();

			mapShopIndex = SetTravelData(dungeonConfigs[index].dungeonId, mapShopIndex, dungeonPositionData);
		}

		float horizontalSpacing = mapIcons[0].mapIcon.border.width;
		float verticalSpacing = mapIcons[0].mapIcon.border.height / 2;

		// Set the mapLine.
		for (int index = 0; index < dungeonConfigs.Count - 1; index++)
		{
			UIElementDungeonLineItem lineItem = linePool.AllocateItem().GetComponent<UIElementDungeonLineItem>();
			lineItem.transform.parent = this.bgButton.CachedTransform;
			mapLines.Add(lineItem);

			lineItem.SetLinePositionAndRotation(mapIcons[index].CachedTransform.localPosition, mapIcons[index + 1].CachedTransform.localPosition, horizontalSpacing, verticalSpacing);
		}

		// Get DungeonRecords.
		Dictionary<int, KodGames.ClientClass.Dungeon> dungeonRecords = GetDungeonRecords(dungeonConfigs);

		int lastBattleDungeonIndex = -1;

		for (int index = 0; index < dungeonConfigs.Count; index++)
		{
			bool canEnter = false;

			if (0 == index)
				canEnter = true;
			else
				canEnter = dungeonRecords[dungeonConfigs[index - 1].dungeonId] != null && dungeonRecords[dungeonConfigs[index - 1].dungeonId].BestRecord > 0;

			// Set the map name.
			mapIcons[index].mapIcon.border.Text = ItemInfoUtility.GetAssetName(dungeonConfigs[index].dungeonId);

			mapIcons[index].mapIcon.EnableButton(canEnter);

			mapIcons[index].mapIcon.SetData(canEnter ? dungeonConfigs[index].iconOpen : dungeonConfigs[index].iconClose);

			// Set the map appraise.
			KodGames.ClientClass.Dungeon dungeonRecord = dungeonRecords[dungeonConfigs[index].dungeonId];
			int appraise = dungeonRecord == null ? -1 : dungeonRecord.BestRecord;

			mapIcons[index].mapAppraiseIcon.SetQualityByQulityLv(appraise, false);

			// Set the map difficulty.
			bool isDiffcult = dungeonConfigs[index].difficulty == _Difficulty.Difficulty;
			mapIcons[index].mapDiffcult.Hide(isDiffcult == false);

			// Set the dungeon .
			mapIcons[index].dungeon = dungeonConfigs[index];

			// Record last dungeon index.
			if (dungeonConfigs[index].dungeonId == SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastBattleDungeonId)
				lastBattleDungeonIndex = index;
		}

		// Init MapAnimation Position.
		this.mapAnimation.transform.parent = mapIcons[lastBattleDungeonIndex < 0 ? 0 : lastBattleDungeonIndex].mapAnimation.transform;
		this.mapAnimation.transform.localPosition = Vector3.zero;

		bool autoScrollMap = false;
		bool playOpenAnim = false;
		currentIndex = GetLastUnlockDungeonIndex();

		if (lastBattleDungeonIndex != -1 && lastBattleDungeonIndex < mapIcons.Count - 1)
		{
			KodGames.ClientClass.Dungeon dungeon = dungeonRecords[mapIcons[lastBattleDungeonIndex].dungeon.dungeonId];
			int nextDungeonStatus = dungeonRecords[mapIcons[lastBattleDungeonIndex + 1].dungeon.dungeonId] == null ? 0 : dungeonRecords[mapIcons[lastBattleDungeonIndex + 1].dungeon.dungeonId].DungeonStatus;

			if (dungeon != null && dungeon.BestRecord > 0 && nextDungeonStatus == ClientServerCommon._DungeonStatus.LockState)
			{
				RequestMgr.Inst.Request(new SetDungeonStatusReq(mapIcons[lastBattleDungeonIndex + 1].dungeon.dungeonId, ClientServerCommon._DungeonStatus.UnLockState));

				if (ShouldScrollMapDown())
					autoScrollMap = true;
				else
					autoScrollMap = false;

				playOpenAnim = true;

				currentIndex = lastBattleDungeonIndex + 1;

				// Disable MapIcon.
				mapIcons[currentIndex].mapIcon.EnableButton(false);
				mapIcons[currentIndex].mapIcon.SetData(mapIcons[currentIndex].dungeon.iconClose);
				mapAnimation.SetActive(true);
			}
			else
				currentIndex = lastBattleDungeonIndex;
		}

		// 外部界面跳转副本的处理.
		if (jumpDungeonId != 0)
		{
			for (int index = 0; index < dungeonConfigs.Count; index++)
			{
				if (dungeonConfigs[index].dungeonId != jumpDungeonId)
					continue;

				if (index != currentIndex)
				{
					mapIcons[currentIndex].mapIcon.EnableButton(true);
					mapIcons[currentIndex].mapIcon.SetData(mapIcons[currentIndex].dungeon.iconOpen);
					currentIndex = index;
					autoScrollMap = false;
					playOpenAnim = false;
				}

				break;
			}

			jumpDungeonId = 0;
		}

		StartAnimation(autoScrollMap, playOpenAnim);
	}

	private void ShowCombatUI(bool show)
	{
		OperatorBase.gameObject.SetActive(show);

		if (show)
			SetContinueUI();
	}

	public void SetContinueUI()
	{
		CampaignConfig.Dungeon dungeonConfig = mapIcons[currentIndex].dungeon;
		KodGames.ClientClass.Dungeon dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonConfig.ZoneId, dungeonConfig.dungeonId);
		bool enableContinueCombat = dungeonConfig.canContinueCombat &&
								 (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.GameConfig.continueCombat.continueCombatOpenLevel) &&
								 (SysLocalDataBase.Inst.LocalPlayer.VipLevel >= ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(_OpenFunctionType.ContinueCombat)) &&
								 (dungeonRecord != null && dungeonRecord.BestRecord > 0 && dungeonRecord.TodayCompleteTimes < dungeonConfig.enterCount);

		// Three Star Limit.
		if (ConfigDatabase.DefaultCfg.GameConfig.continueCombat.enableThreeStarLimit)
			enableContinueCombat = enableContinueCombat && (dungeonRecord != null && dungeonRecord.BestRecord == 3);

		// Set Combat Data, for assistant.
		combatBase.GetComponent<UIElemAssistantBase>().assistantData = dungeonConfig.dungeonId;

		// Hide Button.
		operatorControll.HideChildObj(combatBase.gameObject, false);
		operatorControll.HideChildObj(continueCombatBase.gameObject, !enableContinueCombat);

		// Set Position.
		Vector3 mapPos = mapIcons[currentIndex].CachedTransform.localPosition;
		mapPos.y -= mapIcons[currentIndex].mapIcon.border.height / 2;

		if (bgButton.width / 2 - Mathf.Abs(mapPos.x) < OperatorBase.width / 2)
		{
			if (mapPos.x < 0)
				mapPos.x += OperatorBase.width / 2 - (bgButton.width / 2 - Mathf.Abs(mapPos.x));
			else
				mapPos.x -= OperatorBase.width / 2 - (bgButton.width / 2 - Mathf.Abs(mapPos.x));
		}

		mapPos.z = -0.001f;

		OperatorBase.CachedTransform.localPosition = new Vector3(mapPos.x, mapPos.y, OperatorBase.CachedTransform.localPosition.z);
	}

	private Dictionary<int, KodGames.ClientClass.Dungeon> GetDungeonRecords(List<CampaignConfig.Dungeon> dungeonConfigs)
	{
		Dictionary<int, KodGames.ClientClass.Dungeon> dungeonRecord = new Dictionary<int, KodGames.ClientClass.Dungeon>();

		foreach (CampaignConfig.Dungeon tempDungeon in dungeonConfigs)
			dungeonRecord.Add(tempDungeon.dungeonId, null);

		KodGames.ClientClass.Zone zone = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(zoneId);

		if (zone != null && zone.DungeonDifficulties != null && zone.DungeonDifficulties.Count > 0)
		{
			foreach (var diff in zone.DungeonDifficulties)
			{
				if (diff == null || diff.Dungeons.Count <= 0 || diff.DifficultyType != currentDiffcultTab)
					continue;

				foreach (var tempDungeon in diff.Dungeons)
					dungeonRecord[tempDungeon.DungeonId] = tempDungeon;
			}
		}

		return dungeonRecord;
	}

	private bool IsDungeonComplement(Dictionary<int, KodGames.ClientClass.Dungeon> dungeonRecords)
	{
		foreach (var dungeonRecord in dungeonRecords)
		{
			if (dungeonRecord.Value == null || dungeonRecord.Value.BestRecord != 3)
				return false;
		}

		return true;
	}

	private void StartAnimation(bool autoScroll, bool playOpenAnim)
	{
		// Show Dungeon Info.
		ShowCampaignBottomInfo();

		SetAnimation(playOpenAnim);

		float autoScrollPos = 0f;
		float scrollDuration = 0f;
		if (autoScroll)
		{
			autoScrollPos = ShouldScrollMapDown() ? 1.0f : 0f;
			scrollDuration = autoScrollDuration;
		}
		else
		{
			if (prevScrollPosition == -1)
				autoScrollPos = ShouldScrollMapDown() ? 1.0f : 0f;
			else
			{
				autoScrollPos = prevScrollPosition;
				prevScrollPosition = -1;
			}
		}

		SetTraceTarget(autoScrollPos, scrollDuration, mapEasingType);
	}

	private void SetTraceTarget(float distance, float duration, EZAnimation.EASING_TYPE easingType)
	{
		taretDistanceEasingData = new EasingData();
		taretDistanceEasingData.timer = 0;
		taretDistanceEasingData.duration = duration;
		taretDistanceEasingData.interpolator = EZAnimation.GetInterpolator(easingType);
		taretDistanceEasingData.distance = distance;
	}

	private float GetTargetDistance()
	{
		if (taretDistanceEasingData != null)
		{
			return taretDistanceEasingData.interpolator(taretDistanceEasingData.timer, targetDistance, taretDistanceEasingData.distance - targetDistance, taretDistanceEasingData.duration);
		}
		else
		{
			return targetDistance;
		}
	}

	public void Update()
	{
		// Auto Scroll ScrollList.
		if (taretDistanceEasingData != null)
		{
			taretDistanceEasingData.timer += Time.deltaTime;

			if (taretDistanceEasingData.timer >= taretDistanceEasingData.duration)
			{
				targetDistance = taretDistanceEasingData.distance;

				taretDistanceEasingData = null;
			}

			scrollList.ScrollPosition = GetTargetDistance();
		}

	}

	// Whether should scroll current dungeon in the scrollList's down.
	private bool ShouldScrollMapDown()
	{
		return System.Math.Abs(mapIcons[currentIndex].transform.localPosition.y) >= (bgButton.height / 2);
	}

	private int GetLastUnlockDungeonIndex()
	{
		int lastUnlockMapIndex = -1;
		for (int index = 0; index < mapIcons.Count; index++)
		{
			if (mapIcons[index].mapIcon.border.controlIsEnabled == false)
			{
				lastUnlockMapIndex = index - 1;
				break;
			}
		}

		if (lastUnlockMapIndex == -1)
			lastUnlockMapIndex = mapIcons.Count - 1;

		return lastUnlockMapIndex;
	}

	private void SetAnimation(bool playOpenAnim)
	{
		if (playOpenAnim)
		{
			isAnimationPlay = true;

			FXController fxController = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.campaignDungeonOpenParticle)).GetComponent<FXController>();
			ObjectUtility.AttachToParentAndResetLocalTrans(mapIcons[currentIndex].CachedTransform, fxController.CachedTransform);
			fxController.AddFinishCallback(data =>
				{
					if (!mapIcons[currentIndex].mapIcon.border.controlIsEnabled)
					{
						mapIcons[currentIndex].mapIcon.EnableButton(true);
						mapIcons[currentIndex].mapIcon.SetData(mapIcons[currentIndex].dungeon.iconOpen);
					}

					// 如果粒子播放完毕任然没有显示操作按钮，再次调用显示方法
					if (!this.mapAnimation.activeInHierarchy)
						PlayMapIconAnim();

					if (!this.OperatorBase.gameObject.activeInHierarchy)
						ShowCombatUI(true);

					isAnimationPlay = false;

				}, null);
		}

		PlayMapIconAnim();
	}

	private void PlayMapIconAnim()
	{
		if (!this.mapAnimation.activeInHierarchy)
			this.mapAnimation.SetActive(true);

		if (this.mapAnimation.transform.parent != mapIcons[currentIndex].mapAnimation.transform)
		{
			this.mapAnimation.transform.parent = mapIcons[currentIndex].mapAnimation.transform;
			AnimatePosition.Do(this.mapAnimation, EZAnimation.ANIM_MODE.FromTo, Vector3.zero, EZAnimation.GetInterpolator(animationEasingType), animationDuration, 0f, null, (data) => { ShowCombatUI(true); });
		}
		else
			ShowCombatUI(true);
	}

	#region Invoke Method
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDiffChose(UIButton btn)
	{
		SysUIEnv.Instance.GetUIModule<UIDlgCampaignDiffChose>().SetClickChangeDel(ChangeDungeonTab);
		SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgCampaignDiffChose, zoneId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetStarReward(UIButton btn)
	{
		var element = btn.Data as UIElemDungeonStarRewardItem;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDungeonStarReward), element.ZoneId, currentDiffcultTab, element.StarRewardIndex);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCombat(UIButton btn)
	{
		if (GameUtility.CheckPackageCapacity() == false)
			return;

		var dungeonCfg = mapIcons[currentIndex].dungeon;

		if (string.IsNullOrEmpty(dungeonCfg.plotCombatFile))
		{
			if (SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonRecruiteNpcs(dungeonCfg.dungeonId) == null)
				RequestMgr.Inst.Request(new QueryRecruiteNpcReq(dungeonCfg.dungeonId));
			else
				ShowBefroeCombatUI(dungeonCfg.dungeonId);
		}
		else
			RequestMgr.Inst.Request(new OnCombatReq(dungeonCfg.dungeonId, IDSeg.InvalidId, null, scrollList.ScrollPosition));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickContinueCombat(UIButton btn)
	{
		if (GameUtility.CheckPackageCapacity() == false)
			return;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgCampaignContinue), mapIcons[currentIndex].dungeon.dungeonId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMapIcon(UIButton btn)
	{
		UIElemDungeonMapIcon mapIcon = (btn.Data as UIElemAssetIcon).Data as UIElemDungeonMapIcon;

		if (this.mapAnimation.transform.parent != mapIcon.mapAnimation.transform)
			RefreshDungeonItemState(mapIcon.IndexInMap);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickShopIcon(UIButton btn)
	{
		var travelCfg = (btn.Data as UIElemAssetIcon).Data as CampaignConfig.TravelTrader;

		if (SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonTravelDataByDungeonId(travelCfg.dungeonId) == null)
			RequestMgr.Inst.Request(new QueryTravelReq(travelCfg.dungeonId));
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDungeonTravelShop), travelCfg.dungeonId);
	}

	public void ShowBefroeCombatUI(int dungeonId)
	{
		CombatData combatData = new CombatData();
		combatData.zoneID = this.zoneId;
		combatData.dungeonID = dungeonId;
		combatData.positionId = SysLocalDataBase.Inst.LocalPlayer.CampaignData.PositionId == 0 ? SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId : SysLocalDataBase.Inst.LocalPlayer.CampaignData.PositionId;
		combatData.uiDungeonMapPosition = scrollList.ScrollPosition;

		var rercuiteNpcs = SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonRecruiteNpcs(dungeonId);

		// Package capacity check.
		combatData.recruiteNpc = rercuiteNpcs.Count == 1 ? rercuiteNpcs[0] : null;

		bool isFam = ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(zoneId);

		if (rercuiteNpcs.Count > 1)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleRecuite), combatData);
		else
		{
			if (isFam)
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.ActivityCampaign, combatData);
			else
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.Campaign, combatData);
		}
	}

	private void RefreshDungeonItemState(int indexInMap)
	{
		if (isAnimationPlay)
			return;

		ShowCombatUI(false);

		currentIndex = indexInMap;

		StartAnimation(false, false);
	}

	public void OnResponseCombatErrOfEnterTimes(int dungeonId)
	{
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		var dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfg.ZoneId, dungeonCfg.dungeonId);
		int resetMaxCount = GetMaxDungeonResetCount(dungeonCfg, SysLocalDataBase.Inst.LocalPlayer.VipLevel);

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		string title = GameUtility.GetUIString("UIDlgMessage_Title_CampaignEnterTimesNotEngouth");
		string message = string.Empty;

		MainMenuItem rechargeItem = new MainMenuItem();
		rechargeItem.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");

		MainMenuItem resetItem = null;

		if (dungeonCfg.resetCosts == null || dungeonCfg.resetCosts.Count <= 0 || resetMaxCount <= 0)
		{
			message += "\n";
			message += "\n";
			message += GameUtility.GetUIString("UIPnlCampaign_DungeonCount_Limit");
		}
		else
		{
			message += GameUtility.FormatUIString("UIPnlCampaign_DungeonCount_ResetCost", dungeonCfg.resetCosts[0].count, ItemInfoUtility.GetAssetName(dungeonCfg.resetCosts[0].id));
			message += "\n";
			message += "\n";
			if (SysLocalDataBase.Inst.LocalPlayer.VipLevel <= 0)
				message += GameUtility.FormatUIString("UIPnlCampaign_DungeonCount_VIP0Reset", dungeonRecord.TodayAlreadyResetTimes, resetMaxCount);
			else
				message += GameUtility.FormatUIString("UIPnlCampaign_DungeonCount_VIPReset", SysLocalDataBase.Inst.LocalPlayer.VipLevel, dungeonRecord.TodayAlreadyResetTimes, resetMaxCount);
			message += "\n";
			message += "\n";

			if (SysLocalDataBase.Inst.LocalPlayer.VipLevel < ConfigDatabase.DefaultCfg.VipConfig.maxVipLevel)
				message += GameUtility.FormatUIString("UIPnlCampaign_DungeonCount_NextVIPReset", GameDefines.textColorRed, SysLocalDataBase.Inst.LocalPlayer.VipLevel + 1, GetMaxDungeonResetCount(dungeonCfg, SysLocalDataBase.Inst.LocalPlayer.VipLevel + 1));

			resetItem = new MainMenuItem();
			resetItem.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Reset");
			resetItem.Callback =
				(data) =>
				{
					RequestMgr.Inst.Request(new ResetDungeonCompleteTimesReq(dungeonId));
					return true;
				};
		}

		showData.SetData(title, message, rechargeItem, resetItem);
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	private int GetMaxDungeonResetCount(CampaignConfig.Dungeon dungeonCfg, int vipLevel)
	{
		int count = dungeonCfg.resetCount;

		if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(dungeonCfg.ZoneId))
		{
			switch (dungeonCfg.DungeonDifficulty)
			{
				case _DungeonDifficulity.Common:
					count += ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(vipLevel, VipConfig._VipLimitType.DungeonSecretAddCommonResetCount);
					break;
				case _DungeonDifficulity.Hard:
					count += ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(vipLevel, VipConfig._VipLimitType.DungeonSecretAddHardResetCount);
					break;
				case _DungeonDifficulity.Nightmare:
					count += ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(vipLevel, VipConfig._VipLimitType.DungeonSecretAddNightmareResetCount);
					break;
			}
		}
		else
		{
			switch (dungeonCfg.DungeonDifficulty)
			{
				case _DungeonDifficulity.Common:
					count += ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(vipLevel, VipConfig._VipLimitType.DungeonAddCommonResetCount);
					break;
				case _DungeonDifficulity.Hard:
					count += ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(vipLevel, VipConfig._VipLimitType.DungeonAddHardResetCount);
					break;
				case _DungeonDifficulity.Nightmare:
					count += ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(vipLevel, VipConfig._VipLimitType.DungeonAddNightmareResetCount);
					break;
			}
		}

		return count;

	}

	public void ShowCampaignBottomInfo()
	{
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignSceneBottom))
			SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneBottom>().RefreshViews(mapIcons[currentIndex].dungeon.dungeonId);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignSceneBottom), mapIcons[currentIndex].dungeon.dungeonId);
	}

	public void OnResponseRefreshDungeonInfo(int dungeonId)
	{
		ShowCampaignBottomInfo();

		// Refresh Combat UI.
		SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.SetContinueUI();
	}

	public void OnResponseGetDungeonRewardSuccess()
	{
		SetDungeonStarRewardUI();

		// Refresh 3d Model.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlCampaignActivityScene)))
			SysUIEnv.Instance.GetUIModule<UIPnlCampaignActivityScene>().SetCampaignStar(this.zoneId);
		else
			SysUIEnv.Instance.GetUIModule<UIPnlCampaignScene>().SetCampaignStar(this.zoneId);
	}

	#endregion
}
