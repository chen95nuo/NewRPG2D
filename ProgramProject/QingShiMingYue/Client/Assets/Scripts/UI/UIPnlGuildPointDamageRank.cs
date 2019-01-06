using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using com.kodgames.corgi.protocol;

public class UIPnlGuildPointDamageRank : UIModule
{
	//Tab button.
	public UIScrollList guideList;
	public GameObjectPool guidePool;
	public SpriteText titleLable;
	public SpriteText myRankLable;
	public SpriteText muHarmLable;
	public UIButton backBG;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		if (userDatas != null && userDatas.Length > 0)
		{
			if (userDatas[0] is BossRank)
			{
				var bossRank = userDatas[0] as BossRank;
				RequestMgr.Inst.Request(new GuildStageQueryBossRankDetailReq(bossRank.mapNum, bossRank.index));
			}
			else if (userDatas[0] is int[])
			{
				int[] datas = userDatas[0] as int[];
				RequestMgr.Inst.Request(new GuildStageQueryBossRankDetailReq(datas[0], datas[1]));
			}
			backBG.Hide(!(bool)userDatas[1]);
		}
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

	public void OnGuildStageQueryBossRankDetailSuccess(Rank rank, BossRank bossrank)
	{

		if (bossrank == null) return;
		ClearList();

		foreach (var bRank in bossrank.ranks)
		{
			UIListItemContainer item = guidePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemGuildPointDamagerRankItem rankItem = item.gameObject.GetComponent<UIElemGuildPointDamagerRankItem>();
			rankItem.SetData(bRank,DamagerRankItemData.Damage);
			guideList.AddItem(item);
		}
		titleLable.Text = bossrank.name;
		myRankLable.Text = GameUtility.FormatUIString("UIPnlGuildPointDamageRank_MyRank", rank.rankValue);
		muHarmLable.Text = GameUtility.FormatUIString("UIPnlGuildPointDamageRank_MyDamage", (int)rank.damage);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}

	
}
