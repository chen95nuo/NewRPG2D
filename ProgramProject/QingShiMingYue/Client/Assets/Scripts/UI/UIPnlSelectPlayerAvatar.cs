using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UIPnlSelectPlayerAvatar : UIModule
{
	public SpriteText selectAvatarDescLabel;
	public SpriteText selectAvatarTraitLabel;
	public UITextField playerNameLabel;
	public UIButton playerNameRandomButton;
	public UIButton gamestartButton;

	//public float dragDelta;

	private int currentIndex;
	private List<string> playerNames;
	private int playerNameIndex = 0;
	private int soundIndex = -1;

	private GameConfig.InitAvatarConfig.InitAvatar avatarId;
	//拖拽是否完成
	public bool isScrolling;
	//拖拽之后延迟一秒是否完成
	//private bool isDelay;
	private Dictionary<int, Avatar> avatars = new Dictionary<int, Avatar>();
	//private EZAnimation delayAnimation;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// Record Game Data.
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.ShowSelectAvatar);

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
		InitData();
		StartCoroutine("FillData");
	}

	private void Update()
	{
		//Update Idle Animation	
		if (avatars.ContainsKey(currentIndex))
			if (avatars[currentIndex].IsLoopAnim() == false && avatars[currentIndex].AvatarAnimation.IsEnd() == true)
				PlayAnimByActionType(avatars[currentIndex], AvatarAction._Type.DefaultIdle);
	}

	private void InitData()
	{
		avatarId = ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.initAvatars[0];
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillData()
	{
		yield return null;

		// Load Avatar Model.
		var model = SelectPlayerSceneData.Instance.marks[0];
		int avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId.avatarResourceId).GetHighAvatarAssetId(0);

		var avatar = Avatar.CreateAvatar(avatarId.avatarResourceId);

		if (avatar.Load(avatarAssetId) == false)
			yield break;

		List<AvatarConfig.WeaponAsset> weapons = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId.avatarResourceId).showWeaponAssets;
		for (int i = 0; i < weapons.Count; i++)
		{
			avatar.UseComponent(weapons[i].avatarAssetId, weapons[i].mountBone);
		}

		while (avatar.AvatarObject == null)
			yield return null;

		avatars.Add(currentIndex, avatar);

		// Set avatar capture
		model.transform.localPosition = SelectPlayerSceneData.Instance.marks[0].transform.localPosition;
		model.transform.localRotation = SelectPlayerSceneData.Instance.marks[0].transform.localRotation;

		// Put to mount bone.
		ObjectUtility.AttachToParentAndResetLocalTrans(model, avatar.gameObject);

		ShowSelectAvatarInfo();
	}

	private void ShowSelectAvatarInfo()
	{

		selectAvatarDescLabel.Text = avatarId.avatarDesc;
		selectAvatarTraitLabel.Text = avatarId.avatarTraitDesc;

		PlayAvatarVoice();
		PlaySelectAnim();
	}

	private void PlayAvatarVoice()
	{
		List<string> voiceList = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId.avatarResourceId).voices;
		if (voiceList != null && voiceList.Count > 0)
		{
			soundIndex = Random.Range(0, voiceList.Count);
			if (!string.IsNullOrEmpty(voiceList[soundIndex]))
				AudioManager.Instance.PlayStreamSound(voiceList[soundIndex], 0f);
		}
	}

	public void StopVoice()
	{
		if (ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId.avatarResourceId) == null)
			return;

		if (ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId.avatarResourceId).voices == null || ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId.avatarResourceId).voices.Count == 0)
			return;

		List<string> voiceList = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId.avatarResourceId).voices;

		if (!string.IsNullOrEmpty(voiceList[soundIndex]))
			AudioManager.Instance.StopSound(voiceList[soundIndex]);
	}

	private void PlaySelectAnim()
	{
		// Play SelectRole animation.
		int avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId.avatarResourceId).GetAvatarAssetId(0);
		var actionSelectRole = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.SpecialRole, 0);
		avatars[currentIndex].PlayAnim(actionSelectRole.GetAnimationName(avatarAssetId));
		//isDelay = false;
	}

	private void PlayAnimByActionType(Avatar avatar, int Type)
	{
		int avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.AvatarId).GetAvatarAssetId(0);

		var actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, Type, 0);
		if (actionCfg != null)
			avatar.PlayAnim(actionCfg.GetAnimationName(avatarAssetId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRandomAvatarName(UIButton btn)
	{
		RandomPlayerName();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickStartGame(UIButton btn)
	{
		switch (IsValidName(playerNameLabel.Text))
		{
			case 0:
				int avatarReourceId = avatarId.avatarResourceId;
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
		gamestartButton.controlIsEnabled = false;
	}
}
