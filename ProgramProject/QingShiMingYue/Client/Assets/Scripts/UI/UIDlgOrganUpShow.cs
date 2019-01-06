using System;
using System.Collections;
using ClientServerCommon;
using KodGames;
using UnityEngine;

public class UIDlgOrganUpShow : UIModule
{
	public SpriteText organQuality;

	public GameObjectPool attrPool;
	public UIScrollList attrList;

	public SpriteText skillTitle;
	public SpriteText skillDesc;

	private KodGames.ClientClass.Beast oldBeast = new KodGames.ClientClass.Beast();
	private KodGames.ClientClass.Beast newBeast;

	//private bool isBreakUp = false;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		//KodGames.ClientClass.Beast oldBeast1 = userDatas[0] as KodGames.ClientClass.Beast;
		//this.oldBeast.ResourceId = oldBeast1.ResourceId;
		//this.oldBeast.BreakthoughtLevel = oldBeast1.BreakthoughtLevel;
		//this.oldBeast.LevelAttrib.Level = oldBeast1.LevelAttrib.Level;
		this.oldBeast = userDatas[0] as KodGames.ClientClass.Beast;

		newBeast = userDatas[1] as KodGames.ClientClass.Beast;
		
		//SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().MainBorderBottomMask.gameObject.SetActive(false);

		//if (newBeast.BreakthoughtLevel > oldBeast.BreakthoughtLevel)
		//    isBreakUp = true;
		//else
		//    isBreakUp = false;

		var beastLevel = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastLevelUpByLevelNow(newBeast.LevelAttrib.Level);

		string message = string.Format(GameUtility.GetUIString("UIPnlOrgansBeastTab_Quality_Label_NoAdd"), ItemInfoUtility.GetAssetQualityLevelCNDesc(beastLevel.Quality)); 
		if (beastLevel.AddLevel > 0)
			message = string.Format(GameUtility.GetUIString("UIPnlOrgansBeastLevel_Quality_Show"), ItemInfoUtility.GetAssetQualityLevelCNDesc(beastLevel.Quality), beastLevel.AddLevel);

		organQuality.Text = message;

		ClearData();
		StartCoroutine("FillList");

		return true;
	}

	public void ClearData()
	{
		StopCoroutine("FillList");

		attrList.ClearList(false);
		attrList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		var oldAttrs = PlayerDataUtility.GetBeastAttributes(oldBeast);
		var newAttrs = PlayerDataUtility.GetBeastAttributes(newBeast);

		for (int i = 0; i < oldAttrs.Count && i < newAttrs.Count; i++)
		{
			UIElemOrganAttribute attribute = attrPool.AllocateItem().GetComponent<UIElemOrganAttribute>();
			attribute.SetData(oldAttrs[i], newAttrs[i]);
			attrList.AddItem(attribute.gameObject);
		}

		var oldBreakLevel = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(oldBeast.ResourceId, oldBeast.BreakthoughtLevel, oldBeast.LevelAttrib.Level);
		var newBreakLevel = ConfigDatabase.DefaultCfg.BeastConfig.GetBreakthoughtAndLevel(newBeast.ResourceId, newBeast.BreakthoughtLevel, newBeast.LevelAttrib.Level);

		int getSkillId = 0;

		foreach (var newSkill in newBreakLevel.StarSkills)
		{
			bool isNew = true;

			foreach (var skill in oldBreakLevel.StarSkills)
			{
				if (skill.SkillId == newSkill.SkillId)
					isNew = false;
			}

			if (isNew)
			{
				getSkillId = newSkill.SkillId;
				break;
			}
		}

		if (getSkillId == 0)
		{
			foreach (var newSkill in newBreakLevel.LevelSkills)
			{
				bool isNew = true;

				foreach (var skill in oldBreakLevel.LevelSkills)
				{
					if (skill.SkillId == newSkill.SkillId)
						isNew = false;
				}

				if (isNew)
				{
					getSkillId = newSkill.SkillId;
					break;
				}
			}
		}

		skillTitle.Text = "";
		skillDesc.Text = "";

		if (getSkillId != 0)
		{
			var beastSkill = ConfigDatabase.DefaultCfg.BeastConfig.GetBeastSkillByBeastSkillId(getSkillId);
			if (beastSkill != null)
			{
				skillTitle.Text = beastSkill.SkillName;
				skillDesc.Text = beastSkill.Desc;
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackBtn(UIButton btn)
	{
		SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().SetMeshBtn(false);
		HideSelf();

		//if (isBreakUp)
		//    SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().OnQueryBeastStartUpSuccess();
		//else
		//    SysUIEnv.Instance.GetUIModule<UIPnlOrganGrowPage>().OnQueryBeastLevelUpSuccess();
	}
}
