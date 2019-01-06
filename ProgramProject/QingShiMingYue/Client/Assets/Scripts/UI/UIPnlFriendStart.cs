using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendStart : UIModule
{
	//阵容ID list
	public UIScrollList positionList;
	public GameObjectPool positionPool;

	//阵容下面人物角色
	public List<UIElemWoldExpeditionBattles> lineUpAvatars;

	//阵容ID
	private int positionID;
	private int lastPositionID;
	private List<int> lastFriendIds;

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
		if (!base.OnShow(layer, userDatas))
			return false;

		lastPositionID = (int)userDatas[0];
		positionID = lastPositionID;
		lastFriendIds = userDatas[1] as List<int>;

		ShowPositionList();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		positionList.ClearList(false);
		positionList.ScrollPosition = 0f;
	}

	private void ShowPositionList()
	{
		TempPlayer.PositionData.Positions.Sort((b1, b2) =>
		{
			int id1 = b1.PositionId;
			int id2 = b2.PositionId;
			return id1 - id2;
		});

		for (int index = 0; index < TempPlayer.PositionData.Positions.Count; index++)
		{
			UIListItemContainer container = positionPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemAvatarPositionItem positionItem = container.GetComponent<UIElemAvatarPositionItem>();

			positionItem.SetData(TempPlayer.PositionData.Positions[index].PositionId);
			container.Data = positionItem;
			positionList.AddItem(container);
		}

		if (positionID == IDSeg.InvalidId)
			positionID = TempPlayer.PositionData.ActivePositionId;

		SetPositionList();

		ShowPosition();
	}

	//根据阵容ID去渲染阵容内的角色
	private void ShowPosition()
	{
		//渲染之前清空一下内容
		ClearPosition();

		//先去角色身上按照阵容ID去找到该阵容下有多少个角色
		var positionData = TempPlayer.PositionData.GetPositionById(positionID);

		if (positionData != null)
		{
			for (int i = 0; i < lineUpAvatars.Count; i++)
			{
				KodGames.ClientClass.Avatar avatar = null;
				bool isRecruite = false;

				//对比他们之间的位置
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

	private void ClearPosition()
	{
		for (int index = 0; index < lineUpAvatars.Count; index++)
		{
			lineUpAvatars[index].ClearData();
		}
	}

	private void SetPositionList()
	{
		for (int index = 0; index < positionList.Count; index++)
		{
			UIElemAvatarPositionItem positionItem = positionList.GetItem(index).Data as UIElemAvatarPositionItem;
			positionItem.SetControllEnable(positionID);
		}
	}

	#region
	//点击阵容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClikBattle(UIButton btn)
	{
		positionID = (int)btn.Data;

		SetPositionList();
		ShowPosition();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);
	}

	//点击头像
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickAssisent(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		KodGames.ClientClass.Avatar avatar = assetIcon.Data as KodGames.ClientClass.Avatar;
		if (avatar != null)
			SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIDlgAvatarInfo, avatar, false, true, false, false, null, SysLocalDataBase.Inst.LocalPlayer);
	}

	//点击调整阵容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickPosition(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlAvatar);
	}

	//点击下一步
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickNextOperation(UIButton btn)
	{
		//判断一下当前所选的阵容是否有角色
		var positionData = TempPlayer.PositionData.GetPositionById(positionID);
		bool pRet = false;
		if (positionData != null)
		{
			for (int index = 0; index < positionData.AvatarLocations.Count; index++)
			{
				if (TempPlayer.SearchAvatar(positionData.AvatarLocations[index].Guid) != null)
				{
					pRet = true;
					break;
				}
			}
		}

		if (pRet)
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlFriendSelectFriends), positionID, lastFriendIds, lastPositionID == positionID ? true : false);
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlFriendStart_Tips"));
	}
	#endregion
}
