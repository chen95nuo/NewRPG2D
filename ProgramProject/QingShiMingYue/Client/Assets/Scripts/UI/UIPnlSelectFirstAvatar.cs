using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UIPnlSelectFirstAvatar : UIModule
{
	public List<UIButton> avatarBtns;

	public SpriteText selectAvatarDescLabel;
	public SpriteText selectAvatarTraitLabel;
	public UITextField playerNameLabel;
	public UIButton selectAvatarButton;
	public UIButton playerNameRandomButton;

	public List<GameObject> avatarModels;

	public float dragDelta;

	private int currentIndex;
	private List<string> playerNames;
	private int playerNameIndex = 0;
	private int soundIndex = -1;

	private List<GameConfig.InitAvatarConfig.InitAvatar> avatarIds;
	private Vector3 startVector = new Vector3(0, 0, 0);
	private readonly Color colorGary = new Color(128f / 255f, 128 / 255f, 128f / 255f, 255f / 255f);
	private readonly Color colorWhite = GameDefines.colorWhite;
	//拖拽是否完成
	private bool isScrolling;
	//拖拽之后延迟一秒是否完成
	private bool isDelay;
	private Dictionary<int, Avatar> avatars = new Dictionary<int, Avatar>();
	private EZAnimation delayAnimation;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		selectAvatarButton.SetDragDropDelegate(DragAvatarSelectButtonDel);

		return true;
	}

	private void Update()
	{
		//Update Idle Animation
		if (avatars.ContainsKey(currentIndex))
			if (avatars[currentIndex].IsLoopAnim() == false && avatars[currentIndex].AvatarAnimation.IsEnd() == true)
				PlayAnimByActionType(avatars[currentIndex], AvatarAction._Type.DefaultIdle);
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		InitView();

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		isScrolling = false;
	}

	private void InitView()
	{
		// Init Input Label.
		playerNameLabel.Text = "";
		currentIndex = 1;
		// Query random playerName.
		RequestMgr.Inst.Request(new FetchRandomPlayerNameReq());
		InitAvatarBtns();
		InitData();
		StartCoroutine("FillData");
	}

	private void InitData()
	{
		for (int index = 0; index < avatarBtns.Count; index++)
		{
			avatarBtns[index].data = index;
		}

		avatarIds = ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.initAvatars;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillData()
	{
		yield return null;

		// Load Avatar Model.

		for (int index = 0; index < avatarModels.Count; index++)
		{
			var model = avatarModels[index];
			int avatarId = avatarIds[index].avatarResourceId;
			int avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId).GetAvatarAssetId(0);
			//int avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId).GetHighAvatarAssetId(0);

			var avatar = Avatar.CreateAvatar(avatarId);
			//List<AvatarConfig.WeaponAsset> weapons = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId).showWeaponAssets;
			//for (int i = 0; i < weapons.Count; i++)
			//{
			//    avatar.UseComponent(weapons[i].avatarAssetId, weapons[i].mountBone);
			//}

			//avatar.UseComponent(weaponId, boneName);

			if (avatar.Load(avatarAssetId) == false)
				yield break;

			while (avatar.AvatarObject == null)
				yield return null;

			avatars.Add(index, avatar);

			// Set avatar capture
			model.transform.localPosition = SelectPlayerSceneData.Instance.marks[index].transform.localPosition;
			model.transform.localRotation = SelectPlayerSceneData.Instance.marks[index].transform.localRotation;

			// Put to mount bone.
			ObjectUtility.AttachToParentAndResetLocalTrans(model, avatar.gameObject);
		}

		ShowSelectAvatarInfo();
	}

	private void ShowSelectAvatarInfo()
	{
		InitAvatarBtns();
		selectAvatarDescLabel.Text = avatarIds[currentIndex].avatarDesc;
		selectAvatarTraitLabel.Text = avatarIds[currentIndex].avatarTraitDesc;

		if (delayAnimation != null)
		{
			delayAnimation.Clear();
			EZAnimator.instance.StopAnimation(delayAnimation, true);
		}

		delayAnimation = FadeSprite.Do(selectAvatarButton,
														  EZAnimation.ANIM_MODE.By,
														  selectAvatarButton.color,
														  EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Linear),
														  0f,
														  1f,
														  (data) =>
														  {
															  // Play Music
															  PlayAvatarVoice();
															  PlaySelectAnim();
														  },
														  null);

		//StartCoroutine(WaitTime(1.0f, currentIndex));
	}

	private void PlayAvatarVoice()
	{
		List<string> voiceList = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarIds[currentIndex].avatarResourceId).voices;
		int tempSoundIndex = Random.Range(0, voiceList.Count);

		if (tempSoundIndex == soundIndex && tempSoundIndex < voiceList.Count - 1)
			tempSoundIndex++;
		else if (tempSoundIndex == soundIndex && tempSoundIndex > 0)
			tempSoundIndex--;

		soundIndex = tempSoundIndex;

		if (!string.IsNullOrEmpty(voiceList[soundIndex]))
			AudioManager.Instance.PlayStreamSound(voiceList[soundIndex], 0f);
	}

	public void StopVoice()
	{
		if (ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarIds[currentIndex].avatarResourceId) == null)
			return;

		if (ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarIds[currentIndex].avatarResourceId).voices == null || ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarIds[currentIndex].avatarResourceId).voices.Count == 0)
			return;

		List<string> voiceList = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarIds[currentIndex].avatarResourceId).voices;

		if (!string.IsNullOrEmpty(voiceList[soundIndex]))
			AudioManager.Instance.StopSound(voiceList[soundIndex]);
	}

	private void PlaySelectAnim()
	{
		// Play SelectRole animation.
		int avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarIds[currentIndex].avatarResourceId).GetAvatarAssetId(0);
		var actionSelectRole = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.SpecialRole, 0);
		avatars[currentIndex].PlayAnim(actionSelectRole.GetAnimationName(avatarAssetId));
		isDelay = false;
	}

	private void PlayAnimByActionType(Avatar avatar, int Type)
	{
		int avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.AvatarId).GetAvatarAssetId(0);

		var actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, Type, 0);
		if (actionCfg != null)
			avatar.PlayAnim(actionCfg.GetAnimationName(avatarAssetId));
	}

	public void DragAvatarSelectButtonDel(EZDragDropParams parms)
	{
		switch (parms.evt)
		{
			case EZDragDropEvent.Update:

				if (isScrolling)
					return;

				float dragDelta = parms.dragObj.transform.localPosition.x;
				if (System.Math.Abs(dragDelta) > this.dragDelta)
				{
					if (dragDelta < 0)
						if (currentIndex + 1 != avatarBtns.Count)
							DragSelectAvatar(true);
					if (dragDelta > 0)
						if (currentIndex - 1 >= 0)
							DragSelectAvatar(false);
				}
				break;
		}
	}

	private void DragSelectAvatar(bool pre)
	{
		// Set Scroll State.
		isScrolling = true;
		StopVoice();
		//currentIndex = pre ? (currentIndex + 1 == avatarBtns.Count ? 0 : currentIndex + 1) : (currentIndex - 1 < 0 ? avatarBtns.Count - 1 : currentIndex - 1);
		currentIndex = pre ? currentIndex + 1 : currentIndex - 1;
		ShowSelectPlayer();
	}

	private void InitAvatarBtns()
	{
		for (int index = 0; index < avatarBtns.Count; index++)
		{
			if (index == currentIndex)
			{
				avatarBtns[index].controlIsEnabled = false;
				avatarBtns[index].Color = colorWhite;
			}
			else
			{
				avatarBtns[index].controlIsEnabled = true;
				avatarBtns[index].Color = colorGary;
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRandomAvatarName(UIButton btn)
	{
		RandomPlayerName();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatar(UIButton btn)
	{
		Debug.Log("false" + isDelay.ToString());
		//if (avatars[currentIndex].IsLoopAnim() == false || isDelay)
		//    return;
		//StopVoice();
		//PlayAvatarVoice();
		//PlayAnimByActionType(avatars[currentIndex], AvatarAction._Type.SelectRole);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelectAvatarBtnItem(UIButton btn)
	{
		//int startIndex = currentIndex;
		//切换清理

		StopVoice();
		currentIndex = (int)btn.data;
		InitAvatarBtns();

		ShowSelectPlayer();
	}

	private void ShowSelectPlayer()
	{
		GameObject scrollObj = SelectPlayerSceneData.Instance.cameraRoot;

		startVector.x = (currentIndex - 1) * (75);

		AnimatePosition.Do(scrollObj,
							   EZAnimation.ANIM_MODE.To,
							   startVector,
							   EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Linear),
							   0.5f,
							   0f,
							   (data) =>
							   {
								   PlayAnimByActionType(avatars[currentIndex], AvatarAction._Type.DefaultRole);
								   //延迟前禁止点击
								   isDelay = true;
							   },
							   (data) =>
							   {
								   ShowSelectAvatarInfo();
								   isScrolling = false;

							   });
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickStartGame(UIButton btn)
	{
		switch (IsValidName(playerNameLabel.Text))
		{
			case 0:
				int avatarReourceId = avatarIds[currentIndex].avatarResourceId;
				RequestMgr.Inst.Request(new GetTutorialAvatarAndSetPlayerNameReq(avatarReourceId, playerNameLabel.Text, ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.selectPlayerAvatarTutorialId));
				break;
			case 1:
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UITutorial_CreateAvatar_EmptyName"));
				break;
			case 2:
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UITutorial_CreateAvatar_InValidNameLength"));
				break;
			case 3:
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UITutorial_CreateAvatar_InValidNameFormat"));
				break;
		}
	}

	private int IsValidName(string name)
	{
		if (name == null || name.Length == 0)
			return 1;
		else if (name.Length > ConfigDatabase.DefaultCfg.GameConfig.playerNameMax)
			return 2;
		else if (name.IndexOf(" ") >= 0)
			return 3;

		return 0;
	}

	private void RandomPlayerName()
	{
		if (playerNames.Count > 0 && playerNameIndex >= playerNames.Count)
		{
			RequestMgr.Inst.Request(new FetchRandomPlayerNameReq());
			return;
		}

		if (playerNames != null && playerNames.Count > 0)
		{
			playerNameLabel.Text = playerNames[playerNameIndex++];
		}
		else
			playerNameLabel.Text = "";
	}

	public void OnResponseFetchRandomPlayerNameSuccess(List<string> playerNames)
	{
		this.playerNames = playerNames;
		this.playerNameIndex = 0;

		RandomPlayerName();
	}

	public void OnResponseGetTutorialAvatarSuccess()
	{
		HideSelf();
	}
}
