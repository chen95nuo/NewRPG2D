#if ENABLE_AUCTION
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemPurchaseDetailCostItem : MonoBehaviour
{
	public UIElemAssetIcon costAvatarIcon;

	public UIButton breakThroughBtn;
	public UIButton changeAvatarBtn;

	public SpriteText costAvatarNameLabel;
	private List<KodGames.ClientClass.Avatar> canDealAvatars;

	private KodGames.ClientClass.Avatar localAvatarData;

	public void SetData(KodGames.ClientClass.Cost cost)
	{
		canDealAvatars = new List<KodGames.ClientClass.Avatar>();
		costAvatarIcon.Hide(false);
		changeAvatarBtn.data = cost;

		foreach (KodGames.ClientClass.Avatar avatar in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (avatar.Bind == 1)
				continue;

			if (cost.Guid == avatar.Guid)
			{
				if (ItemInfoUtility.IsAvatarEquipped(avatar))
					costAvatarIcon.border.spriteText.Color = GameDefines.txColorRed;
				else
					costAvatarIcon.border.spriteText.Color = GameDefines.txColorWhite;
				break;
			}
		}


		costAvatarIcon.SetData(cost.Id);
		foreach (KodGames.ClientClass.Avatar avatar in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (avatar.Bind == 1 || avatar.StatusDidExchange == 1)
				continue;

			if (avatar.ResourceId == cost.Id)
			{
				canDealAvatars.Add(avatar);
			}
		}

		if (canDealAvatars.Count > 1)
		{
			changeAvatarBtn.Hide(false);
			costAvatarIcon.EnableButton(true);
		}
		else if (canDealAvatars.Count == 1)
		{
			changeAvatarBtn.Hide(true);
			costAvatarIcon.EnableButton(true);
		}
		else
		{
			changeAvatarBtn.Hide(true);
			costAvatarIcon.EnableButton(false);
		}

		if (canDealAvatars.Count >= 1)
		{
			costAvatarIcon.border.SetColor(new Color(1f, 1f, 1f, 1f));
			costAvatarIcon.EnableButton(true);
			SetAvatarDetail(canDealAvatars[0]);
			localAvatarData = canDealAvatars[0];
		}
		else
		{
			costAvatarIcon.border.SetColor(new Color(128f / 255f, 128f / 255f, 128f / 255f, 1f));
			costAvatarIcon.EnableButton(false);
			costAvatarNameLabel.Text = string.Format(ConfigDatabase.DefaultCfg.StringsConfig.GetString(GameDefines.strBlkAssetDescs,
					ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(cost.Id).name));
			localAvatarData = null;
		}
	}

	private void SetAvatarDetail(KodGames.ClientClass.Avatar costAvatar)
	{
		KodGames.ClientClass.Cost cost = changeAvatarBtn.data as KodGames.ClientClass.Cost;

		if (costAvatar != null)
		{
			cost.Guid = costAvatar.Guid;

			if (costAvatar.BreakthoughtLevel != 0)
			{
				breakThroughBtn.Hide(false);
				breakThroughBtn.Text = string.Format("+{0}", costAvatar.BreakthoughtLevel);
			}
			if (ItemInfoUtility.IsAvatarEquipped(costAvatar))
			{

				costAvatarNameLabel.Text = string.Format(GameUtility.GetUIString("UIPnlAuction_CostItemLevel"),
					GameDefines.txColorRed, costAvatar.LevelAttrib.Level,
					ConfigDatabase.DefaultCfg.StringsConfig.GetString(GameDefines.strBlkAssetDescs,
					ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(costAvatar.ResourceId).name));
			}
			else
			{
				costAvatarNameLabel.Text = string.Format(GameUtility.GetUIString("UIPnlAuction_CostItemLevel"),
					GameDefines.txColorWhite, costAvatar.LevelAttrib.Level,
					ConfigDatabase.DefaultCfg.StringsConfig.GetString(GameDefines.strBlkAssetDescs,
					ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(costAvatar.ResourceId).name));
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnChangeAvatarClick(UIButton btn)
	{
		UIPnlAuctionChangeAvatar.SelectAvatarCallbackDelegate avatarChangedHandler = OnAvatarChanged;
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlAuctionChangeAvatar, canDealAvatars, avatarChangedHandler, localAvatarData);
	}

	private void OnAvatarChanged(KodGames.ClientClass.Avatar avatar)
	{
		localAvatarData = avatar;
		SetAvatarDetail(avatar);
	}

	public void ResetCostAvatarIconStatus()
	{
		breakThroughBtn.Hide(true);
		changeAvatarBtn.Hide(true);
		costAvatarNameLabel.SetColor(GameDefines.txColorWhite);
		costAvatarNameLabel.Text = "";
		costAvatarIcon.Hide(true);
		changeAvatarBtn.scriptWithMethodToInvoke = this;
		changeAvatarBtn.methodToInvoke = "OnChangeAvatarClick";
	}
}


#endif
