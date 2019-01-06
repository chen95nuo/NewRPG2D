using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIPnlDanInfo : UIPnlItemInfoBase
{
	public delegate void SelectDelegate(KodGames.ClientClass.Dan selected);
	//public SpriteText danNameLabel;

	public UIElemAssetIcon danIcon;

	public UIScrollList suiteList;
	public SpriteText danDescLabel;
	public SpriteText danLevelLabel;

	public SpriteText[] danAttrs;

	public UIButton changeDanBtn;
	public UIButton powerUpBtn;
	public UIButton okBtn;
	public UIButton selectBtn;
	public UIButton closeBtn;
	public UIBox notifIcon;

	private KodGames.ClientClass.Dan danData = new KodGames.ClientClass.Dan();
	private com.kodgames.corgi.protocol.ShowReward showRewardData = new com.kodgames.corgi.protocol.ShowReward();
	private KodGames.ClientClass.Location location;
	private KodGames.ClientClass.Avatar avatar;

	private int danType = 0;
	private int atticTabType = -1;
	private SelectDelegate selectDel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SetNotifIcon(true);

		if (userDatas[0] is KodGames.ClientClass.Dan)
		{
			danData = userDatas[0] as KodGames.ClientClass.Dan;
			if (userDatas.Length > 1)
			{
				atticTabType = (int)userDatas[1];
				if (userDatas.Length > 2)
				{
					if (userDatas.Length > 7)
						selectDel = userDatas[7] as SelectDelegate;
					else
						selectDel = null;
				}
				ShowDanInfoButton((bool)userDatas[2], (bool)userDatas[3], (bool)userDatas[4], (bool)userDatas[5], (bool)userDatas[6]);
			}
			else
			{
				if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanAlchemy)))
					ShowDanInfoButton(false, false, true, false, false);
				else
					ShowDanInfoButton(false, true, false, false, true);
			}
			ShowKodDanInfo();

		}
		else if (userDatas[0] is com.kodgames.corgi.protocol.ShowReward)
		{
			showRewardData = userDatas[0] as com.kodgames.corgi.protocol.ShowReward;
			ShowDanRewardInfo();
			ShowDanInfoButton(false, false, true, false, false);
		}
		else if (userDatas[0] is ClientServerCommon.Reward)
		{
			var ccReward = userDatas[0] as ClientServerCommon.Reward;
			showRewardData = new com.kodgames.corgi.protocol.ShowReward();
			showRewardData.id = ccReward.id;
			showRewardData.breakthought = ccReward.breakthoughtLevel;
			showRewardData.level = ccReward.level;

			for (int i = 0; i < ccReward.breakthoughtLevel; i++)
				showRewardData.attributeIds.Add(-1);

			ShowDanInfoButton(false, false, true, false, false);

			ShowDanRewardInfo();
		}
		else if (userDatas[0] is KodGames.ClientClass.Location)
		{
			location = userDatas[0] as KodGames.ClientClass.Location;
			danData = SysLocalDataBase.Inst.LocalPlayer.SearchDan(location.Guid);
			if (userDatas.Length > 1)
			{
				danType = (int)userDatas[1];
				avatar = userDatas[2] as KodGames.ClientClass.Avatar;
				ShowDanInfoButton((bool)userDatas[3], (bool)userDatas[4], (bool)userDatas[5], (bool)userDatas[6], (bool)userDatas[7]);
			}
			ShowKodDanInfo();
		}

		return true;
	}

	public void UpdateDanData(KodGames.ClientClass.Dan dan)
	{
		danData = dan;
		ShowKodDanInfo();
	}

	public override void OnHide()
	{
		atticTabType = -1;
		base.OnHide();
	}

	//用于展示礼包可兑换的肯能带有固定属性或者随即属性的内丹
	private void ShowDanRewardInfo()
	{
		danIcon.SetData(showRewardData.id, showRewardData.breakthought, showRewardData.level);

		int index = 0;

		for (int i = 0; i < danAttrs.Length; i++)
		{
			danAttrs[i].Text = string.Format(GameUtility.GetUIString("UIPnlDanInfo_Attribute1_Label"), GameDefines.textColorDackGray, ItemInfoUtility.GetAssetQualityColor(i + 1), ItemInfoUtility.GetDanTextQuality(i + 1), GameDefines.textColorDackGray);

			if (i < showRewardData.attributeIds.Count)
			{
				if (showRewardData.attributeIds[i] <= 0)
					danAttrs[i].Text = string.Format(GameUtility.GetUIString("UIPnlDanInfo_RandomAttr_Label"), GameDefines.textColorBtnYellow);
				else
				{
					string attr = "";
					//描述
					attr = attr + showRewardData.danAttributeGroups[index].attributeDesc;
					//数值
					attr = attr + string.Format(GameUtility.GetUIString("UIPnlDanInfo_UpAttr_Label"), System.Math.Round(showRewardData.danAttributeGroups[index].danAttributes[0].propertyModifierSets[showRewardData.level - 1].modifiers[0].attributeValue * 100, 3));
					danAttrs[i].Text = attr;
					index++;
				}
			}
		}

		danLevelLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanInfo_DanLevel_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, showRewardData.level, GameDefines.textColorBtnYellow);
		danDescLabel.Text = ItemInfoUtility.GetAssetDesc(showRewardData.id, showRewardData.breakthought);

		suiteList.RepositionItems();
		suiteList.ScrollListTo(0f);
	}

	//用于显示玩家已经抽到的丹，无随即属性显示
	private void ShowKodDanInfo()
	{
		if (danIcon != null && danData != null)
			danIcon.SetData(danData.ResourceId, danData.BreakthoughtLevel, danData.LevelAttrib.Level);

		for (int i = 0; i < danAttrs.Length; i++)
		{
			danAttrs[i].Text = string.Format(GameUtility.GetUIString("UIPnlDanInfo_Attribute1_Label"), GameDefines.textColorDackGray, ItemInfoUtility.GetAssetQualityColor(i + 1), ItemInfoUtility.GetDanTextQuality(i + 1), GameDefines.textColorDackGray);
			//玩家已抽到的
			if (danData.DanAttributeGroups != null && i < danData.DanAttributeGroups.Count)
				danAttrs[i].Text = ItemInfoUtility.GetDanAttributeDesc(danData.DanAttributeGroups[i], danData.LevelAttrib.Level);
		}

		danLevelLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanInfo_DanLevel_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, danData.LevelAttrib.Level, GameDefines.textColorBtnYellow);
		danDescLabel.Text = ItemInfoUtility.GetAssetDesc(danData.ResourceId, danData.BreakthoughtLevel);

		if (!powerUpBtn.IsHidden())
			SetNotifIcon(!ItemInfoUtility.IsAbilityUpImprove_Dan(danData));

		suiteList.RepositionItems();
		suiteList.ScrollListTo(0f);
	}

	private void ShowDanInfoButton(bool change, bool powerUp, bool ok, bool select, bool close)
	{
		changeDanBtn.Hide(!change);
		powerUpBtn.Hide(!powerUp);
		okBtn.Hide(!ok);
		selectBtn.Hide(!select);
		closeBtn.Hide(!close);
	}

	private void SetNotifIcon(bool isHide)
	{
		if (notifIcon != null)
		{
			notifIcon.Hide(isHide);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChange(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIPnlSelectDanList>(danType, location, avatar);
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		if (atticTabType != -1)
		{
			if (!SysUIEnv.Instance.GetUIModule<UIPnlDanAttic>().IsShown)
				SysUIEnv.Instance.ShowUIModule<UIPnlDanAttic>(atticTabType);
			else
				SysUIEnv.Instance.GetUIModule<UIPnlDanAttic>().UpdateShowUI(atticTabType);
		}
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPowerUp(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIPnlDanCultureTab>(btn.Data != null ? btn.Data as KodGames.ClientClass.Dan : danData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnDanHelpInfoClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanIntroduce), DanConfig._IntroduceType.DanAttri);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelect(UIButton btn)
	{
		if (selectDel != null)
			selectDel(danData);

		HideSelf();
	}
}