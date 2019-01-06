using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;

class GameState_GuildPoint : GameStateBase
{
	public override bool IsGamingState { get { return true; } }

	List<object> parameters;

	public override void Create(object userData)
	{
		this.parameters = userData as List<object>;
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
		Debug.Log("GameState_GuildPoint.OnEscape");

		if (!SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildPointMain)))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildPointMain);
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

		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.guildPointSceneId);
		//if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
		yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName, true);

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlGuildPointMain), parameters[0] as List<com.kodgames.corgi.protocol.Stage>, (int)parameters[1], (int)parameters[2], (int)parameters[3], (int)parameters[4],
													parameters[5] as KodGames.ClientClass.Cost, (int)parameters[6], (int)parameters[7], (string)parameters[8], (string)parameters[9], (long)parameters[10], (int)parameters[11], (int)parameters[12], (bool)parameters[13], (int)parameters[14]);

		UIPnlLoading.HidePanel(false);
	}
}