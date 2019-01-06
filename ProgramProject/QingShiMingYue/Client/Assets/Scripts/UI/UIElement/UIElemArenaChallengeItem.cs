using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemArenaChallengeItem : MonoBehaviour
{
	public SpriteText playerNameLabel;
	public UIBox playerRankBox;
	public SpriteText playerScoreLabel;
	public SpriteText playerLevelLabel;
	public SpriteText playerPowerLabel;
	public UIButton fightBtn;

	public List<UIElemAssetIcon> avatarIcons;

	private com.kodgames.corgi.protocol.PlayerRecord playerRecord;
	public com.kodgames.corgi.protocol.PlayerRecord PlayerRecord
	{
		get { return playerRecord; }
	}

	public void SetData(com.kodgames.corgi.protocol.PlayerRecord record, MainMenuItem menu, params object[] userDatas)
	{
		playerRecord = record;

		// Set Rank State.
		if (record.rank == 1)
			playerRankBox.SetToggleState("RankLevel1");
		else if (record.rank == 2)
			playerRankBox.SetToggleState("RankLevel2");
		else
			playerRankBox.SetToggleState("Others");

		//Common information
		playerNameLabel.Text = record.playerName;
		playerRankBox.Text = record.rank.ToString();
		playerLevelLabel.Text = record.playerLevel.ToString();

		if (record.power > 0)
			playerPowerLabel.Text = PlayerDataUtility.GetPowerString(record.power);
		else
			playerPowerLabel.Text = GameUtility.GetUIString("UIPnlAvatar_PowerNull");

		playerScoreLabel.Text = string.Format("+{0}", playerRecord.speed);

		//Set fight button state.
		fightBtn.scriptWithMethodToInvoke = menu.ScriptMethodToInvoke;
		fightBtn.methodToInvoke = menu.MethodToInvoke;
		fightBtn.Text = menu.ControlText;
		fightBtn.data = record;

		Dictionary<int, int> dic_avatar = new Dictionary<int, int>();

		int index = 0;
		for (; index < record.avatarResourceIds.Count; index++)
		{
			int battleIndex = PlayerDataUtility.GetIndexPosByBattlePos(record.avatarBattlePositions[index]);
			if (battleIndex < ConfigDatabase.DefaultCfg.GameConfig.maxColumnInFormation && !dic_avatar.ContainsKey(battleIndex))
				dic_avatar.Add(battleIndex, record.avatarResourceIds[index]);
		}

		for (index = 0; index < avatarIcons.Count; index++)
		{
			if (dic_avatar.ContainsKey(index))
				avatarIcons[index].SetData(dic_avatar[index]);
			else
				avatarIcons[index].SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconBgBtn, null);
		}
	}
}
