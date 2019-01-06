using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using MathFactory;

public class UIPnlEquipmentLevelup : UIModule
{
	public UIElemAssetIcon equipIcon;
	public UIElemBreakThroughBtn breakThroughBtn;

	// 装备基本信息UI
	public SpriteText equipNameLabel;
	public SpriteText equipLevelLabel;

	// 装备基本属性UI
	public UIBox[] attribBorders;
	public SpriteText[] attribTypes;
	public SpriteText[] attribValues;

	//	Equipment Level Up Attribute 
	public GameObject[] levelUpAttribBorders;
	public SpriteText[] levelUpAttribLables;

	// 升级相关UI
	public GameObject levelUpActionRoot;

	public UIButton strengthMaxBtn;
	public UIElemAssetIcon strengthMaxCostIcon;
	public SpriteText strengthMaxCost;

	public UIButton strengthBtn;
	public UIElemAssetIcon strengthCostIcon;
	public SpriteText strengthCost;

	public SpriteText equipMaxLevelLabel;

	//能一件满级提示
	public UIBox promptLeveUp;

	private KodGames.ClientClass.Equipment equipment;

	private float equipmentPower;
	private float positionPower;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		equipmentPower = 0f;
		positionPower = 0f;

		equipment = userDatas[0] as KodGames.ClientClass.Equipment;
		SysUIEnv.Instance.GetUIModule<UIPnlEquipmentPowerUpTab>().ChangeTabButtons(_UIType.UIPnlEquipmentLevelup, equipment);

		InitCommonView();

		return true;
	}

	// Set the common view 
	private void InitCommonView()
	{
		// Common view : equip_name, equip_desc,equip_breakLevel,equi_quality,equip icon ,equip_attr_Icon,cost icon.

		// Set equip 2D icon.
		equipIcon.SetData(equipment.ResourceId);

		// Set equip_name.
		equipNameLabel.Text = ItemInfoUtility.GetAssetName(equipment.ResourceId);

		// Set equip level 
		equipLevelLabel.Text = string.Format(GameUtility.GetUIString("UIPnlEquip_Level_Info2"), GameDefines.textColorBtnYellow.ToString(),
			equipment.LevelAttrib.Level, GameDefines.textColorInOrgYew.ToString(),
			ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetBreakthroughByTimes(equipment.BreakthoughtLevel).breakThrough.powerUpLevelLimit);

		// Set Quality Star
		breakThroughBtn.SetBreakThroughIcon(equipment.BreakthoughtLevel);

		// 设置武器基本属性
		var baseAttribs = PlayerDataUtility.GetEquipmentAttributes(equipment);
		for (int i = 0; i < attribBorders.Length; ++i)
		{
			if (i < baseAttribs.Count)
			{
				attribBorders[i].gameObject.SetActive(true);
				attribTypes[i].Text = _AvatarAttributeType.GetDisplayNameByType(baseAttribs[i].type, ConfigDatabase.DefaultCfg);
				attribValues[i].Text = ItemInfoUtility.GetAttribDisplayString(baseAttribs[i]);
			}
			else
				attribBorders[i].gameObject.SetActive(false);
		}

		// 获取下一级武器提升的属性
		var levelUpAttribs = new List<AttributeCalculator.Attribute>();
		if (equipment.LevelAttrib.Level < ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetBreakthroughByTimes(equipment.BreakthoughtLevel).breakThrough.powerUpLevelLimit)
		{
			KodGames.ClientClass.Equipment nextLevelEquipment = new KodGames.ClientClass.Equipment();
			nextLevelEquipment.ResourceId = equipment.ResourceId;
			nextLevelEquipment.LevelAttrib.Level = equipment.LevelAttrib.Level + 1;
			nextLevelEquipment.BreakthoughtLevel = equipment.BreakthoughtLevel;
			var nextAttribs = PlayerDataUtility.GetEquipmentAttributes(nextLevelEquipment);

			foreach (var baseAttrib in baseAttribs)
				foreach (var nextAttrib in nextAttribs)
					if (baseAttrib.type == nextAttrib.type)
						levelUpAttribs.Add(new AttributeCalculator.Attribute(baseAttrib.type, nextAttrib.value - baseAttrib.value));

			// 可以升级, 显示升级按就和消耗
			levelUpActionRoot.gameObject.SetActive(true);
			equipMaxLevelLabel.gameObject.SetActive(false);

			// 显示升级一级消耗值
			int costId = IDSeg.InvalidId;
			int costCount = 0;
			foreach (var cost in ConfigDatabase.DefaultCfg.EquipmentConfig.GetQualityCostByLevelAndQuality(equipment.LevelAttrib.Level, ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).qualityLevel).costs)
			{
				costId = cost.id;
				costCount += cost.count;
			}
			strengthCostIcon.SetData(costId);
			strengthCost.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob3"),
										SysLocalDataBase.Inst.LocalPlayer.GameMoney >= costCount ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
										costCount);

			// 显示升级满级消耗
			int costMaxId = IDSeg.InvalidId;
			int costMaxCount = 0;
			for (int index = 0; index < ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetBreakthroughByTimes(equipment.BreakthoughtLevel).breakThrough.powerUpLevelLimit - equipment.LevelAttrib.Level; index++)
			{
				foreach (var cost in ConfigDatabase.DefaultCfg.EquipmentConfig.GetQualityCostByLevelAndQuality(equipment.LevelAttrib.Level + index, ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).qualityLevel).costs)
				{
					costMaxId = cost.id;
					costMaxCount += cost.count;
				}
			}
			strengthMaxCostIcon.SetData(costMaxId);
			strengthMaxCost.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob3"),
										SysLocalDataBase.Inst.LocalPlayer.GameMoney >= costMaxCount ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
										costMaxCount);


			promptLeveUp.Hide(!ItemInfoUtility.IsLevelNotifyActivity_Equip(equipment));
		}
		else
		{
			foreach (var baseAttrib in baseAttribs)
				levelUpAttribs.Add(new AttributeCalculator.Attribute(baseAttrib.type, 0));

			// 达到最大级, 显示提示
			levelUpActionRoot.gameObject.SetActive(false);
			equipMaxLevelLabel.gameObject.SetActive(true);
			equipMaxLevelLabel.Text = GameUtility.GetUIString("UIPnlEquipmentLevelUp_LvlUpMax");
		}

		// 设置底板UI
		for (int i = 0; i < levelUpAttribBorders.Length; ++i)
		{
			levelUpAttribBorders[i].SetActive(i < Mathf.CeilToInt(levelUpAttribs.Count / 2.0f));
		}

		// 设置提升的属性UI
		for (int i = 0; i < levelUpAttribLables.Length; ++i)
		{
			if (i < levelUpAttribs.Count)
			{
				var levelUpAttrib = levelUpAttribs[i];

				// 属性
				levelUpAttribLables[i].gameObject.SetActive(true);
				levelUpAttribLables[i].Text = GameUtility.FormatUIString("UIAttributeTextWithColor",
					GameDefines.textColorBtnYellow,
					_AvatarAttributeType.GetDisplayNameByType(levelUpAttrib.type, ConfigDatabase.DefaultCfg),
					GameDefines.textColorWhite,
					ItemInfoUtility.GetAttribDisplayString(levelUpAttrib.type, levelUpAttrib.value));
			}
			else
			{
				levelUpAttribLables[i].gameObject.SetActive(false);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickStrengthEquip(UIButton btn)
	{
		CalculatePower();
		RequestMgr.Inst.Request(new EquipmentLevelUpReq(equipment.Guid, true));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickStengthEquipMax(UIButton btn)
	{
		CalculatePower();
		RequestMgr.Inst.Request(new EquipmentLevelUpReq(equipment.Guid, false));
	}

	private void CalculatePower()
	{
		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, equipment.Guid, equipment.ResourceId))
			positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		equipmentPower = ConfigDatabase.DefaultCfg.EquipmentConfig.GetOneEquipmentBasePower(equipment.ResourceId, equipment.LevelAttrib.Level, equipment.BreakthoughtLevel);
	}

	//*****************
	//升级协议返回【已经对该书籍进行了修改，所以需要在升级前对书籍进行一次临时保存】
	//如果该书籍在主力阵容里面，那么在对书籍修改前还需要对主力阵容的战力进行一次保存
	//******************
	public void OnReponseEquipLevelUpSuccess(bool isNormalPowerUp, int levelBefore, int critCount, KodGames.ClientClass.Cost cost)
	{
		// Set Del.
		SysUIEnv.Instance.GetUIModule<UIEffectPowerUp>().SetEffectHideCallback(
			(data) =>
			{
				string successMsg = string.Empty;
				if (isNormalPowerUp)
					successMsg = GameUtility.FormatUIString("UIPnlEquipmentLevelUp_LvlUpLabel", equipment.LevelAttrib.Level);
				else
				{
					int needCostCount = 0;

					for (int index = 0; index < equipment.LevelAttrib.Level - levelBefore; index++)
					{
						foreach (var quickCost in ConfigDatabase.DefaultCfg.EquipmentConfig.GetQualityCostByLevelAndQuality(levelBefore + index, ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).qualityLevel).costs)
						{
							needCostCount += quickCost.count;
						}
					}
					successMsg = GameUtility.FormatUIString("UIEquipment_TopPowerUp_Result", levelBefore, equipment.LevelAttrib.Level, cost.Count, critCount, needCostCount - cost.Count);
				}

				//show currentLevel's properties and preLevel's properties
				AvatarAttributeUpdateDetail(levelBefore, equipment.LevelAttrib.Level);

				// Reset UI.
				InitCommonView();

				// Show Tips.
				SysUIEnv.Instance.AddTip(successMsg);
				float tempEquipmentPower = ConfigDatabase.DefaultCfg.EquipmentConfig.GetOneEquipmentBasePower(equipment.ResourceId, equipment.LevelAttrib.Level, equipment.BreakthoughtLevel);
				if (tempEquipmentPower > equipmentPower)
					SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(tempEquipmentPower - equipmentPower)));
				else if (tempEquipmentPower < equipmentPower)
					SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneDown", (int)(equipmentPower - tempEquipmentPower)));

				if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, equipment.Guid, equipment.ResourceId))
				{
					float tempPositionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
					if (tempPositionPower > positionPower)
						SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionUp", (int)(tempPositionPower - positionPower)));
					else if (tempPositionPower < positionPower)
						SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionDown", (int)(positionPower - tempPositionPower)));
				}
			});

		//Show effect
		SysUIEnv.Instance.ShowUIModule(typeof(UIEffectPowerUp), equipment.ResourceId, critCount > 0 ? UIEffectPowerUp.LabelType.Crit : UIEffectPowerUp.LabelType.Success, isNormalPowerUp);
	}

	private void AvatarAttributeUpdateDetail(int levelOne, int leveltwo)
	{
		//Avatar详细信息
		UIPnlAvatarAttributeUpdateDetail.ChangeData data0 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
		data0.Level = levelOne;
		data0.BreakthoughtLevel = equipment.BreakthoughtLevel;
		data0.ResourceId = equipment.ResourceId;

		UIPnlAvatarAttributeUpdateDetail.ChangeData data1 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
		data1.Level = leveltwo;
		data1.BreakthoughtLevel = equipment.BreakthoughtLevel;
		data1.ResourceId = equipment.ResourceId;

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarAttributeUpdateDetail), false, UIPnlAvatarAttributeUpdateDetail._UIShowType.EquipmentUpgrate, data0, data1);
	}
}
