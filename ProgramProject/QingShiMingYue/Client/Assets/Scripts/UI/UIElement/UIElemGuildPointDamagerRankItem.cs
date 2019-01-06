using UnityEngine;
using System.Collections;
using ClientServerCommon;
using com.kodgames.corgi.protocol;

public enum DamagerRankItemData
{
	None,
	Schedule,
	Damage,
	Racing
}

public class UIElemGuildPointDamagerRankItem : MonoBehaviour
{
	public UIElemAssetIcon icon;
	public SpriteText nameLabel;
	public SpriteText towerLebel;
	public SpriteText explorLabel;

	public void SetData(Rank rank, DamagerRankItemData itemType)
	{
		towerLebel.Text = rank.rankValue.ToString();
		nameLabel.Text = rank.name.ToString();
		explorLabel.Text = rank.intValue.ToString();
		if (icon != null && rank.showReward != null)
			icon.SetData(rank.showReward.id,rank.showReward.count);
		switch (itemType)
		{
			case DamagerRankItemData.Damage:
				explorLabel.Text = ((int)rank.damage).ToString();
				if (icon != null && rank.showReward != null)
					icon.GetComponent<UIBox>().spriteText.Text = GameUtility.FormatUIString("UIPnlGuildPointDamageRank_Money", rank.showReward.count);
				break;
			case DamagerRankItemData.Racing:
				if (icon != null && rank.showReward != null)
					icon.GetComponent<UIButton>().Data = rank.showReward.id;
				break;
		}

	}

}
