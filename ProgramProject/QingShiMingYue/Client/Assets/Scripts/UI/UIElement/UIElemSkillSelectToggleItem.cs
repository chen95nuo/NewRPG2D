using UnityEngine;
using System.Collections;
using ClientServerCommon;
using KodGames;
using KodGames.ClientClass;

public class UIElemSkillSelectToggleItem : MonoBehaviour
{
	//List item skill icon button.
	public UIStateToggleBtn skillIconSelectBtn;
	public UIElemAssetIcon skillIconBtn;
	public UIButton skillQualityBtn;
	public SpriteText skillLevelLabel;
	public SpriteText skillNameLabel;
	
	private KodGames.ClientClass.Skill skillData;
	public KodGames.ClientClass.Skill SkillData
	{
		get { return this.skillData; }
	}

	private bool isSelected = false;
	public bool IsSelected
	{
		get { return this.isSelected; }
	}
	
	/// <summary>
	/// set the skill icon item data.
	/// </summary>
	/// <param name='skill'>
	/// skill
	/// </param>
	public void SetData(KodGames.ClientClass.Skill skill)
	{
		skillData = skill;

		//Set the icon button data.
		skillIconBtn.Data = this;

		if(skill == null)
		{
			skillIconBtn.gameObject.SetActive(false);
			skillIconSelectBtn.Hide(true);
			return;
		}
		else
		{
			skillIconBtn.gameObject.SetActive(true);
			skillIconSelectBtn.Hide(false);
		}
		
		if(skillQualityBtn != null)
		{
			//Skill quality.
//			int quality = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).qualityLevel;
			
			//Set quality button icon.
			UIElemTemplate uiElemTemplate = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIElemTemplate>();
		 	UIElemAvatarQualityTemplate skillQualityTemplate = uiElemTemplate.avatarQualityTemplate;
			uiElemTemplate.gameObject.SetActive(false);
			
			//If quality is right, set the star icon.
			UIUtility.CopyIcon(skillQualityBtn, skillQualityTemplate.avatarQualityBtn);
		}
		
		if(skillLevelLabel != null)
		{
			skillLevelLabel.Text = string.Format(GameUtility.GetUIString("UIPnlAvatar_Label_SkillLevel"), skill.LevelAttrib.Level);
		}

		if (skillNameLabel != null)
		{
			skillNameLabel.Text = ItemInfoUtility.GetAssetName(skill.ResourceId);
		}
		
		
		//Set the skill icon by resource Id.
		skillIconBtn.SetData(skill);
	}

	public void ToggleState()
	{
		isSelected = !isSelected;
		SelectBtnToggle(isSelected);
	}

	public void ResetToggleState(bool selected)
	{
		isSelected = selected;
		SelectBtnToggle(selected);
	}

	private void SelectBtnToggle(bool selected)
	{
		//skillIconSelectBtn.Start();
		if (selected)
		{
			skillIconSelectBtn.SetToggleState("On");
			UIUtility.CopyIconTrans(skillIconBtn.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardNormal);
		}
		else
		{
			skillIconSelectBtn.SetToggleState("Off");
			UIUtility.CopyIconTrans(skillIconBtn.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardGray);
		}
	}
}
