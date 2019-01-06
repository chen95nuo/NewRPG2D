using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemEastSeaElementRankingList : MonoBehaviour
{
	public SpriteText nameLable;
	public SpriteText eastSeaNumLable;
	public UIElemAssetIcon exchangeIcon;
	public SpriteText noPlayName;
	public SpriteText posText;
	public SpriteText rankText;

	public void SetData(KodGames.ClientClass.ZentiaRank zentiaRank)
	{

		if (zentiaRank != null)
		{
			if (zentiaRank.PlayerId != -1)
			{
				SetNoRankName(false);
				nameLable.Text = zentiaRank.PlayerName;
				eastSeaNumLable.Text = zentiaRank.PlayerZentiaPoint.ToString();
			}
			else
			{
				SetNoRankName(true);
			}
			if (zentiaRank.IconId != 0)
				exchangeIcon.SetData(zentiaRank.IconId);
			exchangeIcon.GetComponent<UIButton>().Data = zentiaRank;
			rankText.Text = zentiaRank.Rank.ToString();
		}
	}

	private void SetNoRankName(bool isNo)
	{
		posText.gameObject.SetActive(!isNo);
		nameLable.gameObject.SetActive(!isNo);
		noPlayName.gameObject.SetActive(isNo);
		eastSeaNumLable.gameObject.SetActive(!isNo);
	}
}
