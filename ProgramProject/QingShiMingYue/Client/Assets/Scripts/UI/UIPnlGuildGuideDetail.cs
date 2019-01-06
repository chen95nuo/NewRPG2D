using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;

public class UIPnlGuildGuideDetail : UIModule
{
	public class Goods
	{
		public int id;
		public string name;

		public void SetData(int iconId, string goodsName)
		{
			id = iconId;
			name = goodsName;
		}
	}

	public UIScrollList guideDetailList;
	public GameObjectPool guideDetailLabelPool;
	public GameObjectPool guideDetailItemPool;
	public GameObjectPool guideTaskPool;

	public SpriteText titleLabel;
	public GameObject backBg;

	//控制每行有多少个Icon
	private const int C_COLUMN_COUNT = 4;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;		

		if (SysGameStateMachine.Instance.CurrentState is GameState_GuildPoint)
			backBg.gameObject.SetActive(true);
		else
			backBg.gameObject.SetActive(false);

		StartCoroutine("FillGuideDetailList", userDatas[0] as ClientServerCommon.MainType);

		return true;
	}

	public override void OnHide()
	{
		StopCoroutine("FillGuideDetailList");
		guideDetailList.ClearList(false);
		guideDetailList.ScrollListTo(0f);
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGuideDetailList(ClientServerCommon.MainType guideItem)
	{
		yield return null;

		titleLabel.Text = string.Format("{0}", guideItem.name);

		for (int index = 0; index < guideItem.subTypes.Count; index++)
		{
			UIListItemContainer guideDetailItem = guideDetailLabelPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemGuildGuideLabelItem detail = guideDetailItem.gameObject.GetComponent<UIElemGuildGuideLabelItem>();
			detail.SetData(guideItem.subTypes[index]);
			guideDetailList.AddItem(guideDetailItem);

			//渲图标
			List<Reward> reWards = new List<Reward>();
			List<Goods> goods = new List<Goods>();
			switch (guideItem.subTypes[index].assetIconType)
			{
				case GuildConfig._GuideType.InvisibleTask:
					reWards = ConfigDatabase.DefaultCfg.GuildConfig.GetInvisibleGuildTaskById(guideItem.subTypes[index].assetIconId).Rewards;
					break;
				case GuildConfig._GuideType.PublicShopGuide:
					goods = ItemInfoUtility.GetGoodsListByGuildShopType(GuildConfig._GuideType.PublicShopGuide);
					break;
				case GuildConfig._GuideType.PrivateShopGuide:
					goods = ItemInfoUtility.GetGoodsListByGuildShopType(GuildConfig._GuideType.PrivateShopGuide);
					break;
				case GuildConfig._GuideType.ExchangeShopGuide:
					goods = ItemInfoUtility.GetGoodsListByGuildShopType(GuildConfig._GuideType.ExchangeShopGuide);
					break;
			}

			if (reWards != null && reWards.Count > 0)
			{
				int row = reWards.Count % C_COLUMN_COUNT == 0 ? reWards.Count / C_COLUMN_COUNT - 1 : reWards.Count / C_COLUMN_COUNT;

				List<Reward> wards = null;
				for (int i = 0; i <= row; i++)
				{
					if (i < row)
						wards = reWards.GetRange(i * C_COLUMN_COUNT, C_COLUMN_COUNT);
					else
						wards = reWards.GetRange(i * C_COLUMN_COUNT, Mathf.Min(reWards.Count - i * C_COLUMN_COUNT, C_COLUMN_COUNT));

					UIListItemContainer guideItemIcon = guideDetailItemPool.AllocateItem().GetComponent<UIListItemContainer>();
					UIElemGuildGuideIconItem detailItemIcon = guideItemIcon.gameObject.GetComponent<UIElemGuildGuideIconItem>();
					detailItemIcon.SetData(wards);
					guideDetailList.AddItem(guideItemIcon);
				}
			}
			else
			{
				int row = goods.Count % C_COLUMN_COUNT == 0 ? goods.Count / C_COLUMN_COUNT - 1 : goods.Count / C_COLUMN_COUNT;

				List<Goods> wards = null;
				for (int i = 0; i <= row; i++)
				{
					if (i < row)
						wards = goods.GetRange(i * C_COLUMN_COUNT, C_COLUMN_COUNT);
					else
						wards = goods.GetRange(i * C_COLUMN_COUNT, Mathf.Min(goods.Count - i * C_COLUMN_COUNT, C_COLUMN_COUNT));

					UIListItemContainer guideItemIcon = guideDetailItemPool.AllocateItem().GetComponent<UIListItemContainer>();
					UIElemGuildGuideIconItem detailItemIcon = guideItemIcon.gameObject.GetComponent<UIElemGuildGuideIconItem>();
					detailItemIcon.SetData(wards);
					guideDetailList.AddItem(guideItemIcon);
				}
			}
		}

		for (int i = 0; i < guideItem.subTypes.Count; i++)
		{
			if (guideItem.subTypes[i].assetIconId == GuildConfig._GuideType.InvisibleTask)
			{
				UIElemGuildGuideTask item = guideTaskPool.AllocateItem().GetComponent<UIElemGuildGuideTask>();
				item.SetData();
				guideDetailList.AddItem(item.gameObject);
				break;
			}
		}
	}

	#region OnClick

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIcom(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId);
	}

	#endregion
}
