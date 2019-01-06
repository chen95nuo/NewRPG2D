using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UIPnlGuildConstruct : UIModule
{
	[System.Serializable]
	public class ConstructItem
	{
		public UIButton itemButton;
		public UIElemAssetIcon itemIcon;
		public UIBox itemSelectBox;
		public UIBox itemQualityBox;
		public SpriteText itemNameLabel;
		public SpriteText itemStatusLabel;
	}

	public GameObject guildTopRoot;
	public SpriteText guildLvLabel;
	public SpriteText guildConstructLabel;
	public SpriteText guildRequireConstructLabel;
	public SpriteText guildMoneyLabel;
	public SpriteText guildBossCountLabel;
	public SpriteText guildMoveCountLabel;
	public SpriteText guildConstructTimeLabel;

	public GameObject guildCenterRoot;
	public List<ConstructItem> constructItems;
	public UIButton refreshButton;
	public UIElemAssetIcon refreshCostIcon;
	public SpriteText todayFinishCountLabel;

	public GameObject guildBottomRoot;
	public UIElemAssetIcon selectItemIcon;
	public UIBox selectItemQualityIcon;
	public SpriteText selectNameLabel;
	public SpriteText selectDescLabel;
	public SpriteText selectRewardLabel;
	public SpriteText selectGuildRewardLabel;
	public SpriteText selectCountLabel;
	public UIElemAssetIcon choiceAssetIcon;
	public UIButton operationButton;
	public UIElemAssetIcon quickFinishIcon;
	public GameObject quickFinishBtn;

	private float deltaTime;
	private bool waitControl;
	private int selectedIndex;
	private bool firstQuery;
	private KodGames.ClientClass.ConstructionInfo constructionInfo;
	private Dictionary<int, List<string>> avaliableGuids = new Dictionary<int, List<string>>();
	private List<string> selectedGuids = new List<string>();
	private List<string> doingGuids = new List<string>();
	private const int qualityLevel = 3;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlGuildConstructTab>().ChangeTabButtons(_UIType.UIPnlGuildConstruct);
		firstQuery = true;
		SetCommonView();
		QueryConstructionInfo();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
	}

	private void ClearData()
	{
		this.waitControl = false;
		this.constructionInfo = null;
		this.avaliableGuids.Clear();
		this.selectedGuids.Clear();
		this.doingGuids.Clear();
		this.selectedIndex = 0;
	}

	private void QueryConstructionInfo()
	{
		this.waitControl = true;

		RequestMgr.Inst.Request(new QueryConstructionTaskReq());
	}

	private void SetCommonView()
	{
		guildCenterRoot.SetActive(false);
		guildBottomRoot.SetActive(false);

		var preTxColor = GameDefines.textColorBtnYellow;
		var nexTxColor = GameDefines.textColorWhite;
		var guildMiniInfo = SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo;
		var guildLevelCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetGuildLevelByLevel(guildMiniInfo.GuildLevel);

		// Set Guild Infos.
		// Set Guild Level.
		guildLvLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildLv", preTxColor, nexTxColor, guildMiniInfo.GuildLevel);

		// Set Guild Construct.
		guildConstructLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildContribution", preTxColor, nexTxColor, guildMiniInfo.GuildConstruct);

		// Set Guild Require Construction.
		if (guildMiniInfo.GuildLevel < ConfigDatabase.DefaultCfg.GuildConfig.MaxGuildLevel)
			guildRequireConstructLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildRequireNoName1", preTxColor, nexTxColor, guildLevelCfg.NextLevelNeedConstruct - guildMiniInfo.GuildConstruct);
		else
			guildRequireConstructLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildRequireNoName2", preTxColor, nexTxColor);

		// Set Guild Money.
		guildMoneyLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildMoney", preTxColor, nexTxColor, SysLocalDataBase.Inst.LocalPlayer.GuildMoney);

		// Set Guild Boss Count.
		guildBossCountLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildTotalContribution", preTxColor, nexTxColor, guildMiniInfo.TotalContribution);

		// Set Guild Move Count.
		guildMoveCountLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildMoveCount", preTxColor, nexTxColor, ItemInfoUtility.GetGameItemCount(ConfigDatabase.DefaultCfg.ItemConfig.exploreItem));
	}

	private void SetTimeLabel(long nowTime)
	{
		if (constructionInfo == null || constructionInfo.JoinTime + constructionInfo.WaitTime < nowTime)
			guildConstructTimeLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_GuildTaskCount", constructionInfo.GuildMaxAccomplishPerDay - constructionInfo.GuildAccomplishTaskCount);
		else
			guildConstructTimeLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_TimeLabel", (int)constructionInfo.WaitTime / 3600000, GameUtility.Time2String(constructionInfo.JoinTime + constructionInfo.WaitTime - nowTime));
	}

	private void SetDynamicView()
	{
		guildCenterRoot.SetActive(true);
		guildBottomRoot.SetActive(true);

		// Set Time Label.
		SetTimeLabel(SysLocalDataBase.Inst.LoginInfo.NowTime);

		// Set Refresh Cost.
		refreshButton.gameObject.SetActive(constructionInfo.PlayerAccomplishTaskCount < constructionInfo.PlayerMaxAccomplishPerDay && constructionInfo.GuildMaxAccomplishPerDay - constructionInfo.GuildAccomplishTaskCount > 0);

		refreshCostIcon.SetData(constructionInfo.RefershCosts[0].Id);
		refreshCostIcon.border.Text = constructionInfo.RefershCosts[0].Count.ToString();

		// Set Complete TaskCount.
		todayFinishCountLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_CompleteCount", constructionInfo.PlayerMaxAccomplishPerDay - constructionInfo.PlayerAccomplishTaskCount, constructionInfo.PlayerMaxAccomplishPerDay);

		// Set Tasks.
		for (int index = 0; index < constructItems.Count; index++)
			constructItems[index].itemButton.gameObject.SetActive(index < constructionInfo.Tasks.Count);

		for (int index = 0; index < constructItems.Count && index < constructionInfo.Tasks.Count; index++)
			SetConstructTaskItem(index);

		if (firstQuery)
			selectedIndex = GetFirstSelectIndex(constructionInfo);

		SetSelectedView(selectedIndex);
	}

	private void SetConstructTaskItem(int index)
	{
		// Set Data.
		constructItems[index].itemButton.Data = index;

		// Set Task Icon.
		constructItems[index].itemIcon.SetData(constructionInfo.Tasks[index].TaskIconId);

		// Set Task Quality Icon.
		UIElemTemplate.Inst.SetGuildConstructionQualityIcon(constructItems[index].itemQualityBox, constructionInfo.Tasks[index].TaskQuality);

		// Set Task Name Label.
		constructItems[index].itemNameLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_TaskName", constructionInfo.Tasks[index].Color, constructionInfo.Tasks[index].TaskName);

		// Set Task Status.
		if (constructionInfo.Tasks[index].IsDoing)
			constructItems[index].itemStatusLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_TaskAccept", GameDefines.guildConstructionAccept);
		else
			constructItems[index].itemStatusLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_TaskAcceptable", GameDefines.guildConstructionIdle);
	}

	private void SetSelectedView(int selectedIndex)
	{
		selectedGuids.Clear();

		this.selectedIndex = selectedIndex;

		var taskInfo = constructionInfo.Tasks[selectedIndex];

		// Show Select Box.
		for (int index = 0; index < constructItems.Count; index++)
			constructItems[index].itemSelectBox.Hide(selectedIndex != index);

		// Set Select Task Icon.
		selectItemIcon.SetData(taskInfo.TaskIconId);

		// Set Task Quality Icon.
		UIElemTemplate.Inst.SetGuildConstructionQualityIcon(selectItemQualityIcon, taskInfo.TaskQuality);

		// Set Select Task Name.
		selectNameLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_TaskName", taskInfo.Color, taskInfo.TaskName);

		// Set Select Task Desc.
		selectDescLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_TaskDesc", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, taskInfo.TaskDesc);

		// Set Select Reward Icon.
		var rewardStr = string.Empty;

		for (int index = 0; index < taskInfo.Reward.Count; index++)
		{
			rewardStr += GameUtility.FormatUIString("UIPnlGuildConstruct_TaskRewardItem", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(taskInfo.Reward[index].Id), GameDefines.textColorWhite, taskInfo.Reward[index].Count);

			if (index < taskInfo.Reward.Count - 1)
				rewardStr += "\t";
		}
		selectGuildRewardLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_TaskRewardGuild", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, taskInfo.GuildConstruct);
		selectRewardLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_TaskRewardPlayer", GameDefines.textColorBtnYellow, rewardStr);

		// Set Quick Finish Cost.
		quickFinishIcon.SetData(taskInfo.OneClickCompletedCosts[0].Id);
		quickFinishIcon.border.Text = taskInfo.OneClickCompletedCosts[0].Count.ToString();

		// Set Select Cards.
		object cost = null;
		bool canPickReward = false;
		if (taskInfo.CostAssets.Count > 0)
			cost = taskInfo.CostAssets;
		else
			cost = taskInfo.Costs;

		choiceAssetIcon.Data = cost;

		if (ItemInfoUtility.ExchangeIsOptionCost(cost))
		{
			// Set Current Own Count.
			selectCountLabel.Text = string.Empty;

			if (doingGuids.Count <= 0 || !taskInfo.IsDoing)
			{
				if (selectedGuids.Count <= 0)
					UIUtility.CopyIcon(choiceAssetIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconGuildConstructAdd);
				else
					choiceAssetIcon.SetData(ItemInfoUtility.GetResourceIdByGuid(selectedGuids[0]));
			}
			else
			{
				if (doingGuids.Count <= 0)
					UIUtility.CopyIcon(choiceAssetIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconGuildConstructAdd);
				else
				{
					if (ItemInfoUtility.ExchangeGetCostType(cost) == IDSeg._AssetType.Dan)
					{
						int danId = ItemInfoUtility.GetResourceIdByGuid(doingGuids[0]);
						int breakLevel = ItemInfoUtility.GetBreakLevelByGuid(doingGuids[0]);
						var danCfg = ConfigDatabase.DefaultCfg.DanConfig.GetDanIconByResourseIdAndBreakLevel(danId, breakLevel);

						choiceAssetIcon.SetData(danCfg.IconId);
					}
					else
						choiceAssetIcon.SetData(ItemInfoUtility.GetResourceIdByGuid(doingGuids[0]));
				}
			}
			canPickReward = doingGuids.Count > 0;
		}
		else
		{
			int requireCount = ItemInfoUtility.ExchangeGetCostCount(cost);
			int currentCount = ItemInfoUtility.GetGameItemCount((cost as KodGames.ClientClass.ItemEx).Id);

			// Set Choice Icon.
			choiceAssetIcon.SetData((cost as KodGames.ClientClass.ItemEx).Id);

			// Set Current Own Count.
			selectCountLabel.Text = GameUtility.FormatUIString("UIPnlGuildConstruct_TaskSelectCount", requireCount > currentCount ? GameDefines.textColorRed : GameDefines.textColorGreen, currentCount, requireCount);

			canPickReward = requireCount <= currentCount;
		}


		quickFinishBtn.SetActive(!taskInfo.IsDoing || !canPickReward);

		// Set Operation Button.
		operationButton.Data = canPickReward;
		if (taskInfo.IsDoing)
			operationButton.Text = GameUtility.GetUIString(canPickReward ? "UIPnlGuildConstruct_TaskOperatorGetReward" : "UIPnlGuildConstruct_TaskOperatorQuit");
		else
			operationButton.Text = GameUtility.GetUIString("UIPnlGuildConstruct_TaskOperatorAccept");
	}

	private int GetFirstSelectIndex(KodGames.ClientClass.ConstructionInfo constructionInfo)
	{
		firstQuery = false;

		for (int index = 0; index < constructionInfo.Tasks.Count; index++)
			if (constructionInfo.Tasks[index].IsDoing)
				return index;

		return IDSeg.InvalidId;
	}

	private void ResetSelectedGuids(object costOption, List<string> guids)
	{
		doingGuids.Clear();
		doingGuids = guids;

		if (HasTaskDoing() && doingGuids.Count > 0 && (constructionInfo.PlayerAccomplishTaskCount == constructionInfo.PlayerMaxAccomplishPerDay - 1
			|| constructionInfo.GuildAccomplishTaskCount == constructionInfo.GuildMaxAccomplishPerDay - 1))
			refreshButton.gameObject.SetActive(false);
		else
			refreshButton.gameObject.SetActive(true);

		SetSelectedView(selectedIndex);
	}

	private void Update()
	{
		if (this.constructionInfo == null || this.IsShown == false || this.IsOverlayed || this.waitControl)
			return;

		deltaTime += Time.deltaTime;
		if (deltaTime > 1.0f)
		{
			var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

			SetTimeLabel(nowTime);

			if (constructionInfo.NextRefreshTime < nowTime)
				QueryConstructionInfo();
		}
	}

	private bool HasTaskDoing()
	{
		foreach (var task in constructionInfo.Tasks)
			if (task.IsDoing)
				return true;

		return false;
	}

	private bool HasQualityTask(int qualityLevel)
	{
		foreach (var task in constructionInfo.Tasks)
			if (task.TaskQuality >= qualityLevel && !task.IsDoing)
				return true;

		return false;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickConstructItem(UIButton btn)
	{
		selectedGuids.Clear();
		SetSelectedView((int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefreshButton(UIButton btn)
	{
		if (HasQualityTask(qualityLevel))
		{
			MainMenuItem okCallback = new MainMenuItem();
			okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK_Space");
			okCallback.Callback = (userData) =>
			{
				RequestMgr.Inst.Request(new RefreshConstructionTaskReq());

				return true;
			};

			MainMenuItem cancelCallback = new MainMenuItem();
			cancelCallback.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(GameUtility.GetUIString("UIPnlGuildConstruct_TaskRefreshTitle"), GameUtility.GetUIString("UIPnlGuildConstruct_TaskRefreshSelectText"), cancelCallback, okCallback);

			SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
			return;
		}

		RequestMgr.Inst.Request(new RefreshConstructionTaskReq());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChoiceButton(UIButton btn)
	{
		if (!HasTaskDoing() && !constructionInfo.Tasks[selectedIndex].IsDoing)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildConstruct_TaskChooseNo"));
			return;
		}
		else if (!constructionInfo.Tasks[selectedIndex].IsDoing)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildConstruct_TaskChooseFaild"));
			return;
		}

		UIElemAssetIcon cost = btn.Data as UIElemAssetIcon;
		if (ItemInfoUtility.ExchangeIsOptionCost(cost.Data))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlChooseCard), cost.Data, new UIPnlChooseCard.OnChooseCardSuccessDel(ResetSelectedGuids));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOperationButton(UIButton btn)
	{
		var taskInfo = constructionInfo.Tasks[selectedIndex];
		var canPickReward = (bool)btn.Data;

		if (canPickReward && taskInfo.IsDoing)
		{
			var costs = new List<KodGames.ClientClass.Cost>();
			object cost = null;
			if (taskInfo.CostAssets != null && taskInfo.CostAssets.Count > 0)
				cost = taskInfo.CostAssets;
			else
				cost = taskInfo.Costs;

			if (ItemInfoUtility.ExchangeIsOptionCost(cost) == false)
			{
				var kd_cost = new KodGames.ClientClass.Cost((cost as KodGames.ClientClass.ItemEx).Id, (cost as KodGames.ClientClass.ItemEx).Count, string.Empty);
				costs.Add(kd_cost);
			}
			else
			{
				foreach (var guid in doingGuids)
				{
					var kd_cost = new KodGames.ClientClass.Cost(ItemInfoUtility.GetResourceIdByGuid(guid), 1, guid);
					costs.Add(kd_cost);
				}
			}

			RequestMgr.Inst.Request(new CompleteConstructionTaskReq(taskInfo.TaskId, costs, taskInfo.TaskIndex));
		}
		else
		{
			if (taskInfo.IsDoing)
				RequestMgr.Inst.Request(new GiveUpConstructionTaskReq(taskInfo.TaskId, taskInfo.TaskIndex));
			else
				RequestMgr.Inst.Request(new AcceptConstructionTaskReq(taskInfo.TaskId, taskInfo.TaskIndex));
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickQuickFinishButton(UIButton btn)
	{
		var taskInfo = constructionInfo.Tasks[selectedIndex];
		RequestMgr.Inst.Request(new CompleteConstructionTaskReq(taskInfo.TaskId, null, taskInfo.TaskIndex));
	}

	public void RefreshConstructInfo(KodGames.ClientClass.ConstructionInfo constructionInfo)
	{
		this.constructionInfo = constructionInfo;

		this.waitControl = false;

		this.selectedGuids.Clear();

		SetCommonView();

		SetDynamicView();
	}

	public void OnCompleteConstructTask(KodGames.ClientClass.ConstructionInfo constructionInfo, KodGames.ClientClass.Reward reward)
	{
		RefreshConstructInfo(constructionInfo);

		doingGuids.Clear();

		// Show Reward Message.
		var rewardStr = SysLocalDataBase.GetRewardDesc(reward, false, false, false);
		if (string.IsNullOrEmpty(rewardStr) == false)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), rewardStr);
	}
}