using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIPnlSkillPowerUp : UIPnlItemInfoBase
{
	// control object
	public GameObject rootPowerUp;
	public GameObject rootResult;
	public GameObject rootMaxLevel;

	//////////////////////////////////////////////////////////////////////////
	// SKILL INFOS
	//////////////////////////////////////////////////////////////////////////
	// skill icon
	public UIElemAssetIcon skillIconBtn;
	// skill attributes
	public SpriteText skillNameLabel;
	public SpriteText skillQualityLabel;
	public SpriteText skillLevelLabel;
	public SpriteText skillUnlockDescLabel;
	public SpriteText runTimeskillExp;

	public UIElemSelectItem selectBtn;
	// skill level progress bar
	public UIProgressBar skillExp, resultOriProgress, resultCurProgress;
	public SpriteText addedExpLabel;

	// skill data
	private KodGames.ClientClass.Skill skillToPowerUp;

	//////////////////////////////////////////////////////////////////////////
	// SKILL LIST
	//////////////////////////////////////////////////////////////////////////
	// skill list
	public UIScrollList skillPageList;
	public UIScrollList skillUnlockList;
	public GameObjectPool skillPageObjectPool;
	public GameObjectPool viewMoreObjPool;
	public SpriteText emptyTip;
	public SpriteText maxLevelTip;

	// origin and new skill result infos
	//升级后的提示
	public SpriteText skillLevel;
	public SpriteText skillDesc;
	public SpriteText orgSkillLevel;
	public SpriteText orgSkillDesc;

	//消耗铜币
	public SpriteText constNumberLable;
	public List<SpriteText> maxLevelAddProperties;

	private readonly int ROW_COUNT_SHOW_ONCE = 3;
	private readonly int ITEM_COUNT_PER_ROW = 5;
	private List<KodGames.ClientClass.Skill> totalSkills = new List<KodGames.ClientClass.Skill>();
	private List<KodGames.ClientClass.Skill> lowQualitySkills = new List<KodGames.ClientClass.Skill>();

	private UIListItemContainer viewMoreBtnItem;
	private UIListItemContainer tipItem;

	private int currentPos = 0;
	// to-be-promoted skill's temporary information
	private KodGames.ClientClass.Skill runTimeSkill;
	private int caledSkillNum = 0;
	private string skillGuid;

	//临时保存书籍战力和阵容战力，如果不在主力阵容里面，战力为零
	private float skillPower;
	private float positionPower;

	// UI FUNCTIONS & SKILL INFO FUNCTIONS
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		maxLevelTip.Text = GameUtility.GetUIString("UIPnlSkillPU_Tip");
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().SetIsOverlaySetPower();

		/*selectBtn.selectBtn.Text = GameUtility.GetUIString("");*/
		selectBtn.InitState("", OnClickSelectAll);

		this.skillGuid = (userDatas[0] as KodGames.ClientClass.Skill).Guid;
		ShowPromoteView(userDatas[0] as KodGames.ClientClass.Skill);

		skillPower = 0f;
		positionPower = 0f;

		return true;
	}

	public override void OnHide()
	{
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageSkillTab))
			SysUIEnv.Instance.GetUIModule<UIPnlPackageSkillTab>().RefreshView(this.skillGuid);

		skillPower = 0f;
		positionPower = 0f;

		ClearList();
		base.OnHide();
	}

	private void ClearList()
	{
		//Stop coroutine and clear the list control.
		StopCoroutine("FillSkillList");

		//Reset scroll list.
		skillPageList.ScrollListTo(0f);
		skillPageList.ClearList(false);

		//skillUnlockList.ScrollListTo(0f);
		//skillUnlockList.ClearList(false);

		// ReSet Data.
		totalSkills.Clear();
		lowQualitySkills.Clear();
		currentPos = 0;
		viewMoreBtnItem = null;
		tipItem = null;
		caledSkillNum = 0;
		skillGuid = null;

		// Reset UI.
		emptyTip.Text = "";
		rootMaxLevel.SetActive(false);
	}

	private void ShowPromoteView(KodGames.ClientClass.Skill skill)
	{
		selectBtn.SetState(false);

		rootResult.SetActive(false);
		rootPowerUp.SetActive(true);

		SetData(skill);

		if (skill.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).maxLevel)
			OnSkillReachMaxLevel();

		caledSkillNum = 0;

		if (rootPowerUp.activeSelf)
			StartCoroutine("FillSkillList");
	}

	//*****************
	//升级协议返回【已经对该书籍进行了修改，所以需要在升级前对书籍进行一次临时保存】
	//如果该书籍在主力阵容里面，那么在对书籍修改前还需要对主力阵容的战力进行一次保存
	//******************
	public void ShowRresultView(KodGames.ClientClass.LevelAttrib orgSkillAttrib, KodGames.ClientClass.Skill skill)
	{
		rootResult.SetActive(true);
		rootPowerUp.SetActive(false);
		RefreshResultData(orgSkillAttrib, skill);
		SetData(skill);

		//战力tips提示
		float tempPositionPower = 0f;
		float tempSkillPower = 0f;

		tempSkillPower = ConfigDatabase.DefaultCfg.SkillConfig.GetOneSkillBasePower(skillToPowerUp.ResourceId, skillToPowerUp.LevelAttrib.Level);
		if (tempSkillPower > skillPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(tempSkillPower - skillPower)));
		else if (tempSkillPower < skillPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneDown", (int)(skillPower - tempSkillPower)));

		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, skillToPowerUp.Guid, skillToPowerUp.ResourceId))
		{
			tempPositionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
			if (tempPositionPower > positionPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionUp", (int)(tempPositionPower - positionPower)));
			else if (tempPositionPower < positionPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionDown", (int)(positionPower - tempPositionPower)));
		}

		skillPower = 0f;
		positionPower = 0f;
	}

	private void OnSkillReachMaxLevel()
	{
		rootResult.SetActive(false);
		rootPowerUp.SetActive(false);
		rootMaxLevel.SetActive(true);
	}

	private void UpdataLowQualitySelectToggleBtn()
	{
		List<KodGames.ClientClass.Skill> selectedSkills = GetSelectedSkills();
		if (lowQualitySkills.Count > 0)
		{
			for (int i = 0; i < lowQualitySkills.Count; i++)
				if (!selectedSkills.Contains(lowQualitySkills[i]))
				{
					break;
				}
		}
	}

	private void SetlowQualitySkillSelected(bool selected)
	{
		for (int pIndex = 0; pIndex < skillPageList.Count; pIndex++)
		{
			UIElemSkillPageItem pItem = skillPageList.GetItem(pIndex).Data as UIElemSkillPageItem;
			if (pItem == null)
				continue;

			for (int itemIdx = 0; itemIdx < pItem.skillList.Count; itemIdx++)
			{
				UIElemSkillSelectToggleItem toogleItem = pItem.skillList[itemIdx];
				if (toogleItem.SkillData == null)
					continue;
				if (selected && toogleItem.IsSelected)
					continue;
				if (IsLowLevelSkill(toogleItem.SkillData.ResourceId))
				{
					toogleItem.ResetToggleState(selected);
					if (selected)
					{
						//int playerLv = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level;
						int skillMaxLv = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(runTimeSkill.ResourceId).maxLevel;
						//if ((runTimeSkill.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(runTimeSkill.ResourceId).maxLevel)
						if (/*runTimeSkill.LevelAttrib.Level == playerLv || */runTimeSkill.LevelAttrib.Level == skillMaxLv)//去掉玩家等级限制
						{
							SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString(
								/*runTimeSkill.LevelAttrib.Level == playerLv ? "UIPnlLevelUpTab_Tip_SkillLvBeyondPlayerLv" : */"UIPnlLevelUpTab_Tip_SkillLvFullLv"));
							toogleItem.ToggleState();
							return;
						}
					}
					UpdateRunTimeLvExp();
				}
			}
		}
	}

	private bool IsLowLevelSkill(int skillId)
	{
		return ItemInfoUtility.GetAssetQualityLevel(skillId) < 4;
	}

	private void SetSkillControls(KodGames.ClientClass.Skill skill)
	{
		// Skill Config.
		var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);

		// Set performance icon
		skillIconBtn.SetData(skill);

		// Set skill name
		skillNameLabel.Text = ItemInfoUtility.GetAssetName(skill.ResourceId);

		string str = "";
		for (int cont = 0; cont < ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs.Count; cont++)
		{
			if (skill.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs[cont].level)
			{
				//已经解锁
				str += string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob3"), GameDefines.textColorWhite,
													ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs[cont].unlockDesc);
			}
			else
			{
				//未解锁
				str += string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob4"), GameDefines.textColorBtnYellow,
									ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs[cont].unlockDesc,
									ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs[cont].level);
			}

			str += "\n";
		}
		skillUnlockDescLabel.Text = str;

		skillUnlockList.ScrollPosition = 0f;
		// Skill quality
		skillQualityLabel.Text = string.Format("{0}", ItemInfoUtility.GetAssetQualityLongColorDesc(skill.ResourceId));

		// Set level attribute
		skillLevelLabel.Text = GameUtility.FormatUIString("UIPnlLevelUpTab_Label_Level", skill.LevelAttrib.Level);

		if (skill.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).maxLevel)
		{
			skillExp.Value = 0f;
			skillExp.Text = "";

			var modifers = skillCfg.GetLevelModifers(skill.LevelAttrib.Level);
			if (modifers != null)
			{
				var attributes = PlayerDataUtility.MergeAttributes(modifers, true, true);

				for (int i = 0; i < maxLevelAddProperties.Count; i++)
				{
					if (i < attributes.Count)
					{
						maxLevelAddProperties[i].Hide(false);
						maxLevelAddProperties[i].Text = GameUtility.FormatUIString(
						"UIPnlSkillPowerUp_MaxLevelAttri",
						GameDefines.textColorBtnYellow,
						_AvatarAttributeType.GetDisplayNameByType(attributes[i].type, ConfigDatabase.DefaultCfg),
						GameDefines.textColorWhite,
						ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value));
					}
					else
					{
						maxLevelAddProperties[i].Hide(true);
					}
				}
			}
		}
		else
		{
			int requiredExp = GetSkillUpgradeNeedExperience(skill);
			if (requiredExp != 0)
				skillExp.Value = (float)skill.LevelAttrib.Experience / (float)requiredExp;
			else
				skillExp.Value = 1.0f;
			UpdateLvlProcessBar();
		}

		SetCostNumber();
	}

	private void SetData(KodGames.ClientClass.Skill powerUpSkill)
	{
		ClearList();

		skillToPowerUp = new KodGames.ClientClass.Skill();
		skillToPowerUp.Guid = powerUpSkill.Guid;
		skillToPowerUp.ResourceId = powerUpSkill.ResourceId;
		skillToPowerUp.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
		skillToPowerUp.LevelAttrib.Level = powerUpSkill.LevelAttrib.Level;
		skillToPowerUp.LevelAttrib.Experience = powerUpSkill.LevelAttrib.Experience;

		// set [runTimeSkill] datas
		runTimeSkill = new KodGames.ClientClass.Skill();
		runTimeSkill.Guid = skillToPowerUp.Guid;
		runTimeSkill.ResourceId = skillToPowerUp.ResourceId;
		runTimeSkill.LevelAttrib = new LevelAttrib();
		runTimeSkill.LevelAttrib.Level = skillToPowerUp.LevelAttrib.Level;
		runTimeSkill.LevelAttrib.Experience = skillToPowerUp.LevelAttrib.Experience;

		// Init Data.
		foreach (KodGames.ClientClass.Skill skill in SysLocalDataBase.Inst.LocalPlayer.Skills)
		{
			if (!skill.Guid.Equals(skillToPowerUp.Guid) &&
				ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).type == CombatTurn._Type.PassiveSkill && !PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, skill))//只有被动技能可以升级
			{
				totalSkills.Add(skill);

				if (IsLowLevelSkill(skill.ResourceId))
					lowQualitySkills.Add(skill);
			}
		}

		totalSkills.Sort(DataCompare.CompareSkillReverse);

		// Show Common UI.
		SetSkillControls(powerUpSkill);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillSkillList()
	{
		yield return null;
		if (totalSkills.Count > (currentPos + ROW_COUNT_SHOW_ONCE) * ITEM_COUNT_PER_ROW)
		{
			InsertSkillRows(totalSkills.GetRange(currentPos * ITEM_COUNT_PER_ROW, ITEM_COUNT_PER_ROW * ROW_COUNT_SHOW_ONCE));

			if (viewMoreBtnItem == null)
			{
				viewMoreBtnItem = viewMoreObjPool.AllocateItem().GetComponent<UIListItemContainer>();
				skillPageList.AddItem(viewMoreBtnItem);
			}
		}
		else if (totalSkills.Count < (currentPos + ROW_COUNT_SHOW_ONCE) * ITEM_COUNT_PER_ROW)
		{
			InsertSkillRows(totalSkills.GetRange(currentPos * ITEM_COUNT_PER_ROW, totalSkills.Count % (ITEM_COUNT_PER_ROW * ROW_COUNT_SHOW_ONCE)));

			if (viewMoreBtnItem != null)
			{
				skillPageList.RemoveItem(viewMoreBtnItem, false);
				viewMoreBtnItem = null;
			}
		}
		else
		{
			InsertSkillRows(totalSkills.GetRange(currentPos * ITEM_COUNT_PER_ROW, ITEM_COUNT_PER_ROW * ROW_COUNT_SHOW_ONCE));

			if (viewMoreBtnItem != null)
			{
				skillPageList.RemoveItem(viewMoreBtnItem, false);
				viewMoreBtnItem = null;
			}
		}

		if (skillPageList.Count <= 0 && !emptyTip.Text.Equals(GameUtility.GetUIString("UIPnlSkillPowerUp_Label_EmptyMessage")))
			emptyTip.Text = GameUtility.GetUIString("UIPnlSkillPowerUp_Label_EmptyMessage");
		else if (skillPageList.Count > 0 && !emptyTip.Text.Equals(""))
			emptyTip.Text = "";
		else if (totalSkills.Count <= 0 && tipItem != null)
		{
			skillPageList.RemoveItem(tipItem, false);
			tipItem = null;
		}
	}

	private List<KodGames.ClientClass.Skill> GetInsertSameSkill()
	{
		int index = caledSkillNum;
		List<KodGames.ClientClass.Skill> list = new List<KodGames.ClientClass.Skill>();

		int skillCount = 0;
		for (; index < totalSkills.Count && skillCount < ITEM_COUNT_PER_ROW * ROW_COUNT_SHOW_ONCE; index++)
		{
			if (totalSkills[index].ResourceId == skillToPowerUp.ResourceId)
			{
				list.Add(totalSkills[index]);
				skillCount++;
			}
		}
		caledSkillNum = index;

		return list;
	}

	private void InsertSkillRows(List<KodGames.ClientClass.Skill> skillItems)
	{
		int idx = 0;
		for (idx = 0; idx < (skillItems.Count / ITEM_COUNT_PER_ROW); idx++)
		{
			InsertOneRow(skillItems.GetRange(idx * ITEM_COUNT_PER_ROW, ITEM_COUNT_PER_ROW));
		}
		if (skillItems.Count % ITEM_COUNT_PER_ROW != 0)
			InsertOneRow(skillItems.GetRange(idx * ITEM_COUNT_PER_ROW, skillItems.Count % ITEM_COUNT_PER_ROW));
	}

	private void InsertOneRow(List<KodGames.ClientClass.Skill> rowSkills)
	{
		UIListItemContainer container = skillPageObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemSkillPageItem pageItem = container.gameObject.GetComponent<UIElemSkillPageItem>();
		container.Data = pageItem;
		pageItem.SetData(rowSkills);
		if (tipItem != null)
			skillPageList.InsertItem(container, currentPos + 1);
		else
			skillPageList.InsertItem(container, currentPos);

		currentPos++;
	}

	/// <summary>
	/// update datas and UI when player's choice changed
	/// </summary>
	private void UpdateRunTimeLvExp()
	{
		runTimeSkill.LevelAttrib.Level = skillToPowerUp.LevelAttrib.Level;
		runTimeSkill.LevelAttrib.Experience = skillToPowerUp.LevelAttrib.Experience;

		RunTimeSkillAddExp(GetSelectedExpSum());

		UpdateLvlProcessBar();

		UpdataLowQualitySelectToggleBtn();
	}

	private void RunTimeSkillAddExp(int exp)
	{
		if (exp <= 0 || GetSkillUpgradeNeedExperience(runTimeSkill) <= 0)
			return;

		int newExp = runTimeSkill.LevelAttrib.Experience + exp;
		while (newExp >= GetSkillUpgradeNeedExperience(runTimeSkill))
		{
			newExp -= GetSkillUpgradeNeedExperience(runTimeSkill);
			runTimeSkill.LevelAttrib.Level++;

			if (runTimeSkill.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillToPowerUp.ResourceId).maxLevel)
			{
				return;
			}
		}
		runTimeSkill.LevelAttrib.Experience = newExp;
	}

	/// <summary>
	/// reset exp process bar to keep it the same as [runTimeSkill]'s data
	/// </summary>
	private void UpdateLvlProcessBar()
	{
		if (runTimeSkill.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillToPowerUp.ResourceId).maxLevel)
		{
			skillLevelLabel.Text = string.Format(GameUtility.GetUIString("UIPnlAvatar_Label_SkillLevel"), runTimeSkill.LevelAttrib.Level);
			runTimeSkill.LevelAttrib.Level--;
			skillExp.Text = string.Format("{0}/{1}", GetSkillUpgradeNeedExperience(runTimeSkill), GetSkillUpgradeNeedExperience(runTimeSkill));
			runTimeSkill.LevelAttrib.Level++;

			addedExpLabel.Text = GameDefines.txColorGreen + "+" + GetSelectedExpSum();

			skillExp.Value = 1f;
			return;
		}
		int expSum = GetSelectedExpSum();
		if (expSum == 0) addedExpLabel.Text = GameDefines.txColorGreen + "+0";
		else addedExpLabel.Text = GameDefines.txColorGreen + "+" + expSum;

		skillExp.Text = string.Format("{0}/{1}", runTimeSkill.LevelAttrib.Experience, GetSkillUpgradeNeedExperience(runTimeSkill));
		skillExp.Value = (float)runTimeSkill.LevelAttrib.Experience / (float)GetSkillUpgradeNeedExperience(runTimeSkill);
		skillLevelLabel.Text = string.Format(GameUtility.GetUIString("UIPnlAvatar_Label_SkillLevel"), runTimeSkill.LevelAttrib.Level);
	}

	private int GetSelectedExpSum()
	{
		int exp = 0;

		foreach (var skill in GetSelectedSkills())
		{
			var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);
			if (skillCfg == null)
				continue;

			exp += ConfigDatabase.DefaultCfg.SkillConfig.GetSkillUpgradeSettingByQualityLevel(skillCfg.qualityLevel).GetSkillUpgradeByLevel(skill.LevelAttrib.Level).supplyExperiences;
		}

		return exp;
	}

	private List<KodGames.ClientClass.Skill> GetSelectedSkills()
	{
		int infoCount = 0;
		if (viewMoreBtnItem != null)
			infoCount = skillPageList.Count - 1;
		else
			infoCount = skillPageList.Count;

		var selectedSkills = new List<KodGames.ClientClass.Skill>();
		for (int index = 0; index < infoCount; index++)
		{
			var pageItem = (UIElemSkillPageItem)skillPageList.GetItem(index).Data;
			if (pageItem == null || pageItem.skillList == null)
				continue;

			foreach (UIElemSkillSelectToggleItem skillItem in pageItem.skillList)
			{
				if (skillItem.SkillData != null && skillItem.IsSelected)
				{
					selectedSkills.Add(skillItem.SkillData);
				}
			}
		}

		return selectedSkills;
	}

	// RESULT FUNCTIONS
	private void RefreshResultData(KodGames.ClientClass.LevelAttrib orgSkillAttrib, KodGames.ClientClass.Skill skill)
	{
		var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillToPowerUp.ResourceId);

		// set result texts
		orgSkillLevel.Text = GameUtility.FormatUIString("UIPnlSkillPowerUpRes_Label_Level", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(), skillToPowerUp.LevelAttrib.Level.ToString());
		orgSkillDesc.Text = GetDesc(skillCfg.GetLevelModifers(orgSkillAttrib.Level));
		resultOriProgress.Text = string.Format("{0}/{1}", orgSkillAttrib.Experience, GetSkillUpgradeNeedExperience(skillToPowerUp));
		resultOriProgress.Value = (float)orgSkillAttrib.Experience / (float)GetSkillUpgradeNeedExperience(skillToPowerUp);

		SetCostNumber();

		skillLevel.Text = GameUtility.FormatUIString("UIPnlSkillPowerUpRes_Label_Level", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(), runTimeSkill.LevelAttrib.Level.ToString());

		string str = "\n";
		int tempLevel = -1;
		for (int cont = 0; cont < ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs.Count; cont++)
		{
			if (tempLevel != ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs[cont].level && skill.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs[cont].level && orgSkillAttrib.Level < ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs[cont].level)
			{
				tempLevel = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs[cont].level;
				str += GameUtility.FormatUIString("UIPnlSkillPU_UnLock", GameDefines.textColorWhite.ToString(), ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).skillUnlockDescs[cont].level) + "\n";
			}
		}

		skillDesc.Text = GetDesc(skillCfg.GetLevelModifers(skill.LevelAttrib.Level)) + "\n" + str;

		if (runTimeSkill.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(runTimeSkill.ResourceId).maxLevel)
		{
			resultCurProgress.Text = GameDefines.txColorRed + GameUtility.GetUIString("UIPnlSkillPowerUpRes_Label_MaxLvlReached");
			resultCurProgress.Value = 1;
		}
		else
		{
			resultCurProgress.Text = string.Format("{0}/{1}", runTimeSkill.LevelAttrib.Experience, GetSkillUpgradeNeedExperience(runTimeSkill));
			resultCurProgress.Value = (float)runTimeSkill.LevelAttrib.Experience / (float)GetSkillUpgradeNeedExperience(runTimeSkill);
		}
	}

	private int CalExpExceed(KodGames.ClientClass.LevelAttrib orgLevelAttrib, KodGames.ClientClass.LevelAttrib levelAttrib)
	{
		KodGames.ClientClass.Skill rtmSkill = new KodGames.ClientClass.Skill();
		rtmSkill.Guid = skillToPowerUp.Guid;
		rtmSkill.ResourceId = skillToPowerUp.ResourceId;
		rtmSkill.LevelAttrib = new LevelAttrib();
		rtmSkill.LevelAttrib.Level = orgLevelAttrib.Level;
		rtmSkill.LevelAttrib.Experience = orgLevelAttrib.Experience;

		if (levelAttrib.Level < orgLevelAttrib.Level)
			return -1;

		if (levelAttrib.Level == orgLevelAttrib.Level)
			return (levelAttrib.Experience - orgLevelAttrib.Experience);

		int exp = 0;

		exp += (GetSkillUpgradeNeedExperience(rtmSkill) - orgLevelAttrib.Experience);
		rtmSkill.LevelAttrib.Level++;
		orgLevelAttrib.Level++;
		exp += levelAttrib.Experience;

		while (orgLevelAttrib.Level < levelAttrib.Level)
		{
			exp += GetSkillUpgradeNeedExperience(rtmSkill);
			rtmSkill.LevelAttrib.Level++;
			orgLevelAttrib.Level++;
		}

		return exp;
	}

	/*[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]*/
	private void OnClickSelectAll(bool a, object data)
	{
		if (lowQualitySkills.Count > 0)
		{
			SetlowQualitySkillSelected(selectBtn.IsSelected);
		}
		else
		{
			selectBtn.SetState(false);
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlSkillPowerUp_Label_NoLowLvSkill"));
		}

		SetCostNumber();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnViewMoreClick(UIButton btn)
	{
		if (viewMoreBtnItem != null)
		{
			skillPageList.RemoveItem(viewMoreBtnItem, false);
			viewMoreBtnItem = null;
		}

		if (rootPowerUp.activeSelf)
			StartCoroutine("FillSkillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSkillItemClick(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		UIElemSkillSelectToggleItem skillToggleItem = assetIcon.Data as UIElemSkillSelectToggleItem;

		if ((runTimeSkill.LevelAttrib.Level >= ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillToPowerUp.ResourceId).maxLevel)
			&& (!(skillToggleItem.IsSelected)))
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlSkillPowerUpRes_Label_MaxLvlReached"));
			return;
		}

		skillToggleItem.ToggleState();

		UpdateRunTimeLvExp();
		SetCostNumber();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageSkillTab))
			SysUIEnv.Instance.GetUIModule<UIPnlPackageSkillTab>().RefreshView(runTimeSkill.Guid);
	}

	/// raise the eat skill click event
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSkillPowerUpClick(UIButton btn)
	{
		if (GetSelectedSkills().Count <= 0)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgSkillPowerUp_NotSelectSkills"));

			return;
		}

		List<string> skillGuidList = new List<string>();

		bool hasHighQualitySkill = false;
		foreach (KodGames.ClientClass.Skill skill in GetSelectedSkills())
		{
			int quality = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).qualityLevel;

			if (quality >= 4)
			{
				hasHighQualitySkill = true;
			}
			skillGuidList.Add(skill.Guid);
		}

		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, skillToPowerUp.Guid, skillToPowerUp.ResourceId))
			positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		skillPower = ConfigDatabase.DefaultCfg.SkillConfig.GetOneSkillBasePower(skillToPowerUp.ResourceId, skillToPowerUp.LevelAttrib.Level);

		if (hasHighQualitySkill)
		{
			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			string title = GameUtility.GetUIString("UIDlgMessage_Title");
			string message = GameUtility.GetUIString("UIDlgMessage_Msg_Warn");

			MainMenuItem cancelCallback = new MainMenuItem();
			cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");

			MainMenuItem okCallback = new MainMenuItem();
			okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
			okCallback.Callback =
				(data) =>
				{
					RequestMgr.Inst.Request(new SkillLevelUpReq(skillToPowerUp.Guid, skillGuidList));
					return true;
				};

			showData.SetData(title, message, cancelCallback, okCallback);
			SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIDlgMessage>().ShowDlg(showData);
		}
		else
		{
			RequestMgr.Inst.Request(new SkillLevelUpReq(skillToPowerUp.Guid, skillGuidList));
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOKButtonClick(UIButton btn)
	{
		ShowPromoteView(runTimeSkill);
	}

	int GetSkillUpgradeNeedExperience(KodGames.ClientClass.Skill kd_skill)
	{
		return ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(kd_skill.ResourceId).GetUpgrateExperience(kd_skill.LevelAttrib.Level);
	}

	private void SetCostNumber()
	{
		//消耗铜币
		int number = 0;

		var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillToPowerUp.ResourceId);

		for (int level = skillToPowerUp.LevelAttrib.Level; level < runTimeSkill.LevelAttrib.Level; level++)
		{
			var upgradeCost = skillCfg.GetUpgrateCosts(level);

			if (upgradeCost == null)
			{
				Debug.LogWarning(string.Format("SkillCfg Quality Level {0} Not Have Level {1} Setting.", skillCfg.id.ToString("X"), level));
				continue;
			}

			for (int i = 0; i < upgradeCost.Count; i++)
			{
				if (upgradeCost[i].id != IDSeg._SpecialId.GameMoney)
					continue;

				number += upgradeCost[i].count;
			}
		}

		constNumberLable.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob3"),
											SysLocalDataBase.Inst.LocalPlayer.GameMoney >= number ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											number);
	}

	private string GetDesc(List<ClientServerCommon.PropertyModifier> modifiers)
	{
		if (null == modifiers)
			return "";

		var attributes = PlayerDataUtility.MergeAttributes(modifiers, true, true);
		string strDec = "";
		for (int count = 0; count < attributes.Count; count++)
		{
			strDec = GameUtility.AppendString(strDec,
				GameUtility.FormatUIString(
				"UIPnlSkillPowerUp_DescAttri",
				GameDefines.textColorBtnYellow.ToString(),
				_AvatarAttributeType.GetDisplayNameByType(attributes[count].type, ConfigDatabase.DefaultCfg),
				GameDefines.textColorWhite.ToString(),
				ItemInfoUtility.GetAttribDisplayString(attributes[count].type, attributes[count].value)
				),
				true);
		}

		return strDec;
	}
}
