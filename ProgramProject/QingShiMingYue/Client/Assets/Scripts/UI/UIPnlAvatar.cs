using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public enum SelectChange
{
	None,
	Dan,
	Equipment,
	Skill,
	Beast
}
public class UIPnlAvatar : UIPnlItemInfoBase
{
	private enum UIMode
	{
		AvatarLineUp, // 布阵
		AvatarOn,   // 上阵
	}

	// 阵位List.
	public SpriteText powerValue;
	public UIScrollList positionList;
	public GameObjectPool positionPool;
	public UIButton positionActiveButton;
	public UIBox positionActiveBox;
	public UIButton uimodeButton;

	// 角色List.
	public UIScrollList avatarIconList;
	public GameObjectPool avatarIconPool;
	public GameObjectPool splitIconPool;

	// 上阵UI.
	public GameObject avatarOnRoot;
	public GameObject avatarDetailRoot;
	public UIElemAvatarCard avatarCardIcon;
	public UIButton avatarCardClickBtn;
	public UIElemBreakThroughBtn avatarBreakThougthBtn;
	public UIBox countryImage;
	public SpriteText avatarLvlLabel;
	public UIBox avatarTraitBox;
	public UIButton avatarDetailBtn;
	public SpriteText avatarHPTitle;
	public SpriteText hpLabel;
	public SpriteText avatarATKTitle;
	public SpriteText dpsLabel;
	public SpriteText avatarSpeedTitle;
	public SpriteText speedLabel;
	public GameObject avatarMaker;
	public UIElemAvatarCard avatarImage;
	public UIBox avatarModelColorBox;
	public UIScrollList activeSkillList;
	public GameObjectPool activeSkillPool;
	public SpriteText assembleLable;
	public UIButton equipChangeBtn;
	public UIButton skillChangeBtn;
	public UIButton noDefendDanBox;
	public UIButton danChangeBtn;
	public UIButton noDefendBeastBox;
	public UIButton beastChangeBtn;
	public UIBox beastBg;
	public UIScrollList equipOrSkillList;
	public UIElemAssetIcon beastIcon;
	public SpriteText beastTips;

	public GameObjectPool equipOrSkillPool;
	public UIChildLayoutControl powerBtnControl;
	public UIButton powerBtn;
	public UIButton changeBtn;
	public UIButton oneKeyBtn;

	// 布阵UI.
	public GameObject avatarLineUpRoot;
	public List<UIElemLineUpAvatarItem> lineUpAvatars;

	// 小伙伴.
	public GameObject cheerAvatarRoot;
	public List<UIElemAssetIcon> cheerAvatarIcons;
	public UIScrollList cheerAvatarList;
	public GameObjectPool cheerAvatarPool;
	public SpriteText addATKByFriends;
	public SpriteText addHPByFriends;
	public SpriteText addSpeedByFriends;
	public float avatarModelZ;

	public UIBox avatarPowerNotifyIcon;
	public UIBox equipTabNotifyIcon;
	public UIBox skillTabNotifyIcon;
	public UIBox danTabNotifyIcon;
	public UIBox beastTabNotifyIcon;

	//下面用于挡按钮的板
	public UIButton mainBackBtn;
	private const int C_EQUIP_COUNT = 5;
	private UIMode currentMode;
	private int currentPositionId;
	private int currentShowLocationId;
	private Avatar avatarModel;
	private int delayShowAvatarLocationId = -1;
	private float delta = 0f;
	private List<int> hasAssembleSettingId = new List<int>();
	private List<string> tipMsgs = new List<string>();
	private static KodGames.ClientClass.Player CurrentPlayer
	{
		get { return SysLocalDataBase.Inst.LocalPlayer; }
	}
	private SelectChange selectType = SelectChange.Equipment;

	//角色、装备、书籍、内丹有变动时是否触发战力变化tips提示
	private bool isShowPowerTips;
	private int changPower;
	private bool isOverlaySetPower;
	private bool isSetMaster;


	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		for (int i = 0; i < lineUpAvatars.Count; i++)
		{
			lineUpAvatars[i].Init(PlayerDataUtility.GetBattlePosByIndexPos(i));
			lineUpAvatars[i].DragHandle = OnEZDragDropHandler;
		}

		isShowPowerTips = false;
		changPower = -1;
		isOverlaySetPower = false;
		isSetMaster = false;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		InitUI();

		return true;
	}

	public override void OnHide()
	{

		base.OnHide();
		ClearData();
		// Update UIPnlMainMenuBot.
		SetMenuBottomIconNotify();
	}

	public override void Overlay()
	{
		base.Overlay();
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		if (isOverlaySetPower)
		{
			changPower = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, currentPositionId);
			isOverlaySetPower = false;
		}

		InitMainBot();

		if (delayShowAvatarLocationId > 0)
		{
			SetAvatarControls(delayShowAvatarLocationId);
			delayShowAvatarLocationId = -1;
		}
		else if (currentMode == UIMode.AvatarOn)
		{
			if (!cheerAvatarRoot.activeInHierarchy)
			{
				//SetEquipOrSkillUI(equipChangeBtn.controlIsEnabled ? SelectChange.Skill : SelectChange.Equipment);
				SetEquipOrSkillUI(selectType);
				UpdateAvatarDynamicView();

				if (avatarModel != null)
				{
					var actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Idle, 0);
					if (actionCfg != null)
						avatarModel.PlayAnim(actionCfg.GetAnimationName(avatarModel.AvatarAssetId));
				}
			}
			else
			{
				InitPartnerUI();
				CaculateAddPropertyByFriends();
			}

			// Set Avatar Control Notify.
			SetAvatarControlsNotify(currentShowLocationId);
		}

		// Update UIPnlMainMenuBot.
		SetMenuBottomIconNotify();
	}

	private void ClearData()
	{
		currentMode = UIMode.AvatarOn;
		currentPositionId = IDSeg.InvalidId;
		currentShowLocationId = -1;
		delayShowAvatarLocationId = -1;
		selectType = SelectChange.Equipment;
		// Clear Position.
		positionList.ClearList(false);
		positionList.ScrollPosition = 0f;

		// Clear AvatarIcon.

		for (int index = 0; index < avatarIconList.Count; index++)
		{
			var avatarIconItem = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;
			if (avatarIconItem == null)
				continue;

			avatarIconItem.ClearData();
		}

		avatarIconList.ClearList(false);
		avatarIconList.ScrollPosition = 0f;

		// Clear Skill Or Equip.
		equipOrSkillList.ClearList(false);
		equipOrSkillList.ScrollPosition = 0f;

		// Clear activeSkill.
		activeSkillList.ClearList(false);
		activeSkillList.ScrollPosition = 0f;

		// Clear CheerAvatarIcon List.
		cheerAvatarList.ClearList(false);
		cheerAvatarList.ScrollPosition = 0f;

		isShowPowerTips = false;
		changPower = -1;

		StopCoroutine("LoadAvatarModel");

		if (avatarModel != null)
			avatarModel.Destroy();
	}

	private void InitMainBot()
	{
		if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
		{
			//show main menu bot.
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainMenuBot);

			// Set MainBotton Light.
			SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlAvatar);

			mainBackBtn.Hide(true);
		}
		else
		{
			mainBackBtn.Hide(false);
		}

		//重新计算战力值
		int value = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, currentPositionId);
		powerValue.Text = GameUtility.FormatUIString("UIPnlAvatar_Label_Power1", PlayerDataUtility.GetPowerString(value));
	}

	private void InitUI()
	{
		InitMainBot();
		// Set Default UIMode.
		currentMode = UIMode.AvatarOn;

		// Fill Position ScrollList.
		for (int index = 0; index < ConfigDatabase.DefaultCfg.PositionConfig.Positions.Count; index++)
		{
			UIListItemContainer container = positionPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemAvatarPositionItem positionItem = container.GetComponent<UIElemAvatarPositionItem>();

			positionItem.SetData(ConfigDatabase.DefaultCfg.PositionConfig.Positions[index].Id);
			container.Data = positionItem;
			positionList.AddItem(container);
		}

		ChangePositon(CurrentPlayer.PositionData.ActivePositionId);

		changPower = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, currentPositionId);
	}

	#region  Position
	private bool IsCheerAvatarOpened
	{
		get { return ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Partner) <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level; }
	}

	private void ChangePositon(int positionId)
	{
		// Set Current Position Id.
		this.currentPositionId = positionId;

		// Set Position Tab.
		for (int index = 0; index < positionList.Count; index++)
		{
			UIElemAvatarPositionItem positionItem = positionList.GetItem(index).Data as UIElemAvatarPositionItem;
			positionItem.SetControllEnable(positionId);
		}

		// Set Position State ( Whether is active as Combat Position).
		positionActiveButton.controlIsEnabled = CurrentPlayer.PositionData.ActivePositionId != positionId;
		positionActiveBox.Hide(CurrentPlayer.PositionData.ActivePositionId != positionId);

		// Set UIMode.
		ChangeUIMode(UIMode.AvatarOn, false);
	}

	private void SetUIModueUI(UIMode uiMode)
	{
		InitMainBot();
		this.currentMode = uiMode;

		if (currentMode == UIMode.AvatarOn)
			uimodeButton.Text = GameUtility.GetUIString("UIPnlAvatar_Mode_AvatarLineUp");
		else
			uimodeButton.Text = GameUtility.GetUIString("UIPnlAvatar_Mode_AvatarOn");
	}

	private void ChangeUIMode(UIMode uiMode, bool isReset)
	{
		SetUIModueUI(uiMode);

		this.currentShowLocationId = -1;

		int value = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, currentPositionId);

		powerValue.Text = GameUtility.FormatUIString("UIPnlAvatar_Label_Power1", PlayerDataUtility.GetPowerString(value));

		if (currentMode == UIMode.AvatarOn)
			ShowAvatarOnUI();
		else
			ShowAvatarLineUpUI();
	}

	private bool OpenPosition(object positionId)
	{
		RequestMgr.Inst.Request(new OpenPositionReq((int)positionId));
		return true;
	}

	public void OnOpenPositionSuccess(int positionId)
	{
		ChangePositon(positionId);
	}

	public void OnOneKeyOffSucess()
	{
		isShowPowerTips = true;
		ShowPower();

		ChangePositon(currentPositionId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPositionItem(UIButton btn)
	{
		int positionId = (int)btn.Data;

		if (CurrentPlayer.PositionData.GetPositionById(positionId) == null)
		{
			PositionConfig.Position positionConfig = ConfigDatabase.DefaultCfg.PositionConfig.GetPositionById(positionId);

			if (CurrentPlayer.LevelAttrib.Level < positionConfig.OpenLevel)
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlAvatar_Position_OpenLevel", positionConfig.OpenLevel));
			else if (CurrentPlayer.VipLevel < positionConfig.OpenVipLevel)
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlAvatar_Position_OpenVIPLevel", positionConfig.OpenVipLevel));
			else
			{
				if (positionConfig.Costs.Count > 0)
				{
					MainMenuItem openBtn = new MainMenuItem();
					openBtn.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OpenPosition");
					openBtn.Callback = OpenPosition;
					openBtn.CallbackData = positionId;

					MainMenuItem cancelBtn = new MainMenuItem();
					cancelBtn.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");

					UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
					//通过阵容ID来获取阵容名称作为message的Title
					showData.SetData(GameUtility.FormatUIString("UIPnlAvatar_Position_OpenTitle") + ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(positionId).name,
										GameUtility.FormatUIString("UIPnlAvatar_Position_OpenMessage", SysLocalDataBase.GetCostsDesc(positionConfig.Costs)), cancelBtn, openBtn);
					SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
				}
			}
		}
		else
		{
			ChangePositon(positionId);
			changPower = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, currentPositionId);
			ShowPower();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOneClickPositionOff(UIButton btn)
	{
		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById((int)btn.Data);

		MainMenuItem openBtn = new MainMenuItem();
		openBtn.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK_Space");
		openBtn.Callback =
			(data) =>
			{
				RequestMgr.Inst.Request(new OneClickPositionOffReq(currentPositionId));
				return true;
			};
		openBtn.CallbackData = currentPositionId;

		MainMenuItem cancelBtn = new MainMenuItem();
		cancelBtn.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel_Space");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.FormatUIString("UIPnlAvatar_OneKey_Title"),
					GameUtility.FormatUIString("UIPnlAvatar_OneKey_Message", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetQualityColor(avatarCfg.qualityLevel),
					ItemInfoUtility.GetAssetName(avatarCfg.id), GameDefines.textColorBtnYellow), cancelBtn, openBtn);
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData, true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeUIState(UIButton btn)
	{
		if (currentMode == UIMode.AvatarOn)
			currentMode = UIMode.AvatarLineUp;
		else
			currentMode = UIMode.AvatarOn;

		ChangeUIMode(currentMode, true);
	}

	#endregion

	#region  AvatarOn UI
	private void ShowAvatarOnUI()
	{
		// Show AvatarOn UI.
		avatarOnRoot.SetActive(true);
		avatarLineUpRoot.SetActive(false);

		// Clear AvatarIconList.
		for (int index = 0; index < avatarIconList.Count; index++)
		{
			var avatarIconItem = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;
			if (avatarIconItem == null)
				continue;

			avatarIconItem.ClearData();
		}

		avatarIconList.ClearList(false);
		avatarIconList.ScrollPosition = 0f;
		// Get the local avatar list.
		List<KodGames.ClientClass.Location> avartarLocations = PlayerDataUtility.GetAvatarLocations(CurrentPlayer, this.currentPositionId);

		//Sort by battle position.
		avartarLocations.Sort(DataCompare.CompareLocationByShowPos);

		var positionCfg = ConfigDatabase.DefaultCfg.PositionConfig.GetPositionById(currentPositionId);
		int maxLineUpAvatarsCount = ConfigDatabase.DefaultCfg.GameConfig.maxColumnInFormation * ConfigDatabase.DefaultCfg.GameConfig.maxRowInFormation;
		int employIndexPos = PlayerDataUtility.GetIndexPosByBattlePos
			(CurrentPlayer.PositionData.GetPositionById(currentPositionId).EmployShowLocationId);

		for (int index = 0; index < maxLineUpAvatarsCount; index++)
		{
			UIElemLineUpAvatar item = null;

			if (avatarIconList.Count <= index)
			{
				UIListItemContainer itemContainer = avatarIconPool.AllocateItem().GetComponent<UIListItemContainer>();
				item = itemContainer.gameObject.GetComponent<UIElemLineUpAvatar>();
				itemContainer.Data = item;
			}
			else
				item = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;

			bool isOpen = CurrentPlayer.LevelAttrib.Level >= positionCfg.PositionNums[index].Level;

			KodGames.ClientClass.Location avatarLocation = null;
			for (int i = 0; i < avartarLocations.Count; i++)
			{
				if (avartarLocations[i].ShowLocationId == PlayerDataUtility.GetBattlePosByIndexPos(index))
				{
					avatarLocation = avartarLocations[i];
					break;
				}
			}

			// Add Split Icon.
			if (index == employIndexPos)
			{
				avatarIconList.AddItem(splitIconPool.AllocateItem());
			}
			if (avatarLocation != null)
				item.SetData(avatarLocation, this, "OnClickAvatarIcon");
			else
				item.SetData(index, isOpen, index == employIndexPos, this, isOpen ? "OnAddLineupAvatarClick" : "OnOpenLvTipShow");

			avatarIconList.AddItem(item.gameObject);

		}
		// If CheerAvatar Opened ,add cheer Icon.
		if (IsCheerAvatarOpened)
		{
			UIListItemContainer itemContainer = avatarIconPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemLineUpAvatar item = itemContainer.gameObject.GetComponent<UIElemLineUpAvatar>();
			item.avatarIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCheerBtn, string.Empty);
			itemContainer.Data = item;

			item.SetData(this, "OnClickCheerAvatar");
			item.Index = avatarIconList.Count + 1;
			avatarIconList.AddItem(itemContainer);
		}
		// Show First Item.
		UIElemLineUpAvatar lineUpAvatar = avatarIconList.GetItem(0).Data as UIElemLineUpAvatar;
		SetAvatarControls(lineUpAvatar.avatarIcon.Data is KodGames.ClientClass.Location ? (lineUpAvatar.avatarIcon.Data as KodGames.ClientClass.Location).ShowLocationId : 0);
	}

	private void SetAvatarControls(int avatarShowLocationId)
	{
		// Set Notify.
		SetAvatarControlsNotify(avatarShowLocationId);

		this.currentShowLocationId = avatarShowLocationId;

		//重新计算战力值
		int value = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, currentPositionId);

		powerValue.Text = GameUtility.FormatUIString("UIPnlAvatar_Label_Power", PlayerDataUtility.GetPowerString(value));

		var location = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, avatarShowLocationId);
		var avatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, currentPositionId, avatarShowLocationId);

		// Set UIMode.
		SetUIModueUI(UIMode.AvatarOn);

		// Init View State.
		avatarOnRoot.SetActive(true);
		avatarDetailRoot.SetActive(avatar != null);
		cheerAvatarRoot.SetActive(false);
		avatarLineUpRoot.SetActive(false);

		// clear list.
		activeSkillList.ClearList(false);
		SetEquipOrSkillUI(selectType);

		// Set Light.
		SetLight(PlayerDataUtility.GetIndexPosByBattlePos(location != null ? location.ShowLocationId : avatarShowLocationId));

		bool isRecruite = location != null && CurrentPlayer.PositionData.GetPositionById(currentPositionId).EmployShowLocationId == location.ShowLocationId;

		powerBtnControl.HideChildObj(changeBtn.gameObject, false);
		powerBtnControl.HideChildObj(powerBtn.gameObject, isRecruite);

		// Set avatar UI.
		if (avatar != null)
		{
			// Load Avatar Model.
			StartCoroutine("LoadAvatarModel", avatar);

			AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);

			// Set AvatarCardIcon.
			avatarCardIcon.SetData(avatarConfig.id, false, false, null);

			//设置国家image
			UIElemTemplate.Inst.SetAvatarCountryIcon(countryImage, ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).countryType);

			// Set Avatar Trait icon.
			UIElemTemplate.Inst.SetAvatarTraitIcon(avatarTraitBox, avatarConfig.traitType);

			// Avatar Attribute and Assemble.
			UpdateAvatarDynamicView();

			// Avatar detail
			avatarDetailBtn.Data = avatar;

			// Set Icon Control State.
			SetAvatarIconListState(PlayerDataUtility.GetIndexPosByBattlePos(avatarShowLocationId));

			//设置是否显示一键下阵
			if (GameUtility.CheckFuncOpened(_OpenFunctionType.OneClickPositionOff, false, true))
				if (avatarShowLocationId == 0)
				{
					oneKeyBtn.Hide(false);
					oneKeyBtn.Data = avatarConfig.id;
				}
				else
					oneKeyBtn.Hide(true);
			else oneKeyBtn.Hide(true);
		}
	}

	private void SetAvatarIconListState(int itemDataIndex)
	{
		for (int index = 0; index < avatarIconList.Count; index++)
		{
			var item = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;
			if (item == null)
				continue;

			if (item.Index == itemDataIndex)
				UIUtility.CopyIconTrans(item.avatarIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconBgBtn);
			else
				UIUtility.CopyIconTrans(item.avatarIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardNormal);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator LoadAvatarModel(KodGames.ClientClass.Avatar avatar)
	{
		yield return null;

		if (avatarModel != null)
			avatarModel.Destroy();

		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);

		avatarModel = Avatar.CreateAvatar(avatarCfg.id);
		int avatarAssetId = IDSeg.InvalidId;
		if (avatarCfg.id != IDSeg.InvalidId)
		{
			avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarCfg.id).GetAvatarAssetId(avatar.BreakthoughtLevel);
			// Load avatar.
			if (avatarModel.Load(avatarAssetId, false, true) == false)
				yield break;
		}

		// Set to current layer.
		avatarModel.SetGameObjectLayer(GameDefines.AvatarCaptureLayer);

		// Put to mount bone.
		ObjectUtility.AttachToParentAndKeepLocalTrans(avatarMaker, avatarModel.gameObject);

		avatarMaker.transform.localPosition = new Vector3(avatarMaker.transform.localPosition.x, avatarMaker.transform.localPosition.y, avatarModelZ);
		avatarMaker.transform.localRotation = new Quaternion(avatarMaker.transform.localRotation.x, 180f, avatarMaker.transform.localRotation.z, avatarMaker.transform.localRotation.w);

		//Play Idle animation.
		var actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Idle, 0);
		if (actionCfg != null)
		{
			avatarModel.PlayAnim(actionCfg.GetAnimationName(avatarAssetId));

			//添加武器
			foreach (AvatarConfig.WeaponAsset weaponAsset in avatarCfg.showWeaponAssets)
			{
				avatarModel.UseComponent(weaponAsset.avatarAssetId, weaponAsset.mountBone);
			}
		}

		// Set Model Color.
		switch (avatarCfg.qualityLevel)
		{
			case 3:
				avatarModelColorBox.SetState(0);
				break;
			case 4:
				avatarModelColorBox.SetState(1);
				break;
			case 5:
				avatarModelColorBox.SetState(2);
				break;
		}
	}

	// 刷新属性值与缘分信息
	private void UpdateAvatarDynamicView()
	{
		var avatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, currentPositionId, currentShowLocationId);

		if (avatar == null)
			return;

		var avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);
		var attributes = PlayerDataUtility.GetLocationAvatarAttributes(PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, currentShowLocationId), CurrentPlayer);

		// Breakthrough
		avatarBreakThougthBtn.SetBreakThroughIcon(avatar.BreakthoughtLevel);

		int currentAvatarLevel = avatar.LevelAttrib.Level;
		int maxAvatarLevel = avatarConfig.GetAvatarBreakthrough(avatar.BreakthoughtLevel).breakThrough.powerUpLevelLimit;
		// Set Avatar Level.
		avatarLvlLabel.Text = GameUtility.FormatUIString(
											"UIPnlAvatar_AvatarLevel",
											currentAvatarLevel,
											GameDefines.textColorInOrgYew.ToString(),
											maxAvatarLevel);

		// Set Attribute label.
		avatarHPTitle.Text = GameUtility.GetUIString("AvatarAttribute_HP");
		avatarATKTitle.Text = GameUtility.GetUIString("AvatarAttribute_AP");
		avatarSpeedTitle.Text = GameUtility.GetUIString("AvatarAttribute_Speed");

		hpLabel.Text = "0";
		speedLabel.Text = "0";
		dpsLabel.Text = "0";

		for (int i = 0; i < attributes.Count; i++)
		{
			switch (attributes[i].type)
			{
				case _AvatarAttributeType.Speed:
					speedLabel.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
					break;

				case _AvatarAttributeType.MAP:
				case _AvatarAttributeType.PAP:
					dpsLabel.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
					break;

				case _AvatarAttributeType.MaxHP:
					hpLabel.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
					break;
			}
		}

		// Avatar Assemble.
		string assembleDesc = string.Empty;
		for (int i = 0; i < avatarConfig.assemableIds.Count; i++)
		{
			SuiteConfig.AssembleSetting assembleSetting = ConfigDatabase.DefaultCfg.SuiteConfig.GetAssembleSettingById(avatarConfig.assemableIds[i]);
			if (assembleSetting == null)
				continue;

			bool isAssembleActive = PlayerDataUtility.CheckAvatarAssemble(assembleSetting, avatar, CurrentPlayer, currentPositionId, currentShowLocationId);
			assembleDesc += string.Format(GameUtility.GetUIString("UIDlgViewAvatar_YuanFen"), isAssembleActive ? GameDefines.textColorGreen : GameDefines.textColorInBlack, assembleSetting.Name);
		}
		assembleLable.Text = assembleDesc;

		// Avatar Active skill.
		activeSkillList.ClearList(false);
		foreach (var activeSkill in PlayerDataUtility.GetAvatarActiveSkill(avatar.ResourceId, avatar.BreakthoughtLevel))
		{
			var skillItem = activeSkillPool.AllocateItem().GetComponent<UIElemAvatarInfoSkill>();
			skillItem.SetData(activeSkill);

			activeSkillList.AddItem(skillItem.gameObject);
		}
	}

	private void ControlSelectChangeBtnToEnabledAndNotify(SelectChange selectChangeType)
	{
		//Set btn 
		equipChangeBtn.controlIsEnabled = selectChangeType != SelectChange.Equipment;
		skillChangeBtn.controlIsEnabled = selectChangeType != SelectChange.Skill;

		//Set notyfy 
		equipTabNotifyIcon.Hide(selectChangeType == SelectChange.Equipment || !CheckEquipUsableOrPowerNotify(currentPositionId, currentShowLocationId));
		skillTabNotifyIcon.Hide(selectChangeType == SelectChange.Skill || !CheckSkillUsableNotify(currentPositionId, currentShowLocationId));

		if (ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.DanHome) > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
		{
			danTabNotifyIcon.Hide(true);
			danChangeBtn.controlIsEnabled = false;
			if (noDefendDanBox != null)
				noDefendDanBox.Hide(false);
		}
		else
		{
			danTabNotifyIcon.Hide(selectChangeType == SelectChange.Dan || !CheckDanUsableOrPowerNotify(currentPositionId, currentShowLocationId));
			danChangeBtn.controlIsEnabled = selectChangeType != SelectChange.Dan;
			if (noDefendDanBox != null)
				noDefendDanBox.Hide(true);
		}

		if (ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Beast) > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
		{
			beastTabNotifyIcon.Hide(true);
			beastChangeBtn.controlIsEnabled = false;
			if (noDefendBeastBox != null)
				noDefendBeastBox.Hide(false);
		}
		else
		{
			//小绿点接口预留
			beastTabNotifyIcon.Hide(true);

			beastChangeBtn.controlIsEnabled = selectChangeType != SelectChange.Beast;
			if (noDefendBeastBox != null)
				noDefendBeastBox.Hide(true);
		}

		beastBg.gameObject.SetActive(selectChangeType == SelectChange.Beast);

		for (int index = 0; index < equipOrSkillList.Count; index++)
		{
			var buttomItem = equipOrSkillList.GetItem(index).Data as UIElemAvatarBottomItem;

			switch (selectChangeType)
			{
				case SelectChange.Equipment:
					buttomItem.HideDan(true);
					buttomItem.SetNotify(CheckEquipUsableOrPowerNotify(currentPositionId, currentShowLocationId, EquipmentConfig._Type.Weapon + index));
					break;
				case SelectChange.Skill:
					buttomItem.HideDan(true);
					var datas = buttomItem.assetIcon.Data as List<object>;
					var location = datas[1] as KodGames.ClientClass.Location;
					buttomItem.SetNotify(string.IsNullOrEmpty(location.Guid) ? CheckSkillUsableNotify(currentPositionId, currentShowLocationId) : false);
					break;
				case SelectChange.Dan:
					//buttomItem.HideDan(false);
					buttomItem.SetNotify(CheckDanUsableOrPowerNotify(currentPositionId, currentShowLocationId, DanConfig._DanType.Sky + index));
					break;
			}

		}
	}

	private void SetEquipOrSkillUI(SelectChange selectChangeType)
	{
		// Clear bottom list.
		equipOrSkillList.ClearList(false);
		equipOrSkillList.ScrollPosition = 0f;

		beastIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconAddBgBtn, string.Empty);

		// Set Weapon Or Skills Or Dan.
		switch (selectChangeType)
		{
			case SelectChange.Dan:
				var danLocations = PlayerDataUtility.GetDanLocations(CurrentPlayer, currentPositionId, currentShowLocationId);
				danLocations.Sort((l1, l2) =>
				{
					var d1 = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(l1.ResourceId);
					var d2 = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(l2.ResourceId);

					return d1.Type - d2.Type;
				});

				for (int i = 0; i < C_EQUIP_COUNT; i++)
				{
					UIElemAvatarBottomItem item = equipOrSkillPool.AllocateItem().GetComponent<UIElemAvatarBottomItem>();
					item.SetTriggerMethod(this, "OnClickChangeSearchDan");

					int danType = DanConfig._DanType.GetRegisterTypeByIndex(i);
					KodGames.ClientClass.Location location = null;

					int j = 0;
					for (; j < danLocations.Count; j++)
					{
						var danConfig = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(danLocations[j].ResourceId);
						if (danConfig.Type == danType && CurrentPlayer.SearchDan(danLocations[j].Guid) != null)
						{
							location = danLocations[j];
							break;
						}
					}

					if (j >= danLocations.Count)
					{
						location = new KodGames.ClientClass.Location();
						location.PositionId = currentPositionId;
						location.ResourceId = IDSeg.InvalidId;
						location.ShowLocationId = currentShowLocationId;
						location.Guid = string.Empty;
					}

					item.SetData(CurrentPlayer, location, IDSeg._AssetType.Dan, danType);
					equipOrSkillList.AddItem(item.container);
				}
				break;
			case SelectChange.Equipment:

				var equipLocations = PlayerDataUtility.GetEquipmentLocations(CurrentPlayer, currentPositionId, currentShowLocationId);
				equipLocations.Sort((l1, l2) =>
				{
					var e1 = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(l1.ResourceId);
					var e2 = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(l2.ResourceId);

					return e1.type - e2.type;
				});

				for (int i = 0; i < C_EQUIP_COUNT; i++)
				{
					UIElemAvatarBottomItem item = equipOrSkillPool.AllocateItem().GetComponent<UIElemAvatarBottomItem>();
					item.SetTriggerMethod(this, "OnClickChangeWeapon");

					int equipType = EquipmentConfig._Type.GetRegisterTypeByIndex(i);
					KodGames.ClientClass.Location location = null;

					int j = 0;
					for (; j < equipLocations.Count; j++)
					{
						var equipConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipLocations[j].ResourceId);

						if (equipConfig.type == equipType)
						{
							location = equipLocations[j];
							break;
						}
					}

					if (j >= equipLocations.Count)
					{
						location = new KodGames.ClientClass.Location();
						location.PositionId = currentPositionId;
						location.ResourceId = IDSeg.InvalidId;
						location.ShowLocationId = currentShowLocationId;
						location.Guid = string.Empty;
					}

					item.SetData(CurrentPlayer, location, IDSeg._AssetType.Equipment, equipType);
					equipOrSkillList.AddItem(item.container);
				}
				break;
			case SelectChange.Skill:
				var skillLocations = PlayerDataUtility.GetSkillLocations(CurrentPlayer, currentPositionId, currentShowLocationId);

				for (int i = 0; i < C_EQUIP_COUNT; i++)
				{
					UIElemAvatarBottomItem item = equipOrSkillPool.AllocateItem().GetComponent<UIElemAvatarBottomItem>();
					// Set Invoke Method.
					item.SetTriggerMethod(this, "OnClickChangePassiveSkill");

					KodGames.ClientClass.Location location = null;

					for (int j = 0; j < skillLocations.Count; j++)
					{
						if (skillLocations[j].Index == i)
						{
							location = skillLocations[j];
							break;
						}
					}

					if (location == null)
					{
						location = new KodGames.ClientClass.Location();
						location.PositionId = currentPositionId;
						location.ResourceId = IDSeg.InvalidId;
						location.ShowLocationId = currentShowLocationId;
						location.Index = i;
						location.Guid = string.Empty;
					}

					item.SetData(CurrentPlayer, location, IDSeg._AssetType.CombatTurn, i);
					equipOrSkillList.AddItem(item.container);
				}
				break;
			case SelectChange.Beast:
				var beastLocations = PlayerDataUtility.GetBeastLocations(CurrentPlayer, currentPositionId, currentShowLocationId);

				beastTips.Text = GameUtility.GetUIString("UIPnlOrganInfo_Avatar_Attribute_Default");

				KodGames.ClientClass.Location location = null;
				if (beastLocations != null && beastLocations.Count > 0)
				{
					if (CurrentPlayer.SearchBeast(beastLocations[0].Guid) == null)
					{
						beastIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconAddBgBtn, string.Empty);
					}
					else
					{
						var beastShow = CurrentPlayer.SearchBeast(beastLocations[0].Guid);
						var beastBaseInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastShow.ResourceId);

						beastIcon.SetData(beastShow);
						var beastAvatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, currentPositionId, currentShowLocationId);


						beastTips.Text = string.Format(GameUtility.GetUIString("UIPnlOrganInfo_Avatar_Attribute"), beastBaseInfo.BeastName, ItemInfoUtility.GetAssetName(beastAvatar.ResourceId));
					}					
					location = beastLocations[0];
				}

				if (location == null)
				{
					location = new KodGames.ClientClass.Location();
					location.PositionId = currentPositionId;
					location.ResourceId = IDSeg.InvalidId;
					location.ShowLocationId = currentShowLocationId;
					location.Index = 0;
					location.Guid = string.Empty;
				}

				beastIcon.Data = location;
				break;
		}

		// Set Equip Or Skill Notify.
		ControlSelectChangeBtnToEnabledAndNotify(selectChangeType);
	}

	public void SetLight(int avatarIconIndex)
	{
		SetLight(avatarIconIndex, true);
	}

	public void SetLight(int avatarIconIndex, bool controllClip)
	{
		for (int i = 0; i < avatarIconList.Count; i++)
		{
			UIListItemContainer container = avatarIconList.GetItem(i) as UIListItemContainer;
			UIElemLineUpAvatar avatarElem = (UIElemLineUpAvatar)container.Data;

			if (avatarElem == null)
				continue;

			if (avatarElem.Index == avatarIconIndex)
			{
				avatarElem.SetSelectedStat(true);

				if (controllClip)
				{
					if (avatarElem.avatarIcon.GetComponent<UIButton>().Clipped)
						avatarIconList.ScrollToItem(i, 0.3f, EZAnimation.EASING_TYPE.Linear);
				}
				else
					avatarIconList.ScrollToItem(i, 0f, EZAnimation.EASING_TYPE.Linear);
			}
			else
				avatarElem.SetSelectedStat(false);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickActiveSkillIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var skill = assetIcon.Data as KodGames.ClientClass.Skill;
		if (skill != null)
			SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgSkillInfo, skill, false, true, false, false, null, false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSetMasterPosition(UIButton btn)
	{
		RequestMgr.Inst.Request(new SetMasterPositionReq(currentPositionId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarCardIcon(UIButton btn)
	{
		SaveAssemableHistroy();
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarInfo), PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, currentShowLocationId), true, false, false, true, null, true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarChange(UIButton btn)
	{
		SaveAssemableHistroy();
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlSelectAvatarList), PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, currentShowLocationId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPowerUpAvatar(UIButton btn)
	{
		isOverlaySetPower = true;
		SaveAssemableHistroy();
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarLevelUp), PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, currentPositionId, currentShowLocationId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarDetail(UIButton btn)
	{
		var avatarLocation = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, currentShowLocationId);
		var attributes = PlayerDataUtility.GetLocationAvatarAttributes(avatarLocation, CurrentPlayer, false);

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAttributeDetailTip), attributes, CurrentPlayer.SearchAvatar(avatarLocation.Guid), true, currentPositionId, currentShowLocationId, CurrentPlayer);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var avatarLocation = assetIcon.Data as KodGames.ClientClass.Location;

		if (avatarLocation.ShowLocationId == currentShowLocationId)
			return;

		SetAvatarControls(avatarLocation.ShowLocationId);
		SaveAssemableHistroy();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCheerAvatar(UIButton btn)
	{
		if (this.currentShowLocationId < 0)
			return;

		this.currentShowLocationId = -1;

		UIElemLineUpAvatar avatar = (btn.Data as UIElemAssetIcon).Data as UIElemLineUpAvatar;
		InitPartnerUI();
		SetAvatarControlsNotify(avatar.Index);
		SetLight(avatar.Index);
		CaculateAddPropertyByFriends();
		SaveAssemableHistroy();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAddLineupAvatarClick(UIButton btn)
	{
		SaveAssemableHistroy();
		int index = (int)(btn.Data as UIElemAssetIcon).Data;
		KodGames.ClientClass.Location location = new KodGames.ClientClass.Location();
		location.PositionId = currentPositionId;
		location.LocationId = PlayerDataUtility.GetBattlePosByIndexPos(index);
		location.ShowLocationId = location.LocationId;
		location.ResourceId = IDSeg.InvalidId;
		location.Guid = string.Empty;

		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlSelectAvatarList, location);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOpenLvTipShow(UIButton btn)
	{
		int pos = (int)(btn.Data as UIElemAssetIcon).Data;
		int requireLv = ConfigDatabase.DefaultCfg.PositionConfig.GetPositionById(currentPositionId).PositionNums[pos].Level;

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlSlotCantOpen_Text", requireLv));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeWeapon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		List<object> datas = assetIcon.Data as List<object>;

		int equipType = (int)datas[0];
		var location = datas[1] as KodGames.ClientClass.Location;

		selectType = SelectChange.Equipment;

		SaveAssemableHistroy();

		if (string.IsNullOrEmpty(location.Guid))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlSelectEquipmentList, equipType, location);
		else
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlEquipmentInfo, location, true, false, false, true, null, true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangePassiveSkill(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		List<object> datas = assetIcon.Data as List<object>;

		int skillIndex = (int)datas[0];
		var location = datas[1] as KodGames.ClientClass.Location;

		selectType = SelectChange.Skill;

		SaveAssemableHistroy();
		if (string.IsNullOrEmpty(location.Guid))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlSelectSkillList, skillIndex, location);
		else
			SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgSkillInfo, location, true, false, false, true, null, true, skillIndex);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeBeastBtn(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;		
		
		var location = assetIcon.Data as KodGames.ClientClass.Location;

		selectType = SelectChange.Beast;

		SaveAssemableHistroy();
		if (string.IsNullOrEmpty(location.Guid))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlSelectOrganList), location);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrganSelectInfo), location);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeSearchDan(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		List<object> datas = assetIcon.Data as List<object>;

		int danType = (int)datas[0];
		var location = datas[1] as KodGames.ClientClass.Location;

		selectType = SelectChange.Dan;

		SaveAssemableHistroy();
		var avatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, currentPositionId, location.ShowLocationId);
		if (string.IsNullOrEmpty(location.Guid))
			SysUIEnv.Instance.ShowUIModule<UIPnlSelectDanList>(danType, location, avatar);
		else
			SysUIEnv.Instance.ShowUIModule<UIPnlDanInfo>(location, danType, avatar, true, true, false, false, false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeEquip(UIButton btn)
	{
		SetEquipOrSkillUI(SelectChange.Equipment);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeSkill(UIButton btn)
	{
		SetEquipOrSkillUI(SelectChange.Skill);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeDan(UIButton btn)
	{
		SetEquipOrSkillUI(SelectChange.Dan);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeBeast(UIButton btn)
	{
		SetEquipOrSkillUI(SelectChange.Beast);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeDanPlayerLevelByOpenFunciton(UIButton btn)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlAvatar_PlayerLevelByOpenFunciton", ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.DanHome)));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeBeastPlayerLevelByOpenFunciton(UIButton btn)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlAvatar_Beast_PlayerLevelByOpenFunciton", ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Beast)));
	}

	public void OnSetMasterSuccess(int lastPositionId)
	{
		isShowPowerTips = true;
		isSetMaster = true;
		changPower = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, lastPositionId);
		ShowPower();

		positionActiveButton.controlIsEnabled = false;
		positionActiveBox.Hide(false);

		// Update UIPnlMainMenuBot.
		SetMenuBottomIconNotify();
	}

	public void OnChangeSkillSuccess(KodGames.ClientClass.Location location, int skillSoltIndex)
	{
		isShowPowerTips = true;

		OnChangeSuccessCheckAssemble(location, SuiteConfig.AssembleSetting.Requirement._Type.CombatTurn);
	}

	public void OnChangeEquipmentSuccess(KodGames.ClientClass.Location location, int equipType)
	{
		isShowPowerTips = true;

		OnChangeSuccessCheckAssemble(location, SuiteConfig.AssembleSetting.Requirement._Type.Equipment);
	}

	public void OnChangeDanSuccess(KodGames.ClientClass.Location location, int equipType)
	{
		isShowPowerTips = true;
		ShowPower();

		//OnChangeSuccessCheckAssemble(location, SuiteConfig.AssembleSetting.Requirement._Type.Dan);
	}

	public void OnChangeBeastSuccess(KodGames.ClientClass.Location location)
	{
		Debug.Log("OnChangeBeastSuccess");

		isShowPowerTips = true;
		ShowPower();

		//OnChangeSuccessCheckAssemble(location, SuiteConfig.AssembleSetting.Requirement._Type.Dan);
	}

	public void OnChangeAvatarSuccess(KodGames.ClientClass.Location location)
	{
		isShowPowerTips = true;

		int avatarIconIndex = PlayerDataUtility.GetIndexPosByBattlePos(location.ShowLocationId);

		// Change Avatar Icon.
		for (int index = 0; index < avatarIconList.Count; index++)
		{
			UIElemLineUpAvatar listItem = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;
			if (listItem == null)
				continue;

			if (avatarIconIndex == listItem.Index)
			{
				listItem.SetData(location, this, "OnClickAvatarIcon");
				break;
			}
		}

		if (!this.IsOverlayed && this.IsShown)
			SetAvatarControls(location.ShowLocationId);
		else
			delayShowAvatarLocationId = location.ShowLocationId;

		OnChangeSuccessCheckAssemble(location, SuiteConfig.AssembleSetting.Requirement._Type.Avatar);
	}

	/// <summary>
	/// 验证是否有套装和缘分激活
	/// </summary>
	/// <param name="location"></param>
	private void OnChangeSuccessCheckAssemble(KodGames.ClientClass.Location location, int type)
	{
		if (location == null)
			return;
		//ClearTips();
		//得到当前角色
		var avatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, currentPositionId, location.ShowLocationId);
		if (avatar == null) return;
		//得到当前选中角色信息
		AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);
		if (avatarConfig == null) return;
		//装备
		List<KodGames.ClientClass.Equipment> equipedEquipment = PlayerDataUtility.GetLineUpEquipments(CurrentPlayer, location.PositionId, location.ShowLocationId);
		//书籍
		List<KodGames.ClientClass.Skill> equipedSkills = PlayerDataUtility.GetLineUpSkills(CurrentPlayer, location.PositionId, location.ShowLocationId);

		//获取当前角色装备套装激活ID和GUID
		object[] allIDs = GetEquipMatchSuitIds(equipedEquipment, equipedSkills);
		List<string> guids = allIDs[1] as List<string>;
		List<int> equipMatchIds = allIDs[0] as List<int>;

		//更新角色时，跟新其他角色缘分激活
		if (type == SuiteConfig.AssembleSetting.Requirement._Type.Avatar)
		{

			//阵容
			List<KodGames.ClientClass.Avatar> equipedAvatars = PlayerDataUtility.GetLineUpAvatars(CurrentPlayer, location.PositionId);
			//雇佣 缘分
			List<KodGames.ClientClass.Avatar> equipedCheerAvatars = PlayerDataUtility.GetCheerAvatars(CurrentPlayer, location.PositionId);

			//得到阵中其他角色ID
			List<int> OtherAvatarMatchIds = GetAvatarMatchFateIds(equipedAvatars);
			//阵中其他角色
			List<KodGames.ClientClass.Avatar> squadAvatars = new List<KodGames.ClientClass.Avatar>();

			//重新排序
			List<KodGames.ClientClass.Avatar> resultAvatars = new List<KodGames.ClientClass.Avatar>();
			for (int index = 0; index < avatarIconList.Count; index++)
			{
				UIElemLineUpAvatar item = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;
				if (item == null)
					continue;
				var locationAvatar = item.avatarIcon.Data as KodGames.ClientClass.Location;
				if (locationAvatar == null)
					continue;
				foreach (KodGames.ClientClass.Avatar equiAvatar in equipedAvatars)
				{
					if (locationAvatar.ResourceId == equiAvatar.ResourceId)
						squadAvatars.Add(equiAvatar);
				}
			}

			//去重操作
			foreach (KodGames.ClientClass.Avatar equiAvatar in squadAvatars)
			{
				if (!resultAvatars.Contains(equiAvatar))
					resultAvatars.Add(equiAvatar);
			}

			List<int> OtherIds = new List<int>();

			OtherIds.AddRange(OtherAvatarMatchIds);
			foreach (KodGames.ClientClass.Avatar juniorAvatar in equipedCheerAvatars)
				OtherIds.Add(juniorAvatar.ResourceId);


			hasAssembleSettingId.AddRange(GetHasActivateFate(resultAvatars, OtherIds, location.ResourceId));

			foreach (KodGames.ClientClass.Avatar equiAvatar in resultAvatars)
			{
				AvatarConfig.Avatar equiAvatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(equiAvatar.ResourceId);
				if (equiAvatar.ResourceId == location.ResourceId)
					MuchFateActivity(equiAvatar, equiAvatarConfig, location, OtherIds);
				else
				{
					List<int> ResourceIds = new List<int>();
					ResourceIds.Add(location.ResourceId);
					MuchFateActivity(equiAvatar, equiAvatarConfig, location, ResourceIds);
				}

			}
		}

		//缘分
		MuchFateActivity(avatar, avatarConfig, location, equipMatchIds);
		//套装
		MuchSuitActivity(location, avatar, equipMatchIds, guids);

		//重新计算战力值
		int value = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, currentPositionId);

		powerValue.Text = GameUtility.FormatUIString("UIPnlAvatar_Label_Power", PlayerDataUtility.GetPowerString(value));
		ShowPower();
	}

	/// <summary>
	/// 得到玩家角色已经激活过的缘分对象
	/// </summary>
	/// <param name="resultAvatars"></param>
	/// <returns></returns>
	private List<int> GetHasActivateFate(List<KodGames.ClientClass.Avatar> resultAvatars,
		List<int> OtherIds, int currentUpdateAvatarResourceId)
	{
		List<int> hasAssembleSettings = new List<int>();
		if (OtherIds.Contains(currentUpdateAvatarResourceId))
			OtherIds.Remove(currentUpdateAvatarResourceId);

		foreach (KodGames.ClientClass.Avatar equiAvatar in resultAvatars)
		{

			AvatarConfig.Avatar equiAvatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(equiAvatar.ResourceId);
			List<int> assemableIds = equiAvatarConfig.assemableIds;
			if (equiAvatar.ResourceId == currentUpdateAvatarResourceId) continue;
			for (int i = 0; i < assemableIds.Count; i++)
			{
				//根据缘分或者套装ID得到Info
				SuiteConfig.AssembleSetting assembleSetting = ConfigDatabase.DefaultCfg.SuiteConfig.GetAssembleSettingById(assemableIds[i]);
				if (assembleSetting == null)
					continue;

				List<List<int>> assembleList = GetAssembleSettingByAllRequireIds(assembleSetting);

				if (IsRequirementTwoList(assembleList, OtherIds))
				{
					//if (CheckCurrentAssembleIdIsList(assembleList, currentUpdateAvatarResourceId) && hasAssembleSettingId.Contains(assembleSetting.Id))
					//	hasAssembleSettingId.Remove(assembleSetting.Id);
					//else
					hasAssembleSettings.Add(assembleSetting.Id);
				}
			}
		}
		return hasAssembleSettings;
	}

	private bool CheckCurrentAssembleIdIsList(List<List<int>> assembleList, int currentUpdateAvatarResourceId)
	{
		foreach (List<int> assmableID in assembleList)
		{
			if (assmableID.Contains(currentUpdateAvatarResourceId))
				return true;
		}
		return false;
	}

	/// <summary>
	/// 保存历史的缘分
	/// </summary>
	private void SaveAssemableHistroy()
	{
		hasAssembleSettingId.Clear();
		if (currentShowLocationId == -1 || currentPositionId == IDSeg.InvalidId)
			return;

		var avatarLocation = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, currentShowLocationId);
		if (avatarLocation == null)
			return;

		List<KodGames.ClientClass.Avatar> avatarOthers = new List<KodGames.ClientClass.Avatar>();
		//阵容
		List<KodGames.ClientClass.Avatar> equipedAvatars = PlayerDataUtility.GetLineUpAvatars(CurrentPlayer, avatarLocation.PositionId);
		//雇佣
		avatarOthers.AddRange(equipedAvatars);
		foreach (KodGames.ClientClass.Avatar equiAvatar in avatarOthers)
		{
			var avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(equiAvatar.ResourceId);
			for (int i = 0; i < avatarConfig.assemableIds.Count; i++)
			{
				SuiteConfig.AssembleSetting assembleSetting = ConfigDatabase.DefaultCfg.SuiteConfig.GetAssembleSettingById(avatarConfig.assemableIds[i]);
				if (assembleSetting == null)
					continue;
				bool isAssembleActive = PlayerDataUtility.CheckAvatarAssemble(assembleSetting, equiAvatar, CurrentPlayer, currentPositionId, currentShowLocationId);
				if (isAssembleActive) hasAssembleSettingId.Add(assembleSetting.Id);
			}
		}
	}
	/// <summary>
	/// 验证是否有套装和缘分激活
	/// </summary>
	/// <param name="location"></param>
	private void OnChangeSuccessCheckAssemble(KodGames.ClientClass.Avatar avatar, int type)
	{
		if (avatar == null)
			return;
		//ClearTips();

		//更新角色时，跟新其他角色缘分激活
		if (type == SuiteConfig.AssembleSetting.Requirement._Type.Avatar)
		{

			List<int> OtherAvatarMatchIds = new List<int>();
			//当前阵位上的角色
			List<KodGames.ClientClass.Location> avartarLocations = PlayerDataUtility.GetAvatarLocations(CurrentPlayer, this.currentPositionId);
			foreach (var location in avartarLocations)
				OtherAvatarMatchIds.Add(location.ResourceId);

			//当前助阵上的角色
			var parterAvatars = PlayerDataUtility.GetCheerCalculatorAvatars(CurrentPlayer, currentPositionId);
			for (int i = 0; i < parterAvatars.Count; i++)
				OtherAvatarMatchIds.Add(parterAvatars[i].id);


			for (int index = 0; index < avatarIconList.Count; index++)
			{
				UIElemLineUpAvatar item = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;
				if (item == null)
					continue;
				var location = item.avatarIcon.Data as KodGames.ClientClass.Location;
				if (location == null)
					continue;
				AvatarConfig.Avatar equiAvatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(location.ResourceId);
				//缘分
				List<int> assemableIds = equiAvatarConfig.assemableIds;//得到当前角色所有的缘分，或套装ID

				for (int i = 0; i < assemableIds.Count; i++)
				{
					//根据缘分或者套装ID得到Info
					SuiteConfig.AssembleSetting assembleSetting = ConfigDatabase.DefaultCfg.SuiteConfig.GetAssembleSettingById(assemableIds[i]);
					if (assembleSetting == null)
						continue;

					if (hasAssembleSettingId.Contains(assembleSetting.Id))
					{
						hasAssembleSettingId.Remove(assembleSetting.Id);
						continue;
					}

					//根据该角色下所有匹配的缘分，进行匹配缘分，并取得信息
					if (IsAssembleSettingInfosByMatchIDForLiz(assembleSetting, OtherAvatarMatchIds, avatar.ResourceId))
					{
						int maxCount = 0;
						string infos = GetModifierInfoStr(assembleSetting, 0, ref maxCount);
						string message = GameUtility.FormatUIString("UIPnlAvatarInfo_TraitDesc_AssembleTipContext",
							string.Format("{0}{1}{2}", ItemInfoUtility.GetAssetQualityLongColor(location.ResourceId),
								ItemInfoUtility.GetAssetName(location.ResourceId), GameDefines.textColorTipsInBlack),
							GameDefines.txColorGreen,
							assembleSetting.Name,
							 GameDefines.textColorTipsInBlack,
							infos);

						if (!tipMsgs.Contains(message))
							tipMsgs.Add(message);
					}
				}
			}
		}

		//重新计算战力值
		int value = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, currentPositionId);

		powerValue.Text = GameUtility.FormatUIString("UIPnlAvatar_Label_Power", PlayerDataUtility.GetPowerString(value));

		ShowPower();
	}

	private void ShowTips()
	{
		foreach (string msg in tipMsgs)
		{
			SysUIEnv.Instance.AddTip(msg);
		}
		tipMsgs.Clear();
		//ClearTips();
	}

	//private void ClearTips()
	//{
	//    tipMsgs.Clear();
	//}

	/// <summary>
	/// 得到一个缘分或者套装所要满足的条件，所有的ID
	/// </summary>
	/// <param name="assembleSetting"></param>
	/// <returns></returns>
	private List<List<int>> GetAssembleSettingByAllRequireIds(SuiteConfig.AssembleSetting assembleSetting)
	{
		List<List<int>> assembleList = new List<List<int>>();
		foreach (SuiteConfig.AssembleSetting.Part part in assembleSetting.Parts)
		{
			if (part.Requiremets == null || part.Requiremets.Count == 0)
				return null;
			List<int> requireIds = new List<int>();
			foreach (SuiteConfig.AssembleSetting.Requirement require in part.Requiremets)
			{
				requireIds.Add(require.Value);
			}
			assembleList.Add(requireIds);
		}
		return assembleList;
	}

	/// <summary>
	/// 当前角色的有多少缘分激活，并得到显示信息
	/// </summary>
	/// <param name="avatar"></param>
	/// <param name="avatarConfig"></param>
	/// <param name="location"></param>
	/// <param name="matchIds"></param>
	/// <returns></returns>
	private void MuchFateActivity(KodGames.ClientClass.Avatar avatar, AvatarConfig.Avatar avatarConfig,
		KodGames.ClientClass.Location location, List<int> matchIds)
	{
		//缘分
		List<int> assemableIds = avatarConfig.assemableIds;//得到当前角色所有的缘分，或套装ID
		for (int i = 0; i < assemableIds.Count; i++)
		{
			//根据缘分或者套装ID得到Info
			SuiteConfig.AssembleSetting assembleSetting = ConfigDatabase.DefaultCfg.SuiteConfig.GetAssembleSettingById(assemableIds[i]);
			if (assembleSetting == null)
				continue;


			if (hasAssembleSettingId.Contains(assembleSetting.Id))
			{
				hasAssembleSettingId.Remove(assembleSetting.Id);
				continue;
			}

			//验证是否有缘分被激活
			bool isAssembleActive = PlayerDataUtility.CheckAvatarAssemble(assembleSetting, avatar, CurrentPlayer, currentPositionId, location.ShowLocationId);
			if (!isAssembleActive)
				continue;

			//根据该角色下所有匹配的缘分或套装 ID，进行匹配缘分，或套装，并取得信息
			if (IsAssembleSettingInfosByMatchID(assembleSetting, matchIds))
			{
				int maxCount = 0;
				string infos = GetModifierInfoStr(assembleSetting, 0, ref maxCount);
				string message = GameUtility.FormatUIString("UIPnlAvatarInfo_TraitDesc_AssembleTipContext",
					string.Format("{0}{1}{2}", ItemInfoUtility.GetAssetQualityLongColor(avatar.ResourceId),
						ItemInfoUtility.GetAssetName(avatar.ResourceId), GameDefines.textColorTipsInBlack),
					GameDefines.txColorGreen,
					assembleSetting.Name,
					 GameDefines.textColorTipsInBlack,
					infos);
				if (!tipMsgs.Contains(message))
					tipMsgs.Add(message);
			}
		}
	}

	/// <summary>
	/// 角色套装激活信息[多个]
	/// </summary>
	/// <param name="location"></param>
	/// <param name="suitIdList">所有装备和图书ID</param>
	/// <returns></returns>
	private void MuchSuitActivity(KodGames.ClientClass.Location location,
		KodGames.ClientClass.Avatar avatar, List<int> suitIdList, List<string> guids)
	{
		List<SuiteConfig.AssembleSetting> suits = ConfigDatabase.DefaultCfg.SuiteConfig.GetEquipmentSuitesByRequireId(location.ResourceId);

		if (suits == null || suits.Count == 0)
			return;
		foreach (SuiteConfig.AssembleSetting assembleSetting in suits)
		{
			if (assembleSetting == null)
				continue;

			if (hasAssembleSettingId.Contains(assembleSetting.Id))
			{
				hasAssembleSettingId.Remove(assembleSetting.Id);
				continue;
			}
			List<List<int>> assembleList = GetAssembleSettingByAllRequireIds(assembleSetting);
			List<string> resultGUIDs = SortGUID(GetRequirementForGUIIDList(assembleList, suitIdList, guids));
			int requiredCount = resultGUIDs.Count;
			if (requiredCount > 1)
			{
				if (IsAssembleSettingInfosByMatchID(assembleSetting, new List<int>() { location.ResourceId }))
				{
					int maxCount = 0;
					//根据该角色下所有匹配的缘分或套装 ID，进行匹配缘分，或套装，并取得信息
					string infos = GetModifierInfoStr(assembleSetting, requiredCount, ref maxCount);
					if (infos != null)
					{
						KodGames.ClientClass.Equipment equipment = SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(resultGUIDs[0]);
						KodGames.ClientClass.Skill skill = null;
						if (equipment == null)
							skill = SysLocalDataBase.Inst.LocalPlayer.SearchSkill(resultGUIDs[0]);
						if (skill != null || equipment != null)
						{

							string message = GameUtility.FormatUIString("UIPnlAvatarInfo_TraitDesc_AssembleTipContext_Suit",
								ItemInfoUtility.GetAssetQualityLongColor(equipment == null ? skill.ResourceId : equipment.ResourceId),
							assembleSetting.Name,
							 GameDefines.textColorTipsInBlack,
							infos,
							requiredCount,
							maxCount
							);
							if (!tipMsgs.Contains(message))
								tipMsgs.Add(message);
						}
					}
				}
			}
		}

	}

	/// <summary>
	/// 获取当前角色装备套装激活ID
	/// </summary>
	/// <param name="equipedEquipment"></param>
	/// <param name="equipedSkills"></param>
	/// <returns></returns>
	private object[] GetEquipMatchSuitIds(List<KodGames.ClientClass.Equipment> equipedEquipment,
		List<KodGames.ClientClass.Skill> equipedSkills)
	{
		object[] objects = new object[2];
		List<string> guids = new List<string>();
		List<int> matchIds = new List<int>();
		for (int i = 0; i < equipedEquipment.Count; i++)
		{
			if (!matchIds.Contains(equipedEquipment[i].ResourceId))
				matchIds.Add(equipedEquipment[i].ResourceId);
			if (!guids.Contains(equipedEquipment[i].Guid))
				guids.Add(equipedEquipment[i].Guid);
		}
		for (int i = 0; i < equipedSkills.Count; i++)
		{
			if (!matchIds.Contains(equipedSkills[i].ResourceId))
				matchIds.Add(equipedSkills[i].ResourceId);
			if (!guids.Contains(equipedSkills[i].Guid))
				guids.Add(equipedSkills[i].Guid);
		}
		objects[0] = matchIds;
		objects[1] = guids;
		return objects;
	}

	/// <summary>
	/// 获得当前角色所有满足条件的缘分激活ID
	/// </summary>
	/// <param name="equipedAvatars"></param>
	/// <returns></returns>
	private List<int> GetAvatarMatchFateIds(List<KodGames.ClientClass.Avatar> equipedAvatars)
	{
		var matchIds = new List<int>();

		for (int i = 0; i < equipedAvatars.Count; i++)
			if (!matchIds.Contains(equipedAvatars[i].ResourceId))
			{
				matchIds.Add(equipedAvatars[i].ResourceId);
			}

		return matchIds;
	}

	/// <summary>
	/// 检查是否有满足的套装，如果有，返回套装RequiredCount
	/// </summary>
	/// <param name="assembleList">缘分或者套装必备ID</param>
	/// <param name="suitIdList">当前用户所持有的ID</param>
	/// <returns></returns>
	private List<string> GetRequirementForGUIIDList(List<List<int>> assembleList, List<int> suitIdList, List<string> guids)
	{
		List<string> levels = new List<string>();
		foreach (List<int> ids in assembleList)
		{
			string level = GetRequirementToGUID(ids, suitIdList, guids);
			if (level != null)
				levels.Add(level);
		}
		return levels;
	}

	/// <summary>
	/// 检查是否有满足的套装
	/// </summary>
	/// <param name="assembleList">缘分或者套装必备ID</param>
	/// <param name="suitIdList">当前用户所持有的ID</param>
	/// <returns></returns>
	private bool IsRequirementTwoList(List<List<int>> assembleList, List<int> suitIdList)
	{
		foreach (List<int> ids in assembleList)
		{
			if (!IsRequirementToSuit(ids, suitIdList))
			{
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// 根据guid得到级别倒序排序 
	/// </summary>
	/// <param name="resultGUIDs"></param>
	/// <returns></returns>
	private List<string> SortGUID(List<string> resultGUIDs)
	{
		string temp;
		for (int i = 0; i < resultGUIDs.Count; i++)
		{
			for (int j = 0; j < resultGUIDs.Count - i - 1; j++)
			{
				int levelFirest = -1;
				int levelNext = -1;

				var equip1 = SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(resultGUIDs[j]);
				if (equip1 == null)
				{
					var skill1 = SysLocalDataBase.Inst.LocalPlayer.SearchSkill(resultGUIDs[j]);
					if (skill1 != null)
						levelFirest = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill1.ResourceId).qualityLevel;
				}
				else
				{
					levelFirest = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equip1.ResourceId).qualityLevel;
				}
				var equip2 = SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(resultGUIDs[j + 1]);
				if (equip2 == null)
				{
					var skill2 = SysLocalDataBase.Inst.LocalPlayer.SearchSkill(resultGUIDs[j + 1]);
					if (skill2 != null)
						levelNext = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill2.ResourceId).qualityLevel;
				}
				else
				{
					levelNext = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equip2.ResourceId).qualityLevel;
				}
				if (levelFirest > levelNext)
				{
					temp = resultGUIDs[j];
					resultGUIDs[j] = resultGUIDs[j + 1];
					resultGUIDs[j + 1] = temp;
				}

			}
		}
		return resultGUIDs;
	}

	/// <summary>
	/// 检查套装是否满足
	/// </summary>
	/// <returns></returns>
	private bool IsRequirementToSuit(List<int> ids, List<int> suitIdList)
	{
		for (int i = 0; i < ids.Count; i++)
		{
			if (suitIdList.Contains(ids[i]))
				return true;
		}
		return false;
	}

	/// <summary>
	/// 检查套装是否满足
	/// </summary>
	/// <returns></returns>
	private string GetRequirementToGUID(List<int> ids, List<int> suitIdList, List<string> guids)
	{
		for (int i = 0; i < ids.Count; i++)
		{
			for (int j = 0; j < suitIdList.Count; j++)
			{
				if (suitIdList[j] == ids[i])
					return guids[j];
			}
		}
		return null;
	}

	/// <summary>
	///	验证是否该角色下所有匹配的缘分或套装 ID有满足条件
	/// </summary>
	/// <param name="avatarAssemble"></param>
	/// <param name="matchIds"></param>
	/// <returns></returns>
	private bool IsAssembleSettingInfosByMatchID(SuiteConfig.AssembleSetting avatarAssemble, List<int> matchIds)
	{
		if (matchIds == null || matchIds.Count == 0)
			return false;
		if (avatarAssemble.Parts == null || avatarAssemble.Parts.Count == 0)
			return false;

		foreach (SuiteConfig.AssembleSetting.Part part in avatarAssemble.Parts)
		{
			if (part.Requiremets == null || part.Requiremets.Count == 0)
				return false;

			foreach (SuiteConfig.AssembleSetting.Requirement require in part.Requiremets)
			{
				if (matchIds.Contains(require.Value))
					return true;
			}
		}
		return false;
	}

	/// <summary>
	///	验证是否该缘分或套装 ID有满足条件  [助阵里面的选择]
	/// </summary>
	/// <param name="avatarAssemble"></param>
	/// <param name="matchIds"></param>
	/// <returns></returns>
	private bool IsAssembleSettingInfosByMatchIDForLiz(SuiteConfig.AssembleSetting avatarAssemble, List<int> matchIds, int avtarHaveID)
	{
		if (matchIds == null || matchIds.Count == 0)
			return false;
		if (avatarAssemble.Parts == null || avatarAssemble.Parts.Count == 0)
			return false;
		if (!IsAvatarIdExistAssembleSetting(avatarAssemble, avtarHaveID))
			return false;

		foreach (SuiteConfig.AssembleSetting.Part part in avatarAssemble.Parts)
		{
			if (part.Requiremets == null || part.Requiremets.Count == 0)
				return false;
			if (!IsContainsItems(matchIds, part.Requiremets))
				return false;
		}
		return true;
	}

	/// <summary>
	/// 判断缘分是否存在某个角色ID
	/// </summary>
	/// <param name="avatarAssemble">缘分</param>
	/// <param name="avtarHaveID">角色ID</param>
	/// <returns></returns>
	private bool IsAvatarIdExistAssembleSetting(SuiteConfig.AssembleSetting avatarAssemble, int avtarHaveID)
	{
		foreach (SuiteConfig.AssembleSetting.Part part in avatarAssemble.Parts)
		{
			foreach (var requirement in part.Requiremets)
			{
				if (requirement.Value == avtarHaveID)
					return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 判断缘分下的Requitement 或
	/// </summary>
	/// <param name="matchIds"></param>
	/// <param name="requirList"></param>
	/// <returns></returns>
	private bool IsContainsItems(List<int> matchIds, List<SuiteConfig.AssembleSetting.Requirement> requirList)
	{
		foreach (var requir in requirList)
		{
			if (matchIds.Contains(requir.Value))
				return true;
		}
		return false;
	}


	private List<int> _types;
	private List<int> AvatarAttributeTypes
	{
		get
		{
			if (_types == null)
			{
				_types = new List<int>();
				_types.Add(ClientServerCommon._AvatarAttributeType.PAR);
				_types.Add(ClientServerCommon._AvatarAttributeType.MAR);
				_types.Add(ClientServerCommon._AvatarAttributeType.PDR);
				_types.Add(ClientServerCommon._AvatarAttributeType.MDR);
				_types.Add(ClientServerCommon._AvatarAttributeType.PMDR);
				_types.Add(ClientServerCommon._AvatarAttributeType.HR);
				_types.Add(ClientServerCommon._AvatarAttributeType.DgR);
				_types.Add(ClientServerCommon._AvatarAttributeType.CSR);
				_types.Add(ClientServerCommon._AvatarAttributeType.RR);
				_types.Add(ClientServerCommon._AvatarAttributeType.CR);
				_types.Add(ClientServerCommon._AvatarAttributeType.RCR);
				_types.Add(ClientServerCommon._AvatarAttributeType.AR);
				_types.Add(ClientServerCommon._AvatarAttributeType.DR);
				_types.Add(ClientServerCommon._AvatarAttributeType.FAR);
			}
			return _types;
		}
	}

	/// <summary>
	/// 取得增加属性信息 用于显示
	/// </summary>
	/// <param name="avatarAssemble"></param>
	/// <returns></returns>
	private string GetModifierInfoStr(SuiteConfig.AssembleSetting avatarAssemble, int requiredCount, ref int maxCount)
	{
		string attributes = null;


		List<SuiteConfig.AssembleSetting.Assemble> assembleList = avatarAssemble.Assembles;

		if (assembleList != null && assembleList.Count > 0)
			maxCount = avatarAssemble.Assembles[avatarAssemble.Assembles.Count - 1].RequiredCount;

		foreach (SuiteConfig.AssembleSetting.Assemble assemble in assembleList)
		{
			SuiteConfig.AssembleSetting.Assemble assInfo = null;
			if (avatarAssemble.Type == SuiteConfig._Type.AvatarAssemble)
			{
				assInfo = assemble;
			}
			else if (avatarAssemble.Type == SuiteConfig._Type.EquipmentSuite && assemble.RequiredCount == requiredCount)
			{
				assInfo = assemble;
			}
			if (assInfo != null)
			{
				List<PropertyModifier> propertyModifiers = assInfo.ModifierSet.modifiers;
				for (int i = 0, j = 0; i < propertyModifiers.Count; i++)
				{
					PropertyModifier pmod = propertyModifiers[i];
					if (pmod.attributeValue > 0)
					{
						if (IsAttributeTypeEqual(propertyModifiers, pmod))
							continue;

						string attributeValue = "";
						if (pmod.modifyType == PropertyModifier._ValueModifyType.Percentage
						   || (pmod.modifyType == PropertyModifier._ValueModifyType.Value && AvatarAttributeTypes.Contains(pmod.attributeType))
						   )
							attributeValue = (pmod.attributeValue * 100) + "%";
						else
							attributeValue = pmod.attributeValue.ToString();
						string formatstr = GameUtility.GetUIString("UIPnlAvatarInfo_TraitDesc_InfoMessage");
						if (j % 2 != 0)
							formatstr += "\n";
						attributes += string.Format(formatstr,
							ClientServerCommon._AvatarAttributeType.GetDisplayNameByType(pmod.attributeType, ConfigDatabase.DefaultCfg),
							GameUtility.GetUIString("UIPnlAvatarInfo_TraitDesc_Ascension"),
							GameDefines.txColorGreen,
							attributeValue,
							GameDefines.textColorTipsInBlack
							);
						j++;
					}

				}
			}
		}

		return attributes != null ? attributes.Substring(0, attributes.Length - 2) : null;
	}

	private bool IsAttributeTypeEqual(List<PropertyModifier> propertyModifiers, PropertyModifier pmod)
	{
		foreach (PropertyModifier proMod in propertyModifiers)
		{
			if (proMod.attributeType == ClientServerCommon._AvatarAttributeType.MAP
				&& proMod.attributeValue == pmod.attributeValue
				&& pmod.attributeType == ClientServerCommon._AvatarAttributeType.PAP

				)
				return true;
			else if (proMod.attributeType == ClientServerCommon._AvatarAttributeType.MAR
				&& proMod.attributeValue == pmod.attributeValue
				&& pmod.attributeType == ClientServerCommon._AvatarAttributeType.PAR

				)
				return true;
		}
		return false;
	}

	public void OnDinerAvatarFiredByTimes()
	{
		ChangeUIMode(this.currentMode, false);
	}

	#endregion

	#region CheerAvatar
	private void InitPartnerUI()
	{
		// InActive avatarCrontroll .
		avatarOnRoot.SetActive(false);
		avatarLineUpRoot.SetActive(false);
		cheerAvatarRoot.SetActive(true);

		// Set Icon Control State.
		SetAvatarIconListState(avatarIconList.Count);

		// Set UIModel.
		SetUIModueUI(UIMode.AvatarOn);

		// Init Partner. 
		var partners = ConfigDatabase.DefaultCfg.PartnerConfig.Partners;
		var dic_partners = GetPartnerAvatars(currentPositionId);

		// Set partner icon.
		for (int i = 0; i < partners.Count; i++)
		{
			cheerAvatarIcons[i].Data = partners[i].PartnerId;
			cheerAvatarIcons[i].Hide(!partners[i].IsOpen);

			if (!partners[i].IsOpen)
				continue;

			KodGames.ClientClass.Partner partner = null;
			if (dic_partners.ContainsKey(partners[i].PartnerId))
				partner = dic_partners[partners[i].PartnerId];

			SetPartnerIcon(cheerAvatarIcons[i], partner);
		}

		// Set CheerAvatar Icon.
		cheerAvatarList.ClearList(false);
		cheerAvatarList.ScrollPosition = 0f;
		for (int i = 0; i < partners.Count; i++)
		{
			if (!partners[i].IsOpen)
				continue;

			var cheerAvarar = cheerAvatarPool.AllocateItem();
			var itemContainer = cheerAvarar.GetComponent<UIListItemContainer>();
			UIElemAvatarBottomItem item = cheerAvarar.GetComponent<UIElemAvatarBottomItem>();
			item.assetIcon.Data = partners[i].PartnerId;
			itemContainer.Data = item;
			KodGames.ClientClass.Partner partner = null;
			if (dic_partners.ContainsKey(partners[i].PartnerId))
				partner = dic_partners[partners[i].PartnerId];

			SetPartnerBottomIcon(item, partner);

			cheerAvatarList.AddItem(item.container);
		}

		cheerAvatarList.ScrollToItem(0, 0);

		SnapToPartner(partners[0].PartnerId);
	}

	private void SetPartnerIcon(UIElemAssetIcon partIcon, KodGames.ClientClass.Partner partner)
	{
		int partnerId = (int)partIcon.Data;

		Color whiteColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1.0f);
		Color whiteAlphaColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0.5f);

		if (partner == null)
		{
			partIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCheerAvatarClose, string.Empty);
			partIcon.SetTriggerMethod(this, "OnClickPartnerIconForActive");
			partIcon.assetNameLabel.SetColor(whiteAlphaColor);
		}
		else if (string.IsNullOrEmpty(partner.AvatarGuid))
		{
			partIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCheerAvatarActive, string.Empty);
			partIcon.SetTriggerMethod(this, "OnClickPartnerIconForShowDesc");
			partIcon.assetNameLabel.SetColor(whiteAlphaColor);
		}
		else
		{
			partIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCheerAvatarOpen, string.Empty);
			partIcon.SetTriggerMethod(this, "OnClickPartnerIconForShowDesc");
			partIcon.assetNameLabel.SetColor(whiteColor);
		}

		partIcon.Text = ItemInfoUtility.GetAssetName(partnerId).Substring(0, 1);
	}



	private void SetPartnerBottomIcon(UIElemAvatarBottomItem item, KodGames.ClientClass.Partner partner)
	{
		int partnerId = (int)item.assetIcon.Data;

		if (partner == null)
		{
			item.assetIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, GameUtility.FormatUIString("UIPnlAvatarPartner_Level", ConfigDatabase.DefaultCfg.PartnerConfig.GetPartnerById(partnerId).RequirePlayerLevel));
			item.SetTriggerMethod(this, "OnClickPartnerIconForActive");

		}
		else
		{
			item.SetTriggerMethod(this, "OnClickPartnerIconForChangeAvatar");

			if (string.IsNullOrEmpty(partner.AvatarGuid))
			{
				item.assetIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, ItemInfoUtility.GetAssetName(partnerId));
			}
			else
			{
				item.assetIcon.SetData(CurrentPlayer.SearchAvatar(partner.AvatarGuid));
			}
		}

		item.SetSelectedStat(false);
	}

	private void SnapToPartner(int partnerId)
	{
		for (int i = 0; i < cheerAvatarList.Count; i++)
		{
			UIElemAvatarBottomItem item = cheerAvatarList.GetItem(i).Data as UIElemAvatarBottomItem;

			if (item == null)
				continue;

			item.SetSelectedStat((int)item.assetIcon.Data == partnerId);

			if ((int)item.assetIcon.Data == partnerId)
			{
				item.SetSelectedStat(true);
				cheerAvatarList.ScrollToItem(i, 0.3f, EZAnimation.EASING_TYPE.Linear);
			}
			else
				item.SetSelectedStat(false);
		}
	}

	private UIElemAvatarBottomItem GetSkillOrEquipByIndex(int index)
	{
		UIElemAvatarBottomItem item = equipOrSkillList.GetItem(index).Data as UIElemAvatarBottomItem;

		return item;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPartnerIconForActive(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		int partnerId = (int)assetIcon.Data;
		var partnerCfg = ConfigDatabase.DefaultCfg.PartnerConfig.GetPartnerById(partnerId);

		// Snap Partner Icon.
		SnapToPartner(partnerId);

		if (partnerCfg.RequirePlayerLevel > CurrentPlayer.LevelAttrib.Level || partnerCfg.RequireVipLevel > CurrentPlayer.VipLevel)
		{
			string tips = string.Empty;
			if (partnerCfg.RequireVipLevel > 0)
				tips = GameUtility.FormatUIString("UIPnlAvatarPartner_Open1", partnerCfg.RequirePlayerLevel, partnerCfg.RequireVipLevel, SysLocalDataBase.GetCostsDesc(partnerCfg.Costs));
			else
				tips = GameUtility.FormatUIString("UIPnlAvatarPartner_Open2", partnerCfg.RequirePlayerLevel, SysLocalDataBase.GetCostsDesc(partnerCfg.Costs));

			tips += "\n" + ItemInfoUtility.GetAssetDesc(partnerId);

			UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
			showData.SetData(tips, true, false);
			SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
		}
		else
		{
			MainMenuItem activeMenu = new MainMenuItem();
			activeMenu.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
			activeMenu.CallbackData = partnerCfg.PartnerId;
			activeMenu.Callback = (data) =>
			{
				RequestMgr.Inst.Request(new PartnerOpenReq((int)data));
				return true;
			};

			MainMenuItem cancelMenu = new MainMenuItem();
			cancelMenu.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			string message = GameUtility.FormatUIString("UIDlgMessage_Msg_ActiveCheerPos",
				ItemInfoUtility.GetAssetName(partnerId),
				SysLocalDataBase.GetCostsDesc(SysLocalDataBase.ConvertIdCountList(partnerCfg.Costs), false, false)
				);

			message += "\n" + ItemInfoUtility.GetAssetDesc(partnerId);

			showData.SetData(GameUtility.GetUIString("UIDlgMessage_Title_ActiveCheerPos"), message,
				cancelMenu, activeMenu);

			SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
		}

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPartnerIconForShowDesc(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		int partnerId = (int)assetIcon.Data;

		// Snap Partner Icon.
		SnapToPartner(partnerId);

		// Show Partner Description.
		string tips = ItemInfoUtility.GetAssetDesc(partnerId);
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(tips, true, false);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPartnerIconForChangeAvatar(UIButton btn)
	{
		int partnerId = (int)(btn.Data as UIElemAssetIcon).Data;

		// Snap Partner Icon.
		SnapToPartner(partnerId);

		var partner = CurrentPlayer.PositionData.GetPositionById(currentPositionId).GetPartnerById(partnerId);

		KodGames.ClientClass.Partner tempPartner = new KodGames.ClientClass.Partner();
		tempPartner.PartnerId = partnerId;
		tempPartner.AvatarGuid = partner == null ? string.Empty : partner.AvatarGuid;
		tempPartner.PositionId = currentPositionId;

		if (string.IsNullOrEmpty(tempPartner.AvatarGuid))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlSelectAvatarList), tempPartner);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarInfo), tempPartner, true, false, false, true, null);

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCheerAvatarDetail(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAvatarCheerDetail), CurrentPlayer, currentPositionId);
	}

	public void OnActivePartnerSuccess(int partnerId)
	{
		var partner = CurrentPlayer.PositionData.GetPositionById(currentPositionId).GetPartnerById(partnerId);

		for (int i = 0; i < cheerAvatarIcons.Count; i++)
		{
			if ((int)cheerAvatarIcons[i].Data == partnerId)
			{
				SetPartnerIcon(cheerAvatarIcons[i], partner);
				break;
			}
		}

		for (int i = 0; i < cheerAvatarList.Count; i++)
		{
			var item = cheerAvatarList.GetItem(i).Data as UIElemAvatarBottomItem;

			if ((int)item.assetIcon.Data == partnerId)
			{
				SetPartnerBottomIcon(item, partner);
				break;
			}
		}

		SnapToPartner(partnerId);

		// Show success Message.
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlAvatarPartner_ActiveSucess", ItemInfoUtility.GetAssetName(partnerId)));
		SetAvatarControlsNotify(currentShowLocationId);
	}

	public void OnChangeParternSuccess(int parterId, string avatarOffGuid, string avatarOnGuid, List<KodGames.ClientClass.Partner> partners)
	{
		Debug.Log("OnChangeParternSuccess");
		isShowPowerTips = true;

		for (int i = 0; i < cheerAvatarIcons.Count; i++)
		{
			for (int j = 0; j < partners.Count; j++)
			{
				if ((int)cheerAvatarIcons[i].Data == partners[j].PartnerId)
				{
					SetPartnerIcon(cheerAvatarIcons[i], partners[j]);
					break;
				}
			}
		}

		for (int i = 0; i < cheerAvatarList.Count; i++)
		{
			var item = cheerAvatarList.GetItem(i).Data as UIElemAvatarBottomItem;

			for (int j = 0; j < partners.Count; j++)
			{
				if ((int)item.assetIcon.Data == partners[j].PartnerId)
				{
					SetPartnerBottomIcon(item, partners[j]);
					break;
				}
			}
		}

		SnapToPartner(partners[0].PartnerId);

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPartnerComparison), parterId, avatarOffGuid, avatarOnGuid);

		var avatarSelected = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(avatarOnGuid);
		OnChangeSuccessCheckAssemble(avatarSelected, SuiteConfig.AssembleSetting.Requirement._Type.Avatar);
	}

	public void CaculateAddPropertyByFriends()
	{
		double atkProperty = 0;
		double hpProperty = 0;
		double speedProperty = 0;

		var parterAvatars = PlayerDataUtility.GetCheerCalculatorAvatars(CurrentPlayer, currentPositionId);

		for (int i = 0; i < parterAvatars.Count; i++)
		{
			var attributes = PlayerDataUtility.GetAvatarAttributesForAssistant(parterAvatars[i]);

			for (int j = 0; j < attributes.Count; j++)
			{
				var attrib = attributes[j];

				switch (attrib.type)
				{
					case _AvatarAttributeType.Speed:
						speedProperty += attrib.value;
						break;
					case _AvatarAttributeType.MaxHP:
						hpProperty += attrib.value;
						break;
					case _AvatarAttributeType.MAP:
					case _AvatarAttributeType.PAP:
						atkProperty += attrib.value;
						break;
				}
			}
		}

		addATKByFriends.Text = atkProperty.ToString();
		addHPByFriends.Text = hpProperty.ToString();
		addSpeedByFriends.Text = speedProperty.ToString();
	}

	#endregion

	#region Lineup
	private void ShowAvatarLineUpUI()
	{
		// Show LineUp UI.
		avatarOnRoot.SetActive(false);
		cheerAvatarRoot.SetActive(false);
		avatarLineUpRoot.SetActive(true);

		// Set Icon Control State.
		SetAvatarIconListState(-1);
		SetLight(-1);

		// Init LineUp Icon.
		var positionData = CurrentPlayer.PositionData.GetPositionById(currentPositionId);

		if (positionData != null)
		{
			for (int i = 0; i < lineUpAvatars.Count; i++)
			{
				lineUpAvatars[i].IsClose = false;

				KodGames.ClientClass.Avatar avatar = null;
				bool isRecruite = false;

				for (int j = 0; j < positionData.AvatarLocations.Count; j++)
				{
					if (positionData.AvatarLocations[j].LocationId == lineUpAvatars[i].Position)
					{
						avatar = CurrentPlayer.SearchAvatar(positionData.AvatarLocations[j].Guid);
						isRecruite = positionData.AvatarLocations[j].LocationId == positionData.EmployLocationId;
						break;
					}
				}

				lineUpAvatars[i].SetData(avatar, isRecruite);
			}
		}

		SetAvatarSpeedNumber();
	}

	private void SetAvatarSpeedNumber()
	{
		// LineUp Avatars' Speed.
		List<KodGames.Pair<int, double>> speeds = new List<KodGames.Pair<int, double>>();

		for (int i = 0; i < lineUpAvatars.Count; i++)
		{
			if (lineUpAvatars[i].IsClose || !lineUpAvatars[i].HasAvatar)
				continue;

			var avatarLocation = PlayerDataUtility.GetAvatarLocationByLocationId(CurrentPlayer, currentPositionId, lineUpAvatars[i].Position);

			List<AttributeCalculator.Attribute> attributes = null;
			if (avatarLocation != null)
				attributes = PlayerDataUtility.GetLocationAvatarAttributes(avatarLocation, CurrentPlayer);
			else
				attributes = new List<AttributeCalculator.Attribute>();

			double speed = 0;
			for (int j = 0; j < attributes.Count; j++)
			{
				if (attributes[j].type == _AvatarAttributeType.Speed)
				{
					speed = attributes[j].value;
					break;
				}
			}

			var pair = new KodGames.Pair<int, double>();
			pair.first = lineUpAvatars[i].Position;
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

		for (int i = 0; i < lineUpAvatars.Count; i++)
		{
			if (lineUpAvatars[i].IsClose || !lineUpAvatars[i].HasAvatar)
				lineUpAvatars[i].AvatarShotNum = -1;
			else
				lineUpAvatars[i].AvatarShotNum = locationIds.IndexOf(lineUpAvatars[i].Position) + 1;
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

	private void OnEZDragDropHandler(EZDragDropParams parms)
	{

		UIElemLineUpAvatarItem sourceItem = GetSourceItem(parms.dragObj);
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

					int locationId1 = sourceItem.Position;
					int locationId2 = targetItem.Position;

					KodGames.ClientClass.Avatar sourceAvatar = sourceItem.AvatarData;
					KodGames.ClientClass.Avatar targetAvatar = targetItem.AvatarData;
					bool sIsRecruite = sourceItem.IsRecruiteAvatar;
					bool tIsRecruite = targetItem.IsRecruiteAvatar;
					sourceItem.SetData(targetAvatar, tIsRecruite);
					targetItem.SetData(sourceAvatar, sIsRecruite);

					RequestMgr.Inst.Request(new EmBattleReq(currentPositionId, locationId1, locationId2));
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

	public void OnEmBattleSuccess()
	{
		SetAvatarSpeedNumber();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickLineUpDetail(UIButton btn)
	{
		string detailString = string.Empty;

		var emBattles = ConfigDatabase.DefaultCfg.PositionConfig.EmBattleAttributes;

		emBattles.Sort((e1, e2) =>
		{
			return e1.type - e2.type;
		});

		for (int i = 0; i < emBattles.Count; i++)
		{
			detailString += string.Format("{0}{1}:{2}\n", GameDefines.textColorBtnYellow, PositionConfig._EmBattleType.GetDisplayNameByType(emBattles[i].type, ConfigDatabase.DefaultCfg), GameDefines.textColorWhite);
			detailString += emBattles[i].desc + "\n";
		}

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(detailString, true, false);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}
	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClick3DAvatar(UIButton btn)
	{
		delta = 0f;
		PlayAnimation();
	}

	private void PlayAnimation()
	{
		if (avatarModel == null || avatarModel.AvatarId == IDSeg.InvalidId || avatarModel.CachedTransform.GetComponentInChildren<Animation>() == null)
			return;

		int selectCount = ConfigDatabase.DefaultCfg.ActionConfig.GetActionCountInType(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.SelectRole);
		int idleCount = ConfigDatabase.DefaultCfg.ActionConfig.GetActionCountInType(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Idle);

		var actionCfgSelected = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.SelectRole, Random.Range(0, selectCount));
		var actionCfgIdle = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Idle, Random.Range(0, idleCount));

		string idleAnimationName = actionCfgIdle.GetAnimationName(avatarModel.AvatarAssetId);
		string selectAnimationName = actionCfgSelected.GetAnimationName(avatarModel.AvatarAssetId);

		//播放点击后的动画
		if (actionCfgSelected != null && !avatarModel.transform.GetComponentInChildren<Animation>().IsPlaying(selectAnimationName))
		{
			avatarImage.StopVoice();
			avatarModel.PlayAnim(selectAnimationName);
			avatarImage.SetData(avatarModel.AvatarId, true, true, null);
		}

		//播放idle动画（还原）
		avatarModel.SetAnimationFinishDeletage(
			(e1, e2) =>
			{
				if (actionCfgIdle != null && !avatarModel.transform.GetComponentInChildren<Animation>().IsPlaying(idleAnimationName))
					avatarModel.PlayAnim(idleAnimationName);
			},
			idleAnimationName,
			selectAnimationName
			);
	}

	private void Update()
	{
		delta += Time.deltaTime;

		if (delta > 10.0f)
		{
			delta = 0f;
			PlayAnimation();
		}
		if (Input.GetMouseButtonDown(0))
		{
			SysUIEnv.Instance.ClearAllTip();
		}
	}

	#region Notify
	private void SetAvatarControlsNotify(int showAvatarLocationId)
	{
		int employIndexPos = PlayerDataUtility.GetIndexPosByBattlePos(CurrentPlayer.PositionData.GetPositionById(currentPositionId).EmployShowLocationId);
		// Set Avatar Control Notify.
		avatarPowerNotifyIcon.Hide(!CheckAvatarPowerNotify(currentPositionId, showAvatarLocationId));

		for (int index = 0; index < avatarIconList.Count; index++)
		{
			if (index == employIndexPos)
				continue;
			var item = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;
			var showLocationId = PlayerDataUtility.GetBattlePosByIndexPos(index > employIndexPos ? index - 1 : index);

			if (index <= employIndexPos + 1)
			{
				// Set Avatar Notify.
				if (showAvatarLocationId == showLocationId)
					item.SetNotify(false);
				else
					item.SetNotify(CheckAvatarControlNotify(currentPositionId, showLocationId));
			}
			else
			{
				if (showAvatarLocationId == avatarIconList.Count)
					item.SetNotify(false);
				else
					item.SetNotify(CheckPartnerNotify(currentPositionId));
			}
		}

		SetPartnerNotify(currentPositionId);
	}

	public static bool CheckUIAvatarNotify()
	{
		int maxLineUpAvatarsCount = ConfigDatabase.DefaultCfg.GameConfig.maxColumnInFormation * ConfigDatabase.DefaultCfg.GameConfig.maxRowInFormation;
		foreach (var position in ConfigDatabase.DefaultCfg.PositionConfig.Positions)
		{
			if (CurrentPlayer.PositionData.ActivePositionId != position.Id || CurrentPlayer.PositionData.GetPositionById(position.Id) == null)
				continue;

			for (int index = 0; index < maxLineUpAvatarsCount; index++)
			{
				if (CheckAvatarControlNotify(position.Id, PlayerDataUtility.GetBattlePosByIndexPos(index)))
					return true;
			}

			if (CheckPartnerNotify(position.Id))
				return true;
		}

		return false;
	}

	private static bool CheckPartnerNotify(int positionId)
	{
		var partners = ConfigDatabase.DefaultCfg.PartnerConfig.Partners;
		var dic_partners = GetPartnerAvatars(positionId);
		for (int i = 0; i < partners.Count; i++)
		{
			if (!partners[i].IsOpen)
				continue;
			int partnerId = partners[i].PartnerId;

			KodGames.ClientClass.Partner partner = null;
			if (dic_partners.ContainsKey(partnerId))
				partner = dic_partners[partnerId];

			if (partner == null)
			{
				if (CheckCostEnough(partnerId))
					return true;
			}
			else
			{
				if (string.IsNullOrEmpty(partner.AvatarGuid))
					return true;
				else
				{
					if (ItemInfoUtility.IsAbilityUpImprove_Avatar(CurrentPlayer.SearchAvatar(partner.AvatarGuid)))
						return true;
				}
			}
		}

		return false;
	}

	private static Dictionary<int, KodGames.ClientClass.Partner> GetPartnerAvatars(int positionId)
	{
		Dictionary<int, KodGames.ClientClass.Partner> dic_partners = new Dictionary<int, KodGames.ClientClass.Partner>();

		var partners = CurrentPlayer.PositionData.GetPositionById(positionId).Partners;
		for (int i = 0; i < partners.Count; i++)
		{
			if (dic_partners.ContainsKey(partners[i].PartnerId))
				continue;

			dic_partners.Add(partners[i].PartnerId, partners[i]);
		}

		return dic_partners;
	}

	private void SetPartnerNotify(UIElemLineUpAvatar cheerAvatar, bool isShow)
	{
		if (cheerAvatar != null)
			cheerAvatar.SetNotify(isShow);
	}

	private void SetPartnerNotify(int positionId)
	{
		for (int i = 0; i < cheerAvatarList.Count; i++)
		{
			UIElemAvatarBottomItem item = cheerAvatarList.GetItem(i).Data as UIElemAvatarBottomItem;
			int partnerId = (int)item.assetIcon.Data;
			KodGames.ClientClass.Partner partner = CurrentPlayer.PositionData.GetPositionById(positionId).GetPartnerById(partnerId);

			if (partner == null)
			{
				if (CheckCostEnough(partnerId))
					item.SetNotify(true);
				else
					item.SetNotify(false);
			}
			else
			{
				if (string.IsNullOrEmpty(partner.AvatarGuid))
					item.SetNotify(true);
				else
					item.SetNotify(ItemInfoUtility.IsAbilityUpImprove_Avatar(CurrentPlayer.SearchAvatar(partner.AvatarGuid)));
			}
		}
	}



	private static bool CheckAvatarControlNotify(int positionId, int showLocationId)
	{
		var hasNotify = CheckAvatarUsableOrPowerNotify(positionId, showLocationId);

		if (!hasNotify)
			hasNotify = CheckEquipUsableOrPowerNotify(positionId, showLocationId);

		if (!hasNotify)
			hasNotify = CheckSkillUsableNotify(positionId, showLocationId);

		if (!hasNotify)
			hasNotify = CheckDanUsableOrPowerNotify(positionId, showLocationId);
		return hasNotify;
	}

	private static bool CheckAvatarUsableOrPowerNotify(int positionId, int showLocationId)
	{
		var avatarLocation = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, positionId, showLocationId);

		if (avatarLocation == null)
		{
			var postionCfg = ConfigDatabase.DefaultCfg.PositionConfig.GetPositionById(positionId);
			if (CurrentPlayer.LevelAttrib.Level < postionCfg.PositionNums[PlayerDataUtility.GetIndexPosByBattlePos(showLocationId)].Level)
				return false;

			// Check AvatarUsable.
			if (showLocationId == CurrentPlayer.PositionData.GetPositionById(positionId).EmployShowLocationId)
			{
				foreach (var avatar in SysLocalDataBase.Inst.LocalPlayer.Avatars)
				{
					if (avatar.IsAvatar)
						continue;

					return true;
				}
			}
			else
			{
				var avatarIdsInPosition = PlayerDataUtility.GetLineUpAvatarIds(CurrentPlayer, positionId);

				foreach (var avatar in SysLocalDataBase.Inst.LocalPlayer.Avatars)
				{
					if (!avatar.IsAvatar)
						continue;

					if (!avatarIdsInPosition.Contains(avatar.ResourceId))
						return true;
				}
			}
		}

		return CheckAvatarPowerNotify(positionId, showLocationId);
	}

	private static bool CheckAvatarPowerNotify(int positionId, int showLocationId)
	{
		var avatarLocation = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, positionId, showLocationId);

		if (avatarLocation == null)
			return false;

		return ItemInfoUtility.IsAbilityUpImprove_Avatar(SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(avatarLocation.Guid));
	}

	private static bool CheckDanUsableOrPowerNotify(int positionId, int showLocationId)
	{
		for (int index = 0; index < C_EQUIP_COUNT; index++)
		{
			if (CheckDanUsableOrPowerNotify(positionId, showLocationId, DanConfig._DanType.Sky + index))
				return true;
		}

		return false;
	}

	private static bool CheckDanUsableOrPowerNotify(int positionId, int showLocationId, int type)
	{
		var avatarLocation = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, positionId, showLocationId);
		if (avatarLocation == null)
			return false;

		var dan = PlayerDataUtility.GetLineUpDanByType(CurrentPlayer, positionId, showLocationId, type);
		if (dan == null)
		{
			List<string> lineUpGuids = new List<string>();
			foreach (var danPos in CurrentPlayer.PositionData.GetPositionById(positionId).DanLocations)
				if (!lineUpGuids.Contains(danPos.Guid))
					lineUpGuids.Add(danPos.Guid);

			// Check Usable.
			foreach (var danPlay in SysLocalDataBase.Inst.LocalPlayer.Dans)
			{
				var danCfg = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(danPlay.ResourceId);
				if (danCfg.Type != type)
					continue;

				if (!lineUpGuids.Contains(danPlay.Guid))
					return true;
			}
		}

		// Check PowerUp.
		return ItemInfoUtility.IsAbilityUpImprove_Dan(dan);
	}

	private static bool CheckEquipUsableOrPowerNotify(int positionId, int showLocationId)
	{
		for (int index = 0; index < C_EQUIP_COUNT; index++)
		{
			if (CheckEquipUsableOrPowerNotify(positionId, showLocationId, EquipmentConfig._Type.Weapon + index))
				return true;
		}

		return false;
	}

	private static bool CheckEquipUsableOrPowerNotify(int positionId, int showLocationId, int type)
	{
		var avatarLocation = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, positionId, showLocationId);
		if (avatarLocation == null)
			return false;

		var equipment = PlayerDataUtility.GetLineUpEquipmentByType(CurrentPlayer, positionId, showLocationId, type);
		if (equipment == null)
		{
			List<string> lineUpGuids = new List<string>();
			foreach (var equip in CurrentPlayer.PositionData.GetPositionById(positionId).EquipLocations)
				if (!lineUpGuids.Contains(equip.Guid))
					lineUpGuids.Add(equip.Guid);

			// Check Usable.
			foreach (var equip in SysLocalDataBase.Inst.LocalPlayer.Equipments)
			{
				var equipCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equip.ResourceId);
				if (equipCfg.type != type)
					continue;

				if (!lineUpGuids.Contains(equip.Guid))
					return true;
			}
		}

		// Check PowerUp.
		return ItemInfoUtility.IsAbilityUpImprove_Equip(equipment);
	}

	private static bool CheckCostEnough(int partnerId)
	{
		var partnerCfg = ConfigDatabase.DefaultCfg.PartnerConfig.GetPartnerById(partnerId);
		if (partnerCfg.RequirePlayerLevel <= CurrentPlayer.LevelAttrib.Level &&
			partnerCfg.RequireVipLevel <= CurrentPlayer.VipLevel)
		{
			foreach (var cost in partnerCfg.Costs)
			{
				if (!ItemInfoUtility.CheckCostEnough(cost.id, cost.count))
					return false;
			}
			return true;
		}
		else
			return false;
	}

	private static bool CheckSkillUsableNotify(int positionId, int showLocationId)
	{
		var avatarLocation = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, positionId, showLocationId);

		if (avatarLocation == null)
			return false;

		var skillIdsInPosition = PlayerDataUtility.GetLineUpSkillIds(CurrentPlayer, positionId, showLocationId);
		if (skillIdsInPosition.Count == C_EQUIP_COUNT)
			return false;

		List<string> lineUpGuids = new List<string>();
		foreach (var skill in CurrentPlayer.PositionData.GetPositionById(positionId).SkillLocations)
			if (!lineUpGuids.Contains(skill.Guid))
				lineUpGuids.Add(skill.Guid);

		foreach (var skill in SysLocalDataBase.Inst.LocalPlayer.Skills)
		{
			var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);
			if (skillCfg == null || skillCfg.type != CombatTurn._Type.PassiveSkill)
				continue;

			if (!skillIdsInPosition.Contains(skill.ResourceId) && !lineUpGuids.Contains(skill.Guid))
				return true;
		}
		return false;
	}

	private void SetMenuBottomIconNotify()
	{
		// Update UIPnlMainMenuBot.
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlMainMenuBot)))
			SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().UpdateAvatarLineUpNotify();
	}

	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBack(UIButton btn)
	{
		this.HideSelf();
	}

	#region//战力提示

	private void ShowPower()
	{
		//对战力变化进行tips提示
		if (isShowPowerTips)
		{
			int value = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, currentPositionId);

			if (value > changPower)
			{
				if (isSetMaster)
					tipMsgs.Add(GameUtility.FormatUIString("UITipsPower_PositionUp", value - changPower));
				else
					tipMsgs.Add(GameUtility.FormatUIString("UITipsPower_Up", value - changPower));
			}
			else if (value < changPower)
			{
				if (isSetMaster)
					tipMsgs.Add(GameUtility.FormatUIString("UITipsPower_PositionDown", changPower - value));
				else
					tipMsgs.Add(GameUtility.FormatUIString("UITipsPower_Down", changPower - value));
			}

			isSetMaster = false;
			changPower = value;
			isShowPowerTips = false;
		}

		ShowTips();
	}

	//设置移除Overlay时是否要覆盖缓存战力
	public void SetIsOverlaySetPower()
	{
		this.isOverlaySetPower = true;
	}

	#endregion
}