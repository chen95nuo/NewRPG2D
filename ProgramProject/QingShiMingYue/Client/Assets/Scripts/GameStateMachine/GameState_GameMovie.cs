using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class GameState_GameMovie : GameStateBase
{
	public override bool IsGamingState { get { return true; } }

	public override void Enter()
	{
		base.Enter();

		UIPnlLoading.ShowPanel(
			() =>
		{
			StartCoroutine("LoadingProgress");
		},
			() =>
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlGameMovie));
		});
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_GameMovie.OnEscape");

		Platform.Instance.AndroidQuit();
	}

#endif

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator LoadingProgress()
	{
		// Load scene
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.gameMovieSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		UIPnlLoading.HidePanel(false);
	}
}
