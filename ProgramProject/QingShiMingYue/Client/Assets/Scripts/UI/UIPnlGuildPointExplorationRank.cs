using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using com.kodgames.corgi.protocol;

public class UIPnlGuildPointExplorationRank : UIModule
{
	public UIScrollList guideList;
	public GameObjectPool guidePool;
	public SpriteText weekContext;
	public SpriteText myRankLable;
	public SpriteText muHarmLable;
	public SpriteText noRankLable;
	public GameObject noRankRoot;
	public UIButton showWeekBtn;


	private int type = ClientServerCommon.GuildStageConfig._WeekType.ThisWeek;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		RequestMgr.Inst.Request(new GuildStageQueryExploreRankReq(type));
		return true;
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	private void ClearList()
	{
		guideList.ClearList(false);
		guideList.ScrollListTo(0f);
	}

	public void OnGuildStageQueryExploreRankResSuccess(Rank myRank, List<Rank> ranks)
	{
		ClearList();
		
		noRankLable.Text = GameUtility.GetUIString("UIPnlGuildPointExplorationRank_NoRankLable");
		noRankRoot.SetActive(false);
		myRankLable.Text = string.Empty;
		muHarmLable.Text = string.Empty;
	
		foreach (var rank in ranks)
		{
			UIListItemContainer item = guidePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemGuildPointDamagerRankItem guide = item.gameObject.GetComponent<UIElemGuildPointDamagerRankItem>();
			guide.SetData(rank,DamagerRankItemData.None);
			guideList.AddItem(item);
		}

		if (type == ClientServerCommon.GuildStageConfig._WeekType.ThisWeek)
		{
			weekContext.Text = GameUtility.GetUIString("UIPnlGuildPointExplorationRank_WeekContext2");
			showWeekBtn.spriteText.Text = GameUtility.GetUIString("UIPnlGuildPointExplorationRank_ShowBtn1");
			showWeekBtn.Data = ClientServerCommon.GuildStageConfig._WeekType.PreviousWeek;
			if (ranks != null && ranks.Count > 0)
			{
				noRankLable.Text = string.Empty;
				noRankRoot.SetActive(true);
				myRankLable.Text = GameUtility.FormatUIString("UIPnlGuildPointDamageRank_MyRank", myRank.rankValue);
				muHarmLable.Text = GameUtility.FormatUIString("UIPnlGuildPointDamageRank_MyHarm0", myRank.intValue);
			}
		}
		else if (type == ClientServerCommon.GuildStageConfig._WeekType.PreviousWeek)
		{
			weekContext.Text = GameUtility.GetUIString("UIPnlGuildPointExplorationRank_WeekContext1");
			showWeekBtn.spriteText.Text = GameUtility.GetUIString("UIPnlGuildPointExplorationRank_ShowBtn2");
			showWeekBtn.Data = ClientServerCommon.GuildStageConfig._WeekType.ThisWeek;
			if (ranks != null && ranks.Count > 0)
			{
				noRankLable.Text = string.Empty;
				noRankRoot.SetActive(true);
				myRankLable.Text = GameUtility.FormatUIString("UIPnlGuildPointDamageRank_MyRank", myRank.rankValue);
				muHarmLable.Text = GameUtility.FormatUIString("UIPnlGuildPointDamageRank_MyHarm1", myRank.intValue);
			}
		}
		
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnShowWeekClick(UIButton btn)
	{
		type = (int)btn.Data;
		RequestMgr.Inst.Request(new GuildStageQueryExploreRankReq(type));
	}

}
