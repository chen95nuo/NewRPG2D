using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;

public class UIPnlTowerNpcLineUp : UIModule
{
	public SpriteText armbandTitle;
	public SpriteText armbandLabel;
	public UIElemAssetIcon armbandIcon;
	public SpriteText moneyTitle;
	public SpriteText moneyLabel;
	public UIElemAssetIcon moneyIcon;

	public SpriteText itemLabel1;
	public SpriteText itemLabel2;

	public SpriteText tipsLabel;

	public SpriteText layerTitleLabel;
	public SpriteText staminaLabel;

	public AutoSpriteControlBase alreadyGet;

	public List<UIElemNpcLineUp> npcIcons;
	public List<UIElemAssetIcon> rewardIcons;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		//楼层NPC信息
		if(userDatas[0] is List<com.kodgames.corgi.protocol.NpcInfo>)
			FillNpcLine(userDatas[0] as List<com.kodgames.corgi.protocol.NpcInfo>);
		
		//楼层奖励信息
		FillReward((int)userDatas[1]);

		return true;
	}

	private void FillNpcLine(List<com.kodgames.corgi.protocol.NpcInfo> npcInfos)
	{
		List<int> emptyPositions = new List<int>();
		for (int i = 0; i < npcIcons.Count; i++)
			npcIcons[i].SetEmpty();

		//楼层NPC信息
		for (int i = 0; i < npcIcons.Count && i < npcInfos.Count; i++)
		{
			int position = PlayerDataUtility.GetIndexPosByBattlePos(npcInfos[i].battlePosition);
			if (position >= npcIcons.Count)
				continue;

			npcIcons[position].SetData(npcInfos[i]);

			emptyPositions.Add(position);
		}
	}

	//楼层奖励信息
	private void FillReward(int layerLevel)
	{
		MelaleucaFloorConfig.Floor floorCfg = null;

		int maxLayer = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.Floors.Count;

		if (layerLevel > maxLayer)
			floorCfg = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(maxLayer);
		else
			floorCfg = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(layerLevel);

		tipsLabel.Text = floorCfg.Guide;

		if(floorCfg.PassReward.Count > 0)
		{
			armbandTitle.Text = string.Format(GameUtility.GetUIString("UIPnlTowerNpcLineUp_PassReward_Label"), ItemInfoUtility.GetAssetName(floorCfg.PassReward[0].id));
			if (IDSeg.ToAssetType(floorCfg.PassReward[0].id) == IDSeg._AssetType.Special)
			{
				armbandIcon.Hide(false);
				armbandIcon.SetData(floorCfg.PassReward[0].id);
				armbandLabel.Text = floorCfg.PassReward[0].count.ToString();
				itemLabel1.Text = string.Empty;
			}			
			else
			{
				armbandIcon.Hide(true);
				armbandLabel.Text =string.Empty;
				itemLabel1.Text = floorCfg.PassReward[0].count.ToString();
			}
		}
		else
		{
			itemLabel1.Text = string.Empty;
			armbandTitle.Text = string.Empty;
			armbandLabel.Text = string.Empty;
			armbandIcon.Hide(true);
		}
		if (floorCfg.PassReward.Count > 1)
		{
			moneyTitle.Text = string.Format(GameUtility.GetUIString("UIPnlTowerNpcLineUp_PassReward_Label"), ItemInfoUtility.GetAssetName(floorCfg.PassReward[1].id));
			if (IDSeg.ToAssetType(floorCfg.PassReward[1].id) == IDSeg._AssetType.Special)
			{
				moneyIcon.Hide(false);
				moneyIcon.SetData(floorCfg.PassReward[1].id);
				itemLabel2.Text = string.Empty;
				moneyLabel.Text = floorCfg.PassReward[1].count.ToString();
			}			
			else
			{
				moneyIcon.Hide(true);
				moneyLabel.Text = string.Empty;
				itemLabel2.Text = floorCfg.PassReward[1].count.ToString();
			}
		}			
		else 
		{
			itemLabel2.Text = string.Empty;
			moneyTitle.Text = string.Empty;
			moneyLabel.Text = string.Empty;
			moneyIcon.Hide(true);
		}
			
		alreadyGet.Hide(true);
		if (layerLevel <= SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.MaxPassLayer)
			alreadyGet.Hide(false);

		layerTitleLabel.Text = string.Format(GameUtility.GetUIString("UIPnlTowerNpcLineUp_TitleLabel_Label"), layerLevel);

		for (int i = 0; i < rewardIcons.Count; i++)
			rewardIcons[i].Hide(true);

		for (int i = 0; i < rewardIcons.Count && i < floorCfg.FirstPassReward.Count; i++)
		{
			rewardIcons[i].SetData(floorCfg.FirstPassReward[i].id, floorCfg.FirstPassReward[i].count);
			rewardIcons[i].Data = floorCfg.FirstPassReward[i].id;
			rewardIcons[i].Hide(false);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseBtn(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardBtn(UIButton btn)
	{

		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		int id = (int)assetIcon.Data;
		switch(IDSeg.ToAssetType(id))
		{
			case IDSeg._AssetType.Avatar :
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgAvatarInfo, id);
				break;
			case IDSeg._AssetType.Equipment:
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlEquipmentInfo, id);
				break;
			case IDSeg._AssetType.CombatTurn :
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgSkillInfo, id);
				break;
			case IDSeg._AssetType.Item:
			case IDSeg._AssetType.Special :
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, id);
				break;
			default: break;
		}		
	}
}
