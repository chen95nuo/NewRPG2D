﻿﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene manager.
/// * Manage level
///		* Load level
///	* Manage game object
///		* Create/destroy game object
///		* Modify game object attribute
///	* Character management
/// </summary>
public class SysSceneManager : SysModule
{
	public static SysSceneManager Instance
	{
		get { return SysModuleManager.Instance.GetSysModule<SysSceneManager>(); }
	}

	public interface ISceneManagerListener
	{
		void OnSceneWillChange(SysSceneManager manager, string currentScene, string newScene);
		void OnSceneChanged(SysSceneManager manager, string oldScene, string currentScene);
	}

	private string startScene = "";
	public string StartScene { get { return startScene; } }

	public string CurrentScene { get { return Application.loadedLevelName; } }

	public List<ISceneManagerListener> sceneManagerListeners = new List<ISceneManagerListener>();

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		// Save start scene and current scene
		startScene = CurrentScene;

		return true;
	}

	public bool IsSceneLoaded(string sceneName)
	{
		return CurrentScene.Equals(GetSceneName(sceneName), StringComparison.InvariantCultureIgnoreCase);
	}

	private bool CheckSceneChanging(string sceneName, bool forceLoad)
	{
		if (sceneName == startScene)
		{
			Debug.LogError("Can not reload start scene.");
			return false;
		}

		if (forceLoad == false && sceneName.Equals(CurrentScene))
			return false;

		return true;
	}

	public void AddSceneManagerListener(ISceneManagerListener listener)
	{
		if (sceneManagerListeners.Contains(listener))
			return;

		sceneManagerListeners.Add(listener);
	}

	public void RemoveSceneManagerListener(ISceneManagerListener listener)
	{
		sceneManagerListeners.Remove(listener);
	}

	private static string GetSceneName(string sceneName)
	{
		return System.IO.Path.GetFileNameWithoutExtension(sceneName).ToLower();
	}

	public void ChangeScene(string sceneName, bool forceLoad, bool sync)
	{
		if (sync)
			ChangeSceneSync(sceneName, forceLoad);
		else
		{
			StopCoroutine("DoChangeSceneAsync");
			StartCoroutine("DoChangeSceneAsync", new object[] { sceneName, forceLoad });
		}
	}

	// Change current scene.
	public void ChangeSceneSync(string sceneName)
	{
		ChangeSceneSync(sceneName, false);
	}

	public void ChangeSceneSync(string sceneName, bool forceLoad)
	{
		sceneName = GetSceneName(sceneName);

		if (CheckSceneChanging(sceneName, forceLoad) == false)
			return;

		// Notice listener scene will change
		for (int i = 0; i < sceneManagerListeners.Count; ++i)
			sceneManagerListeners[i].OnSceneWillChange(this, CurrentScene, sceneName);

		// Load level
		Debug.Log("LoadLevel : " + sceneName);
		string oldScene = CurrentScene;
		Application.LoadLevel(sceneName);

		OnChangeSceneCompleted(oldScene);
	}

	// Change current scene asynchronously
	public Coroutine ChangeSceneAsync(string sceneName)
	{
		return ChangeSceneAsync(sceneName, false);
	}

	public Coroutine ChangeSceneAsync(string sceneName, bool forceLoad)
	{
		StopCoroutine("DoChangeSceneAsync");
		return StartCoroutine("DoChangeSceneAsync", new object[] { sceneName, forceLoad });
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private System.Collections.IEnumerator DoChangeSceneAsync(object[] param)
	{
		string sceneName = param[0] as string;
		bool forceLoad = (bool)param[1];

		sceneName = GetSceneName(sceneName);

		// Previous action maybe stop, Wait for previous loading finished
		while (Application.isLoadingLevel)
		{
			Debug.LogWarning("Waiting for previous loading finished");
			yield return null;
		}

		if (CheckSceneChanging(sceneName, forceLoad) == false)
			yield break;

		// Notice listener scene will change
		for (int i = 0; i < sceneManagerListeners.Count; ++i)
			sceneManagerListeners[i].OnSceneWillChange(this, CurrentScene, sceneName);

		// Load unity level and save the destination scene.
		string oldScene = CurrentScene;
		Debug.Log("LoadLevelAsync : " + sceneName);
		yield return Application.LoadLevelAsync(sceneName);

		OnChangeSceneCompleted(oldScene);
	}

	/// <summary>
	/// Call this function when ChangeSceneAsync completed, this function must be called. 
	/// </summary>
	private void OnChangeSceneCompleted(string oldScene)
	{
		// Notice listener scene changed
		for (int i = 0; i < sceneManagerListeners.Count; ++i)
			sceneManagerListeners[i].OnSceneChanged(this, CurrentScene, oldScene);

		// Player BGM
		string currentMusic = ClientServerCommon.ConfigDatabase.DefaultCfg.SceneConfig.GetBgMusicBySceneName(CurrentScene);

		if (!AudioManager.Instance.IsMusicPlaying(currentMusic))
		{
			AudioManager.Instance.StopMusic();

			if (!string.IsNullOrEmpty(currentMusic))
				AudioManager.Instance.PlayMusic(currentMusic, true);
		}
	}
}
