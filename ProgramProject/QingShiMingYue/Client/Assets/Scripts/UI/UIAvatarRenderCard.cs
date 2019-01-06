using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;
using System.Collections;

public class UIAvatarRenderCard : MonoBehaviour, ICameraTraceable
{
	//parent transform which role model attached to.
	public GameObject avatarModelMarker;
	//RenderTexture
	public UIBtnCamera btnCamera;
	//Extra input area possible
	//public UIButton inputReceiver;
	//Rotation Support
	public CameraController_RenderTexHelper renderCam;
	public Transform renderCamTraceTarget;
	public float renderCamTraceDistance = 1;

	private float randomAnimTimer = 0;
	private BattleRole currentRole;
	private int defaultIdleAnimType = AvatarAction._Type.MelaleucaFloorIdle;
	private int selectRoleAnimType = AvatarAction._Type.SelectRole;

	private int avatarAssetId = IDSeg.InvalidId;
	private int avatarResourceId = IDSeg.InvalidId;

	public bool useShadow = false;
	public bool showIllusionWeapon = true;
	public bool showDefaultWeapon = true;
	public bool avatarPlayAnimWhenClick = false;
	public bool avatarRandomAnim = false;
	public float randomAnimIntervalTime = 10;

	public void Initialize(int avatarResourceId, int defaultIdleAnimType, int selectRoleAnimType)
	{
		this.avatarResourceId = avatarResourceId;
		this.avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarResourceId).GetAvatarAssetId(0);
		this.defaultIdleAnimType = defaultIdleAnimType;
		this.selectRoleAnimType = selectRoleAnimType;

		btnCamera.SetInputDelegate(InputDelegate);
		renderCam.SetTraceTarget(this, renderCamTraceDistance, 0, EZAnimation.EASING_TYPE.Default);

		StopCoroutine("ReCreateRole");
		StartCoroutine("ReCreateRole");
	}

	public void ResetAngles()
	{
		renderCam.ResetAngles();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	IEnumerator ReCreateRole()
	{
		if (currentRole != null)
			Destroy(currentRole.gameObject);

		Avatar avatar = Avatar.CreateAvatar(avatarAssetId);

		ObjectUtility.AttachToParentAndResetLocalTrans(avatarModelMarker, avatar.gameObject);

		currentRole = avatar.gameObject.AddComponent<BattleRole>();

		KodGames.ClientClass.CombatAvatarData avatarData = new KodGames.ClientClass.CombatAvatarData();
		avatarData.ResourceId = avatarResourceId;
		avatarData.DisplayName = name;
		avatarData.Attributes = new List<KodGames.ClientClass.Attribute>();

		currentRole.AvatarData = avatarData;
		currentRole.Awake();
		currentRole.AvatarAssetId = avatarAssetId;
		currentRole.Avatar.Load(avatarAssetId, true, useShadow);
		var illusionAvatarData = SysLocalDataBase.Inst.LocalPlayer.IllusionData.illusionAvatars.Find(n => n.recourseId == this.avatarResourceId);
		if (illusionAvatarData != null)
		{
			var illusion = illusionAvatarData.illusions.Find(n => n.useStatus != IllusionConfig._UseStatus.UnUse && n.useStatus != IllusionConfig._UseStatus.Unknow);
			if (illusion != null)
				currentRole.AvatarData.IllusionID = illusion.illusionId;
		}

		yield return null;

		if (showDefaultWeapon)
			currentRole.UseDefaultWeapons();

		if (showIllusionWeapon)
			currentRole.UseIllusionWeapons();

		currentRole.Avatar.SetGameObjectLayer(GameDefines.AvatarCaptureLayer);
		currentRole.Avatar.PlayAnimationByActionType(defaultIdleAnimType);
	}

	public void AvatarUseDefaultWeapons()
	{
		currentRole.DeleteIllusionWeapons();
		currentRole.UseDefaultWeapons();
	}

	public void AvatarChangeIllusionWeapon(int illusionId)
	{
		currentRole.DeleteDefaultWeapons();
		currentRole.DeleteIllusionWeapons();
		currentRole.AvatarData.IllusionID = illusionId;

		if (showDefaultWeapon)
			currentRole.UseDefaultWeapons();//recover default weapons

		currentRole.UseIllusionWeapons();//use new illusions
	}

	public void PlayIdleAnimation()
	{
		currentRole.Avatar.PlayAnimationByActionType(defaultIdleAnimType);
	}

	public void AvatarRecoverDefaultWeapon()
	{
		currentRole.DeleteIllusionWeapons();
		currentRole.UseDefaultWeapons();
	}

	//Random play role animation.
	private void RandomPlayAnimation()
	{
		if (currentRole.Avatar == null || currentRole.Avatar.CachedTransform.GetComponentInChildren<Animation>() == null)
			return;

		int selectCount = ConfigDatabase.DefaultCfg.ActionConfig.GetActionCountInType(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, selectRoleAnimType);
		var actionCfgSelected = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, selectRoleAnimType, Random.Range(0, selectCount));

		string selectAnimationName = actionCfgSelected.GetAnimationName(currentRole.AvatarAssetId);

		if (actionCfgSelected != null && !currentRole.Avatar.transform.GetComponentInChildren<Animation>().IsPlaying(selectAnimationName))
			currentRole.Avatar.PlayAnim(selectAnimationName);

		//Play Idle Anim when anim finished.
		currentRole.Avatar.SetAnimationFinishDeletage(
			(e1, e2) =>
			{
				currentRole.Avatar.PlayAnimationByActionType(defaultIdleAnimType);
			},
			null, null
			);
	}

	private void Update()
	{
		if (avatarRandomAnim)
		{
			randomAnimTimer += Time.deltaTime;

			if (randomAnimTimer > randomAnimIntervalTime)
			{
				randomAnimTimer = 0f;
				RandomPlayAnimation();
			}
		}
	}

	private void InputDelegate(ref POINTER_INFO data)
	{
		renderCam.OnInput(data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatar(UIBtnCamera ctrl)
	{
		if (avatarPlayAnimWhenClick)
		{
			randomAnimTimer = 0f;
			RandomPlayAnimation();
		}
	}

	#region ICameraTraceable Members

	public Vector3 TracingPosition
	{
		get
		{
			return renderCamTraceTarget.position;
		}
	}

	#endregion
}
