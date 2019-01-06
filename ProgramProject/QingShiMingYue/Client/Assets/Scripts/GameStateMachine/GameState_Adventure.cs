using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;
using UnityEngine;

internal class GameState_Adventure : GameStateBase
{
	private List<object> parameters;

	public static int sceneID;

	public override bool IsGamingState { get { return true; } }

	private List<com.kodgames.corgi.protocol.DelayReward> getDelayRewards;
	private com.kodgames.corgi.protocol.MarvellousProto marvellousProto = null;

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
			() =>
			{
				AdventureSceneData.Instance.getDelayRewards = getDelayRewards;
				if(marvellousProto == null)
					RequestMgr.Inst.Request(new MarvellousQueryDelayRewardReq());
			}
		);
	}

	public override void Exit()
	{
		base.Exit();
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_Adventure.OnEscape");

		Platform.Instance.AndroidQuit();
	}

#endif

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private System.Collections.IEnumerator LoadingProgress()
	{
		if (parameters != null && parameters.Count > 0 && parameters[0] is com.kodgames.corgi.protocol.MarvellousProto)
		{
			marvellousProto = parameters[0] as com.kodgames.corgi.protocol.MarvellousProto;
			if (marvellousProto != null)
			{
				var marrvellous = ConfigDatabase.DefaultCfg.MarvellousAdventureConfig.GetMarvellousById(marvellousProto.marvellousId);
				if (marrvellous == null)
				{
					Debug.LogWarning("Can't find the marvellous by marvellousId : " + marvellousProto.marvellousId.ToString("X"));
				}
				else if (sceneID != marrvellous.SceneId)
				{
					sceneID = marrvellous.SceneId;
				}
			}
		}

		if (parameters != null && parameters.Count > 1 && parameters[1] is List<com.kodgames.corgi.protocol.DelayReward>)
			getDelayRewards = parameters[1] as List<com.kodgames.corgi.protocol.DelayReward>;

		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(sceneID);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName, true);
		if (marvellousProto == null)
			SysUIEnv.Instance.ShowUIModule<UIPnlAdventureScene>();
		else
			AdventureSceneData.Instance.GetAdventureTypeByEventId(marvellousProto);
		UIPnlLoading.HidePanel(false);
	}
}