using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class AvatarAnimation
{
	public class AnimationEvent
	{
		public float triggerTime;
		public bool processed;
		public bool loop; // Processed only once.
		public object userData0;
		public object userData1;
	}

	public class PlayingAnim
	{
		public string animationName;
		public float speed = 1.0f;
		public AnimationState animationState;
		public List<AnimationEvent> eventList = new List<AnimationEvent>(); // AnimationEvent List.
		public Avatar.AnimationFinishDelegate animationFinishDelegate;
		public Avatar.AnimationEventDelegate animationEventDelegate;
		public object userData0;
		public object userData1;

		// AnimationData info.
		public float startFrame;
		public float endFrame;

		// Move info.
		public float movementStartTime; // Move start time.
		public float movementDuration; // Move duration.
		public bool movementBegin; // Move begin flag.

		public float fadeTime; // The time of cross fade animation.

		public void ResetEventList()
		{
			foreach (AnimationEvent evt in eventList)
				if (evt.loop)
					evt.processed = false;
		}

		public bool IsReverse
		{
			get { return animationState.speed < 0; }
		}

		public bool IsLoop
		{
			get { return animationState.wrapMode == WrapMode.Loop; }
		}

		public float Speed
		{
			get { return speed; }
			set
			{
				speed = value;
				animationState.speed = value;
			}
		}

		public float Length
		{
			get { return animationState.length; }
		}

		public float Time
		{
			get { return animationState.time; }
			set { animationState.time = value; }
		}

		// If clamped, return true.
		public bool ClampTime()
		{
			if (IsEnd)
			{
				// Reset animation time.
				if (IsReverse)
					Time += Length;
				else
					Time -= Length;

				return true;
			}

			return false;
		}

		public bool IsEnd
		{
			get
			{
				return (!IsReverse && Time > Length - fadeTime) || (IsReverse && Time < fadeTime);
			}
		}

		public bool TriggerTime(float time)
		{
			return (IsReverse && Time < time) || (!IsReverse && Time > time);
		}
	}

	private bool pause = false;
	public bool Pause
	{
		get { return pause; }
		set
		{
			if (pause == value)
				return;

			pause = value;

			if (playingAnimation != null)
			{
				foreach (AnimationState state in avatarAnimCmp)
				{
					if (state == playingAnimation.animationState)
					{
						state.speed = pause ? 0f : playingAnimation.speed;
						break;
					}
				}
			}
		}
	}

	private Avatar avatar; // The avatar which this animation module belongs to.
	private Animation avatarAnimCmp;
	private float animCrossTime; // AnimationData cross time.

	private PlayingAnim playingAnimation; // Current playing animation.
	public PlayingAnim PlayingAnimation
	{
		get { return playingAnimation; }
	}

	public AvatarAnimation(Avatar avatar)
	{
		this.avatar = avatar;
	}

	public void Release()
	{

	}

	public void CreateAnimation()
	{
		// Create animations when load resource from asset bundle.
		avatarAnimCmp = avatar.AvatarObject.GetComponent<Animation>();
		if (avatarAnimCmp == null)
			avatarAnimCmp = avatar.AvatarObject.AddComponent<Animation>();

		// Set playAutomatically false
		avatarAnimCmp.playAutomatically = false;
		avatarAnimCmp.cullingType = AnimationCullingType.AlwaysAnimate;
	}

	public bool IsLoopAnim()
	{
		if (playingAnimation == null)
			return false;

		return playingAnimation.IsLoop;
	}

	public bool IsEnd()
	{
		if (playingAnimation == null)
			return true;

		return playingAnimation.IsEnd;
	}

	public bool IsFinished()
	{
		if (playingAnimation == null)
			return true;

		return playingAnimation.animationState.normalizedTime == 1;
	}

	// Is animation moving begin.
	public bool IsAnimationMoveBegin()
	{
		// If animation is null, we think moving begin.
		if (playingAnimation == null)
			return true;

		return playingAnimation.movementBegin;
	}

	public void Update()
	{
		if (playingAnimation != null)
		{
			// AnimationData finish flag.
			bool animFinish = IsFinished();

			// Check animation end points if still playing.
			if (!animFinish)
			{
				// Loop animation, check it's reset events points.
				if (playingAnimation.IsLoop)
				{
					// If the time is clamped, reset all events.
					if (playingAnimation.ClampTime())
					{
						// Reset event.
						playingAnimation.ResetEventList();
					}
				}
				// Once animation, skip the cross fade time.
				else
				{
					animFinish = playingAnimation.IsEnd;
				}
			}
            
			// If animation finished, callback.
			if (animFinish)
			{
				if (playingAnimation.animationFinishDelegate != null)
				{
					playingAnimation.animationFinishDelegate(playingAnimation.userData0, playingAnimation.userData1);
					StopAnim();
				}
			}
			// Check events.
			else
			{
				// Process Event.
				for (int i = 0; i < playingAnimation.eventList.Count; i++)
				{
					AnimationEvent animEvent = playingAnimation.eventList[i];

					if (playingAnimation.TriggerTime(animEvent.triggerTime))
					{
						// If this anim is loop, events will be touched every loop.
						if (playingAnimation.IsLoop)
						{
							if (!animEvent.processed)
							{
                                if (playingAnimation.animationEventDelegate != null)
                                {
                                    playingAnimation.animationEventDelegate(animEvent.userData0, animEvent.userData1);
                                }
								animEvent.processed = true;
							}
						}
						else
						{
							if (playingAnimation.animationEventDelegate != null)
								playingAnimation.animationEventDelegate(animEvent.userData0, animEvent.userData1);

							// If current animation is stopped in the event callback, return directly.
							if (playingAnimation == null)
								return;

							playingAnimation.eventList.RemoveAt(i--);
						}
					}
				}

				// Check move flag.
				if (!playingAnimation.movementBegin && playingAnimation.TriggerTime(playingAnimation.movementStartTime))
				{
					playingAnimation.movementBegin = true;
				}
			}
		}
	}

	public void PreLoadAnimation(string animationName)
	{
		LoadAnimation(animationName);
	}

	private void LoadAnimation(string animationName)
	{
		AnimationConfig.Animation animationCfg = ConfigDatabase.DefaultCfg.AnimationConfig.GetAnimation(animationName);
		if (animationCfg == null)
			return;

		if (Avatar.LogAnim)
			avatar.Log(System.String.Format("Load missing animation:{0}", animationName));

		// Check if loaded
		if (avatarAnimCmp[animationCfg.name] != null)
			return;

		// Load the animation
		GameObject animGO = ResourceManager.Instance.LoadAsset<GameObject>(PathUtility.Combine(GameDefines.chrPath, animationCfg.assetName));
		if (animGO == null)
			return;

		// Add all animation clips
		Animation anim = animGO.animation;
		if (anim == null)
			return;

		foreach (AnimationState _animStat in anim)
		{
			avatarAnimCmp.AddClip(_animStat.clip, _animStat.name);

			AnimationState animStat = avatarAnimCmp[_animStat.name];
			animStat.blendMode = _animStat.blendMode;
			animStat.layer = _animStat.layer;
			animStat.normalizedSpeed = _animStat.normalizedSpeed;
			animStat.normalizedTime = _animStat.normalizedTime;
			animStat.speed = _animStat.speed;
			//animStat.time = _animStat.time;
			animStat.weight = _animStat.weight;
			animStat.wrapMode = animStat.wrapMode;
		}
	}

	public bool PlayAnim(string animationName)
	{
		// Get animation config.
		AnimationConfig.Animation animationCfg = ConfigDatabase.DefaultCfg.AnimationConfig.GetAnimation(animationName);
		if (animationCfg == null)
		{
			Debug.LogWarning("Miss animation in config : " + animationName);
			return false;
		}

		// If current animation is playing, return directly.
		if (playingAnimation != null && playingAnimation.animationName == animationCfg.name && avatarAnimCmp.IsPlaying(animationCfg.name))
			return true;

		// Stop current animation.
		StopAnim();

		// Load animation
		LoadAnimation(animationName);

		// If has no this animation state, return.
		if (avatarAnimCmp[animationCfg.name] == null)
		{
			Debug.LogWarning("Miss animation  : " + animationCfg.name);
			return false;
		}

		// New animation data.
		playingAnimation = new PlayingAnim();
		playingAnimation.animationName = animationCfg.name;
		playingAnimation.animationState = avatarAnimCmp[animationCfg.name];
		playingAnimation.startFrame = animationCfg.startFrame;
		playingAnimation.endFrame = animationCfg.endFrame;

		// Set animation speed.
		playingAnimation.Speed = animationCfg.speed;

		// Adjust animation play direction.
		playingAnimation.Speed = Mathf.Abs(playingAnimation.Speed) * (animationCfg.endFrame < animationCfg.startFrame ? -1 : 1);

		// Restart animation.
		playingAnimation.Time = playingAnimation.IsReverse ? playingAnimation.Length : 0;

		if (playingAnimation.IsLoop)
		{
			// Loop animation not set extra fade time.
			playingAnimation.fadeTime = 0;
			playingAnimation.movementStartTime = playingAnimation.IsReverse ? playingAnimation.Length : 0;
			playingAnimation.movementDuration = playingAnimation.Length;
		}
		else
		{
			// Calculate cross fade time.
			playingAnimation.fadeTime = playingAnimation.Length * 0.1f;

			// Set max cross time.
			if (playingAnimation.fadeTime > GameDefines.animCrossFadeTime)
				playingAnimation.fadeTime = GameDefines.animCrossFadeTime;

			// Calculate movement start time
			playingAnimation.movementStartTime = (animationCfg.moveStartFrame - Mathf.Min(animationCfg.moveEndFrame, animationCfg.moveStartFrame)) / playingAnimation.animationState.clip.frameRate;
			if (playingAnimation.movementStartTime < 0)
				playingAnimation.movementStartTime = playingAnimation.IsReverse ? playingAnimation.Length : 0;

			// Calculate movement duration
			playingAnimation.movementDuration = Mathf.Abs(animationCfg.moveStartFrame - animationCfg.moveEndFrame) / playingAnimation.animationState.clip.frameRate;

			if (playingAnimation.movementDuration <= 0)
				playingAnimation.movementDuration = playingAnimation.Length;

			// Force non-loop animation to WrapMode.ClampForever, to ensure AnimationState.time works correctly
			if (playingAnimation.animationState.wrapMode == WrapMode.Default || playingAnimation.animationState.wrapMode == WrapMode.Once)
				playingAnimation.animationState.wrapMode = WrapMode.ClampForever;
		}

		// Switch to this animation state.
		if (avatarAnimCmp.isPlaying)
			avatarAnimCmp.CrossFade(playingAnimation.animationName, playingAnimation.fadeTime <= 0 ? GameDefines.animCrossFadeTime : playingAnimation.fadeTime);
		else
			avatarAnimCmp.Play(playingAnimation.animationName);

		if (Avatar.LogAnim)
			avatar.Log(System.String.Format("PlayAnim Anim:{0}", playingAnimation.animationName));

		return true;
	}

	public void PlayDefaultAnim(int avatarAssetType)
	{
		if (ConfigDatabase.DefaultCfg.AnimationConfig == null)
			return;

		avatar.PlayAnim(ConfigDatabase.DefaultCfg.AnimationConfig.GetDefaultAnimation(avatarAssetType));
	}

	public void StopAnim()
	{
		if (Avatar.LogAnim)
			avatar.Log(System.String.Format("Stop Anim:{0}", playingAnimation != null ? playingAnimation.animationName : "null"));

		// Reset the old animation.
		playingAnimation = null;
	}

	public bool AddAnimationEvent(int eventID, bool loop, object userData0, object userData1)
	{
		if (playingAnimation == null)
			return false;

		// Calculate trigger time.
		float triggerTime;

		// Find event by id.
		AnimationConfig.Event animationEventCfg = ConfigDatabase.DefaultCfg.AnimationConfig.GetAnimationEvent(playingAnimation.animationName, eventID);
		if (animationEventCfg == null)
		{
			triggerTime = playingAnimation.IsReverse ? playingAnimation.Length : 0;
		}
		else
		{
			int keyFrame = animationEventCfg.keyFrame;
			keyFrame -= (int)Mathf.Min(playingAnimation.endFrame, playingAnimation.startFrame);
			triggerTime = keyFrame / playingAnimation.animationState.clip.frameRate;
		}

		// Clamp to range without fade time.
		if (playingAnimation.IsReverse && triggerTime > playingAnimation.Length - playingAnimation.fadeTime)
			triggerTime = playingAnimation.Length - playingAnimation.fadeTime;
		else if (!playingAnimation.IsReverse && triggerTime < playingAnimation.fadeTime)
			triggerTime = playingAnimation.fadeTime;

		// Push this event.
		AnimationEvent animationEvent = new AnimationEvent();
		animationEvent.triggerTime = triggerTime;
		animationEvent.userData0 = userData0;
		animationEvent.userData1 = userData1;
		animationEvent.loop = loop;
		playingAnimation.eventList.Add(animationEvent);

		return true;
	}

	public void SetAnimationDuration(float duration)
	{
		if (playingAnimation == null)
			return;

		// Loop animation will not scale duration.
		if (playingAnimation.IsLoop || duration <= 0)
			return;

		float spdVal = playingAnimation.Length / duration;

		playingAnimation.Speed = (playingAnimation.IsReverse ? -1 : 1) * spdVal;

		if (Avatar.LogAnim)
			avatar.Log(System.String.Format("SetAnimationDuration animSpd:{0}", playingAnimation.Speed));
	}

	public float GetAnimationDuration()
	{
		if (playingAnimation == null)
			return 0.0f;

		float spd = Mathf.Abs(playingAnimation.Speed);
		if (Mathf.Approximately(spd, 0.0f))
			return Mathf.Infinity;

		return playingAnimation.Length / spd;
	}

	public void SetAnimationDurationByMoveTime(float duration)
	{
		if (playingAnimation == null)
			return;

		// Loop animation will not scale duration.
		if (playingAnimation.IsLoop || duration <= 0)
			return;

		float spdVal = playingAnimation.movementDuration / duration;

		playingAnimation.Speed = (playingAnimation.IsReverse ? -1 : 1) * spdVal;

		if (Avatar.LogAnim)
			avatar.Log(System.String.Format("SetAnimationDurationByMoveTime animSpd:{0}", playingAnimation.Speed));
	}

	public bool SetAnimationFinishDeletage(Avatar.AnimationFinishDelegate animationFinishDelegate, object userData0, object userData1)
	{
		if (playingAnimation == null)
			return false;

		playingAnimation.animationFinishDelegate += animationFinishDelegate;
		playingAnimation.userData0 = userData0;
		playingAnimation.userData1 = userData1;

		return true;
	}

	public bool SetAnimationEventDelegate(Avatar.AnimationEventDelegate animationEventDelegate)
	{
		if (playingAnimation == null)
			return false;

		playingAnimation.animationEventDelegate += animationEventDelegate;

		return true;
	}
}
