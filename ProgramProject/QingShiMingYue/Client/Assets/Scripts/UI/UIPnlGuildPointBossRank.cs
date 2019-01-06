using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using com.kodgames.corgi.protocol;

public class UIPnlGuildPointBossRank : UIModule
{
	//Tab button.
	public UIScrollList guideList;
	public GameObjectPool guidePool;
	public SpriteText noRankLable;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		RequestMgr.Inst.Request(new GuildStageQueryBossRankReq());
		return true;
	}

	public void OnGuildStageQueryBossRankResSuccess(List<BossRank> bossRanks)
	{
		ClearList();
		noRankLable.Text=string.Empty;
		if (bossRanks == null || bossRanks.Count == 0)
			noRankLable.Text = GameUtility.GetUIString("UIPnlGuildPointBossRank_NoRankLable");
		foreach (var bossrank in bossRanks)
		{
			UIListItemContainer item = guidePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemGuildPointBossRankItem guide = item.gameObject.GetComponent<UIElemGuildPointBossRankItem>();
			guide.SetData(bossrank);
			guideList.AddItem(item);
		}
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

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGuideItemClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIPnlGuildPointDamageRank>(btn.Data,true);
	}
}
