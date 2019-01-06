using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemAvatarSelectListItem : MonoBehaviour
{
	//Icon.
	public UIElemAssetIcon avatarIconBtn;

	//Attributes.
	public SpriteText avatarNameLabel;
	public SpriteText avatarQualityLabel;
	public SpriteText avatarGrowUpLabel;
	public SpriteText equipOwnerLabel;
	// Select Item.
	public UIButton selectBtn;

	//Local data.
	public KodGames.ClientClass.Avatar AvatarData;
	public int indexInAvatarList;

	public void SetData(KodGames.ClientClass.Avatar avatar)
	{
		this.AvatarData = avatar;

		//Set avatar icon.
		avatarIconBtn.SetData(avatar);
		avatarIconBtn.Data = this;
		this.selectBtn.Data = this;
		this.selectBtn.IndexData = avatar.ResourceId;

		//Set avatar name.
		avatarNameLabel.Text = ItemInfoUtility.GetAssetName(avatar.ResourceId);

		// Growth Description.
		avatarGrowUpLabel.Text = GameUtility.FormatUIString("UI_Growth", ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).growthDesc);

		// Set equip Quality Description.
		avatarQualityLabel.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(AvatarData.ResourceId);

		// Set equip owner.
		equipOwnerLabel.Text = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, avatar.Guid, avatar.ResourceId) ?
							   GameUtility.GetUIString("UIPnl_LineUp") :
							   (PlayerDataUtility.IsLineUpInParter(SysLocalDataBase.Inst.LocalPlayer, avatar) ? GameUtility.GetUIString("UIPnl_Cheer") : string.Empty);
	}
}
