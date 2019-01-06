using System;
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgAvatarMeridian : UIModule
{
	public SpriteText titlelabel;
	public UIBox meridianIcon;
	public SpriteText meridianName;
	public SpriteText meridianCost;
	public SpriteText saveOrNot;

	public UIChildLayoutControl layoutControl;
	public AutoSpriteControlBase cancelBase;
	public AutoSpriteControlBase saveBase;
	public AutoSpriteControlBase maridianBase;
	public UIButton closeBtn;

	//结果
	public GameObject afterAction;
	public List<UIBox> afterAttributeLabels;

	//洗脉 易经 后
	public GameObject beforeAction;

	public SpriteText beforeChangeText;
	public SpriteText afterChangeText;

	public List<UIBox> beforeChangeLabels;
	public List<UIBox> afterChangeLabels;

	private KodGames.ClientClass.Avatar avatarData;
	private int meridianID;
	private bool isFirstActive;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		avatarData = userDatas[0] as KodGames.ClientClass.Avatar;
		meridianID = (int)userDatas[1];

		InitButtonText();

		InitView(null, false, (bool)userDatas[3]);

		HideChildLayout((bool)userDatas[2]);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatarMeridianTab))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarMeridianTab>().UpDataMeridianUI(meridianID, true);
	}

	private void InitButtonText()
	{
		MeridianConfig.Meridian meridianConfig = ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianById(meridianID);

		if (meridianConfig != null)
		{
			maridianBase.Text = (meridianConfig.type == MeridianConfig._MeridianType.Twelve) ? GameUtility.GetUIString("UIAvatarMeridian_Twelve") : GameUtility.GetUIString("UIAvatarMeridian_Eight");
		}
	}

	private void InitView(List<KodGames.ClientClass.PropertyModifier> unSaveModifiers, bool hasUnSavedMeridian, bool isFirstActive)
	{
		this.isFirstActive = isFirstActive;

		MeridianConfig.Meridian meridianConfig = ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianById(meridianID);
		KodGames.ClientClass.MeridianData meridianData = avatarData.GetMeridianByID(meridianID);

		// Set meridianIcon and Name.
		string titlelabelText = "";
		if (isFirstActive)//激活
		{
			titlelabelText = GameUtility.FormatUIString("UIPnlAvatarMeridian_ActiveSuccess");
			meridianCost.Text = "";
			saveOrNot.Text = "";
			maridianBase.Text = GameUtility.FormatUIString("UIDlgMessage_CtrlBtn_OK");
		}
		else if (meridianConfig.type == MeridianConfig._MeridianType.Twelve)//易经
		{
			maridianBase.Text = GameUtility.FormatUIString("UIAvatarMeridian_Twelve");
			titlelabelText = GameUtility.FormatUIString("UIPnlAvatarMeridian_Twelve");
			beforeChangeText.Text = GameUtility.GetUIString("UIAvatarMeridian_Twelve") + GameUtility.GetUIString("UIAvatarMeridian_Before");
			afterChangeText.Text = GameUtility.GetUIString("UIAvatarMeridian_Twelve") + GameUtility.GetUIString("UIAvatarMeridian_After");
		}
		else if (meridianConfig.type == MeridianConfig._MeridianType.Eight)//洗脉
		{
			maridianBase.Text = GameUtility.FormatUIString("UIAvatarMeridian_Eight");
			titlelabelText = GameUtility.FormatUIString("UIPnlAvatarMeridian_Eight");
			beforeChangeText.Text = GameUtility.GetUIString("UIAvatarMeridian_Eight") + GameUtility.GetUIString("UIAvatarMeridian_Before");
			afterChangeText.Text = GameUtility.GetUIString("UIAvatarMeridian_Eight") + GameUtility.GetUIString("UIAvatarMeridian_After");
		}

		titlelabel.Text = hasUnSavedMeridian ? maridianBase.Text : titlelabelText;
		meridianName.Text = meridianConfig.name;

		// Set Icon.
		UIUtility.CopyIcon(meridianIcon, HasMeridianActivied(meridianData.Modifiers) ? UIElemTemplate.Inst.iconBorderTemplate.iconActiveMeridian : UIElemTemplate.Inst.iconBorderTemplate.iconOpenMeridian);

		//GetAttributeStr(PlayerDataUtility.MergeAttributes(TurnDataStruct(meridianData.Modifiers)));

		// Set Attribute Label.

		List<KodGames.ClientClass.PropertyModifier> modifiers;

		switch (unSaveModifiers == null)
		{
			case true:
				modifiers = PlayerDataUtility.MergeModifiers(meridianData.Modifiers, true, true);
				for (int i = 0; i < afterAttributeLabels.Count; ++i)
				{
					if (i < modifiers.Count)
					{
						afterAttributeLabels[i].Text = string.Format("{0}{1}: {2}+{3}",
							GameDefines.textColorBtnYellow,
							_AvatarAttributeType.GetDisplayNameByType(modifiers[i].AttributeType, ConfigDatabase.DefaultCfg),
							GameDefines.txColorWhite,
							ItemInfoUtility.GetAttribDisplayString(modifiers[i]));
					}
					else
					{
						afterAttributeLabels[i].Text = GameUtility.FormatUIString("UIPnlAvatarMeridian_NotMeridan", GameDefines.txColorYellow3);
					}
				}

				// Set Cost Label.
				int meridianTimes = 0;
				if (meridianData != null)
					meridianTimes = meridianData.Times;

				List<Cost> costs = meridianConfig.GetCostsByMeridianTimes(meridianTimes);

				if (!isFirstActive && costs != null)
				{
					string costStr = "";
					for (int index = 0; index < costs.Count; index++)
					{
						costStr = GameUtility.AppendString(costStr, string.Format("{0}{1}{2}", GameDefines.textColorWhite.ToString(), costs[index].count, ItemInfoUtility.GetAssetName(costs[index].id)), false);
						if (index + 1 < costs.Count)
							costStr = GameUtility.FormatUIString("UIAvatarMeridian_CostContainer", costStr, GameDefines.textColorBtnYellow.ToString());
					}

					saveOrNot.Text = "";

					if (meridianConfig.type == MeridianConfig._MeridianType.Twelve)
						meridianCost.Text = GameUtility.FormatUIString("UIAvatarMeridian_TwelveMeridianCost", GameDefines.textColorBtnYellow, costStr, GameDefines.textColorBtnYellow);
					else
						meridianCost.Text = GameUtility.FormatUIString("UIAvatarMeridian_EightMeridianCost", GameDefines.textColorBtnYellow, costStr, GameDefines.textColorBtnYellow);
				}
				else
					Debug.Log("Meridian " + meridianID.ToString("X") + " no Cost.");


				break;
			case false:

				//洗练 易经前
				modifiers = PlayerDataUtility.MergeModifiers(meridianData.Modifiers, true, true);
				for (int i = 0; i < beforeChangeLabels.Count; ++i)
				{
					if (i < modifiers.Count)
					{
						beforeChangeLabels[i].Text = string.Format("{0}{1}: {2}+{3}",
							GameDefines.textColorBtnYellow.ToString(),
							_AvatarAttributeType.GetDisplayNameByType(modifiers[i].AttributeType, ConfigDatabase.DefaultCfg),
							GameDefines.txColorWhite,
							ItemInfoUtility.GetAttribDisplayString(modifiers[i]));
					}
					else
					{
						beforeChangeLabels[i].Text = GameUtility.FormatUIString("UIPnlAvatarMeridian_NotMeridan", GameDefines.txColorYellow3);
					}
				}

				//洗练 易经 后
				modifiers = PlayerDataUtility.MergeModifiers(unSaveModifiers, true, true);
				for (int i = 0; i < afterChangeLabels.Count; ++i)
				{
					if (i < modifiers.Count)
					{
						afterChangeLabels[i].Text = string.Format("{0}{1}: {2}+{3}",
							GameDefines.textColorBtnYellow.ToString(),
							_AvatarAttributeType.GetDisplayNameByType(modifiers[i].AttributeType, ConfigDatabase.DefaultCfg),
							GameDefines.txColorWhite,
							ItemInfoUtility.GetAttribDisplayString(modifiers[i]));
					}
					else
					{
						afterChangeLabels[i].Text = GameUtility.FormatUIString("UIPnlAvatarMeridian_NotMeridan", GameDefines.txColorYellow3);
					}
				}

				meridianCost.Text = "";
				saveOrNot.Text = GameUtility.FormatUIString("UIAvatarMeridian_AskSave", GameDefines.textColorWhite, maridianBase.Text);

				break;
		}
	}

	private bool HasMeridianActivied(List<KodGames.ClientClass.PropertyModifier> modifiers)
	{
		if (modifiers == null || modifiers.Count <= 0)
			return false;

		foreach (var modifier in modifiers)
			if (modifier.AttributeValue != 0)
				return true;

		return false;
	}

	private void HideChildLayout(bool hasUnSavedMeridian)
	{
		layoutControl.HideChildObj(maridianBase.gameObject, hasUnSavedMeridian);
		layoutControl.HideChildObj(cancelBase.gameObject, !hasUnSavedMeridian);
		layoutControl.HideChildObj(saveBase.gameObject, !hasUnSavedMeridian);

		closeBtn.Hide(hasUnSavedMeridian);

		afterAction.SetActive(hasUnSavedMeridian);
		beforeAction.SetActive(!hasUnSavedMeridian);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		if (saveBase.gameObject.activeInHierarchy)
		{
			HideSelf();
			RequestMgr.Inst.Request(new SaveMeridianReq(avatarData.Guid, false, meridianID));//原始
		}
		else
			HideSelf();

		AddActiveEffect();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMeridian(UIButton btn)
	{
		if (btn.Text == GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK"))
		{
			OnHide();
			AddActiveEffect();
		}
		else
			RequestMgr.Inst.Request(new ChangeMeridianReq(meridianID, avatarData.Guid));//action
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSave(UIButton btn)
	{
		RequestMgr.Inst.Request(new SaveMeridianReq(avatarData.Guid, true, meridianID));//保留
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCancleSaveMeridian(UIButton btn)
	{
		RequestMgr.Inst.Request(new SaveMeridianReq(avatarData.Guid, false, meridianID));//原始
	}

	//洗脉（已经）回调
	public void OnChangeMeridianSuccess(List<KodGames.ClientClass.PropertyModifier> modifiers)
	{
		InitView(modifiers, true, false);

		HideChildLayout(true);
	}

	//保留原始数据还是新的数据
	public void OnSaveMeridianSuccess(bool saveOrNot, List<KodGames.ClientClass.PropertyModifier> modifiers)
	{
		if (saveOrNot && SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatarMeridianTab))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarMeridianTab>().UpDataMeridianUI(meridianID);

		InitView(null, false, false);

		HideChildLayout(false);
	}

	private void AddActiveEffect()
	{
		MeridianConfig.Meridian meridian = ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianByPreMeridianId(meridianID);

		if (this.isFirstActive && SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatarMeridianTab) && meridian != null)
		{
			SysUIEnv.Instance.GetUIModule<UIPnlAvatarMeridianTab>().AddActiveEffect(meridian.id);
		}
	}
}