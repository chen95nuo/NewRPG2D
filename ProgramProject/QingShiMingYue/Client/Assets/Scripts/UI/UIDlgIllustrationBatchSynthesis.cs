using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgIllustrationBatchSynthesis : UIModule
{

	public SpriteText viewCountText;
	public SpriteText coppercoin_valueText;
	public SpriteText debris_valueText;

	public SpriteText coppercoin_keyText;
	public SpriteText debris_keyText;

	public SpriteText titleText;
	public SpriteText messageText;
	public UIElemAssetIcon itemIcon;

	private int currentCount = 0;
	private IllustrationConfig.Illustration illustration;
	private Cost cost;
	private Color CopperTextColor = GameDefines.txcolorYellowAndWhite;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;
		titleText.text = GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_Title");
		coppercoin_keyText.text = GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_Copper");
		return true;
	}
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		if (userDatas != null && userDatas.Length > 0)
		{
			illustration = userDatas[0] as IllustrationConfig.Illustration;
			cost = illustration.Cost[0];
			currentCount = (int)userDatas[1];
			itemIcon.SetData(illustration.Id);
			UpdateView();
		}

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	private void UpdateView()
	{
		int countCopper = cost.count * currentCount;

		if (!ItemInfoUtility.CheckCostEnough(cost.id, countCopper))
			CopperTextColor = GameDefines.textColorRed;

		viewCountText.Text = currentCount.ToString();
		coppercoin_valueText.Text = (cost.count * currentCount).ToString();
		coppercoin_valueText.SetColor(CopperTextColor);
		debris_valueText.Text = (illustration.FragmentCount * currentCount).ToString();
		if (IDSeg.ToAssetType(illustration.Id) == IDSeg._AssetType.Avatar)
			debris_keyText.Text = GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_Soul");
		else
			debris_keyText.Text = GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_Debris");
		messageText.Text = GameUtility.FormatUIString("UIPnlHandBook_PiLiangHeCheng_Context", ItemInfoUtility.GetAssetName(illustration.Id));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		this.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCancel(UIButton btn)
	{
		this.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSynthetic(UIButton btn)
	{
		if (!CheckCopperNotEnough(cost.count * currentCount, true) && !CheckDebrisNotEnough(illustration.FragmentCount * currentCount, true))
		{
			RequestMgr.Inst.Request(new MergeIllustrationReq(illustration.Id, currentCount));
			this.OnHide();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSubtract(UIButton btn)
	{
		currentCount = currentCount-- <= 1 ? 1 : currentCount--;

		CopperTextColor = CheckCopperNotEnough(cost.count * currentCount, false) ? GameDefines.textColorRed : GameDefines.txcolorYellowAndWhite;
		UpdateView();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAdd(UIButton btn)
	{

		if (CheckDebrisNotEnough(illustration.FragmentCount * (currentCount + 1), true)) return;

		CopperTextColor = CheckCopperNotEnough(cost.count * (currentCount + 1), true) ? GameDefines.textColorRed : GameDefines.txcolorYellowAndWhite;
		currentCount++;
		UpdateView();
	}

	//验证铜币不足
	private bool CheckCopperNotEnough(int countCopper, bool isShowTips)
	{
		if (!ItemInfoUtility.CheckCostEnough(cost.id, countCopper))
		{
			if (isShowTips)
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_ErrorContext_Copper"));
			return true;
		}
		return false;
	}

	//验证 碎片/魂魄 不足
	private bool CheckDebrisNotEnough(int countDeris, bool isShowTips)
	{
		var consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(illustration.FragmentId);//获取碎片信息
		if (countDeris > consumable.Amount)
		{
			if (isShowTips)
			{
				if (IDSeg.ToAssetType(illustration.Id) == IDSeg._AssetType.Avatar)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_ErrorContext_Soul"));
				else
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_ErrorContext_Debris"));
				return true;
			}
		}
		return false;
	}
}
