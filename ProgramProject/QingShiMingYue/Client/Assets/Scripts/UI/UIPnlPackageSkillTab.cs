using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlPackageSkillTab : UIPnlPackageBase
{
	public UIScrollList skillList;
	public GameObjectPool skillObjectPool;
	public GameObjectPool getObjectPool;
	public GameObjectPool topItemPool;
	public GameObjectPool bottomItemPool;
	public SpriteText emptyTip;
	public SpriteText selectedText;

	List<KodGames.ClientClass.Skill> skills = new List<KodGames.ClientClass.Skill>();

	private int maxRowsInPage;
	private int maxRows;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		maxRowsInPage = ConfigDatabase.DefaultCfg.GameConfig.uiShowSetting.piecePageCount;
		maxRows = ConfigDatabase.DefaultCfg.GameConfig.uiShowSetting.maxCountItemInUI;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlPackageTab>().ChangeTabButtons(_UIType.UIPnlPackageSkillTab);

		InitUI();
		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public void RefreshView(string skillGuid)
	{
		KodGames.ClientClass.Player player = SysLocalDataBase.Inst.LocalPlayer;

		List<UIListItemContainer> containers = new List<UIListItemContainer>();

		for (int index = 0; index < skillList.Count; index++)
		{
			UIListItemContainer container = skillList.GetItem(index) as UIListItemContainer;

			if (container.Data != null && container.Data is UIElemPackageSkillItem)
			{
				UIElemPackageSkillItem item = container.Data as UIElemPackageSkillItem;
				if (item.Skill.Guid.Equals(skillGuid))
					item.SetData(item.Skill);
				else if (player.SearchSkill(item.Skill.Guid) == null)
					containers.Add(container);
			}
		}

		List<KodGames.ClientClass.Skill> removedSkills = new List<KodGames.ClientClass.Skill>();
		for (int i = 0; i < skills.Count; ++i)
		{
			if (player.SearchSkill(skills[i].Guid) == null)
				removedSkills.Add(skills[i]);
		}

		for (int i = 0; i < removedSkills.Count; i++)
		{
			skills.Remove(removedSkills[i]);
		}

		foreach (var container in containers)
			skillList.RemoveItem(container, false);

	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		skillList.ClearList(false);
		skillList.ScrollPosition = 0f;
		skills.Clear();
		emptyTip.Text = "";
	}

	private void InitUI()
	{
		ClearData();
		InitData();
		StartCoroutine("FillList", false);
	}

	protected override void InitData()
	{
		base.InitData();

		var skillFilter = PackageFilterData.Instance.GetPackgetFilterByType(PackageFilterData._DataType.PackageSkill);
		var skillQualityFilter = skillFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);

		if (SysLocalDataBase.Inst.LocalPlayer.Skills.Count > 0)
			SysLocalDataBase.Inst.LocalPlayer.Skills.Sort(DataCompare.CompareSkill);

		foreach (var skill in SysLocalDataBase.Inst.LocalPlayer.Skills)
		{
			var skillConfig = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);
			if (skillConfig.type != CombatTurn._Type.PassiveSkill)
				continue;

			// 数据筛选 品质
			if (!skillQualityFilter.Contains(skillConfig.qualityLevel))
				continue;

			skills.Add(skill);
		}

		if (skills.Count > 0)
			skills.Sort(DataCompare.CompareSkill);
		// 设置空信息提示
		if (skills.Count > 0)
		{
			emptyTip.Text = string.Empty;
		}
		else
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Skill");

		if (ItemInfoUtility.CheckSkillAllSelected(PackageFilterData._DataType.PackageSkill))
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_AllSelected");
		else
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_NoAllSelected");
	}


	#region MoreButton Controll

	private int GetSkillListItemCount()
	{
		if (skillList.Count <= 0)
			return 0;

		int count = skillList.Count;
		if (skillList.GetItem(0).Data == null)
			count--;

		if (skillList.Count <= 1)
			return count;

		if (skillList.GetItem(skillList.Count - 1).Data == null)
			count--;

		return count;
	}

	private UIElemPackageItemBase GetFirstSkillListItem(bool headNotTail)
	{
		if (headNotTail)
		{
			if (skillList.Count <= 0)
				return null;

			if (skillList.GetItem(0).Data != null)
				return skillList.GetItem(0).Data as UIElemPackageItemBase;

			if (skillList.Count <= 1)
				return null;

			if (skillList.GetItem(1).Data is UIElemPackageItemBase)
				return skillList.GetItem(1).Data as UIElemPackageItemBase;
			else
				return null;
		}
		else
		{
			if (skillList.Count <= 0)
				return null;

			if (skillList.GetItem(skillList.Count - 1).Data != null)
				return skillList.GetItem(skillList.Count - 1).Data as UIElemPackageItemBase;

			if (skillList.Count <= 1)
				return null;

			if (skillList.GetItem(skillList.Count - 2).Data is UIElemPackageItemBase)
				return skillList.GetItem(skillList.Count - 2).Data as UIElemPackageItemBase;
			else
				return null;
		}
	}

	private bool HasShowMoreButton(bool headNotTail)
	{
		if (skillList.Count == 0)
			return false;

		if (headNotTail)
			return skillList.GetItem(0).Data == null;
		else
			return skillList.GetItem(skillList.Count - 1).Data == null;
	}

	private void AddShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (HasShowMoreButton(true))
				return;

			UIListItemContainer viewMoreContainer = topItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			skillList.InsertItem(viewMoreContainer, 0, false, "", false);
		}
		else
		{
			if (HasShowMoreButton(false))
				skillList.RemoveItem(skillList.Count - 1, false, true, false);

			UIListItemContainer viewMoreContainer = bottomItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			skillList.InsertItem(viewMoreContainer, skillList.Count, true, "", false);
		}
	}

	private void RemoveShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (HasShowMoreButton(true) == false)
				return;

			skillList.RemoveItem(0, false, false, false);
		}
		else
		{
			UIListItemContainer getContainer = getObjectPool.AllocateItem().GetComponent<UIListItemContainer>();

			if (HasShowMoreButton(false) == false)
			{
				// ��� ȥ�������װ�� ��ť
				skillList.InsertItem(getContainer, skillList.Count, true, "", false);
				return;
			}

			skillList.RemoveItem(skillList.Count - 1, false, true, false);
			// ��� ȥ�������װ�� ��ť
			skillList.InsertItem(getContainer, skillList.Count, true, "", false);
		}
	}

	private void UpdateShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (skills.Count == 0)
				// û��Ҫ���Ľ�ɫ��
				RemoveShowMoreButton(true);
			else
			{
				var item = GetFirstSkillListItem(true);
				Debug.Assert(item != null);
				if (item.indexInList > 0)
					AddShowMoreButton(true);
				else
					RemoveShowMoreButton(true);
			}
		}
		else
		{
			if (skills.Count == 0)
				// û��Ҫ���Ľ�ɫ��
				RemoveShowMoreButton(false);
			else
			{
				var item = GetFirstSkillListItem(false);
				Debug.Assert(item != null);
				if (item.indexInList < skills.Count - 1)
					AddShowMoreButton(false);
				else
					RemoveShowMoreButton(false);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(bool headNotTail)
	{
		yield return null;

		if (headNotTail)
		{
			// �������һ��Item
			var firstItem = GetFirstSkillListItem(true);

			// ���û�У��ӵ�һ����ʼ����, ����ӵ�һ����ǰһ����ʼ����
			int firstIndex = firstItem == null ? 0 : firstItem.indexInList - 1; ;

			int firstStartindex = firstIndex;
			for (; firstStartindex > firstIndex - maxRowsInPage && firstStartindex >= 0; firstStartindex--)
			{
				UIListItemContainer firstContainer = null;

				firstContainer = skillObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemPackageSkillItem item = firstContainer.gameObject.GetComponent<UIElemPackageSkillItem>();

				item.SetData(skills[firstStartindex]);
				item.indexInList = firstStartindex;

				// ��ǰ����
				if (HasShowMoreButton(true) == false)
					skillList.InsertItem(firstContainer, 0, false, "", false);
				else
					skillList.InsertItem(firstContainer, 1, false, "", false);
			}

			UpdateShowMoreButton(true);
		}
		else
		{
			// �������һ��Item
			var lastItem = GetFirstSkillListItem(false);

			// ���û�У��ӵ�һ����ʼ����, ��������һ������һ����ʼ����
			int lastIndex = lastItem == null ? 0 : lastItem.indexInList + 1;
			int lastStartindex = lastIndex;
			for (; lastStartindex < lastIndex + maxRowsInPage && lastStartindex < skills.Count; lastStartindex++)
			{
				UIListItemContainer lastContainer = null;
				lastContainer = skillObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemPackageSkillItem item = lastContainer.gameObject.GetComponent<UIElemPackageSkillItem>();
				item.SetData(skills[lastStartindex]);
				item.indexInList = lastStartindex;


				// If "view more" button is in the list, insert before it
				if (HasShowMoreButton(false) == false)
					skillList.InsertItem(lastContainer, skillList.Count, true, "", false);
				else
					skillList.InsertItem(lastContainer, skillList.Count - 1, true, "", false);
			}

			UpdateShowMoreButton(false);
		}

		int avatarItemCount = GetSkillListItemCount();
		if (avatarItemCount > maxRows)
		{
			bool hasHeadShowMore = HasShowMoreButton(true);

			if (headNotTail)
			{
				// �ӿ�ʼɾ��������ʾ������item
				for (int headIndex = 0; headIndex < avatarItemCount - maxRows; ++headIndex)
				{
					int headRemoveIndex = hasHeadShowMore ? maxRows + 1 : maxRows;
					skillList.RemoveItem(headRemoveIndex, false, true, false);
				}

				UpdateShowMoreButton(false);
			}
			else
			{
				// �ӿ�ʼɾ��������ʾ������item
				for (int tailIndex = avatarItemCount - maxRows - 1; tailIndex >= 0; --tailIndex)
				{
					int tailRemoveIndex = hasHeadShowMore ? tailIndex + 1 : tailIndex;
					skillList.RemoveItem(tailRemoveIndex, false, false, false);
				}

				UpdateShowMoreButton(true);
			}
		}
	}
	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPrePageClick(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList", true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnNextPageClick(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList", false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSkillIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgSkillInfo, assetIcon.Data as KodGames.ClientClass.Skill, false, true, false, true, null, true, 0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSkillLevelUp(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlSkillPowerUp, btn.Data as KodGames.ClientClass.Skill);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickExplicitEquip(UIButton btn)
	{
		ItemInfoUtility.ShowLineUpSkillDesc(btn.Data as KodGames.ClientClass.Skill);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFilterSkill(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPackageSkillFilter), PackageFilterData._DataType.PackageSkill, new UIDlgPackageSkillFilter.OnSelectFilterSkill(InitUI));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSell(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlPackageSell, _UIType.UIPnlPackageSkillTab);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetSkill(UIButton btn)
	{
		GameUtility.JumpUIPanel(_UIType.UI_Dungeon);
	}


}
