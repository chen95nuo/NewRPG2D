using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendBattle : UIModule
{
	//下面的助阵角色
	public UIScrollList friendInfosList;
	public GameObjectPool friendInfosRootPool;

	//是否可以挑战以及挑战数据
	public UIButton comBatButton;
	public SpriteText comBatcount;

	private List<KodGames.ClientClass.FriendCampaignPosition> friendInfos;
	private int playerId; //保存当前玩家Id

	public List<UIElemWolfMyBattleItem> battleItems;//阵容角色

	private int alreadyResetCount;


	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		for (int index = 0; index < battleItems.Count; index++)
		{
			battleItems[index].init(PlayerDataUtility.GetBattlePosByIndexPos(index));
			battleItems[index].DragHandle = OnEZDragDropHandler;
		}

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		friendInfos = userDatas[0] as List<KodGames.ClientClass.FriendCampaignPosition>;
		Sort();

		alreadyResetCount = (int)userDatas[1];
		playerId = (int)userDatas[2];

		StartCoroutine("FillFriendInfosList");

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillFriendInfosList");
		friendInfosList.ClearList(false);
		friendInfosList.ScrollListTo(0);
	}

	private int GetResetResidueCount()
	{
		return ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.FriendCampaignResetCount) - this.alreadyResetCount;
	}

	private int GetResetAllCount()
	{
		return ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.FriendCampaignResetCount);
	}

	//对返回数据进行排序
	private void Sort()
	{
		if (friendInfos.Count > 1 && friendInfos[friendInfos.Count - 1].Player.PlayerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
		{
			KodGames.ClientClass.FriendCampaignPosition playerTemp = friendInfos[friendInfos.Count - 1];

			for (int index = friendInfos.Count - 1; index > 0; index--)
			{
				friendInfos[index] = friendInfos[index - 1];
			}

			friendInfos[0] = playerTemp;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillFriendInfosList()
	{
		yield return null;

		for (int index = 0; index < friendInfos.Count; index++)
		{
			UIElemPlayerListItemRoot item = friendInfosRootPool.AllocateItem(false).GetComponent<UIElemPlayerListItemRoot>();
			item.SetData(friendInfos[index]);
			friendInfosList.AddItem(item);
		}

		if (playerId == IDSeg.InvalidId && friendInfos != null && friendInfos.Count > 0)
			playerId = friendInfos[0].Player.PlayerId;

		SelectPlayerBox();
		ShowPosition();
	}

	//判断阵容当中还有没有没有死亡的角色
	private bool GetFriendInfoHp()
	{
		for (int index = 0; index < friendInfos.Count; index++)
			if (friendInfos[index].TotalLeftHpPercent > 0)
				return true;

		return false;
	}

	//渲染挑战按钮状态以及挑战剩余次数
	private void SetBtnAndCount()
	{
		////////////////////////////////////////////////////////
		//********				说明				**********//
		//********该阵容全阵容，显示为灰色按钮，提示**********//
		//*******全阵容全死亡，判断重置次数，有，重置*********//
		//*******全阵容全死亡，判断重置次数，无，提示*********//
		////////////////////////////////////////////////////////

		bool thisHp = GetFriendByPlayerId().TotalLeftHpPercent > 0 ? true : false;
		bool allHp = GetFriendInfoHp();

		bool count = GetResetResidueCount() > 0 ? true : false;

		if (thisHp)
		{
			UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetBigButon1(comBatButton, false);
			comBatcount.Text = GameUtility.GetUIString("UIPnlFriendBattle_Combatcount_01");
		}
		else
		{
			if (allHp)
			{
				UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetBigButon1(comBatButton, true);
				comBatcount.Text = GameUtility.GetUIString("UIPnlFriendBattle_Combatcount_01");
			}
			else
			{
				if (count)
				{
					UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetBigButon1(comBatButton, false);
					comBatcount.Text = GameUtility.FormatUIString("UIPnlFriendBattle_Combatcount_02",
						GetResetResidueCount(),
						GetResetAllCount());
				}
				else
				{
					UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetBigButon1(comBatButton, true);
					comBatcount.Text = GameUtility.FormatUIString("UIPnlFriendBattle_Combatcount_02",
						GetResetResidueCount(),
						GetResetAllCount());
				}
			}
		}
	}

	//点击头像后重新渲染界面
	private void SelectPlayerId(int playerId)
	{
		this.playerId = playerId;
		SelectPlayerBox();
		ShowPosition();
	}

	//重新计算头像的亮框显示
	private void SelectPlayerBox()
	{
		SetBtnAndCount();

		for (int index = 0; index < friendInfosList.Count; index++)
		{
			UIElemPlayerListItemRoot item = friendInfosList.GetItem(index) as UIElemPlayerListItemRoot;
			item.SetSelectData(this.playerId);
		}
	}

	//渲染该角色下的阵容
	private void ShowPosition()
	{
		var positionData = GetPositionDataByPlayerId();
		var playerData = GetPlayerByPlayerId();
		if (positionData != null && playerData != null)
		{
			for (int i = 0; i < battleItems.Count; i++)
			{
				battleItems[i].IsClose = false;

				KodGames.ClientClass.Avatar avatar = null;
				bool isRecruite = false;

				for (int j = 0; j < positionData.Positions[0].AvatarLocations.Count; j++)
				{
					if (positionData.Positions[0].AvatarLocations[j].LocationId == battleItems[i].Position)
					{
						avatar = playerData.SearchAvatar(positionData.Positions[0].AvatarLocations[j].Guid);
						isRecruite = (positionData.Positions[0].AvatarLocations[j].Guid.Equals(GetGuId(positionData)));
						break;
					}
				}
				battleItems[i].SetData(avatar, isRecruite);
			}

			SetAvatarHP(true);
			SetAvatarSpeedNumber();
		}
	}

	//获取阵容门客所对应的GuId
	private string GetGuId(KodGames.ClientClass.PositionData positionData)
	{
		for (int index = 0; index < positionData.Positions[0].AvatarLocations.Count; index++)
		{
			if (positionData.Positions[0].AvatarLocations[index].ShowLocationId == positionData.Positions[0].EmployShowLocationId)
			{
				positionData.Positions[0].EmployLocationId = positionData.Positions[0].AvatarLocations[index].LocationId;
				return positionData.Positions[0].AvatarLocations[index].Guid;
			}
		}

		return string.Empty;
	}

	//按照速度对阵容角色出手顺序进行排序
	private void SetAvatarSpeedNumber()
	{
		if (playerId == IDSeg.InvalidId)
			return;

		var positionData = GetPositionDataByPlayerId();
		if (positionData != null)
		{
			// LineUp Avatars' Speed.
			List<KodGames.Pair<int, double>> speeds = new List<KodGames.Pair<int, double>>();

			for (int i = 0; i < battleItems.Count; i++)
			{
				if (battleItems[i].IsClose || !battleItems[i].HasAvatar)
					continue;

				int number = -1;
				for (int index = 0; index < positionData.Positions[0].AvatarLocations.Count; index++)
				{
					if (positionData.Positions[0].AvatarLocations[index].LocationId == battleItems[i].Position)
						number = index;
				}

				double speed = GetSpeed(positionData.Positions[0].AvatarLocations[number]);

				var pair = new KodGames.Pair<int, double>();
				pair.first = battleItems[i].Position;
				pair.second = speed;
				speeds.Add(pair);
			}

			// Sort By Speed Number.
			speeds.Sort((d1, d2) =>
			{
				return KodGames.Math.RoundToInt(d2.second) - KodGames.Math.RoundToInt(d1.second);
			});

			// Add By Sorted Sequence.
			List<int> locationIds = new List<int>();
			for (int i = 0; i < speeds.Count; i++)
				locationIds.Add(speeds[i].first);

			for (int i = 0; i < battleItems.Count; i++)
			{
				if (battleItems[i].IsClose || !battleItems[i].HasAvatar)
					battleItems[i].AvatarShotNum = -1;
				else
					battleItems[i].AvatarShotNum = locationIds.IndexOf(battleItems[i].Position) + 1;
			}
		}
	}

	//设置角色血量
	private void SetAvatarHP(bool pRet)
	{
		var positionData = GetPositionDataByPlayerId();
		if (positionData != null)
		{
			for (int index = 0; index < battleItems.Count; index++)
			{
				battleItems[index].SetData(null, 0);

				for (int i = 0; i < positionData.Positions[0].AvatarLocations.Count; i++)
				{
					if (positionData.Positions[0].AvatarLocations[i].LocationId == battleItems[index].Position)
						battleItems[index].SetData(positionData.Positions[0].AvatarLocations[i], GetAvatarHP(positionData.Positions[0].AvatarLocations[i].Guid));
				}
			}
		}
	}

	//获取阵容下角色的血量
	private float GetAvatarHP(string guiD)
	{
		KodGames.ClientClass.FriendCampaignPosition player = GetFriendByPlayerId();

		for (int index = 0; index < player.AvatarHpInfos.Count; index++)
		{
			if (player.AvatarHpInfos[index].Guid.Equals(guiD))
				return (float)player.AvatarHpInfos[index].LeftHP;
		}

		return 0;
	}

	//获取角色详细
	private KodGames.ClientClass.FriendCampaignPosition GetFriendByPlayerId()
	{
		for (int index = 0; index < friendInfos.Count; index++)
		{
			if (friendInfos[index].Player.PlayerId == playerId)
				return friendInfos[index];
		}

		return null;
	}

	//获取角色阵容
	private KodGames.ClientClass.PositionData GetPositionDataByPlayerId()
	{
		KodGames.ClientClass.PositionData positionData = null;
		if (playerId == IDSeg.InvalidId)
			return null;
		for (int id = 0; id < friendInfos.Count; id++)
		{
			if (friendInfos[id].Player.PlayerId == playerId)
			{
				positionData = friendInfos[id].Player.PositionData;
				break;
			}
		}

		return positionData;
	}

	//获取角色
	private KodGames.ClientClass.Player GetPlayerByPlayerId()
	{
		if (playerId == IDSeg.InvalidId)
			return null;

		for (int id = 0; id < friendInfos.Count; id++)
		{
			if (friendInfos[id].Player.PlayerId == playerId)
				return friendInfos[id].Player;
		}

		return null;
	}

	private UIElemWolfMyBattleItem GetSourceItem(IUIObject obj)
	{
		UIElemWolfMyBattleItem avatarItem = null;

		foreach (var item in battleItems)
		{
			if (item.avatarIconBtn.border.Equals(obj))
			{
				avatarItem = item;
				break;
			}
		}

		return avatarItem;
	}

	private UIElemWolfMyBattleItem GetTargetItem(GameObject obj)
	{
		foreach (var item in battleItems)
		{
			if (item.gameObject == obj || item.avatarIconBtn.gameObject == obj || item.avatarBgBtn.gameObject == obj)
				return item;
		}

		return null;
	}

	//计算当前速度
	private double GetSpeed(KodGames.ClientClass.Location location)
	{
		double speed = 0;
		if (location == null)
			return speed;

		double speedPercent = 0;

		//获取基础值
		List<AttributeCalculator.Attribute> attributes = null;
		attributes = PlayerDataUtility.GetLocationAvatarAttributes(location, GetPlayerByPlayerId());
		for (int index = 0; index < attributes.Count; index++)
			if (attributes[index].type == _AvatarAttributeType.Speed)
				speed = attributes[index].value;

		speed = speed * (1 + speedPercent);
		return speed;
	}

	//计算速度加成
	private float GetSpeedFromEmbattle(EmBattleAttribute embattle, int location)
	{
		float speed = 0f;

		switch (embattle.type)
		{
			case PositionConfig._EmBattleType.All:
				speed += GetValueFromModifiers(embattle.modifiers, _AvatarAttributeType.Speed);
				break;
			case PositionConfig._EmBattleType.Front:
				if (location < 3)
					speed += GetValueFromModifiers(embattle.modifiers, _AvatarAttributeType.Speed);
				break;
			case PositionConfig._EmBattleType.Back:
				if (location > 2)
					speed += GetValueFromModifiers(embattle.modifiers, _AvatarAttributeType.Speed);
				break;
		}

		return speed;
	}

	//传入参数进行计算值
	private float GetValueFromModifiers(List<PropertyModifier> modifiers, int type)
	{
		float value = 0.0f;

		if (modifiers != null && modifiers.Count > 0)
		{
			for (int index = 0; index < modifiers.Count; index++)
			{
				if (modifiers[index].attributeType == type)
					value = modifiers[index].attributeValue;
			}
		}

		return value;
	}

	//拖动后对阵容进行修改
	private void OnEZDragDropHandler(EZDragDropParams parms)
	{
		UIElemWolfMyBattleItem sourceItem = GetSourceItem(parms.dragObj);
		switch (parms.evt)
		{
			case EZDragDropEvent.Begin:
				sourceItem.IsDraged = true;
				break;

			case EZDragDropEvent.Update:
				break;

			case EZDragDropEvent.DragEnter:
				break;

			case EZDragDropEvent.DragExit:
				break;

			case EZDragDropEvent.Dropped:

				var positionData = GetPositionDataByPlayerId();

				sourceItem = GetSourceItem(parms.dragObj);
				var targetItem = GetTargetItem(parms.dragObj.DropTarget);

				if (targetItem == null || sourceItem == targetItem)
				{
					parms.dragObj.DropHandled = false;
					sourceItem.IsDraged = false;
				}
				else
				{
					parms.dragObj.DropHandled = false;

					if (targetItem.IsClose)
						break;

					int locationId1 = sourceItem.Position;
					int locationId2 = targetItem.Position;

					KodGames.ClientClass.Avatar sourceAvatar = sourceItem.AvatarData;
					KodGames.ClientClass.Avatar targetAvatar = targetItem.AvatarData;
					bool sIsRecruite = sourceItem.IsRecruiteAvatar;
					bool tIsRecruite = targetItem.IsRecruiteAvatar;


					//根据阵容内的角色来判断该角色的LocationId
					KodGames.ClientClass.Location avatarLocation1 = null;
					KodGames.ClientClass.Location avatarLocation2 = null;

					for (int index = 0; index < positionData.Positions[0].AvatarLocations.Count; index++)
					{
						if (positionData.Positions[0].AvatarLocations[index].LocationId == locationId1)
							avatarLocation1 = positionData.Positions[0].AvatarLocations[index];
						if (positionData.Positions[0].AvatarLocations[index].LocationId == locationId2)
							avatarLocation2 = positionData.Positions[0].AvatarLocations[index];
					}

					// Change LocationId.
					if (avatarLocation1 != null)
						avatarLocation1.LocationId = locationId2;
					if (avatarLocation2 != null)
						avatarLocation2.LocationId = locationId1;


					if (positionData.Positions[0].EmployLocationId == locationId1)
						positionData.Positions[0].EmployLocationId = locationId2;
					else if (positionData.Positions[0].EmployLocationId == locationId2)
						positionData.Positions[0].EmployLocationId = locationId1;

					sourceItem.SetData(targetAvatar, tIsRecruite);
					targetItem.SetData(sourceAvatar, sIsRecruite);

					//重新计算阵位角色速度
					SetAvatarSpeedNumber();
				}
				//重新计算血量
				SetAvatarHP(false);
				break;

			case EZDragDropEvent.Cancelled:
				sourceItem.IsDraged = false;
				break;

			case EZDragDropEvent.CancelDone:
				sourceItem.IsDraged = false;
				break;
		}
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击玩家头像
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickPlayerIcon(UIButton btn)
	{
		KodGames.ClientClass.FriendCampaignPosition playerTemp = (btn.Data as UIElemAssetIcon).Data as KodGames.ClientClass.FriendCampaignPosition;
		if (playerId != playerTemp.Player.PlayerId)
			SelectPlayerId(playerTemp.Player.PlayerId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickCombatBtn(UIButton btn)
	{
		////////////////////////////////////////////////////////
		//********				说明				**********//
		//********该阵容全阵容，显示为灰色按钮，提示**********//
		//*******全阵容全死亡，判断重置次数，有，重置*********//
		//*******全阵容全死亡，判断重置次数，无，提示*********//
		////////////////////////////////////////////////////////

		bool thisHp = GetFriendByPlayerId().TotalLeftHpPercent > 0 ? true : false;
		bool allHp = GetFriendInfoHp();

		bool count = GetResetResidueCount() > 0 ? true : false;

		if (thisHp)
			RequestMgr.Inst.Request(new CombatFriendCampaignReq(GetPlayerByPlayerId().PlayerId, GetPlayerByPlayerId().PositionData.Positions[0]));
		else
		{
			if (allHp)
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlFriendBattle_CombatCount_03"));
			else
			{
				if (count)
					SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFriendCampaignReset), alreadyResetCount);
				else
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlFriendBattle_CombatCount_04"));
			}
		}
	}
}
