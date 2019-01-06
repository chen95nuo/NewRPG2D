using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlOrganChipsTab : UIModule
{
	public SpriteText chipDescLabel;
	public SpriteText chipNameLabel;
	public SpriteText chipsCountLabel;	
	public GameObjectPool chipsPool;
	public UIScrollList chipsList;
	public UIElemAssetIcon chipIcon;
	public AutoSpriteControlBase itemBg;

	public SpriteText noChipLabel;

	private int defaultId = 0;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlOrganTab>().ChangeTabButtons(_UIType.UIPnlOrganChipsTab);

		Init();

		return true;
	}

	private void Init()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	private void ClearData()
	{		
		StopCoroutine("FillList");
		chipsList.ClearList(false);
		chipsList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		List<KodGames.ClientClass.Consumable> chips = new List<KodGames.ClientClass.Consumable>();

		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.Consumables.Count; i++)
		{
			if (ItemConfig._Type.ToItemType(SysLocalDataBase.Inst.LocalPlayer.Consumables[i].Id) == ItemConfig._Type.BeastPart)
				chips.Add(SysLocalDataBase.Inst.LocalPlayer.Consumables[i]);
		}

		chips.Sort(DataCompare.CompareBeastPart);

		if (chips.Count > 0)
		{
			itemBg.gameObject.SetActive(true);
			defaultId = chips[0].Id;
			SetShowChipInfo();
		}
		else
		{
			itemBg.gameObject.SetActive(false);
			noChipLabel.Text = GameUtility.GetUIString("UIOrganChip_NoChipTips_Label");
		}
	
		int rowCount = 5;
		
		for (int i = 0; i <= chips.Count / rowCount; i++)
		{
			List<KodGames.ClientClass.Consumable> fillChips = new List<KodGames.ClientClass.Consumable>();

			for (int j = 0; j < rowCount && i * rowCount + j < chips.Count; j++)
			{
				fillChips.Add(chips[i * rowCount + j]);
			}

			if(fillChips.Count > 0)
			{
				UIListItemContainer uiContainer = chipsPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemOrganChips item = uiContainer.GetComponent<UIElemOrganChips>();
				uiContainer.data = item;
				item.SetData(fillChips);
				chipsList.AddItem(item.gameObject);
			}
		}

		SetLight();
	}

	public void SetLight()
	{
		for (int i = 0; i < chipsList.Count; i++ )
		{
			UIListItemContainer container = chipsList.GetItem(i) as UIListItemContainer;
			UIElemOrganChips chipsElem = (UIElemOrganChips)container.Data;
			chipsElem.SetLight(defaultId);
		}
	}

	public void SetShowChipInfo()
	{
		if(defaultId != 0)
		{
			chipIcon.SetData(defaultId);
			chipNameLabel.Text = ItemInfoUtility.GetAssetName(defaultId);
			chipDescLabel.Text = ItemInfoUtility.GetAssetDesc(defaultId);

			int partCount = 0;
			var partBeast = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(defaultId);
			if (partBeast != null)
			{
				partCount = partBeast.Amount;
			}

			chipsCountLabel.Text = string.Format(GameUtility.GetUIString("UIOrganChip_NumberCount"), partCount);
		}
		else
		{
			chipNameLabel.Text = "";
			chipDescLabel.Text = "";
			chipsCountLabel.Text = "";
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChipInfoBtnClick(UIButton btn)
	{		
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrganChipInfo), defaultId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChipIconClick(UIButton btn)
	{
		defaultId = (int)btn.Data;
		SetShowChipInfo();
		SetLight();
	}
}
