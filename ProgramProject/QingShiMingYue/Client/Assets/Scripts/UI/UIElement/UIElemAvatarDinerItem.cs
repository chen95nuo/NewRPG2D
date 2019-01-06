using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemAvatarDinerItem : MonoBehaviour
{
	public UIElemAvatarCard avatarCardIcon;
	public UIButton avatarInfoBtn;
	public UIBox avatarBorderIcon;
	public UIBox dinerStatusBox;
	public UIBox dinerTraitBox;
	public UIBox dinerCountryBox;
	public UIElemBreakThroughBtn dinerBreakBox;
	public UIBox messageBox;
	public SpriteText avatarNameLabel;

	// Hire Info.
	public SpriteText avatarLvlLabel;
	public SpriteText hireCostLabel;
	public UIElemAssetIcon hireCostIcon;
	public SpriteText hireDurationLabel;
	public SpriteText hireDurationLabelTime;
	public UIButton dinerButton;
	public UIBox dinerdBox;
	// Hired Info.
	public SpriteText avatarLineUpLabel;
	public SpriteText hireLeftTimeLabel;
	public UIButton fireButton;

	private UIPnlAvatarDiner.ShowData showData;
	public UIPnlAvatarDiner.ShowData ShowData { get { return showData; } }

	public void SetData(UIPnlAvatarDiner.ShowData showData)
	{
		// Set Data.
		this.showData = showData;

		// Init View State.
		avatarLvlLabel.Hide(showData.hired);
		hireCostLabel.gameObject.SetActive(!showData.hired);
		hireDurationLabel.Hide(showData.hired);
		hireDurationLabelTime.Hide(showData.hired);
		dinerButton.Hide(showData.hired);
		dinerdBox.Hide(showData.hired);

		avatarLineUpLabel.Hide(!showData.hired);
		hireLeftTimeLabel.Hide(!showData.hired);
		fireButton.Hide(!showData.hired);

		// AvatarCfg.
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(showData.avatar.ResourceId);

		// Set Avatar Icon.
		avatarCardIcon.SetData(avatarCfg.id, true, false, null);

		// Set Avatar Border Icon.
		SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>().SetBorderIcon(avatarBorderIcon, avatarCfg.qualityLevel);

		// Set DinerStatus.
		switch (showData.state)
		{
			case DinerConfig.DinerRecommendState.Unkonw:
				dinerStatusBox.Hide(true);
				break;

			case DinerConfig.DinerRecommendState.Activity:
				dinerStatusBox.Hide(false);
				UIUtility.CopyIcon(dinerStatusBox, UIElemTemplate.Inst.shopIconTemplate.iconGoodActivity);
				break;

			case DinerConfig.DinerRecommendState.Recommend:
				dinerStatusBox.Hide(false);
				UIUtility.CopyIcon(dinerStatusBox, UIElemTemplate.Inst.shopIconTemplate.iconGoodRecommad);
				break;
		}

		// Set Avatar TraitType.
		UIElemTemplate.Inst.SetAvatarTraitIcon(dinerTraitBox, avatarCfg.traitType);

		//set name
		avatarNameLabel.Text = ItemInfoUtility.GetAssetName(avatarCfg.id);

		// Set Avatar Country.
		UIElemTemplate.Inst.SetAvatarCountryIcon(dinerCountryBox, avatarCfg.countryType);

		// Set Avatar BreakThought.
		dinerBreakBox.SetBreakThroughIcon(showData.avatar.BreakthoughtLevel);

		// Set AvatarIcon Click.
		avatarInfoBtn.Data = this;

		// Set Avatar Message Color.
		switch (avatarCfg.qualityLevel)
		{
			case 3:
				messageBox.SetToggleState("LanSe");
				break;
			case 4:
				messageBox.SetToggleState("ZiSe");
				break;
			case 5:
				messageBox.SetToggleState("ChengSe");
				break;
			default:
				messageBox.SetToggleState("LanSe");
				break;
		}

		if (showData.hired)
		{
			fireButton.Data = this;

			// lineUp Status.
			List<int> positionIds = new List<int>();
			foreach (var position in SysLocalDataBase.Inst.LocalPlayer.PositionData.Positions)
			{
				foreach (var avatarLocation in position.AvatarLocations)
					if (avatarLocation.Guid.Equals(showData.avatar.Guid))
					{
						positionIds.Add(position.PositionId);
						break;
					}
			}

			string lineUpMsg = string.Empty;
			for (int i = 0; i < positionIds.Count; i++)
			{
				string zhenRong = ItemInfoUtility.GetAssetName(positionIds[i]);

				if (i < positionIds.Count - 1)
					lineUpMsg += zhenRong.Substring(zhenRong.Length - 1, 1) + GameUtility.GetUIString("UI_Dot");
				else
					lineUpMsg += zhenRong.Substring(zhenRong.Length - 1, 1);
			}

			if (positionIds.Count <= 0)
				avatarLineUpLabel.Text = GameUtility.FormatUIString("UIPnlAvatarDiner_LineUpEmpty", GameDefines.textColorBtnYellow);
			else
				avatarLineUpLabel.Text = GameUtility.FormatUIString("UIPnlAvatarDiner_LineUp", GameDefines.textColorBtnYellow, GameDefines.txColorWhite, lineUpMsg);
		}
		else
		{
			var queryDiner = SysLocalDataBase.Inst.LocalPlayer.HireDinerData.GetQueryDiner(showData.qualityType, showData.dinerId);
			if (queryDiner == null)
			{
				Debug.Log("QueryDiner Not Found " + showData.qualityType + " " + showData.dinerId.ToString("X8"));
				return;
			}

			dinerButton.Data = this;

			var dinerCfg = ConfigDatabase.DefaultCfg.DinerConfig.GetDinerById(showData.dinerId);
			var dinerPackage = SysLocalDataBase.Inst.LocalPlayer.HireDinerData.GetDinerPackageByQualityType(showData.qualityType);

			// Set Level.
			avatarLvlLabel.Text = GameUtility.FormatUIString("UIPnlAvatarDiner_Level", GameDefines.textColorBtnYellow, showData.avatar.LevelAttrib.Level, GameDefines.textColorInOrgYew, avatarCfg.GetAvatarBreakthrough(showData.avatar.BreakthoughtLevel).breakThrough.powerUpLevelLimit);

			// Set Hire Cost.
			hireCostIcon.SetData(queryDiner.Costs[0].Id);
			hireCostIcon.border.Text = queryDiner.Costs[0].Count.ToString();

			// Set Hired Days.
			hireDurationLabel.Text = GameUtility.FormatUIString("UIPnlAvatarDiner_LeftHiredTime", GameDefines.textColorBtnYellow);
			hireDurationLabelTime.Text = GameUtility.FormatUIString("UIPnlAvatarDiner_HiredTime", GameDefines.txColorWhite, dinerCfg.RemainTime / 24, GameDefines.textColorBtnYellow);

			var hiredDiner = SysLocalDataBase.Inst.LocalPlayer.HireDinerData.GetHiredDiner(showData.qualityType, showData.dinerId);
			if (hiredDiner == null)
			{
				dinerButton.Hide(false);
				dinerdBox.Hide(true);
				dinerButton.methodToInvoke = "OnClickForHire";
				dinerButton.Text = GameUtility.GetUIString("UIPnlAvatarDiner_HireLabel");
			}
			else
			{
				bool hired = hiredDiner.HireTime > dinerPackage.LastRefreshTime;

				dinerButton.Hide(hired);
				dinerdBox.Hide(!hired);
				dinerButton.methodToInvoke = hired ? string.Empty : "OnClickForRenew";
				dinerButton.Text = hired ? string.Empty : GameUtility.GetUIString("UIPnlAvatarDiner_ReHireLabel");
			}
		}

		RefreshDynamicView();
	}

	public void HiredDinerSuccess()
	{
		dinerButton.Hide(true);
		dinerdBox.Hide(false);
	}

	public void RefreshDynamicView()
	{
		if (showData == null)
			return;

		if (showData.hired)
		{
			var hiredDiner = SysLocalDataBase.Inst.LocalPlayer.HireDinerData.GetHiredDiner(showData.qualityType, showData.dinerId);

			if (hiredDiner == null)
				return;

			if (SysLocalDataBase.Inst.LoginInfo.NowTime <= hiredDiner.EndTime)
				hireLeftTimeLabel.Text = GameUtility.FormatUIString("UIPnlAvatarDiner_HiredCountDown", GameDefines.textColorBtnYellow, GameDefines.txColorBlue, GameUtility.Time2String(hiredDiner.EndTime - SysLocalDataBase.Inst.LoginInfo.NowTime));
			else
				hireLeftTimeLabel.Text = string.Empty;
		}
	}

	public void Hide(bool tf)
	{
		this.gameObject.SetActive(!tf);

		this.showData = null;
		this.fireButton.Data = null;
		this.dinerButton.Data = null;

		if (tf)
			avatarCardIcon.Clear();
	}
}
