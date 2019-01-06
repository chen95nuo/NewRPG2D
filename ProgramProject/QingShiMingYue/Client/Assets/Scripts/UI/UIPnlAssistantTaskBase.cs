using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlAssistantTaskBase : UIModule
{
	public UIScrollList taskList;
	public GameObjectPool taskItemPool;
	public SpriteText emptyLabel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		taskList.ClearList(false);
		taskList.ScrollPosition = 0;
	}

	private UIListItemContainer GetUIListitemByQuestID(int questID)
	{
		for (int index = 0; index < taskList.Count; index++)
		{
			UIListItemContainer container = taskList.GetItem(index) as UIListItemContainer;
			if ((container.Data as UIElemTaskItem).Quest.QuestId == questID)
				return container;
		}

		return null;
	}

	public void OnSyncUI(List<KodGames.ClientClass.Quest> changedLists, KodGames.ClientClass.Reward reward)
	{
		string rewardDesc = SysLocalDataBase.GetRewardFormatDesc(reward);
		if (string.IsNullOrEmpty(rewardDesc) == false)
		{
			UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
			showData.SetData(rewardDesc, true, true);

			// Set Reward Message hide Del.
			showData.OnHideCallback = UpdateTaskData;
			showData.onHideCallbackObj = changedLists;

			SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlTip>().ShowTip(showData);
		}

		// Refresh View.
		var currentShowType = SysUIEnv.Instance.GetUIModule<UIPnlAssistant>().ShowType;
		int currentQuestType = currentShowType == _UIType.UIPnlAssistantDailyTask ? QuestConfig._Type.DailyQuest : QuestConfig._Type.RepeatedQuest;
		foreach (var quest in changedLists)
		{
			if (ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(quest.QuestId).type != currentQuestType)
				continue;

			UIListItemContainer container = GetUIListitemByQuestID(quest.QuestId);

			bool canShow = quest.Phase >= QuestConfig._PhaseType.Active && quest.Phase <= QuestConfig._PhaseType.FinishedAndGotReward;

			if (container == null)
			{
				if (canShow)
				{
					UIElemTaskItem item = taskItemPool.AllocateItem().GetComponent<UIElemTaskItem>();
					item.SetData(quest);

					taskList.AddItem(item.gameObject);
				}
			}
			else
			{
				if (canShow)
				{
					(container.Data as UIElemTaskItem).SetData(quest);
				}
			}
		}

		// Refresh Delete or Complement Quest if Not Show Reward Tips.
		if (string.IsNullOrEmpty(rewardDesc))
			UpdateTaskData(changedLists);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnClickIcon(UIButton btn)
	{
		UIElemAssetIcon item = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(item.AssetId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnClickGoto(UIButton btn)
	{
		KodGames.ClientClass.Quest quest = btn.Data as KodGames.ClientClass.Quest;
		QuestConfig.Quest questConfig = ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(quest.QuestId);

		if (questConfig.eventType == QuestConfig._EventType.UseStaminaItem)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAffordCost), IDSeg._SpecialId.Stamina);
		else if (questConfig.eventType == QuestConfig._EventType.WebPage)
		{
			Application.OpenURL(GameUtility.GetUIString("UIPnlCentralCityPlayerInfo_WebPageUrl"));
			RequestMgr.Inst.Request(new FacebookRewardReq());
		}
		else
			GameUtility.JumpUIPanel(questConfig.gotoUI);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnClickFinsh(UIButton btn)
	{
		KodGames.ClientClass.Quest quest = btn.Data as KodGames.ClientClass.Quest;
		RequestMgr.Inst.Request(new PickQuestRewardReq(quest.QuestId, OnSyncUI));
	}

	protected bool UpdateTaskData(object obj)
	{
		List<KodGames.ClientClass.Quest> changedLists = obj as List<KodGames.ClientClass.Quest>;

		// Update TabButtonCount.
		SysUIEnv.Instance.GetUIModule<UIPnlAssistant>().ShowTabButtonCount();

		var currentShowType = SysUIEnv.Instance.GetUIModule<UIPnlAssistant>().ShowType;
		int currentQuestType = currentShowType == _UIType.UIPnlAssistantDailyTask ? QuestConfig._Type.DailyQuest : QuestConfig._Type.RepeatedQuest;

		List<UIListItemContainer> deleteContainers = new List<UIListItemContainer>();
		List<UIListItemContainer> complementContainers = new List<UIListItemContainer>();

		foreach (var quest in changedLists)
		{
			if (ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(quest.QuestId).type != currentQuestType)
				continue;

			UIListItemContainer container = GetUIListitemByQuestID(quest.QuestId);

			bool canShow = ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(quest.QuestId).notHideWhenFinished ||
						  (quest.Phase >= QuestConfig._PhaseType.Active && quest.Phase <= QuestConfig._PhaseType.FinishedAndGotReward);

			if (container != null)
			{
				if (canShow)
				{
					if (quest.Phase >= QuestConfig._PhaseType.FinishedAndGotReward)
						complementContainers.Add(container);
				}
				else
					deleteContainers.Add(container);
			}
		}

		foreach (var deleteItem in deleteContainers)
			taskList.RemoveItem(deleteItem, false);

		if (complementContainers.Count < SysLocalDataBase.Inst.LocalPlayer.QuestData.Quests.Count)
		{
			foreach (var complementItem in complementContainers)
			{
				UIElemTaskItem item = taskItemPool.AllocateItem().GetComponent<UIElemTaskItem>();
				item.SetData((complementItem.Data as UIElemTaskItem).Quest);

				taskList.AddItem(item.container);
			}

			foreach (var complementItem in complementContainers)
				taskList.RemoveItem(complementItem, false);
		}

		return true;
	}
}
