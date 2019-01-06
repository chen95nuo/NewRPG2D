using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlEastSeaElementItem : UIModule
{

	public SpriteText currentEastSeaNumLable;
	public UIScrollList eastSeaCardList;
	public GameObjectPool objectPool;

	private List<KodGames.ClientClass.ZentiaGood> zentiaGoods;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlEastSeaExchangeTab>().SetButtonType(_UIType.UIPnlEastSeaElementItem);

		RequestMgr.Inst.Request(new EastSeaQueryZentiaGoodReq());

		return true;
	}

	public void OnQueryZentiaGoodSuccess(List<KodGames.ClientClass.ZentiaGood> zentiaGoods, bool isRankOpen)
	{
		SysUIEnv.Instance.GetUIModule<UIPnlEastSeaExchangeTab>().HidetankingTab(isRankOpen);
		this.zentiaGoods = zentiaGoods;

		currentEastSeaNumLable.Text = SysLocalDataBase.Inst.LocalPlayer.Zentia.ToString();

		StartCoroutine("FillData");
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public void OnEastSeaBuyZentiaGoodRes()
	{
		currentEastSeaNumLable.Text = SysLocalDataBase.Inst.LocalPlayer.Zentia.ToString();
	}

	private void ClearData()
	{
		eastSeaCardList.ClearList(false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillData()
	{
		yield return null;
		eastSeaCardList.ScrollPosition = 0f;
		foreach (KodGames.ClientClass.ZentiaGood zentiaGood in zentiaGoods)
		{
			var container = objectPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemEastSeaElementItem>();
			item.SetData(zentiaGood);
			eastSeaCardList.AddItem(container.gameObject);
		}
		eastSeaCardList.ScrollToItem(0, 0);
	}



	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIconDetailedBtn(UIButton btn)
	{
		GameUtility.ShowAssetInfoUI(btn.Data as ClientServerCommon.Reward);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickExchangeBtn(UIButton btn)
	{
		var zentiaGood = btn.Data as KodGames.ClientClass.ZentiaGood;
		RequestMgr.Inst.Request(new EastSeaBuyZentiaGoodReq(zentiaGood.ExchangeGoodId));
	}


}


