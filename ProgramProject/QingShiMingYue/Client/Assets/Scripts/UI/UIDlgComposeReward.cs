using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;
using System;
using KodGames;

public class UIDlgComposeReward : UIModule
{

	public GameObjectPool gameObjectPool;
	public UIScrollList scrollList;
	public SpriteText titleLabel;
	public SpriteText giftLabel;
	public UIButton okBtn;
	public UIButton clostBtn;

	private const int MAX_COLUMN_NUM = 3;
	//private int illustrationId = 0;
	private List<Pair<int, int>> rewardPackagePars;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;
		if (userDatas != null && userDatas.Length > 0)
		{
			rewardPackagePars = userDatas[0] as List<Pair<int, int>>;
			//illustrationId = (int)userDatas[0];
			FillData();
			titleLabel.Text = userDatas.Length > 1 && userDatas[1] != null ? userDatas[1].ToString() : GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_GongXiGet_Title");
			giftLabel.Text = userDatas.Length > 2 && userDatas[2] != null ? userDatas[2].ToString() : GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_HeChengGet_Title");
		}
		return true;
	}


	private void ClearData()
	{
		scrollList.ClearList(false);
		titleLabel.Text = "";
		giftLabel.Text = "";

	}
	private void FillData()
	{
		UIListItemContainer container = gameObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemShopGiftItem item = container.GetComponent<UIElemShopGiftItem>();
		container.Data = item;
		// if(item.itemIcons!=null && item.itemIcons.Count>0)
		//     item.itemIcons[0].SetData(illustrationId, UIDlgIllustrationBatchSynthesis.currentCount);

		for (int i = 0; i < item.itemIcons.Count; ++i)
		{
			if (i < rewardPackagePars.Count)
				item.itemIcons[i].SetData(rewardPackagePars[i].first, rewardPackagePars[i].second);
			else
				item.itemIcons[i].Hide(true);
			//item.itemIcons[i].SetData(illustrationId,UIDlgIllustrationBatchSynthesis.currentCount);
		}
		scrollList.AddItem(container.gameObject);
	}


	public override void OnHide()
	{
		base.OnHide();
	}



	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		ClearData();
		HideSelf();

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOkBtnClick(UIButton btn)
	{
		ClearData();
		HideSelf();

	}


	//µã»÷Í¼±ê
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId, _UILayer.Top);
	}
}
