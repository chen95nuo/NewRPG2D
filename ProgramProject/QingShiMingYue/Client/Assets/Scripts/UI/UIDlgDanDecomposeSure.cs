using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgDanDecomposeSure : UIModule
{
	//Message to show.
	public SpriteText costLabel;
	public SpriteText itemCostLabel;
	public SpriteText[] selectLabel;
	public UIBox itemCostBg;

	public UIBox danBg1;
	public UIBox danBg2;

	private int freeCount;
	private int costCount;
	private List<KodGames.ClientClass.Dan> decomposeDans = new List<KodGames.ClientClass.Dan>();
	private KodGames.ClientClass.Cost cost = new KodGames.ClientClass.Cost();

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if(!base.OnShow(layer, userDatas)) 
			return false;

		freeCount = (int)userDatas[0];
		costCount = (int)userDatas[1];
		decomposeDans = userDatas[2] as List<KodGames.ClientClass.Dan>;
		cost = userDatas[3] as KodGames.ClientClass.Cost;

		danBg1.gameObject.SetActive(false);
		danBg2.gameObject.SetActive(false);

		if (freeCount > 0)
		{
			costLabel.Text = string.Format(GameUtility.GetUIString("UIDlgDanDecomposeSure_FreeCount"), decomposeDans.Count);
			itemCostBg.gameObject.SetActive(false);
		}
		else if (costCount > 0)
		{
			costLabel.Text = string.Format(GameUtility.GetUIString("UIDlgDanDecomposeSure_CostCount"), decomposeDans.Count);
			itemCostBg.gameObject.SetActive(true);
			itemCostLabel.Text = string.Format(GameUtility.GetUIString("UIDlgDanDecomposeSure_ItemCount"), ItemInfoUtility.GetAssetName(cost.Id), cost.Count);
		}		
		
		int[] danCounts =new int[5];

		for (int i = 0; i < decomposeDans.Count; i++)
		{
			danCounts[decomposeDans[i].BreakthoughtLevel - 1]++;
		}

		int danTypeCount = 0;
		for (int i = 0; i < danCounts.Length && i < selectLabel.Length; i++)
		{
			selectLabel[i].Text = "";
			if (danCounts[i] > 0)
			{
				selectLabel[danTypeCount].Text = string.Format(GameUtility.GetUIString("UIDlgDanDecomposeSure_DanCost_Label" + (i + 1)), danCounts[i]);
				danTypeCount++;
			}
		}

		if (danTypeCount > 2)
			danBg1.gameObject.SetActive(true);
		if (danTypeCount > 4)
			danBg2.gameObject.SetActive(true);

		return true;
	}
	
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOkBtn(UIButton btn)
	{
		int bossItemCount = ItemInfoUtility.GetGameItemCount(cost.Id);

		if (freeCount <= 0 && cost.Count > bossItemCount)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), string.Format(GameUtility.GetUIString("UIDlgDanDecomposeSure_ItemTips"), ItemInfoUtility.GetAssetName(cost.Id), ItemInfoUtility.GetAssetName(cost.Id)));
			return;
		}

		int decomposeType = 0;

		if (freeCount > 0)
			decomposeType = DanConfig._DecomposeType.Free;
		else decomposeType = DanConfig._DecomposeType.Charge;

		List<string> guids = new List<string>();
		foreach (var dan in decomposeDans)
		{
			guids.Add(dan.Guid);
		}

		RequestMgr.Inst.Request(new DanDecomposeReq(decomposeType, guids, cost));
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickQuitBtn(UIButton btn)
	{
		HideSelf();
	}
}