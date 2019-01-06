using System;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;
using UnityEngine;

public class UIDlgOrganGetWay : UIModule
{	
	public SpriteText descLabel;		
	public SpriteText titleLabel;
	public UIElemBeastGotoItem getwayItem;

	private BeastConfig.BaseInfo baseInfo;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		KodGames.ClientClass.Beast beastInfo = userDatas[0] as KodGames.ClientClass.Beast;
		baseInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastInfo.ResourceId);

		titleLabel.Text = GameUtility.GetUIString("UIDlgOrganGetWay_GetWayTitle");				
		descLabel.Text = string.Format(GameUtility.GetUIString("UIDlgOrganGetWay_GetWayTitle_Message"), ItemInfoUtility.GetAssetName(beastInfo.ResourceId));


		if (baseInfo.GetWay != null && baseInfo.GetWay.type != _UIType.UnKonw)
			getwayItem.SetData(baseInfo.GetWay);
		else
			getwayItem.SetData(baseInfo.Id);

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackBtn(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoBtn(UIButton btn)
	{
		ClientServerCommon.GetWay getway = this.baseInfo.GetWay;

		if (getway.type != _UIType.UI_ActivityDungeon && getway.type != _UIType.UI_Dungeon)
			GameUtility.JumpUIPanel(getway.type);
		else
			GameUtility.JumpUIPanel(getway.type, getway.data);
	}
}