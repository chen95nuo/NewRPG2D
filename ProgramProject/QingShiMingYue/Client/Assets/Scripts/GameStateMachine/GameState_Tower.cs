using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;

class GameState_Tower : GameStateBase
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
		Debug.Log("GameState_Tower.OnEscape");

		if (!SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlTowerPlayerInfo)))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlTowerScene);
		else
			Platform.Instance.AndroidQuit();
	}

#endif

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private System.Collections.IEnumerator LoadingProgress()
	{
		// Reset the main camera state to inactive. 
		MainSceneData mainSceneDataObj = GameObject.FindObjectOfType(typeof(MainSceneData)) as MainSceneData;
		if (mainSceneDataObj != null)
			mainSceneDataObj.mainCamera.enabled = true;

		SceneConfig.Scene sceneCfg =ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.towerSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTowerScene));

		if (userData != null && userData is UserDataBase)
			((UserDataBase)userData).Process();

		UIPnlLoading.HidePanel(false);
	}
}