using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlDanOneKeyDecompose : UIModule
{
	public UIScrollList danList;
	public GameObjectPool danlistPool;
	public UIBox isSelectBtn;

	public SpriteText freeCountLabel;
	public SpriteText bossCountLabel;
	public SpriteText costCountLabel;

	private List<KodGames.ClientClass.Dan> decomposeDans = new List<KodGames.ClientClass.Dan>();
	private List<KodGames.ClientClass.Dan> playerDans = new List<KodGames.ClientClass.Dan>();

	private bool isAllSelect;

	private int freeCount;
	private int costCount;

	private int maxCount;
	private int maxItemCount;
	private int selectCount;

	com.kodgames.corgi.protocol.Cost decomposeCost;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		playerDans = userDatas[0] as List<KodGames.ClientClass.Dan>;

		if(SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanMenuBot)))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlDanMenuBot);			

		var decomposeInfo = userDatas[1] as com.kodgames.corgi.protocol.DecomposeInfo;

		freeCount = decomposeInfo.danDecomposeCout;
		costCount = decomposeInfo.danItemDecomposeCount;
		decomposeCost = decomposeInfo.decomposeCost;
		maxCount = decomposeInfo.maxDanDecomposeCout;
		maxItemCount = decomposeInfo.maxDanItemDecomposeCount;

		ShowTextCount();		

		ClearData();
		StartCoroutine("FillDanList");

		return true;
	}

	private void ClearData()
	{
		isAllSelect = false;
		isSelectBtn.Hide(!isAllSelect);
		selectCount = 0;
		decomposeDans.Clear();

		StopCoroutine("FillDanList");
		danList.ClearList(false);
		danList.ScrollPosition = 0f;
	}

	private void ShowTextCount()
	{
		if (freeCount > 0)
		{
			freeCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanDecompose_Free_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, freeCount, maxCount, GameDefines.textColorBtnYellow, selectCount);
			costCountLabel.Text = "";
			bossCountLabel.Text = "";
		}
		else
		{
			freeCountLabel.Text = "";
			costCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanDecompose_Cost_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, costCount, maxItemCount, GameDefines.textColorBtnYellow, selectCount);

			int bossItemCount = ItemInfoUtility.GetGameItemCount(decomposeCost.id);

			bossCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanDecompose_Boss_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, ItemInfoUtility.GetAssetName(decomposeCost.id), selectCount * decomposeCost.count, bossItemCount);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator FillDanList()
	{
		yield return null;

		playerDans.Sort((a1, a2) =>
		{
			if (a1.BreakthoughtLevel != a2.BreakthoughtLevel)
				return a1.BreakthoughtLevel - a2.BreakthoughtLevel;

			if (a1.LevelAttrib.Level != a2.LevelAttrib.Level)
				return a1.LevelAttrib.Level - a2.LevelAttrib.Level;

			return 1;
		});

		// 填充信息
		for (int i = 0; i < playerDans.Count; i++)
		{
			UIListItemContainer itemContainer = danlistPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemDanDecomposeItem item = itemContainer.GetComponent<UIElemDanDecomposeItem>();
			item.SetData(playerDans[i]);
			itemContainer.Data = item;
			danList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAllSelectClick(UIButton btn)
	{
		isAllSelect = !isAllSelect;

		decomposeDans.Clear();
		selectCount = 0;

		for (int index = 0; index < danList.Count; index++)
		{
			var item = danList.GetItem(index).Data as UIElemDanDecomposeItem;

			if (isAllSelect)
			{
				if (freeCount <= 0 && costCount <= 0)
				{
					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanDecompose_MaxCount_Label"));
					break;
				}
				if (freeCount > 0 && selectCount >= freeCount)
					break;
				if (freeCount <= 0 && costCount > 0 && selectCount >= costCount)
					break;
				selectCount++;
				decomposeDans.Add(item.Dan);
			}
			else
			{
				decomposeDans.Clear();
				selectCount = 0;
			}

			item.SetSelect(isAllSelect);
		}

		if (selectCount > 0 && selectCount < danList.Count)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), string.Format(GameUtility.GetUIString("UIPnlDanDecompose_AllCheckTips_Label"), selectCount));
		}

		ShowTextCount();
		isSelectBtn.Hide(!isAllSelect);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnDanSelectClick(UIButton btn)
	{
		var item = btn.Data as UIElemDanDecomposeItem;
		bool countError = (freeCount > 0 && selectCount >= freeCount || freeCount <= 0 && selectCount >= costCount);

		if (countError && !item.IsSelect)
		{
			//分解数量不足
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanDecompose_MaxCount_Label"));
			return;
		}

		if (item.SetLight())
		{
			selectCount++;
			decomposeDans.Add(item.Dan);
		}
		else
		{
			selectCount--;
			decomposeDans.Remove(item.Dan);
		}

		ShowTextCount();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnDecomposeClick(UIButton btn)
	{
		if (selectCount <= 0)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanDecompose_NoCount_Label"));
			return;
		}

		KodGames.ClientClass.Cost cost = new KodGames.ClientClass.Cost();
		cost.Id = decomposeCost.id;
		cost.Count = decomposeCost.count * selectCount;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanDecomposeSure), freeCount, costCount, decomposeDans, cost);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}
}
