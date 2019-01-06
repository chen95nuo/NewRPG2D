using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlSelectSkillList : UIPnlItemInfoBase
{
	// Skill list.
	public UIScrollList skillList;
	public GameObjectPool skillObjectPool;
	public GameObjectPool viewMoreObjPool;
	public GameObjectPool getObjectPool;
	public SpriteText emptyTip;
	public SpriteText selectedText;

	private const int cItemCountPerPage = 20;
	private List<KodGames.ClientClass.Skill> skillsToFillList = new List<KodGames.ClientClass.Skill>();
	private int currentPosition = 0;
	private UIListItemContainer viewMoreBtnItem;

	private KodGames.ClientClass.Location skillLocation;
	private int skillSoltIndex;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		skillSoltIndex = (int)userDatas[0];
		skillLocation = userDatas[1] as KodGames.ClientClass.Location;

		InitView();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		//Clear skill list.
		StopCoroutine("FillList");
		skillList.ClearList(false);
		skillList.ScrollListTo(0);
		viewMoreBtnItem = null;

		// Clear Data.
		skillLocation = null;

		emptyTip.Text = "";
	}

	private void InitView()
	{
		// Clear Data For filter Data.
		StopCoroutine("FillList");
		skillList.ClearList(false);
		skillList.ScrollListTo(0);
		viewMoreBtnItem = null;

		// Filter By FilterFunction.
		var skillFilter = PackageFilterData.Instance.GetPackgetFilterByType(PackageFilterData._DataType.SelectSkill);
		var skillQualityFilter = skillFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);

		// Avatar Assemble Data.
		var assembleIds = ItemInfoUtility.GetAvatarAssembleRequireIds(PlayerDataUtility.GetLineUpAvatar(SysLocalDataBase.Inst.LocalPlayer, skillLocation.PositionId, skillLocation.ShowLocationId));

		// Init Data.
		List<int> skillIdsEquiped = new List<int>();
		var lineUpSkills = PlayerDataUtility.GetLineUpSkills(SysLocalDataBase.Inst.LocalPlayer, skillLocation.PositionId, skillLocation.ShowLocationId);

		for (int i = 0; i < lineUpSkills.Count; i++)
			if (!skillIdsEquiped.Contains(lineUpSkills[i].ResourceId))
				skillIdsEquiped.Add(lineUpSkills[i].ResourceId);

		skillsToFillList.Clear();
		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.Skills.Count; i++)
		{



			if (skillIdsEquiped.Contains(SysLocalDataBase.Inst.LocalPlayer.Skills[i].ResourceId))
				continue;

			var skillConfig = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(SysLocalDataBase.Inst.LocalPlayer.Skills[i].ResourceId);

			if (skillConfig.type != CombatTurn._Type.PassiveSkill)
				continue;

			// 数据筛选 品质
			if (!skillQualityFilter.Contains(skillConfig.qualityLevel))
				continue;

			// Set Assemble Active value.
			SysLocalDataBase.Inst.LocalPlayer.Skills[i].IsAssembleActive = assembleIds.Contains(SysLocalDataBase.Inst.LocalPlayer.Skills[i].ResourceId);

			skillsToFillList.Add(SysLocalDataBase.Inst.LocalPlayer.Skills[i]);
		}

		skillsToFillList.Sort(DataCompare.CompareSkillForLineUp);

		if (skillsToFillList.Count > 0)
		{
			currentPosition = 0;
			StartCoroutine("FillList");
			emptyTip.Text = string.Empty;
		}
		else
		{
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Skill");
			AddGetPoolItem();
		}

		if (ItemInfoUtility.CheckSkillAllSelected(PackageFilterData._DataType.SelectSkill))
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_AllSelected");
		else
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_NoAllSelected");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		int rangeCount = Mathf.Min(cItemCountPerPage, skillsToFillList.Count - currentPosition);
		List<KodGames.ClientClass.Skill> skills = skillsToFillList.GetRange(currentPosition, rangeCount);

		foreach (KodGames.ClientClass.Skill skill in skills)
		{
			UIListItemContainer itemContainer = skillObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemSkillItem item = itemContainer.gameObject.GetComponent<UIElemSkillItem>();

			// Save item in container
			itemContainer.Data = item;

			item.SetData(skill, skillLocation.PositionId);

			// Add to list
			if (viewMoreBtnItem == null)
				skillList.AddItem(itemContainer);
			else
				skillList.InsertItem(itemContainer, skillList.Count - 1);
		}

		// Increase to next show index
		currentPosition += rangeCount;

		if (skillsToFillList.Count > currentPosition)
		{
			if (viewMoreBtnItem == null)
			{
				UIListItemContainer viewMoreContainer = viewMoreObjPool.AllocateItem().GetComponent<UIListItemContainer>();
				viewMoreBtnItem = viewMoreContainer;
				skillList.AddItem(viewMoreContainer);
			}
		}
		// Check if need to remove "more" button
		else if (viewMoreBtnItem != null)
		{
			skillList.RemoveItem(viewMoreBtnItem, false, true, false);
			viewMoreBtnItem = null;
			AddGetPoolItem();
		}
		else if (currentPosition <= skillsToFillList.Count)
		{
			AddGetPoolItem();
		}
	}

	private void AddGetPoolItem()
	{
		UIListItemContainer getContainer = getObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		skillList.InsertItem(getContainer, skillList.Count, true, "", false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnViewMoreClick(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList");
	}

	//点击图标，显示详细
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIcon(UIButton btn)
	{
		var assetIcon = btn.Data as UIElemAssetIcon;
		//var skill = assetIcon.Data as KodGames.ClientClass.Skill;
		UIElemSkillItem item = assetIcon.Data as UIElemSkillItem;

		var selectDel = new UIPnlSkillInfo.SelectDelegate(SeletSkillItemBySkill);
		SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgSkillInfo, item.Skill, false, true, false, false, selectDel, true);
	}

	//Click to return to UIPnlGuide.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFilterSkill(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPackageSkillFilter), PackageFilterData._DataType.SelectSkill, new UIDlgPackageSkillFilter.OnSelectFilterSkill(InitView));
	}

	//点击更换技能
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectBtnClick(UIButton btn)
	{
		UIElemSkillItem item = btn.data as UIElemSkillItem;
		if (null == item)
			return;
		else
			SeletSkillItemBySkill(item.Skill);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickItemDetailInfo(UIButton btn)
	{
		ItemInfoUtility.ShowLineUpSkillDesc((btn.Data as UIElemSkillItem).Skill);
	}

	public void SeletSkillItemBySkill(KodGames.ClientClass.Skill skill)
	{
		RequestMgr.Inst.Request(new ChangeLocationReq(skill.Guid, skill.ResourceId, skillLocation.Guid, skillLocation.PositionId, skillLocation.ShowLocationId, skillLocation.Index));
	}

	public void OnChangeSkillSuccess(KodGames.ClientClass.Location location)
	{
		HideSelf();

		SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlAvatar>().OnChangeSkillSuccess(location, skillSoltIndex);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetSkill(UIButton btn)
	{
		if (!(SysGameStateMachine.Instance.CurrentState is GameState_Dungeon))
			GameUtility.JumpUIPanel(_UIType.UI_Dungeon);
		else
		{
			this.HideSelf();

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
				SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatar));
		}
	}


}