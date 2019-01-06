#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgOfficialInfo : UIModule
{
	public SpriteText qqGroupLabel;
	public SpriteText weiboLable;
	public SpriteText weixinLabel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SetControls();
		return true;
	}

	private void SetControls()
	{
// 		string qqGroup = "";
// 		foreach (string qqUrl in ConfigDatabase.DefaultCfg.GameConfig.setting.qqURLs)
// 		{
// 			qqGroup += qqUrl + "\t";
// 		}
// 		qqGroupLabel.Text = qqGroup;
// 		weiboLable.Text = ConfigDatabase.DefaultCfg.GameConfig.setting.weiboURL;
// 		weixinLabel.Text = ConfigDatabase.DefaultCfg.GameConfig.setting.weixinURL;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
	}
}

#endif
