using System.Collections.Generic;
using ClientServerCommon;

public class UIElemEquipIllusionItem : MonoBehaviour
{
	public SpriteText illusionNameLabel;
	public SpriteText timeCountDnLabel;
	public SpriteText itemCostInfoLabel;
	public UIElemAssetIcon illusionIcon;
	public UIBox notifIcon;

	public UIBox lightBox;
	public UIBox usingAttr;
	public UIBox usingSurface;
	public UIBox usingAll;

	private com.kodgames.corgi.protocol.Illusion illusion = null;
	public com.kodgames.corgi.protocol.Illusion Illusion
	{
		get { return illusion; }
	}

	public void SetData(com.kodgames.corgi.protocol.Illusion illusion)
	{
		this.illusion = illusion;

		illusionIcon.border.Data = this;

		//激活中的不遮罩
		SetGrayStyle(illusion.endTime != -1 && illusion.endTime < SysLocalDataBase.Inst.LoginInfo.NowTime);

		//永久激活的或已过期的不显示倒计时
		timeCountDnLabel.Text = string.Empty;
		timeCountDnLabel.Hide(illusion.endTime == -1 || illusion.endTime <= SysLocalDataBase.Inst.LoginInfo.NowTime);		

		usingAttr.Hide(illusion.useStatus != IllusionConfig._UseStatus.UseAttr);//"使用中"
		usingSurface.Hide(illusion.useStatus != IllusionConfig._UseStatus.UseFacade);
		usingAll.Hide(illusion.useStatus != IllusionConfig._UseStatus.UseAll);

		lightBox.Hide(true);

		SetNotifIconState();
		ManageItemCostInfoLabel();

		IllusionConfig.Illusion illusionCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(illusion.illusionId);
		illusionNameLabel.Text = ItemInfoUtility.GetAssetName(illusionCfg.Id);
		illusionIcon.SetData(illusion.illusionId);
		SetCountDownLabelStr(UIPnlAvatarIllusion.TimeToIllusionLeftTime(illusion.endTime - SysLocalDataBase.Inst.LoginInfo.NowTime));
	}

	void ManageItemCostInfoLabel()
	{
		long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		if (illusion.endTime == -1 || illusion.endTime > nowTime)
			itemCostInfoLabel.Hide(true);
		else
		{
			itemCostInfoLabel.Hide(false);
			itemCostInfoLabel.Text = "";

			var illusionCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(illusion.illusionId);
			itemCostInfoLabel.Text = GameUtility.FormatUIString("UIPnlAvatarIllusion_IconItemCostInfoLabel", ItemInfoUtility.GetGameItemCount(illusionCfg.ActivateCost.id), illusionCfg.ActivateCost.count);
		}
	}

	public void SetCountDownLabelStr(string val)
	{
		if (timeCountDnLabel.IsHidden())
			return;

		timeCountDnLabel.Text = val;
	}

	private void SetGrayStyle(bool gray)
	{
		if (gray)
			UIUtility.CopyIconTrans(illusionIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardGray);
		else
			UIUtility.CopyIconTrans(illusionIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardNormal);
	}

	public void SetLight(bool light)
	{
		lightBox.Hide(!light);
	}

	private void SetNotifIconState()
	{
		bool canShowNotifyIcon = false;
		long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

		if (illusion.endTime == -1 || illusion.endTime > nowTime)
			canShowNotifyIcon = false;
		else
		{
			var illusionCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(illusion.illusionId);
			if (illusionCfg != null && ItemInfoUtility.CheckCostEnough(illusionCfg.ActivateCost.id, illusionCfg.ActivateCost.count))
				canShowNotifyIcon = true;
		}

		notifIcon.Hide(!canShowNotifyIcon);
	}
}
