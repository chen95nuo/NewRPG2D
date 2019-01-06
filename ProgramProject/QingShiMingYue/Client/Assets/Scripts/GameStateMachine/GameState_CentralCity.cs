//#define GIVE_ME_FIVE_LOG
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Text;

class GameState_CentralCity : GameStateBase
{
	private object userData;

	public override bool IsGamingState { get { return true; } }

	public override void Create(object userData)
	{
		this.userData = userData;
	}

	public override void Enter()
	{
		UIPnlLoading.ShowPanel(() =>
			{
				StartCoroutine("LoadingProgress");
			},
			null);
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_CentralCity.OnEscape");

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlMainMenuBot))
		{
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCentralCityPlayerInfo) == false
				|| SysUIEnv.Instance.GetUIModule(_UIType.UIPnlCentralCityPlayerInfo).IsOverlayed
				|| SysUIEnv.Instance.IsUIModuleShown(_UIType.UIDlgDailyReward)
				|| SysUIEnv.Instance.IsUIModuleShown(_UIType.UIDlgAnnouncement))
				SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().ShowMainScene();
			else
				Platform.Instance.AndroidQuit();
		}
	}

#endif

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private System.Collections.IEnumerator LoadingProgress()
	{
		// Reset the main camera state to inactive. 
		MainSceneData mainSceneDataObj = GameObject.FindObjectOfType(typeof(MainSceneData)) as MainSceneData;
		if (mainSceneDataObj != null)
			mainSceneDataObj.mainCamera.enabled = true;

		// Load main scene
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.mainSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		ShowMainUI();

		UIPnlLoading.HidePanel(false);
	}

	public void ShowMainUI()
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlMainScene));

		SysLocalDataBase.Inst.CheckQueryLevelUpReward();

		if (SysLocalDataBase.Inst.LocalPlayer.UnDoneTutorials.Count <= 0
			|| SysLocalDataBase.Inst.LocalPlayer.UnDoneTutorials.Contains(ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.forceTutorialEndId) == false)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.NoticeData.AutoShowNotice)
			{
				SysUIEnv.Instance.AddShowEvent(new SysUIEnv.UIModleShowEvent(typeof(UIDlgAnnouncement)));

				SysLocalDataBase.Inst.LocalPlayer.NoticeData.AutoShowNotice = false;
			}

			// Show the Daily sign Panel if Today has not signed.
			if (GameUtility.CheckFuncOpened(_OpenFunctionType.DailyReward, false, true) &&
				UIDlgDailyReward.SignInState(SysLocalDataBase.Inst.LoginInfo.NowDateTime.Day) == false && (userData == null || (userData is UserDataBase == false)))
			{
				int type = 0;
				if (userData is int)
					type = (int)userData;
				if (type != _UIType.UIPnlDanFurnace)
					SysUIEnv.Instance.AddShowEvent(new SysUIEnv.UIModleShowEvent(typeof(UIDlgDailyReward)));
			}

#if GIVE_ME_FIVE_LOG
			Debug.Log("KodGames.ExternalCall.KodConfigPlugin.IsAppStore() " + KodGames.ExternalCall.KodConfigPlugin.IsAppStore());
#endif

			// Only app store
			if (KodGames.ExternalCall.KodConfigPlugin.IsAppStore())
				ShowGiveMeFive();

		}

		// Process user data
		if (userData != null)
		{
			if (userData is UserDataBase)
				((UserDataBase)userData).Process();
			else if (userData is int)
			{
				GameUtility.JumpUIPanel((int)userData);
			}
		}
	}

	private void ShowGiveMeFive()
	{
#if GIVE_ME_FIVE_LOG
		StringBuilder sb = new StringBuilder();
		sb.Append("LocalPlayer.Function.ShowGiveMeFive " + SysLocalDataBase.Inst.LocalPlayer.Function.ShowGiveMeFive + "\n");
		sb.Append("SysLocalDataBase.Inst.LocalPlayer.HasEvaluate " + SysLocalDataBase.Inst.LocalPlayer.HasEvaluate + "\n");
		sb.Append("SysLocalDataBase.Inst.LocalPlayer.CancelEvaluateLevel " + SysLocalDataBase.Inst.LocalPlayer.CancelEvaluateLevel + "\n");
		Debug.Log(sb.ToString());
#endif

		//如果未开启或已经评价过
		if (SysLocalDataBase.Inst.LocalPlayer.Function.ShowGiveMeFive == false || SysLocalDataBase.Inst.LocalPlayer.HasEvaluate)
			return;

		int cancelLevel = SysLocalDataBase.Inst.LocalPlayer.CancelEvaluateLevel;

		foreach (var level in ConfigDatabase.DefaultCfg.LevelConfig.giveMeFiveConfig.tipPlayerLevels)
		{
			if (cancelLevel >= level)
				continue;

			if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level >= level)
			{
				SysUIEnv.Instance.AddShowEvent(new SysUIEnv.UIModleShowEvent(typeof(UIDlgGiveMeFive)));
				break;
			}
		}
	}
}