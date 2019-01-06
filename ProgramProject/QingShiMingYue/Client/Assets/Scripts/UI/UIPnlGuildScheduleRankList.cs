using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildScheduleRankList : UIModule
{
	public UIScrollList scheduleRankList;
	public GameObjectPool scheduleRankPool;
	public SpriteText scheduleTitleLable;
	public SpriteText scheduleNoDataLable;
	public SpriteText scheduleMyRankLable;
	public SpriteText scheduleMyGuildRankLable;
	public GameObject scheduleTitleObject;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		SysUIEnv.Instance.GetUIModule<UIPnlGuildRankTab>().ChangeTabButtons(_UIType.UIPnlGuildScheduleRankList);
		RequestMgr.Inst.Request(new GuildStageQueryRankReq(GuildStageConfig._RankType.Progress, RequestQuerySuccess_ScheduleRank));
		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		scheduleRankList.ClearList(false);
		scheduleRankList.ScrollListTo(0f);
	}

	private bool RequestQuerySuccess_ScheduleRank(com.kodgames.corgi.protocol.Rank myRank, List<com.kodgames.corgi.protocol.Rank> ranks)
	{
		ClearData();
		if (ranks != null && ranks.Count > 0)
		{
			for (int index = 0; index < ranks.Count; index++)
			{
				UIElemGuildPointDamagerRankItem item = scheduleRankPool.AllocateItem().GetComponent<UIElemGuildPointDamagerRankItem>();
				item.SetData(ranks[index], DamagerRankItemData.Schedule);
				scheduleRankList.AddItem(item.gameObject);
			}
			scheduleTitleObject.SetActive(true);
			scheduleTitleLable.Text = GameUtility.GetUIString("UIPnlGuildRankList_ScheduleTitleLable");
			scheduleNoDataLable.Text = string.Empty;
		}
		else
		{
			scheduleTitleObject.SetActive(false);
			scheduleTitleLable.Text = string.Empty;
			scheduleNoDataLable.Text = GameUtility.GetUIString("UIPnlGuildRankList_ScheduleNoDataLable");
		}

		scheduleMyRankLable.Text = GameUtility.FormatUIString("UIPnlGuildRankList_ScheduleMyRankLable", myRank.rankValue);
		scheduleMyGuildRankLable.Text = GameUtility.FormatUIString("UIPnlGuildRankList_ScheduleMyGuildRankLable", myRank.intValue);
		return true;
	}
	
}
