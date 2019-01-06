using UnityEngine;
using System.Collections;
using ClientServerCommon;
using KodGames;

public class UIDlgPlayerAttrTip : UIModule
{
	public SpriteText playerName;
	public SpriteText playerLevel;
	public SpriteText playerPrestige;
	public SpriteText playerRealMoney;
	public SpriteText playerGameMoney;
	public SpriteText playerSoul;
	public SpriteText playerVip;
	public SpriteText playerPure;
	public SpriteText playerSiderite;
	public SpriteText playerPower;

	//PVP
	public SpriteText playerPVP;
	public SpriteText nextPVPRecoveryTimer;
	public SpriteText totalPVPRecoveryTimer;

	private float lastUpdateDelta = 0f;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		KodGames.ClientClass.Player player = SysLocalDataBase.Inst.LocalPlayer;
		SetPlayerAttrControls(player);
		return true;
	}

	private void SetPlayerAttrControls(KodGames.ClientClass.Player player)
	{
		if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level > ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel
			|| (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level == ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel
			&& SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Experience >= ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel).playerExp)
			)
		{
			int maxLevel = ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel;
			playerLevel.Text = maxLevel.ToString();
			playerPrestige.Text = string.Format("{0}/{1}", ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(maxLevel).playerExp,
				ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(maxLevel).playerExp);

		}
		else
		{
			playerLevel.Text = player.LevelAttrib.Level.ToString();
			playerPrestige.Text = string.Format("{0}/{1}", player.LevelAttrib.Experience, ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(player.LevelAttrib.Level).playerExp);

		}

		playerName.Text = string.IsNullOrEmpty(player.Name) ? player.PlayerId.ToString() : player.Name.ToString();
		playerRealMoney.Text = player.RealMoney.ToString();
		playerGameMoney.Text = player.GameMoney.ToString();
		playerSoul.Text = player.Soul.ToString();
		playerSiderite.Text = player.Spirit.ToString();
		playerPure.Text = player.Iron.ToString();

		int power = (int)PlayerDataUtility.CalculatePlayerPower(player, player.PositionData.ActivePositionId);

		playerPower.Text = PlayerDataUtility.GetPowerString(power);

		if (player.VipLevel > 0)
			playerVip.Text = string.Format("VIP{0}", player.VipLevel);
		else
			playerVip.Text = "";


		UpdateAttributeControls();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabClose(UIButton btn)
	{
		HideSelf();
	}

	private void Update()
	{
		lastUpdateDelta += Time.deltaTime;
		if (lastUpdateDelta > 1f)
		{
			lastUpdateDelta -= 1f;
			UpdateAttributeControls();
		}
	}

	private void UpdateAttributeControls()
	{
		SysLocalDataBase localDataBase = SysLocalDataBase.Inst;

		//Stamina Point.
		float nextPVPTime = localDataBase.LocalPlayer.Stamina.GetNextGenerationLeftTime(localDataBase.LoginInfo.NowTime);
		if (nextPVPTime >= 0)
		{
			if (!GameUtility.Time2String((long)nextPVPTime).Equals(nextPVPRecoveryTimer.Text))
			{
				nextPVPRecoveryTimer.Text = GameUtility.Time2String((long)nextPVPTime);
			}
		}


		float totalPVPTime = localDataBase.LocalPlayer.Stamina.GetFullGenerationLeftTime(localDataBase.LoginInfo.NowTime);
		if (totalPVPTime > 0)
		{
			if (!GameUtility.Time2String((long)totalPVPTime).Equals(totalPVPRecoveryTimer.Text))
			{
				totalPVPRecoveryTimer.Text = GameUtility.Time2String((long)totalPVPTime);
			}
		}
		if (localDataBase.LocalPlayer.Stamina.IsPointFull())
		{
			if (!nextPVPRecoveryTimer.Text.Equals(GameUtility.GetUIString("UIDlgPlayerAttrTip_Label_StaminaFull")))
				nextPVPRecoveryTimer.Text = GameUtility.GetUIString("UIDlgPlayerAttrTip_Label_StaminaFull");

			if (!totalPVPRecoveryTimer.Text.Equals(GameUtility.GetUIString("UIDlgPlayerAttrTip_Label_StaminaFull")))
				totalPVPRecoveryTimer.Text = GameUtility.GetUIString("UIDlgPlayerAttrTip_Label_StaminaFull");
		}

		if (!playerPVP.Text.Equals(localDataBase.LocalPlayer.Stamina.Point.Value.ToString()))
			playerPVP.Text = string.Format("{0}/{1}", localDataBase.LocalPlayer.Stamina.Point.Value, localDataBase.LocalPlayer.Stamina.MaxPoint);
	}
}