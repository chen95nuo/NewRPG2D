using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlEastElementAllServerReward : UIModule
{
	public SpriteText contextLable;
	public SpriteText eastSeaNumLable;
	public UIScrollList eastSeaCardList;
	public GameObjectPool objectPool;

	private int totalZentiaPoint; //个人累计仙缘值
	private long serverZentiaPoint;
	private List<KodGames.ClientClass.ZentiaServerReward> zentiaServerRewards;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlEastSeaExchangeTab>().SetButtonType(_UIType.UIPnlEastElementAllServerReward);

		RequestMgr.Inst.Request(new EastSeaQueryServerZentiaRewardReq());

		return true;
	}

	public void OnQueryServerZentiaRewardSuccess(int totalZentiaPoint, long serverZentiaPoint, List<KodGames.ClientClass.ZentiaServerReward> zentiaServerRewards, string desc)
	{
		eastSeaNumLable.Text = serverZentiaPoint.ToString();
		contextLable.Text = desc;

		OnReceiveSuccess(totalZentiaPoint, serverZentiaPoint, zentiaServerRewards);
	}

	//查询成功回调
	public void OnReceiveSuccess(int totalZentiaPoint, long serverZentiaPoint, List<KodGames.ClientClass.ZentiaServerReward> zentiaServerRewards)
	{
		ClearData();
		this.totalZentiaPoint = totalZentiaPoint;
		this.serverZentiaPoint = serverZentiaPoint;
		this.zentiaServerRewards = zentiaServerRewards;

		StopCoroutine("FillData");
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
		eastSeaCardList.ScrollPosition = 51f;
		for (int i = 0; i < zentiaServerRewards.Count; i++)
		{
			var zServerReward = zentiaServerRewards[i];
			var container = objectPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemEastSeaElementAllServerReward>();
			item.SetData(zServerReward, totalZentiaPoint, serverZentiaPoint, (i == 0), zentiaServerRewards[i - 1 <= 0 ? 0 : i - 1]);
			eastSeaCardList.AddItem(container.gameObject);
		}
		eastSeaCardList.ScrollToItem(0, 0);
	}

	private void ClearData()
	{
		eastSeaCardList.ClearList(false);
	}

	//奖励图标点击
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIconDetailedBtn(UIButton btn)
	{
		if (btn.Data != null)
			GameUtility.ShowAssetInfoUI((ClientServerCommon.Reward)btn.Data);
	}

	//领取奖励按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickToReceiveBtn(UIButton btn)
	{
		var zServerReward = btn.Data as KodGames.ClientClass.ZentiaServerReward;
		if (totalZentiaPoint < zServerReward.TotalZentiaPoint)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlEastElementAllServerReward_TotalContext", GameDefines.textColorTipsInBlack, totalZentiaPoint, GameDefines.textColorTipsInBlack, zServerReward.TotalZentiaPoint));
			return;
		}
		RequestMgr.Inst.Request(new EastSeaGetServerZentiaRewardReq(zServerReward.RewardLevelId));
	}
}



