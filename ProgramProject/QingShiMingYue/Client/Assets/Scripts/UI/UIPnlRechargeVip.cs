using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlRechargeVip : UIModule
{
	public UIScrollList vipPowerList;
	public GameObjectPool vipPowerPool;
	public SpriteText rechargeLabel;
	public AutoSpriteControlBase backBtn;

	public UIProgressBar levelBar;
	public SpriteText myVipLevel;
	public SpriteText levelBar_text;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// 不在主城中 显示背景板
		backBtn.Hide(SysGameStateMachine.Instance.CurrentState is GameState_CentralCity);

		SetControl();
		StartCoroutine("FillList");

		return true;
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	public override void RemoveOverlay()
	{
		SetControl();

		for (int index = 0; index < vipPowerList.Count; index++)
		{
			UIListItemContainer container = vipPowerList.GetItem(index) as UIListItemContainer;
			if (container == null)
				continue;

			UIElemRechargeVip vipItem = container.gameObject.GetComponent<UIElemRechargeVip>();
			if (vipItem == null)
				continue;
			//vipItem.ResetBG();
		}

		base.RemoveOverlay();
	}

	private void SetControl()
	{
		// Self vip label
		int level = SysLocalDataBase.Inst.LocalPlayer.VipLevel >= ConfigDatabase.DefaultCfg.VipConfig.maxVipLevel - 1 ? ConfigDatabase.DefaultCfg.VipConfig.maxVipLevel - 1 : SysLocalDataBase.Inst.LocalPlayer.VipLevel;
		myVipLevel.Text = GameUtility.FormatUIString("UIRankingTab_Txt_MyVipLevel", SysLocalDataBase.Inst.LocalPlayer.VipLevel);
		int levelCostRMB = ConfigDatabase.DefaultCfg.VipConfig.GetVipByLevel(level).costRMB;
		int myVipLevelCostRMB = SysLocalDataBase.Inst.LocalPlayer.RemainingCostRMB >= levelCostRMB ? levelCostRMB : SysLocalDataBase.Inst.LocalPlayer.RemainingCostRMB;
		if (levelCostRMB != 0)
			levelBar.Value = (float)myVipLevelCostRMB / (float)levelCostRMB;
		else
			levelBar.Value = 0;
		if (SysLocalDataBase.Inst.LocalPlayer.VipLevel >= ConfigDatabase.DefaultCfg.VipConfig.maxVipLevel)
		{
			rechargeLabel.Text = GameUtility.FormatUIString("UIRankingTab_Txt_CanNotVipPrivilegeLabel", GameDefines.textColorBtnYellow_font);
			levelBar.Value = 1;
			myVipLevelCostRMB = levelCostRMB;
		}
		else
		{
			float costRMB = (ClientServerCommon.ConfigDatabase.DefaultCfg.VipConfig.GetVipByLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel).costRMB - SysLocalDataBase.Inst.LocalPlayer.RemainingCostRMB) / (float)ConfigDatabase.DefaultCfg.AppleGoodConfig.multiplyingPower;
			rechargeLabel.Text = GameUtility.FormatUIString("UIRankingTab_Txt_VipPrivilegeLabel_CZ",
				GameDefines.textColorBtnYellow_font,
				GameDefines.textColorBtnBlue_font,
				costRMB.ToString(ItemInfoUtility.GetDecimalMedianByType(ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType)),
				ItemInfoUtility.GetCurrencyNameByType(ConfigDatabase.DefaultCfg.AppleGoodConfig.currencyType),
				GameDefines.textColorBtnYellow_font,
				GameDefines.textColorBtnBlue_font,
				SysLocalDataBase.Inst.LocalPlayer.VipLevel + 1,
				GameDefines.textColorBtnYellow_font
				);
		}
		levelBar_text.Text = myVipLevelCostRMB + "/" + levelCostRMB;

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		// Fill list
		foreach (VipConfig.Vip vip in ConfigDatabase.DefaultCfg.VipConfig.vipList)
		{
			if (vip.level == 0 || vip.level > ConfigDatabase.DefaultCfg.VipConfig.maxVipLevel)
				continue;

			UIListItemContainer container = vipPowerPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemRechargeVip vipItem = container.gameObject.GetComponent<UIElemRechargeVip>();
			vipItem.SetData(vip);
			container.ScanChildren();
			vipPowerList.AddItem(container);
		}

		// Scroll current vip level
		if (SysLocalDataBase.Inst.LocalPlayer.VipLevel >= 1)
			vipPowerList.ScrollToItem(SysLocalDataBase.Inst.LocalPlayer.VipLevel - 1, 0f);
		else
			vipPowerList.ScrollListTo(0f);
	}

	private void ClearList()
	{
		StopCoroutine("FillList");
		vipPowerList.ClearList(false);
		vipPowerList.ScrollListTo(0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		ClearList();
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRechargeClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlRecharge);
		SysUIEnv.Instance.HideUIModule(_UIType.UIPnlRechargeVip);
	}
}
