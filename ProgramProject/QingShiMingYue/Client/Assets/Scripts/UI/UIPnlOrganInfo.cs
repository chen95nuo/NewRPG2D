using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlOrganInfo : UIPnlItemInfoBase
{	
	public SpriteText organName;
	public UIElemAssetIcon organIcon;		
	public SpriteText organPowerLabel;

	public UIBox beastType;
	
	public GameObjectPool skillItemPool;
	public GameObjectPool attrItemPool;
	public UIScrollList attrsList;	

	public UIScrollList skillsList;
	public SpriteText attriTitleLabel;

	public UIButton checkBtn;
	public UIBox checkBox;
	public SpriteText checkLabel;

	public UIProgressBar fragmentPro;

	public UIButton levelUpBtn;
	public UIButton gotoBtn;	
	public UIChildLayoutControl layoutControl;

	private KodGames.ClientClass.Beast beastInfo;
	private BeastConfig.BaseInfo beastBaseCfg;
	private BeastConfig.BreakthoughtAndLevel beastLevelInfo;
	//控制是否显示激活技能
	private bool isShowSkills = true;
	private bool isPlayerBeast;
	private bool isOtherBeast;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if(userDatas[0] is KodGames.ClientClass.Beast)
		{
			isShowSkills = false;

			if (userDatas.Length > 1 && (bool)userDatas[1])
			{
				isOtherBeast = true;
				isPlayerBeast = false;
				ShowActionButtons(false, false);
			}
			else
			{
				isOtherBeast = false;
				isPlayerBeast = true;
				ShowActionButtons(false, true);
			}
			checkBtn.gameObject.SetActive(true);
			checkLabel.Text = GameUtility.GetUIString("UIPnlOrganInfo_ShowActiveSkill");

			this.beastInfo = userDatas[0] as KodGames.ClientClass.Beast;
			attriTitleLabel.Text = GameUtility.GetUIString("UIPnlOrganInfo_ShowAttributeNow");
			this.beastBaseCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastInfo.ResourceId);

			FillProgress(true);
		}
		else
		{
			checkLabel.Text = GameUtility.GetUIString("UIPnlOrganInfo_ShowUnActiveSkill");
			checkBtn.gameObject.SetActive(false);
			attriTitleLabel.Text = GameUtility.GetUIString("UIPnlOrganInfo_ShowAttributeMax");
			isShowSkills = true;
			isPlayerBeast = false;
			isOtherBeast = false;
			this.beastBaseCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId((int)userDatas[0]);
			if (userDatas.Length > 1 && (bool)userDatas[1])
				ShowActionButtons(false, false);
			else
				ShowActionButtons(true, false);

			if (beastBaseCfg.GetWay != null && beastBaseCfg.GetWay.type != _UIType.UnKonw)
				gotoBtn.controlIsEnabled = true;
			else
				gotoBtn.controlIsEnabled = false;

			this.beastInfo = new KodGames.ClientClass.Beast();
			this.beastInfo.ResourceId = beastBaseCfg.Id;
			this.beastInfo.BreakthoughtLevel = ConfigDatabase.DefaultCfg.BeastConfig.MaxBreakthought;
			this.beastInfo.LevelAttrib.Level = ConfigDatabase.DefaultCfg.BeastConfig.MaxLevel;
			FillProgress(false);
		}		

		FillData();

		return true;
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		FillData();
	}

	public void FillData()
	{
		if(isPlayerBeast)			
		{
			this.beastInfo = SysLocalDataBase.Inst.LocalPlayer.SearchBeast(this.beastInfo.Guid);
			this.beastBaseCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastInfo.ResourceId);
		}		

		BeastConfig.BaseInfo beastCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastInfo.ResourceId);
		BeastConfig.BeastLevelUp beastLevel = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastLevelUpByLevelNow(beastInfo.LevelAttrib.Level);
		this.beastLevelInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beastInfo.ResourceId, beastInfo.BreakthoughtLevel, beastInfo.LevelAttrib.Level);
		int power = beastLevelInfo.Power;

		//图标实现方式
		if (isPlayerBeast || isOtherBeast)
		{
			organIcon.SetData(beastInfo);
			organPowerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_Quality_BattleNumber"), power.ToString());
		}
		else
		{
			organIcon.SetData(beastInfo.ResourceId, 1, 1, false);
			organPowerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_Quality_BattleNumber_Max"));
		}
		organName.Text = ItemInfoUtility.GetAssetName(beastInfo.ResourceId, beastLevel.Quality);		

		UIElemTemplate.Inst.SetBeastTraitIcon(beastType, beastCfg.BeastType);
		beastType.Hide(false);

		checkBox.Hide(isShowSkills);

		ClearData();
		StartCoroutine("FillList");
	}

	public void ClearData()
	{	
		StopCoroutine("FillList");
		skillsList.ClearList(false);
		skillsList.ScrollPosition = 0f;

		attrsList.ClearList(false);
		attrsList.ScrollPosition = 0f;
	}

	private void ShowActionButtons(bool showGotoBtn, bool showLevelBtn)
	{
		layoutControl.HideChildObj(levelUpBtn.gameObject, !showLevelBtn);
		layoutControl.HideChildObj(gotoBtn.gameObject, !showGotoBtn);		
	}

	private void FillProgress(bool isActive)
	{
		int framentCount = 0;
		var frament = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(beastBaseCfg.FragmentId);

		if (frament != null)
		{
			framentCount = frament.Amount;
		}

		if(isActive)
		{
			BeastConfig.BeastBreakthought beastBreakCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastBreakthoughtByBreakthoughtNow(beastInfo.BreakthoughtLevel);
			var beastInfoCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastInfo.ResourceId);

			float prgValue = 0f;			
			string proMsg = "";

			if (beastBreakCfg.BreakthoughtAfter > 0)
			{
				prgValue = (float)framentCount / (float)beastBreakCfg.UpCostFragmentCount;

				if (prgValue < 1.0f)
					fragmentPro.Value = prgValue;
				else
					fragmentPro.Value = 1.0f;

				proMsg = GameUtility.FormatUIString("UIPnlOrgansBeastTab_CountWithMax", (float)framentCount, ConfigDatabase.DefaultCfg.BeastConfig.GetBeastBreakthoughtByBreakthoughtNow(beastInfoCfg.MinActiveBreakthought).ActiveCostFragmentCount);
			}
			else
			{
				fragmentPro.Value = 1.0f;
				proMsg = GameUtility.GetUIString("UIPnlOrgansInfo_Beasts_StrenghMax");
			}

			fragmentPro.Text = proMsg;
		}
		else
		{
			float prgValue = (float)framentCount / (float)ConfigDatabase.DefaultCfg.BeastConfig.GetBeastBreakthoughtByBreakthoughtNow(beastBaseCfg.MinActiveBreakthought).ActiveCostFragmentCount;

			if (prgValue < 1.0f)
				fragmentPro.Value = prgValue;
			else
				fragmentPro.Value = 1.0f;

			fragmentPro.Text = GameUtility.FormatUIString("UIPnlOrgansBeastTab_CountWithMax", (float)framentCount, ConfigDatabase.DefaultCfg.BeastConfig.GetBeastBreakthoughtByBreakthoughtNow(beastBaseCfg.MinActiveBreakthought).ActiveCostFragmentCount);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		
		var attributes = PlayerDataUtility.GetBeastAttributes(beastInfo);

		for (int i = 0; i < attributes.Count; i++)
		{
			AttributeCalculator.Attribute attr1 = new AttributeCalculator.Attribute(0, 0);
			AttributeCalculator.Attribute attr2 = new AttributeCalculator.Attribute(0, 0);

			attr1 = attributes[i];
			
			i ++;

			if (i < attributes.Count)
				attr2 = attributes[i];

			UIElemOrganAttribute attribute = attrItemPool.AllocateItem().GetComponent<UIElemOrganAttribute>();
			attribute.SetData(attr1,attr2);
			attrsList.AddItem(attribute.gameObject);
		}

		int maxLevel = ConfigDatabase.DefaultCfg.BeastConfig.MaxLevel;
		int maxBreakthought = ConfigDatabase.DefaultCfg.BeastConfig.MaxBreakthought;

		BeastConfig.BreakthoughtAndLevel playerBreakInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beastInfo.ResourceId, beastInfo.BreakthoughtLevel, beastInfo.LevelAttrib.Level);
		BeastConfig.BreakthoughtAndLevel maxBreakInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beastInfo.ResourceId, maxBreakthought, maxLevel);

		UIElemOrganSkillInfo levelItem = skillItemPool.AllocateItem().GetComponent<UIElemOrganSkillInfo>();
		levelItem.SetData(playerBreakInfo.LevelSkills, maxBreakInfo.LevelSkills, GameUtility.GetUIString("UIOrganSkillInfo_LevelSkill_Label"), false, isShowSkills);
		levelItem.AdaptSize();
		if (!isPlayerBeast && !isOtherBeast)
			levelItem.ReSetColor();
		skillsList.AddItem(levelItem.gameObject);

		UIElemOrganSkillInfo starItem = skillItemPool.AllocateItem().GetComponent<UIElemOrganSkillInfo>();
		starItem.SetData(playerBreakInfo.StarSkills, maxBreakInfo.StarSkills, GameUtility.GetUIString("UIOrganSkillInfo_StartSkill_Label"), true, isShowSkills);
		starItem.AdaptSize();
		if (!isPlayerBeast && !isOtherBeast)
			starItem.ReSetColor();
		skillsList.AddItem(starItem.gameObject);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickLevelUpBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrganGrowPage), this.beastInfo);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCheckBoxBtn(UIButton btn)
	{
		isShowSkills = !isShowSkills;				
		checkBox.Hide(isShowSkills);
		FillData();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoBtn(UIButton btn)
	{
		ClientServerCommon.GetWay getway = this.beastBaseCfg.GetWay;
		
		if (getway.type != _UIType.UI_ActivityDungeon && getway.type != _UIType.UI_Dungeon)
			GameUtility.JumpUIPanel(getway.type);
		else
			GameUtility.JumpUIPanel(getway.type, getway.data);
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMenuBot(UIButton btn)
	{
		Debug.Log("OnClickMenuBot  " + "UIPnlchipInfo");
	}
}