using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgSetName : UIModule
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

	private string CfName;	//保存名称
	private bool SetOrCF;	//标识是输入还是确认

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

		//ShowUI.
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
				if (playerName.Text.TrimEnd().Length > ConfigDatabase.DefaultCfg.GameConfig.playerNameMax)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UITutorial_CreateAvatar_InValidNameLength"));
				else if (playerName.Text.TrimEnd().IndexOf(" ") >= 0)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UITutorial_CreateAvatar_InValidNameFormat"));
				else
				{
					SetOrCF = false;
					CfName = playerName.Text.TrimEnd();
					SetHide();
					SetUIShow();
				}
			}
		}
		else
		{
			Debug.Log("PlayerName = " + CfName);
			RequestMgr.Inst.Request(new SetPlayerNameReq(CfName));
			HideSelf();
		}
	}



	//判断使用那个进行修改名字
	private ConsumeCostType JudgeItemOrCost()
	{
		var config = ConfigDatabase.DefaultCfg.ChangeNameConfig;
		if (config == null)
			Debug.Log("[ChangeNameConfig] is not find!");

		if (config.ChangePlayNameItem == null && (config.ChangePlayNameCosts == null || config.ChangePlayNameCosts.Count < 1))
			return ConsumeCostType.NotConsume;

		if (config.ChangePlayNameItem != null &&
			SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(config.ChangePlayNameItem.id) != null &&
			SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(config.ChangePlayNameItem.id).Amount >= config.ChangePlayNameItem.count)
			return ConsumeCostType.ConsumeItem;

		if (config.ChangePlayNameCosts != null && config.ChangePlayNameCosts.Count >= 1)
			return ConsumeCostType.ConsumeCost;

		return ConsumeCostType.Unknow;
	}

	private void SetHide()
	{
		cfObject.SetActive(!SetOrCF);
		setNameObject.SetActive(SetOrCF);

		if (SetOrCF)
		{
			titleLabel.Text = GameUtility.GetUIString("UIDlgSetName_TitleLabel1");
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
			playerNameLabel.Text = GameUtility.FormatUIString("UIDlgSetName_Label", SysLocalDataBase.Inst.LocalPlayer.Name);
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
					costIcon.SetData(config.ChangePlayNameCosts[0].id);
					costIcon.Hide(false);
					costLabel.Hide(false);
					costNumber.Text = GameUtility.FormatUIString("UIDlgSetName_CostNumber", config.ChangePlayNameCosts[0].count);
					break;
				case ConsumeCostType.ConsumeItem:
					costIcon.SetData(config.ChangePlayNameItem.id);
					costIcon.Hide(false);
					costLabel.Hide(false);
					costNumber.Text = GameUtility.FormatUIString("UIDlgSetName_CostNumber", config.ChangePlayNameItem.count);
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
					cfMessageLabel.Text = GameUtility.FormatUIString("UIDlgSetName_CostLabel3", SysLocalDataBase.Inst.LocalPlayer.Name, CfName);
					break;
				case ConsumeCostType.ConsumeCost:
					cfMessageLabel.Text = GameUtility.FormatUIString("UIDlgSetName_CostLabel1", config.ChangePlayNameCosts[0].count,
																							ItemInfoUtility.GetAssetName(config.ChangePlayNameCosts[0].id),
																							SysLocalDataBase.Inst.LocalPlayer.Name,
																							CfName);
					break;
				case ConsumeCostType.ConsumeItem:
					cfMessageLabel.Text = GameUtility.FormatUIString("UIDlgSetName_CostLabel2", ItemInfoUtility.GetAssetName(config.ChangePlayNameItem.id),
																							config.ChangePlayNameItem.count,
																							SysLocalDataBase.Inst.LocalPlayer.Name,
																							CfName);
					break;
			}
		}
	}
}
