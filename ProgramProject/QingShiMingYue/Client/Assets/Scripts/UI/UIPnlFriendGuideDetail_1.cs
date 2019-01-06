using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendGuideDetail_1 : UIModule
{
	public UIScrollList guideDetailList;
	public GameObjectPool guideDetailLabelPool;
	public GameObjectPool guideDetailItemPool;
	public GameObjectPool guideDetailLI;

	public SpriteText titleLabel;

	//控制每行有多少个Icon
	private const int C_COLUMN_COUNT = 4;

	private int maxPassStageId;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		maxPassStageId = (int)userDatas[1];

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
			UIElemWolfGuideDetailItem detail = guideDetailItem.gameObject.GetComponent<UIElemWolfGuideDetailItem>();
			detail.SetData(guideItem.subTypes[index]);
			guideDetailList.AddItem(guideDetailItem);

			//关卡有图标
			if (guideItem.subTypes[index].assetIconType == 1)
			{
				//先渲关卡通过奖励
				List<Reward> reWards = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetStageById(guideItem.subTypes[index].assetIconId).PassRewards;
				int row = reWards.Count % C_COLUMN_COUNT == 0 ? reWards.Count / C_COLUMN_COUNT - 1 : reWards.Count / C_COLUMN_COUNT;

				List<Reward> wards = null;
				for (int i = 0; i <= row; i++)
				{
					if (i < row)
						wards = reWards.GetRange(i * C_COLUMN_COUNT, C_COLUMN_COUNT);
					else
						wards = reWards.GetRange(i * C_COLUMN_COUNT, Mathf.Min(reWards.Count - i * C_COLUMN_COUNT, C_COLUMN_COUNT));

					UIListItemContainer guideItemIcon = guideDetailItemPool.AllocateItem().GetComponent<UIListItemContainer>();
					UIElemWolfGuideDetail detailItemIcon = guideItemIcon.gameObject.GetComponent<UIElemWolfGuideDetail>();
					detailItemIcon.SetData(wards);
					guideDetailList.AddItem(guideItemIcon);
				}

				//渲染首次通关奖励
				List<Reward> stongs = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetStageById(guideItem.subTypes[index].assetIconId).FirstPassRewards;
				int rows = stongs.Count % C_COLUMN_COUNT == 0 ? stongs.Count / C_COLUMN_COUNT - 1 : stongs.Count / C_COLUMN_COUNT;
				List<Reward> swards = null;
				for (int i = 0; i <= rows; i++)
				{
					if (i < rows)
						swards = stongs.GetRange(i * C_COLUMN_COUNT, C_COLUMN_COUNT);
					else
						swards = stongs.GetRange(i * C_COLUMN_COUNT, Mathf.Min(stongs.Count - i * C_COLUMN_COUNT, C_COLUMN_COUNT));

					UIListItemContainer guideItemLI = guideDetailLI.AllocateItem().GetComponent<UIListItemContainer>();
					UIElemWolfGuideDetailLI detailItemLI = guideItemLI.gameObject.GetComponent<UIElemWolfGuideDetailLI>();
					detailItemLI.SetData(swards, ConfigDatabase.DefaultCfg.FriendCampaignConfig.JudgeStageIsJoinById(guideItem.subTypes[index].assetIconId, maxPassStageId));
					guideDetailList.AddItem(guideItemLI);
				}
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
