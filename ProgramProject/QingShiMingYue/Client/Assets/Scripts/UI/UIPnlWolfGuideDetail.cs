using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlWolfGuideDetail : UIModule
{

	//出征页面顶端货币显示
	public UIBox realMoney;
	public UIBox gameMoney;
	public UIBox capTure;

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

		ShowCurrency();
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

			switch ((int)guideItem.subTypes[index].assetIconType)
			{
				//关卡，需要通过关卡ID去关卡详细里面获取奖励
				case 1:
					{
						//渲图标
						List<Reward> reWards = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetStageById(guideItem.subTypes[index].assetIconId).PassRewards;
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

						List<Reward> stongs = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetStageById(guideItem.subTypes[index].assetIconId).FirstPassRewards;
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
							detailItemLI.SetData(swards, guideItem.subTypes[index].assetIconId <= maxPassStageId);
							guideDetailList.AddItem(guideItemLI);
						}
					}
					break;
				case 2:
					{
						List<int> rewars = new List<int>();
						for (int i = 0; i < ConfigDatabase.DefaultCfg.WolfSmokeConfig.EggsEffects.Count; i++)
						{
							rewars.Add(ConfigDatabase.DefaultCfg.WolfSmokeConfig.EggsEffects[i].RewardId);
						}

						int row = rewars.Count % C_COLUMN_COUNT == 0 ? rewars.Count / C_COLUMN_COUNT - 1 : rewars.Count / C_COLUMN_COUNT;
						List<int> rewar = null;
						for (int i = 0; i <= row; i++)
						{
							if (i < row)
								rewar = rewars.GetRange(i * C_COLUMN_COUNT, C_COLUMN_COUNT);
							else
								rewar = rewars.GetRange(i * C_COLUMN_COUNT, Mathf.Min(rewars.Count - i * C_COLUMN_COUNT, C_COLUMN_COUNT));

							UIListItemContainer guideItemIcon = guideDetailItemPool.AllocateItem().GetComponent<UIListItemContainer>();
							UIElemWolfGuideDetail detailItemIcon = guideItemIcon.gameObject.GetComponent<UIElemWolfGuideDetail>();
							detailItemIcon.SetData(rewar);
							guideDetailList.AddItem(guideItemIcon);
						}
					}
					break;
			}
		}
	}

	private void ShowCurrency()
	{
		if (realMoney.Data == null || (int)realMoney.Data != SysLocalDataBase.Inst.LocalPlayer.RealMoney)
		{
			realMoney.Data = SysLocalDataBase.Inst.LocalPlayer.RealMoney;
			realMoney.Text = ItemInfoUtility.GetItemCountStr((int)realMoney.Data);
		}

		if (gameMoney.Data == null || (int)gameMoney.Data != SysLocalDataBase.Inst.LocalPlayer.GameMoney)
		{
			gameMoney.Data = SysLocalDataBase.Inst.LocalPlayer.GameMoney;
			gameMoney.Text = ItemInfoUtility.GetItemCountStr((int)gameMoney.Data);
		}

		if (capTure.Data == null || (int)capTure.Data != SysLocalDataBase.Inst.LocalPlayer.Medals)
		{
			capTure.Data = SysLocalDataBase.Inst.LocalPlayer.Medals;
			capTure.Text = ItemInfoUtility.GetItemCountStr((int)capTure.Data);
		}
	}

	#region OnClick

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击元宝
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickRealMoney(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.RealMoney).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//点击铜币
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickGameMoney(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.GameMoney).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	//点击军工
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickCupTure(UIButton btn)
	{
		string rewardDesc = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(IDSeg._SpecialId.Medals).desc;

		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(rewardDesc, true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
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
