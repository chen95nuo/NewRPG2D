using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIDlgBeforeBattleLineUp : UIModule
{
	public UIScrollList positionList;
	public GameObjectPool positionPool;
	public List<UIElemLineUpAvatarItem> lineUpAvatars;
	public SpriteText tabNameLabel;

	private UIElementDungeonItem.CombatData combatData;
	private int selectPositionId;

	private KodGames.ClientClass.Player CurrentPlayer { get { return SysLocalDataBase.Inst.LocalPlayer; } }

	private int arenaRank;

	private bool isSweepBattle;
	private int combatType = _CombatType.Unknown;

	//切磋，好友信息
	private KodGames.ClientClass.FriendInfo friendInfo;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		for (int i = 0; i < lineUpAvatars.Count; i++)
		{
			lineUpAvatars[i].Init(PlayerDataUtility.GetBattlePosByIndexPos(i));
			lineUpAvatars[i].DragHandle = OnEZDragDropHandler;
		}

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas != null)
			combatType = (int)userDatas[0];

		tabNameLabel.Text = GameUtility.GetUIString("UIPnlAvatar_Mode_AvatarLineUp");

		switch (combatType)
		{
			case _CombatType.Tower:
				isSweepBattle = (bool)userDatas[1];
				int towerBattleType = 0;
				if (isSweepBattle)
					towerBattleType = UIPnlTowerPlayerInfo.sweepBattleType;
				else
					towerBattleType = UIPnlTowerPlayerInfo.battleType;

				switch (towerBattleType)
				{
					case 2: { tabNameLabel.Text = GameUtility.GetUIString("UIPnlTowerAttackMode_Battle_Two_Label"); break; }
					case 4: { tabNameLabel.Text = GameUtility.GetUIString("UIPnlTowerAttackMode_Battle_Four_Label"); break; }
					case 8: { tabNameLabel.Text = GameUtility.GetUIString("UIPnlTowerAttackMode_Battle_Eight_Label"); break; }
					default: break;
				}
				break;

			case _CombatType.CombatFriend:	
				friendInfo = userDatas[1] as KodGames.ClientClass.FriendInfo;				
				break;
			case _CombatType.ActivityCampaign:
			case _CombatType.Campaign:
				combatData = userDatas[1] as UIElementDungeonItem.CombatData;
				break;
			case _CombatType.Arena:
				arenaRank = (int)userDatas[1];
				break;
		}

		SetPositionView();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
	}

	private void ClearData()
	{
		// Clear List.
		positionList.ClearList(false);
		positionList.ScrollPosition = 0f;

		// Clear Data.
		combatData = null;
		selectPositionId = -1;
		combatType = _CombatType.Unknown;
	}

	private void SetPositionView()
	{
		// Fill Position ScrollList.
		for (int index = 0; index < ConfigDatabase.DefaultCfg.PositionConfig.Positions.Count; index++)
		{
			UIListItemContainer container = positionPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemAvatarPositionItem positionItem = container.GetComponent<UIElemAvatarPositionItem>();

			positionItem.SetData(ConfigDatabase.DefaultCfg.PositionConfig.Positions[index].Id);
			container.Data = positionItem;
			positionList.AddItem(container);
		}

		if (InterimPositionData.Instance.GetPositionDataByType(combatType) != null)
			SelectPositon(InterimPositionData.Instance.GetPositionDataByType(combatType).positionId);
		else if (combatData != null)
			SelectPositon(combatData.positionId);
		else
			SelectPositon(CurrentPlayer.PositionData.ActivePositionId);
	}

	private void SelectPositon(int positionId)
	{
		this.selectPositionId = positionId;

		// Set Position Tab.
		for (int index = 0; index < positionList.Count; index++)
		{
			UIElemAvatarPositionItem positionItem = positionList.GetItem(index).Data as UIElemAvatarPositionItem;
			positionItem.SetControllEnable(positionId);
		}

		SetLineUpAvatars();
	}

	private void SetLineUpAvatars()
	{
		var localPosition = InterimPositionData.Instance.GetPositionDataByType(this.combatType);
		var position = CurrentPlayer.PositionData.GetPositionById(selectPositionId);

		for (int i = 0; i < lineUpAvatars.Count; i++)
		{
			KodGames.ClientClass.Avatar avatar = null;
			bool isRecruite = false;
			lineUpAvatars[i].IsClose = false;

			int showLocationId = -1;

			if (localPosition != null && localPosition.positionId == selectPositionId)
				showLocationId = localPosition.GetLocationDataByLocationId(lineUpAvatars[i].Position).showLocationId;
			else
				showLocationId = position.GetPairByLocationId(lineUpAvatars[i].Position).ShowLocationId;

			if (position.EmployShowLocationId == showLocationId)
			{
				isRecruite = true;

				if (combatData != null && combatData.recruiteNpc != null)
				{
					avatar = new UIElemLineUpAvatarItem.LineUpAvatar();
					avatar.ResourceId = combatData.recruiteNpc.AvatarId;
					avatar.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
					avatar.LevelAttrib.Level = combatData.recruiteNpc.Level;
					avatar.BreakthoughtLevel = combatData.recruiteNpc.BreakthroughLevel;
					(avatar as UIElemLineUpAvatarItem.LineUpAvatar).traitType = combatData.recruiteNpc.TraitType;
					(avatar as UIElemLineUpAvatarItem.LineUpAvatar).avatarName = combatData.recruiteNpc.Name;
				}
				else
					avatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, selectPositionId, position.EmployShowLocationId);
			}
			else
				avatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, selectPositionId, showLocationId);

			lineUpAvatars[i].SetData(avatar, isRecruite, showLocationId);
		}

		SetAvatarSpeedNumber();
	}

	private void SetAvatarSpeedNumber()
	{
		// LineUp Avatars' Speed.
		List<KodGames.Pair<int, double>> speeds = new List<KodGames.Pair<int, double>>();
		var avatarLocations = PlayerDataUtility.GetAvatarLocations(CurrentPlayer, selectPositionId);

		for (int i = 0; i < lineUpAvatars.Count; i++)
		{
			if (lineUpAvatars[i].IsClose || !lineUpAvatars[i].HasAvatar)
				continue;

			List<AttributeCalculator.Attribute> attributes = null;

			if (string.IsNullOrEmpty(lineUpAvatars[i].AvatarData.Guid))
				attributes = GetRecruiteNpcAttribute(i, combatData.recruiteNpc.Attributes);
			else
			{
				for (int index = 0; index < avatarLocations.Count; index++)
				{
					if (avatarLocations[index].Guid.Equals(lineUpAvatars[i].AvatarData.Guid))
					{
						var tempLocaiton = new KodGames.ClientClass.Location();
						tempLocaiton.ShallowCopy(avatarLocations[index]);
						tempLocaiton.LocationId = PlayerDataUtility.GetBattlePosByIndexPos(i);
						attributes = PlayerDataUtility.GetLocationAvatarAttributes(tempLocaiton, CurrentPlayer);
						break;
					}
				}
			}

			double speed = 0;
			if (attributes != null)
			{
				for (int j = 0; j < attributes.Count; j++)
				{
					if (attributes[j].type == _AvatarAttributeType.Speed)
					{
						speed = attributes[j].value;
						break;
					}
				}
			}

			var pair = new KodGames.Pair<int, double>();
			pair.first = i;
			pair.second = speed;
			speeds.Add(pair);
		}

		// Sort By Speed Number.
		speeds.Sort((d1, d2) =>
		{
			return KodGames.Math.RoundToInt(d2.second) - KodGames.Math.RoundToInt(d1.second);
		});

		// Add By Sorted Sequence.
		List<int> avatarSpeeds = new List<int>();
		for (int i = 0; i < speeds.Count; i++)
			avatarSpeeds.Add(speeds[i].first);

		for (int i = 0; i < lineUpAvatars.Count; i++)
		{
			if (lineUpAvatars[i].IsClose || !lineUpAvatars[i].HasAvatar)
				lineUpAvatars[i].AvatarShotNum = -1;
			else
				lineUpAvatars[i].AvatarShotNum = avatarSpeeds.IndexOf(i) + 1;
		}
	}

	private List<AttributeCalculator.Attribute> GetRecruiteNpcAttribute(int battleIndex, List<KodGames.ClientClass.Attribute> attributes)
	{
		var comAttributes = new Dictionary<int, double>();

		foreach (var attribute in attributes)
			comAttributes.Add(attribute.Type, attribute.Value);

		int embattleType = PositionConfig._EmBattleType.UnKnow;

		switch (battleIndex / ConfigDatabase.DefaultCfg.GameConfig.maxColumnInFormation)
		{
			case 0:
				embattleType = PositionConfig._EmBattleType.Front;
				break;
			case 1:
				embattleType = PositionConfig._EmBattleType.Back;
				break;
		}

		if (embattleType != PositionConfig._EmBattleType.UnKnow)
		{
			List<PropertyModifier> tempModifiers = new List<PropertyModifier>();
			var emBattleAttribute = ConfigDatabase.DefaultCfg.PositionConfig.GetEmBattleAttributeByType(embattleType);

			if (emBattleAttribute != null)
				ConfigDatabase.DefaultCfg.AttributeCalculator.CombineModifier(emBattleAttribute.modifiers, tempModifiers, false, false, true);

			ConfigDatabase.DefaultCfg.AttributeCalculator.ApplyPropertyModifiers(tempModifiers, comAttributes);
			AttributeCalculator.RoundAttributes(ref comAttributes);
		}

		List<AttributeCalculator.Attribute> returnValue = new List<AttributeCalculator.Attribute>();
		foreach (var kvp in comAttributes)
			returnValue.Add(new AttributeCalculator.Attribute(kvp.Key, kvp.Value));

		return returnValue;
	}

	private List<AttributeCalculator.Attribute> ReplaceAttribute(List<KodGames.ClientClass.Attribute> sourceAttributes)
	{
		List<AttributeCalculator.Attribute> attributes = new List<AttributeCalculator.Attribute>();

		for (int index = 0; index < sourceAttributes.Count; index++)
			attributes.Add(new AttributeCalculator.Attribute(sourceAttributes[index].Type, sourceAttributes[index].Value));

		return attributes;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackBtn(UIButton btn)
	{
		//副本判定
		if (combatData != null)
		{
			var rercuiteNpcs = SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonRecruiteNpcs(combatData.dungeonID);
			if (rercuiteNpcs.Count > 1)
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleRecuite), combatData);
		}

		//好友判定
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlFriendTab) && friendInfo != null)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFriendMessage), friendInfo);

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPosition(UIButton btn)
	{
		int positionId = (int)btn.Data;

		if (CurrentPlayer.PositionData.GetPositionById(positionId) == null)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlCampaign_BeforeBattle_PostionNotOpen"));
		else
			SelectPositon((int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void StartCombatBtn(UIButton btn)
	{
		var position = new KodGames.ClientClass.Position();
		position.PositionId = selectPositionId;
		position.AvatarLocations = new List<KodGames.ClientClass.Location>();
		position.EmployLocationId = -1;

		// 本地阵位
		InterimPositionData.LocalPositionData newPositonData = new InterimPositionData.LocalPositionData(combatType);
		newPositonData.positionId = selectPositionId;

		for (int index = 0; index < lineUpAvatars.Count; index++)
		{
			// 本地阵位信息
			if (lineUpAvatars[index].IsRecruiteAvatar)
			{
				newPositonData.employLocationId = lineUpAvatars[index].Position;
				newPositonData.employShowLocationId = lineUpAvatars[index].ShowLocationId;
			}

			var locationData = new InterimPositionData.LocationData();
			locationData.locationId = lineUpAvatars[index].Position;
			locationData.showLocationId = lineUpAvatars[index].ShowLocationId;

			newPositonData.locationDatas.Add(locationData);

			// 战斗阵位信息
			if (lineUpAvatars[index].IsClose || !lineUpAvatars[index].HasAvatar)
				continue;

			var location = new KodGames.ClientClass.Location();
			location.LocationId = lineUpAvatars[index].Position;
			location.PositionId = selectPositionId;
			location.Guid = lineUpAvatars[index].AvatarData.Guid;
			location.ResourceId = lineUpAvatars[index].AvatarData.ResourceId;
			location.ShowLocationId = lineUpAvatars[index].ShowLocationId;

			if (lineUpAvatars[index].IsRecruiteAvatar)
			{
				position.EmployLocationId = lineUpAvatars[index].Position;
				position.EmployShowLocationId = lineUpAvatars[index].ShowLocationId;
			}

			position.AvatarLocations.Add(location);
		}

		InterimPositionData.Instance.Put(combatType, newPositonData);

		switch(combatType)
		{
			case _CombatType.Campaign:
			case _CombatType.ActivityCampaign:
				RequestMgr.Inst.Request(new OnCombatReq(combatData.dungeonID, combatData.recruiteNpc != null ? combatData.recruiteNpc.NpcId : IDSeg.InvalidId, position, combatData.uiDungeonMapPosition));
				break;

			case _CombatType.Tower:
				if (isSweepBattle)
					RequestMgr.Inst.Request(new MelaleucaFloorConsequentCombatReq(UIPnlTowerPlayerInfo.sweepBattleType, UIPnlTowerPlayerInfo.sweepBattleCount, position));
				else
					RequestMgr.Inst.Request(new MelaleucaFloorCombatReq(UIPnlTowerPlayerInfo.battleType, position));
				break;

			case _CombatType.Arena:
				RequestMgr.Inst.Request(new ArenaCombatReq(arenaRank, position));
				break;

			case _CombatType.CombatFriend:
				RequestMgr.Inst.Request(new CombatFriendReq(friendInfo.PlayerId, position));
				break;

			case _CombatType.Adventure:
				RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._DoubleSelectType.Yes, -1, position));
				break;
		}
	}

	private void OnEZDragDropHandler(EZDragDropParams parms)
	{
		var sourceItem = GetSourceItem(parms.dragObj);
		switch (parms.evt)
		{
			case EZDragDropEvent.Begin:
				sourceItem.IsDraged = true;
				break;

			case EZDragDropEvent.Update:
				Vector3 point = SysUIEnv.Instance.UICam.camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
				UIElemAssetIcon avatarIconBtn = sourceItem.avatarIconBtn;
				avatarIconBtn.CachedTransform.position = new Vector3(point.x, point.y, avatarIconBtn.CachedTransform.position.z);
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

					KodGames.ClientClass.Avatar sourceAvatar = sourceItem.AvatarData;
					KodGames.ClientClass.Avatar targetAvatar = targetItem.AvatarData;

					bool sIsRecruite = sourceItem.IsRecruiteAvatar;
					bool tIsRecruite = targetItem.IsRecruiteAvatar;
					int sShowLocationId = sourceItem.ShowLocationId;
					int tShowLocationId = targetItem.ShowLocationId;

					sourceItem.SetData(targetAvatar, tIsRecruite, tShowLocationId);
					targetItem.SetData(sourceAvatar, sIsRecruite, sShowLocationId);

					SetAvatarSpeedNumber();
				}
				break;

			case EZDragDropEvent.Cancelled:
				sourceItem.IsDraged = false;
				break;

			case EZDragDropEvent.CancelDone:
				sourceItem.IsDraged = false;
				break;
		}
	}

	private UIElemLineUpAvatarItem GetTargetItem(GameObject obj)
	{
		foreach (var item in lineUpAvatars)
		{
			if (item.gameObject == obj || item.avatarIconBtn.gameObject == obj || item.avatarBgBtn.gameObject == obj)
				return item;
		}

		return null;
	}

	private UIElemLineUpAvatarItem GetSourceItem(IUIObject obj)
	{
		UIElemLineUpAvatarItem avatarItem = null;

		foreach (var item in lineUpAvatars)
		{
			if (item.avatarIconBtn.border.Equals(obj))
			{
				avatarItem = item;
				break;
			}
		}

		return avatarItem;
	}
}