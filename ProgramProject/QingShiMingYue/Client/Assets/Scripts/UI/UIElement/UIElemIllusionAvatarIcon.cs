using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using ClientServerCommon;

public class UIElemIllusionAvatarIcon : MonoBehaviour
{
	public UIElemAssetIcon avatarIcon;
	public UIBox notifIcon;
	public UIBox maskBox;
	public SpriteText avatarName;
	public UIButton avatarBtn;

	private com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar;
	public com.kodgames.corgi.protocol.IllusionAvatar IllusionAvatar
	{
		get { return illusionAvatar; }
	}

	public void SetData(com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar)
	{
		if (illusionAvatar == null)
			return;

		this.illusionAvatar = illusionAvatar;
		avatarIcon.SetData(illusionAvatar.recourseId);
		avatarName.Text = ItemInfoUtility.GetAssetName(illusionAvatar.recourseId);
		avatarBtn.Data = this;

		SetIconSelected();
		SetNotifIconState();
	}

	private void SetIconSelected()
	{
		bool alreadyHave = SysLocalDataBase.Inst.LocalPlayer.CardIds.Contains(illusionAvatar.recourseId);

		if (alreadyHave)
		{
			maskBox.SetToggleState(0);
			UIUtility.CopyIconTrans(avatarIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardNormal);
		}
		else
		{
			maskBox.SetToggleState(1);
			UIUtility.CopyIconTrans(avatarIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardGray);
		}
	}

	private void SetNotifIconState()
	{
		notifIcon.Hide(!HasIllusionCanActive());
	}

	bool HasIllusionCanActive()
	{
		long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		foreach (var illusion in illusionAvatar.illusions)
		{
			if (illusion.endTime == -1 || illusion.endTime > nowTime)
				continue;

			var illusionCfg = ConfigDatabase.DefaultCfg.IllusionConfig.GetIllusionById(illusion.illusionId);
			if (illusionCfg == null)
			{
				Debug.LogError(string.Format("Invalid IllusionID {0}", illusion.illusionId.ToString("X8")));
				continue;
			}

			//if (!ItemInfoUtility.CheckCostEnough(illusionCfg.ActivateCost.id, illusionCfg.ActivateCost.count)
			//&& !UIPnlAvatarIllusion.SellInNormalShop(illusionCfg.ActivateCost.id))
			if (ItemInfoUtility.CheckCostEnough(illusionCfg.ActivateCost.id, illusionCfg.ActivateCost.count))
				return true;
		}

		return false;
	}

}
