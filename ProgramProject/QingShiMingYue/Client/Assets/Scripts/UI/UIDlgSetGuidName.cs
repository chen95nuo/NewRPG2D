using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgSetGuidName : UIModule
{
	public SpriteText titleLabel;
	public SpriteText playerNameLabel;
	public UITextField playerName;
	public UIElemAssetIcon costIcon;
	public SpriteText costNumber;
	public SpriteText costLabel;

	public GameObject setNameObject;
	public GameObject cfObject;

	public SpriteText cfMessageLabel;
	public SpriteText setOrcfLabel;

	private string CfName;
	private bool SetOrCF;

	private enum ConsumeCostType
	{
		NotConsume,
		ConsumeItem,
		ConsumeCost,
		Unknow
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		//Clean Data.
		playerName.Text = string.Empty;
		CfName = string.Empty;
		SetOrCF = true;

		//Show UI.
		SetHide();
		SetUIShow();

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickHide(UIButton btn)
	{
		if (SetOrCF)
			HideSelf();
		else
		{
			SetOrCF = true;
			SetHide();
			SetUIShow();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSetName(UIButton btn)
	{
		if (SetOrCF)
		{
			if (playerName.Text.TrimEnd().Equals("") || playerName.Text.TrimEnd().Equals(string.Empty))
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgSetName_InputLabel"));
			else
			{
				SetOrCF = false;
				CfName = playerName.Text.TrimEnd();
				SetHide();
				SetUIShow();
			}
		}
		else
		{
			Debug.Log("PlayerName = " + CfName);
			RequestMgr.Inst.Request(new SetGuildNameReq(CfName));
			HideSelf();
		}
	}

	//判断使用那个进行修改名字
	private ConsumeCostType JudgeItemOrCost()
	{
		var config = ConfigDatabase.DefaultCfg.ChangeNameConfig;
		if (config == null)
			Debug.Log("[ChangeNameConfig] is not find!");

		if (config.ChangeGuildNameItem == null && (config.ChangeGuildNameCosts == null || config.ChangeGuildNameCosts.Count < 1))
			return ConsumeCostType.NotConsume;

		if (config.ChangeGuildNameItem != null &&
			SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(config.ChangeGuildNameItem.id) != null &&
			SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(config.ChangeGuildNameItem.id).Amount >= config.ChangeGuildNameItem.count)
			return ConsumeCostType.ConsumeItem;

		if (config.ChangeGuildNameCosts != null && config.ChangeGuildNameCosts.Count >= 1)
			return ConsumeCostType.ConsumeCost;

		return ConsumeCostType.Unknow;
	}

	private void SetHide()
	{
		cfObject.SetActive(!SetOrCF);
		setNameObject.SetActive(SetOrCF);

		if (SetOrCF)
		{
			titleLabel.Text = GameUtility.GetUIString("UIDlgSetGuidName_TitleLabel1");
			setOrcfLabel.Text = GameUtility.GetUIString("UIDlgSetName_GaiMing");
		}
		else
		{
			titleLabel.Text = GameUtility.GetUIString("UIDlgSetName_TitleLabel2");
			setOrcfLabel.Text = GameUtility.GetUIString("UIDlgSetName_QueRen");
		}
	}

	private void SetUIShow()
	{
		var config = ConfigDatabase.DefaultCfg.ChangeNameConfig;
		if (config == null)
			Debug.Log("ChangeNameConfig is not find!");

		if (SetOrCF)
		{
			playerNameLabel.Text = GameUtility.FormatUIString("UIDlgSetGuidName_Label", SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildName);
			switch (JudgeItemOrCost())
			{
				case ConsumeCostType.Unknow:
					Debug.Log("Config Error!");
					break;
				case ConsumeCostType.NotConsume:
					costIcon.Hide(true);
					costNumber.Text = string.Empty;
					costLabel.Hide(true);
					break;
				case ConsumeCostType.ConsumeCost:
					costIcon.SetData(config.ChangeGuildNameCosts[0].id);
					costIcon.Hide(false);
					costLabel.Hide(false);
					costNumber.Text = GameUtility.FormatUIString("UIDlgSetName_CostNumber", config.ChangeGuildNameCosts[0].count);
					break;
				case ConsumeCostType.ConsumeItem:
					costIcon.SetData(config.ChangeGuildNameItem.id);
					costIcon.Hide(false);
					costLabel.Hide(false);
					costNumber.Text = GameUtility.FormatUIString("UIDlgSetName_CostNumber", config.ChangeGuildNameItem.count);
					break;
			}
		}
		else
		{
			switch (JudgeItemOrCost())
			{
				case ConsumeCostType.Unknow:
					Debug.Log("Config Error!");
					break;
				case ConsumeCostType.NotConsume:
					cfMessageLabel.Text = GameUtility.FormatUIString("UIDlgSetGuidName_CostLabel3", SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildName, CfName);
					break;
				case ConsumeCostType.ConsumeCost:
					cfMessageLabel.Text = GameUtility.FormatUIString("UIDlgSetGuidName_CostLabel1", config.ChangeGuildNameCosts[0].count,
																							ItemInfoUtility.GetAssetName(config.ChangeGuildNameCosts[0].id),
																							SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildName,
																							CfName);
					break;
				case ConsumeCostType.ConsumeItem:
					cfMessageLabel.Text = GameUtility.FormatUIString("UIDlgSetGuidName_CostLabel2", ItemInfoUtility.GetAssetName(config.ChangeGuildNameItem.id),
																							config.ChangeGuildNameItem.count,
																							SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildName,
																							CfName);
					break;
			}
		}
	}
}