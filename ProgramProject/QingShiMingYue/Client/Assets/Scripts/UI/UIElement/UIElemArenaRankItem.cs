using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemArenaRankItem : MonoBehaviour 
{
	public SpriteText myPlayerName;
	public SpriteText myPlayerLevel;
	public SpriteText myPlayerRank;
	public SpriteText myPlayerArenaHonorPoint;
	public SpriteText myPlayerTotalArenaHonorPoint;
	public SpriteText myPlayerRankArenaHonorPoint;
	public UIButton refreshBtn;
	
	private com.kodgames.corgi.protocol.PlayerRecord playerRecord;
	public com.kodgames.corgi.protocol.PlayerRecord PlayerRecord
	{
		get { return playerRecord; }
	}
	
	public void SetData(com.kodgames.corgi.protocol.PlayerRecord myPlayer, int selfHonorPoint)
	{
		refreshBtn.data = this;
		playerRecord = myPlayer;
		
		myPlayerName.Text = myPlayer.playerName;
		myPlayerLevel.Text = myPlayer.playerLevel.ToString();
		myPlayerRank.Text = myPlayer.rank.ToString();
		
		//Calculate arena honor point according to rank level.
		//int rewardCount = ConfigDatabase.DefaultCfg.ArenaConfig.GetRankSettingByRankLevel(myPlayer.Datas[0],myPlayer.Datas[0]).rewards.Count;
		//int arenaHonorPoint = 0;
		//for (int index = 0; index < rewardCount; index++)
		//{
		//    Reward reward = ConfigDatabase.DefaultCfg.ArenaConfig.GetRankSettingByRankLevel(myPlayer.Datas[0],myPlayer.Datas[0]).rewards[index];
		//    if (reward.id == IDSeg._SpecialId.ArenaHonorPoint)
		//    {
		//        arenaHonorPoint += reward.count;
		//    }
		//}
		myPlayerArenaHonorPoint.Text = string.Format("+{0}", myPlayer.speed);
		
		//myPlayerTotalArenaHonorPoint.Text = selfHonorPoint.ToString();
		//myPlayerRankArenaHonorPoint.Text = string.Format(GameUtility.GetUIString("UIPnlArena_MyScore"),
		//                                                 GameDefines.txColorGreen, ConfigDatabase.DefaultCfg.ArenaConfig.restoreArenaChallengeTime, GameDefines.txColorWhite,
		//                                                 GameDefines.txColorGreen, arenaHonorPoint);
	}
	
	public void SetSelfArenaHonorPoint(int selfHonorPoint)
	{
		myPlayerTotalArenaHonorPoint.Text = selfHonorPoint.ToString();
	}
}
