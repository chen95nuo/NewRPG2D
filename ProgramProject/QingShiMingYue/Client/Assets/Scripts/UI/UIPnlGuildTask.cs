using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildTask : UIModule
{
	public SpriteText taskNameLabel;
	public SpriteText taskLevelLabel;
	public SpriteText taskTargetLabel;
	public SpriteText taskRewardLabel;
	public SpriteText taskProcessLabel;
	public SpriteText taskCountLabel;
	public SpriteText taskRefreshCost;
	public SpriteText freeDiceCount;
	public SpriteText diceCostLabel;
	public SpriteText diceCostCount;
	public UIElemAssetIcon diceIcon;
	public UIElemAssetIcon taskRefreshIcon;
	public UIButton guideBtn;
	public UIButton refreshBtn;
	public UIButton backBtn;
	public GameObject taskRoot;
	public GameObject successRoot;
	public GameObject diceRoot;

	private KodGames.ClientClass.GuildTaskInfo taskInfo;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		successRoot.SetActive(false);
		taskRoot.SetActive(false);
		diceRoot.SetActive(false);

		RequestMgr.Inst.Request(new QueryGuildTaskReq());

		return true;
	}

	public void RequestQueryTaskSuccess(KodGames.ClientClass.GuildTaskInfo taskInfo)
	{
		this.taskInfo = taskInfo;

		if (taskInfo.CompletedTaskCount >= taskInfo.MaxCompletedTaskPerDay)
		{
			successRoot.SetActive(true);
			taskRoot.SetActive(false);
			diceRoot.SetActive(false);
		}
		else
		{
			successRoot.SetActive(false);
			taskRoot.SetActive(true);
			diceRoot.SetActive(true);
			SetData(taskInfo);
		}
	}

	public void RequestDiceSuccess(KodGames.ClientClass.GuildTaskInfo taskInfo, List<int> diceResults)
	{
		bool moveNext = this.taskInfo.DoingTaskProgress != taskInfo.DoingTaskProgress;

		string taskDesc=this.taskInfo.TaskDesc;

		this.taskInfo = taskInfo;

		if (taskInfo.CompletedTaskCount >= taskInfo.MaxCompletedTaskPerDay)
		{
			successRoot.SetActive(true);
			taskRoot.SetActive(false);
			diceRoot.SetActive(false);
		}
		else
		{
			successRoot.SetActive(false);
			taskRoot.SetActive(true);
			diceRoot.SetActive(true);
			SetData(taskInfo);
		}

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgGuildDiceResult), diceResults, moveNext, taskDesc);
	}

	private void SetData(KodGames.ClientClass.GuildTaskInfo taskInfo)
	{
		taskNameLabel.Text = taskInfo.TaskName;

		taskLevelLabel.Text = taskInfo.TaskExtRewardDesc;

		taskTargetLabel.Text = taskInfo.TaskDesc;

		taskRewardLabel.Text = ItemInfoUtility.GetAssetName(taskInfo.RewardViews[0].Id) + taskInfo.RewardViews[0].Count.ToString();

		taskRefreshIcon.SetData(taskInfo.RefreshCost.Id);

		taskRefreshCost.Text = taskInfo.RefreshCount.ToString();

		taskProcessLabel.Text = GameUtility.FormatUIString("UIPnlGuildApplyList_MemberCout", taskInfo.DoingTaskProgress, taskInfo.RefreshCount);

		if (taskInfo.FreeDiceCount > 0)
		{
			freeDiceCount.Hide(false);
			diceCostCount.Hide(true);
			diceIcon.Hide(true);
			diceCostLabel.Hide(true);
			freeDiceCount.Text = GameUtility.FormatUIString("UIPnlGuildTask_FreeCount", GameDefines.textColorBtnYellow, GameDefines.textColorBlue, GameDefines.textColorBtnYellow, GameDefines.textColorWhite, taskInfo.FreeDiceCount, GameDefines.textColorBtnYellow);
		}else
		{
			freeDiceCount.Hide(true);
			diceCostCount.Hide(false);
			diceIcon.Hide(false);
			diceCostLabel.Hide(false);
			diceIcon.SetData(taskInfo.DiceCost.Id);
			diceCostCount.Text = taskInfo.DiceCost.Count.ToString();
		}

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDice(UIButton btn)
	{
		if (taskInfo.CompletedTaskCount >= taskInfo.MaxCompletedTaskPerDay)
			return;

		RequestMgr.Inst.Request(new GuildTaskDiceReq());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefresh(UIButton btn)
	{
		if (taskInfo.CompletedTaskCount >= taskInfo.MaxCompletedTaskPerDay)
			return;

		RequestMgr.Inst.Request(new RefreshGuildTaskReq());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGuide(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildGuide);
	}
}
