using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgBeforeBattleRecuite : UIModule
{
	public UIScrollList scrollList;
	public GameObjectPool itemPool;
	public GameObjectPool labelPool;
	public SpriteText messageLabel;

	private UIElementDungeonItem.CombatData combatData;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		this.combatData = userDatas[0] as UIElementDungeonItem.CombatData;
		StartCoroutine("InitView", combatData.dungeonID);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		StopCoroutine("InitView");
		ClearData();
	}

	private void ClearData()
	{
		combatData = null;
		scrollList.ClearList(false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator InitView(int dungeonId)
	{
		messageLabel.Text = GameUtility.GetUIString("UIDlgBeforeBattleRecuite_SelectNpcToFight");

		yield return null;

		foreach (var recruiteNpc in SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonRecruiteNpcs(dungeonId))
		{
			UIElemRecuiteBeforeBattle item = itemPool.AllocateItem().GetComponent<UIElemRecuiteBeforeBattle>();
			item.SetData(recruiteNpc);

			scrollList.AddItem(item.gameObject);
		}

		UIElemDlgBeforeBattleRecuiteDesc label = labelPool.AllocateItem().GetComponent<UIElemDlgBeforeBattleRecuiteDesc>();
		label.SetData(dungeonId);
		scrollList.AddItem(label.gameObject);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickNpcItem(UIButton btn)
	{
		// 新手连点,奔溃.原因是combatData为null.
		if (combatData == null)
			return;

		// Set Npc Id.
		combatData.recruiteNpc = btn.Data as KodGames.ClientClass.RecruiteNpc;

		// Show LineUp UI.
		if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(combatData.zoneID))
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.ActivityCampaign, combatData);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.Campaign, combatData);

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}