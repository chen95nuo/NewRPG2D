using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgOpenPackage : UIModule
{
	public UIElemOpenPackageChoice leftChoice;
	public UIElemOpenPackageChoice rightChoice;
	public List<UIStateToggleBtn> selects;

	public SpriteText tipLabel;
	public UIButton closeBtn;
	public UIButton getRewardBtn;

	private ItemConfig.Item item;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas.Length < 1)
			return true;

		item = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById((int)userDatas[0]);
		if (item == null)
			return true;

		for (int index = 0; index < selects.Count; index++ )
			selects[index].Data = index;

		SetData(item, item.playerLevel <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level && item.vipLevel <= SysLocalDataBase.Inst.LocalPlayer.VipLevel);

		return true;
	}

	private void SetData(ItemConfig.Item item, bool canUseItem)
	{
		if (item.consumeRewardGroup.Count < 2)
			return;

		leftChoice.SetData(item.consumeRewardGroup[0], canUseItem);
		rightChoice.SetData(item.consumeRewardGroup[1], canUseItem);

		tipLabel.Text = "";
		if (canUseItem == false)
			tipLabel.Text = item.cannotUseTip;

		SetButton(canUseItem);
	}

	private void SetButton(bool canUseItem)
	{
		bool canGetAndHaveChoice = canUseItem && (leftChoice.IsSelected || rightChoice.IsSelected);

		closeBtn.Hide(canGetAndHaveChoice);
		getRewardBtn.Hide(!canGetAndHaveChoice);
	}

	private int GetSelectIndex()
	{
		for (int index = 0; index < selects.Count; index++ )
		{
			if (selects[index].StateName.Equals("On"))
				return index;
		}

		return -1;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnSelectClick(UIStateToggleBtn btn)
	{
		foreach (var s in selects)
			if (s.Data.Equals(btn.Data) == false && s.StateName.Equals("On"))
				s.SetToggleState("Off");

		SetButton(item.playerLevel <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level && item.vipLevel <= SysLocalDataBase.Inst.LocalPlayer.VipLevel);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnCloseClick(UIButton btn)
	{
		SysUIEnv.Instance.HideUIModule(typeof(UIDlgOpenPackage));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnGetRewardClick(UIButton btn)
	{
		int chooseIndex = GetSelectIndex();
		if (chooseIndex < 0 || chooseIndex >= selects.Count)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgPhoneNumberVerify_InvalidInput"));
		else
		{
			bool requirePhoneNumber = false;
			foreach (var reward in item.consumeRewardGroup[chooseIndex].consumeReward)
			{
				if (ItemConfig._Type.ToItemType(reward.id) == ItemConfig._Type.TelFee)
					requirePhoneNumber = true;
			}

			if (requirePhoneNumber)
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPhoneNumberVerify), item.id, chooseIndex);
			else
				RequestMgr.Inst.Request(new ConsumeItemReq(item.id, 1, chooseIndex));
		}
	}
}
