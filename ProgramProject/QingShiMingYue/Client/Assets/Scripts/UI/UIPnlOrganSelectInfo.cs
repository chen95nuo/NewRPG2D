using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIPnlOrganSelectInfo : UIPnlItemInfoBase
{
	public delegate void SelectDelegate(KodGames.ClientClass.Beast selected);

	public SpriteText organName;
	public UIElemAssetIcon organIcon;		
	public SpriteText organPowerLabel;

	public UIBox beastType;
	
	public GameObjectPool skillItemPool;
	public GameObjectPool attrItemPool;
	public UIScrollList attrsList;	

	public UIScrollList skillsList;
	public SpriteText attriTitleLabel;
	
	public UIBox checkBox;
	public SpriteText checkLabel;	

	private KodGames.ClientClass.Beast beastInfo;	
	private BeastConfig.BreakthoughtAndLevel beastLevelInfo;
	//控制是否显示激活技能
	private bool isShowSkills = true;
	private KodGames.ClientClass.Location organLocation;

	public UIButton selectBtn;
	public UIButton levelBtn;
	public UIButton closeBtn;
	public UIButton chooseBtn;

	private SelectDelegate selectDel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas[0] is KodGames.ClientClass.Beast)
		{
			this.beastInfo = userDatas[0] as KodGames.ClientClass.Beast;
			selectDel = userDatas[1] as SelectDelegate;
			selectBtn.Hide(true);
			levelBtn.Hide(true);
			closeBtn.Hide(false);
			chooseBtn.Hide(false);
		}			
		else
		{
			selectBtn.Hide(false);
			levelBtn.Hide(false);
			closeBtn.Hide(true);
			chooseBtn.Hide(true);
			organLocation = userDatas[0] as KodGames.ClientClass.Location;
			this.beastInfo = SysLocalDataBase.Inst.LocalPlayer.SearchBeast(organLocation.Guid);
		}

		isShowSkills = false;
		checkLabel.Text = GameUtility.GetUIString("UIPnlOrganInfo_ShowActiveSkill");
		attriTitleLabel.Text = GameUtility.GetUIString("UIPnlOrganInfo_ShowAttributeNow");		

		BeastConfig.BaseInfo beastCfg = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastInfo.ResourceId);
		BeastConfig.BeastLevelUp beastLevel = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastLevelUpByLevelNow(beastInfo.LevelAttrib.Level);
		this.beastLevelInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(beastInfo.ResourceId, beastInfo.BreakthoughtLevel, beastInfo.LevelAttrib.Level);
		
		//图标实现方式
		organIcon.SetData(beastInfo);		
		int power = beastLevelInfo.Power;
		organPowerLabel.Text = string.Format(GameUtility.GetUIString("UIPnlOrgansInfo_Quality_BattleNumber"), power.ToString());
		organName.Text = ItemInfoUtility.GetAssetName(beastInfo.ResourceId, beastLevel.Quality);				

		UIElemTemplate.Inst.SetBeastTraitIcon(beastType, beastCfg.BeastType);
		beastType.Hide(false);

		checkBox.Hide(isShowSkills);
		FillData();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		organLocation = null;
		beastInfo = null;
		selectDel = null;
	}

	public void FillData()
	{
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

			i++;

			if (i < attributes.Count)
				attr2 = attributes[i];

			UIElemOrganAttribute attribute = attrItemPool.AllocateItem().GetComponent<UIElemOrganAttribute>();
			attribute.SetData(attr1, attr2);
			attrsList.AddItem(attribute.gameObject);
		}

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
		levelItem.SetData(playerBreakInfo.LevelSkills, maxBreakInfo.LevelSkills, GameUtility.GetUIString("UIOrganSkillInfo_LevelSkill_Label"), false, isShowSkills);
		levelItem.AdaptSize();
		skillsList.AddItem(levelItem.gameObject);

		UIElemOrganSkillInfo starItem = skillItemPool.AllocateItem().GetComponent<UIElemOrganSkillInfo>();
		starItem.SetData(playerBreakInfo.StarSkills, maxBreakInfo.StarSkills, GameUtility.GetUIString("UIOrganSkillInfo_StartSkill_Label"), true, isShowSkills);
		starItem.AdaptSize();
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
	private void OnClickSelectChangeBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlSelectOrganList),this.organLocation);
		HideSelf();
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChooseBtn(UIButton btn)
	{
		if (selectDel != null)
			selectDel(this.beastInfo);

		HideSelf();		
	}

	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMenuBot(UIButton btn)
	{
		Debug.Log("OnClickMenuBot  " + "UIPnlchipInfo");
	}
}