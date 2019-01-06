using System;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;
using UnityEngine;

public class UIDlgOrganGet : UIModule
{	
	public SpriteText descLabel;
	public UIBox beastType;
	public UIElemAssetIcon organIcon;
	public SpriteText organName;
	public SpriteText titleLabel;

	private KodGames.ClientClass.Beast beastInfo;
	private BeastConfig.BaseInfo beastCfg;
	private bool isScroller;
	private bool isDecompose;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		if (userDatas[0] is KodGames.ClientClass.Beast)
		{
			beastInfo = userDatas[0] as KodGames.ClientClass.Beast;
			beastCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastInfo.ResourceId);
			organIcon.SetData(beastInfo);
			this.isScroller = false;
			this.isDecompose = false;
		}
		else if(userDatas.Length > 1)
		{
			beastCfg = userDatas[0] as BeastConfig.BaseInfo;			
			organIcon.SetData(beastCfg.FragmentId);

			this.isDecompose = (bool)userDatas[1];
			this.isScroller = true;
		}

		if (isScroller)
		{
			organName.Text = string.Format(GameUtility.GetUIString("UIPnlOrganGet_OrganBeast_Name"), beastCfg.BeastName);

			if (isDecompose)
				titleLabel.Text = GameUtility.GetUIString("UIPnlOrganInfo_OrganBeast_ScrollerDecompose");
			else
				titleLabel.Text = GameUtility.GetUIString("UIPnlOrganInfo_OrganBeast_ScrollerTitle");

			string typeDesc = GameUtility.GetUIString("UIPnlOrganInfo_OrganBeast_" + BeastConfig._BeastTraitType.GetNameByType(beastCfg.BeastType));

			descLabel.Text = string.Format(GameUtility.GetUIString("UIPnlOrganInfo_OrganBeast_Active"), typeDesc, beastCfg.BeastName);
		}
		else
		{
			organName.Text = beastCfg.BeastName;
			titleLabel.Text = GameUtility.GetUIString("UIPnlOrganInfo_OrganBeast_ActiveSuccessTitle");
			string typeDesc = GameUtility.GetUIString("UIPnlOrganInfo_OrganBeast_" + BeastConfig._BeastTraitType.GetNameByType(beastCfg.BeastType));
			descLabel.Text = string.Format(GameUtility.GetUIString("UIPnlOrganInfo_OrganBeast_Active"), typeDesc, ItemInfoUtility.GetAssetName(beastInfo.ResourceId));
		}

		UIElemTemplate.Inst.SetBeastTraitIcon(beastType, beastCfg.BeastType);
		beastType.Hide(false);

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackBtn(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGoToSeeBtn(UIButton btn)
	{		
		if(isScroller)
		{
			SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlOrganInfo), _UILayer.Top, beastCfg.Id, true);
		}
		else
			SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlOrganInfo), _UILayer.Top, beastInfo.ResourceId, true);

		HideSelf();
	}
}