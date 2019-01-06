using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgWolfExpedition : UIModule
{
	public SpriteText titleLabel;//说明
	public SpriteText positionLabel;//阵容ID

	//阵容下面人物角色
	public List<UIElemWoldExpeditionBattles> lineUpAvatars;

	private int positionId;

	//临时保存一个玩家数据【相当于C++中定义了个宏】
	private KodGames.ClientClass.Player TempPlayer
	{
		get { return SysLocalDataBase.Inst.LocalPlayer; }
	}

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
		if (base.OnShow(layer, userDatas) == false)
			return false;

		positionId = (int)userDatas[0];

		titleLabel.Text = GameUtility.FormatUIString("UIDlgWolfExpedition_TitleLabel", ItemInfoUtility.GetAssetName(positionId));
		positionLabel.Text = ItemInfoUtility.GetAssetName(positionId);
		ShowPosition();

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		for (int index = 0; index < lineUpAvatars.Count; index++)
			lineUpAvatars[index].ClearData();
	}

	//根据阵容ID去渲染阵容内的角色
	private void ShowPosition()
	{
		ClearData();

		var positionData = TempPlayer.PositionData.GetPositionById(positionId);

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
						avatar = TempPlayer.SearchAvatar(positionData.AvatarLocations[j].Guid);
						isRecruite = positionData.AvatarLocations[j].LocationId == positionData.EmployLocationId;
						break;
					}
				}

				lineUpAvatars[i].SetData(avatar, isRecruite);
			}
		}
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击确定
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickExpedition(UIButton btn)
	{
		var positionData = TempPlayer.PositionData.GetPositionById(positionId);


		if (positionData == null || positionData.AvatarLocations.Count <= 0)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlWoldExpedition_ERROR"));
		else
			RequestMgr.Inst.Request(new QueryJoinWolfSmoke(positionId));
	}
}
