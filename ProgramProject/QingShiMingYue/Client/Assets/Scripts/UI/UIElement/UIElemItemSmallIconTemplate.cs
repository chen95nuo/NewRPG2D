using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemItemSmallIconTemplate : MonoBehaviour
{
	public UIButton gameMoneyButton;
	public UIButton realMoneyButton;
	public UIButton IronButton;
	public UIButton soluButton;
	public UIButton spriteButton;
	public UIButton armHandButton;

	public UIButton staminaButton;
	public UIButton avatarBreakButton;
	public UIButton avatarDomineerButton;
	public UIButton equipmentBreakButton;
	public UIButton changeMeridianButton;
	public UIButton nurseMeridianButton;
	public UIButton medalsButton;

	public UIButton knifeButton;
	public UIButton energyButton;
	public UIButton wineSoulButton;
	public UIButton guildMoneyButton;

	public UIButton avatarUpDan;
	public UIButton bossCountButton;

	public UIButton setPlayerNameIcon;
	public UIButton setGuildNameIcon;


	public bool SetItemSamllIcon(AutoSpriteControlBase btn, int assetId)
	{
		AutoSpriteControlBase target = null;

		switch (assetId)
		{
			case IDSeg._SpecialId.GameMoney: target = gameMoneyButton; break;
			case IDSeg._SpecialId.RealMoney: target = realMoneyButton; break;
			case IDSeg._SpecialId.Iron: target = IronButton; break;
			case IDSeg._SpecialId.Soul: target = soluButton; break;
			case IDSeg._SpecialId.Stamina: target = staminaButton; break;
			case IDSeg._SpecialId.Spirit: target = spriteButton; break;
			case IDSeg._SpecialId.TrialStamp: target = armHandButton; break;
			case IDSeg._SpecialId.Medals: target = medalsButton; break;
			case IDSeg._SpecialId.Energy: target = energyButton; break;
			case IDSeg._SpecialId.WineSoul: target = wineSoulButton; break;
			case IDSeg._SpecialId.GuildMoney: target = guildMoneyButton; break;
			case IDSeg._SpecialId.GuildBossCount: target = bossCountButton; break;
		}

		if (IDSeg.ToAssetType(assetId) == IDSeg._AssetType.Item)
		{
			if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.avatarBreakThroughItemId)
				target = avatarBreakButton;
			else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.domineerItemId)
				target = avatarDomineerButton;
			else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.changeMeridianItemId)
				target = changeMeridianButton;
			else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.nurseMeridianItemId)
				target = nurseMeridianButton;
			else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.knifeCoin)
				target = knifeButton;
			else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.equipmentBreakThroughItemId)
				target = equipmentBreakButton;
			else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.avatarUpDan)
				target = avatarUpDan;
			else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.setPlayerNameId)
				target = setPlayerNameIcon;
			else if (assetId == ConfigDatabase.DefaultCfg.ItemConfig.setGuildNameId)
				target = setGuildNameIcon;
		}

		if (target == null)
		{
			Debug.Log("Set Small AssetIcon Fail , Are You Set the Wrong Id for a AssetItem ?");
			return false;
		}
		else
			UIUtility.CopyIcon(btn, target);

		return true;
	}
}