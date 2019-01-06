using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIDlgItemGetWay : UIModule
{
	public UIElemGetWayTabItem itemIcon;
	public List<UIElemGetWayTabItem> suites;
	public UIScrollList itemGetWayList;
	public GameObjectPool itemObjectPool;

	public SkillConfig skill;
	public EquipmentConfig equipment;
	public AvatarConfig avatar;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		int resourceId = (int)userDatas[0];

		//Set Enter ItemIcon
		itemIcon.SetData(resourceId);
		itemIcon.assembleIcon.Data = resourceId;
		itemIcon.SetSelectedStat(true);

		ShowSuiteList(resourceId);
		FillData(resourceId);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillItemList");
		StopCoroutine("FillSuiteItemList");

		for (int index = 0; index < itemGetWayList.Count; index++)
			(itemGetWayList.GetItem(index).Data as UIElemGetWayItem).ReleaseGotoItem();

		itemGetWayList.ClearList(false);
		itemGetWayList.ScrollPosition = 0f;
	}

	private void FillData(int resourceId)
	{
		ClearData();
		StartCoroutine("FillItemList", resourceId);
	}

	private void ShowSuiteList(int resourceId)
	{
		for (int i = 0; i < suites.Count; i++)
		{
			suites[i].assembleIcon.Hide(true);
			suites[i].SetSelectedStat(false);
		}

		List<SuiteConfig.AssembleSetting> suiteSettings = new List<SuiteConfig.AssembleSetting>();
		if (IDSeg.ToAssetType(resourceId) == IDSeg._AssetType.Avatar)
			suiteSettings = ConfigDatabase.DefaultCfg.SuiteConfig.GetAvatarAssembleByAvatarId(resourceId);
		else
			suiteSettings = ConfigDatabase.DefaultCfg.SuiteConfig.GetEquipmentSuitesByRequireId(resourceId);

		if (suiteSettings == null)
			return;

		for (int i = 0; i < suites.Count && i < suiteSettings.Count; i++)
		{
			suites[i].assembleIcon.Hide(false);
			suites[i].SetSelectedStat(false);
			suites[i].assembleIcon.border.Text = suiteSettings[i].Name;
			suites[i].assembleIcon.Data = suiteSettings[i];
		}
	}

	private void FillAvatarGetList(SuiteConfig.AssembleSetting assemble)
	{
		ClearData();
		StartCoroutine("FillSuiteItemList", assemble);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillItemList(int resourceId)
	{
		yield return null;

		List<GetWay> getWays = null;

		switch (IDSeg.ToAssetType(resourceId))
		{
			case IDSeg._AssetType.Avatar:
				getWays = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(resourceId).getways;
				break;

			case IDSeg._AssetType.Equipment:
				getWays = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(resourceId).getways;
				break;

			case IDSeg._AssetType.CombatTurn:
				getWays = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(resourceId).getways;
				break;
		}

		UIListItemContainer uiContainer = itemObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemGetWayItem item = uiContainer.GetComponent<UIElemGetWayItem>();
		uiContainer.data = item;
		item.SetData(resourceId, getWays);
		uiContainer.ScanChildren();
		itemGetWayList.AddItem(item.gameObject);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillSuiteItemList(SuiteConfig.AssembleSetting assemble)
	{
		yield return null;

		foreach (var part in assemble.Parts)
		{
			foreach (var require in part.Requiremets)
			{
				List<GetWay> getWays = null;
				switch (require.Type)
				{
					case SuiteConfig.AssembleSetting.Requirement._Type.Avatar:
						{
							AvatarConfig.Avatar avatar = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(require.Value);
							getWays = avatar.getways;
						}
						break;
					case SuiteConfig.AssembleSetting.Requirement._Type.Equipment:
						{
							EquipmentConfig.Equipment equip = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(require.Value);
							getWays = equip.getways;
						}
						break;
					case SuiteConfig.AssembleSetting.Requirement._Type.CombatTurn:
						{
							SkillConfig.Skill skill = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(require.Value);
							getWays = skill.getways;
						}
						break;
				}

				UIListItemContainer uiSuiteContainer = itemObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemGetWayItem item = uiSuiteContainer.GetComponent<UIElemGetWayItem>();
				uiSuiteContainer.data = item;
				item.SetData(require.Value, getWays, assemble);
				uiSuiteContainer.ScanChildren();
				itemGetWayList.AddItem(item.gameObject);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoBtn(UIButton btn)
	{
		ClientServerCommon.GetWay getway = (ClientServerCommon.GetWay)btn.data;
		if (getway.type != _UIType.UI_ActivityDungeon && getway.type != _UIType.UI_Dungeon)
			SysUIEnv.Instance.ShowUIModule(getway.type);
		else
			GameUtility.JumpUIPanel(getway.type, getway.data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetWayTab(UIButton btn)
	{
		UIElemAssetIcon itemBtn = btn.Data as UIElemAssetIcon;
		itemIcon.SetSelectedStat(false);

		//SelectLight Control
		if (itemBtn.Data is SuiteConfig.AssembleSetting)
		{
			SuiteConfig.AssembleSetting assemble = itemBtn.Data as SuiteConfig.AssembleSetting;
			for (int i = 0; i < suites.Count; i++)
			{
				if (suites[i].assembleIcon.Data is SuiteConfig.AssembleSetting)
				{
					SuiteConfig.AssembleSetting suiteAssemble = suites[i].assembleIcon.Data as SuiteConfig.AssembleSetting;
					if (suiteAssemble.Id == assemble.Id)
						suites[i].SetSelectedStat(true);
					else
						suites[i].SetSelectedStat(false);
				}
				else suites[i].SetSelectedStat(false);
			}
			FillAvatarGetList(assemble);
		}
		else
		{
			itemIcon.SetSelectedStat(true);
			for (int i = 0; i < suites.Count; i++)
				suites[i].SetSelectedStat(false);

			FillData((int)itemBtn.Data);
		}
	}
}

