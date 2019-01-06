using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIPnlEquipmentInfo : UIPnlItemInfoBase
{
	public delegate void SelectDelegate(KodGames.ClientClass.Equipment selected);

	public SpriteText titleLabel;

	public UIElemAssetIcon equipIcon;
	public SpriteText equipNameLabel;
	public SpriteText equipLevelLabel;
	public UIBox equipType;
	public UIElemBreakThroughBtn equipBreakLvlBtn;

	public GameObject attrRoot1;
	public GameObject attrRoot2;
	public SpriteText[] equipAttributes;

	public SpriteText equipDescLabel;
	public AutoSpriteControlBase equipAssembleBox;
	public UIScrollList equipAssembleList;
	public SpriteText equipAssembleLabel;

	//public UIElemProgressItem qualityStar;

	public UIScrollList suiteList;
	public SpriteText suiteLabel;

	public UIChildLayoutControl actionButtonLayout;
	public UIButton changeBtn;
	public UIButton bigCloseBtn;
	public UIButton powerUpBtn;
	public UIButton gotoPackageBtn;
	public UIButton selectBtn;
	public UIButton prevBtn;
	public UIButton nextBtn;

	//能一件满级提示
	public UIBox promptLeveUp;

	// Local data.
	private Location location;
	private Equipment equipData;
	private Player currentPlayer;
	private SelectDelegate selectDel;
	private bool showSuitProcess;
	private bool isScroll;
	private bool powerUp;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		currentPlayer = SysLocalDataBase.Inst.LocalPlayer;
		powerUp = false;

		if (userDatas[0] is EquipmentConfig.Equipment)
		{
			EquipmentConfig.Equipment equipmentCfg = userDatas[0] as EquipmentConfig.Equipment;
			bool isCardPic = userDatas.Length > 1 ? (bool)userDatas[1] : false;

			List<EquipmentConfig.Equipment> cardPicEquipments = GetCardPictureEquipments();
			int index = cardPicEquipments.IndexOf(equipmentCfg);

			SetCardPicEquipmentUI(equipmentCfg, index, cardPicEquipments.Count, isCardPic);
		}
		else if (userDatas[0] is KodGames.ClientClass.Equipment)
		{
			equipData = userDatas[0] as KodGames.ClientClass.Equipment;
			bool showChange = (bool)userDatas[1];
			bool showBigClose = (bool)userDatas[2];
			bool showGotoPackage = (bool)userDatas[3];
			powerUp = (bool)userDatas[4];
			selectDel = userDatas[5] as SelectDelegate;
			showSuitProcess = (bool)userDatas[6];

			// Show action button
			ShowActionButtons(showChange, showBigClose, showGotoPackage, powerUp, selectDel != null, false, false);
			FillData();
		}
		else if (userDatas[0] is KodGames.ClientClass.Location)
		{
			this.location = userDatas[0] as KodGames.ClientClass.Location;
			bool showChange = (bool)userDatas[1];
			bool showBigClose = (bool)userDatas[2];
			bool showGotoPackage = (bool)userDatas[3];
			powerUp = (bool)userDatas[4];
			selectDel = userDatas[5] as SelectDelegate;
			showSuitProcess = (bool)userDatas[6];

			if (userDatas.Length > 7)
				currentPlayer = userDatas[7] as Player;

			this.equipData = currentPlayer.SearchEquipment(location.Guid);

			// Show action button
			ShowActionButtons(showChange, showBigClose, showGotoPackage, powerUp, selectDel != null, false, false);
			FillData();
		}
		else if (userDatas[0] is int)
		{

			int equipResrouceId;
			if (IDSeg.ToAssetType((int)userDatas[0]) != IDSeg._AssetType.Equipment)
			{
				this.isScroll = true;
				equipResrouceId = ConfigDatabase.DefaultCfg.IllustrationConfig.GetIllustrationByFragmentId((int)userDatas[0]).Id;
			}

			else
				equipResrouceId = (int)userDatas[0];
			var equipCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipResrouceId);
			if (equipCfg == null)
				return true;

			SetCardPicEquipmentUI(equipCfg, 0, 0, false);
		}

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		equipData = null;
		selectDel = null;
		location = null;
		currentPlayer = null;
		isScroll = false;
		showSuitProcess = false;
	}

	private void ShowActionButtons(bool showChange, bool showBigClose, bool showGotoPackage, bool showPowerUp, bool showSelect, bool prev, bool next)
	{
		actionButtonLayout.HideChildObj(changeBtn.gameObject, !showChange);
		actionButtonLayout.HideChildObj(bigCloseBtn.gameObject, !showBigClose);
		actionButtonLayout.HideChildObj(powerUpBtn.gameObject, !showPowerUp);
		actionButtonLayout.HideChildObj(selectBtn.gameObject, !showSelect);
		actionButtonLayout.HideChildObj(gotoPackageBtn.gameObject, !showGotoPackage);
		actionButtonLayout.HideChildObj(prevBtn.gameObject, !prev);
		actionButtonLayout.HideChildObj(nextBtn.gameObject, !next);
	}

	private void FillData()
	{
		EquipmentConfig.Equipment equipmentCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipData.ResourceId);
		if (equipmentCfg == null)
			return;

		// Set Title.
		if (isScroll)
			titleLabel.Text = GameUtility.GetUIString("UIDlgEquipmentInfo_ScrollTitle");
		else
			titleLabel.Text = GameUtility.GetUIString("UIDlgEquipmentInfo_Title");

		// Name
		if (isScroll)
			equipNameLabel.Text = ItemInfoUtility.GetAssetName(ConfigDatabase.DefaultCfg.IllustrationConfig.GetIllustrationById(equipmentCfg.id).FragmentId);
		else
			equipNameLabel.Text = ItemInfoUtility.GetAssetName(equipmentCfg.id);

		// Type
		UIElemTemplate.Inst.SetEquipTypeIcon(equipType, equipmentCfg.type);

		// Breakthrough
		equipBreakLvlBtn.SetBreakThroughIcon(equipData.BreakthoughtLevel);

		//EquipIcon
		equipIcon.SetData(equipData.ResourceId);

		// Level
		equipLevelLabel.Text = GameUtility.FormatUIString(
												 "UIPnlAvatar_AvatarLevel",
												 equipData.LevelAttrib.Level,
												 GameDefines.textColorInOrgYew.ToString(),
												 equipmentCfg.GetBreakthroughByTimes(equipData.BreakthoughtLevel).breakThrough.powerUpLevelLimit);

		// Set Equip Attribute
		for (int index = 0; index < equipAttributes.Length; index++)
			equipAttributes[index].Text = string.Empty;

		var attributes = PlayerDataUtility.GetEquipmentAttributes(equipData);
		for (int i = 0; i < attributes.Count && i < equipAttributes.Length; i++)
		{
			var equipAttributeMax = attributes[i];
			equipAttributes[i].Text = ItemInfoUtility.GetAttributeNameValueString(equipAttributeMax.type, equipAttributeMax.value, GameDefines.textColorBtnYellow, GameDefines.textColorWhite);
		}

		attrRoot1.SetActive(!string.IsNullOrEmpty(equipAttributes[0].Text));
		attrRoot2.SetActive(!string.IsNullOrEmpty(equipAttributes[2].Text));

		// Description
		equipDescLabel.Text = ItemInfoUtility.GetAssetExtraDesc(equipData.ResourceId);

		// Assemble.
		bool noAssemble = string.IsNullOrEmpty(equipmentCfg.activeableAssembleDesc);
		equipAssembleBox.Hide(noAssemble);
		if (noAssemble)
			equipAssembleLabel.Text = string.Empty;
		else
		{
			equipAssembleLabel.Text = equipmentCfg.activeableAssembleDesc;
			equipAssembleList.PositionItems();
			equipAssembleList.ScrollPosition = 0f;
		}

		suiteLabel.Text = ItemInfoUtility.GetSuiteDesc(currentPlayer, equipmentCfg.id, this.location, this.showSuitProcess) +
							  ItemInfoUtility.GetAssetDesc(equipmentCfg.id);
		suiteList.RepositionItems();
		suiteList.ScrollListTo(0f);


		//设置是否可以一件满级提示
		int costMaxCount = 0;
		for (int index = 0; index < ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipData.ResourceId).GetBreakthroughByTimes(equipData.BreakthoughtLevel).breakThrough.powerUpLevelLimit -
									equipData.LevelAttrib.Level; index++)
		{
			foreach (var cost in ConfigDatabase.DefaultCfg.EquipmentConfig.GetQualityCostByLevelAndQuality(equipData.LevelAttrib.Level + index,
									ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipData.ResourceId).qualityLevel).costs)
			{
				costMaxCount += cost.count;
			}
		}

		promptLeveUp.Hide(!ItemInfoUtility.IsAbilityUpImprove_Equip(equipData));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPowerUp(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlEquipmentLevelup, equipData);
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChange(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlSelectEquipmentList, ItemInfoUtility.GetEquipmentType(equipData.ResourceId), location);
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoPachage(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlPackageEquipTab));
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelect(UIButton btn)
	{
		if (selectDel != null)
			selectDel(equipData);

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCardPicturePrevClick(UIButton btn)
	{
		List<int> cardPictures = GetCardPictureIDs();
		int index = cardPictures.IndexOf(equipData.ResourceId);
		int prevIndex = Mathf.Max(0, index - 1);

		if (index == prevIndex)
			return;

		SetCardPicEquipmentUI(prevIndex, cardPictures.Count, true);
	}

	//获得途径
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetWay(UIButton btn)
	{
		if (this.ShowLayer != _UILayer.Top)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgItemGetWay), equipData.ResourceId);
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgItemGetWay), _UILayer.Top, equipData.ResourceId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCardPictureNextClick(UIButton btn)
	{
		List<int> cardPictures = GetCardPictureIDs();
		int index = cardPictures.IndexOf(equipData.ResourceId);
		int nextIndex = Mathf.Min(cardPictures.Count - 1, index + 1);

		if (index == nextIndex)
			return;

		SetCardPicEquipmentUI(nextIndex, cardPictures.Count, true);
	}

	private void SetCardPicEquipmentUI(int index, int count, bool isCardPic)
	{
		List<int> cardPictures = GetCardPictureIDs();
		SetCardPicEquipmentUI(ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(cardPictures[index]), index, count, isCardPic);
	}

	private void SetCardPicEquipmentUI(EquipmentConfig.Equipment equipCfg, int index, int count, bool isCardPic)
	{
		// Construct a fake equipment for filling data
		equipData = new Equipment();
		equipData.ResourceId = equipCfg.id;
		equipData.LevelAttrib.Level = 1;
		equipData.BreakthoughtLevel = 0;

		// Disable selecting
		selectDel = null;

		// Not show suit process.
		showSuitProcess = false;

		if (isCardPic)
			// Show action button, only show "Close"
			ShowActionButtons(false, false, false, false, false, index != 0, index != (count - 1));
		else
			ShowActionButtons(false, true, false, false, false, false, false);

		FillData();
	}

	private List<int> GetCardPictureIDs()
	{
		List<int> cardPictures = new List<int>();
		foreach (EquipmentConfig.Equipment equip in ConfigDatabase.DefaultCfg.EquipmentConfig.equipments)
			cardPictures.Add(equip.id);

		cardPictures.Sort(DataCompare.CompareEquipment);

		return cardPictures;
	}

	private List<EquipmentConfig.Equipment> GetCardPictureEquipments()
	{
		List<EquipmentConfig.Equipment> cardPictures = new List<EquipmentConfig.Equipment>();
		foreach (EquipmentConfig.Equipment equipment in ConfigDatabase.DefaultCfg.EquipmentConfig.equipments)
			cardPictures.Add(equipment);

		cardPictures.Sort(DataCompare.CompareEquipment);

		return cardPictures;
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMenuBot(UIButton btn)
	{
	}
}