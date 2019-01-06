using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using System;

public class UIDlgDanBoxRewardView : UIModule
{
	public GameObjectPool rewardPool;
	public GameObjectPool titlePool;
	public UIScrollList scrollList;
	public SpriteText titleLabel;	
	public UIButton okBtn;
	public UIButton getRewardBtn;
	public UIBox rewardPicked;

	private List<com.kodgames.corgi.protocol.ShowReward> fixedRewards;
	private List<com.kodgames.corgi.protocol.ShowReward> randomRewards;
	private List<com.kodgames.corgi.protocol.ShowReward> getRewards;

	com.kodgames.corgi.protocol.BoxReward boxReward;


	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if(userDatas.Length > 0)
		{
			this.fixedRewards = userDatas[0] as List<com.kodgames.corgi.protocol.ShowReward>;

			if(userDatas.Length > 1)
				this.randomRewards = userDatas[1] as List<com.kodgames.corgi.protocol.ShowReward>;

			if(userDatas.Length > 4)
			{
				okBtn.Hide(!(bool)userDatas[2]);
				getRewardBtn.Hide(!(bool)userDatas[3]);
				rewardPicked.Hide(!(bool)userDatas[4]);
			}

			if (userDatas.Length > 5)
				boxReward = userDatas[5] as com.kodgames.corgi.protocol.BoxReward;
		}

		if(boxReward != null)
			titleLabel.Text = string.Format(GameUtility.GetUIString("UIDlgDanBoxRewardView_Title_View"), boxReward.alchemyCount);

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

	public void QueryPickBoxRewardSuccess(List<com.kodgames.corgi.protocol.ShowReward> getRewards)
	{
		//领取成功
		okBtn.Hide(false);
		getRewardBtn.Hide(true);
		rewardPicked.Hide(true);

		//刷新炼丹界面
		RequestMgr.Inst.Request(new QueryAlchemyReq());

		this.getRewards = getRewards;

		ClearData();
		StartCoroutine("FillGetRewardList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		
		//固定奖励
		if (fixedRewards != null && fixedRewards.Count > 0)
		{
			UIElemShopGiftViewItem viewItem = titlePool.AllocateItem().GetComponent<UIElemShopGiftViewItem>();
			viewItem.SetData(GameUtility.GetUIString("UIDlgDanBoxRewardView_Fix_Title"));
			scrollList.AddItem(viewItem.gameObject);	

			for(int i = 0; i < fixedRewards.Count;)
			{
				UIElemDanBoxReward rewardItem = rewardPool.AllocateItem().GetComponent<UIElemDanBoxReward>();

				List<com.kodgames.corgi.protocol.ShowReward> rewards = new List<com.kodgames.corgi.protocol.ShowReward>();

				for (int j = 0; j < rewardItem.itemIcons.Length && i < fixedRewards.Count; j++)
				{
					rewards.Add(fixedRewards[i]);
					i++;
				}
				
				if(rewards.Count > 0)
				{
					rewardItem.SetData(rewards);
					scrollList.AddItem(rewardItem.gameObject);	
				}				
			}
		}

		//随即奖励
		if (randomRewards != null && randomRewards.Count > 0)
		{
			UIElemShopGiftViewItem viewItem = titlePool.AllocateItem().GetComponent<UIElemShopGiftViewItem>();
			viewItem.SetData(GameUtility.GetUIString("UIDlgDanBoxRewardView_Random_Title"));
			scrollList.AddItem(viewItem.gameObject);

			for (int i = 0; i < randomRewards.Count; )
			{
				UIElemDanBoxReward rewardItem = rewardPool.AllocateItem().GetComponent<UIElemDanBoxReward>();

				List<com.kodgames.corgi.protocol.ShowReward> rewards = new List<com.kodgames.corgi.protocol.ShowReward>();

				for (int j = 0; j < rewardItem.itemIcons.Length && i < randomRewards.Count; j++)
				{
					rewards.Add(randomRewards[i]);
					i++;
				}

				if (rewards.Count > 0)
				{
					rewardItem.SetData(rewards);
					scrollList.AddItem(rewardItem.gameObject);
				}
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGetRewardList()
	{
		yield return null;

		//获得奖励
		if (getRewards != null && getRewards.Count > 0)
		{
			UIElemShopGiftViewItem getTitle = titlePool.AllocateItem().GetComponent<UIElemShopGiftViewItem>();
			getTitle.SetData(GameUtility.GetUIString("UIDlgDanBoxRewardView_GetReward_Title"));
			scrollList.AddItem(getTitle.gameObject);

			for (int i = 0; i < getRewards.Count; )
			{
				UIElemDanBoxReward rewardItem = rewardPool.AllocateItem().GetComponent<UIElemDanBoxReward>();

				List<com.kodgames.corgi.protocol.ShowReward> rewards = new List<com.kodgames.corgi.protocol.ShowReward>();

				for (int j = 0; j < rewardItem.itemIcons.Length && i < getRewards.Count; j++)
				{
					rewards.Add(getRewards[i]);
					i++;
				}

				if (rewards.Count > 0)
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

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGetRewardClick(UIButton btn)
	{
		RequestMgr.Inst.Request(new PickAlchemyBoxReq(boxReward.id));
	}
}
