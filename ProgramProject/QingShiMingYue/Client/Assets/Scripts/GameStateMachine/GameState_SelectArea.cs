using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Runtime.InteropServices;
using KodGames.ExternalCall;

class GameState_SelectArea : GameStateBase
{
	public override bool IsGamingState { get { return false; } }

	public override void Enter()
	{
		// Maybe blocked when loading. Hide loading UI
		UIPnlLoading.HidePanel(false);

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlLoginBackground));
		SysUIEnv.Instance.GetUIModule<UIPnlLoginBackground>().PlayAnimation(true);
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlSelectArea));
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_SelectArea.OnEscape");

		Platform.Instance.AndroidQuit();
	}

#endif

	public override void Exit()
	{
		// Hide dialog
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgActivationCode)))
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgActivationCode));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSelectArea)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlSelectArea));

		// 关闭广告显示
#if UNITY_IPHONE
		if (KodGames.iAdViewer.Instance != null)
			KodGames.iAdViewer.Instance.Visable = false;
#endif

		base.Exit();
	}

	public void ConnectIS(KodGames.ClientClass.Area area)
	{
       // Debug.LogError(area.InterfaceServerIP + "    " + area.Port + "   " + KodGames.ClientHelper.NetType.NETTYPE_TCP+"   "+
       //    area.AreaID + "     " + area.NewAreaId);
		// Update player last login area id.
		if (area != null)
			RequestMgr.Inst.Request(new ConnectISReq(
				area.InterfaceServerIP,
				area.Port,
				KodGames.ClientHelper.NetType.NETTYPE_TCP,
				area.AreaID,
				area.NewAreaId,				
				OnConnectISSuccess,
				OnConnectISFailed));
        //OnConnectISSuccess();
	}

	private void OnConnectISSuccess()
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_RetriveGameData>();
	}

	private void OnConnectISFailed(int errCode, string errMsg)
	{
		// Show message dlg and reconnect
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.Callback = (userData) =>
		{
			SysGameStateMachine.Instance.EnterState<GameState_Upgrading>();
			return true;
		};

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"), GameUtility.GetUIString("UIDlgMessage_DisConnecMessage"), false, null, okCallback);
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}
}
