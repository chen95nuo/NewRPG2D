using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class UIPnlEastElementRankingList : UIPnlEastSeaFindFairyTimer
{
	public SpriteText contextLable;
	public SpriteText eastSeaNumLable;
	public UIScrollList eastSeaCardList;
	public GameObjectPool objectPool;

	private List<KodGames.ClientClass.ZentiaRank> zentiaRanks;	//排行榜信息及奖励

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlEastSeaExchangeTab>().SetButtonType(_UIType.UIPnlEastElementRankingList);

		RequestMgr.Inst.Request(new EastSeaQueryZentiaRankReq());

		return true;
	}

	public void OnQueryZentiaRankSucces(long totalZentiaPoint, List<KodGames.ClientClass.ZentiaRank> zentiaRanks, string desc)
	{
		this.zentiaRanks = zentiaRanks;
		eastSeaNumLable.Text = totalZentiaPoint.ToString();
		contextLable.Text = desc;

		StartCoroutine("FillData");
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillData()
	{
		yield return null;
		eastSeaCardList.ScrollPosition = 0f;
		for (int i = 0; i < zentiaRanks.Count; i++)
		{
			var container = objectPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemEastSeaElementRankingList>();
			item.SetData(zentiaRanks[i]);
			eastSeaCardList.AddItem(container.gameObject);
		}
		eastSeaCardList.ScrollToItem(0, 0);
	}

	private void ClearData()
	{
		eastSeaCardList.ClearList(false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIconDetailedBtn(UIButton btn)
	{
		KodGames.ClientClass.ZentiaRank zentiaRank = btn.Data as KodGames.ClientClass.ZentiaRank;
		SysUIEnv.Instance.ShowUIModule<UIDlgEastRankingReward>(zentiaRank.Reward, GameUtility.GetUIString("UIPnlEastElementRankingList_Reward"), GameUtility.FormatUIString("UIPnlEastElementRankingList_RankNum", ItemInfoUtility.GetLevelCN(zentiaRank.Rank)));
	}

}



