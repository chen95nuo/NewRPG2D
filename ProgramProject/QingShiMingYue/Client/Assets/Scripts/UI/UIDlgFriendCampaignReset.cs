using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgFriendCampaignReset : UIModule
{
	public SpriteText titleLabel;
	public SpriteText messageLabel;

	public UIBox resetBox;
	public SpriteText resetCountLabel;

	public UIChildLayoutControl layoutCoutrol;//按钮组

	private bool resetSuccess;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		Init(false, (int)userDatas[0]);

		return true;
	}

	private void Init(bool resetSussec, int resetCount)
	{
		this.resetSuccess = resetSussec;

		titleLabel.Text = GameUtility.GetUIString("UIDlgFriendCampaignReset_TopTitleLabel");

		if (resetSussec)
		{
			SetLYC(false, true);
			messageLabel.Text = GameUtility.GetUIString("UIDlgFriendCampaignReset_SuccessLabel");
			resetBox.Hide(true);
			resetCountLabel.Text = string.Empty;
		}
		else
		{
			int count = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.FriendCampaignResetCount) - resetCount;

			resetBox.Hide(false);
			resetCountLabel.Text = GameUtility.FormatUIString("UIDlgFriendCampaignReset_ResetCountNumber", GameDefines.textColorBtnYellow.ToString(),
									GameDefines.textColorWhite.ToString(),
									count,
									ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.FriendCampaignResetCount));
			if (count > 0)
			{
				SetLYC(false, false);
				messageLabel.Text = GameUtility.GetUIString("UIDlgFriendCampaignReset_ResetCountMessage");
			}
			else
			{
				SetLYC(false, true);
				messageLabel.Text = GameUtility.GetUIString("UIDlgFriendCampaignReset_ResetCountNotMessage");
			}
		}
	}

	public void ResetFriendCampaignSuccess()
	{
		Init(true, 0);
	}

	private void SetLYC(params bool[] hideF)
	{
		for (int index = 0; index < Mathf.Min(hideF.Length, layoutCoutrol.childLayoutControls.Length); index++)
			layoutCoutrol.HideChildObj(layoutCoutrol.childLayoutControls[index].gameObject, hideF[index]);
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		if (this.resetSuccess)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendBattle)))
				SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendBattle));

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendCombatTab)))
				SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendCombatTab));

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendInfoTab)))
				SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendInfoTab));

			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);
		}

		HideSelf();
	}

	//点击重置
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickResetBtn(UIButton btn)
	{
		RequestMgr.Inst.Request(new ResetFriendCampaignReq());
	}
}
