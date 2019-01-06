using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlOrganGrowPage : UIPnlItemInfoBase
{

	private enum PageSelect
	{
		Skill = 1,
		LevelUp = 2,
		Attribute = 3,
	}

	//button root
	public UIButton[] selectBtns;

	//show organ
	public UIElemAssetIcon organIcon;
	public SpriteText organName;
	public UIBox beastType;
	public SpriteText organQuality;
	public SpriteText powerLabel;
	public SpriteText lineUpLabel;

	public GameObject skillRoot;
	public GameObject levelUpRoot;
	public GameObject attributeRoot;

	public UIButton leftArrowBtn;
	public UIButton rightArrowBtn;
	public UIButton mainBorderBottomMask;

	//SkillRoot
	public UIBox checkBox;
	public UIScrollList skillsList;
	public GameObjectPool skillItemPool;

	//LevelRoot
	public UIElemAssetBeastEquips beastEquips;
	public UIProgressBar fragmentPro;
	public UIButton strengBtn;
	public UIButton equipAllBtn;
	public UIButton levelUpBtn;
	public UIButton gotoAddBtn;

	public GameObject flyLocation;
	public GameObject strLocation;

	public UIElemAssetIconBreakThroughBtn breakBorder;

	public SpriteText titleLabel1;
	public SpriteText titleLabel2;
	public SpriteText btnLabel;
	//AttributeRoot
	public UIScrollList attrsList;
	public GameObjectPool attrPool;

	public GameObjectPool titlePool;

	private KodGames.ClientClass.Beast beastInfo;
	private KodGames.ClientClass.Beast oldBeast = new KodGames.ClientClass.Beast();
	private BeastConfig.BaseInfo beastInfoCfg;

	//控制是否显示激活技能
	private bool isShowSkills = true;
	private PageSelect page = PageSelect.LevelUp;
	private List<GameObject> levelEffects = new List<GameObject>();
	private bool isMoveing = false;

	//战力缓存
	private float beastPower;
	private float positionPower;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		this.beastInfo = userDatas[0] as KodGames.ClientClass.Beast;
		this.beastInfoCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastInfo.ResourceId);

		beastPower = 0f;
		positionPower = 0f;		

		//Button Data
		if (selectBtns.Length > 2)
		{
			selectBtns[0].Data = PageSelect.LevelUp;
			selectBtns[1].Data = PageSelect.Skill;
			selectBtns[2].Data = PageSelect.Attribute;
		}

		if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
			gotoAddBtn.Hide(false);
		else gotoAddBtn.Hide(true);

		FillData();

		return true;
	}

	public override void OnHide()
	{
		foreach (var gameobj in levelEffects)
		{
			GameObject.Destroy(gameobj);
		}
		base.OnHide();
	}

	public void FillData()
	{
		//计算下一个机关兽
		mainBorderBottomMask.gameObject.SetActive(false);
		List<KodGames.ClientClass.Beast> playerBeastLists = SysLocalDataBase.Inst.LocalPlayer.Beasts;

		if (playerBeastLists.Count < 2)
		{
			leftArrowBtn.Hide(true);
			rightArrowBtn.Hide(true);
		}
		else
		{
			int index = 0;

			for (int i = 0; i < playerBeastLists.Count; i++)
			{
				if (this.beastInfo.ResourceId == playerBeastLists[i].ResourceId)
				{
					index = i;
					break;
				}
			}

			if (index == 0)
			{
				leftArrowBtn.Data = playerBeastLists[playerBeastLists.Count - 1];
				rightArrowBtn.Data = playerBeastLists[index + 1];
			}
			else if (index > 0 && index < playerBeastLists.Count - 1)
			{
				leftArrowBtn.Data = playerBeastLists[index - 1];
				rightArrowBtn.Data = playerBeastLists[index + 1];
			}
			else if (index == playerBeastLists.Count - 1)
			{
				leftArrowBtn.Data = playerBeastLists[index - 1];
				rightArrowBtn.Data = playerBeastLists[0];
			}
		}

		//SetDefault Page
		page = PageSelect.LevelUp;

		//Organ
		organIcon.SetData(beastInfo);
		organName.Text = ItemInfoUtility.GetAssetName(beastInfo.ResourceId);
		powerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_Quality_BattleNumber"), PlayerDataUtility.CalculateBeastPower(beastInfo).ToString());

		lineUpLabel.Text = "";
		bool isLineUp = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, beastInfo.Guid, beastInfo.ResourceId);
		if (isLineUp)
			lineUpLabel.Text = GameUtility.GetUIString("UIPnlOrgansBeastTab_LineUp");

		BeastConfig.BeastLevelUp beastLevelCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastLevelUpByLevelNow(beastInfo.LevelAttrib.Level);
		string message = string.Format(GameUtility.GetUIString("UIPnlOrgansBeastTab_Quality_Label_NoAdd"), ItemInfoUtility.GetAssetQualityLevelCNDesc(beastLevelCfg.Quality));

		if (beastLevelCfg.AddLevel > 0)
			message = string.Format(GameUtility.GetUIString("UIPnlOrgansBeastTab_Quality_Label"), ItemInfoUtility.GetAssetQualityLevelCNDesc(beastLevelCfg.Quality), beastLevelCfg.AddLevel);

		organQuality.Text = message;
		
		UIElemTemplate.Inst.SetBeastTraitIcon(beastType, beastInfoCfg.BeastType);
		beastType.Hide(false);
		FIllPageData();
	}

	public void ClearData()
	{
		StopCoroutine("FillSkillList");
		skillsList.ClearList(false);
		skillsList.ScrollPosition = 0f;

		StopCoroutine("FillAttrList");
		attrsList.ClearList(false);
		attrsList.ScrollPosition = 0f;
	}

	private void FIllPageData()
	{
		switch (this.page)
		{
			case PageSelect.Skill:
				checkBox.Hide(isShowSkills);
				ChangePage(true, false, false);
				ClearData();
				StartCoroutine("FillSkillList");
				break;

			case PageSelect.LevelUp:
				ChangePage(false, true, false);
				FillGrowthPage();
				break;

			case PageSelect.Attribute:
				ChangePage(false, false, true);
				ClearData();
				StartCoroutine("FillAttrList");
				break;
		}

		ChangeButton();
	}

	private void FillGrowthPage()
	{
		beastEquips.SetData(this.beastInfo);

		BeastConfig.BeastLevelUp beastLevelNow = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastLevelUpByLevelNow(beastInfo.LevelAttrib.Level);		

		int maxLevel = ConfigDatabase.DefaultCfg.BeastConfig.MaxLevel;
		int maxBreakthought = ConfigDatabase.DefaultCfg.BeastConfig.MaxBreakthought;

		titleLabel1.Text = "";
		if (beastLevelNow.LevelNow < maxLevel)
		{
			BeastConfig.BeastLevelUp beastLevelCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastLevelUpByLevelNow(beastLevelNow.LevelNext);
			string message = string.Format(GameUtility.GetUIString("UIPnlOrgansBeastTab_Quality_Label_NoAdd"), ItemInfoUtility.GetAssetQualityLevelCNDesc(beastLevelCfg.Quality));

			if (beastLevelCfg.AddLevel > 0)
				message = string.Format(GameUtility.GetUIString("UIPnlOrgansBeastTab_Quality_Label"), ItemInfoUtility.GetAssetQualityLevelCNDesc(beastLevelCfg.Quality), beastLevelCfg.AddLevel);

			titleLabel1.Text = string.Format(GameUtility.GetUIString("UIPnlOrganGrowPage_LevelUpLabel_Add"), message);
		}
		
		if (beastInfo.PartIndexs.Count == 5)
		{
			equipAllBtn.Hide(true);

			if (beastLevelNow.LevelNow >= maxLevel)
				levelUpBtn.Hide(true);
			else
				levelUpBtn.Hide(false);
		}
		else
		{
			equipAllBtn.Hide(false);
			levelUpBtn.Hide(true);
		}

		BeastConfig.BeastBreakthought beastBreakCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastBreakthoughtByBreakthoughtNow(beastInfo.BreakthoughtLevel);

		float prgValue = 0f;
		int framentCount = 0;
		var frament = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(beastInfoCfg.FragmentId);
		string proMsg = "";

		if (frament != null)
		{
			framentCount = frament.Amount;
		}

		if (beastBreakCfg.BreakthoughtNow < maxBreakthought)
		{
			titleLabel2.Text = GameUtility.GetUIString("UIPnlOrganGrowPage_BreakBorder");
			breakBorder.SetBreakThroughIcon(beastBreakCfg.BreakthoughtAfter);
			prgValue = (float)framentCount / (float)beastBreakCfg.UpCostFragmentCount;

			if (prgValue < 1.0f)
			{
				fragmentPro.Value = prgValue;
				strengBtn.controlIsEnabled = false;
			}
			else
			{
				fragmentPro.Value = 1.0f;
				strengBtn.controlIsEnabled = true;
			}

			proMsg = GameUtility.FormatUIString("UIPnlOrgansBeastTab_CountWithMax", framentCount, beastBreakCfg.UpCostFragmentCount);
		}
		else
		{
			titleLabel2.Text = "";
			breakBorder.SetBreakThroughIcon(0);
			fragmentPro.Value = 1.0f;
			strengBtn.controlIsEnabled = false;
			proMsg = GameUtility.GetUIString("UIPnlOrgansInfo_Beasts_StrenghMax");
		}

		fragmentPro.Text = proMsg;
	}

	private void ChangePage(bool isSkill, bool isLevelUp, bool isAttrs)
	{
		skillRoot.SetActive(isSkill);
		levelUpRoot.SetActive(isLevelUp);
		attributeRoot.SetActive(isAttrs);
	}
	
	private void ChangeButton()
	{
		for (int i = 0; i < selectBtns.Length; i++)
		{
			if ((PageSelect)selectBtns[i].Data == page)
				selectBtns[i].controlIsEnabled = false;
			else
				selectBtns[i].controlIsEnabled = true;
		}
	}

	private void ShowTips()
	{
		float powerTemp = PlayerDataUtility.CalculateBeastPower(beastInfo);
		if (powerTemp > beastPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_Illusion_Up", ItemInfoUtility.GetAssetName(beastInfo.ResourceId), (int)(powerTemp - beastPower)));
		else if (powerTemp < beastPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_Illusion_Down", ItemInfoUtility.GetAssetName(beastInfo.ResourceId), (int)(beastPower - powerTemp)));

		float tempPositionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
		if (tempPositionPower > positionPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionUp", (int)(tempPositionPower - positionPower)));
		else if (tempPositionPower < positionPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionDown", (int)(positionPower - tempPositionPower)));

		beastPower = powerTemp;
		positionPower = tempPositionPower;
	}

	public void OnQueryBeastSuccess()
	{
		FillNewBeast();

		ShowTips();
	}

	public void OnQueryBeastLevelUpSuccess()
	{
		StartCoroutine("PlayStarAnimation");

		ShowTips();
	}

	public void OnQueryBeastStartUpSuccess()
	{
		StartCoroutine("PlayBreakAnimation");
	}

	private void FillNewBeast()
	{
		this.beastInfo = SysLocalDataBase.Inst.LocalPlayer.SearchBeast(beastInfo.Guid);
		this.beastInfoCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(this.beastInfo.ResourceId);
		FillData();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator PlayBreakAnimation()
	{
		isMoveing = true;
		mainBorderBottomMask.gameObject.SetActive(true);

		int rdm = Random.Range(3, 6);

		int k = 0;

		for (int i = 0; i < rdm; i++)
		{
			yield return new WaitForSeconds(0.6f);

			GameObject breakEff = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.partFlyEffect));
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(organIcon.gameObject, breakEff);
			breakEff.transform.localPosition = strLocation.transform.localPosition;
			levelEffects.Add(breakEff);

			for (int j = 0; j < levelEffects.Count; j++)
			{
				GameObject.Destroy(levelEffects[j].gameObject, 0.6f);
			}
			AudioManager.Instance.PlaySound(GameDefines.menu_Blade2, 0f);

			Vector3 breakFly = new Vector3();

			AnimatePosition.Do(breakEff,
									EZAnimation.ANIM_MODE.FromTo,
									breakEff.transform.localPosition,
									breakFly,
									EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
									0.6f,
									0f,
									null,
									 (data) =>
									 {
										 k++;
										 if (k >= rdm)
										 {
											 GameObject itemGetbig = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.partLevelUp_Success));
											 ObjectUtility.AttachToParentAndResetLocalPosAndRotation(organIcon.gameObject, itemGetbig);
											 levelEffects.Add(itemGetbig);

											 GameObject itembig = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.part_big));
											 ObjectUtility.AttachToParentAndResetLocalPosAndRotation(organIcon.gameObject, itembig);
											 levelEffects.Add(itembig);

											 AudioManager.Instance.PlaySound(GameDefines.meridianOpen, 0f);

											 FillNewBeast();

											 StartCoroutine("PlayCantClickDelay");
										 }
										 else
										 {
											 GameObject itemGetLevel = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.partGetEffect));
											 ObjectUtility.AttachToParentAndResetLocalPosAndRotation(organIcon.gameObject, itemGetLevel);
											 levelEffects.Add(itemGetLevel);
										 }
									 });
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator PlayStarAnimation()
	{
		isMoveing = true;
		mainBorderBottomMask.gameObject.SetActive(true);

		for (int i = 0; i < beastEquips.equipIcons.Length; i++)
		{
			GameObject partZoom = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.equipZoom));
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(beastEquips.equipIcons[i].gameObject, partZoom);
			levelEffects.Add(partZoom);
		}

		yield return new WaitForSeconds(0.9f);

		levelEffects.Clear();

		AudioManager.Instance.PlaySound(GameDefines.menu_Blade2, 0f);
		int k = 0;

		for (int i = 0; i < beastEquips.equipIcons.Length; i++)
		{
			GameObject levelUp = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.partFlyEffect));
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(beastEquips.gameObject, levelUp);
			levelUp.gameObject.transform.localPosition = beastEquips.equipIcons[i].gameObject.transform.localPosition;
			levelEffects.Add(levelUp);

			AnimatePosition.Do(levelUp,
			EZAnimation.ANIM_MODE.FromTo,
			levelUp.gameObject.transform.localPosition,
			flyLocation.transform.localPosition,
			EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
			0.6f,
			0f,
			null,
			 (data) =>
			 {
				 k++;
				 if (k >= beastEquips.equipIcons.Length)
				 {
					 foreach (var gameobj in levelEffects)
					 {
						 GameObject.Destroy(gameobj);
					 }

					 levelEffects.Clear();

					 GameObject itemGetbig = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.equipPart));
					 ObjectUtility.AttachToParentAndResetLocalPosAndRotation(organIcon.gameObject, itemGetbig);
					 levelEffects.Add(itemGetbig);

					 GameObject itembig = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.partLevelUp_Success));
					 ObjectUtility.AttachToParentAndResetLocalPosAndRotation(organIcon.gameObject, itembig);
					 AudioManager.Instance.PlaySound(GameDefines.meridianOpen, 0f);
					 levelEffects.Add(itembig);
					 FillNewBeast();

					 StartCoroutine("PlayCantClickDelay");
				 }
			 });
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator PlayCantClickDelay()
	{		
		yield return new WaitForSeconds(1.0f);
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgOrganUpShow), oldBeast, beastInfo);
	}

	public void SetMeshBtn(bool show)
	{
		isMoveing = show;
		mainBorderBottomMask.gameObject.SetActive(show);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillAttrList()
	{
		yield return null;

		var playerBreakInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beastInfo.ResourceId, beastInfo.BreakthoughtLevel, beastInfo.LevelAttrib.Level);
		var palyerAttributes = PlayerDataUtility.GetBeastAttributes(beastInfo);

		UIElemOrganAttributeTitle titleItem = titlePool.AllocateItem().GetComponent<UIElemOrganAttributeTitle>();
		titleItem.SetData(GameUtility.GetUIString("UIPnlOrganInfo_Attribute_Show"), GameUtility.GetUIString("UIPnlOrganInfo_Attribute_Buff")
			, GameUtility.GetUIString("UIPnlOrganInfo_Attribute_Tips1"), GameUtility.GetUIString("UIPnlOrganInfo_Attribute_Tips2"));

		attrsList.AddItem(titleItem.gameObject);


		//List<ClientServerCommon.Attribute> partAttributes = new List<ClientServerCommon.Attribute>();
		//partAttributes.AddRange(beastPartAttrCfg.Attributes);
		//PlayerDataUtility.MergeClientServerAttributes(ref partAttributes, true);

		Dictionary<int, ClientServerCommon.Attribute> partAttrDic = new Dictionary<int, ClientServerCommon.Attribute>();

		for (int i = 0; i < beastInfo.PartIndexs.Count; i++)
		{
			int attributeId = playerBreakInfo.BeastPartActives[beastInfo.PartIndexs[i] - 1].AttributeId;
			var beastPartAttrCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastPartAttributeGroupById(attributeId);

			foreach (var attribute in beastPartAttrCfg.Attributes)
			{
				if (partAttrDic.ContainsKey(attribute.type))
					partAttrDic[attribute.type].value += attribute.value;
				else partAttrDic.Add(attribute.type, attribute);
			}
		}

		//Self Attribute
		for (int i = 0; i < palyerAttributes.Count; i++)
		{
			UIElemOrganAttribute attributeItem = attrPool.AllocateItem().GetComponent<UIElemOrganAttribute>();

			// 属性类型，属性值，转换系数
			double partEquip = 0;
			if(partAttrDic.ContainsKey(palyerAttributes[i].type))
			{
				partEquip = partAttrDic[palyerAttributes[i].type].value;
			}

			float reNum = 1.0f;

			foreach (var modifier in playerBreakInfo.GuardModifiers)
			{
				if (modifier.attributeType == palyerAttributes[i].type && modifier.attributeValue <= 1)
					reNum = modifier.attributeValue;
			}

			attributeItem.SetData(palyerAttributes[i], partEquip, reNum);
			attrsList.AddItem(attributeItem.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillSkillList()
	{
		yield return null;

		int maxLevel = ConfigDatabase.DefaultCfg.BeastConfig.MaxLevel;
		int maxBreakthought = ConfigDatabase.DefaultCfg.BeastConfig.MaxBreakthought;

		BeastConfig.BreakthoughtAndLevel playerBreakInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beastInfo.ResourceId, beastInfo.BreakthoughtLevel, beastInfo.LevelAttrib.Level);
		if (playerBreakInfo == null)
		{
			Debug.LogError("playerBreak breakthought or level is Error beast:" + beastInfo.BreakthoughtLevel.ToString() + "  level :" + beastInfo.LevelAttrib.Level.ToString());
		}

		BeastConfig.BreakthoughtAndLevel maxBreakInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beastInfo.ResourceId, maxBreakthought, maxLevel);
		if (maxBreakInfo == null)
		{
			Debug.LogError("maxBreakInfo breakthought or level is Error beast:" + maxBreakthought.ToString() + "  level :" + maxLevel.ToString());
		}

		UIElemOrganSkillInfo levelItem = skillItemPool.AllocateItem().GetComponent<UIElemOrganSkillInfo>();
		levelItem.SetData(playerBreakInfo.LevelSkills, maxBreakInfo.LevelSkills, GameUtility.GetUIString("UIPnlOrganGrowPage_LevelActive"), false,isShowSkills);
		levelItem.AdaptSize();
		skillsList.AddItem(levelItem.gameObject);

		UIElemOrganSkillInfo starItem = skillItemPool.AllocateItem().GetComponent<UIElemOrganSkillInfo>();
		starItem.SetData(playerBreakInfo.StarSkills, maxBreakInfo.StarSkills, GameUtility.GetUIString("UIPnlOrganGrowPage_BreakActive"), true, isShowSkills);
		starItem.AdaptSize();
		skillsList.AddItem(starItem.gameObject);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		if (isMoveing)
			return;

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangePageBtn(UIButton btn)
	{
		if (isMoveing)
			return;

		this.page = (PageSelect)btn.Data;
		FIllPageData();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCheckBoxBtn(UIButton btn)
	{
		if (isMoveing)
			return;

		isShowSkills = !isShowSkills;
		FIllPageData();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeUpBeastBtn(UIButton btn)
	{
		if (isMoveing)
			return;

		this.beastInfo = btn.Data as KodGames.ClientClass.Beast;
		this.beastInfoCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(this.beastInfo.ResourceId);
		FillData();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeDownBeastBtn(UIButton btn)
	{
		if (isMoveing)
			return;

		this.beastInfo = btn.Data as KodGames.ClientClass.Beast;
		this.beastInfoCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(this.beastInfo.ResourceId);
		FillData();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBeastChipBtn(UIButton btn)
	{
		if (isMoveing)
			return;

		beastPower = PlayerDataUtility.CalculateBeastPower(beastInfo);
		positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgOrganChipSplit), this.beastInfo, (int)assetIcon.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBeastBreakUpBtn(UIButton btn)
	{
		if (isMoveing)
			return;

		beastPower = PlayerDataUtility.CalculateBeastPower(beastInfo);
		positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		MainMenuItem breakBtn = new MainMenuItem();
		breakBtn.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK_Space");
		breakBtn.Callback = ReqBeastBreakThough;
		breakBtn.CallbackData = beastInfo.Guid;

		MainMenuItem cancelBtn = new MainMenuItem();
		cancelBtn.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel_Space");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		//通过阵容ID来获取阵容名称作为message的Title

		var beastBreakCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastBreakthoughtByBreakthoughtNow(beastInfo.BreakthoughtLevel);
		var beastLevelBefore = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beastInfo.ResourceId, beastInfo.BreakthoughtLevel, beastInfo.LevelAttrib.Level);
		var beastLevelAfter = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beastInfo.ResourceId, beastBreakCfg.BreakthoughtAfter, beastInfo.LevelAttrib.Level);

		List<int> newSkillList = new List<int>();

		foreach (var breakSkillAf in beastLevelAfter.StarSkills)
		{
			bool newFalg = false;

			foreach (var breakSkillBf in beastLevelBefore.StarSkills)
			{
				if (breakSkillBf.SkillId == breakSkillAf.SkillId)
					newFalg = true;
			}

			if (!newFalg)
				newSkillList.Add(breakSkillAf.SkillId);				
		}

		string message = string.Empty;

		if (newSkillList.Count > 0)
		{			
			string messageSkill = "";
			foreach (var newskillId in newSkillList)
			{
				var beastSkill = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastSkillByBeastSkillId(newskillId);
				if (beastSkill != null)
					messageSkill = messageSkill + GameUtility.FormatUIString("UIPnlOrganGrowPage_BreakBorder_NewSkillLabel", beastSkill.SkillName);
			}
			message = string.Format(GameUtility.GetUIString("UIPnlOrganGrowPage_BreakBorder_MessageLabel"), beastBreakCfg.UpItemCost.count, ItemInfoUtility.GetAssetName(beastBreakCfg.UpItemCost.id), ItemInfoUtility.GetAssetName(beastInfo.ResourceId)
														, ItemInfoUtility.GetLevelCN(beastBreakCfg.BreakthoughtAfter), messageSkill);
		}

		if (message.Equals(string.Empty))
		{
			message = string.Format(GameUtility.GetUIString("UIPnlOrganGrowPage_BreakBorder_MessageEmpty"), beastBreakCfg.UpItemCost.count, ItemInfoUtility.GetAssetName(beastBreakCfg.UpItemCost.id), ItemInfoUtility.GetAssetName(beastInfo.ResourceId)
														, ItemInfoUtility.GetLevelCN(beastBreakCfg.BreakthoughtAfter));
		}

		showData.SetData(GameUtility.FormatUIString("UIPnlOrganGrowPage_BreakBorder_Message"), message, cancelBtn, breakBtn);
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	private bool ReqBeastBreakThough(object guid)
	{
		oldBeast.BreakthoughtLevel = beastInfo.BreakthoughtLevel;
		oldBeast.LevelAttrib = beastInfo.LevelAttrib;
		oldBeast.ResourceId = beastInfo.ResourceId;

		RequestMgr.Inst.Request(new BeastBreakthoughtReq(guid.ToString(), beastInfo));
		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEquipAllBtn(UIButton btn)
	{
		if (isMoveing)
			return;

		beastPower = PlayerDataUtility.CalculateBeastPower(beastInfo);
		positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);			

		RequestMgr.Inst.Request(new EquipBeastPartReq(beastInfo.Guid, BeastConfig._PartIndex.AllPut));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickLevelUpBtn(UIButton btn)
	{
		if (isMoveing)
			return;

		oldBeast.BreakthoughtLevel = beastInfo.BreakthoughtLevel;
		oldBeast.LevelAttrib = beastInfo.LevelAttrib;
		oldBeast.ResourceId = beastInfo.ResourceId;

		beastPower = PlayerDataUtility.CalculateBeastPower(beastInfo);
		positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		RequestMgr.Inst.Request(new BeastLevelUpReq(beastInfo.Guid, beastInfo));
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBack(UIButton btn)
	{
		if (isMoveing)
			return;

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAttributeExplain(UIButton btn)
	{
		if (isMoveing)
			return;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgOrganAttributeExplain));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAddGotoBtn(UIButton btn)
	{
		if (isMoveing)
			return;

		if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgOrganGetWay), this.beastInfo);
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMenuBot(UIButton btn)
	{
		Debug.Log("OnClickMenuBot  " + "UIPnlchipInfo");
	}
}