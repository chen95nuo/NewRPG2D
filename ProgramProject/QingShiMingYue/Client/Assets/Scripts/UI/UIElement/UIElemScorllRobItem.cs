using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemScorllRobItem : MonoBehaviour
{
	public SpriteText nameLabel;
	public SpriteText levelLabel;
	public List<UIElemAssetIcon> avatarIcons;
	public SpriteText scorllLabel;
	public UIButton operateBtn;

	public void SetData(KodGames.ClientClass.PlayerRecord player, string buttonName)
	{
		// Player name
		nameLabel.Text = player.PlayerName;

		// Player Level
		levelLabel.Text = GameUtility.FormatUIString("Avatar_Lable_Level", player.PlayerLevel);

		// Player avatars icon
		for (int index = 0; index < avatarIcons.Count; index++)
		{
			avatarIcons[index].Hide(index >= player.AvatarResourceIds.Count);
			if (index < player.AvatarResourceIds.Count)
				avatarIcons[index].SetData(player.AvatarResourceIds[index]);
				
		}

		// Set fragment name
		if (player.Datas.Count != 0)
			scorllLabel.Text = ItemInfoUtility.GetAssetName(player.Datas[0]);

		// Set button
		operateBtn.Text = buttonName;
		operateBtn.Data = player;
	}
}
