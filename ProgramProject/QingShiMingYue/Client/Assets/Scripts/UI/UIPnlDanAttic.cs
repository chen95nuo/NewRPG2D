using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlDanAttic : UIModule
{
	public UIButton lockBtn;
	public UIScrollList danList;
	public GameObjectPool danlistPool;
	public UIElemDanAtticTabItem[] tabButtons;
	public SpriteText danCountLabel;

	public GameObject gotoObj;
	public UIButton gotoBtn;
	public SpriteText gotoLabel;

	public UIBox lockSelect;
	public UIBox unLockSelect;
	public bool isLock;

	private int defaultType;
	private Dictionary<int, long> danStoreTimes = new Dictionary<int, long>();

	public void UpdateShowUI(int defaultType)
	{
		//新类型事先隐藏
		foreach (var tab in tabButtons)
		{
			tab.newBg.Hide(true);
		}
		this.defaultType = defaultType;
		ChangeTabBtn();

		gotoObj.SetActive(false);

		RequestMgr.Inst.Request(new QueryDanStoreReq(defaultType));
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlDanMenuBot>().SetLight(_UIType.UIPnlDanAttic);

		//初始化
		isLock = false;
		lockSelect.Hide(!isLock);
		unLockSelect.Hide(isLock);

		danCountLabel.Text = string.Format(GameUtility.GetUIString("UIDlgDanAttri_Count_Label"), SysLocalDataBase.Inst.LocalPlayer.Dans.Count, ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.DanMaxCount));

		tabButtons[0].tabBtn.Data = DanConfig._DanType.Sky;
		tabButtons[1].tabBtn.Data = DanConfig._DanType.Earth;
		tabButtons[2].tabBtn.Data = DanConfig._DanType.Human;
		tabButtons[3].tabBtn.Data = DanConfig._DanType.Ghost;
		tabButtons[4].tabBtn.Data = DanConfig._DanType.God;

		if (userDatas != null && userDatas.Length > 0)
			UpdateShowUI((int)userDatas[0]);
		else
			UpdateShowUI((int)tabButtons[0].tabBtn.Data);

		return true;
	}

	public void QueryDanStoreSuccess(List<com.kodgames.corgi.protocol.DanStoreQueryTime> danStoreQueryTimes)
	{		
		this.danStoreTimes.Clear();
		foreach (var danStore in danStoreQueryTimes)
		{
			this.danStoreTimes.Add(danStore.type, danStore.lastQueryTime);
		}

		ClearData();
		StartCoroutine("FillDanList");
	}

	public void ChangeLockStateSuccess(int lockType, int danpackage)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanAttic_Label_" + DanConfig._LockType.GetNameByType(lockType)));

		var item = danList.GetItem(danpackage).Data as UIElemDanPackage;
		if (item != null)
			item.ChangeLock(lockType == DanConfig._LockType.Locked);
	}

	private void ClearData()
	{
		gotoObj.gameObject.SetActive(false);

		StopCoroutine("FillDanList");
		danList.ClearList(false);
		danList.ScrollPosition = 0f;
	}

	private void ChangeTabBtn()
	{
		foreach (UIElemDanAtticTabItem tabItem in tabButtons)
			tabItem.tabBtn.controlIsEnabled = ((int)tabItem.tabBtn.Data) != defaultType;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator FillDanList()
	{
		yield return null;

		//TODO 待查如果事先准备5张列表是否节省时间?
		List<KodGames.ClientClass.Dan> playerDans = new List<KodGames.ClientClass.Dan>();		
		//已锁未锁是否其中一个存在丹标志
		bool danLockFlag = false;

		//初始化Tab新获得的丹的数量
		foreach (var tab in tabButtons)
		{
			tab.count = 0;
		}

		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.Dans.Count; i++)
		{
			for (int j = 0; j < tabButtons.Length; j++)
			{
				//页签排除当前标签
				if (SysLocalDataBase.Inst.LocalPlayer.Dans[i].Type != defaultType && SysLocalDataBase.Inst.LocalPlayer.Dans[i].Type == (int)tabButtons[j].tabBtn.Data && SysLocalDataBase.Inst.LocalPlayer.Dans[i].CreateTime > danStoreTimes[(int)tabButtons[j].tabBtn.Data])
					tabButtons[j].count++;
			}
			
			if (SysLocalDataBase.Inst.LocalPlayer.Dans[i].Type == defaultType)
			{
				if (SysLocalDataBase.Inst.LocalPlayer.Dans[i].Locked == isLock)
					playerDans.Add(SysLocalDataBase.Inst.LocalPlayer.Dans[i]);
				else
					danLockFlag = true;
			}				
		}

		for (int j = 0; j < tabButtons.Length; j++)
		{
			tabButtons[j].SetNewBg();
		}

		if (playerDans.Count <= 0)
		{
			gotoObj.SetActive(true);
			
			if (danLockFlag)
				gotoBtn.gameObject.SetActive(false);
			else
				gotoBtn.gameObject.SetActive(true);

			if(!isLock)	
				gotoLabel.Text = GameUtility.GetUIString("UIDlgDanAttri_Goto_" + DanConfig._DanType.GetNameByType(defaultType));
			else
				gotoLabel.Text = GameUtility.GetUIString("UIDlgDanAttri_Locked_" + DanConfig._DanType.GetNameByType(defaultType));
		}

		playerDans.Sort((a1, a2) =>
		{
			int isNew1 = a1.CreateTime > danStoreTimes[defaultType] ? 1 : 0;
			int isNew2 = a2.CreateTime > danStoreTimes[defaultType] ? 1 : 0;
			if (isNew1 != isNew2)
				return isNew2 - isNew1;

			int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, a1) ? 1 : 0;
			int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, a2) ? 1 : 0;

			if (lineuped1 != lineuped2)
				return lineuped2 - lineuped1;

			if (a1.BreakthoughtLevel != a2.BreakthoughtLevel)
				return (int)(a2.BreakthoughtLevel - a1.BreakthoughtLevel);

			if (a1.LevelAttrib.Level != a2.LevelAttrib.Level)
				return (int)(a2.LevelAttrib.Level - a1.LevelAttrib.Level);

			if (a1.CreateTime != a2.CreateTime)
				return (int)(a2.CreateTime - a1.CreateTime);

			return 1;
		});

		// 填充信息
		for (int i = 0; i < playerDans.Count; i++)
		{
			UIListItemContainer itemContainer = danlistPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemDanPackage item = itemContainer.GetComponent<UIElemDanPackage>();
			item.SetData(playerDans[i], danStoreTimes[defaultType], i);
			itemContainer.Data = item;
			danList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		defaultType = (int)btn.Data;
		RequestMgr.Inst.Request(new QueryDanStoreReq(defaultType));

		ChangeTabBtn();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnDanIconClick(UIButton btn)
	{
		UIElemAssetIcon danIcon = btn.Data as UIElemAssetIcon;
		KodGames.ClientClass.Dan dan = danIcon.Data as KodGames.ClientClass.Dan;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanInfo), dan, defaultType, false, true, false, false, true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnDanLockClick(UIButton btn)
	{
		var danItem = btn.Data as UIElemDanPackage;

		int lockType = 0;
		if (danItem.IsLocked)
			lockType = DanConfig._LockType.Unlock;
		else
			lockType = DanConfig._LockType.Locked;

		RequestMgr.Inst.Request(new LockDanReq(lockType, danItem.Dan.Guid, danItem.Index));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnUnLockBtnClick(UIButton btn)
	{		
		if (isLock == false)		
			return;
		else
		{
			isLock = false;
			lockSelect.Hide(!isLock);
			unLockSelect.Hide(isLock);

			ClearData();
			StartCoroutine("FillDanList");
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnLockBtnClick(UIButton btn)
	{
		if (isLock == true)
			return;
		else
		{
			isLock = true;
			lockSelect.Hide(!isLock);
			unLockSelect.Hide(isLock);

			ClearData();
			StartCoroutine("FillDanList");
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnDanHelpInfoClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanIntroduce), DanConfig._IntroduceType.DanAttic);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGotoBtnClick(UIButton btn)
	{
		GameUtility.JumpUIPanel(_UIType.UIPnlDanFurnace);
	}

	//详细
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickExplicitDan(UIButton btn)
	{
		ItemInfoUtility.ShowLineUpDanDesc((btn.Data as UIElemDanPackage).Dan);
	}
}
