#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgGiftSkillLib : UIModule 
{
	public UIScrollList skillList;
	public GameObjectPool skillObjectPool;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		StartCoroutine("FillList");
		return true;
	}
	
	public override void OnHide ()
	{
		ClearList();
		base.OnHide ();
	}
	
	private void ClearList()
	{
		StopCoroutine("FillList");
		skillList.ClearList(false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		
		List<KodGames.ClientClass.Skill> giftSkills = new List<KodGames.ClientClass.Skill>();
		foreach(KodGames.ClientClass.Skill skill in SysLocalDataBase.Inst.LocalPlayer.Skills)
		{
			if(ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).GetCombatTurn() != null 
				&& ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).type != SkillConfig._Type.PassiveSkill)
			{
				giftSkills.Add(skill);
			}
		}
		
		int rowCount = (giftSkills.Count + 3)/4;
		int restCount = giftSkills.Count%4;
		
		for(int index = 0; index < rowCount; index++)
		{
			List<KodGames.ClientClass.Skill> skills = giftSkills.GetRange(index*4, 
				(index + 1)*4 > giftSkills.Count? restCount : 4);
			UIListItemContainer item = skillObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemSkillLibItem libItem = item.gameObject.GetComponent<UIElemSkillLibItem>();
			libItem.SetData(skills, this, "OnGiftIconClick");
			skillList.AddItem(item);
			
			foreach(UISkillIconItem iconItem in libItem.skillIcons)
			{
				iconItem.SetSelected(false);
			}
		}
		
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnGiftIconClick(UIButton btn)
	{
//		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
//		KodGames.ClientClass.Skill skill = assetIcon.Data as KodGames.ClientClass.Skill;
		
		for(int idx = 0; idx < skillList.Count; idx++)
		{
			UIElemSkillLibItem skillsItem = skillList.GetItem(idx).gameObject.GetComponent<UIElemSkillLibItem>();
			
			foreach(UISkillIconItem item in skillsItem.skillIcons)
			{
				item.SetSelected(item.iconBtn == btn);
			}
		}
	}
}
#endif
