using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlCampaignBattleResult : UIPnlBattleResultBase
{
	public SpriteText starDescLabel;
	public SpriteText firstAppraiseThreeLabel;
	public UIScrollList rewardList;
	public GameObjectPool rewardPool;

	//首次通关奖励，界面排版不同-----------------------------------------------------------------------
	public GameObject firstPass_RewardInfoPnl, buttonRoot;
	public UIScrollList firstPass_RewardList;
	public GameObjectPool firstPass_rewardPool;

	public Transform goldLabelFirstPassMarker, expLabelFirstPassMarker;
	public Transform goldLabelNormalMarker, expLabelNormalMarker;
	public Transform goldLabelRoot, expLabelRoot;

	public UIButton playBackBtn;
	public UIButton confirmBtn;

	const float REWARDBG_TOPLEFT_OFFSET_FIRSTPASS = 18;
	const float BUTTONROOT_TOPLEFT_OFFSET_FIRSTPASS = 10;
	const float OBTAINEDCHILDLAYOUT_WIDTHHEIGHT_FIRSTPASS = 28;
	//---------------------------------------------------------------------------------------------------

	public GameObject obtainedChildLayout;
	public UIChildLayoutControl mainChildLayout;

	private bool isLeveDlgshow = false;
	private bool isPalyUpLevel = false;

	public class CampaignBattleResultData : BattleResultData
	{
		private KodGames.ClientClass.CombatResultAndReward battleData;

		private int zoneId;
		public int ZoneId { get { return zoneId; } }

		private int dungeonId;
		public int DungeonId { get { return dungeonId; } }

		private int dungeonType;
		public int DungeonType { get { return dungeonType; } }

		private float mapPosition;
		public float MapPosition { get { return mapPosition; } }

		private int enterUIType;
		public int EnterUIType { get { return enterUIType; } set { enterUIType = value; } }

		public CampaignBattleResultData(KodGames.ClientClass.CombatResultAndReward battleData, int zoneId, int dungeonId, int dungeonType, int preRecord, float mapPosition)
			: base(_UIType.UIPnlCampaignBattleResult)
		{
			this.battleData = battleData;
			this.zoneId = zoneId;
			this.dungeonId = dungeonId;
			this.dungeonType = dungeonType;
			this.mapPosition = mapPosition;

			this.CombatType = ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(zoneId) ? _CombatType.ActivityCampaign : _CombatType.Campaign;
			this.Rating = preRecord;
			this.enterUIType = _UIType.UnKonw;
		}

		public override bool CanShowView()
		{
			return true;
		}

		public override bool CanShowLvlInformation()
		{
			return true;
		}

		public override bool IsWinner()
		{
			if (battleData != null)
				return battleData.BattleRecords[battleData.BattleRecords.Count - 1].TeamRecords[0].IsWinner;

			return false;
		}

		public override int GetAppraiseNumber()
		{
			return battleData.StarCompleteIndexs.Count;
		}

		public override string GetGoldRewardOrOtherStr()
		{
			int goldRewardOrOtherCount = 0;

			foreach (KodGames.ClientClass.Consumable consumable in this.battleData.DungeonReward_ExpSilver.Consumable)
			{
				if (ClientServerCommon.IDSeg._SpecialId.GameMoney == consumable.Id)
					goldRewardOrOtherCount = consumable.Amount;
			}
			return string.Format(GameUtility.GetUIString("UIPnlBattleResult_Gamemoney_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, goldRewardOrOtherCount);
		}

		public override string GetExpRewardOrOtherStr()
		{
			int expRewardOrOtherCount = 0;
			foreach (KodGames.ClientClass.Consumable consumable in battleData.DungeonReward_ExpSilver.Consumable)
			{
				if (ClientServerCommon.IDSeg._SpecialId.Experience == consumable.Id)
					expRewardOrOtherCount = consumable.Amount;
			}

			return string.Format(GameUtility.GetUIString("UIPnlBattleResult_Exprience_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, expRewardOrOtherCount);
		}

		public override KodGames.ClientClass.Reward GetBattleReward()
		{
			return battleData.DungeonReward;
		}

		public override KodGames.ClientClass.Reward GetFirstPassReward()
		{
			return battleData.FirstpassReward;
		}

		public override bool HasFirstPassReward()
		{
			if (GetFirstPassReward() == null)
				return false;

			var dic = SysLocalDataBase.ConvertIdCountList(GetFirstPassReward());

			if (dic == null || dic.Count == 0)
				return false;

			return true;
		}

		public override bool CanShowFailGuid()
		{
			return true;
		}

		public List<int> GetStarConditionIndexs()
		{
			return battleData.StarCompleteIndexs;
		}
	}

	private void ManageLayout()
	{
		mainChildLayout.HideChildObj(loseInfoPanel, battleResultData.IsWinner());
		mainChildLayout.HideChildObj(rewardInfoPanel, !battleResultData.IsWinner(), false);

		if (battleResultData.HasFirstPassReward())
		{
			//有首次通关奖励
			expLabelRoot.localPosition = expLabelFirstPassMarker.localPosition;
			goldLabelRoot.localPosition = goldLabelFirstPassMarker.localPosition;
			mainChildLayout.GetLayoutChildControl(rewardInfoPanel).topLeftOffset = REWARDBG_TOPLEFT_OFFSET_FIRSTPASS;
			mainChildLayout.GetLayoutChildControl(buttonRoot).topLeftOffset = BUTTONROOT_TOPLEFT_OFFSET_FIRSTPASS;
			mainChildLayout.GetLayoutChildControl(obtainedChildLayout).widthHeight = OBTAINEDCHILDLAYOUT_WIDTHHEIGHT_FIRSTPASS;
			mainChildLayout.HideChildObj(firstPass_RewardInfoPnl, false, false);
		}
		else
		{
			expLabelRoot.localPosition = expLabelNormalMarker.localPosition;
			goldLabelRoot.localPosition = goldLabelNormalMarker.localPosition;
			mainChildLayout.HideChildObj(firstPass_RewardInfoPnl, true, false);
		}

		mainChildLayout.Layout();
	}

	public void DisabledButton()
	{
		if (playBackBtn.controlState != UIButton.CONTROL_STATE.DISABLED)
			playBackBtn.SetControlState(UIButton.CONTROL_STATE.DISABLED);
		if (confirmBtn.controlState != UIButton.CONTROL_STATE.DISABLED)
			confirmBtn.SetControlState(UIButton.CONTROL_STATE.DISABLED);
	}

	public void ActiveButton()
	{
		if (playBackBtn.controlState != UIButton.CONTROL_STATE.NORMAL)
			playBackBtn.SetControlState(UIButton.CONTROL_STATE.NORMAL);
		if (confirmBtn.controlState != UIButton.CONTROL_STATE.NORMAL)
			confirmBtn.SetControlState(UIButton.CONTROL_STATE.NORMAL);
	}

	private bool IsActiveButtonToStatus()
	{
		if (playBackBtn.controlState == UIButton.CONTROL_STATE.DISABLED && confirmBtn.controlState == UIButton.CONTROL_STATE.DISABLED)
			return false;
		return true;
	}
	
	private void Update()
	{
		if (!IsActiveButtonToStatus() && isPalyUpLevel)
		{
			Animation ani = resultInfoPanel.GetComponent<Animation>();
			if (ani != null && !ani.isPlaying)
			{
				if (!isLeveDlgshow && SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level <= ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel)
				{
					isLeveDlgshow = true;
					SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPlayerLevelUp));
				}
				UIDlgPlayerLevelUp playerLevelUp = SysUIEnv.Instance.GetUIModule<UIDlgPlayerLevelUp>();
				if (isLeveDlgshow && !playerLevelUp.IsShown)
				{
					Animation leveUp_ani = playerLevelUp.dlgBg.GetComponent<Animation>();
					if (!leveUp_ani.isPlaying)
					{
						ActiveButton();
						isLeveDlgshow = false;
						isPalyUpLevel = false;
					}
				}
			}
		}

	}


	public override void InitViews()
	{
		ManageLayout();

		base.InitViews();

		// Show Star Label.
		starDescLabel.Hide(!battleResultData.IsWinner());

		// Record InterrupteCampaing.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.InterruptCampaign = true;

		bool isWin = battleResultData.IsWinner();

		firstAppraiseThreeLabel.Text = "";
		if (isWin)
		{
			ClearData();
			StartCoroutine("FillRewardList");
		}
		else
			firstAppraiseThreeLabel.Text = GameUtility.GetUIString("UIPnlBattleResult_Dungeon_FailRewardMessage");

		rewardInfoPanel.SetActive(false);
		firstPass_RewardInfoPnl.SetActive(false);

		for (int i = 0; i < btn_Appraises.Count; i++)
		{
			UIButton StarBtn = btn_Appraises[i].GetComponent<UIButton>();
			if (StarBtn != null)
			{
				StarBtn.scriptWithMethodToInvoke = this;
				StarBtn.methodToInvoke = "OnStarClick";
			}
		}
	}

	private void ClearData()
	{
		StopCoroutine("FillRewardList");

		rewardList.ClearList(false);
		rewardList.ScrollPosition = 0f;

		firstPass_RewardList.ClearList(false);
		firstPass_RewardList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		foreach (var kvp in SysLocalDataBase.ConvertIdCountList(battleResultData.GetBattleReward()))
		{
			UIElemBattleResultDungeonItem item = rewardPool.AllocateItem().GetComponent<UIElemBattleResultDungeonItem>();
			item.SetData(kvp.first, kvp.second);

			rewardList.AddItem(item.gameObject);
		}
		//列表能向左无限拖动，暂时性解决办法
		rewardList.ScrollToItem(0, 0);

		if (battleResultData.HasFirstPassReward())//普通奖励，首通奖励两个列表
		{
			foreach (var kvp in SysLocalDataBase.ConvertIdCountList(battleResultData.GetFirstPassReward()))
			{
				UIElemBattleResultDungeonItem item = firstPass_rewardPool.AllocateItem().GetComponent<UIElemBattleResultDungeonItem>();
				item.SetData(kvp.first, kvp.second);

				firstPass_RewardList.AddItem(item.gameObject);
			}
		}
		firstPass_RewardList.ScrollToItem(0, 0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		if (SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("GotoUIType"))
		{
			var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(GetDungeonId());
			bool isActivityZone = ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(dungeonCfg.ZoneId);

			if (isActivityZone)
				GameUtility.JumpUIPanel(_UIType.UI_ActivityDungeon, GetDungeonId());
			else
				GameUtility.JumpUIPanel(_UIType.UI_Dungeon, GetDungeonId());

			return;
		}

		// Record InterrupteCampaing.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.InterruptCampaign = false;

		var campaignBattleResultData = battleResultData as CampaignBattleResultData;

		if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(campaignBattleResultData.ZoneId))
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_ActivityDungeon>(battleResultData);
		else
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_Dungeon>(battleResultData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnReplayClick(UIButton btn)
	{
		HideSelf();

		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		battleState.ReplayBattle();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnStarClick()
	{
		string sb = string.Empty;
		sb += GameDefines.txColorYellow2.ToString();
		sb += GameUtility.GetUIString("UIPnlCampaign_BattleResult_EvaluateCondition") + "\n\n";

		var data = battleResultData as CampaignBattleResultData;
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(data.DungeonId);

		foreach (var complementIndex in data.GetStarConditionIndexs())
			sb += GetConditionStr(dungeonCfg.startCondition[complementIndex], true) + "\n";

		sb += GameDefines.txColorWhite.ToString();
		for (int index = 0; index < dungeonCfg.startCondition.Count; index++)
		{
			if (data.GetStarConditionIndexs().Contains(index))
				continue;

			sb += GetConditionStr(dungeonCfg.startCondition[index], false) + "\n";
		}

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(sb.ToString(), true, true);
		SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	private string GetConditionStr(CampaignConfig.StarCondition condition, bool complement)
	{
		string formatStr = complement ? "UIPnlCampaign_BattleResult_Evaluate_Win" : "UIPnlCampaign_BattleResult_Evaluate_Lose";

		if (condition.compareIntValue > 0 || condition.compareFloatValue > 0)
			return GameUtility.FormatUIString(formatStr,
				string.Format("{0}{1}{2}", _StarRewardEvaType.GetDisplayNameByType(condition.type, ConfigDatabase.DefaultCfg),
				_ConditionValueCompareType.GetDisplayNameByType(condition.compareType, ConfigDatabase.DefaultCfg),
				condition.compareIntValue > condition.compareFloatValue ? condition.compareIntValue.ToString() : condition.compareFloatValue.ToString("P"))
				);
		else
			return GameUtility.FormatUIString(formatStr,
				string.Format("{0}", _StarRewardEvaType.GetDisplayNameByType(condition.type, ConfigDatabase.DefaultCfg)));
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected override IEnumerator ShowRewardPanel()
	{
		Animation ani = resultInfoPanel.GetComponent<Animation>();
		if (ani != null)
			ani.Play();
		yield return new WaitForSeconds(0.5f);
		isPalyUpLevel = true;
		if (!battleResultData.HasFirstPassReward())
		{
			if (battleResultData.IsWinner())
				rewardInfoPanel.SetActive(true);
			else
				loseInfoPanel.SetActive(true);
		}
		else//有首次通关奖励，显示首次通关奖励列表
		{
			if (battleResultData.IsWinner())
			{
				rewardInfoPanel.SetActive(true);
				firstPass_RewardInfoPnl.SetActive(true);
			}
			else
				loseInfoPanel.SetActive(true);
		}
	}

	public int GetDungeonId()
	{
		return (battleResultData as CampaignBattleResultData).DungeonId;
	}
	public bool GetResult()
	{
		return (battleResultData as CampaignBattleResultData).IsWinner() && (battleResultData as CampaignBattleResultData).HasFirstPassReward();
	}
}
