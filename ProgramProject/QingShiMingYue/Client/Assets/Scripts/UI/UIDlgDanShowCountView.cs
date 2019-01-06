using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using System;

public class UIDlgDanShowCountView : UIModule
{
	public GameObjectPool rewardPool;
	public GameObjectPool titlePool;
	public UIScrollList scrollList;
	public SpriteText titleLabel;
	public SpriteText messageLabel;
	public UIButton okBtn;		

	private com.kodgames.corgi.protocol.ShowCounter showCounter;
	
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		showCounter = userDatas[0] as com.kodgames.corgi.protocol.ShowCounter;

		if (showCounter != null)
			titleLabel.Text = string.Format(GameUtility.GetUIString("UIDlgDanShowCountView_GetReward_Title"), showCounter.remainCount);

		messageLabel.Text = string.Format(GameUtility.GetUIString("UIDlgDanShowCountView_GetReward_Mes"), showCounter.remainCount);

		ClearData();
		StartCoroutine("FillList");

		return true;
	}

	public override void OnHide()
	{
		ClearData();

		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		
		//随机获得
		if (showCounter.rewards != null && showCounter.rewards.Count > 0)
		{

			UIElemShopGiftViewItem viewItem = titlePool.AllocateItem().GetComponent<UIElemShopGiftViewItem>();
			
			if (showCounter.rewards.Count == 1)
				viewItem.SetData(GameUtility.GetUIString("UIDlgDanShowCountView_Fix_Title"));
			else
				viewItem.SetData(GameUtility.GetUIString("UIDlgDanShowCountView_Random_Title"));
			
			scrollList.AddItem(viewItem.gameObject);

			for (int i = 0; i < showCounter.rewards.Count; )
			{
				UIElemDanBoxReward rewardItem = rewardPool.AllocateItem().GetComponent<UIElemDanBoxReward>();

				List<com.kodgames.corgi.protocol.ShowReward> rewards = new List<com.kodgames.corgi.protocol.ShowReward>();

				for (int j = 0; j < rewardItem.itemIcons.Length && i < showCounter.rewards.Count; j++)
				{
					rewards.Add(showCounter.rewards[i]);
					i++;
				}
				
				if(rewards.Count > 0)
				{
					rewardItem.SetData(rewards);
					scrollList.AddItem(rewardItem.gameObject);	
				}				
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var showReward = assetIcon.Data as com.kodgames.corgi.protocol.ShowReward;
		GameUtility.ShowAssetInfoUI(showReward, _UILayer.Top);
	}
}
