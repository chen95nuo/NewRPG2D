using UnityEngine;
using ClientServerCommon;

public class UIElemDailyGuidItem : MonoBehaviour
{
	public AutoSpriteControlBase dailyBg;
	public UIBox dailyGuidIconBg;
	public UIElemAssetIcon dailyGuidIcon;
	public SpriteText dailyGuidNameLabel;
	public SpriteText dailyGuidDescLabel;
	public SpriteText dailyGuidExtraDescLabel;
	public AutoSpriteControlBase operatorControlBase;
	public AutoSpriteControlBase completeControlBase;
	public SpriteText dailyGuidStatuLabel;
	public SpriteText dailyGuidProgressLabel;
	public SpriteText rewardLabel;

	public UIListItemContainer container;

	private KodGames.ClientClass.Quest quest;
	public KodGames.ClientClass.Quest Quest { get { return quest; } }

	public void SetData(KodGames.ClientClass.Quest quest)
	{
		container.Data = this;
		this.quest = quest;

		QuestConfig.Quest questConfig = ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(quest.QuestId);

		// Set Item Icon.
		dailyGuidIcon.SetData(quest.QuestId);

		// Set Item Icon Background.
		if (dailyGuidIconBg != null)
			dailyGuidIconBg.Hide(!questConfig.showFx);

		// Set Item Title.
		dailyGuidNameLabel.Text = ItemInfoUtility.GetAssetName(quest.QuestId);//quest.QuestId.ToString("X"); 

		// Set Item Description.
		dailyGuidDescLabel.Text = string.Format("{0}{1}", GameDefines.txColorBrown.ToString(), ItemInfoUtility.GetAssetDesc(quest.QuestId));
		dailyGuidExtraDescLabel.Text = string.Format(ItemInfoUtility.GetAssetExtraDesc(quest.QuestId), GameDefines.txColorYellow, GameDefines.txColorWhite);

		// Set Item Progress Status.
		bool showGoto = quest.Phase < QuestConfig._PhaseType.Finished && questConfig.gotoUI != _UIType.UnKonw;
		bool showGetReward = quest.Phase >= QuestConfig._PhaseType.Finished && quest.Phase < QuestConfig._PhaseType.FinishedAndGotReward;

		// Set Finish Box.
		completeControlBase.Hide(quest.Phase < QuestConfig._PhaseType.FinishedAndGotReward);

		// Set Bg States.
		if (!questConfig.notHideWhenFinished)
			UIElemTemplate.Inst.listItemBgTemplate.SetListItemBg(dailyBg, quest.Phase == QuestConfig._PhaseType.Finished);

		dailyGuidStatuLabel.Hide(showGoto || showGetReward);
		operatorControlBase.Hide(!(showGoto || showGetReward));

		if (showGoto || showGetReward)
		{
			operatorControlBase.Data = quest;
			operatorControlBase.Text = showGoto ? GameUtility.GetUIString("UIPnlDailyGuid_GoTo") : GameUtility.GetUIString("UIPnlDailyGuid_GetReward");
		}
		else
			dailyGuidStatuLabel.Text = quest.Phase >= QuestConfig._PhaseType.Finished ? "" : GameUtility.GetUIString("UIPnlDailyGuid_UnFinished");

		// Set Item Progress label.
		bool showProgress = quest.Phase < QuestConfig._PhaseType.Finished;
		dailyGuidProgressLabel.Hide(showProgress == false);

		if (showProgress)
			dailyGuidProgressLabel.Text = GameUtility.FormatUIString("UIPnlDailyGuid_Progress", quest.CurrentStep, questConfig.totalStepCount);

		// Set Reward Name and Count.
		rewardLabel.Text = "";
		if (questConfig.rewards == null || questConfig.rewards.Count <= 0)
		{
			Debug.Log("Quest " + questConfig.questId.ToString("X") + " do not has reward.");
		}

		string rewardStr = "";
		foreach (var reward in questConfig.rewards)
		{
			rewardStr = GameUtility.AppendString(rewardStr, GameDefines.txColorYellow.ToString() + ItemInfoUtility.GetAssetName(reward.id) + GameDefines.txColorWhite.ToString(), true);
			rewardStr = GameUtility.AppendString(rewardStr, reward.count.ToString(), true);
		}
		rewardLabel.Text = rewardStr;
	}
}