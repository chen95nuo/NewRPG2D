using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlWolfGuide : UIModule
{
	//出征页面顶端货币显示
	public UIBox realMoney;
	public UIBox gameMoney;
	public UIBox capTure;

	public UIScrollList guideList;
	public GameObjectPool guidePool;

	private int maxPassStageId;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		maxPassStageId = IDSeg.InvalidId;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		maxPassStageId = (int)userDatas[0];

		ShowCurrency();
		StartCoroutine("FillGuideList");

		return true;
	}

	public override void OnHide()
	{
		StopCoroutine("FillGuideList");
		guideList.ClearList(false);
		guideList.ScrollListTo(0);
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGuideList()
	{
		yield return null;

		for (int index = 0; index < ConfigDatabase.DefaultCfg.WolfSmokeConfig.MainTypes.Count; index++)
		{
			UIListItemContainer item = guidePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemWolfGuideItem guide = item.gameObject.GetComponent<UIElemWolfGuideItem>();
			guide.SetData(ConfigDatabase.DefaultCfg.WolfSmokeConfig.MainTypes[index]);
			guideList.AddItem(item);
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

	//点击标签
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGuideItemClick(UIButton btn)
	{
		ClientServerCommon.MainType guideItem = btn.Data as ClientServerCommon.MainType;
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfGuideDetail), guideItem, maxPassStageId);
	}

	#endregion
}
