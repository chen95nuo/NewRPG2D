using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIPnlAssistantDailyTask : UIPnlAssistantTaskBase
{
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		emptyLabel.Text = string.Empty;

		SysUIEnv.Instance.GetUIModule<UIPnlAssistant>().ChangeTab(_UIType.UIPnlAssistantDailyTask);

		InitView();

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	void ClearData()
	{
		StopCoroutine("FillList");
		taskList.ClearList(false);
		taskList.ScrollPosition = 0;
	}

	public void InitView()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.DailyQuestReward))
			StartCoroutine("FillList");
		else if (SysLocalDataBase.Inst.LocalPlayer.QuestData.Quests == null)
			RequestMgr.Inst.Request(new QueryQuestInfoReq(InitView));
		else
			StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	IEnumerator FillList()
	{
		yield return null;

		emptyLabel.Text = string.Empty;

		if (SysLocalDataBase.Inst.LocalPlayer.QuestData.Quests == null)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlAssistant_DilyLevel_tips"));
			emptyLabel.Text = GameUtility.GetUIString("UIPnlAssistant_DilyLabel");
		}
		else
		{
			//由本地数据获取每日任务列表临时存储一份
			List<KodGames.ClientClass.Quest> quests = new List<KodGames.ClientClass.Quest>();
			foreach (var quest in SysLocalDataBase.Inst.LocalPlayer.QuestData.Quests)
			{
				if ((ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(quest.QuestId).type != QuestConfig._Type.DailyQuest) ||
					(
					!ConfigDatabase.DefaultCfg.QuestConfig.GetQuestById(quest.QuestId).notHideWhenFinished &&
					(quest.Phase == QuestConfig._PhaseType.Inactive || quest.Phase >= QuestConfig._PhaseType.FinishedAndHiden)
					) ||
					!ConfigDatabase.DefaultCfg.QuestConfig.GetQuestShowLevelControl(quest.QuestId, SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level))
					continue;

				quests.Add(quest);
			}

			//对链表进行排序
			quests.Sort(DataCompare.CompareQuestData);

			foreach (var quest in quests)
			{
				UIElemTaskItem item = taskItemPool.AllocateItem().GetComponent<UIElemTaskItem>();
				item.SetData(quest);
				taskList.AddItem(item.gameObject);
			}
		}


		// Set Empty Label.
		if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.DailyQuestReward))
			emptyLabel.Text = GameUtility.GetUIString("UIPnlAssistant_DilyLabel");
		else
			if (taskList.Count <= 0)
				emptyLabel.Text = GameUtility.GetUIString("UIPnlAssistant_DilyLabel_Not");
	}
}