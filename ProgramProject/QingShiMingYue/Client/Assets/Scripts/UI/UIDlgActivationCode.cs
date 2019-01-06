using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgActivationCode : UIModule
{
	public UITextField inputArea;

	public GameObject normalRoot;
	public GameObject failedRoot;
	public SpriteText failedLabel;

	public UIChildLayoutControl actionButtonLayout;
	public UIButton getCodeBtn;
	public UIButton okButton;

	private string authServerHostName;
	private int port;
	private int accountID = 0;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas.Length > 0 && userDatas[0] is LoginReq)
		{
			LoginReq loginInfo = userDatas[0] as LoginReq;
			authServerHostName = loginInfo.AuthServerHostName;
			port = loginInfo.Port;
		}

		if (userDatas.Length > 0 && userDatas[0] is QuickLoginReq)
		{
			QuickLoginReq qLoginInfo = userDatas[0] as QuickLoginReq;
			authServerHostName = qLoginInfo.AuthServerHostName;
			port = qLoginInfo.Port;
		}

		if (userDatas.Length > 1)
			accountID = (int)userDatas[1];

		inputArea.Text = "";
		normalRoot.SetActive(true);
		failedRoot.SetActive(false);

		actionButtonLayout.HideChildObj(okButton.gameObject, false);
		actionButtonLayout.HideChildObj(getCodeBtn.gameObject, string.IsNullOrEmpty(ConfigDatabase.DefaultCfg.GameConfig.GetActiveCodeGetUrl(Platform.Instance.PlatformType, Platform.Instance.ChannelId)));

		if (SysUIEnv.Instance.IsUIModuleShown(ClientServerCommon._UIType.UIPnlSelectArea))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectArea>().SetView(false);

		return true;
	}

	public override void OnHide()
	{
		if (SysUIEnv.Instance.IsUIModuleShown(ClientServerCommon._UIType.UIPnlSelectArea))
			SysUIEnv.Instance.GetUIModule<UIPnlSelectArea>().SetView(true);
		base.OnHide();
	}

	public void OnCodeFailed(string message)
	{
		normalRoot.SetActive(false);
		failedRoot.SetActive(true);
		failedLabel.Text = message;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGetCodeClick(UIButton btn)
	{
		Application.OpenURL(ConfigDatabase.DefaultCfg.GameConfig.GetActiveCodeGetUrl(Platform.Instance.PlatformType, Platform.Instance.ChannelId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnReturnBtnClick(UIButton btn)
	{
		normalRoot.SetActive(true);
		failedRoot.SetActive(false);
		inputArea.Text = "";
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOKClick(UIButton btn)
	{
		if (string.IsNullOrEmpty(authServerHostName) || port==0)
			return;

		if (!string.IsNullOrEmpty(inputArea.Text) && inputArea.Text.Length == 12)
		{
			RequestMgr.Inst.Request(new AuthActivityCodeReq(authServerHostName, port, accountID, inputArea.Text));
			inputArea.Text = "";
		}else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgActivationCode_EmptyTF"));
	}


}