using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgFriendStartOne : UIModule
{

	public List<UIElemFriendStartOne> playerIcons;
	public SpriteText label;

	private int positionId;
	private List<KodGames.ClientClass.FriendInfo> friendInfos;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		positionId = (int)userDatas[0];
		friendInfos = userDatas[1] as List<KodGames.ClientClass.FriendInfo>;

		InitUI();

		return true;
	}

	private void InitUI()
	{
		label.Text = GameUtility.FormatUIString("UIDlgFriendStartOne_Label", ItemInfoUtility.GetAssetName(positionId));

		for (int index = 0; index < playerIcons.Count; index++)
			playerIcons[index].SetData(0, string.Empty, 0, 0, 0);

		int showId = GetFirstAvatarId();
		if (showId == IDSeg.InvalidId)
			return;
		else
		{
			playerIcons[0].SetData(showId, SysLocalDataBase.Inst.LocalPlayer.Name, SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level, SysLocalDataBase.Inst.LocalPlayer.PlayerId, (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId));

			//渲染好友的角色
			for (int index = 0; index < Mathf.Min(playerIcons.Count - 1, friendInfos.Count); index++)
				playerIcons[index + 1].SetData(friendInfos[index].ShowAvatarId, friendInfos[index].Name, friendInfos[index].Level, friendInfos[index].PlayerId, friendInfos[index].Power);
		}
	}

	//获取自身角色的第一个头像
	private int GetFirstAvatarId()
	{
		int id = IDSeg.InvalidId;
		var positionData = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(positionId);
		if (positionData != null)
		{
			for (int index = 0; index < ConfigDatabase.DefaultCfg.GameConfig.maxColumnInFormation * ConfigDatabase.DefaultCfg.GameConfig.maxRowInFormation; index++)
			{
				for (int i = 0; i < positionData.AvatarLocations.Count; i++)
				{
					if (positionData.AvatarLocations[i].LocationId == PlayerDataUtility.GetBattlePosByIndexPos(index))
					{
						var avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(positionData.AvatarLocations[i].Guid);
						if (avatar != null)
						{
							id = avatar.ResourceId;
							index = ConfigDatabase.DefaultCfg.GameConfig.maxColumnInFormation * ConfigDatabase.DefaultCfg.GameConfig.maxRowInFormation;
							break;
						}
					}
				}
			}
		}

		return id;
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击玩家头像
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickPlayerIconBtn(UIButton btn)
	{
		int playerId = 0;
		playerId = (int)((btn.Data as UIElemAssetIcon).Data);

		if (playerId != 0)
		{
			if (playerId != SysLocalDataBase.Inst.LocalPlayer.PlayerId)
				RequestMgr.Inst.Request(new QueryFriendPlayerInfoReq(playerId));
			else
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFriendAvatarView), SysLocalDataBase.Inst.LocalPlayer, positionId);
		}
	}

	//点击确定
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickOKBtn(UIButton btn)
	{
		List<int> friendIds = new List<int>();
		for (int index = 0; index < friendInfos.Count; index++)
			friendIds.Add(friendInfos[index].PlayerId);
		RequestMgr.Inst.Request(new JoinFriendCampaignReq(positionId, friendIds));

		this.HideSelf();
	}

	//参战失败
	public void JoinFriendCampaignReqNotSuccess(string message)
	{
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(message, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}
}
