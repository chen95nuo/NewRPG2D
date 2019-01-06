using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UITipAdviser : UIModule
{
	private class DialogData
	{
		public int dialogId;
		public int step;
		public int messageStep;
		public float delta;
		public OnHideDelegate onHideDelegate;
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
			if (step < ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId).dialogues.Count)
				return ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId).dialogues[step];
			else
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
			return dialogId != 0 && (step + 1) < ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId).dialogues.Count;
		}
	}

	private DialogData dialogData;
	public delegate void OnHideDelegate();
	private int dialogId;
	private OnHideDelegate onHideDelegate;

	public GameObject dialogueRoot;
	public GameObject dialogueGameObj;
	public AutoSpriteControlBase top;
	public UIElemAssetIcon npcLeft2D;
	public UIElemAssetIcon npcRight2D;
	public UIBtnCamera npcLeft3D;
	public UIBtnCamera npcRight3D;
	public GameObject npcLeft3dMaker;
	public GameObject npcRight3dMaker;
	public SpriteText leftCampLabel;
	public SpriteText rightCampLabel;
	public GameObject anchor;
	public AutoSpriteControlBase dialogueLeftName;
	public AutoSpriteControlBase dialogueRightName;
	public SpriteText dialogueMessage;

	public EZAnimation.EASING_TYPE directEasing;
	public EZAnimation.EASING_TYPE alphaEasing;
	public float dialogueSpeed = 0.5f;
	public Color endColor;

	public float bgShowDelay = 0.5f;
	public float downBgAlphaTime = 1f;
	public float messageBgShowDelay = 2f;

	public float avatarZdepth = 2f;

	private Dictionary<int, Avatar> avatarCaches = new Dictionary<int, Avatar>();
	private int dialogEnemyId;
	private string avatarName = "";

	public override void OnHide()
	{
		dialogData = null;

		// Clear all portrait icon and avatar gameObject
		foreach (var avatarCache in avatarCaches)
		{
			if (avatarCache.Value != null)
				avatarCache.Value.Destroy();
		}
		avatarCaches.Clear();

		base.OnHide();
	}

	public void ShowDialogueAvatar(int dialogId, string avatarName, int enemyId, OnHideDelegate onHideDelegate)
	{
		this.avatarName = avatarName;
		this.dialogEnemyId = enemyId;
		ShowDialogue(dialogId, onHideDelegate);
	}

	public void ShowDialogue(int dialogId, OnHideDelegate onHideDelegate)
	{
		if (ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId) == null)
		{
			Debug.LogError("DialogueId " + dialogId.ToString("X") + " is not Found in the DialogueConfig.");

			if (onHideDelegate != null)
				onHideDelegate();

			return;
		}

		if (IsShown == false)
			ShowSelf();

		this.dialogId = dialogId;
		this.onHideDelegate = onHideDelegate;

		InitView();

		// Delay show Background.
		// Alpha Bottom Background.
		FadeSpriteAlpha.Do(top,
						   EZAnimation.ANIM_MODE.FromTo,
						   top.Color,
						   EZAnimation.GetInterpolator(alphaEasing),
						   bgShowDelay,
						   0f,
						   null,
						   (data) =>
						   {
							   if (top != null)
								   top.gameObject.SetActive(true);
						   });

		Invoke("SetDialogData", 0f);
	}

	private void InitView()
	{
		dialogueRoot.SetActive(true);
		dialogueGameObj.SetActive(false);
		dialogueMessage.Text = "";
		npcLeft2D.transform.parent.gameObject.SetActive(false);
		npcRight2D.transform.parent.gameObject.SetActive(false);
		top.gameObject.SetActive(false);
		dialogueLeftName.Hide(true);
		dialogueRightName.Hide(true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void SetDialogData()
	{
		DialogData tempDialog = new DialogData();
		tempDialog.dialogId = dialogId;
		tempDialog.onHideDelegate = onHideDelegate;
		dialogData = tempDialog;
	}

	private void Update()
	{
		DialogueConfig.Dialogues dialog = dialogData == null ? null : dialogData.GetDialog();

		if (dialog == null)
			return;

		//DialogueConfig.Dialogues dialogPlayer = new DialogueConfig.Dialogues();

		if (dialogueGameObj.activeSelf == false)
		{
			dialogueGameObj.SetActive(true);
		}

		// Set the Portrait Icon.
		SetPortrait(dialog);

		// Set the Portrait Name.
		SetPortraitName(dialog);

		if (anchor.activeSelf != (dialogData.delta < 0))
			anchor.SetActive(dialogData.delta < 0);

		if (dialogData.delta >= 0)
		{
			dialogData.delta += Time.deltaTime;

			if (dialogData.delta >= dialogueSpeed)
			{
				dialogData.delta = 0;
				string dialogValue = dialog.dialogueValue;
				if (dialogData.messageStep < dialogValue.Length)
				{
					dialogueMessage.Text = dialogValue.Substring(0, dialogData.messageStep + 1);
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

	private void SetPortrait(DialogueConfig.Dialogues dialoguesCfg)
	{
		if (dialoguesCfg == null)
			return;

		if (dialoguesCfg.positionType == DialogueConfig._PositionType.Unknown)
		{
			npcLeft2D.CachedTransform.parent.gameObject.SetActive(false);
			npcRight2D.CachedTransform.parent.gameObject.SetActive(false);
			return;
		}

		GameObject showPortRoot = dialoguesCfg.positionType == DialogueConfig._PositionType.Left ? npcLeft2D.CachedTransform.parent.gameObject : npcRight2D.CachedTransform.parent.gameObject;
		GameObject hidePortRoot = dialoguesCfg.positionType == DialogueConfig._PositionType.Left ? npcRight2D.CachedTransform.parent.gameObject : npcLeft2D.CachedTransform.parent.gameObject;

		if (hidePortRoot.activeInHierarchy)
			hidePortRoot.SetActive(false);

		if (showPortRoot.activeInHierarchy)
		{
			if (dialogData.cacheDialogue != null && dialogData.cacheDialogue.dialogueIndex == dialoguesCfg.dialogueIndex)
				return;
		}
		else
			showPortRoot.SetActive(true);

		dialogData.cacheDialogue = dialoguesCfg;

		UIElemAssetIcon portraitIcon = null;
		UIBtnCamera portrait3D = null;
		GameObject portait3dMaker = null;

		if (dialoguesCfg.positionType == DialogueConfig._PositionType.Left)
		{
			portraitIcon = npcLeft2D;
			portrait3D = npcLeft3D;
			portait3dMaker = npcLeft3dMaker;
		}
		else if (dialoguesCfg.positionType == DialogueConfig._PositionType.Right)
		{
			portraitIcon = npcRight2D;
			portrait3D = npcRight3D;
			portait3dMaker = npcRight3dMaker;
		}

		// Set portrait.
		switch (dialoguesCfg.portraitDisplayType)
		{
			case DialogueConfig._PortraitDisplayType.Portrait2D:
				{
					portrait3D.gameObject.SetActive(false);
					portraitIcon.Hide(false);

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
								Debug.LogError("Invalid portraitType : " + dialoguesCfg.portraitType + " Please modify DialogueConfig.xml ,DialogueSetId = " + dialogId.ToString("X"));
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
							avatarId = dialoguesCfg.avatarId;
							//烽火狼烟读取玩家模型
							if (SysGameStateMachine.Instance.CurrentState is GameState_WolfSmoke)
								avatarId = dialogEnemyId;

							break;
						case DialogueConfig._PortraitType.Npc:
							// Find NPC in combat record.
							// Use avatar id of the NPC
							if (SysGameStateMachine.Instance.CurrentState is GameState_Battle)
							{
								GameState_Battle battle = SysGameStateMachine.Instance.CurrentState as GameState_Battle;

								List<KodGames.ClientClass.AvatarResult> avatarResults = battle.CombatResultAndReward.BattleRecords[dialoguesCfg.npcStageIndex].TeamRecords[dialoguesCfg.npcTeamIndex].AvatarResults;

								if (dialoguesCfg.npcId == IDSeg.InvalidId)
								{
									for (int index = 0; index < avatarResults.Count; index++)
									{
										if (avatarResults[index].CombatAvatarData.BattlePosition == dialoguesCfg.npcIndex)
										{
											avatarId = avatarResults[index].CombatAvatarData.ResourceId;
											break;
										}
									}
								}
								else
								{
									for (int index = 0; index < avatarResults.Count; index++)
									{
										if (avatarResults[index].CombatAvatarData.NpcId == dialoguesCfg.npcId)
										{
											avatarId = avatarResults[index].CombatAvatarData.ResourceId;
											break;
										}
									}
								}

								if (avatarId == IDSeg.InvalidId)
								{
									avatarResults.Sort(
										(a1, a2) =>
										{
											return a1.CombatAvatarData.BattlePosition - a2.CombatAvatarData.BattlePosition;
										});
									avatarId = avatarResults[0].CombatAvatarData.ResourceId;
								}
							}
							else
								Debug.LogError("Current State is not GameState_Battle.");
							break;
						default:
							Debug.LogWarning("Invalid portraitType : " + dialoguesCfg.portraitType);
							break;
					}

					for (int index = 0; index < portait3dMaker.transform.childCount; index++)
						portait3dMaker.transform.GetChild(index).gameObject.SetActive(false);

					// Find avatar in cached by avatar id
					// If not exist create a new one and add to cache							
					Avatar avatar = null;
					if (avatarCaches.ContainsKey(avatarId))
						avatar = avatarCaches[avatarId];
					else if (avatarId != IDSeg.InvalidId)
					{
						avatar = Avatar.CreateAvatar(avatarId);
						avatarCaches.Add(avatarId, avatar);

						int avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId).GetAvatarAssetId(0);
						if (avatar.Load(avatarAssetId) == false)
							return;

						// Set layer
						avatar.SetGameObjectLayer(GameDefines.AvatarCaptureLayer);

						ParticleSystem[] ps = avatar.GetComponentsInChildren<ParticleSystem>();
						foreach (var psTemp in ps)
							Destroy(psTemp.gameObject);
					}
					else
					{
						Debug.Log("AvatarId is InvalidId");
						return;
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

		Animation portaitAnim = null;
		if (portraitIcon != null && portraitIcon.IsHidden() == false)
			portaitAnim = portraitIcon.CachedAnimation;
		else if (portrait3D.gameObject.activeInHierarchy)
			portaitAnim = portrait3D.CachedAnimation;

		if (portaitAnim != null)
		{
			portaitAnim.Play();
		}
	}

	private void SetPortraitName(DialogueConfig.Dialogues dialoguesCfg)
	{
		if (dialoguesCfg == null)
			return;

		string portaitName = "";

		if (string.IsNullOrEmpty(dialoguesCfg.portraitName))
		{
			switch (dialoguesCfg.portraitType)
			{
				case DialogueConfig._PortraitType.Player:
					{
						// Use player name
						portaitName = SysLocalDataBase.Inst.LocalPlayer.Name;
					}

					break;
				case DialogueConfig._PortraitType.PlayerAssistant:
					{
						int listId = dialoguesCfg.portraitIconIndex < ConfigDatabase.DefaultCfg.DialogueConfig.playerAssistantPortraitIcons.Count ? dialoguesCfg.portraitIconIndex : 0;
						portaitName = ItemInfoUtility.GetAssetName(ConfigDatabase.DefaultCfg.DialogueConfig.playerAssistantPortraitIcons[listId]);
					}
					break;
				case DialogueConfig._PortraitType.Avatar:
					if (dialoguesCfg.avatarId != IDSeg.InvalidId)
						portaitName = ItemInfoUtility.GetAssetName(dialoguesCfg.avatarId);
					else
						portaitName = this.avatarName;
					// Use avatar name 
					break;
				case DialogueConfig._PortraitType.Npc:
					// Use NPC name in combat record
					if (SysGameStateMachine.Instance.CurrentState is GameState_Battle)
					{
						GameState_Battle battle = SysGameStateMachine.Instance.CurrentState as GameState_Battle;

						List<KodGames.ClientClass.AvatarResult> avatarResults = battle.CombatResultAndReward.BattleRecords[dialoguesCfg.npcStageIndex].TeamRecords[dialoguesCfg.npcTeamIndex].AvatarResults;

						if (dialoguesCfg.npcId == IDSeg.InvalidId)
						{
							for (int index = 0; index < avatarResults.Count; index++)
							{
								if (avatarResults[index].CombatAvatarData.BattlePosition == dialoguesCfg.npcIndex)
								{
									portaitName = avatarResults[index].CombatAvatarData.DisplayName;
									break;
								}
							}
						}
						else
						{
							for (int index = 0; index < avatarResults.Count; index++)
							{
								if (avatarResults[index].CombatAvatarData.NpcId == dialoguesCfg.npcId)
								{
									portaitName = avatarResults[index].CombatAvatarData.DisplayName;
									break;
								}
							}
						}

						if (portaitName.Equals(""))
						{
							avatarResults.Sort(
								(a1, a2) =>
								{
									return a1.CombatAvatarData.BattlePosition - a2.CombatAvatarData.BattlePosition;
								});
							portaitName = avatarResults[0].CombatAvatarData.DisplayName;
						}
					}

					break;
				default:
					Debug.LogWarning("Invalid portraitType : " + dialoguesCfg.portraitType);
					break;
			}
		}
		else
			portaitName = dialoguesCfg.portraitName;

		// Set Name.
		switch (dialoguesCfg.positionType)
		{
			case DialogueConfig._PositionType.Right:
				dialogueLeftName.Hide(true);
				dialogueRightName.Hide(false);
				dialogueRightName.Text = portaitName;
				break;

			default:
				dialogueLeftName.Hide(false);
				dialogueRightName.Hide(true);
				dialogueLeftName.Text = portaitName;
				break;
		}

		// Show Camp Type Label.
		bool combatDialogue = ConfigDatabase.DefaultCfg.DialogueConfig.GetDialogueSet(dialogId).combatDialogue;
		string campLabel = DialogueConfig._CampType.GetDisplayNameByType(dialoguesCfg.dialogueCampType, ConfigDatabase.DefaultCfg);
		leftCampLabel.Text = combatDialogue ? (dialoguesCfg.positionType != DialogueConfig._PositionType.Right ? campLabel : string.Empty) : string.Empty;
		rightCampLabel.Text = combatDialogue ? (dialoguesCfg.positionType == DialogueConfig._PositionType.Right ? campLabel : string.Empty) : string.Empty;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnShowMessage(UIButton btn)
	{
		if (dialogData == null)
			return;

		if (dialogData.delta >= 0)
		{
			// Show All Message of current step.
			dialogData.delta = -1;
			dialogData.messageStep = 0;
			dialogueMessage.Text = dialogData.GetDialog().dialogueValue;
		}
		else
		{
			// Show Next Step.
			if (dialogData.HasNext())
			{
				dialogData.Next();
				dialogueMessage.Text = "";
			}
			else
			{
				if (dialogData.onHideDelegate != null)
					dialogData.onHideDelegate();

				HideSelf();
			}

		}
	}
}
