using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlDanMaterial : UIModule
{	
	public UIScrollList materialList;
	public GameObjectPool materialPool;
	public UIButton[] tabButtons;

	public UIButton gotoBtn;
	public SpriteText gotoLabel;

	private int defaultType;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlDanMenuBot>().SetLight(_UIType.UIPnlDanMaterial);

		tabButtons[0].Data = ItemConfig._Type.DanLevelUpMaterial;					 //升级
		tabButtons[1].Data = ItemConfig._Type.DanBreakthoughtMaterial;			 //升阶
		tabButtons[2].Data = ItemConfig._Type.DanAttributeRefreshMaterial;		 //洗练

		defaultType = (int)tabButtons[0].Data;

		ChangeTabBtn();

		ClearData();
		StartCoroutine("FillDanList");

		return true;
	}

	private void ClearData()
	{
		gotoBtn.gameObject.SetActive(false);

		StopCoroutine("FillDanList");
		materialList.ClearList(false);
		materialList.ScrollPosition = 0f;
	}

	private void ChangeTabBtn()
	{
		foreach (UIButton btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.Data) != defaultType;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator FillDanList()
	{
		yield return null;
		
		//TODO 待查如果事先准备5张列表是否节省时间?
		List<KodGames.ClientClass.Consumable> playerMaterials = new List<KodGames.ClientClass.Consumable>();

		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.Consumables.Count; i++)
		{
			if (ItemConfig._Type.ToItemType(SysLocalDataBase.Inst.LocalPlayer.Consumables[i].Id)== defaultType)
				playerMaterials.Add(SysLocalDataBase.Inst.LocalPlayer.Consumables[i]);
		}

		if (playerMaterials.Count <= 0)
		{
			gotoBtn.gameObject.SetActive(true);
			gotoLabel.Text = GameUtility.GetUIString("UIPnlDanMaterial_Label_" + ItemConfig._Type.GetNameByType(defaultType));
		}

		playerMaterials.Sort(DataCompare.CompareConsumable);

		// 填充信息
		for (int i = 0; i < playerMaterials.Count; i++)
		{
			UIElemDanMaterial item = materialPool.AllocateItem().GetComponent<UIElemDanMaterial>();
			item.SetData(playerMaterials[i]);
			materialList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		defaultType = (int)btn.Data;
		ChangeTabBtn();

		ClearData();
		StartCoroutine("FillDanList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnDanIconClick(UIButton btn)
	{
		UIElemAssetIcon materialIcon = btn.Data as UIElemAssetIcon;
		KodGames.ClientClass.Consumable material = materialIcon.Data as KodGames.ClientClass.Consumable;		
		GameUtility.ShowAssetInfoUI(material.Id);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGotoBtnClick(UIButton btn)
	{
		GameUtility.JumpUIPanel(_UIType.UIPnlDanDecompose);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnDanHelpInfoClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanIntroduce), DanConfig._IntroduceType.DanMaterial);
	}
}
