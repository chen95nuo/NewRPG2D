using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemArenaMyRecordItem : MonoBehaviour
{
	public SpriteText myPlayerName;
	public SpriteText myPlayerLevel;
	public UIBox myPlayerRankBox;
	public SpriteText myPlayerArenaHonorPoint;
	public SpriteText myPlayerTotalArenaHonorPoint;
	public SpriteText myPlayerRankArenaHonorPoint;
	public SpriteText myPlayerPowerLabel;
	public UIButton refreshBtn;

	private com.kodgames.corgi.protocol.PlayerRecord playerRecord;
	public com.kodgames.corgi.protocol.PlayerRecord PlayerRecord
	{
		get { return playerRecord; }
	}

	public void SetData(com.kodgames.corgi.protocol.PlayerRecord myPlayer, long selfHonorPoint, int myGradeId)
	{
		refreshBtn.data = this;
		playerRecord = myPlayer;

		SetSelfArenaHonorPoint(selfHonorPoint);
		myPlayerName.Text = myPlayer.playerName;
		myPlayerLevel.Text = myPlayer.playerLevel.ToString();
		myPlayerRankBox.Text = playerRecord.rank.ToString();

		int powerValue = (int)PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		if (powerValue > 0)
			myPlayerPowerLabel.Text = PlayerDataUtility.GetPowerString(powerValue);
		else
			myPlayerPowerLabel.Text = GameUtility.GetUIString("UIPnlAvatar_PowerNull");

		if (myPlayer.rank == 1)
			myPlayerRankBox.SetToggleState("RankLevel1");
		else if (myPlayer.rank == 2)
			myPlayerRankBox.SetToggleState("RankLevel2");
		else
			myPlayerRankBox.SetToggleState("Others");

		myPlayerArenaHonorPoint.Text = string.Format("+{0}", playerRecord.speed);

		int leftTime = ConfigDatabase.DefaultCfg.ArenaConfig.GetArenaGradeById(myGradeId).keepRandInterval;

		myPlayerRankArenaHonorPoint.Text = string.Format(GameUtility.GetUIString("UIPnlArena_MyScore"),
					GameDefines.txColorGreen, leftTime / 60, GameDefines.txColorWhite,
					GameDefines.txColorGreen, playerRecord.speed);
	}

	public void SetSelfArenaHonorPoint(long selfHonorPoint)
	{
		if (selfHonorPoint < 10000)
			myPlayerTotalArenaHonorPoint.Text = selfHonorPoint.ToString();
		else if (!myPlayerTotalArenaHonorPoint.Text.Equals(GameUtility.FormatUIString("ConsumableCount", selfHonorPoint / 10000)))
			myPlayerTotalArenaHonorPoint.Text = GameUtility.FormatUIString("ConsumableCount", selfHonorPoint / 10000);
	}
}
