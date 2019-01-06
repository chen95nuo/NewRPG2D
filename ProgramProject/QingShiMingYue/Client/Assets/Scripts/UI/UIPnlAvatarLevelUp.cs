using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlAvatarLevelUp : UIModule
{
	public GameObject buttonRoot;//升级按钮
	public GameObject costRoot; //消耗
	public SpriteText maxLevelTips;//满级提示
	public UIBox propertyUpTitle;

	public UIElemAssetIcon totalCostIcon; //一键满级消耗Icon
	public UIElemAssetIcon onceCostIcon;//一次升级消耗Icon
	public SpriteText totalCostCount; //一键满级消耗Count
	public SpriteText onceCostCount;//一次升级消耗Count

	public UIElemBreakThroughBtn breakThroughBtn;//突破按钮

	public SpriteText avatarHPLabel;//当前血量
	public SpriteText avatarATKLabel;//当前攻击
	public SpriteText avatarSPDLabel;//当前速度
	public SpriteText avatarLVLabel;//当前等级

	public SpriteText hpCount; //可提升血量
	public SpriteText papCount;//可提升物攻
	public SpriteText speedCount;//可提升速度

	public UIBox AvatarGrowthBtn;

	//Skill
	public UIScrollList SkillList;//主动技能列表
	public GameObjectPool SkillIconPool;//主动技能池

	//国家图片
	public UIElemAvatarCard avatarCard;
	public UIBox countryImage;

	public UIBox activityNotify_Total;
	//public UIBox activityNotify_one;

	private int costCount;
	private KodGames.ClientClass.Avatar avatarLocalData;

	private float avatarPower;
	private float positionPower;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		avatarPower = 0f;
		positionPower = 0f;

		avatarLocalData = userDatas[0] as KodGames.ClientClass.Avatar;
		SysUIEnv.Instance.GetUIModule<UIPnlAvatarPowerUpTab>().UpdateTabStatus(_UIType.UIPnlAvatarLevelUp, avatarLocalData);

		ShowUI();

		return true;
	}

	private void ShowUI()
	{
		//判断是否已经满级
		int currentLevel = avatarLocalData.LevelAttrib.Level;
		int breakThroughLevel = avatarLocalData.BreakthoughtLevel;
		AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
		int currentMaxLevel = avatarConfig.GetAvatarBreakthrough(breakThroughLevel).breakThrough.powerUpLevelLimit;

		propertyUpTitle.Text = GameUtility.FormatUIString("UIPnlEquipmentLevelUp_OneLevel", GameDefines.textColorBtnYellow, GameDefines.textColorWhite);

		maxLevelTips.Text = GameUtility.GetUIString("UIPnlAvatarLevelUpTab_LvlUpMax");

		if (currentLevel == currentMaxLevel)//达到最大等级
		{
			maxLevelTips.Hide(false);
			buttonRoot.SetActive(false);
			costRoot.SetActive(false);

			//满级后 将属性栏的数字全部置为0
			hpCount.Text = "0";
			papCount.Text = "0";
			speedCount.Text = "0";
		}
		else
		{
			maxLevelTips.Hide(true);
			buttonRoot.SetActive(true);
			costRoot.SetActive(true);

			SetUpgradeUI();
			SetCostUI();
		}

		FillData();
	}

	private void FillData()
	{
		ClearList();

		//加载界面数据
		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
		if (avatarCfg == null)
		{
			Debug.LogWarning("Lose Avatar config with Id : " + avatarLocalData.ResourceId.ToString("X"));
			return;
		}

		// Set AvatarCard.
		avatarCard.SetData(avatarCfg.id, false, false, null);

		// Set breakthrough
		breakThroughBtn.SetBreakThroughIcon(avatarLocalData.BreakthoughtLevel);

		//Growth
		if (AvatarGrowthBtn != null)
			UIElemTemplate.Inst.SetAvatarTraitIcon(AvatarGrowthBtn, avatarCfg.traitType);

		// Level
		avatarLVLabel.Text = GameUtility.FormatUIString("UIPnlAvatar_AvatarLevel",
											avatarLocalData.LevelAttrib.Level,
											GameDefines.textColorInOrgYew.ToString(),
											avatarCfg.GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel).breakThrough.powerUpLevelLimit);

		//Set avatarData attribute.
		AttributeCalculator.Attribute PAP = null, MAP = null;
		foreach (AttributeCalculator.Attribute attribute in PlayerDataUtility.GetAvatarAttributes(avatarLocalData, false))
		{
			switch (attribute.type)
			{
				case _AvatarAttributeType.MaxHP:
					avatarHPLabel.Text = ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value);
					break;
				case _AvatarAttributeType.PAP:
					PAP = attribute;
					break;
				case _AvatarAttributeType.MAP:
					MAP = attribute;
					break;
				case _AvatarAttributeType.Speed:
					avatarSPDLabel.Text = ItemInfoUtility.GetAttribDisplayString(attribute.type, attribute.value);
					break;
			}
		}
		if (PAP != null && MAP != null)
		{
			if (PAP.value > MAP.value)
				avatarATKLabel.Text = ItemInfoUtility.GetAttribDisplayString(PAP.type, PAP.value);
			else
				avatarATKLabel.Text = ItemInfoUtility.GetAttribDisplayString(MAP.type, MAP.value);
		}
		else
		{
			if (PAP == null && MAP != null)
				avatarATKLabel.Text = ItemInfoUtility.GetAttribDisplayString(MAP.type, MAP.value);
			else if (MAP == null && PAP != null)
				avatarATKLabel.Text = ItemInfoUtility.GetAttribDisplayString(PAP.type, PAP.value);
		}

		//country设置国家图片
		if (avatarCfg != null)
			UIElemTemplate.Inst.SetAvatarCountryIcon(countryImage, avatarCfg.countryType);

		// Set Active Skill.
		foreach (var activeSkill in PlayerDataUtility.GetAvatarActiveSkill(avatarLocalData.ResourceId, avatarLocalData.BreakthoughtLevel))
		{
			var skillItem = SkillIconPool.AllocateItem().GetComponent<UIElemAvatarInfoSkill>();
			skillItem.SetData(activeSkill);
			skillItem.skillIcon.border.controlIsEnabled = false;
			SkillList.AddItem(skillItem.gameObject);
		}
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();
		avatarPower = 0f;
		positionPower = 0f;
	}

	public override void OnHide()
	{
		ClearList();
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		avatarPower = 0f;
		positionPower = 0f;
		SkillIconPool.ClearScrollListAndReleaseItemsToPool(SkillList);
	}

	private void ClearList()
	{
		SkillList.ClearList(false);
	}

	private void SetUpgradeUI()
	{
		//加载可提升属性信息
		List<GrowthAttribute> growthAttributes = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId).GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel).breakThrough.growthAttributes; 

		int deltaMaxHP = 0;
		int deltaPAP = 0;
		int deltaMAP = 0;
		int deltaSpeed = 0;
		for (int i = 0; i < growthAttributes.Count; i++)
		{
			GrowthAttribute attribute = growthAttributes[i];
			if (attribute.type == _AvatarAttributeType.MaxHP)
			{
				deltaMaxHP = (int)attribute.deltaValue;
			}
			else if (attribute.type == _AvatarAttributeType.PAP)
			{
				deltaPAP = (int)attribute.deltaValue;
			}
			else if (attribute.type == _AvatarAttributeType.MAP)
			{
				deltaMAP = (int)attribute.deltaValue;
			}
			else if (attribute.type == _AvatarAttributeType.Speed)
			{
				deltaSpeed = (int)attribute.deltaValue;
			}
		}

		hpCount.Text = deltaMaxHP.ToString();
		papCount.Text = Mathf.Max(deltaPAP, deltaMAP).ToString();
		speedCount.Text = deltaSpeed.ToString();
	}

	private int GetMaxLevelCostCount(int fromLevel, int toLevel)
	{
		//获取一键满级消耗Count
		AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
		int costCount = 0;
		for (int i = fromLevel; i < toLevel; i++)
		{
			QualityCost qualityCost = ConfigDatabase.DefaultCfg.AvatarConfig.GetQualityCostByLevelAndQuality(i, avatarConfig.qualityLevel);
			costCount += qualityCost.costs[0].count;
		}

		return costCount;
	}

	private void SetCostUI()
	{
		//加载升级消耗ICON COUNT
		int currentLevel = avatarLocalData.LevelAttrib.Level;
		int breakThroughLevel = avatarLocalData.BreakthoughtLevel;
		AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
		int currentMaxLevel = avatarConfig.GetAvatarBreakthrough(breakThroughLevel).breakThrough.powerUpLevelLimit;
		QualityCost qualityCost = ConfigDatabase.DefaultCfg.AvatarConfig.GetQualityCostByLevelAndQuality(currentLevel, avatarConfig.qualityLevel);

		if (qualityCost.costs.Count == 0)
		{
			return;
		}

		int costItemId = qualityCost.costs[0].id;
		int costItemCount = qualityCost.costs[0].count;

		onceCostIcon.SetData(costItemId);
		totalCostIcon.SetData(costItemId);

		activityNotify_Total.Hide(!ItemInfoUtility.IsLevelNotifyActivity_Avatar(avatarLocalData));


		onceCostCount.Text = costItemCount.ToString();
		onceCostCount.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob3"),
											SysLocalDataBase.Inst.LocalPlayer.GameMoney >= costItemCount ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											costItemCount);
		totalCostCount.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob3"),
											SysLocalDataBase.Inst.LocalPlayer.GameMoney >= GetMaxLevelCostCount(currentLevel, currentMaxLevel) ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											GetMaxLevelCostCount(currentLevel, currentMaxLevel));
	}

	/// <summary>
	/// 升级成功回调
	/// </summary>
	/// <param name="avatarLocalData"> avatar 信息 </param>
	/// <param name="levelUpType"> 升级类型【true 一级  false 满级】 </param>
	/// <param name="preLevel"> 操作前的等级 </param>
	/// <param name="critCount">??</param>
	/// <param name="costAndRewardAndSync"> 消耗信息 </param>
	public void OnAvatarLevelUpResponse(KodGames.ClientClass.Avatar avatarLocalData, bool levelUpType, int preLevel, int critCount, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
		this.avatarLocalData = avatarLocalData;
		costCount = GetMaxLevelCostCount(preLevel, avatarLocalData.LevelAttrib.Level);
		// Set UIEffectPowerUp Del.
		SysUIEnv.Instance.GetUIModule<UIEffectPowerUp>().SetEffectHideCallback(
			(DataCompare) =>
			{
				string successMsg = string.Empty;
				if (levelUpType)
					successMsg = GameUtility.FormatUIString("UIPnlAvatarLevelUpTab_Ctrl_LevelUpSuccess", avatarLocalData.LevelAttrib.Level);
				else
					successMsg = GameUtility.FormatUIString("UIPnlAvatarLevelUpTab_Ctrl_LevelUpFull", preLevel, avatarLocalData.LevelAttrib.Level, costAndRewardAndSync.Costs[0].Count, critCount, costCount - costAndRewardAndSync.Costs[0].Count);

				SysUIEnv.Instance.GetUIModule<UIPnlAvatarPowerUpTab>().UpdateTabStatus(_UIType.UIPnlAvatarLevelUp, avatarLocalData);
				//show currentLevel's properties and preLevel's properties
				AvatarAttributeUpdateDetail(preLevel, avatarLocalData.LevelAttrib.Level);

				// Reset UI.
				ShowUI();

				// Show Tips.
				SysUIEnv.Instance.AddTip(successMsg);
				float tempAvatarPower = PlayerDataUtility.CalculateAvatarPower(avatarLocalData);
				if (tempAvatarPower > avatarPower)
					SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(tempAvatarPower - avatarPower)));
				else if (tempAvatarPower < avatarPower)
					SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(avatarPower - tempAvatarPower)));

				if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, avatarLocalData.Guid, avatarLocalData.ResourceId))
				{
					float tempPositionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
					if (tempPositionPower > positionPower)
						SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionUp", (int)(tempPositionPower - positionPower)));
					else if (tempPositionPower < positionPower)
						SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionDown", (int)(positionPower - tempPositionPower)));
				}

				avatarPower = 0f;
				positionPower = 0f;
			}
			);

		//Show effect
		SysUIEnv.Instance.ShowUIModule(typeof(UIEffectPowerUp), avatarLocalData.ResourceId, critCount > 0 ? UIEffectPowerUp.LabelType.Crit : UIEffectPowerUp.LabelType.Success, levelUpType);
	}

	//计算战力
	private void CalculatePower()
	{
		avatarPower = PlayerDataUtility.CalculateAvatarPower(avatarLocalData);
		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, avatarLocalData.Guid, avatarLocalData.ResourceId))
			positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOnce(UIButton btn)
	{
		CalculatePower();
		//单次升级请求
		RequestMgr.Inst.Request(new AvatarLevelUpReq(avatarLocalData.Guid, true));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTotal(UIButton btn)
	{
		CalculatePower();
		//一键满级请求
		RequestMgr.Inst.Request(new AvatarLevelUpReq(avatarLocalData.Guid, false));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDetail(UIButton btn)
	{
		if (avatarLocalData.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId).GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel).breakThrough.powerUpLevelLimit)
			AvatarAttributeUpdateDetail(avatarLocalData.LevelAttrib.Level, avatarLocalData.LevelAttrib.Level);
		else
			AvatarAttributeUpdateDetail(avatarLocalData.LevelAttrib.Level, avatarLocalData.LevelAttrib.Level + 1);
	}

	private void AvatarAttributeUpdateDetail(int levelOne, int leveltwo)
	{
		//Avatar详细信息
		UIPnlAvatarAttributeUpdateDetail.ChangeData data0 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
		data0.Level = levelOne;
		data0.BreakthoughtLevel = avatarLocalData.BreakthoughtLevel;
		data0.ResourceId = avatarLocalData.ResourceId;

		UIPnlAvatarAttributeUpdateDetail.ChangeData data1 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
		data1.Level = leveltwo;
		data1.BreakthoughtLevel = avatarLocalData.BreakthoughtLevel;
		data1.ResourceId = avatarLocalData.ResourceId;

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarAttributeUpdateDetail), false, UIPnlAvatarAttributeUpdateDetail._UIShowType.LevelUPDetail, data0, data1);
	}
}
