using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemPlayerListItemRoot : UIListItemContainerEx
{
	private UIElemPlayerListItem uiItem;
	private KodGames.ClientClass.FriendCampaignPosition uiData;
	private int selectPlayerId;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemPlayerListItem>();

			SetData(uiData);
			SetSelectData(selectPlayerId);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(KodGames.ClientClass.FriendCampaignPosition playerInfo)
	{
		this.uiData = playerInfo;
		int resourceId = GetAvatarId(playerInfo);

		if (uiItem == null)
			return;

		if (playerInfo == null)
			return;

		if (resourceId == IDSeg.InvalidId)
			uiItem.playerIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
		else
			uiItem.playerIcon.SetData(resourceId);

		uiItem.playerName.Text = playerInfo.Player.Name;
		uiItem.playerLevel.Text = GameUtility.FormatUIString("UILevelPrefix", playerInfo.Player.LevelAttrib.Level);
		uiItem.hpProgress.Value = (float)playerInfo.TotalLeftHpPercent;

		int hp = 0;
		//血量控制
		if ((playerInfo.TotalLeftHpPercent * 100) > 100)
			hp = 100;
		else if ((playerInfo.TotalLeftHpPercent * 100) > 0 && (playerInfo.TotalLeftHpPercent * 100) < 1)
			hp = 1;
		else
			hp = (int)(playerInfo.TotalLeftHpPercent * 100);

		uiItem.hpProgress.Text = GameUtility.FormatUIString("UIPnlFriendBattle_PlayerListHp", hp);
		uiItem.playerDieAll.Hide(hp <= 0 ? false : true);
		uiItem.playerDieAllBg.Hide(hp <= 0 ? false : true);

		//存储数据
		uiItem.playerIcon.Data = playerInfo;

	}

	private int GetAvatarId(KodGames.ClientClass.FriendCampaignPosition playerInfo)
	{
		for (int index = 0; index < (ConfigDatabase.DefaultCfg.GameConfig.maxRowInFormation * ConfigDatabase.DefaultCfg.GameConfig.maxColumnInFormation); index++)
		{
			for (int i = 0; i < playerInfo.Player.PositionData.Positions[0].AvatarLocations.Count; i++)
			{
				if (PlayerDataUtility.GetBattlePosByIndexPos(index) == playerInfo.Player.PositionData.Positions[0].AvatarLocations[i].LocationId)
				{
					var avatar = playerInfo.Player.SearchAvatar(playerInfo.Player.PositionData.Positions[0].AvatarLocations[i].Guid);
					if (avatar != null)
						return avatar.ResourceId;
				}
			}
		}

		return IDSeg.InvalidId;
	}

	//设置亮框
	public void SetSelectData(int selectId)
	{
		this.selectPlayerId = selectId;

		if (uiItem == null)
			return;

		if (selectId == IDSeg.InvalidId)
			uiItem.playerSelect.Hide(true);
		else
		{
			this.selectPlayerId = selectId;
			uiItem.playerSelect.Hide(selectId != this.uiData.Player.PlayerId ? true : false);
		}
	}
}
