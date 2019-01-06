using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlAdventureMessage : UIModule
{
	private class DialogData
	{
		public int dialogId;
		public int step;
		public int messageStep;
		public float delta;
		public DialogueConfig.Dialogues cacheDialogue;

		public DialogData()
		{
			dialogId = 0;
			step = 0;
			messageStep = 0;
			delta = 0;
		}

		public DialogueConfig.Dialogues GetDialog()
		{
			if (ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId) != null)
			{
				if (step < ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId).dialogues.Count)
					return ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId).dialogues[step];
			}
			return null;
		}

		public void Next()
		{
			step++;
			messageStep = 0;
			delta = 0;
		}

		public bool HasNext()
		{
			if (ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId) == null)
				return false;
			return dialogId != 0 && (step + 1) < ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId).dialogues.Count;
		}
	}

	public SpriteText messageText;
	public SpriteText titleText;

	private float dialogueSpeed = 0.05f;

	public UIElemAssetIcon portraitIcon;
	public UIBtnCamera portrait3D;
	public GameObject portait3dMaker;
	public GameObject cursorNext;
	public UIButton backBtn;
	public UIBox delaySign;

	private DialogData dialogData;
	private float avatarZdepth = 2f;
	private Dictionary<int, Avatar> avatarCaches = new Dictionary<int, Avatar>();
	private float[] messLocations = new float[] { -98f, -52f };
	private bool isDialogType = true;
	private int dialogSetId = 0;

	public void SetData(bool isDialogType, int dialogSetId)
	{
		this.isDialogType = isDialogType;
		this.dialogSetId = dialogSetId;

		if (isDialogType)
		{
			cursorNext.SetActive(true);
			//staminaObject.SetActive(true);
			backBtn.gameObject.SetActive(true);
			SetDialogData(dialogSetId);
		}
		else
		{
			backBtn.gameObject.SetActive(false);
			cursorNext.SetActive(false);
			//staminaObject.SetActive(false);
			if (ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogSetId) != null)
			{
				DialogueConfig.Dialogues dialog = ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogSetId).dialogues[0];
				messageText.Text = dialog.dialogueValue;

				titleText.Text = GetCampTypeStr(dialog) + ItemInfoUtility.GetAssetQualityLongColor(dialog.avatarId) + dialog.portraitName;
				GetPositionType(dialog);
				dialogData = new DialogData();
				dialogData.dialogId = dialogSetId;
			}
		}
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SetData((bool)userDatas[0], (int)userDatas[1]);
		
		return true;
	}

	private bool GetPositionType(DialogueConfig.Dialogues dialog)
	{
		if (dialog.positionType == DialogueConfig._PositionType.Unknown)
		{
			portraitIcon.CachedTransform.parent.gameObject.SetActive(false);
			messageText.transform.localPosition = new Vector3(messLocations[0], messageText.transform.localPosition.y, messageText.transform.localPosition.z);
			titleText.transform.localPosition = new Vector3(messLocations[0], titleText.transform.localPosition.y, titleText.transform.localPosition.z);
			titleText.Text = dialog.portraitName;
			return false;
		}
		else
		{
			portraitIcon.CachedTransform.parent.gameObject.SetActive(true);
			messageText.transform.localPosition = new Vector3(messLocations[1], messageText.transform.localPosition.y, messageText.transform.localPosition.z);
			titleText.transform.localPosition = new Vector3(messLocations[1], titleText.transform.localPosition.y, titleText.transform.localPosition.z);
			return true;
		}
	}

	private string GetCampTypeStr(DialogueConfig.Dialogues dialog)
	{
		string titleMess = "";
		if (dialog.dialogueCampType == DialogueConfig._CampType.Enemy)
			GameUtility.GetUIString("UIPnlAdventureMessage_Enemy");
		else if (dialog.dialogueCampType == DialogueConfig._CampType.MyRange)
			titleMess = GameUtility.GetUIString("UIPnlAdventureMessage_MyRange");
		else if (dialog.dialogueCampType == DialogueConfig._CampType.Other)
			titleMess = GameUtility.GetUIString("UIPnlAdventureMessage_Other");
		else if (dialog.dialogueCampType == DialogueConfig._CampType.Unknown)
			titleMess = GameUtility.GetUIString("UIPnlAdventureMessage_Unknown");
		return titleMess;
	}

	private void SetPortrait(DialogueConfig.Dialogues dialoguesCfg)
	{
		if (dialoguesCfg == null)
			return;

		if (!GetPositionType(dialoguesCfg)) return;
		if (portrait3D.GetComponent<BoxCollider>() != null)
			portrait3D.GetComponent<BoxCollider>().enabled = false;
		GameObject showPortRoot = portraitIcon.CachedTransform.parent.gameObject;

		if (showPortRoot.activeInHierarchy)
		{

			if (dialogData.cacheDialogue != null && dialogData.cacheDialogue.dialogueIndex == dialoguesCfg.dialogueIndex)
				return;
		}
		else
			showPortRoot.SetActive(true);

		dialogData.cacheDialogue = dialoguesCfg;

		// Set portrait.
		switch (dialoguesCfg.portraitDisplayType)
		{
			case DialogueConfig._PortraitDisplayType.Portrait2D:
				{
					portrait3D.gameObject.SetActive(false);
					portraitIcon.Hide(false);
					//titleText.Text = ItemInfoUtility.GetAssetQualityLongColor(dialoguesCfg.avatarId) + dialoguesCfg.portraitName;
					titleText.Text = dialoguesCfg.portraitName;
					if (portraitIcon != null)
					{
						switch (dialoguesCfg.portraitType)
						{
							case DialogueConfig._PortraitType.Player:
								{
									int index = dialoguesCfg.portraitIconIndex < ConfigDatabase.DefaultCfg.DialogueConfig.playerPortraitIcons.Count ? dialoguesCfg.portraitIconIndex : 0;
									portraitIcon.SetData(ConfigDatabase.DefaultCfg.DialogueConfig.playerPortraitIcons[index]);
								}
								break;
							case DialogueConfig._PortraitType.PlayerAssistant:
								{
									int index = dialoguesCfg.portraitIconIndex < ConfigDatabase.DefaultCfg.DialogueConfig.playerAssistantPortraitIcons.Count ? dialoguesCfg.portraitIconIndex : 0;
									portraitIcon.SetData(ConfigDatabase.DefaultCfg.DialogueConfig.playerAssistantPortraitIcons[index]);
								}
								break;
							default:
								portraitIcon = null;
								break;
						}
					}
				}
				break;

			case DialogueConfig._PortraitDisplayType.Portrait3D:
				{
					portrait3D.gameObject.SetActive(true);
					portraitIcon.Hide(true);

					int avatarId = IDSeg.InvalidId;

					switch (dialoguesCfg.portraitType)
					{
						case DialogueConfig._PortraitType.Avatar:
							if (dialoguesCfg.dialogueCampType == DialogueConfig._CampType.MyRange)
								avatarId = ItemInfoUtility.GetAvatarFirstIconID();
							else
								avatarId = dialoguesCfg.avatarId;
							break;
						case DialogueConfig._PortraitType.Npc:
							avatarId = dialoguesCfg.npcId;
							break;
						default:
							Debug.LogWarning("Invalid portraitType : " + dialoguesCfg.portraitType);
							break;
					}
					if (dialoguesCfg.dialogueCampType == DialogueConfig._CampType.MyRange)
						titleText.Text = SysLocalDataBase.Inst.LocalPlayer.Name;
					else
						titleText.Text = dialoguesCfg.portraitName;
					for (int index = 0; index < portait3dMaker.transform.childCount; index++)
						portait3dMaker.transform.GetChild(index).gameObject.SetActive(false);

					// Find avatar in cached by avatar id
					// If not exist create a new one and add to cache
					Avatar avatar = null;
					if (avatarCaches.ContainsKey(avatarId))
						avatar = avatarCaches[avatarId];
					else
					{
						avatar = Avatar.CreateAvatar(avatarId);
						avatarCaches.Add(avatarId, avatar);

						var currentAvatar=ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId);
						if (currentAvatar == null) return;
						int avatarAssetId = currentAvatar.GetAvatarAssetId(0);
						if (avatar.Load(avatarAssetId) == false)
							return;

						// Set layer
						avatar.SetGameObjectLayer(GameDefines.AvatarCaptureLayer);

						ParticleSystem[] ps = avatar.GetComponentsInChildren<ParticleSystem>();
						foreach (var psTemp in ps)
							Destroy(psTemp.gameObject);
					}

					// Set avatar capture
					portait3dMaker.transform.localPosition = new Vector3(portait3dMaker.transform.localPosition.x, portait3dMaker.transform.localPosition.y, avatarZdepth);

					// Put to mount bone.
					ObjectUtility.AttachToParentAndResetLocalTrans(portait3dMaker.gameObject, avatar.gameObject);

					if (avatar.gameObject.activeInHierarchy == false)
						avatar.gameObject.SetActive(true);

					// Play animation.
					var actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Dialogue, 0);
					if (actionCfg != null)
						avatar.PlayAnim(actionCfg.GetAnimationName(ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId).GetAvatarAssetId(0)));
				}
				break;
		}
	}

	private void SetDialogData(int dialogId)
	{
		DialogData tempDialog = new DialogData();
		tempDialog.dialogId = dialogId;

		dialogData = tempDialog;
	}

	private void InitView()
	{
		messageText.Text = "";
		//dialogData = null;
	}

	private void Update()
	{
		if (AdventureSceneData.Instance.HadDelayReward > 0)
			delaySign.Hide(false);
		else delaySign.Hide(true);

		if (isDialogType)
		{
			DialogueConfig.Dialogues dialog = dialogData == null ? null : dialogData.GetDialog();

			if (dialog == null)
				return;


			// Set the Portrait Icon.
			SetPortrait(dialog);

			if (dialogData.delta >= 0)
			{
				dialogData.delta += Time.deltaTime;

				if (dialogData.delta >= dialogueSpeed)
				{
					dialogData.delta = 0;
					string dialogValue = dialog.dialogueValue;
					if (dialogData.messageStep < dialogValue.Length)
					{
						messageText.Text = dialogValue.Substring(0, dialogData.messageStep + 1);
						dialogData.messageStep++;
					}
					else
					{
						dialogData.delta = -1;
						dialogData.messageStep = 0;
					}
				}
			}
		}
		else
		{
			if (ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogSetId) != null)
			{
				DialogueConfig.Dialogues dialog = ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogSetId).dialogues[0];
				SetPortrait(dialog);

			}
		}
	}

	public override void OnHide()
	{
		dialogData.cacheDialogue = null;
		dialogData = null;

		// Clear all portrait icon and avatar gameObject
		foreach (var avatarCache in avatarCaches)
		{
			if (avatarCache.Value != null)
				avatarCache.Value.Destroy();
		}
		avatarCaches.Clear();

		base.OnHide();
		messageText.Text = "";
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnShowMessage(UIButton btn)
	{
		if (!isDialogType) return;
		if (dialogData == null)
			return;

		if (dialogData.delta >= 0)
		{
			// Show All Message of current step.
			dialogData.delta = -1;
			dialogData.messageStep = 0;
			if (dialogData.GetDialog() != null)
				messageText.Text = dialogData.GetDialog().dialogueValue;
		}
		else
		{
			// Show Next Step.
			if (dialogData.HasNext())
			{
				dialogData.Next();
				InitView();
			}
			else
			{
				//没有了
				RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(0, -1, null));
				OnHide();
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlAdventureMain));
		OnHide();
	}

	public override void RemoveOverlay()
	{
		if (isDialogType)
		{
			DialogueConfig.Dialogues dialog = dialogData == null ? null : dialogData.GetDialog();
			if (dialog == null)
				return;
			SetPortrait(dialog);
		}
		else
		{
			if (ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogSetId) != null)
				SetPortrait(ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogSetId).dialogues[0]);
		}
		base.RemoveOverlay();


	}
}