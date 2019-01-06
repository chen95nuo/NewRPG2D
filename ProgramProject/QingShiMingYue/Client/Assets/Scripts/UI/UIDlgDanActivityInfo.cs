using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgDanActivityInfo : UIModule
{
	public UIScrollList activityList;
	public UIScrollList giftList;

	public GameObjectPool activityIconPool;
	public GameObjectPool descPool;
	public GameObjectPool fixRewardPool;
	public GameObjectPool randomPool;
	public GameObjectPool titlePool;

	private List<com.kodgames.corgi.protocol.DanActivityTap> danActivityTaps;
	private int defaultSelect = 0;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		danActivityTaps = userDatas[0] as List<com.kodgames.corgi.protocol.DanActivityTap>;

		defaultSelect = danActivityTaps[0].iconId;
		Init();
		return true;
	}

	private void Init()
	{
		ClearData();
		StartCoroutine("FillActivityIconList");
		StartCoroutine("FillGiftList");
	}

	private void ClearData()
	{
		StopCoroutine("FillActivityIconList");
		activityList.ClearList(false);
		activityList.ScrollPosition = 0f;

		StopCoroutine("FillGiftList");
		giftList.ClearList(false);
		giftList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator FillGiftList()
	{
		yield return null;

		com.kodgames.corgi.protocol.DanActivityTap danActivity = FindDanActivityByIconId(defaultSelect);

		UIElemShopGiftViewItem textItem = descPool.AllocateItem().GetComponent<UIElemShopGiftViewItem>();
		textItem.SetData(danActivity.activityDesc);
		giftList.AddItem(textItem.gameObject);

		switch (danActivity.type)
		{
			//炼丹额外赠送活动
			case DanConfig._ActivityDetailType.SpecialDan:		
				for (int i = 0; i < danActivity.danActivityRewards.Count; i++)
				{
					UIElemActivityFixReward fixDanItem = fixRewardPool.AllocateItem().GetComponent<UIElemActivityFixReward>();
					fixDanItem.SetData(danActivity.danActivityRewards[i].baseRewards, danActivity.danActivityRewards[i].extraRewards);
					giftList.AddItem(fixDanItem.gameObject);
				}
				break;
			//炼丹宝箱好礼活动
			case DanConfig._ActivityDetailType.Box:
				for (int i = 0; i < danActivity.danActivityRewards.Count; i++)
				{
					if (danActivity.danActivityRewards[i].baseRewards.Count > 0)
					{
						UIElemShopGiftViewItem titleItem = titlePool.AllocateItem().GetComponent<UIElemShopGiftViewItem>();
						titleItem.SetData(string.Format(GameUtility.GetUIString("UIDlgDanActivityInfo_BaseReward_Title"), GameDefines.textColorTitleBlue, danActivity.danActivityRewards[i].alchemyCount));
						giftList.AddItem(titleItem.gameObject);

						for (int j = 0; j < danActivity.danActivityRewards[i].baseRewards.Count; )
						{
							UIElemDanBoxReward boxItem = randomPool.AllocateItem().GetComponent<UIElemDanBoxReward>();
							List<com.kodgames.corgi.protocol.ShowReward> rewards = new List<com.kodgames.corgi.protocol.ShowReward>();

							for (int k = 0; k < boxItem.itemIcons.Length && j < danActivity.danActivityRewards[i].baseRewards.Count; k++)
							{
								rewards.Add(danActivity.danActivityRewards[i].baseRewards[j]);
								j++;
							}

							if (rewards.Count > 0)
							{
								boxItem.SetData(rewards);
								giftList.AddItem(boxItem.gameObject);
							}
						}
					}
					if (danActivity.danActivityRewards[i].extraRewards.Count > 0)
					{
						UIElemShopGiftViewItem titleItem = titlePool.AllocateItem().GetComponent<UIElemShopGiftViewItem>();
						titleItem.SetData(string.Format(GameUtility.GetUIString("UIDlgDanActivityInfo_RandomRewarde_Title"), GameDefines.textColorTitleBlue, danActivity.danActivityRewards[i].alchemyCount));
						giftList.AddItem(titleItem.gameObject);

						for (int j = 0; j < danActivity.danActivityRewards[i].extraRewards.Count; )
						{
							UIElemDanBoxReward boxItem = randomPool.AllocateItem().GetComponent<UIElemDanBoxReward>();
							List<com.kodgames.corgi.protocol.ShowReward> rewards = new List<com.kodgames.corgi.protocol.ShowReward>();

							for (int k = 0; k < boxItem.itemIcons.Length && j < danActivity.danActivityRewards[i].extraRewards.Count; k++)
							{
								rewards.Add(danActivity.danActivityRewards[i].extraRewards[j]);
								j++;
							}

							if (rewards.Count > 0)
							{
								boxItem.SetData(rewards);
								giftList.AddItem(boxItem.gameObject);
							}
						}
					}
				}
				break;
			//分解分解有惊喜活动
			case DanConfig._ActivityDetailType.DecomposeResult:
				for (int i = 0; i < danActivity.danActivityRewards.Count; i++)
				{
					UIElemShopGiftViewItem titleItem = titlePool.AllocateItem().GetComponent<UIElemShopGiftViewItem>();
					titleItem.SetData(string.Format(GameUtility.GetUIString("UIDlgDanActivityInfo_Decompose_Posibility_Title"),
																	GameDefines.textColorBtnYellow,
																	ItemInfoUtility.GetAssetQualityColor(danActivity.danActivityRewards[i].breakthought),
																	ItemInfoUtility.GetAssetName(danActivity.danActivityRewards[i].resourseId, danActivity.danActivityRewards[i].breakthought),
																	GameDefines.textColorBtnYellow));
					giftList.AddItem(titleItem.gameObject);
					if (danActivity.danActivityRewards[i].extraRewards.Count > 0)
					{
						for (int j = 0; j < danActivity.danActivityRewards[i].extraRewards.Count; )
						{
							UIElemDanBoxReward boxItem = randomPool.AllocateItem().GetComponent<UIElemDanBoxReward>();
							List<com.kodgames.corgi.protocol.ShowReward> rewards = new List<com.kodgames.corgi.protocol.ShowReward>();

							for (int k = 0; k < boxItem.itemIcons.Length && j < danActivity.danActivityRewards[i].extraRewards.Count; k++)
							{
								rewards.Add(danActivity.danActivityRewards[i].extraRewards[j]);
								j++;
							}

							if (rewards.Count > 0)
							{
								boxItem.SetData(rewards);
								giftList.AddItem(boxItem.gameObject);
							}
						}
					}
				}
				break;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator FillActivityIconList()
	{
		yield return null;

		for (int i = 0; i < danActivityTaps.Count; i++)
		{
			UIListItemContainer uiContainer = activityIconPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemDanActivityinfoIcon item = uiContainer.GetComponent<UIElemDanActivityinfoIcon>();
			uiContainer.Data = item;

			item.SetData(danActivityTaps[i].iconId);
			activityList.AddItem(item.gameObject);
		}

		ChangeBtnSelectLight();
	}

	private com.kodgames.corgi.protocol.DanActivityTap FindDanActivityByIconId(int iconId)
	{
		for (int i = 0; i < danActivityTaps.Count; i++)
		{
			if (danActivityTaps[i].iconId == iconId)
				return danActivityTaps[i];
		}

		Debug.Log("The IconId Is Error");
		return danActivityTaps[0];
	}

	private void ChangeBtnSelectLight()
	{
		for (int index = 0; index < activityList.Count; index++)
		{
			var danActivityIcon = activityList.GetItem(index).Data as UIElemDanActivityinfoIcon;

			if (danActivityIcon != null)
				danActivityIcon.SelectLight(danActivityIcon.IconId == defaultSelect);
			else
				Debug.Log("The DanActivityIcon Is Null");
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnActivityIconClick(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		defaultSelect = (int)assetIcon.Data;
		Init();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAssetIconClick(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var showReward = assetIcon.Data as com.kodgames.corgi.protocol.ShowReward;
		GameUtility.ShowAssetInfoUI(showReward, _UILayer.Top);
	}
}

