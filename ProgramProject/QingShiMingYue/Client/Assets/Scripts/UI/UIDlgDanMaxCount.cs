using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgDanMaxCount : UIModule
{
	public UIElemAssetIcon decomposeIcon;
	public SpriteText messageLabel;
	public UIButton decomposeGoto;

	private com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		decomposeInfo = userDatas[0] as com.kodgames.corgi.protocol.DecomposeInfo;

		// Set DecomposeIcon.
		decomposeIcon.SetData(ConfigDatabase.DefaultCfg.DanConfig.GetDanHelpByInfoType(DanConfig._IntroduceType.DanDecompose).IconId);

		//if (decomposeInfo.danDecomposeCout > 0 || decomposeInfo.danItemDecomposeCount > 0)
		//	decomposeGoto.controlIsEnabled = true;
		//else
		//	decomposeGoto.controlIsEnabled = false;

		messageLabel.Text = GameUtility.GetUIString("UIDlgDanIntroduce_Message_Label");

		return true;
	}

	private bool CheckDanDecomposeInfoCount()
	{
		if (decomposeInfo.danDecomposeCout > 0 || decomposeInfo.danItemDecomposeCount > 0)
			return true;
		else
			return false;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGotoClick(UIButton btn)
	{
		if (!CheckDanDecomposeInfoCount())
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgDanMaxCount_DanDecomposeInfoCount"));
		else
			GameUtility.JumpUIPanel(_UIType.UIPnlDanDecompose);
	}
}

