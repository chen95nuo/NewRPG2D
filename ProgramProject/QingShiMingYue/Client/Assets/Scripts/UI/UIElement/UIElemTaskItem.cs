using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTaskItem : MonoBehaviour
{
	public UIElemAssetIcon taskIcon;//显示任务icon

	public SpriteText taskName;//任务名称
	public SpriteText taskDesc;//任务描述
	public SpriteText taskPlan;//任务进度

	//三个状态【两个按钮，一个显示】
	public UIButton gotoBtn;//去往界面
	public UIButton finshBtn;//可以领取
	public UIBox endBox;//已经完成

	//设置奖励
	public List<UIElemAssetIcon> rewards;

	public UIListItemContainer container;

	private KodGames.ClientClass.Quest quest;
	public KodGames.ClientClass.Quest Quest { get { return quest; } }

	private void SetShowBtn()
	{
		gotoBtn.Hide(true);
		finshBtn.Hide(true);
		endBox.Hide(true);

		foreach (var btn in rewards)
			btn.Hide(true);
	}

	public void SetData(KodGames.ClientClass.Quest quest)
	{
		container.Data = this;
		this.quest = quest;

		taskIcon.SetData(quest.QuestId);
		taskName.Text = ItemInfoUtility.GetAssetName(quest.QuestId);
		taskDesc.Text = string.Format(ItemInfoUtility.GetAssetExtraDesc(quest.QuestId), GameDefines.textColorWhite, GameDefines.textColorBtnYellow);


		QuestConfig.Quest questConfig = ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(quest.QuestId);

		taskPlan.Text = GameUtility.FormatUIString("UIPnlAssistant_Play", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(),
																			quest.CurrentStep, questConfig.totalStepCount);


		bool showGoto = quest.Phase < QuestConfig._PhaseType.Finished && questConfig.gotoUI != _UIType.UnKonw;
		bool showFinsh = quest.Phase >= QuestConfig._PhaseType.Finished && quest.Phase < QuestConfig._PhaseType.FinishedAndGotReward;
		bool showEnd = quest.Phase >= QuestConfig._PhaseType.FinishedAndGotReward;

		SetShowBtn();

		if (showGoto == true)
		{
			gotoBtn.Data = quest;
			gotoBtn.Hide(false);
		}
		if (showFinsh == true)
		{
			finshBtn.Data = quest;
			finshBtn.Hide(false);
		}
		if (showEnd == true)
		{
			endBox.Hide(false);
		}

		if (questConfig.rewards == null || questConfig.rewards.Count <= 0)
		{
			Debug.Log("Quest Rewards is NULL");
		}
		else
		{
			for (int index = 0; index < questConfig.rewards.Count; index++)
			{
				rewards[index].Hide(false);
				rewards[index].SetData(questConfig.rewards[index].id, questConfig.rewards[index].count);
			}
		}
	}
}
