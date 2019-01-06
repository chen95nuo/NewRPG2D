using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;

class GameState_WolfSmoke : GameStateBase
{
	ActionFxPlayer actionFxPlayer = new ActionFxPlayer();
	public ActionFxPlayer ActionFxPlayer
	{
		get { return actionFxPlayer; }
	}

	List<object> parameters;
	public override void Create(object userData)
	{
		this.parameters = userData as List<object>;
	}

	public override bool IsGamingState { get { return true; } }

	public override void Enter()
	{
		UIPnlLoading.ShowPanel(() =>
		{
			StartCoroutine("LoadingProgress");
		},
		() =>
		{
			ShowPnl();
		});
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_WolfSmoke.OnEscape");
		
		if (!SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlWolfInfo)))			
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfInfo));
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlWolfMyBattle)))
		{			
			SysUIEnv.Instance.GetUIModule(typeof(UIPnlWolfMyBattle)).OnHide();
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfInfo));
		}			
		else
			Platform.Instance.AndroidQuit();
	}

#endif

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private System.Collections.IEnumerator LoadingProgress()
	{
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.wolfSmokeSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		UIPnlLoading.HidePanel(false);

		UIPnlWolfInfo.WolfSmokeData wolfData = null;
		if (parameters.Count > 0 && parameters[0] is UIPnlWolfInfo.WolfSmokeData)
			wolfData = parameters[0] as UIPnlWolfInfo.WolfSmokeData;

		if (parameters.Count > 1 && parameters[1] is UIPnlWolfBattleResult.WolfBattleResultData)
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfInfo), wolfData, parameters[1] as UIPnlWolfBattleResult.WolfBattleResultData, (bool)parameters[2]);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfInfo), wolfData);
	}

	private void ShowPnl()
	{
		if (parameters.Count > 1 && parameters[1] is UIPnlWolfBattleResult.WolfBattleResultData)
		{
			UIPnlWolfBattleResult.WolfBattleResultData woflBattleInfo = parameters[1] as UIPnlWolfBattleResult.WolfBattleResultData;
			if (woflBattleInfo.IsWinner())
				WolfSmokeSceneData.Instance.PassPoint();
		}
	}
}