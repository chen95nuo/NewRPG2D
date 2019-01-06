using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemChooseCardAvatar : UIElemChooseCardBasic
{
	public SpriteText avatarPos;

	public void SetData(KodGames.ClientClass.Avatar avatar)
	{
		SetData(avatar, false);
	}

	public void SetData(KodGames.ClientClass.Avatar avatar, bool selected)
	{
		SetBaseData(avatar.ResourceId, avatar.Guid, selected);

		itemBg.Data = this;
		itemIcon.SetData(avatar);

		avatarPos.Text =string.Format("{0}{1}",GameDefines.textColorBtnYellow, GameUtility.FormatUIString("UI_Growth", string.Format("{0}{1}", GameDefines.textColorMessWhite, ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).growthDesc)));
	}
}
