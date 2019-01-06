using System;
using System.Collections;
using ClientServerCommon;

public class GameState_SelectPlayerAvatar : GameStateBase
{
	public override bool IsGamingState { get { return true; } }

	public override void Enter()
	{
		base.Enter();

		UIPnlLoading.ShowPanel(() =>
			{
				StartCoroutine("LoadingProgress");
			},
			null);
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_SelectPlayerAvatar.OnEscape");

		Platform.Instance.AndroidQuit();
	}

#endif

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private IEnumerator LoadingProgress()
	{
		// Load scene
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.selectAvatarSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlSelectPlayerAvatar);

		UIPnlLoading.HidePanel(false);
	}
}