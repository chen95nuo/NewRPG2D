using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlSkillInfo : UIPnlItemInfoBase
{
	public delegate void SelectDelegate(KodGames.ClientClass.Skill selected);

	public SpriteText titleLabel;
	public SpriteText skillName;
	public UIElemAssetIcon skillIcon;
	public SpriteText skillLevel;

	public UIScrollList skillDescList;
	public SpriteText skillDescLabel;

	public AutoSpriteControlBase skillAssembleBox;
	public UIScrollList skillAssembleList;
	public SpriteText skillAssembleLabel;

	public AutoSpriteControlBase skillSuiteBox;
	public UIScrollList skillSuiteList;
	public SpriteText skillSuiteLabel;

	//Action buttons.
	public UIChildLayoutControl actionButtonLayout;
	public UIButton skillChangeBtn;
	public UIButton powerUpBtn;
	public UIButton closeBtn;
	public UIButton selectBtn;
	public UIButton gotoPackageBtn;
	public UIButton prevBtn;
	public UIButton nextBtn;

	private KodGames.ClientClass.Skill skillData;
	private KodGames.ClientClass.Location location;
	private KodGames.ClientClass.Player currentPlayer;
	private int positionId;
	private int skillSoltIndex;
	private bool showSuiteProgress;
	private bool isScroll;
	private SelectDelegate selectDel;
	private Vector2 skillDescListSize;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		skillDescListSize = skillDescList.viewableArea;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		currentPlayer = SysLocalDataBase.Inst.LocalPlayer;

		if (userDatas[0] is SkillConfig.Skill)
		{
			SkillConfig.Skill skillConfig = userDatas[0] as SkillConfig.Skill;
			bool isCardPic = userDatas.Length > 1 ? (bool)userDatas[1] : false;

			List<SkillConfig.Skill> cardPicAvatars = GetCardPictureSkills(skillConfig.type);
			int index = cardPicAvatars.IndexOf(skillConfig);

			SetCardPicSkillUI(index, cardPicAvatars.Count, skillConfig.type, isCardPic);
		}
		else if (userDatas[0] is KodGames.ClientClass.Skill)
		{
			this.skillData = userDatas[0] as KodGames.ClientClass.Skill;

			bool showChange = (bool)userDatas[1];
			bool showBigClose = (bool)userDatas[2];
			bool showGotoPackage = (bool)userDatas[3];
			bool showPowerUp = (bool)userDatas[4];
			selectDel = userDatas[5] as SelectDelegate;
			showSuiteProgress = (bool)userDatas[6];
			ShowActionButtons(showChange, showBigClose, showGotoPackage, showPowerUp, selectDel != null, false, false);
			FillData();
		}
		else if (userDatas[0] is KodGames.ClientClass.Location)
		{
			this.location = userDatas[0] as KodGames.ClientClass.Location;
			bool showChange = (bool)userDatas[1];
			bool showBigClose = (bool)userDatas[2];
			bool showGotoPackage = (bool)userDatas[3];
			bool showPowerUp = (bool)userDatas[4];
			selectDel = userDatas[5] as SelectDelegate;
			showSuiteProgress = (bool)userDatas[6];
			skillSoltIndex = (int)userDatas[7];

			if (userDatas.Length > 8)
				currentPlayer = userDatas[8] as KodGames.ClientClass.Player;

			this.skillData = currentPlayer.SearchSkill(location.Guid);

			ShowActionButtons(showChange, showBigClose, showGotoPackage, showPowerUp, selectDel != null, false, false);

			FillData();
		}
		else if (userDatas[0] is int)
		{
			int skillResourceId;
			if (IDSeg.ToAssetType((int)userDatas[0]) != IDSeg._AssetType.CombatTurn)
			{
				skillResourceId = ConfigDatabase.DefaultCfg.IllustrationConfig.GetIllustrationByFragmentId((int)userDatas[0]).Id;
				this.isScroll = true;
			}
			else
				skillResourceId = (int)userDatas[0];
			var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillResourceId);
			if (skillCfg == null)
				return true;

			SetCardPicSkillUI(skillCfg, 0, 0, false);
		}

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		// Clear UI.
		skillDescList.ScrollListTo(0f);
		skillDescLabel.Text = string.Empty;
		skillAssembleList.ScrollListTo(0f);
		skillAssembleLabel.Text = string.Empty;
		skillSuiteList.ScrollListTo(0f);
		skillSuiteLabel.Text = string.Empty;

		// Clear Data.
		location = null;
		skillData = null;
		currentPlayer = null;
		isScroll = false;
		showSuiteProgress = false;
		selectDel = null;
	}

	private void FillData()
	{
		SkillConfig.Skill skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillData.ResourceId);
		if (skillCfg == null)
			return;

		// Set title Label.
		if (isScroll)
			titleLabel.Text = GameUtility.GetUIString("UIDlgSkillInfo_ScrollTitle");
		else if (skillCfg.type == CombatTurn._Type.PassiveSkill)
			titleLabel.Text = GameUtility.GetUIString("UIDlgSkillInfo_Title");
		else
			titleLabel.Text = GameUtility.GetUIString("UIDlgSkillInfo_Title1");

		//Set name label.
		if (isScroll)
			skillName.Text = ItemInfoUtility.GetAssetName(ConfigDatabase.DefaultCfg.IllustrationConfig.GetIllustrationById(skillCfg.id).FragmentId);
		else
			skillName.Text = ItemInfoUtility.GetAssetName(skillData.ResourceId);

		// Icon
		skillIcon.SetData(skillData.ResourceId);

		// GetWay Button.
		if (!getWayButton.IsHidden())
			getWayButton.Hide(skillCfg.type != CombatTurn._Type.PassiveSkill);

		// Description
		if (skillCfg.type == CombatTurn._Type.CompositeSkill)
			skillDescList.SetViewableArea(skillDescListSize.x, skillDescListSize.y + skillAssembleBox.height + skillAssembleList.viewableArea.y);
		else
			skillDescList.SetViewableArea(skillDescListSize.x, skillDescListSize.y);

		skillDescLabel.Text = ItemInfoUtility.GetAssetDesc(skillData.ResourceId);
		skillDescList.PositionItems();
		skillDescList.ScrollPosition = 0f;

		// SetSkillAssemble
		bool unHasAssemble = string.IsNullOrEmpty(skillCfg.activeableAssembleDesc);
		skillAssembleBox.gameObject.SetActive(!unHasAssemble);
		if (!unHasAssemble)
		{
			skillAssembleLabel.Text = skillCfg.activeableAssembleDesc;
			skillAssembleList.PositionItems();
			skillAssembleList.ScrollPosition = 0f;
		}

		//Set level label.
		skillLevel.Text = string.Format("{0}/{1}{2}",
										GameUtility.FormatUIString("UIPnlAvatar_Label_SkillLevel", skillData.LevelAttrib.Level),
										GameDefines.textColorInOrgYew.ToString(),
										skillCfg.maxLevel.ToString());

		//Set Suite Description and level Description.
		string suiteAndLvlStr = string.Empty;

		// 组合技能等级描述显示
		string skillLevelDesc = string.Empty;
		if (skillCfg.type == CombatTurn._Type.CompositeSkill)
		{
			var skillComposite = skillData as SkillComposite;
			var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(skillComposite.AvatarResourceId);

			for (int level = skillData.LevelAttrib.Level; level <= skillCfg.maxLevel; level++)
			{
				var levelDesc = ItemInfoUtility.GetSkillLevelDesc(skillData.ResourceId, level);
				if (string.IsNullOrEmpty(levelDesc))
					continue;

				if (skillData.LevelAttrib.Level < level)
				{
					var breakCfg = avatarCfg.GetAvatarBreakthrough(avatarCfg.GetBreakThoughtLevelByCompositeSkillLevel(level) - 1);

					if (breakCfg.leastSameCardCount > 0)
						levelDesc = GameUtility.FormatUIString("UIDlgSkillInfo_Desc_CompositeLV1_SameCard", breakCfg.leastSameCardCount, levelDesc);

					levelDesc = GameUtility.FormatUIString("UIDlgSkillInfo_Desc_CompositeLV1",
														   GameDefines.textColorInBlack,
														   breakCfg.breakThrough.fromTimes + 1,
														   level,
														   levelDesc);
				}
				else
					levelDesc = GameUtility.FormatUIString("UIDlgSkillInfo_Desc_CompositeLV2",
														   GameDefines.textColorGreen,
														   levelDesc);

				skillLevelDesc += levelDesc;
			}
		}
		else // 其他技能等级描述显示
		{
			suiteAndLvlStr = GameDefines.textColorBtnYellow.ToString();
			skillLevelDesc = ItemInfoUtility.GetSkillLevelDesc(skillData.ResourceId, skillData.LevelAttrib.Level);
			if (skillData.LevelAttrib.Level < skillCfg.maxLevel)
			{
				skillLevelDesc = GameUtility.FormatUIString("UIDlgSkillInfo_Desc_CurrentLV",
															 skillData.LevelAttrib.Level,
															 skillLevelDesc);

				skillLevelDesc += GameUtility.FormatUIString("UIDlgSkillInfo_Desc_NextLV",
															 skillData.LevelAttrib.Level + 1,
															 ItemInfoUtility.GetSkillLevelDesc(skillData.ResourceId, skillData.LevelAttrib.Level + 1));
			}
		}

		suiteAndLvlStr += skillLevelDesc;
		skillSuiteLabel.Text = suiteAndLvlStr + "\n\n" + ItemInfoUtility.GetSuiteDesc(currentPlayer, skillCfg.id, this.location, showSuiteProgress);

		//重新设置布局
		skillSuiteList.RepositionItems();
		skillSuiteList.ScrollPosition = 0f;
	}

	List<string> GetSkillLevelDescFormatParms(SkillConfig.Skill skillCfg, int level)
	{
		List<PropertyModifier> modifiers = skillCfg.GetLevelModifers(level);

		var mergedAttribs = PlayerDataUtility.MergeAttributes(modifiers, true, true);

		List<string> formatParams = new List<string>();

		for (int i = 0; i < mergedAttribs.Count; i++)
		{
			formatParams.Add(_AvatarAttributeType.GetDisplayNameByType(mergedAttribs[i].type, ConfigDatabase.DefaultCfg));
			formatParams.Add(ItemInfoUtility.GetAttribDisplayString(mergedAttribs[i].type, mergedAttribs[i].value));
		}

		return formatParams;
	}

	private void ShowActionButtons(bool showChange, bool showBigClose, bool showGotoPackage, bool showPowerUp, bool showSelect, bool prev, bool next)
	{
		actionButtonLayout.HideChildObj(skillChangeBtn.gameObject, !showChange);
		actionButtonLayout.HideChildObj(closeBtn.gameObject, !showBigClose);
		actionButtonLayout.HideChildObj(powerUpBtn.gameObject, !showPowerUp);
		actionButtonLayout.HideChildObj(gotoPackageBtn.gameObject, !showGotoPackage);
		actionButtonLayout.HideChildObj(selectBtn.gameObject, !showSelect);
		actionButtonLayout.HideChildObj(prevBtn.gameObject, !prev);
		actionButtonLayout.HideChildObj(nextBtn.gameObject, !next);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChangeSkillClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlSelectSkillList, skillSoltIndex, location);
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPowerUpClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlSkillPowerUp, skillData);
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelect(UIButton btn)
	{
		if (selectDel != null)
			selectDel(skillData);

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGoToPackageClick(UIButton btn)
	{
		HideSelf();
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlPackageSkillTab));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCardPicturePrevClick(UIButton btn)
	{
		SkillConfig.Skill skillCfgData = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillData.ResourceId);

		List<int> cardPictures = GetCardPictureIDs(skillCfgData.type);
		int index = cardPictures.IndexOf(skillData.ResourceId);
		int prevIndex = Mathf.Max(0, index - 1);

		if (index == prevIndex)
			return;

		SetCardPicSkillUI(prevIndex, cardPictures.Count, skillCfgData.type, true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCardPictureNextClick(UIButton btn)
	{
		SkillConfig.Skill skillCfgData = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skillData.ResourceId);

		List<int> cardPictures = GetCardPictureIDs(skillCfgData.type);
		int index = cardPictures.IndexOf(skillData.ResourceId);
		int nextIndex = Mathf.Min(cardPictures.Count - 1, index + 1);

		if (index == nextIndex)
			return;

		SetCardPicSkillUI(nextIndex, cardPictures.Count, skillCfgData.type, true);
	}

	//获得途径
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetWay(UIButton btn)
	{
		if (this.ShowLayer != _UILayer.Top)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgItemGetWay), skillData.ResourceId);
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgItemGetWay), _UILayer.Top, skillData.ResourceId);
	}

	private void SetCardPicSkillUI(int index, int count, int skillType, bool isCardPic)
	{
		List<SkillConfig.Skill> skills = GetCardPictureSkills(skillType);
		SkillConfig.Skill skillCfg = skills[index];

		SetCardPicSkillUI(skillCfg, index, count, isCardPic);
	}

	private void SetCardPicSkillUI(SkillConfig.Skill skillCfg, int index, int count, bool isCardPic)
	{
		skillData = new KodGames.ClientClass.Skill();
		skillData.ResourceId = skillCfg.id;
		skillData.LevelAttrib.Level = 1;

		skillSoltIndex = 0;

		// Disable selecting
		selectDel = null;

		if (isCardPic)
			ShowActionButtons(false, false, false, false, false, index != 0, index != (count - 1));
		else
			ShowActionButtons(false, true, false, false, false, false, false);

		FillData();
	}

	private List<int> GetCardPictureIDs(int skillType)
	{
		List<int> cardPictures = new List<int>();
		foreach (SkillConfig.Skill skill in ConfigDatabase.DefaultCfg.SkillConfig.skills)
		{
			if (cardPictures.Contains(skill.id))
				continue;

			if (skill.type == skillType)
				cardPictures.Add(skill.id);
		}
		cardPictures.Sort(DataCompare.CompareSkill);

		return cardPictures;
	}

	private List<SkillConfig.Skill> GetCardPictureSkills(int skillType)
	{
		List<SkillConfig.Skill> cardPictures = new List<SkillConfig.Skill>();
		foreach (SkillConfig.Skill skill in ConfigDatabase.DefaultCfg.SkillConfig.skills)
		{
			if (skill.type == skillType)
				cardPictures.Add(skill);
		}

		cardPictures.Sort(DataCompare.CompareSkill);

		return cardPictures;
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMenuBot(UIButton btn)
	{
	}
}
