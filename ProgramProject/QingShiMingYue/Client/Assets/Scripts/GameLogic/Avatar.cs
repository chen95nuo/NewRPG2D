using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;

public class Avatar : MonoBehaviour
{
	// Animation callback.
	public delegate void AnimationFinishDelegate(object userData0, object userData1);
	public delegate void AnimationEventDelegate(object userData0, object userData1);

	//-------------------------------------------------------------------------
	// Log interface.
	//-------------------------------------------------------------------------
	protected static int logFlag; // Log flag.

	protected class LOG
	{
		public const int ANIM = 1 << 1;
		public const int STYLE = 1 << 2;
	}

	// Animation log flag.
	public static bool LogAnim
	{
		get { return (logFlag & LOG.ANIM) != 0; }
		set { logFlag = value ? (logFlag | LOG.ANIM) : (logFlag & ~LOG.ANIM); }
	}

	// Style log flag.
	public static bool LogStyle
	{
		get { return (logFlag & LOG.STYLE) != 0; }
		set { logFlag = value ? (logFlag | LOG.STYLE) : (logFlag & ~LOG.STYLE); }
	}

	public void Log(string msg)
	{
		Debug.Log(System.String.Format("Avatar {0} {1}", avatarId.ToString("X8"), msg));
	}

	// Avatar id.
	protected int avatarId;
	public int AvatarId
	{
		get { return avatarId; }
	}

	protected int avatarAssetId;
	public int AvatarAssetId
	{
		get { return avatarAssetId; }
	}

	protected int avatarTypeId;	// Avatar id.
	public int AvatarTypeId
	{
		get { return avatarTypeId; }
	}

	public bool Loaded
	{
		get { return avatarTypeId != IDSeg.InvalidId; }
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

			avatarAnimation.Pause = value;
		}
	}

	// Avatar object.
	protected GameObject avatarObject;
	public GameObject AvatarObject
	{
		get { return avatarObject; }
		set
		{
			if (value == null)
				return;

			avatarObject = value;

			avatarObject.name = ConfigDatabase.DefaultCfg.AvatarAssetConfig.GetAvatarById(avatarTypeId).skeleton;
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(gameObject, AvatarObject);

			// Find left&right foot transform.
			leftFootTransform = ObjectUtility.FindChildObject(AvatarObject, GameDefines.lftFoot).transform;
			rightFootTransform = ObjectUtility.FindChildObject(AvatarObject, GameDefines.rftFoot).transform;
		}
	}

	public GameObject Shadow
	{
		get { return shadow.gameObject; }
	}

	protected Transform leftFootTransform;   // Left foot trans.
	protected Transform rightFootTransform;   // right foot trans.

	protected Transform shadow; 	// Shadow object.
	protected Vector3 shadowPs;

	protected AvatarAnimation avatarAnimation; // Avatar animation module.
	public AvatarAnimation AvatarAnimation
	{
		get { return avatarAnimation; }
	}

	protected AvatarComponent avatarComponent; // Avatar component module.
	protected AvatarFx avatarFx; // Avatar effect module.

	private Color materialColor = GameDefines.defaultAvatarColor;

	public static Avatar CreateAvatar(int avatarId)
	{
		// Create root game object
		GameObject gameObject = new GameObject("avatar " + avatarId.ToString("X8"));

		// Not destroy this avatar when load level.
		//GameObject.DontDestroyOnLoad(gameObject);

		// Create avatar component
		Avatar avatar = gameObject.AddComponent<Avatar>();
		avatar.avatarId = avatarId;

		return avatar;
	}

	public void Awake()
	{
		// Create modules.
		avatarComponent = new AvatarComponent(this); // Avatar component module.
		avatarAnimation = new AvatarAnimation(this);
		avatarFx = new AvatarFx(this); // Avatar effect module.
	}

	public bool Load(int avatarAssetId)
	{
		return Load(avatarAssetId, false, true);
	}

	public bool Load(int avatarAssetId, bool playDefaultAnim, bool useShadow)
	{
		this.avatarAssetId = avatarAssetId;

		// Save the avatar id.
		this.avatarTypeId = AvatarAssetConfig.ComponentIdToAvatarTypeId(avatarAssetId);

		// Check avatar id.
		if (ConfigDatabase.DefaultCfg.AvatarAssetConfig.GetAvatarById(avatarTypeId) == null)
		{
			Debug.LogError(System.String.Format("Invalid avatar id: {0:X}", avatarTypeId));
			return false;
		}

		// Save old scale.
		Vector3 oldScale = gameObject.transform.localScale;
		gameObject.transform.localScale = new Vector3(1, 1, 1);

		// Create avatar object.
		if (avatarComponent.UseComponent(avatarAssetId, ""))
		{
			// Create avatar animation
			avatarAnimation.CreateAnimation();

			// Play the default animation.
			if (playDefaultAnim)
				avatarAnimation.PlayDefaultAnim(avatarTypeId);
		}

		// Create shadow.
		if (useShadow)
		{
			GameObject shadowGO = ResourceManager.Instance.InstantiateAsset<GameObject>(GameDefines.avtShadow);
			if (shadowGO != null)
			{
				shadow = shadowGO.transform;
				ObjectUtility.AttachToParentAndKeepLocalTrans(gameObject, shadow.gameObject);
				shadow.gameObject.name = "shadow";
			}
		}

		// Finished loading recover old scale
		gameObject.transform.localScale = oldScale;

		// Process delay used styles.
		avatarComponent.ReuseDelayStyles();

		return true;
	}

	public void Destroy()
	{
		// Release avatar modules.
		if (avatarComponent != null)
			avatarComponent.Release();

		if (avatarAnimation != null)
			avatarAnimation.Release();

		if (avatarFx != null)
			avatarFx.Release();

		if (shadow != null)
		{
			GameObject.Destroy(shadow.gameObject);
		}

		GameObject.Destroy(gameObject);
	}

	public void Update()
	{
		// Update avatar animation.
		avatarAnimation.Update();

		// Update avatar component.
		avatarComponent.Update();

		// Update avatar effect.
		avatarFx.Update();

		// Update shadow.
		if (shadow != null && leftFootTransform != null && rightFootTransform != null)
		{
			Vector3 shadowPos = (leftFootTransform.position + rightFootTransform.position) / 2;
			shadowPos.y = 0.01f;
			shadow.position = shadowPos;
		}
	}

	public void SetGameObjectLayer(int layer)
	{
		transform.gameObject.layer = layer;

		foreach (Transform subObject in transform.GetComponentsInChildren<Transform>())
		{
			// except clickRoleButton
			UIButton3D clickRoleButton = subObject.gameObject.GetComponent<UIButton3D>();
			if (clickRoleButton != null)
			{
				continue;
			}

			subObject.gameObject.layer = layer;
		}
	}

	public void SetColor(Color color)
	{
		if (avatarObject == null)
			return;

		if (materialColor == color)
			return;

		materialColor = color;

		Renderer[] renderers = avatarObject.GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers)
			foreach (var material in renderer.materials)
				material.color = color;
	}

	//////////////////////////////////////////////////////////////////////////
	// Component
	public bool UseComponent(int componentId, string mountBone)
	{
		var cfg = ConfigDatabase.DefaultCfg.AvatarAssetConfig.GetComponentById(componentId);
		if (cfg == null)
		{
			Debug.LogError("UseComponent cfg == null componentId:" + componentId.ToString("X8"));
			return false;
		}

		if (cfg.Part.assembleType == AvatarAssetConfig._AssembleType.Skin)
		{
			Debug.LogError("Can not change skin at runtime.");
			return false;
		}

		return avatarComponent.UseComponent(componentId, mountBone);
	}

	public void DeleteComponent(int componentId)
	{
		avatarComponent.DeleteCompoent(componentId);
	}

	//public GameObject ExtractMeshComponent(int componentId)
	//{
	//    return avatarComponent.ExtractMeshComponent(componentId);
	//}

	public string GetComponentAssetPath(int componentId)
	{
		return avatarComponent.GetComponentAssetPath(componentId);
	}

	public GameObject CloneComponent(int componentId)
	{
		return avatarComponent.CloneComponent(componentId);
	}

	public AvatarComponent.UsedComponent GetUsedComponent(int componentId)
	{
		return avatarComponent.GetUsedComponent(componentId);
	}

	public void SetActiveComponent(int componentId, bool active)
	{
		avatarComponent.SetActiveComponent(componentId, active);
	}

	#region Animation
	//////////////////////////////////////////////////////////////////////////
	// Animation
	public bool PlayAnim(string animName)
	{
		return avatarAnimation.PlayAnim(animName);
	}

	public void PreLoadAnimation(string animName)
	{
		avatarAnimation.PreLoadAnimation(animName);
	}

	public void StopAnim()
	{
		avatarAnimation.StopAnim();

		// Stop animation pfx.
		avatarFx.StopAnimPfx();
	}

	public bool IsLoopAnim()
	{
		return avatarAnimation.IsLoopAnim();
	}

	public bool SetAnimationFinishDeletage(AnimationFinishDelegate animationFinishDelegate, object userData0, object userData1)
	{
		return avatarAnimation.SetAnimationFinishDeletage(animationFinishDelegate, userData0, userData1);
	}

	public bool SetAnimEventCb(AnimationEventDelegate animationEventDelegate)
	{
		return avatarAnimation.SetAnimationEventDelegate(animationEventDelegate);
	}

	public bool AddAnimationEvent(int eventID, bool loop, object userData0, object userData1)
	{
		return avatarAnimation.AddAnimationEvent(eventID, loop, userData0, userData1);
	}

	public void SetAnimDuration(float duration)
	{
		avatarAnimation.SetAnimationDuration(duration);
	}

	public void SetAnimDurationByMoveTime(float duration)
	{
		avatarAnimation.SetAnimationDurationByMoveTime(duration);
	}

	public float GetAnimationDuration()
	{
		return avatarAnimation.GetAnimationDuration();
	}

	// Is animation moving begin.
	public bool IsAnimMoveBegin()
	{
		return avatarAnimation.IsAnimationMoveBegin();
	}
	#endregion

	#region  Particle
	//////////////////////////////////////////////////////////////////////////
	// Particle
	public void PlayPfx(FXController pfx, int destroyMode, int usd, string modelBone, string bone, bool boneFollow, Vector3 offset, Vector3 rotate, bool useSpecificPosition, Vector3 specificPosition)
	{
		avatarFx.PlayPfx(pfx, destroyMode, usd, modelBone, bone, boneFollow, offset, rotate, useSpecificPosition, specificPosition);
	}

	public void StopPfxByInstID(int instID)
	{
		avatarFx.StopPfxByInstID(instID);
	}

	public void StopPfxByUsd(int usd)
	{
		avatarFx.StopPfxByUsd(usd);
	}

	public void StopPfxByName(string name)
	{
		avatarFx.StopPfxByName(name);
	}

	public void StopAllPfx()
	{
		avatarFx.StopAllPfx();
	}

	public List<int> GetPfxInstIDListByUsd(int usd)
	{
		return avatarFx.GetPfxInstIDListByUsd(usd);
	}
	#endregion

	#region 非战斗中使用
	
	/// <summary>
	/// 非战斗中使用
	/// </summary>
	public void PlayAnimationByActionType(int actionType)
	{
		PlayAnimationByActionType(actionType, EquipmentConfig._WeaponType.Empty, _CombatStateType.Default);
	}
	
	/// <summary>
	/// 非战斗中使用
	/// </summary>
	public void PlayAnimationByActionType(int actionType, int weaponType, int combatStateType)
	{
		int actionCount = ConfigDatabase.DefaultCfg.ActionConfig.GetActionCountInType(weaponType, combatStateType, actionType);
		if (actionCount == 0)
		{
			AvatarAnimation.PlayDefaultAnim(AvatarAssetConfig.ComponentIdToAvatarTypeId(AvatarAssetId));
			return;
		}

		AvatarAction animAction = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(weaponType, combatStateType, actionType, 0);

		if (animAction.GetAnimationName(AvatarAssetId) == "")
		{
			Debug.LogError(string.Format("Animation name is empty in action({0})", animAction.id.ToString("X")));
			return;
		}

		PlayAnim(animAction.GetAnimationName(AvatarAssetId));
	}

	#endregion
}
