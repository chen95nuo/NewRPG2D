using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgFriendAvatarView : UIModule
{

	public List<UIElemWoldExpeditionBattles> lineUpAvatars;
	public SpriteText label;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		for (int index = 0; index < lineUpAvatars.Count; index++)
			lineUpAvatars[index].Init(PlayerDataUtility.GetBattlePosByIndexPos(index));

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		int positionId = IDSeg.InvalidId;

		if ((userDatas[0] as KodGames.ClientClass.Player).PlayerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
		{
			positionId = (int)userDatas[1];
			label.Text = GameUtility.FormatUIString("UIDlgFriendAvatarView_Label_01", (userDatas[0] as KodGames.ClientClass.Player).Name, ItemInfoUtility.GetAssetName(positionId));
		}
		else
		{
			positionId = (userDatas[0] as KodGames.ClientClass.Player).PositionData.ActivePositionId;
			label.Text = GameUtility.FormatUIString("UIDlgFriendAvatarView_Label_02", (userDatas[0] as KodGames.ClientClass.Player).Name);
		}

		ShowPosition(userDatas[0] as KodGames.ClientClass.Player, positionId);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		for (int index = 0; index < lineUpAvatars.Count; index++)
			lineUpAvatars[index].ClearData();
	}

	private void ShowPosition(KodGames.ClientClass.Player player, int positionId)
	{
		ClearData();
		//判断是否是空的阵容数据
		if (player.PositionData.Positions.Count > 0)
		{
			//判断一下是不是自己的阵容进来的，如果是就选取阵容，不是就取默认阵容
			var positionData = positionId == IDSeg.InvalidId ? player.PositionData.Positions[0] : player.PositionData.GetPositionById(positionId);
			if (positionData != null)
			{
				for (int i = 0; i < lineUpAvatars.Count; i++)
				{
					KodGames.ClientClass.Avatar avatar = null;
					bool isRecruite = false;

					for (int j = 0; j < positionData.AvatarLocations.Count; j++)
					{
						if (positionData.AvatarLocations[j].LocationId == lineUpAvatars[i].Position)
						{
							avatar = player.SearchAvatar(positionData.AvatarLocations[j].Guid);
							isRecruite = positionData.AvatarLocations[j].LocationId == positionData.EmployLocationId;
							break;
						}
					}
					lineUpAvatars[i].SetData(avatar, isRecruite);
				}
			}
		}
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}
}
