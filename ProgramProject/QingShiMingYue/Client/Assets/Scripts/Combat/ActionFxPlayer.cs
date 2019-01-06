using UnityEngine;
using KodGames.ClientClass;
using ClientServerCommon;
using System.Collections.Generic;

public class ActionFxPlayer
{
	public void PlayActionFx(BattleRole targetRole, int actionId, bool useActionAnim)
	{
		AvatarAction actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionById(actionId);

		//必须先播放动画
		if (useActionAnim)
			targetRole.PlayAction(actionId);
		else
			targetRole.ChangeState(BattleRole._ActionState.Busy);

		// Set event callback.
		targetRole.Avatar.SetAnimEventCb(ActionEventCallback);

		// Push non-logic event
		foreach (var eventCfg in actionCfg.events)
		{
			if (eventCfg.IsLogicEvent())
				continue;

			//if (eventCfg.eventType != AvatarAction.Event._Type.PlayEffect)
			//    continue;

			// Construct event record to process
			EventRecord eventRecord = new EventRecord();
			eventRecord.EventIndex = eventCfg.index;

			EventTargetRecord eventTargetRecord = new EventTargetRecord();
			eventTargetRecord.EventType = eventCfg.eventType;
			//eventTargetRecord.TargetIndex = targetAvatarIndex;

			eventRecord.EventTargetRecords.Add(eventTargetRecord);

			List<object> parameters = new List<object>();
			parameters.Add(eventRecord);
			parameters.Add(eventCfg);
			parameters.Add(targetRole);

			// Push this event.
			targetRole.Avatar.AddAnimationEvent(eventCfg.keyFrameId, eventCfg.loop, parameters, actionCfg.id);
		}
	}


	private void ActionEventCallback(object userData0, object userData1)
	{
		List<object> parameters = (List<object>)userData0;
		EventRecord eventRecord = parameters[0] as EventRecord;
		AvatarAction.Event actionEventCfg = parameters[1] as AvatarAction.Event;
		BattleRole actionAvatar = parameters[2] as BattleRole;

		int actionId = (int)userData1;

		// Process event record.
		ProcessEventRecord(actionId, eventRecord, actionEventCfg, actionAvatar);
	}

	// Process event record.
	public void ProcessEventRecord(int actionId, EventRecord eventRecord, AvatarAction.Event actionEvent, BattleRole battleAvatar)
	{
		// Process event targets.
		bool processedOnTarget = false;

		foreach (var eventTargetRecord in eventRecord.EventTargetRecords)
		{
			// 对于非逻辑事件, 判断是否需要在所有目标上面播放. (逻辑事件从原理上面讲应该需要在所有目标上面播放)
			if (actionEvent != null && actionEvent.IsLogicEvent() == false && actionEvent.playOnAllTarget == false && processedOnTarget == true)
				break;

			processedOnTarget = true;

			// Process event AI.
			BattleEvent battleEvent = BattleEvent.CreateEvent(eventTargetRecord.EventType);
			//battleEvent.battleRecordPlayer = this;
			battleEvent.actAvatar = battleAvatar;
			battleEvent.targetAvatar = battleAvatar;
			battleEvent.eventTargetRecord = eventTargetRecord;
			battleEvent.actionEventCfg = actionEvent;

			battleEvent.Process();

			//// Log.
			//if (LogEvent)
			//    LogMsg(String.Format("ProcessEventRecord EvnTrgIndex:{0} EvnTrgAvtID:{1} EnvTrgType:{2} AIEvn:{3}", i, targetAvatar.RollData.RollId, eventTargetRecord.EventType, battleEvent.ToString()));
		}

		//// Log.
		//if (LogEvent)
		//    LogMsg(String.Format("ProcessEventRecord EvnIndex:{0} EvnTrgNum:{1} EvnAvtID:{2} EvnType:{3} ActID:{4:X}", eventRecord.EventIndex, eventRecord.GetRecordCount(), battleAvatar.RollData.RollId, actionEventCfg.EventType, actionId));
	}

}
