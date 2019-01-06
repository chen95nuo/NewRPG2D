using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgConfirmExchange : UIModule
{
	public delegate void OnConfrimDel();

	public UIElemAssetIcon itemIcon;
	public SpriteText itemDesc;

	private OnConfrimDel onConfimDel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas.Length < 2)
		{
			Debug.LogError("Error Params. Need 2 Params.");
			return false;
		}

		int resourceId = (int)userDatas[0];
		onConfimDel = userDatas[1] as OnConfrimDel;

		// Set ItemIcon.
		itemIcon.SetData(resourceId);

		// Set Desc.
		itemDesc.Text = ItemInfoUtility.GetAssetExtraDesc(resourceId);

		return true;
	}

	public override void OnHide()
	{
		this.onConfimDel = null;

		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCancle(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCommit(UIButton btn)
	{
		if (onConfimDel != null)
			onConfimDel();

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		SysUIEnv.Instance.HideUIModule(typeof(UIDlgConfirmExchange));
	}
}
