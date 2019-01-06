using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIDlgOrganChipSplit : UIModule
{
	public SpriteText chipName;

	public GameObjectPool chipItemPool;
	public UIScrollList chipsList;
	public UIScrollList attributeList;
	public GameObjectPool attributePool;

	public UIElemAssetBeastEquips beastEquips;

	public AutoSpriteControlBase[] selectBgs;
	public UIBox noGetWayBg;

	public SpriteText splitLabel;
	public SpriteText partPower;

	public UIButton okButton;
	public UIButton upButton;

	private int chipId;
	private KodGames.ClientClass.Beast playerBeast;
	private BeastConfig.BreakthoughtAndLevel beastBreakAndLevelCfg;
	private int defaultPart = BeastConfig._PartIndex.One;

	private List<GameObject> levelEffects = new List<GameObject>();

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		this.playerBeast = userDatas[0] as KodGames.ClientClass.Beast;

		if (userDatas.Length > 1)
			defaultPart = (int)userDatas[1];
		else
			defaultPart = BeastConfig._PartIndex.One;

		selectBgs[0].Data = BeastConfig._PartIndex.One;
		selectBgs[1].Data = BeastConfig._PartIndex.Two;
		selectBgs[2].Data = BeastConfig._PartIndex.Three;
		selectBgs[3].Data = BeastConfig._PartIndex.Four;
		selectBgs[4].Data = BeastConfig._PartIndex.Five;
	
		FillChipInfo();

		return true;
	}

	public override void OnHide()
	{
		foreach (GameObject particle in levelEffects)
		{
			GameObject.Destroy(particle);
		}

		levelEffects.Clear();

		base.OnHide();
	}

	// Set chip info.
	private void FillChipInfo()
	{
		beastEquips.SetData(playerBeast);
		beastBreakAndLevelCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(playerBeast.ResourceId, playerBeast.BreakthoughtLevel, playerBeast.LevelAttrib.Level);
		this.chipId = beastBreakAndLevelCfg.BeastPartActives[defaultPart - 1].PartCost.id;

		chipName.Text = ItemInfoUtility.GetAssetName(chipId);
		int chipPower = beastBreakAndLevelCfg.BeastPartActives[defaultPart - 1].PartPower;
		partPower.Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_Quality_BattleNumber"), chipPower.ToString());

		bool isEquip = false;
		for (int i = 0; i < playerBeast.PartIndexs.Count; i++)
		{
			if (defaultPart == playerBeast.PartIndexs[i])
				isEquip = true;
		}

		int playerHaveCount = 0;
		var consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(chipId);
		if (consumable != null)
			playerHaveCount = consumable.Amount;

		int needCount = beastBreakAndLevelCfg.BeastPartActives[defaultPart - 1].PartCost.count;

		if (isEquip)
			splitLabel.Text = string.Format(GameUtility.GetUIString("UIDlgOrganChipSplit_Split_Label"), GameDefines.txColorGreen);
		else
			splitLabel.Text = string.Format(GameUtility.GetUIString("UIDlgOrganChipSplit_Chip_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, playerHaveCount, needCount);

		upButton.Hide(isEquip);
		okButton.Hide(!isEquip);

		FillGetWayData();
		SelectChange();
	}

	public void FillGetWayData()
	{
		ClearData();
		StartCoroutine("FillList");
	}	

	public void ClearData()
	{
		StopCoroutine("FillList");
		chipsList.ClearList(false);
		chipsList.ScrollPosition = 0f;

		attributeList.ClearList(false);
		attributeList.ScrollPosition = 0f;		
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		int attributeId = beastBreakAndLevelCfg.BeastPartActives[defaultPart - 1].AttributeId;
		var beastPartAttrCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastPartAttributeGroupById(attributeId);

		List<ClientServerCommon.Attribute> partAttributes = new List<ClientServerCommon.Attribute>();
		partAttributes.AddRange(beastPartAttrCfg.Attributes);
		PlayerDataUtility.MergeClientServerAttributes(ref partAttributes, true);

		for (int i = 0; i < partAttributes.Count; i++)
		{
			AttributeCalculator.Attribute attr1 = new AttributeCalculator.Attribute(0, 0);
			AttributeCalculator.Attribute attr2 = new AttributeCalculator.Attribute(0, 0);

			attr1.type = partAttributes[i].type;
			attr1.value = partAttributes[i].value;
			i++;

			if (i < partAttributes.Count)
			{
				attr2.type = partAttributes[i].type;
				attr2.value = partAttributes[i].value;
			}

			UIElemOrganAttribute attribute = attributePool.AllocateItem().GetComponent<UIElemOrganAttribute>();
			attribute.SetDataWithAdd(attr1, attr2);
			attributeList.AddItem(attribute.gameObject);
		}

		BeastConfig.BeastPart beastChip = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastPartByBeastPartId(chipId);

		foreach (var getway in beastChip.GetWays)
		{
			if(getway != null && getway.type != _UIType.UnKonw)
			{
				UIElemBeastGotoItem item = chipItemPool.AllocateItem().GetComponent<UIElemBeastGotoItem>();
				item.SetData(getway);
				chipsList.AddItem(item.gameObject);
			}			
		}

		if (beastChip.GetWays.Count > 0)
			noGetWayBg.gameObject.SetActive(false);
		else
			noGetWayBg.gameObject.SetActive(true);
	}

	private void SelectChange()
	{
		for (int i = 0; i < selectBgs.Length; i++)
		{
			if ((int)selectBgs[i].Data == defaultPart)
				selectBgs[i].gameObject.SetActive(true);
			else
				selectBgs[i].gameObject.SetActive(false);
		}
	}

	public void OnEquipSplitSuccess(KodGames.ClientClass.Beast beast, int type)
	{		
		if (beastEquips.equipIcons.Length >= type)
		{
			GameObject levelUpEff = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.equipPart));
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(beastEquips.equipIcons[type - 1].gameObject, levelUpEff);
			levelEffects.Add(levelUpEff);
			AudioManager.Instance.PlaySound(GameDefines.meridianOpen, 0f);
		}

		this.playerBeast = beast;
		FillChipInfo();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChipBtn(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		defaultPart = (int)assetIcon.Data;

		FillChipInfo();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEquipChip(UIButton btn)
	{
		
		int playerHaveCount = 0;
		var consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(chipId);
		if (consumable != null)
			playerHaveCount = consumable.Amount;

		int needCount = beastBreakAndLevelCfg.BeastPartActives[defaultPart - 1].PartCost.count;
		if(needCount > playerHaveCount)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgOrganChipSplit_Chip_NotEnought"));
		}
		else
			RequestMgr.Inst.Request(new EquipBeastPartReq(playerBeast.Guid, defaultPart));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoBtn(UIButton btn)
	{
		ClientServerCommon.GetWay getway = btn.Data as ClientServerCommon.GetWay;

		if (getway.type != _UIType.UI_ActivityDungeon && getway.type != _UIType.UI_Dungeon)
			GameUtility.JumpUIPanel(getway.type);
		else
			GameUtility.JumpUIPanel(getway.type, getway.data);
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMenuBot(UIButton btn)
	{
		Debug.Log("OnClickMenuBot  " + "UIPnlchipInfo");
	}
}