using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;

public class UIPnlSelectDanList : UIPnlItemInfoBase
{
	public UIScrollList scrollList;
	public GameObjectPool objectPool;
	public GameObjectPool moreItemPool;
	public GameObjectPool getBtnObjectPool;
	public SpriteText emptyTip;
	public AutoSpriteControlBase tabControllBase;
	public UIBox selectedBox;

	private KodGames.ClientClass.Location danLocation;
	private int danType;
	private KodGames.ClientClass.Avatar avatar;

	private const int sMaxRows = 20;
	private int currentPosition = -1;
	private List<KodGames.ClientClass.Dan> dansToFillList = new List<KodGames.ClientClass.Dan>();
	private bool isMoveUpInSpecialPosition = false;
	private UIListItemContainer viewMoreBtnItem;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		selectedBox.Hide(!isMoveUpInSpecialPosition);
		this.danType = (int)userDatas[0];
		this.danLocation = userDatas[1] as KodGames.ClientClass.Location;
		this.avatar = userDatas[2] as KodGames.ClientClass.Avatar;
		InitView();

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		// Clear Data.
		ResetData();
		//danLocation = null;
		dansToFillList.Clear();
	}

	private void ResetData()
	{
		// Clear Data For Filter Data.
		StopCoroutine("FillData");
		currentPosition = 0;
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0;
		viewMoreBtnItem = null;
		emptyTip.Text = "";
	}

	private void InitView()
	{
		ClearData();

		// Init Data.
		foreach (var dan in SysLocalDataBase.Inst.LocalPlayer.Dans)
		{
			var danConfig = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(dan.ResourceId);

			// Skip by equipment type
			if (danType != danConfig.Type)
				continue;

			// Skip Current equipment.
			if (dan.Guid.Equals(danLocation.Guid))
				continue;

			dansToFillList.Add(dan);

		}
		//danLocation
		dansToFillList.Sort(CompareDanForLineUp);

		if (dansToFillList.Count > 0)
			currentPosition = 0;

		StartCoroutine("FillData");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillData()
	{
		yield return null;

		if (dansToFillList.Count > 0)
			emptyTip.Text = string.Empty;
		else
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Dan");

		int rangeCount = 0;
		List<KodGames.ClientClass.Dan> dans = new List<KodGames.ClientClass.Dan>();
		if (isMoveUpInSpecialPosition)
		{
			List<KodGames.ClientClass.Dan> positionDans = MoveUpInSpecialPosition();
			if (positionDans != null && positionDans.Count > 0)
			{
				rangeCount = Mathf.Min(sMaxRows, positionDans.Count - currentPosition);
				dans = positionDans.GetRange(currentPosition, rangeCount);
			}
		}
		else
		{
			rangeCount = Mathf.Min(sMaxRows, dansToFillList.Count - currentPosition);//40
			dans = dansToFillList.GetRange(currentPosition, rangeCount);
		}


		foreach (KodGames.ClientClass.Dan dan in dans)
		{
			UIListItemContainer container = objectPool.AllocateItem().GetComponent<UIListItemContainer>();

			UIElemDanSelectItem item = container.gameObject.GetComponent<UIElemDanSelectItem>();
			item.SetData(dan, danLocation.PositionId, !IsRecommended(dan, avatar));

			if (viewMoreBtnItem == null)
				scrollList.AddItem(container);
			else
				scrollList.InsertItem(item.container, scrollList.Count - 1);
		}

		currentPosition += rangeCount;

		if ((dansToFillList.Count > currentPosition && !isMoveUpInSpecialPosition) || (isMoveUpInSpecialPosition && MoveUpInSpecialPosition().Count > currentPosition))
		{
			if (viewMoreBtnItem == null)
			{
				UIListItemContainer viewMoreContainer = moreItemPool.AllocateItem().GetComponent<UIListItemContainer>();
				viewMoreBtnItem = viewMoreContainer;
				scrollList.AddItem(viewMoreContainer);
			}
		}
		else if (viewMoreBtnItem != null)
		{
			scrollList.RemoveItem(viewMoreBtnItem, false, true, false);
			viewMoreBtnItem = null;
			AddGetPoolItem();
		}
		else if (currentPosition <= dansToFillList.Count)
		{

			AddGetPoolItem();
		}

	}

	private void AddGetPoolItem()
	{
		UIListItemContainer getContainer = getBtnObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		scrollList.InsertItem(getContainer, scrollList.Count, true, "", false);
	}

	private List<KodGames.ClientClass.Dan> MoveUpInSpecialPosition()
	{
		List<KodGames.ClientClass.Dan> listDan = new List<KodGames.ClientClass.Dan>();
		foreach (var dan in dansToFillList)
		{
			if (!PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, danLocation.PositionId, dan.Guid, dan.ResourceId))
				listDan.Add(dan);

		}
		return listDan;
	}

	/// <summary>
	/// ：已装备＞品质＞等级＞推荐＞获得时间ID
	/// </summary>
	private int CompareDanForLineUp(KodGames.ClientClass.Dan e1, KodGames.ClientClass.Dan e2)
	{
		int lineuped1 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, e1) ? 1 : 0;
		int lineuped2 = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, e2) ? 1 : 0;

		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;

		if (e1.BreakthoughtLevel != e2.BreakthoughtLevel)
			return e2.BreakthoughtLevel - e1.BreakthoughtLevel;

		if (e1.LevelAttrib.Level != e2.LevelAttrib.Level)
			return e2.LevelAttrib.Level - e1.LevelAttrib.Level;

		lineuped1 = IsRecommended(e1, avatar) ? 1 : 0;
		lineuped2 = IsRecommended(e2, avatar) ? 1 : 0;


		if (lineuped1 != lineuped2)
			return lineuped2 - lineuped1;

		if (e1.CreateTime != e2.CreateTime)
			return (int)(e2.CreateTime - e1.CreateTime);

		return 0;
	}

	private bool IsRecommended(KodGames.ClientClass.Dan dan, KodGames.ClientClass.Avatar avatar)
	{

		for (int i = 0; i < dan.DanAttributeGroups.Count; i++)
		{
			if (dan.DanAttributeGroups[i] != null)
			{
				for (int j = 0; j < dan.DanAttributeGroups[i].DanAttributes.Count; j++)
				{
					if (dan.DanAttributeGroups[i].DanAttributes[j] != null &&
						dan.DanAttributeGroups[i].DanAttributes[j].TargetConditions != null &&
						dan.DanAttributeGroups[i].DanAttributes[j].TargetConditions.Count > 0 &&
						dan.DanAttributeGroups[i].DanAttributes[j].TargetConditions[0] != null &&
						avatar != null &&
						dan.DanAttributeGroups[i].DanAttributes[j].TargetConditions[0].IntValue == avatar.ResourceId)
						return true;
				}
			}
		}
		return false;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMoreCardShow(UIButton btn)
	{
		StopCoroutine("FillData");
		StartCoroutine("FillData");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectedClickBtn(UIButton btn)
	{
		isMoveUpInSpecialPosition = !isMoveUpInSpecialPosition;
		selectedBox.Hide(!isMoveUpInSpecialPosition);
		ResetData();
		StartCoroutine("FillData");
	}

	//Click to return to UIPnlGuide.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnDanIconClick(UIButton btn)
	{
		UIElemAssetIcon danIcon = btn.Data as UIElemAssetIcon;
		KodGames.ClientClass.Dan dan = danIcon.Data as KodGames.ClientClass.Dan;

		UIPnlDanInfo.SelectDelegate selectDelegate = new UIPnlDanInfo.SelectDelegate(SeletDanItemByDan);

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanInfo), dan, -1, false, false, false, true, true, selectDelegate);

	}

	//详细
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickExplicitDan(UIButton btn)
	{
		ItemInfoUtility.ShowLineUpDanDesc((btn.Data as UIElemDanSelectItem).Dan);
	}

	//点击更换内丹
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectBtnClick(UIButton btn)
	{
		UIElemDanSelectItem item = btn.data as UIElemDanSelectItem;
		if (null == item)
			return;
		else
			SeletDanItemByDan(item.Dan);
	}

	//点击更换内丹
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetDan(UIButton btn)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignSceneMid))
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(_UIType.UIPnlDanFurnace);
		else
			GameUtility.JumpUIPanel(_UIType.UIPnlDanFurnace);
	}

	public void OnChangeDanSuccess(KodGames.ClientClass.Location location)
	{
		HideSelf();

		SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlAvatar>().OnChangeDanSuccess(location, danType);
	}

	public void SeletDanItemByDan(KodGames.ClientClass.Dan dan)
	{
		RequestMgr.Inst.Request(new ChangeLocationReq(dan.Guid, dan.ResourceId, danLocation.Guid, danLocation.PositionId, danLocation.ShowLocationId));
	}


}