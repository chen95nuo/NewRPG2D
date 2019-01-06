using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlAssistant : UIModule
{
	public List<UIButton> tabBtns;
	public List<UIBox> tabBoxs;
	public UIButton closeBtn;

	private int showType;
	public int ShowType { get { return showType; } }

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabBtns[0].Data = _UIType.UIPnlAssistantTask;//小助手
		tabBtns[1].Data = _UIType.UIPnlAssistantDailyTask;//每日任务
		tabBtns[2].Data = _UIType.UIPnlAssistantFixedTask;//固定任务
		tabBtns[3].Data = _UIType.UIPnlFreshmanAdvise;//帮助
		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		return true;
	}

	public void ChangeTab(int uiType)
	{
		this.showType = uiType;

		foreach (var btn in tabBtns)
			btn.controlIsEnabled = ((int)btn.data) != uiType;

		ShowTabButtonCount();

		SysUIEnv.Instance.ShowUIModule(showType);
	}

	public void ShowTabButtonCount()
	{
		int[] tabCount = new int[3] { 0, 0, 0 };
		tabCount[0] = SysLocalDataBase.Inst.LocalPlayer.TaskData.Tasks != null ? SysLocalDataBase.Inst.LocalPlayer.TaskData.Tasks.Count : 0;
		tabCount[1] = SysLocalDataBase.Inst.LocalPlayer.QuestData.QuestQuick != null ? SysLocalDataBase.Inst.LocalPlayer.QuestData.QuestQuick.CanPickDailyQuestsCount : 0;
		tabCount[2] = SysLocalDataBase.Inst.LocalPlayer.QuestData.QuestQuick != null ? SysLocalDataBase.Inst.LocalPlayer.QuestData.QuestQuick.CanPickRepeatedQuestsCount : 0;

		switch (showType)
		{
			case _UIType.UIPnlAssistantTask:
				tabCount[0] = 0;
				break;
			case _UIType.UIPnlAssistantDailyTask:
				tabCount[1] = 0;
				break;
			case _UIType.UIPnlAssistantFixedTask:
				tabCount[2] = 0;
				break;
		}

		for (int i = 0; i < tabBoxs.Count; i++)
		{
			tabBoxs[i].Hide(tabCount[i] <= 0);
			tabBoxs[i].Text = tabCount[i].ToString();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeTaye(UIButton btn)
	{
		ChangeTab((int)btn.Data);
	}
}
