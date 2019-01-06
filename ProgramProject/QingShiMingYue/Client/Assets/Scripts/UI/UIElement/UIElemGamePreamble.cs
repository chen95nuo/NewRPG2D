using UnityEngine;

public class UIElemGamePreamble : MonoBehaviour
{
	public UIButton borderButton;
	public UIPnlGamePreamble gamePreamble;
	public float[] timeStamps;
	public bool[] pauseStates;
	public bool enableBorderAtStart = true;

	public bool SkipToStep()
	{
		AnimationState currentAnimState = GetPlayingAnimState();
		if (currentAnimState == null)
			return false;

		float timeStamp = currentAnimState.length;
		bool pauseState = true;
		for (int i = 0; i < timeStamps.Length; ++i)
		{
			if (timeStamps[i] > currentAnimState.time)
			{
				// Skip to next time stamp
				timeStamp = timeStamps[i];
				pauseState = pauseStates[i];
				break;
			}
		}

		// Skip to next
		currentAnimState.time = timeStamp;
		//if (pauseState)
		//    currentAnimState.speed = 0;

		return pauseState;
	}

	public bool IsPlaying()
	{
		return CachedAnimation == null ? true : CachedAnimation.isPlaying;
	}

	public bool IsPausing()
	{
		foreach (AnimationState animState in CachedAnimation)
			if (CachedAnimation.IsPlaying(animState.name))
				return animState.speed == 0;

		return false;
	}

	public void SetPause(bool pause)
	{
		AnimationState currentAnimState = GetPlayingAnimState();
		if (currentAnimState == null)
			return;

		currentAnimState.speed = pause ? 0 : 1;
	}

	public void SetEanbleSkip(string parameter)
	{
		bool enable = false;
		if (bool.TryParse(parameter, out enable) == false)
		{
			Debug.LogError("SetEanbleSkip Parse failed : " + parameter);
			return;
		}

		borderButton.controlIsEnabled = enable;
	}

	private AnimationState GetPlayingAnimState()
	{
		foreach (AnimationState animState in CachedAnimation)
			if (CachedAnimation.IsPlaying(animState.name))
				return animState;

		return null;
	}

	public void SendTutorialCombatReq()
	{
		RequestMgr.Inst.Request(new NoviceCombatReq());
	}
}