using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using System;
using KodGames;


public class UIPnlGuildRacingRankList : UIModule
{

	public UIScrollList racingRankList;
	public GameObjectPool racingRankPool;
	public SpriteText racingTitleLable;
	public SpriteText racingNoTimeLable;
	public SpriteText racingTimerLable;
	public SpriteText racingMyRankLable;
	public SpriteText racingMyGuildRankLable;
	public SpriteText noDataLable;

	private bool isShow = false;
	private com.kodgames.corgi.protocol.Rank myRank;
	private List<com.kodgames.corgi.protocol.Rank> ranks;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		SysUIEnv.Instance.GetUIModule<UIPnlGuildRankTab>().ChangeTabButtons(_UIType.UIPnlGuildRacingRankList);
		RequestMgr.Inst.Request(new GuildStageQueryRankReq(GuildStageConfig._RankType.Speed, RequestQuerySuccess_RacingRank));
		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		racingRankList.ClearList(false);
		racingRankList.ScrollListTo(0f);
	}

	private bool RequestQuerySuccess_RacingRank(com.kodgames.corgi.protocol.Rank myRank, List<com.kodgames.corgi.protocol.Rank> ranks)
	{
		StopCoroutine("SendGuildStageQueryRankReq");
		this.myRank = myRank;
		this.ranks = ranks;
		ClearData();
		StartCoroutine("SetData");

		return true;
	}

	private void Update()
	{
		if (isShow)
		{
			DateTime refreshTime = TimeEx.GetNextWeekTimeToMaxNowTime(Convert.ToDateTime(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.ThreeDayRefreshTime));
			long surplusTimer = KodGames.TimeEx.DateTimeToInt64(refreshTime) - KodGames.TimeEx.DateTimeToInt64(SysLocalDataBase.Inst.LoginInfo.NowDateTime);
			UIUtility.UpdateUIText(racingTimerLable, string.Format("{0}{1}", GameDefines.zentiaTimerTextColor, GameUtility.Time2String(surplusTimer)));
			if (GameUtility.IsTime2DownZero(surplusTimer))
			{
				isShow = false;
				StartCoroutine("SendGuildStageQueryRankReq");
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator SetData()
	{
		yield return null;
		int sevenWeek = (int)Convert.ToDateTime(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.SevenDayRefreshTime).DayOfWeek;
		int threeWeek = (int)Convert.ToDateTime(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.ThreeDayRefreshTime).DayOfWeek;
		int nowWeek = (int)SysLocalDataBase.Inst.LoginInfo.NowDateTime.DayOfWeek;
		isShow = threeWeek > nowWeek && nowWeek > sevenWeek;
		if (!isShow)
		{
			racingNoTimeLable.Text = string.Empty;
			racingTimerLable.Text = string.Empty;

			racingTitleLable.Text = GameUtility.GetUIString("UIPnlGuildRankList_RacingTitleLable");
			if (myRank.rankValue <= ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.GuildSpeedRankSize)
				racingMyRankLable.Text = GameUtility.FormatUIString("UIPnlGuildRankList_ScheduleMyRankLable", myRank.rankValue);
			else
				racingMyRankLable.Text = GameUtility.FormatUIString("UIPnlGuildRankList_ScheduleMyRankLable", GameUtility.GetUIString("UIPnlGuildRankList_NoRankLable"));
			racingMyGuildRankLable.Text = GameUtility.FormatUIString("UIPnlGuildRankList_RacingMyGuildRankLable", myRank.intValue);

			if (ranks != null && ranks.Count > 0)
			{
				for (int index = 0; index < ranks.Count; index++)
				{
					UIElemGuildPointDamagerRankItem item = racingRankPool.AllocateItem().GetComponent<UIElemGuildPointDamagerRankItem>();
					item.SetData(ranks[index], DamagerRankItemData.Racing);
					racingRankList.AddItem(item.gameObject);
				}
				noDataLable.Text = string.Empty;
			}
			else
				noDataLable.Text = GameUtility.GetUIString("UIPnlGuildRacingRankList_NoDataLable");
		}
		else
		{
			noDataLable.Text = string.Empty;
			racingTitleLable.Text = string.Empty;
			racingMyRankLable.Text = string.Empty;
			racingMyGuildRankLable.Text = string.Empty;
			//DateTime refreshTime = TimeEx.GetNextWeekTimeToMaxNowTime(new DateTime(Convert.ToDateTime(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.ThreeDayRefreshTime).Ticks, DateTimeKind.Utc));
			//UIUtility.UpdateUIText(racingTimerLable, string.Format("{0}{1}", GameDefines.zentiaTimerTextColor, GameUtility.Time2String(KodGames.TimeEx.DateTimeToInt64(refreshTime) - KodGames.TimeEx.DateTimeToInt64(SysLocalDataBase.Inst.LoginInfo.NowDateTime))));
			racingNoTimeLable.Text = GameUtility.GetUIString("UIPnlGuildRankList_RacingNoTimerLable");
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator SendGuildStageQueryRankReq()
	{
		yield return new WaitForSeconds(1f);
		RequestMgr.Inst.Request(new GuildStageQueryRankReq(GuildStageConfig._RankType.Speed, RequestQuerySuccess_RacingRank));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkIcon(UIButton btn)
	{
		GameUtility.ShowAssetInfoUI((int)btn.Data);
	}

}
