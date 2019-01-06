using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgTutorialReward : UIModule
{
	public UIElemAssetIcon itemIcon;
	public SpriteText title;
	public UIButton proceedBtn;
	public SpriteText explainLabel;

	private int assetId;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
				return false;
		if(userDatas.Length>0)
			assetId = (int)userDatas[0];
	
		SetData();
		return true;
	}

	private void SetData()
	{	
		if (assetId > 0)
		{
			itemIcon.SetData(assetId);
			title.Text = GameUtility.FormatUIString("UIDlgTutorialReward_ItemName", ItemInfoUtility.GetAssetName(assetId));
		}else
			Debug.LogError("AssetId is not Find!");

		SetExplainLabel();
	}

	private void SetExplainLabel()
	{
		if (ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(assetId) != null)
			explainLabel.Text = GameUtility.GetUIString("UITutorial_Equipment");
		else if(ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(assetId) != null)
			explainLabel.Text = GameUtility.GetUIString("UITutorial_Skill");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickToAvatar(UIButton btn)
	{
		HideSelf();

		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlAvatar);
	}
}
