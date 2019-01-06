//#define SYS_TUTORIAL_ENABLE_LOG
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class SysTutorial : SysModule
{
	public class TutorialStepData
	{
		public TutorialConfig.Step stepConfig;
		public bool active = false;
		public long activeTime = 0;
	}

	public class TutorialData
	{
		public TutorialConfig.Tutorial tutorialConfig;
		public TutorialStepData stepData;
		public int stepIndex;

		public bool finished = false;

		public bool HasNotStarted()
		{
			return finished == false && (stepData == null || stepData.active == false);
		}
	}

	//public bool TutorialHasStarted
	//{
	//    get
	//    {
	//        if (finishedNoviceQuestList.Count != 0)
	//            return true;

	//        foreach (var data in unfinishedTutorialDatas)
	//            if (data.HasNotStarted() == false)
	//                return true;

	//        return false;
	//    }
	//}

	private Dictionary<string, GameObject> cachedTagObjects = new Dictionary<string, GameObject>();

	private List<TutorialData> unfinishedTutorialDatas = new List<TutorialData>();
	private List<int> finishedTutorialIds = new List<int>();

	public static SysTutorial Instance { get { return SysModuleManager.Instance.GetSysModule<SysTutorial>(); } }

	private IUIObject changedUIObject;

	private bool pause;
	public bool Pause
	{
		get { return pause; }
		set
		{
			pause = value;

			if (pause)
			{
				SysUIEnv uiEnv = SysUIEnv.Instance;

				// Unlock UI
				uiEnv.UnlockUIInput();

				// Hide HelpTips 
				uiEnv.HideUIModule(typeof(UITipHelp));
			}
		}
	}
	private bool broken = false;
	public void Broken()
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UITipHelp)))
			SysUIEnv.Instance.HideUIModule(typeof(UITipHelp));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UITipDragHelp)))
			SysUIEnv.Instance.HideUIModule(typeof(UITipDragHelp));

		broken = true;
	}

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		unfinishedTutorialDatas.Clear();
		finishedTutorialIds.Clear();
		changedUIObject = null;
		return true;
	}

	public override void OnUpdate()
	{
		if (broken || pause)
			return;

		UpdateTutorial();
	}

	public void UpdateTutorial()
	{
		UpdateAllQuest(changedUIObject);
	}

	public bool CombatEndStep()
	{
		var player = SysLocalDataBase.Inst.LocalPlayer;
		return player.UnDoneTutorials.Count > 0 && player.UnDoneTutorials.Contains(ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.combatEndId);
	}

	public void SetQuestList(List<int> unFinishedQuestIds)
	{
		for (int i = 0; i < unFinishedQuestIds.Count; ++i)
		{
			TutorialConfig.Tutorial tutorial = ConfigDatabase.DefaultCfg.TutorialConfig.GetTutorialById(unFinishedQuestIds[i]);

			if (tutorial == null)
				continue;

			// New Tutorial Data.
			TutorialData tutorialData = new TutorialData();
			tutorialData.tutorialConfig = tutorial;

			// Set step Data.
			tutorialData.stepData = new TutorialStepData();
			tutorialData.stepIndex = 0;

			if (tutorialData.stepIndex >= tutorial.steps.Count)
				continue;

			tutorialData.stepData.stepConfig = tutorial.steps[tutorialData.stepIndex];

			unfinishedTutorialDatas.Add(tutorialData);
		}
	}

	private void UpdateAllQuest(IUIObject changedUIObject)
	{

		if (unfinishedTutorialDatas.Count <= 0)
			return;

		// Remove finished quest 
		for (int i = 0; i < unfinishedTutorialDatas.Count; ++i)
		{
			TutorialData questData = unfinishedTutorialDatas[i];
			if (questData.finished)
			{
				unfinishedTutorialDatas.RemoveAt(i);
				--i;
			}
		}

		// Update actived quest 
		for (int i = 0; i < unfinishedTutorialDatas.Count; ++i)
		{
			var tutorialData = unfinishedTutorialDatas[i];
			if (tutorialData.finished == false && tutorialData.stepData.active == true)
				UpdateQuest(tutorialData, changedUIObject);
		}

		// Update novice quest 
		for (int i = 0; i < unfinishedTutorialDatas.Count; ++i)
		{
			var tutorialData = unfinishedTutorialDatas[i];
			if (tutorialData.finished == false && tutorialData.stepData.active == false)
				UpdateQuest(tutorialData, changedUIObject);
		}
	}

	private void UpdateQuest(TutorialData tutorialData, IUIObject changedUIObject)
	{
		// Check terminal condition first
		bool terminal = false;
		List<TutorialConfig.Condition> teriamlCondtions = tutorialData.stepData.stepConfig.GetConditionByType(TutorialConfig._Phase.Terminal);
		if (teriamlCondtions != null)
		{
			for (int i = 0; i < teriamlCondtions.Count; ++i)
			{
				var conditionCfg = teriamlCondtions[i];
				terminal = true;
				if (CheckCondition(conditionCfg, tutorialData.stepData, changedUIObject) == false)
				{
					terminal = false;
					break;
				}
			}
		}

		// If meet all terminal condition , finish the quest
		if (terminal)
		{
			TerminalStep(tutorialData);
			return;
		}

		// If inactive, check active condition
		if (tutorialData.stepData.active == false)
		{
			List<TutorialConfig.Condition> activeConditions = tutorialData.stepData.stepConfig.GetConditionByType(TutorialConfig._Phase.Active);
			if (activeConditions != null)
			{
				for (int i = 0; i < activeConditions.Count; ++i)
				{
					var conditionCfg = activeConditions[i];
					if (CheckCondition(conditionCfg, tutorialData.stepData, changedUIObject) == false)
					{
						// Check if can skip this step
						if (tutorialData.stepData.stepConfig.skipWhenInaction)
							Go2NextSetp(tutorialData);

						return;
					}
				}
			}

			// Meet condition (AND operator)
			ActiveStep(tutorialData.stepData);

			// Talking Game Tutorial Begin.
			GameAnalyticsUtility.OnTutorialBegin(tutorialData);
		}

		// For active quest, check finish condition
		if (tutorialData.stepData.active)
		{
			bool finished = true;
			List<TutorialConfig.Condition> finishCondition = tutorialData.stepData.stepConfig.GetConditionByType(TutorialConfig._Phase.Finish);
			if (finishCondition != null)
			{
				for (int i = 0; i < finishCondition.Count; ++i)
				{
					var conditionCfg = finishCondition[i];
					if (CheckCondition(conditionCfg, tutorialData.stepData, changedUIObject) == false)
					{
						finished = false;
						break;
					}
				}
			}

			if (finished == true)
			{
				// Meet condition (OR operator)
				changedUIObject = null;
				FinishStep(tutorialData);
			}
		}
	}

	private bool CheckCondition(TutorialConfig.Condition conditionConfig, TutorialStepData stepData, IUIObject changeUIObject)
	{
		switch (conditionConfig.type)
		{
			case TutorialConfig._ConditionType.PressControl:
				IUIObject uiObj = GetUIObjectWithTag(conditionConfig.strValue, conditionConfig.intValue - 1, conditionConfig.extraValue);
				RegisterListenUIChangedDelegate(uiObj);
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("Check Condition PressControl " + conditionConfig.strValue + " " + (changeUIObject != null && changeUIObject == uiObj));
#endif
				return changeUIObject != null && changeUIObject == uiObj;

			case TutorialConfig._ConditionType.PressControlByQualityLevel:
				IUIObject Obj = GetUIObjectWithTag(conditionConfig.strValue, conditionConfig.intValue, conditionConfig.extraValue);
				RegisterListenUIChangedDelegate(Obj);
				return changeUIObject != null && changeUIObject == Obj;

			case TutorialConfig._ConditionType.MoveIcon:

				if (!SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("DragObjectTag") || !SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("DragTargetTag"))
					return false;

				string objectTag = SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("DragObjectTag") as string;
				string objectTargetTag = SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("DragTargetTag") as string;


				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, conditionConfig.strValue.Equals(objectTag) && conditionConfig.iconStrValue.Equals(objectTargetTag));

			case TutorialConfig._ConditionType.UIShown:
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("Check Condition UiShow " + ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, IsUIPanelShow(conditionConfig.intValue)));
#endif
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, IsUIModuelShow(conditionConfig.intValue));

			case TutorialConfig._ConditionType.UIButtonEnable:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, IsUIButtonWithTagEnable(conditionConfig.strValue, conditionConfig.intValue - 1, conditionConfig.extraValue));

			case TutorialConfig._ConditionType.UIButtonExist:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, IsUIButtonExist(conditionConfig.strValue, conditionConfig.intValue - 1, conditionConfig.extraValue));

			case TutorialConfig._ConditionType.QuestFinished:
				TutorialConfig.Tutorial tutorial = ConfigDatabase.DefaultCfg.TutorialConfig.GetTutorialById(conditionConfig.intValue);
				if (tutorial == null)
					break;

				bool finished = IsNoviceQuestFinished(conditionConfig.intValue); ;
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("Check QuestFinished " + conditionConfig.intValue.ToString("X") + finished + " " + ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, finished));
#endif
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, finished);

			case TutorialConfig._ConditionType.QuestCurrentStepActived:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, stepData.active);

			case TutorialConfig._ConditionType.HasEquipmentOnAvatar:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, HasEquipmentOnAvatar(conditionConfig.intValue));

			case TutorialConfig._ConditionType.HasSkillOnAvatar:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, HasSkillOnAvatar());

			case TutorialConfig._ConditionType.PlayerLevel:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);

			case TutorialConfig._ConditionType.CurrentState:
				return SysGameStateMachine.Instance.CurrentState.GetType().Name.Equals(conditionConfig.strValue);

			case TutorialConfig._ConditionType.UIObjActive:
				GameObject go = GetObjectWithTag(conditionConfig.strValue);
				if (go == null)
					return false;

				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, go.activeInHierarchy);

			case TutorialConfig._ConditionType.UIScrollListItemCount:
				GameObject scrollObj = GetObjectWithTag(conditionConfig.strValue);

				if (scrollObj == null)
				{
#if SYS_TUTORIAL_ENABLE_LOG
					Debug.Log("Could Not Found ScrollList with tag " + conditionConfig.strValue);
#endif
				}
				else
				{
					UIScrollList scrollList = scrollObj.GetComponent<UIScrollList>();
					return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, scrollList.Count);
				}

				return false;

			case TutorialConfig._ConditionType.DailyGuid:
				bool hasDailyQuestMatch = SysLocalDataBase.Inst.LocalPlayer.QuestData.Quests != null && SysLocalDataBase.Inst.LocalPlayer.QuestData.Quests.Count > 0;

				if (hasDailyQuestMatch)
				{
					KodGames.ClientClass.Quest quest = SysLocalDataBase.Inst.LocalPlayer.QuestData.GetQuestByQuestID(conditionConfig.intValue);

					if (quest != null)
						hasDailyQuestMatch = (quest.Phase == conditionConfig.extraIntValue);
					else
						hasDailyQuestMatch = false;
				}

				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, hasDailyQuestMatch);

			case TutorialConfig._ConditionType.CampaignZoneState:
				int zoneId = conditionConfig.extraIntValue;

				if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(zoneId) || SysLocalDataBase.Inst.LocalPlayer.CampaignData == null)
					return false;
				else
				{
					KodGames.ClientClass.Zone zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(zoneId);

					return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, zoneRecord == null ? _ZoneStatus.UnlockAnimation : zoneRecord.Status);
				}

			case TutorialConfig._ConditionType.CheckClientDynamicValue:
				int clientValue = SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue(conditionConfig.strValue) ? (int)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue(conditionConfig.strValue) : 0;
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, clientValue);

			case TutorialConfig._ConditionType.CheckActivityState:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, ActivityManager.Instance.IsActivityAccessiable(conditionConfig.intValue));

			case TutorialConfig._ConditionType.ButtonLocked:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, IsButtonLocked(conditionConfig.strValue));

			case TutorialConfig._ConditionType.StarReward:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, StarReward(conditionConfig));

			case TutorialConfig._ConditionType.ContentQualityLevel:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, PlayerDataUtility.IsAvatarsQualityLevelContent(SysLocalDataBase.Inst.LocalPlayer, conditionConfig.intValue));

			case TutorialConfig._ConditionType.ScrollListItemData:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, ScrollListItemData(conditionConfig));

			case TutorialConfig._ConditionType.IsOpenHardDifficulity:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, IsOpenHardDiff(conditionConfig.intValue));

			case TutorialConfig._ConditionType.HasStartServerReward:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, HasStartServerReward(conditionConfig.strValue));

			case TutorialConfig._ConditionType.IsAssistant:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, GetAssistantPressControl(conditionConfig.strValue));

			case TutorialConfig._ConditionType.IsCameraMove:
				if (SysGameStateMachine.Instance.GetCurrentState() is GameState_CentralCity)
					return !KodGames.Camera.main.GetComponent<CentralCityCameraController>().IsAutoRotating;
				else
					return false;
			case TutorialConfig._ConditionType.SearchConsumable:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, SearchConsumble(conditionConfig.intValue));

			case TutorialConfig._ConditionType.PressControlQualityLevel:
				conditionConfig.intValue = GetIndexByQualityLevel(4, conditionConfig.strValue);
				IUIObject obj = GetUIObjectWithTag(conditionConfig.strValue, conditionConfig.intValue, conditionConfig.extraValue);
				RegisterListenUIChangedDelegate(obj);
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("Check Condition PressControl " + conditionConfig.strValue + " " + (changeUIObject != null && changeUIObject == obj));
#endif
				return changeUIObject != null && changeUIObject == obj;

			case TutorialConfig._ConditionType.GameMoneyEnough:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, IsGameMoneyEnough(conditionConfig.intValue));

			case TutorialConfig._ConditionType.HasAssistantUI:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, SysAssistant.Instance.HasUIGuid() || SysUIEnv.Instance.IsUIModuleShown(_UIType.UITipAssistant));
			case TutorialConfig._ConditionType.CameraIsScrolling:
				if (!SysGameStateMachine.Instance.CurrentState.GetType().Name.Equals(conditionConfig.strValue))
					return false;
				else
				{
					UIScroller scroller = KodGames.Camera.main.gameObject.GetComponentInChildren<UIScroller>();

					if (scroller != null)
						return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, scroller.IsScrolling);
				}

				return false;
			case TutorialConfig._ConditionType.AnimationIsPlaying:
				GameObject UIObj = GameObject.FindGameObjectWithTag(conditionConfig.strValue);
				if (UIObj == null)
					return false;

				Animation animation = UIObj.GetComponent<Animation>();
				if (animation == null)
					return false;

				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, string.IsNullOrEmpty(conditionConfig.iconStrValue) ? animation.isPlaying : animation.IsPlaying(conditionConfig.iconStrValue));

			case TutorialConfig._ConditionType.BreakThroughConditon:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, BreakThroughConditon(conditionConfig.intValue));

			case TutorialConfig._ConditionType.IsClose:
				return SysLocalDataBase.Inst.LocalPlayer.Function.ShowStartGameVideo == conditionConfig.boolValue;

			case TutorialConfig._ConditionType.LastDungeonId:
				if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignBattleResult))
					return SysUIEnv.Instance.GetUIModule<UIPnlCampaignBattleResult>().GetDungeonId() == conditionConfig.intValue
						&& SysUIEnv.Instance.GetUIModule<UIPnlCampaignBattleResult>().GetResult();

				return false;

			case TutorialConfig._ConditionType.HasEquipOnAvatar:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, PlayerDataUtility.IsLineUpEquipOnAvatar(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, conditionConfig.levelValue, conditionConfig.intValue));

			case TutorialConfig._ConditionType.PressControlByData:
				int index = 0;
				GameObject obj2 = null;
				SysUIEnv.Instance.FindControlInListByTag(conditionConfig.strValue, -1, conditionConfig.intValue, ref obj2);
				index = (obj2.GetComponent<AutoSpriteControlBase>().Container as UIListItemContainer).Index;
				uiObj = GetUIObjectWithTag(conditionConfig.strValue, index, conditionConfig.extraValue);

				RegisterListenUIChangedDelegate(uiObj);

				return changeUIObject != null && changeUIObject == uiObj;
			case TutorialConfig._ConditionType.QualityLevelAvatarsCount:
				return PlayerDataUtility.QualityLevelAvatarsCount(SysLocalDataBase.Inst.LocalPlayer, conditionConfig.intValue) > 1;
			case TutorialConfig._ConditionType.SearchAvatar:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, SearchAvatar(conditionConfig.intValue));
			case TutorialConfig._ConditionType.TutorialOpen:
				return !conditionConfig.boolValue;
			case TutorialConfig._ConditionType.HasEquipmentOrAvatar:
				return ConfigDatabase.DefaultCfg.TutorialConfig.CompareConditionValue(conditionConfig, HasEquipmentOrAvatar(conditionConfig.intValue));
		}

		return false;
	}

	private void UnregisterConditionDelegate(TutorialStepData stepData, int phaseType)
	{
		List<TutorialConfig.Condition> conditions = stepData.stepConfig.GetConditionByType(phaseType);
		if (conditions != null)
		{
			for (int i = 0; i < conditions.Count; ++i)
			{
				var conditionCfg = conditions[i];
				switch (conditionCfg.type)
				{
					case TutorialConfig._ConditionType.PressControl:
						UnRegisterListenUIChangedDelegate(GetUIObjectWithTag(conditionCfg.strValue, conditionCfg.intValue - 1, conditionCfg.extraValue));
						break;
				}
			}
		}
	}

	private void ActiveStep(TutorialStepData stepData)
	{
#if SYS_TUTORIAL_ENABLE_LOG
		Debug.Log("ActiveStep");
#endif
		stepData.active = true;
		stepData.activeTime = (long)(KodGames.TimeEx.realtimeSinceStartup * 1000);

		// Do action
		List<TutorialConfig.Action> actions = stepData.stepConfig.GetActionByType(TutorialConfig._Phase.Active);
		if (actions != null)
			for (int i = 0; i < actions.Count; ++i)
				DoAction(actions[i]);

		// Unregister active condition delegate
		UnregisterConditionDelegate(stepData, TutorialConfig._Phase.Active);
	}

	private void FinishStep(TutorialStepData stepData)
	{
		// Unregister active condition delegate
		UnregisterConditionDelegate(stepData, TutorialConfig._Phase.Finish);

		List<TutorialConfig.Action> actions = stepData.stepConfig.GetActionByType(TutorialConfig._Phase.Finish);
		if (actions != null)
			for (int i = 0; i < actions.Count; ++i)
				DoAction(actions[i]);

		stepData.active = false;
		stepData.activeTime = 0;
	}

	private void FinishStep(TutorialData questData)
	{
		FinishStep(questData.stepData);
		Go2NextSetp(questData);
	}

	private void TerminalStep(TutorialStepData stepData)
	{
		// Unregister active condition delegate
		UnregisterConditionDelegate(stepData, TutorialConfig._Phase.Finish);

		List<TutorialConfig.Action> actions = stepData.stepConfig.GetActionByType(TutorialConfig._Phase.Terminal);
		if (actions != null)
			for (int i = 0; i < actions.Count; ++i)
				DoAction(actions[i]);

		stepData.active = false;
		stepData.activeTime = 0;
	}

	private void TerminalStep(TutorialData questData)
	{
		TerminalStep(questData.stepData);
		TerminalQuest(questData);
	}

	private void TerminalQuest(TutorialData questData)
	{
		// Quest finished
		questData.finished = true;
	}

	private void Go2NextSetp(TutorialData tutorialData)
	{
		// Set next step
		tutorialData.stepIndex++;
		if (tutorialData.stepIndex < tutorialData.tutorialConfig.steps.Count)
		{
			tutorialData.stepData.stepConfig = tutorialData.tutorialConfig.steps[tutorialData.stepIndex];
			tutorialData.stepData.active = false;
			tutorialData.stepData.activeTime = 0;
		}
		else
		{
			// Quest finished
			tutorialData.finished = true;
		}
	}

	private void DoAction(TutorialConfig.Action action)
	{
		switch (action.type)
		{
			case TutorialConfig._ActionType.ShowPanel:
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("DoAction ShowPanel " + action.intValue);
#endif
				bool showPanel = action.boolValue;
				if (showPanel)
				{
					if (action.buttonData > 0)
						SysUIEnv.Instance.ShowUIModule(action.intValue, action.buttonData);
					else
						SysUIEnv.Instance.ShowUIModule(action.intValue);
				}
				else if (SysUIEnv.Instance.IsUIModuleShown(action.intValue))
					SysUIEnv.Instance.HideUIModule(action.intValue);

				break;

			case TutorialConfig._ActionType.ShowTip:
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("DoAction ShowTip");
#endif
				if (action.boolValue)
				{
					if (!SysUIEnv.Instance.IsUIModuleShown(_UIType.UITipHelp))
						SysUIEnv.Instance.GetUIModule<UITipHelp>().ShowHelp(action);
				}
				else
					SysUIEnv.Instance.HideUIModule(typeof(UITipHelp));
				break;

			case TutorialConfig._ActionType.ShowAdviserTip:
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("Action : ShowAdviserTip");
#endif
				if (action.boolValue)
					SysUIEnv.Instance.GetUIModule<UITipAdviser>().ShowDialogue(action.intValue, null);
				else
					SysUIEnv.Instance.HideUIModule(typeof(UITipAdviser));
				break;

			case TutorialConfig._ActionType.ChangeGameState:
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("Action : changeGameState");
#endif

				string gameStateName = action.strValue;

				if (gameStateName.Equals(typeof(GameState_SelectPlayerAvatar).ToString()))
					SysGameStateMachine.Instance.EnterState<GameState_SelectPlayerAvatar>();
				else if (gameStateName.Equals(typeof(GameState_CentralCity).ToString()))
					SysGameStateMachine.Instance.EnterState<GameState_CentralCity>();
				else if (gameStateName.Equals(typeof(GameState_GameMovie).ToString()))
					SysGameStateMachine.Instance.EnterState<GameState_GameMovie>();
				break;
			case TutorialConfig._ActionType.LockUIInput:
				if (action.boolValue)
				{
					if (action.buttonData <= 0)
						SysUIEnv.Instance.LockUIInput(action.strValue, action.iconStrValue, action.intValue - 1, -1, action.isSkillOrEquip, action.componentType);
					else
						SysUIEnv.Instance.LockUIInput(action.strValue, action.iconStrValue, -1, action.buttonData, action.isSkillOrEquip, action.componentType);
				}
				else
					SysUIEnv.Instance.UnlockUIInput();

				break;

			case TutorialConfig._ActionType.MarkQuestFinished:
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("Action : MakeQuestFinish " + action.boolValue);
#endif
				MarkQuestFinished(action.intValue, action.boolValue);
				break;

			case TutorialConfig._ActionType.ScrollToItem:

				GameObject scrollObj = GetObjectWithTag(action.strValue);

				if (scrollObj == null)
					Debug.Log("Could Not Found ScrollList with tag " + action.strValue);
				else
				{
					UIScrollList scrollList = scrollObj.GetComponent<UIScrollList>();

					if (scrollList == null)
						break;

					if (action.buttonData > 0)
					{
						//scrollList.InsertItem(scrollList.GetItem(GetListIndexByData(action)), 0);
						scrollList.ScrollToItem(GetListIndexByData(action), 0f);
					}
					else
						scrollList.ScrollToItem(action.intValue - 1, 0f);
				}

				break;

			case TutorialConfig._ActionType.SetClientDynamicValue:

				SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue(action.strValue, action.intValue);

				break;

			case TutorialConfig._ActionType.RemoveClientDynamicValue:

				SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.RemoveDynamicValue(action.strValue);

				break;

			case TutorialConfig._ActionType.ScrollListEnableTouch:

				GameObject scrollListObj = GetObjectWithTag(action.strValue);

				if (scrollListObj == null)
					Debug.Log("Could Not Found ScrollList with tag " + action.strValue);
				else
				{
					UIScrollList list = scrollListObj.GetComponent<UIScrollList>();

					if (list != null)
						list.touchScroll = action.boolValue;
					else
					{
						UIScroller scroll = scrollListObj.GetComponent<UIScroller>();

						if (scroll != null)
							scroll.TouchScroll = action.boolValue;
					}
				}

				break;

			case TutorialConfig._ActionType.ShowDlgMessageForShowUI:

				MainMenuItem cancelItem = new MainMenuItem();
				cancelItem.ControlText = action.dlgMessageCancel;

				MainMenuItem okItem = new MainMenuItem();
				okItem.ControlText = action.dlgMessageOk;
				okItem.Callback = (userData) =>
				{
					SysUIEnv.Instance.ShowUIModule(action.intValue);
					return true;
				};

				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
				showData.SetData(
					action.text,
					action.strValue,
					cancelItem,
					okItem);

				SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIDlgMessage>().ShowDlg(showData);
				break;

			case TutorialConfig._ActionType.AddDragDelegate:
				List<IUIObject> objs = GetUIObjectsWithTag(action.strValue);
				for (int index = 0; index < objs.Count; index++)
					objs[index].AddDragDropDelegate(OnEZDragDropHandler);
				break;

			case TutorialConfig._ActionType.ShowDragAnimation:
				if (action.boolValue)
					SysUIEnv.Instance.GetUIModule<UITipDragHelp>().ShowHelp(action);
				else
					SysUIEnv.Instance.HideUIModule(typeof(UITipDragHelp));
				break;
			case TutorialConfig._ActionType.SceneScrollToItem:
				CampaignSceneData.Instance.ScrollView(action.intValue, 0, false, null);
				break;
			case TutorialConfig._ActionType.LockUIInputByStartServerReward:
				if (!action.boolValue)
					SysUIEnv.Instance.UnlockUIInput();
				else
					LockUIInputByStartServerReward(action);
				break;
			case TutorialConfig._ActionType.ShowTipByStartServerReward:
				if (!action.boolValue)
					SysUIEnv.Instance.HideUIModule(_UIType.UITipHelp);
				else
					ShowTipByStartServerReward(action);
				break;
			case TutorialConfig._ActionType.ScrollToItemByStartServerReward:
				ScrollToItemByStartServerReward(action);
				break;
			case TutorialConfig._ActionType.MoveCamera:
				if (SysGameStateMachine.Instance.GetCurrentState() is GameState_CentralCity)
					KodGames.Camera.main.GetComponent<CentralCityCameraController>().GazeAt(CentralCityCameraController.BuildingGazeTarget.arena);
				break;
			case TutorialConfig._ActionType.ShowTipArena:
				action.intValue = GetArenaIndex(action) + 1;
				SysUIEnv.Instance.GetUIModule<UITipHelp>().ShowHelp(action);
				break;
			case TutorialConfig._ActionType.LockUIInputArena:
				action.intValue = GetArenaIndex(action);
				SysUIEnv.Instance.LockUIInput(action.attachComponentName, "", action.intValue, -1, false, true);
				break;
			case TutorialConfig._ActionType.ScrollToItemArena:
				GameObject Obj = GetObjectWithTag(action.iconStrValue);

				UIScrollList itemList = Obj.GetComponent<UIScrollList>();

				if (itemList == null) return;

				itemList.ScrollToItem(GetArenaIndex(action) + 1, 0f);
				break;
			case TutorialConfig._ActionType.AddParticle:

				GameObject obj = GetObjectWithTag(action.attachComponentName);

				if (obj == null) return;

				GameObject particle = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, action.strValue));

				ObjectUtility.AttachToParentAndResetLocalTrans(obj, particle);

				break;
			case TutorialConfig._ActionType.ShowTipQualityLevel:

				action.intValue = GetIndexByQualityLevel(4, action.attachComponentName) + 1;
				SysUIEnv.Instance.GetUIModule<UITipHelp>().ShowHelp(action);
				break;
			case TutorialConfig._ActionType.LockUIIputQualityLevel:

				action.intValue = GetIndexByQualityLevel(4, action.strValue);
				SysUIEnv.Instance.LockUIInput(action.strValue, "", action.intValue, -1, false, true);

				break;
			case TutorialConfig._ActionType.SetScrollState:

				CampaignSceneData.Instance.SetScrollState(action.boolValue);
				break;

			case TutorialConfig._ActionType.RemoveParticle:

				obj = GetObjectWithTag(action.strValue);

				if (obj == null)
					return;

				GameObject.DestroyObject(obj);

				break;
			case TutorialConfig._ActionType.PlayBattle:

				if (SysLocalDataBase.Inst.LocalPlayer.CampaignData.Zones == null || SysLocalDataBase.Inst.LocalPlayer.CampaignData.Zones.Count <= 0)
				{
					RequestMgr.Inst.Request(new QueryDungeonListReq(() =>
					{
						RequestMgr.Inst.Request(new OnCombatReq(action.intValue, IDSeg.InvalidId, null, 0f));
					}));
				}
				else
					RequestMgr.Inst.Request(new OnCombatReq(action.intValue, IDSeg.InvalidId, null, 0f));
				break;
			case TutorialConfig._ActionType.AddPtrListener:
				AddPtrListener();
				break;
		}
	}

	private GameObject GetObjectWithTag(string tag)
	{
		GameObject go = GameObject.FindWithTag(tag);

		if (go == null)
		{
			if (cachedTagObjects.ContainsKey(tag) == false)
				return null;

			go = cachedTagObjects[tag];

			return go;
		}

		if (cachedTagObjects.ContainsKey(tag) == false)
			cachedTagObjects.Add(tag, go);
		else
			cachedTagObjects[tag] = go;

		return go;
	}

	private List<IUIObject> GetUIObjectsWithTag(string tag)
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag(tag);
		List<IUIObject> uiObj = new List<IUIObject>();

		for (int index = 0; index < go.Length; index++)
		{
			if (cachedTagObjects.ContainsKey(tag) == false)
				cachedTagObjects.Add(tag, go[index]);
			else
				cachedTagObjects[tag] = go[index];

			AutoSpriteControlBase asControl = go[index].GetComponent<AutoSpriteControlBase>();
			if (asControl != null)
			{
				uiObj.Add(asControl);
				break;
			}

			ControlBase control = go[index].GetComponent<ControlBase>();
			if (control != null)
				uiObj.Add(asControl);
		}

		return uiObj;
	}

	private IUIObject GetUIObjectWithTag(string tag, int index, bool isLisItemObj)
	{
		GameObject go = null;

		if (isLisItemObj)
			SysUIEnv.Instance.FindControlInListByTag(tag, index, -1, ref go);
		else
			go = GameObject.FindWithTag(tag);

		if (go == null)
		{
			if (cachedTagObjects.ContainsKey(tag) == false)
				return null;

			go = cachedTagObjects[tag];

			if (go == null)
				return null;
		}

		// Caching this object
		if (cachedTagObjects.ContainsKey(tag) == false)
			cachedTagObjects.Add(tag, go);
		else
			cachedTagObjects[tag] = go;

		AutoSpriteControlBase asControl = go.GetComponent<AutoSpriteControlBase>();
		if (asControl != null)
			return asControl;

		ControlBase control = go.GetComponent<ControlBase>();
		return control;
	}



	#region Condition

	public bool IsNoviceQuestFinished(int id)
	{
		for (int i = 0; i < unfinishedTutorialDatas.Count; ++i)
		{
			var tutorialData = unfinishedTutorialDatas[i];
			if (tutorialData.tutorialConfig.id == id)
				return tutorialData.finished;
		}

		return true;
	}


	private bool ScrollListItemData(TutorialConfig.Condition action)
	{
		GameObject scrollObj = GetObjectWithTag(action.strValue);

		if (scrollObj == null)
		{
			Debug.Log("Could Not Found ScrollList with tag " + action.strValue);
		}
		else
		{
			UIScrollList scrollList = scrollObj.GetComponent<UIScrollList>();
			for (int i = 0; i < scrollList.Count; i++)
			{
				UIListItemContainer container = scrollList.GetItem(i) as UIListItemContainer;
				if (container == null || container.Data == null)
					continue;

				UIElemEquipSelectItem item = container.Data as UIElemEquipSelectItem;
				if (item != null)
				{
					int id = (int)item.selectBtn.indexData;

					if (id == action.intValue)
					{
						return true;
					}
				}
				else
				{
					UIElemSkillItem skillitem = container.Data as UIElemSkillItem;
					int id = (int)skillitem.selectBtn.indexData;

					if (id == action.intValue)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private void UIValueChangedDelegate(IUIObject obj)
	{
#if SYS_TUTORIAL_ENABLE_LOG
		Debug.Log("Value Change Delegate.");
#endif
		//UpdateAllQuest(obj);

		changedUIObject = obj;
	}

	private void RegisterListenUIChangedDelegate(IUIObject uiObj)
	{
		if (uiObj != null)
			uiObj.SetValueChangedDelegate(UIValueChangedDelegate);
	}

	private void UnRegisterListenUIChangedDelegate(IUIObject uiObj)
	{
		if (uiObj != null)
			uiObj.SetValueChangedDelegate(null);
	}

	private bool IsUIButtonExist(string tag, int index, bool isLisItemObj)
	{
		IUIObject go = GetUIObjectWithTag(tag, index, isLisItemObj);

		return go != null;
	}

	private bool IsUIButtonWithTagEnable(string tag, int index, bool isLisItemObj)
	{
		IUIObject go = GetUIObjectWithTag(tag, index, isLisItemObj);
		if (go == null)
			return false;

		//return go.gameObject.activeInHierarchy;

		if (go.gameObject.activeSelf == false)
			return false;

		UIButton btnRoot = go.gameObject.GetComponent<UIButton>();

		if (btnRoot != null)
			return btnRoot.controlIsEnabled;

		UIButton3D btnRoot3D = go.gameObject.GetComponent<UIButton3D>();

		if (btnRoot3D != null)
			return btnRoot3D.controlIsEnabled;

		return false;
	}

	private bool IsButtonLocked(string tag)
	{
		GameObject go = GameObject.FindGameObjectWithTag(tag);
		if (go == null)
			return false;

		return go.transform.childCount <= 0;
	}

	private bool StarReward(TutorialConfig.Condition condition)
	{
		int zoneId = 0;
		int currentDiffcult = 0;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignSceneMid))
		{
			zoneId = SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.ZoneId;
			currentDiffcult = SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.CurrentDiffcultTab;
		}

		if (zoneId != condition.intValue || currentDiffcult != _DungeonDifficulity.Common)
			return false;

		int boxIndex = condition.extraIntValue;
		int sumStar = 0;

		KodGames.ClientClass.Zone zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(zoneId);
		List<int> alreadGetRewardIndexs = new List<int>();

		KodGames.ClientClass.DungeonDifficulty diffRecord = null;
		if (zoneRecord != null)
			diffRecord = zoneRecord.GetDungeonDiffcultyByDiffcultyType(currentDiffcult);

		if (diffRecord != null)
		{
			alreadGetRewardIndexs = diffRecord.BoxPickedIndexs;

			if (diffRecord.Dungeons != null)
				foreach (var dungeon in diffRecord.Dungeons)
					sumStar += dungeon.BestRecord;
		}

		return !alreadGetRewardIndexs.Contains(boxIndex) && ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneId).GetDungeonDifficultyByDifficulty(currentDiffcult).starRewardConditions[boxIndex].requireStarCount <= sumStar;
	}

	private bool IsOpenHardDiff(int zoneId)
	{
		int zoneID = 0;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignSceneMid))
			zoneID = SysUIEnv.Instance.GetUIModule<UIPnlCampaignSceneMid>().dungeonItem.ZoneId;

		if (zoneId != zoneID)
			return false;

		return CampaignData.IsDiffcultComplement(zoneId, _DungeonDifficulity.Common);
	}

	private bool HasEquipmentOnAvatar(int equipmentType)
	{
		var localPlayer = SysLocalDataBase.Inst.LocalPlayer;

		return PlayerDataUtility.IsLineUpEquipmentOnAvatars(localPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, equipmentType);

	}

	private bool HasStartServerReward(string tag)
	{
		List<StartServerRewardConfig.StartServerReward> startServerRewards = ConfigDatabase.DefaultCfg.StartServerRewardConfig.startServerRewards;

		if (SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo.UnPickIds.Count != ConfigDatabase.DefaultCfg.StartServerRewardConfig.startServerRewards.Count)
			return false;

		for (int i = 0; i < startServerRewards.Count; i++)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo.DayCount >= startServerRewards[i].day)
				if (SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo.UnPickIds.Contains(startServerRewards[i].id))
				{
					if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlStartServerReward))
						FindUnPickedRewardIndex(tag);

					return true;
				}
		}
		return false;
	}

	private bool IsUIModuelShow(int uiType)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(uiType))
			return true;
		else
			return SysUIEnv.Instance.ModulePool.GetShowModules(SysUIEnv.Instance.GetClassByType(uiType)).Count > 0;
	}

	private bool GetAssistantPressControl(string tag)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UITipAssistant)))
			if (!SysUIEnv.Instance.GetUIModule<UITipAssistant>().isTutorialObj)
				SysUIEnv.Instance.GetUIModule<UITipAssistant>().IsTutorialObjTag(tag);

		if (SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("PressControl"))
			return (bool)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("PressControl");
		else
			return false;
	}

	private bool HasSkillOnAvatar()
	{
		var localPlayer = SysLocalDataBase.Inst.LocalPlayer;

		return PlayerDataUtility.IsLineUpSkillOnAvatars(localPlayer, ConfigDatabase.DefaultCfg.PositionConfig.Positions[0].Id);
	}

	private bool SearchConsumble(int consumbleId)
	{
		return SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(consumbleId) != null;
	}

	private bool IsGameMoneyEnough(int count)
	{
		return SysLocalDataBase.Inst.LocalPlayer.GameMoney >= count;
	}

	private bool BreakThroughConditon(int avatarId)
	{
		int activePositionId = SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId;
		var player = SysLocalDataBase.Inst.LocalPlayer;
		var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(activePositionId);

		int showLocationId = PlayerDataUtility.GetBattlePosByIndexPos(2);

		KodGames.ClientClass.Avatar avatar = null;

		for (int i = 0; i < position.AvatarLocations.Count; i++)
			if (position.AvatarLocations[i].ShowLocationId == showLocationId && position.AvatarLocations[i].ResourceId == avatarId)
				avatar = PlayerDataUtility.GetLineUpAvatar(player, activePositionId, position.AvatarLocations[i].ShowLocationId);

		if (avatar == null)
			return false;

		if (avatar.BreakthoughtLevel > 0)
			return false;

		AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);
		int currentMaxLevel = avatarConfig.GetAvatarBreakthrough(avatar.BreakthoughtLevel).breakThrough.powerUpLevelLimit;
		if (avatar.LevelAttrib.Level < currentMaxLevel)
			return false;

		return true;
	}

	private bool SearchAvatar(int avatarId)
	{
		var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		for (int i = 0; i < position.AvatarLocations.Count; i++)
			if (position.AvatarLocations[i].ResourceId == avatarId)
				return true;

		return false;
	}

	private bool HasEquipmentOrAvatar(int buttonData)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlSelectEquipmentList))
		{
			UIScrollList scrollList = SysUIEnv.Instance.GetUIModule<UIPnlSelectEquipmentList>().scrollList;
			for (int i = 0; i < scrollList.Count; i++)
			{
				UIElemEquipSelectItem item = scrollList.GetItem(i).Data as UIElemEquipSelectItem;
				if (item != null && buttonData <= 0)
					return true;
				else if (item != null && buttonData > 0)
					if (item.selectBtn.IndexData == buttonData)
						return true;
			}
		}

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlSelectSkillList))
		{
			UIScrollList scrollList = SysUIEnv.Instance.GetUIModule<UIPnlSelectSkillList>().skillList;
			for (int i = 0; i < scrollList.Count; i++)
			{
				UIElemSkillItem item = scrollList.GetItem(i).Data as UIElemSkillItem;
				if (item != null)
					return true;
			}
		}

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlSelectAvatarList))
		{
			UIScrollList scrollList = SysUIEnv.Instance.GetUIModule<UIPnlSelectAvatarList>().avatarList;
			for (int i = 0; i < scrollList.Count; i++)
			{
				UIElemSellAvatarItem item = scrollList.GetItem(i).Data as UIElemSellAvatarItem;
				if (item != null)
					return true;
			}
		}
		return false;
	}



	#endregion

	#region Action

	private void MarkQuestFinished(int id, bool makeLocal)
	{
		if (makeLocal)
		{
			TutorialData markedQuestData = null;
			for (int i = 0; i < unfinishedTutorialDatas.Count; ++i)
			{
				var tutorialData = unfinishedTutorialDatas[i];
				if (tutorialData.tutorialConfig.id == id)
				{
					markedQuestData = tutorialData;
					break;
				}
			}

			if (markedQuestData != null)
			{
#if SYS_TUTORIAL_ENABLE_LOG
				Debug.Log("Make quest finish.");
#endif
				// If has not finished, set finished flag
				markedQuestData.finished = true;
			}
		}

		// Всµг
		if (finishedTutorialIds.Contains(id) == false)
		{
			finishedTutorialIds.Add(id);

			// Record Finish Tutorial.
			SysGameAnalytics.Instance.RecordGameData(GameRecordType.Tutorial, id.ToString("X"));
		}

		// Send request
		if (SysLocalDataBase.Inst.LocalPlayer.UnDoneTutorials.Contains(id))
			RequestMgr.Inst.Request(new CompleteTutorialReq(id));
	}

	private int GetListIndexByData(TutorialConfig.Action action)
	{
		GameObject scrollObj = GetObjectWithTag(action.strValue);

		if (scrollObj == null)
			Debug.Log("Could Not Found ScrollList with tag " + action.strValue);
		else
		{
			UIScrollList scrollList = scrollObj.GetComponent<UIScrollList>();
			for (int i = 0; i < scrollList.Count; i++)
			{
				UIListItemContainer container = scrollList.GetItem(i) as UIListItemContainer;
				if (container == null || container.Data == null)
					continue;

				UIElemEquipSelectItem item = container.Data as UIElemEquipSelectItem;

				if (item != null)
				{
					int id = (int)item.selectBtn.indexData;

					if (id == action.buttonData)
					{
						return i;
					}
				}
				else
				{
					UIElemSkillItem skillitem = container.Data as UIElemSkillItem;
					int id = (int)skillitem.selectBtn.indexData;

					if (id == action.buttonData)
					{
						return i;
					}
				}
			}
		}
		return 0;
	}

	private void OnEZDragDropHandler(EZDragDropParams parms)
	{
		switch (parms.evt)
		{
			case EZDragDropEvent.Dropped:

				if (!string.IsNullOrEmpty(parms.dragObj.gameObject.tag))
					SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue("DragObjectTag", parms.dragObj.gameObject.tag);

				if (parms.dragObj.DropTarget != null && !string.IsNullOrEmpty(parms.dragObj.DropTarget.tag))
					SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue("DragTargetTag", parms.dragObj.DropTarget.tag);

				break;
		}
	}

	private void FindUnPickedRewardIndex(string tag)
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
		for (int i = 0; i < gos.Length; i++)
		{
			AutoSpriteControlBase control = gos[i].gameObject.GetComponent<AutoSpriteControlBase>();
			if (control.controlIsEnabled)
			{
				var container = control.Container as UIListItemContainer;
				SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue("ServerRewardIndex", container.Index);
			}
		}
	}

	private void ScrollToItemByStartServerReward(TutorialConfig.Action action)
	{
		GameObject scrollObj = GetObjectWithTag(action.strValue);

		UIScrollList scrollList = scrollObj.GetComponent<UIScrollList>();

		if (scrollList == null)
			return;

		int index = -1;
		if (SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("ServerRewardIndex"))
			index = (int)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("ServerRewardIndex");

		scrollList.ScrollToItem(index, 0f);

	}

	private void ShowTipByStartServerReward(TutorialConfig.Action action)
	{

		if (SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("ServerRewardIndex"))
			action.intValue = (int)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("ServerRewardIndex") + 1;

		if (!SysUIEnv.Instance.IsUIModuleShown(_UIType.UITipHelp))
			SysUIEnv.Instance.GetUIModule<UITipHelp>().ShowHelp(action);
	}

	private void LockUIInputByStartServerReward(TutorialConfig.Action action)
	{

		if (SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("ServerRewardIndex"))
			action.intValue = (int)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("ServerRewardIndex");

		SysUIEnv.Instance.LockUIInput(action.strValue, action.iconStrValue, action.intValue, -1, action.isSkillOrEquip, action.componentType);
	}

	private int GetIndexByQualityLevel(int qualityLevel, string tag)
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);

		foreach (var go in gos)
		{
			UIElemAssetIcon ascb = go.GetComponent<UIElemAssetIcon>();
			if (ascb == null || ascb.Data == null)
				continue;

			var avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById((ascb.Data as KodGames.ClientClass.Location).ResourceId);
			if (avatarConfig.qualityLevel < qualityLevel)
				continue;

			var avatarRecord = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar((ascb.Data as KodGames.ClientClass.Location).Guid);
			if (avatarRecord.LevelAttrib.Level < avatarConfig.GetAvatarBreakthrough(avatarRecord.BreakthoughtLevel).breakThrough.powerUpLevelLimit)
			{
				AutoSpriteControlBase ascb1 = go.GetComponent<AutoSpriteControlBase>();
				return (ascb1.Container as UIListItemContainer).Index;
			}

		}
		return 0;
	}

	private int GetArenaIndex(TutorialConfig.Action action)
	{
		if (string.IsNullOrEmpty(action.attachComponentName))
			return 0;

		GameObject[] gos = GameObject.FindGameObjectsWithTag(action.attachComponentName);
		if (gos == null || gos.Length <= 0)
			return 0;

		var tempControlObj = gos[0];
		var ascb = tempControlObj.GetComponent<AutoSpriteControlBase>();
		if (ascb == null)
			return 0;


		var item = ascb.Container as UIListItemContainer;

		UIScrollList scrollList = item.GetScrollList();

		for (int i = 0; i < scrollList.Count; i++)
		{
			UIListItemContainer container = scrollList.GetItem(i) as UIListItemContainer;
			if (container == null || container.Data == null)
				continue;

			UIElemArenaChallengeItem changeItem = container.Data as UIElemArenaChallengeItem;
			if (changeItem == null)
				return container.Index - 1;
		}

		return 0;
	}

	private void AddPtrListener()
	{
		UIManager.instance.AddMouseTouchPtrListener(MouseTouchPtrListener);
	}

	protected virtual void MouseTouchPtrListener(POINTER_INFO data)
	{

		if (data.evt == POINTER_INFO.INPUT_EVENT.PRESS)
		{
			GameObject obj = GetObjectWithTag("UIFxGuideHand");

			if (obj == null)
				return;

			GameObject.DestroyObject(obj);

			UIManager.instance.RemoveMouseTouchPtrListener(MouseTouchPtrListener);
		}
	}
	#endregion

}