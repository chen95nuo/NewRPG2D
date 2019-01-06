using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgPackageSellSceondSure : UIModule
{
	public List<SpriteText> selectText;

	public List<SpriteText> getBySell;

	public UIBox MeridianAttributeBg2;

	public UIButton cancleBtn;
	public UIButton okBtn;
	public UIButton closeBtn;

	public delegate bool OnCallback(object data);
	private OnCallback callback;
	public OnCallback Callback
	{
		get { return this.callback; }
		set { this.callback = value; }
	}

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		InitView((List<string>)userDatas[0], (List<string>)userDatas[1]);

		return true;
	}

	private void InitView(List<string> sellItems, List<string> getBySell)
	{
		for (int i = 0; i < selectText.Count; i += 1)
		{
			if (i < sellItems.Count)
				selectText[i].Text = sellItems[i];
			else
				selectText[i].Text = "";
		}

		MeridianAttributeBg2.Hide(sellItems.Count <= 2 ? true : false);

		for (int j = 0; j < this.getBySell.Count; j += 1)
		{
			if (j < getBySell.Count)
				this.getBySell[j].Text = getBySell[j];
			else
				this.getBySell[j].Text = "";
		}
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOkBtn(UIButton btn)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageSell))
			SysUIEnv.Instance.GetUIModule<UIPnlPackageSell>().OnSell((Object)btn);
		this.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCancelBtn(UIButton btn)
	{
		this.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCloseBtn(UIButton btn)
	{
		this.OnHide();
	}

}
