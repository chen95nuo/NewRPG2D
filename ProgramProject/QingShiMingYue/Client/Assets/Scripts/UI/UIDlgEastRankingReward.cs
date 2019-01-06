using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;
using System;
using KodGames;

public class UIDlgEastRankingReward : UIModule
{

	public GameObjectPool gameObjectPool;
	public UIScrollList scrollList;
	public SpriteText titleLabel;
	public SpriteText giftLabel;
	public UIButton okBtn;
	public UIButton clostBtn;

	private const int MAX_COLUMN_NUM = 3;
	private List<Pair<int, int>> rewardPackagePars;
	private KodGames.ClientClass.Reward reward;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;
		if (userDatas != null && userDatas.Length > 0)
		{
			if (userDatas[0] is KodGames.ClientClass.Reward)
			{
				reward = userDatas[0] as KodGames.ClientClass.Reward;
				rewardPackagePars = SysLocalDataBase.ConvertIdCountList(reward);
			}
			else if (userDatas[0] is List<Pair<int, int>>)
			{
				rewardPackagePars = userDatas[0] as List<Pair<int, int>>;
			}

			FillData();
			titleLabel.Text = userDatas.Length > 1 && userDatas[1] != null ? userDatas[1].ToString() : GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_GongXiGet_Title");
			giftLabel.Text = userDatas.Length > 2 && userDatas[2] != null ? userDatas[2].ToString() : GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_HeChengGet_Title");
		}
		return true;
	}


	private void ClearData()
	{
		scrollList.ClearList(false);
		titleLabel.Text = "";
		giftLabel.Text = "";

	}
	private void FillData()
	{
		UIListItemContainer container = gameObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemShopGiftItem item = container.GetComponent<UIElemShopGiftItem>();
		container.Data = item;

		for (int i = 0; i < item.itemIcons.Count; ++i)
		{
			if (i < rewardPackagePars.Count)
				GetItemIconById(item.itemIcons[i], rewardPackagePars[i].first);
			else
				item.itemIcons[i].Hide(true);
		}
		scrollList.AddItem(container.gameObject);
	}

	private void GetItemIconById(UIElemAssetIcon item, int id)
	{
		List<KodGames.ClientClass.Avatar> avatars = reward.Avatar;
		List<KodGames.ClientClass.Equipment> equis = reward.Equip;
		List<KodGames.ClientClass.Skill> skills = reward.Skill;
		List<KodGames.ClientClass.Consumable> consumables = reward.Consumable;

		foreach (var avatar in avatars)
		{
			if (avatar.ResourceId == id)
			{
				item.SetData(avatar);
				item.Data = avatar;
			}
		}
		foreach (var equi in equis)
		{
			if (equi.ResourceId == id)
			{
				item.SetData(equi);
				item.Data = equi;
			}
		}
		foreach (var skill in skills)
		{
			if (skill.ResourceId == id)
			{
				item.SetData(skill);
				item.Data = skill;
			}
		}

		foreach (var consumable in consumables)
		{
			if (consumable.Id == id)
			{
				item.SetData(consumable);
				item.Data = consumable;
			}
		}
	}


	public override void OnHide()
	{
		base.OnHide();
	}



	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		ClearData();
		HideSelf();

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOkBtnClick(UIButton btn)
	{
		ClearData();
		HideSelf();

	}


	//µã»÷Í¼±ê
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarIcon(UIButton btn)
	{

		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		ClientServerCommon.Reward assetReward = new ClientServerCommon.Reward();
		if (assetIcon.Data is KodGames.ClientClass.Avatar)
		{
			KodGames.ClientClass.Avatar avatar = assetIcon.Data as KodGames.ClientClass.Avatar;
			assetReward.id = avatar.ResourceId;
			assetReward.count = 1;
			assetReward.breakthoughtLevel = avatar.BreakthoughtLevel;
			assetReward.level = avatar.LevelAttrib.Level;

		}
		else if (assetIcon.Data is KodGames.ClientClass.Equipment)
		{
			KodGames.ClientClass.Equipment equipment = assetIcon.Data as KodGames.ClientClass.Equipment;
			assetReward.id = equipment.ResourceId;
			assetReward.count = 1;
			assetReward.breakthoughtLevel = equipment.BreakthoughtLevel;
			assetReward.level = equipment.LevelAttrib.Level;
		}
		else if (assetIcon.Data is KodGames.ClientClass.Skill)
		{
			KodGames.ClientClass.Skill skill = assetIcon.Data as KodGames.ClientClass.Skill;
			assetReward.id = skill.ResourceId;
			assetReward.count = 1;
			assetReward.level = skill.LevelAttrib.Level;
		}
		else
		{
			assetReward.id = assetIcon.AssetId;
		}
		GameUtility.ShowAssetInfoUI(assetReward, _UILayer.Top);

	}
}
