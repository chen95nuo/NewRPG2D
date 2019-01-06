using UnityEngine;
using System.Collections.Generic;
using KodGames;
using KodGames.ClientClass;
using ClientServerCommon;

//播放霸气技特效。
//根据时间是否达到或播放任务是否为空来决定是否可以进入下一个Round，原因：如果中间延迟时间略长，大于特效的存在时间，在此期间角色清空了DelayObject后自动进入Idle
//因此不能够通过判断角色是否处于Busy来判断本Round是否结束
public class EnterBattleSkillRound : BattleRound
{
	//提取所有特效事件按照播放顺序与制定AvatarIndex组合
	//[AvatarIndex,[EventConfig,EventRecord]]
	Dictionary<int, List<Pair<AvatarAction.Event, EventRecord>>> domineerEffects;

	//开始前延迟或下一次播放的时间
	float nextPlayTime = 0.1f;

	//所有特效播放完毕后的延迟时间，用于等待特效小时，防止下一回合开始过快
	float afterDelayTime = 0.8f;

	//两次播放之间的延迟
	float buffDelayTime = 0.6f;

	//是否所有特效都播放完成了
	bool playFinished = false;

	//计时器
	float roundTime = 0;

	public EnterBattleSkillRound(RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{
		//按照AvatarIndex和Event分组
		domineerEffects = new Dictionary<int, List<Pair<AvatarAction.Event, EventRecord>>>();

		foreach (var turnRecord in roundRecord.TurnRecords)
		{
			foreach (var actionRecord in turnRecord.ActionRecords)
			{
				//获取AvatarIndex
				int avatarIndex = actionRecord.SrcAvatarIndex;

				if (actionRecord.ActionId == IDSeg.InvalidId)
				{
					Debug.LogError("DomineerSkillFxProcess at [EnterBattleSkillRound] ActionId=IDSeg.InvalidId");
					continue;
				}

				AvatarAction actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionById(actionRecord.ActionId);

				if (actionCfg == null)
					continue;

				foreach (var eventCfg in actionCfg.events)
				{
					//忽略逻辑事件
					if (eventCfg == null || eventCfg.IsLogicEvent())
						continue;

					//只取播放特效的事件
					if (eventCfg.eventType != AvatarAction.Event._Type.PlayEffect)
						continue;

					//构造EventRecord，用于播放特效
					EventRecord eventRecord = new EventRecord();
					EventTargetRecord etRecord = new EventTargetRecord();
					etRecord.TargetIndex = avatarIndex;
					etRecord.EventType = eventCfg.eventType;

					//在一个人的头上播放一次特效
					eventRecord.EventTargetRecords.Add(etRecord);

					//构建播放任务
					Pair<AvatarAction.Event, EventRecord> pair = new Pair<AvatarAction.Event, EventRecord>()
					{
						first = eventCfg,
						second = eventRecord
					};

					if (!domineerEffects.ContainsKey(avatarIndex))
						domineerEffects.Add(avatarIndex, new List<Pair<AvatarAction.Event, EventRecord>>());

					domineerEffects[avatarIndex].Add(pair);
				}
			}
		}
	}

	public override bool Start()
	{
		if (!base.Start())
			return false;

		return true;
	}

	public override bool CanStartAfterRound()
	{
		if (!base.CanStartAfterRound())
			return false;

		if (RoundState == _RoundState.NotStarted)
			return false;

		//所有特效播放完毕，并且后期延迟时间达到
		if (playFinished && roundTime > nextPlayTime)
			return true;

		return false;
	}

	public override void Update()
	{
		base.Update();

		if (RoundState == _RoundState.Running || RoundState == _RoundState.Finished)
			roundTime += Time.deltaTime;

		//指定时间间隔播放
		if (roundTime > nextPlayTime && !playFinished)
		{
			PlayAction();

			if (domineerEffects.Count != 0)
				nextPlayTime += buffDelayTime;
			else
			{
				//所有特效都播放完成了，但之后还有延迟时间
				playFinished = true;
				nextPlayTime += afterDelayTime;
			}
		}
	}

	void PlayAction()
	{
		List<int> indexToRemove = new List<int>();
		foreach (var effect in domineerEffects)
		{
			BattleRole actAvatar = battleRecordPlayer.BattleRoles[effect.Key];

			if (effect.Value.Count > 0)
			{
				//指定角色播放一次霸气特效
				battleRecordPlayer.ProcessEventRecord(0, effect.Value[0].second, effect.Value[0].first, actAvatar);
				effect.Value.RemoveAt(0);
			}

			if (effect.Value.Count == 0)
				indexToRemove.Add(effect.Key);
		}

		//移除播放完毕后的任务
		foreach (int index in indexToRemove)
			if (domineerEffects.ContainsKey(index))
				domineerEffects.Remove(index);

		indexToRemove.Clear();
	}
}