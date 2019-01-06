//#define LOG_ANIMATION_EVENT
using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class AnimationEventHandler : MonoBehaviour
{
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void PlaySoundEvtHandler(string parameter)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("PlaySoundEvtHandler " + parameter);
#endif
		string[] parameters = parameter.Split(',');
		if (parameters.Length != 1 && parameters.Length != 3)
			return;

		string soundName = parameters[0];
		if (parameters.Length == 1)
		{
			AudioManager.Instance.PlaySound(soundName, 0f);
			return;
		}

		float volume = StrParser.ParseFloat(parameters[1], 0f);
		float delay = StrParser.ParseFloat(parameters[2], 0f);
		AudioManager.Instance.PlaySound(soundName, volume, delay);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void PlayMusicEvtHandler(string parameter)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("PlayMusicEvtHandler " + parameter);
#endif
		string[] parameters = parameter.Split(',');
		if (parameters.Length != 1 && parameters.Length != 3)
			return;

		string audioName = parameters[0];
		if (parameters.Length == 1)
		{
			if (AudioManager.Instance.IsMusicPlaying(audioName) == false)
				AudioManager.Instance.PlayMusic(audioName, true);
			return;
		}

		float delay = StrParser.ParseFloat(parameters[1], 0f);
		float fadeTime = StrParser.ParseFloat(parameters[2], 0f);
		if (AudioManager.Instance.IsMusicPlaying(audioName) == false)
			AudioManager.Instance.PlayMusic(audioName, true, delay, fadeTime);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void StopMusicEvtHandler(float fadeTime)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("StopMusicEvtHandler");
#endif
		AudioManager.Instance.StopMusic(fadeTime);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void StepMusicToNormalState(string parameter)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("StepMusicToNormalState " + parameter);
#endif
		AudioManager.Instance.StepMusicToNormalState(parameter);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void SetAnimationSpeed(float speed)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("SetAnimationSpeed " + speed);
#endif
		if (speed == 0)
		{
			Debug.LogWarning("Can not set speed to zero");
			speed = 1;
		}

		foreach (AnimationState animState in gameObject.animation)
			if (this.animation.IsPlaying(animState.name))
				animState.speed = speed;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void ResetAnimationSpeed()
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("ResetAnimationSpeed");
#endif
		foreach (AnimationState animState in gameObject.animation)
			if (this.animation.IsPlaying(animState.name))
				animState.speed = 1;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void ShakeCamera(string parameter)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("ShakeCamera " + parameter);
#endif
		string[] parameters = parameter.Split(',');
		float intensity, duration, interval;
		if (parameters.Length < 3
			|| float.TryParse(parameters[0], out intensity) == false
			|| float.TryParse(parameters[1], out duration) == false
			|| float.TryParse(parameters[2], out interval) == false)
		{
			Debug.LogWarning("ShakeCamera : invalid parameter : " + parameter);
			return;
		}

		if (Camera.main == null)
			return;

		KodGames.Effect.CameraShaker.Shake(Camera.main.gameObject, intensity, duration, interval);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void ShakeSelf(string parameter)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("ShakeSelf " + parameter);
#endif
		string[] parameters = parameter.Split(',');
		float intensity, duration, interval;
		if (parameters.Length < 3
			|| float.TryParse(parameters[0], out intensity) == false
			|| float.TryParse(parameters[1], out duration) == false
			|| float.TryParse(parameters[2], out interval) == false)
		{
			Debug.LogWarning("ShakeCamera : invalid parameter : " + parameter);
			return;
		}

		KodGames.Effect.CameraShaker.Shake(this.gameObject, intensity, duration, interval);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void ShakeUICamera(string parameter)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("ShakeSelf " + parameter);
#endif
		string[] parameters = parameter.Split(',');
		float intensity, duration, interval;
		if (parameters.Length < 3
			|| float.TryParse(parameters[0], out intensity) == false
			|| float.TryParse(parameters[1], out duration) == false
			|| float.TryParse(parameters[2], out interval) == false)
		{
			Debug.LogWarning("ShakeCamera : invalid parameter : " + parameter);
			return;
		}

		var uiContainer = ObjectUtility.FindChildObject(SysUIEnv.Instance.UICam.transform, GameDefines.uiCnt).gameObject;
		if (uiContainer == null)
			return;

		KodGames.Effect.CameraShaker.Shake(uiContainer, intensity, duration, interval);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void ShakeUI(string parameter)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("ShakeSelf " + parameter);
#endif
		string[] parameters = parameter.Split(',');
		float intensity, duration, interval;
		if (parameters.Length < 4
			|| float.TryParse(parameters[0], out intensity) == false
			|| float.TryParse(parameters[1], out duration) == false
			|| float.TryParse(parameters[2], out interval) == false
			|| string.IsNullOrEmpty(parameters[3]))
		{
			Debug.LogWarning("ShakeCamera : invalid parameter : " + parameter);
			return;
		}

		int uiType = _UIType.Parse(parameters[3],_UIType.UnKonw);
		if(uiType == _UIType.UnKonw)
		{
			Debug.LogWarning(string.Format("UIName {0} is invalid.",parameters[3]));
			return;
		}

		KodGames.Effect.CameraShaker.Shake(SysUIEnv.Instance.GetUIModule(uiType).gameObject, intensity, duration, interval);
	}

	[ContextMenu("Step")]
	void Step()
	{
		foreach (AnimationState animState in gameObject.animation)
			if (this.animation.IsPlaying(animState.name))
				animState.time = animState.length;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void PlayEffect(string parameter)
	{
#if LOG_ANIMATION_EVENT
		Debug.Log("PlayEffect " + parameter);
#endif
		string[] parameters = parameter.Split(',');
		if (parameters.Length < 2)
		{
			Debug.LogWarning("PlayEffect : invalid parameter : " + parameter);
			return;
		}

		string effectName = parameters[0];
		string parentGameObjectName = parameters[1];

		Transform parentGO = this.gameObject.transform;
		if (string.IsNullOrEmpty(parentGameObjectName) == false)
			parentGO = ObjectUtility.FindChildObject(parentGO, parentGameObjectName);

		SysFx.Instance.PlayFX(effectName, parentGO.gameObject, false, true, true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void LockUI()
	{
		SysUIEnv.Instance.LockUIInput();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void UnLockUI()
	{
		SysUIEnv.Instance.UnlockUIInput();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void AutoDestroy()
	{
		GameObject.Destroy(this.gameObject);
	}

	public delegate void UserEventDelegate(string eventName, object userData);
	public UserEventDelegate userEventDelegate;
	public object userData;

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void UserDefinedFunction(string eventName)
	{
		if (userEventDelegate != null)
		{
			userEventDelegate(eventName, userData);
		}
	}
}

