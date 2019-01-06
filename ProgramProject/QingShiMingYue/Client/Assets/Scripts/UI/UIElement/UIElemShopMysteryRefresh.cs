using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class UIElemShopMysteryRefresh : MonoBehaviour
{
	public UIElemAssetIcon bgIcon;
	public UIElemAssetIcon refreshTypeIcon;
	public UIElemAssetIcon costIcon;
	public SpriteText constraintLabel;
	public SpriteText costLabel;
	public UIButton refreshButton;

	public void SetData(MysteryShopConfig.Refresh refresh)
	{
		if (SysLocalDataBase.Inst.LocalPlayer.VipLevel >= refresh.vipLevel)
			constraintLabel.Text = string.Empty;
		else constraintLabel.Text = GameUtility.FormatUIString("UIElemShopMysteryRefresh_Constraint", "VIP ", refresh.vipLevel);

		costIcon.SetData(refresh.cost.id);
		costLabel.Text = refresh.cost.count.ToString();
		refreshButton.Data = refresh;
		refreshTypeIcon.SetData(refresh.iconId);

		//bgIcon.SetData(refresh.backgroundIconId);
	}
}