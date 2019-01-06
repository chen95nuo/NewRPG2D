using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemDailyRewardSignInItem : MonoBehaviour
{
	public UIButton signInRewardBtn;
	public UIElemAssetIcon signInRewardIcon;
	public AutoSpriteControlBase signInRewardFinshIcon;
	public DailySignInConfig.StepReward stepReward;
	public SpriteText signInCountText;

	public void SetData(DailySignInConfig.StepReward reward)
	{
		//this.stepReward = GameUtility.GetItemInList(ConfigDatabase.DefaultCfg.DailySignInConfig.stepRewards, stepRewardIndex);

		if (reward != null)
		{
			this.stepReward = reward;

			Hide(false);

			// Set the setReward Icon and button's Data.
			signInRewardBtn.Data = this;

			//if (stepReward.rewards > 0)
			signInRewardIcon.SetData(stepReward.rewards[0].id, stepReward.rewards[0].count);

			if (stepReward.signInCount == _SignCount.WholeMonth)
				signInCountText.Text = GameUtility.FormatUIString("UIPnlDaily_SiginIn_Complement", GameDefines.textColorBtnYellow);
			else
				signInCountText.Text = GameUtility.FormatUIString(("UIPnlDaily_StepRewardSiginIn_Times"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, stepReward.signInCount, GameDefines.textColorBtnYellow);

			// Set the signInReawrdFinishIcon state.
			SetRewardFinishIconState();

		}
		else
		{
			this.stepReward = null;
			Hide(true);
		}
	}

	public void SetRewardFinishIconState()
	{
		if (this.stepReward == null)
			return;

		System.DateTime serverTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
		int daysInMonth = System.DateTime.DaysInMonth(serverTime.Year, serverTime.Month);

		int signCount = stepReward.signInCount == _SignCount.WholeMonth ? daysInMonth : stepReward.signInCount;
		bool hasFinish = signCount <= SysLocalDataBase.Inst.LocalPlayer.SignData.SignCount;

		signInRewardFinshIcon.Hide(!hasFinish);
	}

	public void Hide(bool hide)
	{
		signInRewardBtn.Hide(hide);
		signInRewardIcon.Hide(hide);
		signInRewardFinshIcon.Hide(hide);
		signInCountText.Hide(hide);
	}

}
