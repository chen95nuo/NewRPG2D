using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using System;

using Dan = KodGames.ClientClass.Dan;

public class UIPnlDanCulture : UIModule
{
	public SpriteText titleText;
	public SpriteText titleContextText;
	public SpriteText danLevelUpContext;
	public List<UIButton> selectedsBtn;
	public SpriteText upBtnText;

	public UIElemAssetIcon danIcon;
	public SpriteText danLevelText;

	public List<UIBox> danAttributes;
	public UIScrollList levelUpMaterialList;
	public GameObjectPool poolGameObject;
	public UIElemAssetIcon moneyIcon;
	public SpriteText moneyText;
	public SpriteText moneyNameText;
	public UIBox fullBox;

	public GameObject maxLevelObject;
	public GameObject noMaxLevelObject;
	public Transform levelUpAniPoint;
	public Transform arrowUpAniPoint;
	public UIBox notifIcon;

	public GameObject defendTouch;
	//admintion
	public GameObject UIFX_Q_LevelUpSuccess; //升级成功
	public GameObject UIFX_Q_LevelUpFailed;//升级失败
	public GameObject UIFX_Q_LevelUpSuccess_Grade;//升阶成功
	public GameObject UIFX_Q_LevelUpFailed_Grade;//升阶失败
	public GameObject UIFX_Q_LevelUpSuccess_Perfect;//完美升阶
	public GameObject UIFX_Q_ArrowUp;//绿色向上箭头
	public GameObject UIFX_Q_ArrowDown;//红色向下箭头
	public GameObject UIFX_Q_TextChange;//洗炼

	private Dan dan = new Dan();
	public Dan Dan
	{
		get { return dan; }
	}

	private DanCultureUIType danCultureUIType = DanCultureUIType.None;
	private const int MAXLEVEL = 9;
	private int minBreakthoughtLevel = 0;

	private List<int> selectedGUIDList = new List<int>();
	private int selectedCount = 0;
	private float animationTimer = 1.5f;

	private float danPower;
	private float positionPower;

	private void ClearData()
	{
		levelUpMaterialList.ClearList(false);
		minBreakthoughtLevel = 0;
		danCultureUIType = DanCultureUIType.None;
		selectedGUIDList.Clear();
		selectedCount = 0;
		ResetSelectedListData();
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		danPower = 0f;
		positionPower = 0f;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().SetIsOverlaySetPower();

		if (userDatas != null && userDatas.Length > 0)
		{
			dan = userDatas[0] as Dan;
			UpdateUI((DanCultureUIType)userDatas[1]);
		}

		return true;
	}

	public void UpdateUI(DanCultureUIType danCultureUIType)
	{
		ResetSelectedListData();
		this.danCultureUIType = danCultureUIType;
		//SetShowUI();
		StartCoroutine("SetShowUI", 0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator SetShowUI(float timer)
	{
		yield return new WaitForSeconds(timer);
		defendTouch.SetActive(false);
		SetCultureForType();
		danIcon.SetData(dan.ResourceId, dan.BreakthoughtLevel, dan.LevelAttrib.Level);
		danLevelText.Text = GameUtility.FormatUIString("UIPnlDanCulture_LevelText", dan.LevelAttrib.Level);

		for (int i = 0; i < danAttributes.Count; i++)
		{
			var lable = danAttributes[i].GetComponentInChildren<SpriteText>();
			if (i < dan.DanAttributeGroups.Count)
			{
				if (danCultureUIType == DanCultureUIType.DanAttributeRefresh)
				{
					selectedsBtn[i].gameObject.SetActive(true);
					selectedsBtn[i].Data = dan.DanAttributeGroups[i].Id;
				}

				if (i < dan.BreakthoughtLevel)
					lable.Text = string.Format("{0}{1}{2}%", dan.DanAttributeGroups[i].AttributeDesc, GameDefines.textColorBtnYellow, Math.Round(dan.DanAttributeGroups[i].DanAttributes[0].PropertyModifierSets[dan.LevelAttrib.Level - 1].Modifiers[0].AttributeValue * 100, 3));
				else
					lable.Text = GameUtility.FormatUIString("UIPnlDanCulture_AttrContext", GameDefines.textColorDackGray, ItemInfoUtility.GetAssetQualityColor(i + 1), ItemInfoUtility.GetDanTextQuality(i + 1), GameDefines.textColorDackGray);

			}
			else
			{
				lable.Text = GameUtility.FormatUIString("UIPnlDanCulture_AttrContext", GameDefines.textColorDackGray, ItemInfoUtility.GetAssetQualityColor(i + 1), ItemInfoUtility.GetDanTextQuality(i + 1), GameDefines.textColorDackGray);
				selectedsBtn[i].gameObject.SetActive(false);
			}
		}

		StartCoroutine("SetData");
	}

	private void SetCultureForType()
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanCultureTab)))
			SysUIEnv.Instance.GetUIModule<UIPnlDanCultureTab>().UpdateData(dan);
		switch (danCultureUIType)
		{
			case DanCultureUIType.DanLevelUp:
				SetIsMaxLevel(dan.LevelAttrib.Level >= MAXLEVEL);
				fullBox.SetState(0);
				var info = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(dan.ResourceId).GetLevelInfoByBreakAndLevel(dan.BreakthoughtLevel, dan.LevelAttrib.Level);
				UpdateSelectBox(false);
				SetNotifIcon(!ItemInfoUtility.IsLevelNotifyActivity_Dan(dan));

				titleContextText.Text = string.Empty;
				upBtnText.Text = GameUtility.GetUIString("UIPnlDanCulture_UpBtnText1");
				titleText.Text = GameUtility.GetUIString("UIPnlDanCulture_DanLevelUpTitle");
				danLevelUpContext.Text = info != null ? info.LevelUpResultDesc : string.Empty;
				break;
			case DanCultureUIType.DanBreakthought:
				var breakthoughtInfo = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(dan.ResourceId).GetBreakthoughtInfoByBreakthought(dan.BreakthoughtLevel);
				if (breakthoughtInfo != null)
				{
					SetIsMaxLevel(dan.BreakthoughtLevel < breakthoughtInfo.MinLevel);
					titleContextText.Text = GameUtility.FormatUIString("UIPnlDanCulture_TitleContextText_DanBreakthought", breakthoughtInfo.MinLevel);
					foreach (var breakthoughtDetial in breakthoughtInfo.BreakthoughtDetials)
					{
						if (breakthoughtDetial.LevelBefore == dan.BreakthoughtLevel)
							danLevelUpContext.Text = breakthoughtDetial.BreakthoughtResultDesc;
					}
				}
				else
				{
					SetIsMaxLevel(true);
					titleContextText.Text = GameUtility.GetUIString("UIPnlDanCulture_TitleContextText_MaxDanBreakthought");
					danLevelUpContext.Text = string.Empty;
				}

				fullBox.SetState(1);

				UpdateSelectBox(false);
				SetNotifIcon(!ItemInfoUtility.IsBreakNotifyActivity_Dan(dan));

				upBtnText.Text = GameUtility.GetUIString("UIPnlDanCulture_UpBtnText2");
				titleText.Text = GameUtility.GetUIString("UIPnlDanCulture_DanBreakthoughtTitle");


				break;
			case DanCultureUIType.DanAttributeRefresh:
				SetIsMaxLevel(false);
				UpdateSelectBox(true);
				SetNotifIcon(true);

				titleContextText.Text = GameUtility.GetUIString("UIPnlDanCulture_TitleContextText_DanAttributeRefresh");
				upBtnText.Text = GameUtility.GetUIString("UIPnlDanCulture_UpBtnText3");
				titleText.Text = GameUtility.GetUIString("UIPnlDanCulture_DanAttributeRefreshTitle");
				danLevelUpContext.Text = string.Empty;
				break;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator SetData()
	{
		levelUpMaterialList.ClearList(false);
		yield return null;
		if (noMaxLevelObject.activeSelf)
		{
			var danConfig = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(dan.ResourceId);
			switch (danCultureUIType)
			{
				case DanCultureUIType.DanLevelUp:

					DanConfig.LevelInfo levelInfo = danConfig.GetLevelInfoByBreakAndLevel(dan.BreakthoughtLevel, dan.LevelAttrib.Level);
					if (levelInfo != null)
					{
						List<Cost> costs = levelInfo.Costs;
						for (int i = 0; i < costs.Count; i++)
						{
							UIListItemContainer container = poolGameObject.AllocateItem().GetComponent<UIListItemContainer>();
							UIElemDanCultureItem item = container.gameObject.GetComponent<UIElemDanCultureItem>();
							item.SetData(costs[i], 1);
							container.Data = item;
							levelUpMaterialList.AddItem(container);
						}

						moneyIcon.SetData(levelInfo.MoneyCost.id);
						moneyText.Text = string.Format("{0}{1}", ItemInfoUtility.GetGameItemCount(levelInfo.MoneyCost.id) >= levelInfo.MoneyCost.count ? GameDefines.textColorBtnYellow : GameDefines.textColorRed, levelInfo.MoneyCost.count);
						moneyNameText.Text = string.Format("{0}:", ItemInfoUtility.GetAssetName(levelInfo.MoneyCost.id));
					}

					break;
				case DanCultureUIType.DanBreakthought:
					var breakthoughtInfo = danConfig.GetBreakthoughtInfoByBreakthought(dan.BreakthoughtLevel);
					if (breakthoughtInfo != null)
					{
						List<Cost> costs = breakthoughtInfo.Costs;
						for (int i = 0; i < costs.Count; i++)
						{
							UIListItemContainer container = poolGameObject.AllocateItem().GetComponent<UIListItemContainer>();
							UIElemDanCultureItem item = container.gameObject.GetComponent<UIElemDanCultureItem>();
							item.SetData(costs[i], 1);
							container.Data = item;
							levelUpMaterialList.AddItem(container);
						}
						minBreakthoughtLevel = breakthoughtInfo.MinLevel;
						moneyIcon.SetData(breakthoughtInfo.MoneyCost.id);
						moneyText.Text = string.Format("{0}{1}", ItemInfoUtility.GetGameItemCount(breakthoughtInfo.MoneyCost.id) >= breakthoughtInfo.MoneyCost.count ? GameDefines.textColorBtnYellow : GameDefines.textColorRed, breakthoughtInfo.MoneyCost.count);
						moneyNameText.Text = string.Format("{0}:", ItemInfoUtility.GetAssetName(breakthoughtInfo.MoneyCost.id));
					}
					break;
				case DanCultureUIType.DanAttributeRefresh:
					var attributeRefreshCost = GetAttributeRefreshCostInfoByBreakthought(dan.BreakthoughtLevel, ConfigDatabase.DefaultCfg.DanConfig.AttributeRefreshCosts);
					if (attributeRefreshCost != null)
					{
						List<Cost> costs = attributeRefreshCost.Costs;
						for (int i = 0; i < costs.Count; i++)
						{
							UIListItemContainer container = poolGameObject.AllocateItem().GetComponent<UIListItemContainer>();
							UIElemDanCultureItem item = container.gameObject.GetComponent<UIElemDanCultureItem>();
							item.SetData(costs[i], selectedCount);
							container.Data = item;
							levelUpMaterialList.AddItem(container);
						}

						moneyIcon.SetData(attributeRefreshCost.MoneyCost.id);
						moneyText.Text = string.Format("{0}{1}", ItemInfoUtility.GetGameItemCount(attributeRefreshCost.MoneyCost.id) >= attributeRefreshCost.MoneyCost.count * selectedCount ? GameDefines.textColorBtnYellow : GameDefines.textColorRed, attributeRefreshCost.MoneyCost.count * selectedCount);
						moneyNameText.Text = string.Format("{0}:", ItemInfoUtility.GetAssetName(attributeRefreshCost.MoneyCost.id));
					}
					break;
			}
		}
	}

	private void SetIsMaxLevel(bool isMax)
	{
		maxLevelObject.SetActive(isMax);
		noMaxLevelObject.SetActive(!isMax);
	}

	private void UpdateSelectBox(bool isShow)
	{
		foreach (var box in selectedsBtn)
		{
			box.gameObject.SetActive(isShow);
		}

	}

	private DanConfig.AttributeRefreshCost GetAttributeRefreshCostInfoByBreakthought(int breakthought, List<DanConfig.AttributeRefreshCost> attributeRefreshCosts)
	{

		foreach (DanConfig.AttributeRefreshCost attributeRefreshCost in attributeRefreshCosts)
		{

			if (attributeRefreshCost.Breakthought == breakthought)
			{
				return attributeRefreshCost;
			}
		}
		return null;
	}

	//升级协议成功回调
	public void OnDanLevelUpResSuccess(Dan dan)
	{
		Debug.Log(string.Format("OnDanLevelUpResSuccess: The Level Up Power = {0}\nThe Level Power = {1}", this.dan.DanPower, dan.DanPower));

		defendTouch.SetActive(true);
		int count = 0;
		//播放动画
		if (dan.LevelAttrib.Level > dan.LevelAttrib.Level)
		{
			CreateUpAnimation(UIFX_Q_LevelUpSuccess, UIFX_Q_ArrowUp);
			count = dan.BreakthoughtLevel;
		}
		else
			CreateUpAnimation(UIFX_Q_LevelUpFailed, null);


		for (int i = 0; i < count; i++)
		{
			CreateArrowAnimation(UIFX_Q_TextChange, danAttributes[i].gameObject.transform, UIFX_Q_ArrowUp);
		}

		this.dan = dan;
		danLevelText.Text = GameUtility.FormatUIString("UIPnlDanCulture_LevelText", dan.LevelAttrib.Level);
		UpdateSysPlayerDan(dan);
		//SetShowUI();
		StartCoroutine("SetShowUI", 0.5f);

		ShowUpSuccessTips();
	}

	//升阶
	public void OnDanBreakthoughtResSuccess(Dan dan)
	{
		Debug.Log(string.Format("OnDanLevelUpResSuccess: The Level Up Power = {0}\nThe Level Power = {1}", this.dan.DanPower, dan.DanPower));

		defendTouch.SetActive(true);
		if (dan.BreakthoughtLevel > this.dan.BreakthoughtLevel && dan.LevelAttrib.Level >= this.dan.LevelAttrib.Level)
		{
			CreateUpAnimation(UIFX_Q_LevelUpSuccess_Perfect, null);
			CreateArrowAnimation(UIFX_Q_TextChange, danAttributes[dan.BreakthoughtLevel - 1].gameObject.transform, null);
		}
		else if (dan.BreakthoughtLevel > this.dan.BreakthoughtLevel && dan.LevelAttrib.Level < this.dan.LevelAttrib.Level)
		{
			CreateUpAnimation(UIFX_Q_LevelUpSuccess_Grade, UIFX_Q_ArrowDown);
			CreateArrowAnimation(UIFX_Q_TextChange, danAttributes[dan.BreakthoughtLevel - 1].gameObject.transform, null);
		}
		else if (dan.BreakthoughtLevel <= this.dan.BreakthoughtLevel)
			CreateUpAnimation(UIFX_Q_LevelUpFailed_Grade, null);

		this.dan = dan;
		UpdateSysPlayerDan(dan);
		StartCoroutine("SetShowUI", 0.5f);

		ShowUpSuccessTips();
	}

	//洗髓
	public void OnDanAttributeRefreshResSuccess(Dan dan)
	{
		Debug.Log(string.Format("OnDanLevelUpResSuccess: The Level Up Power = {0}\nThe Level Power = {1}", this.dan.DanPower, dan.DanPower));

		defendTouch.SetActive(true);
		for (int i = 0; i < selectedsBtn.Count; i++)
		{
			if (selectedsBtn[i].Data != null && selectedGUIDList.Contains((int)selectedsBtn[i].Data))
			{
				CreateArrowAnimation(UIFX_Q_TextChange, (i < danAttributes.Count ? danAttributes[i].gameObject.transform : this.transform), null);
			}
		}

		this.dan = dan;
		UpdateSysPlayerDan(dan);
		StartCoroutine("SetShowUI", 0.5f);

		ShowUpSuccessTips();
	}

	private void ShowUpSuccessTips()
	{
		float tempDanPower = dan.DanPower;
		if (tempDanPower > danPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(tempDanPower - danPower)));
		else if (tempDanPower < danPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneDown", (int)(danPower - tempDanPower)));

		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, dan.Guid, dan.ResourceId))
		{
			float tempPositionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
			if (tempPositionPower > positionPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionUp", (int)(tempPositionPower - positionPower)));
			else if (tempPositionPower < positionPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionDown", (int)(positionPower - tempPositionPower)));
		}

		danPower = 0f;
		positionPower = 0f;
	}

	private void UpdateSysPlayerDan(Dan dan)
	{
		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.Dans.Count; i++)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.Dans[i].Guid == dan.Guid)
			{
				SysLocalDataBase.Inst.LocalPlayer.Dans.Remove(SysLocalDataBase.Inst.LocalPlayer.Dans[i]);
				SysLocalDataBase.Inst.LocalPlayer.Dans.Add(dan);
				break;
			}
		}

	}

	private void CreateArrowAnimation(GameObject aniGameObject, Transform parent, GameObject arrowObject)
	{
		if (aniGameObject != null)
		{
			GameObject ani = Instantiate(aniGameObject) as GameObject;
			ani.transform.parent = parent;
			ani.transform.localPosition = new Vector3(0, 0, -1);
			Destroy(ani, animationTimer);
		}
		if (arrowObject != null)
		{
			GameObject arrow = Instantiate(arrowObject) as GameObject;
			arrow.transform.parent = parent;
			arrow.transform.localPosition = new Vector3(251f, -20f, 0);
			Destroy(arrow, animationTimer);
		}
	}

	private void CreateUpAnimation(GameObject aniGameObject, GameObject arrowGameObject)
	{
		if (aniGameObject != null)
		{
			GameObject ani = Instantiate(aniGameObject) as GameObject;
			ani.transform.parent = levelUpAniPoint;
			ani.transform.localPosition = Vector3.zero;
			Destroy(ani, animationTimer);
		}
		if (arrowGameObject != null)
		{
			GameObject aniArrow = Instantiate(arrowGameObject) as GameObject;
			aniArrow.transform.parent = arrowUpAniPoint;
			aniArrow.transform.localPosition = Vector3.zero;
			Destroy(aniArrow, animationTimer);
		}

	}

	private void ResetSelectedListData()
	{
		for (int i = 0; i < selectedsBtn.Count; i++)
		{
			if (IsHideSelectedGou(selectedsBtn[i]))
				HideSelectedGou(selectedsBtn[i], false);
		}
		selectedCount = 0;
	}

	private bool IsHideSelectedGou(UIButton selBtn)
	{
		if (selBtn.GetComponentInChildren<UIBox>() != null)
			return selBtn.GetComponentInChildren<UIBox>().gameObject.GetComponent<MeshRenderer>().enabled;
		return false;
	}

	private void HideSelectedGou(UIButton selBtn, bool isSelected)
	{
		if (isSelected)
			selBtn.GetComponentInChildren<UIBox>().gameObject.GetComponent<MeshRenderer>().enabled = !selBtn.GetComponentInChildren<UIBox>().gameObject.GetComponent<MeshRenderer>().enabled;
		else
			selBtn.GetComponentInChildren<UIBox>().gameObject.GetComponent<MeshRenderer>().enabled = isSelected;
	}


	/// <summary>
	/// 判断玩家是否有满足消耗的物品
	/// </summary>
	/// <param name="costs"></param>
	/// <returns></returns>
	private bool IsCost(List<Cost> costs, int multipleNum)
	{
		for (int i = 0; i < costs.Count; i++)
		{
			if (ItemInfoUtility.GetGameItemCount(costs[i].id) < costs[i].count * multipleNum)
				return false;
		}
		return true;
	}

	/// <summary>
	/// 根据type判断玩家是否有满足的消耗物品
	/// </summary>
	/// <returns></returns>
	private bool IsMeetCostRewardByType()
	{
		var danConfig = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(dan.ResourceId);
		switch (danCultureUIType)
		{
			case DanCultureUIType.DanLevelUp:
				var levelInfo = danConfig.GetLevelInfoByBreakAndLevel(dan.BreakthoughtLevel, dan.LevelAttrib.Level);
				if (levelInfo != null)
				{
					if (IsCost(levelInfo.Costs, 1))
						return true;
				}
				break;
			case DanCultureUIType.DanBreakthought:
				var breakthoughtInfo = danConfig.GetBreakthoughtInfoByBreakthought(dan.BreakthoughtLevel);
				if (breakthoughtInfo != null)
				{
					if (IsCost(breakthoughtInfo.Costs, 1))
						return true;
				}
				break;
			case DanCultureUIType.DanAttributeRefresh:
				var attributeRefreshCost = GetAttributeRefreshCostInfoByBreakthought(dan.BreakthoughtLevel, ConfigDatabase.DefaultCfg.DanConfig.AttributeRefreshCosts);
				if (attributeRefreshCost != null)
				{
					if (IsCost(attributeRefreshCost.Costs, selectedCount))
						return true;
				}
				break;
		}
		return false;
	}

	/// <summary>
	/// 根据type判断玩家是否有满足的游戏币物品
	/// </summary>
	/// <returns></returns>
	private bool IsMeetCostMoneyByType(ref string name)
	{
		name = string.Empty;
		var danConfig = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(dan.ResourceId);
		switch (danCultureUIType)
		{
			case DanCultureUIType.DanLevelUp:
				var levelInfo = danConfig.GetLevelInfoByBreakAndLevel(dan.BreakthoughtLevel, dan.LevelAttrib.Level);
				if (levelInfo != null)
				{
					name = ItemInfoUtility.GetAssetName(levelInfo.MoneyCost.id);
					if (ItemInfoUtility.GetGameItemCount(levelInfo.MoneyCost.id) >= levelInfo.MoneyCost.count)
						return true;
				}
				break;
			case DanCultureUIType.DanBreakthought:
				var breakthoughtInfo = danConfig.GetBreakthoughtInfoByBreakthought(dan.BreakthoughtLevel);
				if (breakthoughtInfo != null)
				{
					name = ItemInfoUtility.GetAssetName(breakthoughtInfo.MoneyCost.id);
					if (ItemInfoUtility.GetGameItemCount(breakthoughtInfo.MoneyCost.id) >= breakthoughtInfo.MoneyCost.count)
						return true;
				}
				break;
			case DanCultureUIType.DanAttributeRefresh:
				var attributeRefreshCost = GetAttributeRefreshCostInfoByBreakthought(dan.BreakthoughtLevel, ConfigDatabase.DefaultCfg.DanConfig.AttributeRefreshCosts);
				if (attributeRefreshCost != null)
				{
					name = ItemInfoUtility.GetAssetName(attributeRefreshCost.MoneyCost.id);
					if (ItemInfoUtility.GetGameItemCount(attributeRefreshCost.MoneyCost.id) >= attributeRefreshCost.MoneyCost.count)
						return true;
				}
				break;
		}
		return false;
	}

	private void FullSelectedGUIDList()
	{
		if (selectedGUIDList == null)
			selectedGUIDList = new List<int>();
		selectedGUIDList.Clear();
		for (int i = 0; i < selectedsBtn.Count; i++)
		{
			if (IsHideSelectedGou(selectedsBtn[i]))
			{
				if (selectedsBtn[i].Data != null && !selectedGUIDList.Contains((int)selectedsBtn[i].Data))
					selectedGUIDList.Add((int)selectedsBtn[i].Data);
			}
		}
	}

	private void SetNotifIcon(bool isHide)
	{
		if (notifIcon != null)
		{
			notifIcon.Hide(isHide);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickUp(UIButton btn)
	{
		//提前计算当前等级单的战力数据，预防协议里面返回后造成读脏数据
		danPower = dan.DanPower;
		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, dan.Guid, dan.ResourceId))
			positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		string name = string.Empty;
		switch (danCultureUIType)
		{
			case DanCultureUIType.DanLevelUp:
				if (!IsMeetCostRewardByType())
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanCulture_NoCostMeet_Tips_A"));
				else if (!IsMeetCostMoneyByType(ref name))
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlDanCulture_NoMoneyMeet_Tips_A", name));
				else
					RequestMgr.Inst.Request(new DanLevelUpReq(dan.Guid));

				break;
			case DanCultureUIType.DanBreakthought:
				if (dan.LevelAttrib.Level < minBreakthoughtLevel)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanCulture_NoMinLevel_Tips"));
				else if (!IsMeetCostRewardByType())
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanCulture_NoCostMeet_Tips_B"));
				else if (!IsMeetCostMoneyByType(ref name))
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlDanCulture_NoMoneyMeet_Tips_B", name));
				else
					RequestMgr.Inst.Request(new DanBreakthoughtReq(dan.Guid));
				break;
			case DanCultureUIType.DanAttributeRefresh:

				FullSelectedGUIDList();

				if (selectedGUIDList == null || selectedGUIDList.Count == 0)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanCulture_TitleContextText_DanAttributeRefresh"));
				else if (!IsMeetCostRewardByType())
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlDanCulture_NoCostMeet_Tips_C"));
				else if (!IsMeetCostMoneyByType(ref name))
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlDanCulture_NoMoneyMeet_Tips_C", name));
				else
					RequestMgr.Inst.Request(new DanAttributeRefreshReq(dan.Guid, selectedGUIDList));
				break;
		}

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickXiang(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIDlgDanXiang>(dan.DanAttributeGroups, dan.LevelAttrib.Level);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCostIconBtn(UIButton btn)
	{
		if (btn.Data == null) return;
		GameUtility.ShowAssetInfoUI((btn.Data as Cost).id);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIntroduceBtn(UIButton btn)
	{
		switch (danCultureUIType)
		{
			case DanCultureUIType.DanLevelUp:
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanIntroduce), DanConfig._IntroduceType.DanLevelUp);
				break;
			case DanCultureUIType.DanBreakthought:
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanIntroduce), DanConfig._IntroduceType.DanBreakUp);
				break;
			case DanCultureUIType.DanAttributeRefresh:
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanIntroduce), DanConfig._IntroduceType.DanWish);
				break;
		}

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectedClickBtn(UIButton btn)
	{
		HideSelectedGou(btn, true);
		selectedCount = 0;
		for (int i = 0; i < selectedsBtn.Count; i++)
			if (IsHideSelectedGou(selectedsBtn[i]))
				selectedCount++;

		StartCoroutine("SetData");
	}



	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnDanIconClick(UIButton btn)
	{
		UIElemAssetIcon danIcon = btn.Data as UIElemAssetIcon;
		KodGames.ClientClass.Dan dan = danIcon.Data as KodGames.ClientClass.Dan;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlDanInfo), dan);
	}

}
