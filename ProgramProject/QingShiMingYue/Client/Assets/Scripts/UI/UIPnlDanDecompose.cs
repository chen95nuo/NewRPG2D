using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlDanDecompose : UIModule
{
	public UIScrollList danList;
	public GameObjectPool danlistPool;
	public UIBox isSelectBtn;

	public SpriteText freeCountLabel;
	public SpriteText bossCountLabel;
	public SpriteText costCountLabel;

	public UIButton gotoBtn;
	public SpriteText gotoLabel;

	public UIButton[] tabButtons;
	private int defaultType;

	private List<KodGames.ClientClass.Dan> decomposeDans = new List<KodGames.ClientClass.Dan>();
	private bool isAllSelect;

	private com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo;

	private int selectCount;
	public int SelectCount
	{
		get { return selectCount; }
		set { selectCount = value; }
	}

	public SpriteText activityTimeLabel;
	public SpriteText activityTitleLabel;

	private long nextRefreshTime;
	private System.DateTime loginTime;
	private System.DateTime endTime;
	private bool isNeedRefresh;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlDanMenuBot>().SetLight(_UIType.UIPnlDanDecompose);

		if (activityTimeLabel != null && ActivityManager.Instance.GetActivity<ActivityDecompose>() != null)
			activityTimeLabel.Text = ActivityManager.Instance.GetActivity<ActivityDecompose>().GetActivityTime();
		
		//Tab标签品质初始化
		for (int i = 0; i < tabButtons.Length; i++)
		{
			tabButtons[i].Data = i + 1;
		}

		defaultType = (int)tabButtons[0].Data;
		selectCount = 0;

	    decomposeInfo = userDatas[0] as com.kodgames.corgi.protocol.DecomposeInfo;
		nextRefreshTime = (long)userDatas[1];
		
		//刷新标志初始化
		isNeedRefresh = true;

		ShowTextCount();
		ChangeTabBtn();

		ClearData();
		StartCoroutine("FillDanList");

		return true;
	}

	private void Update()
	{
		loginTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
		endTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(nextRefreshTime);

		if (loginTime > endTime && isNeedRefresh)
		{
			RequestMgr.Inst.Request(new QueryAlchemyReq());
			isNeedRefresh = false;
		}
	}

	private void ClearData()
	{
		isAllSelect = false;
		isSelectBtn.Hide(!isAllSelect);
		selectCount = 0;
		decomposeDans.Clear();
		gotoBtn.gameObject.SetActive(false);

		StopCoroutine("FillDanList");
		danList.ClearList(false);
		danList.ScrollPosition = 0f;
	}

	private void ShowTextCount()
	{
		if (decomposeInfo.danDecomposeCout > 0)
		{
			freeCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanDecompose_Free_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, decomposeInfo.danDecomposeCout - selectCount, decomposeInfo.maxDanDecomposeCout, GameDefines.textColorBtnYellow, selectCount);
			costCountLabel.Text = "";
			bossCountLabel.Text = "";
		}
		else
		{
			freeCountLabel.Text = "";
			costCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanDecompose_Cost_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, decomposeInfo.danItemDecomposeCount - selectCount, decomposeInfo.maxDanItemDecomposeCount, GameDefines.textColorBtnYellow, selectCount);

			int bossItemCount = ItemInfoUtility.GetGameItemCount(decomposeInfo.decomposeCost.id);

			bossCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanDecompose_Boss_Label"), GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(decomposeInfo.decomposeCost.id), GameDefines.textColorWhite, selectCount * decomposeInfo.decomposeCost.count, bossItemCount);
		}
	}

	public void DecomposeSuccess(com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
	{
		this.decomposeInfo = decomposeInfo;
		selectCount = 0;

		ShowTextCount();
		ClearData();
		StartCoroutine("FillDanList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator FillDanList()
	{
		yield return null;

		List<KodGames.ClientClass.Dan> playerDans = new List<KodGames.ClientClass.Dan>();

		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.Dans.Count; i++)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.Dans[i].BreakthoughtLevel == defaultType && !SysLocalDataBase.Inst.LocalPlayer.Dans[i].Locked && !PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.Dans[i]))
				playerDans.Add(SysLocalDataBase.Inst.LocalPlayer.Dans[i]);
		}

		if (playerDans.Count <= 0)
		{
			gotoBtn.gameObject.SetActive(true);
			gotoLabel.Text = GameUtility.GetUIString("UIPnlDanDecompose_Goto_Label");
		}
		
		playerDans.Sort((a1, a2) =>
		{
			return (int)(a1.LevelAttrib.Level - a2.LevelAttrib.Level);
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

	private void ChangeTabBtn()
	{
		foreach (UIButton btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.Data) != defaultType;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnAllSelectClick(UIButton btn)
	{
		isAllSelect = !isAllSelect;

		decomposeDans.Clear();
		selectCount = 0;

		for (int index = 0; index < danList.Count; index++)
		{
			var item = danList.GetItem(index).Data as UIElemDanDecomposeItem;

			if (isAllSelect)
			{
				if(decomposeInfo.danDecomposeCout <= 0 && decomposeInfo.danItemDecomposeCount <= 0)
				{
					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanDecompose_MaxCount_Label"));
					break;
				}
				if (decomposeInfo.danDecomposeCout > 0 && selectCount >= decomposeInfo.danDecomposeCout)
					break;
				if (decomposeInfo.danDecomposeCout <= 0 && decomposeInfo.danItemDecomposeCount > 0 && selectCount >= decomposeInfo.danItemDecomposeCount)
					break;
				selectCount++;
				decomposeDans.Add(item.Dan);
			}

			item.SetSelect(isAllSelect);
		}

		if(selectCount > 0 && selectCount < danList.Count)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), string.Format(GameUtility.GetUIString("UIPnlDanDecompose_AllCheckTips_Label"),selectCount));
		}

		ShowTextCount();
		isSelectBtn.Hide(!isAllSelect);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnTabButtonClick(UIButton btn)
	{
		defaultType = (int)btn.Data;
		ChangeTabBtn();

		ClearData();
		StartCoroutine("FillDanList");

		ShowTextCount();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnDanSelectClick(UIButton btn)
	{		
		var item = btn.Data as UIElemDanDecomposeItem;
		bool countError = (decomposeInfo.danDecomposeCout > 0 && selectCount >= decomposeInfo.danDecomposeCout || decomposeInfo.danDecomposeCout <= 0 && selectCount >= decomposeInfo.danItemDecomposeCount);

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
		if(selectCount <= 0)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanDecompose_NoCount_Label"));
			return;
		}

		KodGames.ClientClass.Cost cost = new KodGames.ClientClass.Cost();
		cost.Id = decomposeInfo.decomposeCost.id;
		cost.Count = decomposeInfo.decomposeCost.count * selectCount;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanDecomposeSure), decomposeInfo.danDecomposeCout, decomposeInfo.danItemDecomposeCount, decomposeDans, cost);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnGotoBtnClick(UIButton btn)
	{
		GameUtility.JumpUIPanel(_UIType.UIPnlDanFurnace);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnDanHelpInfoClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanIntroduce), DanConfig._IntroduceType.DanDecompose);
	}
}
