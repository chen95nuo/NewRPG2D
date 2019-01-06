using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlWolfMyBattle : UIModule
{

	//出征页面顶端货币显示
	public UIBox realMoney;
	public UIBox gameMoney;
	public UIBox capTure;

	//标题显示
	public SpriteText titleLabel;

	public List<UIElemWolfMyBattleItem> battleItems;//阵容角色

	//显示返回还是战斗
	public UIChildLayoutControl layoutCoutrol;


	public SpriteText title_up;
	public SpriteText title_down;

	private int gainId;
	private int stargeId;

	private KodGames.ClientClass.Player playerData;
	private KodGames.ClientClass.PositionData positionData;
	private List<KodGames.ClientClass.WolfSelectedAddition> additions;
	private List<double> hp_maxHp;
	private List<string> avatarGuid;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		for (int index = 0; index < battleItems.Count; index++)
		{
			battleItems[index].init(PlayerDataUtility.GetBattlePosByIndexPos(index));
			battleItems[index].DragHandle = OnEZDragDropHandler;
		}

		gainId = IDSeg.InvalidId;
		stargeId = IDSeg.InvalidId;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;


		if (userDatas[5] != null)
		{
			gainId = (int)userDatas[5];
			stargeId = (int)userDatas[6];
		}

		if (gainId != IDSeg.InvalidId)
			titleLabel.Text = GameUtility.GetUIString("UIPnlWolfMyBattle_Title2");
		else
			titleLabel.Text = GameUtility.GetUIString("UIPnlWolfMyBattle_Title");

		playerData = userDatas[0] as KodGames.ClientClass.Player;
		positionData = userDatas[1] as KodGames.ClientClass.PositionData;
		additions = userDatas[2] as List<KodGames.ClientClass.WolfSelectedAddition>;
		avatarGuid = userDatas[3] as List<string>;
		hp_maxHp = userDatas[4] as List<double>;

		ShowCurrency();

		ShowPositionUI();

		//计算加成
		ShowBonus();

		//显示战斗还是返回
		//如果没有增益，说明是主动点击阵容
		layoutCoutrol.HideChildObj(layoutCoutrol.childLayoutControls[0].gameObject, gainId == IDSeg.InvalidId ? false : true);
		layoutCoutrol.HideChildObj(layoutCoutrol.childLayoutControls[1].gameObject, gainId == IDSeg.InvalidId ? true : false);

		return true;
	}

	//获取阵容门客所对应的GuId
	private string GetGuId()
	{
		string enemyAvatarGuid = string.Empty;

		for (int index = 0; index < positionData.Positions[0].AvatarLocations.Count; index++)
		{
			if (positionData.Positions[0].AvatarLocations[index].ShowLocationId == positionData.Positions[0].EmployShowLocationId)
			{
				enemyAvatarGuid = positionData.Positions[0].AvatarLocations[index].Guid;

				positionData.Positions[0].EmployLocationId = positionData.Positions[0].AvatarLocations[index].LocationId;
			}
		}

		return enemyAvatarGuid;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	public void ClearData()
	{
		playerData = null;
		positionData = null;
		additions = null;
		gainId = IDSeg.InvalidId;
	}

	//渲染阵容角色
	private void ShowPositionUI()
	{
		if (positionData != null)
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
						isRecruite = (positionData.Positions[0].AvatarLocations[j].Guid.Equals(GetGuId()));
						break;
					}
				}

				battleItems[i].SetData(avatar, isRecruite);
			}
		}

		//计算出手顺序
		SetAvatarSpeedNumber();
		//计算角色血量
		SetAvatarHP(true);
	}

	//按照数据生成字符串
	private void SetText(int type, double maxhp, double speed, double map)
	{
		string tempStr = string.Empty;

		if ((maxhp + speed + map) > 0)
		{
			//		颜色
			//		名字
			//		加好
			//      颜色
			//      数据
			//      百分号和分隔符
			if (maxhp != 0)
				tempStr += GameDefines.textColorBtnYellow.ToString() +
							_AvatarAttributeType.GetDisplayNameByType(_AvatarAttributeType.MaxHP, ConfigDatabase.DefaultCfg) + GameUtility.GetUIString("UIPnlWolfMyBattle_MaxHP") +
							GameUtility.FormatUIString("UIPnlWolfMyBattle_Plus") +
							GameDefines.textColorWhite.ToString() +
							KodGames.Math.RoundToInt(maxhp * 100).ToString() +
							GameUtility.FormatUIString("UIPnlWolfMyBattle_PCT") + "   ";
			if (map != 0)
				tempStr += GameDefines.textColorBtnYellow.ToString() +
							_AvatarAttributeType.GetDisplayNameByType(_AvatarAttributeType.MAP, ConfigDatabase.DefaultCfg) +
							GameUtility.FormatUIString("UIPnlWolfMyBattle_Plus") +
							GameDefines.textColorWhite.ToString() +
							KodGames.Math.RoundToInt(map * 100).ToString() +
							GameUtility.FormatUIString("UIPnlWolfMyBattle_PCT") + "   ";
			if (speed != 0)
				tempStr += GameDefines.textColorBtnYellow.ToString() +
							_AvatarAttributeType.GetDisplayNameByType(_AvatarAttributeType.Speed, ConfigDatabase.DefaultCfg) +
							GameUtility.FormatUIString("UIPnlWolfMyBattle_Plus") +
							GameDefines.textColorWhite.ToString() +
							KodGames.Math.RoundToInt(speed * 100).ToString() +
							GameUtility.FormatUIString("UIPnlWolfMyBattle_PCT");
		}

		if (type == PositionConfig._EmBattleType.Back)
			title_down.Text = tempStr.Equals(string.Empty) ? GameUtility.FormatUIString("UIPnlWolfMyBattle_Know", GameDefines.textColorBtnYellow) : tempStr;
		if (type == PositionConfig._EmBattleType.Front)
			title_up.Text = tempStr.Equals(string.Empty) ? GameUtility.FormatUIString("UIPnlWolfMyBattle_Know", GameDefines.textColorBtnYellow) : tempStr;
	}

	//渲染加成
	private void ShowBonus()
	{
		double Back_hp = 0;//血量
		double Back_sp = 0;//速度
		double Back_ap = 0;//攻击

		double Front_hp = 0;//血量
		double Front_sp = 0;//速度
		double Front_ap = 0;//攻击

		//计算以往加成
		for (int j = 0; j < additions.Count; j++)
		{
			var config = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetAdditionById(additions[j].AdditionId);
			if (config != null)
			{
				switch (config.EmBattleAttribute.type)
				{
					case PositionConfig._EmBattleType.Back:
						Back_hp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.MaxHP);
						Back_ap += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.MAP);
						Back_sp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.Speed);
						break;
					case PositionConfig._EmBattleType.Front:
						Front_hp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.MaxHP);
						Front_ap += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.MAP);
						Front_sp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.Speed);
						break;
				}
			}
		}

		//计算本关加成
		if (gainId != IDSeg.InvalidId)
		{
			var config = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetAdditionById(gainId);

			if (config != null)
			{
				switch (config.EmBattleAttribute.type)
				{
					case PositionConfig._EmBattleType.Back:
						Back_hp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.MaxHP);
						Back_ap += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.MAP);
						Back_sp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.Speed);
						break;
					case PositionConfig._EmBattleType.Front:
						Front_hp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.MaxHP);
						Front_ap += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.MAP);
						Front_sp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.Speed);
						break;
				}
			}
		}

		//对前后进行渲染
		SetText(PositionConfig._EmBattleType.Front, Front_hp, Front_sp, Front_ap);
		SetText(PositionConfig._EmBattleType.Back, Back_hp, Back_sp, Back_ap);
	}

	//渲染金币
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

	#region Function block

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

	//计算剩余血量比例
	private float GetMaxHp(KodGames.ClientClass.Location avatarLocation, int emBattleType, bool first)
	{
		//根据Guid获取角色血量比例下标
		int n = 0;
		for (; n < avatarGuid.Count; n++)
		{
			if (avatarGuid[n].Equals(avatarLocation.Guid))
				break;
		}

		double addition_hp = 0;
		if (gainId != IDSeg.InvalidId)
		{
			var config = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetAdditionById(gainId);
			if (config != null)
			{
				switch (config.EmBattleAttribute.type)
				{
					case PositionConfig._EmBattleType.All:
						addition_hp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.HP);
						break;
					case PositionConfig._EmBattleType.Front:
						if (emBattleType < 3)
							addition_hp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.HP);
						break;
					case PositionConfig._EmBattleType.Back:
						if (emBattleType > 2)
							addition_hp += GetValueFromModifiers(config.EmBattleAttribute.modifiers, _AvatarAttributeType.HP);
						break;
				}
			}
		}

		float hp = (float)(((hp_maxHp[n] + addition_hp) > 0 && (hp_maxHp[n] + addition_hp) < 0.01) ? 0.01 : (hp_maxHp[n] + addition_hp));
		return (hp > 1 ? 1 : hp);
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
		attributes = PlayerDataUtility.GetLocationAvatarAttributes(location, playerData);
		for (int index = 0; index < attributes.Count; index++)
			if (attributes[index].type == _AvatarAttributeType.Speed)
				speed = attributes[index].value;

		//计算以往加成
		for (int i = 0; i < additions.Count; i++)
		{
			var config = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetAdditionById(additions[i].AdditionId);
			if (config != null)
			{
				speedPercent += GetSpeedFromEmbattle(config.EmBattleAttribute, location.LocationId);
			}
		}

		//计算当前加成
		if (gainId != IDSeg.InvalidId)
		{
			var config = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetAdditionById(gainId);
			if (config != null)
			{
				speedPercent += GetSpeedFromEmbattle(config.EmBattleAttribute, location.LocationId);
			}
		}

		speed = speed * (1 + speedPercent);
		return speed;
	}

	//按照速度对阵容角色出手顺序进行排序
	private void SetAvatarSpeedNumber()
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
				{
					number = index;
					break;
				}
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

	//设置角色血量
	private void SetAvatarHP(bool pRet)
	{
		for (int index = 0; index < battleItems.Count; index++)
		{
			battleItems[index].SetData(null, 0);

			for (int i = 0; i < positionData.Positions[0].AvatarLocations.Count; i++)
			{
				if (positionData.Positions[0].AvatarLocations[i].LocationId == battleItems[index].Position)
					battleItems[index].SetData(positionData.Positions[0].AvatarLocations[i], GetMaxHp(positionData.Positions[0].AvatarLocations[i], index, pRet));
			}
		}
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

	public void Myhide()
	{
		HideSelf();
	}

	#endregion

	#region OnClick

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		Myhide();
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

	//点击战斗
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickFight(UIButton btn)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlWolfInfo)))
			SysUIEnv.Instance.GetUIModule<UIPnlWolfInfo>().FightSever(gainId);

	}

	//点击详细
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickParticular(UIButton btn)
	{
		if (gainId == IDSeg.InvalidId)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfParticular), additions, null);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfParticular), additions, gainId, stargeId);
	}

	#endregion
}
