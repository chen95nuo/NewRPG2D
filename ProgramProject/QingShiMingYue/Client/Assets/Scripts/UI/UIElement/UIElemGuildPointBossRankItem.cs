using UnityEngine;
using System.Collections;
using ClientServerCommon;
using com.kodgames.corgi.protocol;

public class UIElemGuildPointBossRankItem : MonoBehaviour 
{
	public SpriteText guideTitleLabel;
	public UIButton guideInfoBtn;
	
	public void SetData(BossRank bossRank)
	{
		guideInfoBtn.Data = bossRank;
		guideTitleLabel.Text =bossRank.name;
	}
	
}
