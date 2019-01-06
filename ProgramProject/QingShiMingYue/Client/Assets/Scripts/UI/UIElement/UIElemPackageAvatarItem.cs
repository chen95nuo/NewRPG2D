//using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemPackageAvatarItem : UIElemPackageItemBase
{
	public UIElemAssetIcon itemIcon;
	public UIButton evelateBtn;
	public List<UIButton> DetailBtns;//"Ïê"
	public SpriteText itemNameText;
	public SpriteText itemQualitText;
	public SpriteText itemGrownText;
	public SpriteText itemLineText;

	private KodGames.ClientClass.Avatar avatar;
	public KodGames.ClientClass.Avatar Avatar { get { return avatar; } }

	public void SetData(KodGames.ClientClass.Avatar avatar)
	{
		this.avatar = avatar;
		container.Data = this;

		// Set the avatar icon and Data.
		itemIcon.SetData(avatar);
		itemIcon.Data = avatar;

		// Set the avatar name.
		itemNameText.Text = ItemInfoUtility.GetAssetName(avatar.ResourceId);

		// Set the avatar quality label.
		itemQualitText.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(avatar.ResourceId);

		bool avatarLineUped = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, avatar);
		if (!avatarLineUped)
			avatarLineUped = PlayerDataUtility.IsLineUpInParter(SysLocalDataBase.Inst.LocalPlayer, avatar);

		// Set the avatar LineUp.
		itemLineText.Text = avatarLineUped ? GameUtility.FormatUIString("UIPnlAvatar_State_LineUp", GameDefines.txColorWhite.ToString()) : "";

		// Set the avatar cheer.
		if (string.IsNullOrEmpty(itemLineText.Text))
			itemLineText.Text = ItemInfoUtility.IsAvatarCheered(avatar) ? GameUtility.FormatUIString("UIPnlAvatar_State_Encourage", GameDefines.txColorAttGreen.ToString()) : "";

		// Set the avatar grownText.
		itemGrownText.Text = string.Format(GameUtility.GetUIString("UI_Growth"), ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).growthDesc);

		// Set the EveLateButton's Data.
		evelateBtn.Data = avatar;

		// Set the DetailButton's Data.
		for (int i = 0; i < DetailBtns.Count; i++)
		{
			DetailBtns[i].Data = avatar;
			DetailBtns[i].Hide(!avatarLineUped);
		}
	}
}
