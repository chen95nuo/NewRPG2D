using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildRankList : UIModule
{
	public UIScrollList guildRankList;
	public GameObjectPool guildRankPool;
	public GameObjectPool moreItemPool;

	private List<KodGames.ClientClass.GuildRankRecord> guildRecods;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		SysUIEnv.Instance.GetUIModule<UIPnlGuildRankTab>().ChangeTabButtons(_UIType.UIPnlGuildRankList);
		RequestMgr.Inst.Request(new GuildQueryRankList());
		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		guildRankList.ClearList(false);
		guildRankList.ScrollListTo(0f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		int currentCount = GetLastListItemIndex();
		int index = currentCount;

		for (; index < currentCount + ConfigDatabase.DefaultCfg.GuildConfig.GuildCountPerPage && index < guildRecods.Count; index++)
		{
			UIElemGuildRankItem item = guildRankPool.AllocateItem().GetComponent<UIElemGuildRankItem>();

			item.SetData(guildRecods[index], index);

			if (guildRankList.Count == 0 || guildRankList.GetItem(guildRankList.Count - 1).Data is UIElemGuildRankItem)
				guildRankList.AddItem(item.container);
			else
				guildRankList.InsertItem(item.container, guildRankList.Count - 1);
		}

		if (guildRankList.Count > 0 && guildRankList.GetItem(guildRankList.Count - 1).Data == null)
			guildRankList.RemoveItem(guildRankList.Count - 1, false);

		if (GetLastListItemIndex() < guildRecods.Count)
			guildRankList.AddItem(moreItemPool.AllocateItem());
	}

	private int GetLastListItemIndex()
	{
		if (guildRankList.Count <= 0)
			return 0;

		if (guildRankList.GetItem(guildRankList.Count - 1).Data is UIElemGuildRankItem)
			return guildRankList.Count;
		else
			return guildRankList.Count - 1;
	}

	public void RequestQuerySuccess_GuildRank(List<KodGames.ClientClass.GuildRankRecord> guildRecods)
	{
		this.guildRecods = guildRecods;

		this.guildRecods.Sort(DataCompare.CompaceRankGuildRecord);

		StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickView(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildApplyInfo, (int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMoreItem(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList");
	}

}
