using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using ClientServerCommon;
using KodGames;

public class UIPnlGuildTab : UIModule
{
	public SpriteText nameLabel;
	public SpriteText DeclarationLabel;
	public SpriteText guildMoneyLabel;
	public SpriteText guildForceLabel;
	public SpriteText guildContributeLabel;
	public UIButton declarationChangeBtn;
	public UIButton infoBtn;
	public UIButton constructBtn;
	public UIButton shopBtn;
	public UIButton rankBtn;
	public UIButton combatBtn;
	public GameObject newMsgRoot;
	public UIBox shopNotify;
	public UIBox constructNotify;

	private System.DateTime nextRefreshTime;
	private KodGames.ClientClass.GuildMiniInfo GuildMiniInfo
	{
		get { return SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo; }
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas.Length > 0)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), (string)userDatas[0]);

		if (GuildMiniInfo != null && GuildMiniInfo.GuildId > 0)
			SetData();
		else
			RequestMgr.Inst.Request(new GuildQueryReq(() =>
			{
				SetData();
				return true;
			}));

		this.nextRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;

		return true;
	}

	public void SetData()
	{
		GuildMiniInfo miniInfo = SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo;

		nameLabel.Text = GameUtility.FormatUIString("UIDlgGuildTab_NameAndLV", GameDefines.textColorGuildInfo, miniInfo.GuildName, GameDefines.textColorWhite, miniInfo.GuildLevel);

		DeclarationLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildAnnouncement", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, ItemInfoUtility.GetGuildAnnouncement());

		//if (miniInfo.RoleId <= 0)
		//    miniInfo.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		var roleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(miniInfo.RoleId);

		declarationChangeBtn.Hide(roleCfg.Rights.Contains(GuildConfig._RoleRightType.ChangeDeclaration) == false);

		guildMoneyLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildMoney", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, SysLocalDataBase.Inst.LocalPlayer.GuildMoney);

		guildForceLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildMoveCount", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, ItemInfoUtility.GetGameItemCount(ConfigDatabase.DefaultCfg.ItemConfig.exploreItem));

		guildContributeLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildBossCount", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, SysLocalDataBase.Inst.LocalPlayer.GuildBossCount);

		SetNotifyView();

	}

	private void Update()
	{
		if (this.IsShown == false || this.IsOverlayed)
			return;

		if (SysLocalDataBase.Inst.LoginInfo.NowDateTime < nextRefreshTime)
			return;

		nextRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime.AddSeconds(1);

		if (GuildMiniInfo != null)
			SetNotifyView();
	}

	private void SetNotifyView()
	{
		// Set Message Notify.
		if (newMsgRoot != null && GuildMiniInfo != null)
			newMsgRoot.SetActive(GuildMiniInfo.MsgLeft > 0);

		// Set Construct Notify.
		constructNotify.Hide(!SysLocalDataBase.Inst.GetPlayerFunctionStatus(GreenPointType.GuildConstruction));

		// Set Shop Notify.
		if (GuildMiniInfo.PublicShopNextRefreshTime < SysLocalDataBase.Inst.LoginInfo.NowTime)
			GuildMiniInfo.PublicShopNextRefreshTime = TimeEx.DateTimeToInt64(TimeEx.GetTimeAfterTime(
												TimeEx.ToUTCDateTime(ConfigDatabase.DefaultCfg.GuildPublicShopConfig.RefreshTime),
												SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(GuildMiniInfo.PublicShopNextRefreshTime),
												_TimeDurationType.Day).AddHours(-SysLocalDataBase.Inst.LoginInfo.ServerTimeZone));

		bool hideShopNotify = GuildMiniInfo.PublicShopNextRefreshTime <= 0;
		if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.PublicShopLastNotifyTime > 0)
		{
			var lastNotifyTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.PublicShopLastNotifyTime);
			nextRefreshTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(GuildMiniInfo.PublicShopNextRefreshTime);
			var lastRefreshTime = TimeEx.GetTimeBeforeTime(
										TimeEx.ToUTCDateTime(ConfigDatabase.DefaultCfg.GuildPublicShopConfig.RefreshTime),
										nextRefreshTime,
										_TimeDurationType.Day);

			hideShopNotify = hideShopNotify || (lastNotifyTime >= lastRefreshTime && lastNotifyTime <= nextRefreshTime);
		}

		shopNotify.Hide(hideShopNotify);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChange(UIButton btn)
	{
		//修改公告界面
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgGuildNotifyModify), false, new UIDlgGuildNotifyModify.OnModifySuccess(() =>
		{
			DeclarationLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildAnnouncement", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, ItemInfoUtility.GetGuildAnnouncement());
		}));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChat(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildChat);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickInfo(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildIntroInfo);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickShop(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildPublicShop);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickConstruct(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildConstruct);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRank(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIPnlGuildRankList>();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCombat(UIButton btn)
	{
		//关卡界面
		RequestMgr.Inst.Request(new QueryGuildStageReq(GuildStageConfig._LayerType.Now));
	}
}
