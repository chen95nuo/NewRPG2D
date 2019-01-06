using UnityEngine;
using ClientServerCommon;

public class UIPnlPlayerInfoBase : UIModule
{
	public SpriteText playerNameLabel;
	public SpriteText playerVipLabel;
	public SpriteText levelLabel;
	public UIProgressBar expPro;
	public SpriteText realMoneyLabel;
	public SpriteText gameMoneyLabel;
	public SpriteText staminaLabel;
	public AutoSpriteControlBase vipBg;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		//Init the top menu items.
		RefreshView(true);

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected void OnVIPClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlRechargeVip);
	}

	public void RefreshView()
	{
		RefreshView(false);
	}

	public void RefreshView(bool forceRefresh)
	{
		if (forceRefresh || (int)realMoneyLabel.Data != SysLocalDataBase.Inst.LocalPlayer.RealMoney)
		{
			realMoneyLabel.Data = SysLocalDataBase.Inst.LocalPlayer.RealMoney;
			realMoneyLabel.Text = ItemInfoUtility.GetItemCountStr((int)realMoneyLabel.Data);
		}

		if (forceRefresh || (int)gameMoneyLabel.Data != SysLocalDataBase.Inst.LocalPlayer.GameMoney)
		{
			gameMoneyLabel.Data = SysLocalDataBase.Inst.LocalPlayer.GameMoney;
			gameMoneyLabel.Text = ItemInfoUtility.GetItemCountStr((int)gameMoneyLabel.Data);
		}

		int curStamina = KodGames.Math.CombineValue(SysLocalDataBase.Inst.LocalPlayer.Stamina.Point.Value, SysLocalDataBase.Inst.LocalPlayer.Stamina.MaxPoint);
		if (forceRefresh || (int)staminaLabel.Data != curStamina)
		{
			staminaLabel.Data = curStamina;
			string str = SysLocalDataBase.Inst.LocalPlayer.Stamina.Point.Value >= 1000 ? GameUtility.FormatUIString("UIPnlPlayerInfos_ADD") : SysLocalDataBase.Inst.LocalPlayer.Stamina.Point.Value.ToString();
			str = str + GameUtility.FormatUIString("UIPnlPlayerInfos_Max", SysLocalDataBase.Inst.LocalPlayer.Stamina.MaxPoint);
			staminaLabel.Text = str;
		}

		if (string.IsNullOrEmpty(SysLocalDataBase.Inst.LocalPlayer.Name))
		{
			if (forceRefresh || !playerNameLabel.Text.Equals(SysLocalDataBase.Inst.LocalPlayer.PlayerId))
				playerNameLabel.Text = SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString();
		}
		else if (forceRefresh || !playerNameLabel.Text.Equals(SysLocalDataBase.Inst.LocalPlayer.Name))
			playerNameLabel.Text = SysLocalDataBase.Inst.LocalPlayer.Name.ToString();

		if (playerVipLabel != null)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.VipLevel > 0)
			{
				if (forceRefresh || !GameUtility.EqualsFormatString(playerVipLabel.Text, "VIP{0}", SysLocalDataBase.Inst.LocalPlayer.VipLevel))
					playerVipLabel.Text = string.Format("VIP{0}", SysLocalDataBase.Inst.LocalPlayer.VipLevel);
			}
			else
				playerVipLabel.Text = string.Format("VIP{0}", SysLocalDataBase.Inst.LocalPlayer.VipLevel);
		}
		if (vipBg != null)
			vipBg.Hide(string.IsNullOrEmpty(playerVipLabel.Text));

		int actualLevel = Mathf.Min(SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level, ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel);
		if (forceRefresh || (int)levelLabel.Data != actualLevel)
		{
			levelLabel.Data = actualLevel;
			levelLabel.Text = string.Format("LV{0}", actualLevel);
		}
		if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level > ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel
			|| (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level == ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel
			&& SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Experience >= ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(ConfigDatabase.DefaultCfg.LevelConfig.playerMaxLevel).playerExp)
			)
		{
			if (expPro.Value != 1.0f)
				expPro.Value = 1.0f;
			if (!GameUtility.EqualsFormatString(expPro.Text, "{0}/{1}", ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp, ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp))
				expPro.Text = GameUtility.FormatUIString("UIPnlPlayerInfos_CountWithMax", ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp, ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp);
		}
		else
		{
			float prgValue = (float)SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Experience / (float)ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp;
			if (!prgValue.Equals(expPro.Value))
				expPro.Value = prgValue;

			if (!GameUtility.EqualsFormatString(expPro.Text, "{0}/{1}", SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Experience, ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp))
				expPro.Text = GameUtility.FormatUIString("UIPnlPlayerInfos_CountWithMax", SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Experience, ConfigDatabase.DefaultCfg.LevelConfig.GetLevelByLevel(actualLevel).playerExp);
		}
	}
}