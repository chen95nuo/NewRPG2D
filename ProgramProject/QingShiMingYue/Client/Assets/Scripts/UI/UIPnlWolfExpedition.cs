using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlWolfExpedition : UIModule
{
	//出征页面顶端货币显示
	public UIBox realMoney;
	public UIBox gameMoney;
	public UIBox capTure;

	//阵容ID list
	public UIScrollList positionList;
	public GameObjectPool positionPool;

	//阵容下面人物角色
	public List<UIElemWoldExpeditionBattles> lineUpAvatars;

	//阵容ID
	private int positionID;
	private int lastPositionID;

	//临时保存一个玩家数据【相当于C++中定义了个宏】
	private KodGames.ClientClass.Player TempPlayer
	{
		get { return SysLocalDataBase.Inst.LocalPlayer; }
	}

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		positionID = IDSeg.InvalidId;

		for (int index = 0; index < lineUpAvatars.Count; index++)
			lineUpAvatars[index].Init(PlayerDataUtility.GetBattlePosByIndexPos(index));

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		lastPositionID = (int)userDatas[0];

		ShowCurrency();
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

		ClearPosition();
	}

	private void ClearPosition()
	{
		for (int index = 0; index < lineUpAvatars.Count; index++)
			lineUpAvatars[index].ClearData();
	}

	private void ShowCurrency()
	{
		if (realMoney.Data == null || (int)realMoney.Data != SysLocalDataBase.Inst.LocalPlayer.RealMoney)
		{
			realMoney.Data = SysLocalDataBase.Inst.LocalPlayer.RealMoney;
			realMoney.Text = ItemInfoUtility.GetItemCountStr((int)realMoney.Data);
		}

		if (gameMoney.Data == null || (int)gameMoney.Data != SysLocalDataBase.Inst.LocalPlayer.GameMoney)
		{
			gameMoney.Data = SysLocalDataBase.Inst.LocalPlayer.GameMoney;
			gameMoney.Text = ItemInfoUtility.GetItemCountStr((int)gameMoney.Data);
		}

		if (capTure.Data == null || (int)capTure.Data != SysLocalDataBase.Inst.LocalPlayer.Medals)
		{
			capTure.Data = SysLocalDataBase.Inst.LocalPlayer.Medals;
			capTure.Text = ItemInfoUtility.GetItemCountStr((int)capTure.Data);
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

		positionID = TempPlayer.PositionData.ActivePositionId;
		SetPositionList();

		ShowPosition();
	}

	private void ShowPosition()
	{
		ClearPosition();

		var positionData = TempPlayer.PositionData.GetPositionById(positionID);

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

	#region OnClick

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);
	}

	//点击阵容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClikBattle(UIButton btn)
	{
		positionID = (int)btn.Data;
		SetPositionList();
		ShowPosition();
	}

	//点击参战
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickExpedition(UIButton btn)
	{
		var positionData = TempPlayer.PositionData.GetPositionById(positionID);

		if (positionData == null || positionData.AvatarLocations.Count <= 0)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlWoldExpedition_ERROR"));
		else if (positionID != lastPositionID)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfExpedition), positionID);
		else
			RequestMgr.Inst.Request(new QueryJoinWolfSmoke(positionID));
	}

	//点击调整阵容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickPosition(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlAvatar);
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

	//点击元宝
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickRealMoney(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.RealMoney).desc;
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//点击铜币
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickGameMoney(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.GameMoney).desc;
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//点击军工
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickCupTure(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.Medals).desc;
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	#endregion
}