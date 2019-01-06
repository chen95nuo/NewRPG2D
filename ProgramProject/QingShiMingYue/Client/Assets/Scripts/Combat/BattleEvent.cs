using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using KodGames.ClientClass;
using ClientServerCommon;

public class BattleEvent
{
	public BattleRecordPlayer battleRecordPlayer;
	public EventTargetRecord eventTargetRecord; // Event target record data.
	public AvatarAction.Event actionEventCfg; // Action event.
	public BattleRole actAvatar; // Action role.
	public BattleRole targetAvatar; // Target role.
	public bool hasBeenDelayed = false; // Delayed flag.

	public static BattleEvent CreateEvent(int eventType)
	{
		//Debug.Log(AvatarAction.Event._Type.GetNameByType(eventType));
		switch (eventType)
		{
			case AvatarAction.Event._Type.EnterState:
				return new EnterStateEvent();
			case AvatarAction.Event._Type.LeaveState:
				return new LeaveStateEvent();
			case AvatarAction.Event._Type.Damage:
				return new DamageEvent();
			case AvatarAction.Event._Type.ThrowDamage:
				return new ThrowDamageEvent();
			case AvatarAction.Event._Type.SPDamage:
				return new SpDamageEvent();
			case AvatarAction.Event._Type.EnterBattleGround:
				return new EnterBattleGroundEvent();
			case AvatarAction.Event._Type.ShowAvatar:
				return new ShowAvatarEvent();
			case AvatarAction.Event._Type.InactiveAvatar:
				return new InactiveAvatarEvent();
			case AvatarAction.Event._Type.SetSkillPower:
				return new SetSkillPowerEvent();
			case AvatarAction.Event._Type.AddBuff:
				return new AddBuffEvent();
			case AvatarAction.Event._Type.RemoveBuff:
				return new RemoveBuffEvent();
			case AvatarAction.Event._Type.BuffTimeUp:
				return new BuffTimeUpEvent();
			case AvatarAction.Event._Type.SkillStart:
			case AvatarAction.Event._Type.CompositeSkillStart:
				return new SkillStartEvent();
			case AvatarAction.Event._Type.SuperSkillEffect:
				return new SuperSkillEffectEvent();
			case AvatarAction.Event._Type.Heal:
				return new HealEvent();
			case AvatarAction.Event._Type.Dan_DamageHeal:
				return new DmgHealEvent();
			case AvatarAction.Event._Type.CounterAttackStart:
				return new CounterAttackStartEvent();
			case AvatarAction.Event._Type.HideWeapon:
				return new HideWeaponEvent();
			case AvatarAction.Event._Type.ShowWeapon:
				return new ShowWeaponEvent();
			case AvatarAction.Event._Type.ChangeWeapon:
				return new ReplaceWeaponEvent();
			case AvatarAction.Event._Type.RecoverWeapon:
				return new RecoverWeaponEvent();
			case AvatarAction.Event._Type.DeleteBuff:
				return new DeleteBuffEvent();
			//手动生成战斗
			case AvatarAction.Event._Type.ShowDialogue:
				return new ShowDialogueEvent();
			case AvatarAction.Event._Type.ChangeBattlePosition:
				return new ChangeBattlePositionEvent();
			case AvatarAction.Event._Type.SetBattleTrans:
				return new SetBattleTransEvent();
			case AvatarAction.Event._Type.ShowBattleBar:
				return new ShowBattleBarEvent();
			case AvatarAction.Event._Type.AttributeChange:
				return new AttributeChangeEvent();
			default:
				return new BattleEvent();
		}

	}

	// Process this event.
	public virtual void Process()
	{
		// If need to be delayed or has been delayed, process
		if (actionEventCfg == null || actionEventCfg.delay == 0 || hasBeenDelayed)
		{
			// Process game logic
			DoAI();

			// Process visual logic
			DoEffect();

            if (PrcPsvActRcd() && eventTargetRecord != null)
            {
                foreach (var record in eventTargetRecord.PassiveActionRecords)
                    battleRecordPlayer.PlayActionRecord(record);
            }
		}
		else
		{
			// This event need to be delay by time
			actAvatar.AddDelayObject(new DelayTime(actionEventCfg.delay, DelayProcess, null, null));
			hasBeenDelayed = true;
		}
	}

	// Delay process event.
	protected void DelayProcess(object userData0, object userData1)
	{
		Process();
	}

	/// <summary>
	/// Check if the passive action record should be processed directly, specially for the event will be delay processing.
	/// </summary>
	/// <returns>Return true to enable process passive action record</returns>
	public virtual bool PrcPsvActRcd()
	{
		return true;
	}

	// Block flag of passive action for this event.
	public virtual bool PsvActRcdBlock()
	{
		return false;
	}

	// Do event AI.
	protected virtual void DoAI()
	{
	}

	// Do event effect.
	protected virtual void DoEffect()
	{
		if (actionEventCfg == null)
			return;

		// Do event effect.
		for (int i = 0; i < actionEventCfg.effects.Count; i++)
		{
			AvatarAction.Effect effect = actionEventCfg.effects[i];

			// Get-hit-FX will determined by damage event, skipped here.
			if (effect.isGetHitFX == true)
				continue;

			DoEffect(effect);
		}
	}

	protected virtual void DoEffect(AvatarAction.Effect effect)
	{
		switch (effect.type)
		{
			case AvatarAction.Effect._Type.PFX:
				ProcessPFX(effect);
				break;
			case AvatarAction.Effect._Type.SFX:
				ProcessSFX(effect);
				break;
			case AvatarAction.Effect._Type.CameraShake:
				ProcessCameraShake(effect);
				break;
			case AvatarAction.Effect._Type.TimeScale:
				ProcessTimeScale(effect);
				break;
		}
	}

	// Process PFX effectCfg.
	protected virtual void ProcessPFX(AvatarAction.Effect effect)
	{
		if (effect.pfx_Model == "")
			return;

		// Stop pfx with same name 
		targetAvatar.Avatar.StopPfxByName(effect.pfx_Model);

		// Play this PFX on the role.
		BattleRole role = PfxRole(effect);
		if (role == null)
			return;

		// Set Target Role
		role.TargetRole = targetAvatar;

		// Create the PFX
		var fx = CreateAnimatedPFX(effect, targetAvatar.TeamIndex == 1);
		if (fx == null)
			return;

		bool useSpecificPosition = false;
		Vector3 specificPosition = Vector3.zero;
		switch (effect.pfx_PlayTargetType)
		{
			case AvatarAction.Effect._PlayTargetType.Avatar:
				break;

			case AvatarAction.Effect._PlayTargetType.TeamCenter:
				useSpecificPosition = true;
				specificPosition = battleRecordPlayer.BattleScene.GetTeamBattleCenter(battleRecordPlayer.BattleIndex, role.TeamIndex);
				break;

			case AvatarAction.Effect._PlayTargetType.RowCenter:
				break;

			case AvatarAction.Effect._PlayTargetType.ColumnFront:
				break;

			case AvatarAction.Effect._PlayTargetType.SceneCenter:
				useSpecificPosition = true;
				Vector3 sponsorCenter = battleRecordPlayer.BattleScene.GetTeamBattleCenter(battleRecordPlayer.BattleIndex, battleRecordPlayer.SponsorTeamIndex);
				Vector3 opponentCenter = battleRecordPlayer.BattleScene.GetTeamBattleCenter(battleRecordPlayer.BattleIndex, battleRecordPlayer.OpponentTeamIndex);
				specificPosition = Vector3.Lerp(sponsorCenter, opponentCenter, 0.5f);
				break;
		}

		// Player PFX
		role.Avatar.PlayPfx(fx, effect.pfx_DestroyType, GetPfxUsd(effect), effect.pfx_ModelBone, effect.pfx_Bone, effect.pfx_BoneFollow, Converter.ToVector3(effect.pfx_Offset), Converter.ToVector3(effect.pfx_Rotate), useSpecificPosition, specificPosition);

		// If curve faces to target, adjust PFX transform.
		Vector3 start, end;
		if (effect.pfx_CurveToTarget && GetPFXStartEnd(effect, out start, out end))
		{
			// Face to dest.
			fx.Root.transform.forward = end - start;
			fx.Root.transform.forward.Normalize();
		}

		// Do PFX AI.
		PFXAI(effect, fx);
	}

	// Process sound effectCfg.
	protected virtual void ProcessSFX(AvatarAction.Effect effectCfg)
	{
		if (effectCfg.sfx_Sound == "")
			return;

		AudioManager.Instance.PlaySound(effectCfg.sfx_Sound, effectCfg.sfx_Volume, 0);
	}

	// Process camera effectCfg.
	protected virtual void ProcessCameraShake(AvatarAction.Effect effectCfg)
	{
		Camera.main.GetComponent<CameraController>().Shake(effectCfg.cfx_Intensity, effectCfg.cfx_Duration, effectCfg.cfx_Interval);
		//SysModuleManager.Instance.GetSysModule<SysCamera>().Shake(effectCfg.cfx_Intensity, effectCfg.cfx_Duration, effectCfg.cfx_Interval);
	}

	// Process time effectCfg.
	protected virtual void ProcessTimeScale(AvatarAction.Effect effectCfg)
	{
		SysModuleManager.Instance.GetSysModule<SysFx>().ScaleTime(effectCfg.tfx_Scale, effectCfg.tfx_Duration);
	}

	// Get the PFX playing role.
	protected virtual BattleRole PfxRole(AvatarAction.Effect effectCfg)
	{
		return effectCfg.pfx_PlayOnTarget ? targetAvatar : actAvatar;
	}

	// Create PFX.
	protected virtual FXController CreatePFX(string pfxName)
	{
		if (pfxName == "")
			return null;

		return SysFx.Instance.CreateFx(KodGames.PathUtility.Combine(GameDefines.pfxPath, pfxName));
	}

	// Pfx curve.
	protected virtual string GetPFXCurve(AvatarAction.Effect effectCfg)
	{
		return effectCfg.pfx_Curve;
	}

	// Pfx value range.
	protected virtual Dictionary<string, float> GetPFXValueRange(AvatarAction.Effect effectCfg)
	{
		Dictionary<string, float> valRange = new Dictionary<string, float>();

		Vector3 start;
		Vector3 end;

		if (effectCfg.pfx_CurveToTarget && GetPFXStartEnd(effectCfg, out start, out end))
			valRange["m_LocalPosition.z"] = Vector3.Distance(start, end);

		return valRange;
	}

	protected virtual FXController CreateAnimatedPFX(AvatarAction.Effect effectCfg, bool needReverse)
	{
		Debug.Assert(effectCfg.type == AvatarAction.Effect._Type.PFX);

		FXController pfxScript = CreatePFX(effectCfg.pfx_Model);
		if (pfxScript == null)
			return null;

		string curveName = GetPFXCurve(effectCfg);

		// Add curve.
		KodGames.AnimCurve animCurve = SysModuleManager.Instance.GetSysModule<SysFx>().AnimCurve;
		if (animCurve != null && animCurve.AddCrvClipToObj(pfxScript.gameObject, curveName, GetPFXValueRange(effectCfg), effectCfg.pfx_CurveTranslateSpeed))
		{
			// Set speed.
			pfxScript.CachedAnimation[curveName].speed = effectCfg.pfx_CurveSpeed;

			// Play animation
			pfxScript.CachedAnimation.Play(effectCfg.pfx_Curve);
		}

		//// Add a parent
		//Transform pfxParent = new GameObject(pfxScript.name).transform;
		//ObjectUtility.UnifyWorldTrans(pfxScript.CachedTransform, pfxParent);
		//pfxScript.MoungintRoot = pfxParent.gameObject;

		//// Keep FXController local scale
		//Vector3 oldScale = pfxScript.CachedTransform.localScale;
		//pfxScript.CachedTransform.localScale = new Vector3(1, 1, 1);
		//ObjectUtility.AttachToParentAndResetLocalPosAndRotation(pfxParent, pfxScript.CachedTransform);
		//pfxParent.localScale = oldScale;

		if (needReverse)
			pfxScript.Root.Rotate(new Vector3(0, 180, 0));

		return pfxScript;
	}

	protected virtual int GetPfxUsd(AvatarAction.Effect effectCfg)
	{
		return IDSeg.InvalidId;
	}

	protected virtual void PFXAI(AvatarAction.Effect effectCfg, FXController pfxObj)
	{
		if (effectCfg.pfx_DestroyType == AvatarAction.Effect._DestroyType.BlockAction)
		{
			PfxRole(effectCfg).AddDelayObject(new DelayFX(pfxObj, null, null, null));
		}
	}

	// Get FXController start and end position.
	protected bool GetPFXStartEnd(AvatarAction.Effect effect, out Vector3 start, out Vector3 end)
	{
		BattleRole role = PfxRole(effect);

		// Get start position.
		Transform startTrans = role.Avatar.CachedTransform;

		if (effect.pfx_Bone != "")
		{
			// Start from bone
			var boneObj = ObjectUtility.FindChildObject(role.Avatar.AvatarObject, effect.pfx_Bone);
			if (boneObj != null)
				startTrans = boneObj;
			else
				Debug.LogWarning("Missing bone when play effect : " + effect.pfx_Bone);
		}

		startTrans.Translate(Converter.ToVector3(effect.pfx_Offset));
		start = startTrans.position;
		startTrans.Translate(-Converter.ToVector3(effect.pfx_Offset));

		// Get end position.
		Transform endTrans = role.TargetRole.Avatar.CachedTransform;

		if (effect.pfx_CurveTargetBone != "")
		{
			// target to bone
			var boneObj = ObjectUtility.FindChildObject(role.TargetRole.Avatar.AvatarObject, effect.pfx_CurveTargetBone);
			if (boneObj != null)
				endTrans = boneObj;
			else
				Debug.LogWarning("Missing bone when play effect : " + effect.pfx_Bone);
		}

		endTrans.Translate(Converter.ToVector3(effect.pfx_CurveTargetOffset));
		end = endTrans.position;
		endTrans.Translate(-Converter.ToVector3(effect.pfx_CurveTargetOffset));

		return true;
	}
}

// Damage event.
public class DamageEvent : BattleEvent
{
	protected override void DoAI()
	{
		PrcDamage();
	}

	protected void PrcDamage()
	{
		// Process HP.
		targetAvatar.AvatarHP -= eventTargetRecord.Value;
		targetAvatar.BattleBar.UpdateHP();

		// Pop damage effectCfg.
		SysFx sysFx = SysModuleManager.Instance.GetSysModule<SysFx>();
		if (sysFx != null)
		{
			if ((eventTargetRecord.TestType & CombatTurn._TestType.CriticalHit) != 0)
				sysFx.PopHurtText(targetAvatar.FxPosition, eventTargetRecord.Value, HURT_TEXT_TP.CRITICAL);
			else if ((eventTargetRecord.TestType & CombatTurn._TestType.Dodged) != 0)
				sysFx.PopHurtText(targetAvatar.FxPosition, eventTargetRecord.Value, HURT_TEXT_TP.MISS);
			else if (eventTargetRecord.Value > 0)
				sysFx.PopHurtText(targetAvatar.FxPosition, eventTargetRecord.Value, HURT_TEXT_TP.HURT);
		}

		// Take valid damage, do get-hit effectCfg
		if (eventTargetRecord.Value > 0)
		{
			DoGetHitEffect();
		}
	}

	protected void DoGetHitEffect()
	{
		if (actionEventCfg == null)
			return;

		// Do event effectCfg.
		foreach (var effectCfg in actionEventCfg.effects)
		{
			// Process get hit FX and skip all other FX.
			if (effectCfg.isGetHitFX == false)
				continue;

			DoEffect(effectCfg);
		}
	}
}

// Throw Damage
public class ThrowDamageEvent : DamageEvent
{
	private bool hasProcess = false;

	protected override void DoAI()
	{
	}

	public override bool PrcPsvActRcd()
	{
		return false;
	}

	protected override FXController CreatePFX(string pfxName)
	{
		if (string.Compare(pfxName, GameDefines.btPfxClnCurWpn, true) == 0)
		{
			// Create weapon FX
			string assetPath = actAvatar.GetWeaponAssetPath();
			if (assetPath.Equals(""))
			{
				Debug.LogError("No weapon to clone");
				return null;
			}

			// process throw weapon particle
			// same transform with avatar weapon
			FXController fxc = SysFx.Instance.CreateFxAndBuildParent(assetPath);
			string assetName = System.IO.Path.GetFileName(assetPath);
			AvatarComponent.UsedComponent weaponComponent = actAvatar.Avatar.GetUsedComponent(actAvatar.GetWeaponAssetID());
			Transform avatarWeapon = ObjectUtility.FindChildObject(weaponComponent.rootObject, assetName, true);
			Transform particleWeapon = ObjectUtility.FindChildObject(fxc.transform, assetName, true);
			particleWeapon.localPosition = new Vector3(avatarWeapon.localPosition.x, avatarWeapon.localPosition.y, avatarWeapon.localPosition.z);
			particleWeapon.localRotation = new Quaternion(avatarWeapon.localRotation.x, avatarWeapon.localRotation.y, avatarWeapon.localRotation.z, avatarWeapon.localRotation.w);

			return fxc;
		}
		else
		{
			return base.CreatePFX(pfxName);
		}
	}

	protected override string GetPFXCurve(AvatarAction.Effect effectCfg)
	{
		if ((eventTargetRecord.TestType & CombatTurn._TestType.Dodged) != 0)
			return effectCfg.pfx_Curve_Miss;

		return effectCfg.pfx_Curve;
	}

	protected override void PFXAI(AvatarAction.Effect effectCfg, FXController pfxObj)
	{
		base.PFXAI(effectCfg, pfxObj);

		var animCurve = pfxObj.GetComponentInChildren<KodGames.AnimCurve>();
		if (animCurve)
			PfxRole(effectCfg).AddDelayObject(new DelayFX(pfxObj, OnCurveTranslateEnd, null, null));
		//else
		//    OnCurveTranslateEnd(null, null);
	}

	protected void OnCurveTranslateEnd(object userData0, object userData1)
	{
		// Just Process callback once ,when there are several curve
		if (hasProcess)
		{
			return;
		}
		hasProcess = true;

		PrcDamage();

		if (eventTargetRecord.PassiveActionRecords != null)
			foreach (var record in eventTargetRecord.PassiveActionRecords)
				battleRecordPlayer.PlayActionRecord(record);
	}
}

public class SpDamageEvent : BattleEvent
{
	protected override void DoAI()
	{
		PrcSpDamage();
	}

	protected void PrcSpDamage()
	{
		if (actAvatar.IsDead())
			return;

		int skillPower = (int)targetAvatar.BattleBar.hpBar.SPValue - eventTargetRecord.Value;
		skillPower = Mathf.Max(0, skillPower);
		targetAvatar.BattleBar.hpBar.BarPowerValue = skillPower;

		SysFx sysFx = SysModuleManager.Instance.GetSysModule<SysFx>();

		if ((eventTargetRecord.TestType & CombatTurn._TestType.CriticalHit) != 0)
			sysFx.PopHurtText(targetAvatar.FxPosition, eventTargetRecord.Value, HURT_TEXT_TP.SP_CRITICAL);
		else if ((eventTargetRecord.TestType & CombatTurn._TestType.Dodged) != 0)
			sysFx.PopHurtText(targetAvatar.FxPosition, eventTargetRecord.Value, HURT_TEXT_TP.MISS);
		else if (eventTargetRecord.Value >= 0)
			sysFx.PopHurtText(targetAvatar.FxPosition, eventTargetRecord.Value, HURT_TEXT_TP.SP_HURT);

		if (eventTargetRecord.Value >= 0)
		{
			DoGetHitEffect();
		}
	}

	protected void DoGetHitEffect()
	{
		if (actionEventCfg == null)
			return;

		// Do event effectCfg.
		foreach (var effectCfg in actionEventCfg.effects)
		{
			// Process get hit FX and skip all other FX.
			if (effectCfg.isGetHitFX == false)
				continue;

			DoEffect(effectCfg);
		}
	}
}

// Enter state.
public class EnterStateEvent : BattleEvent
{
	protected override void DoAI()
	{
		targetAvatar.AddCombatState(eventTargetRecord.Value);
	}
}

// Leave state.
public class LeaveStateEvent : BattleEvent
{
	protected override void DoAI()
	{
		targetAvatar.RemoveCombatState(eventTargetRecord.Value);
	}
}

public class EnterBattleGroundEvent : BattleEvent
{
	protected override void DoAI()
	{
		actAvatar.BattlePosition = eventTargetRecord.Value;

		Vector3 position = battleRecordPlayer.BattleScene.GetBattlePosition(battleRecordPlayer.BattleIndex, actAvatar.TeamIndex, actAvatar.GetBattlePositionRow(), actAvatar.GetBattlePositionColumn());
		actAvatar.MoveTo(position, ConfigDatabase.DefaultCfg.GameConfig.combatSetting.switchColumnSpeed);
	}
}

public class ShowAvatarEvent : BattleEvent
{
	protected override void DoAI()
	{
		actAvatar.Hide = false;
	}
}

public class InactiveAvatarEvent : BattleEvent
{
	protected override void DoAI()
	{
		//烽火狼烟播放特效时，角色身上没有BattleBar
		if (actAvatar.BattleBar != null)
			actAvatar.BattleBar.Hide(true);
		actAvatar.Hide = true;
		//actAvatar.BattleBar.gameObject.SetActive(false);
		//actAvatar.gameObject.SetActive(false);
		actAvatar.ChangeState(BattleRole._ActionState.Idle);
	}
}

public class SetSkillPowerEvent : BattleEvent
{
	protected override void DoAI()
	{
		if (actAvatar.IsDead())
		{
			return;
		}

		int skillPower = (int)targetAvatar.BattleBar.hpBar.SPValue + eventTargetRecord.Value;
		skillPower = Mathf.Max(0, skillPower);

		bool isUp = false;
		if (targetAvatar.BattleBar.hpBar.SPValue < skillPower)
		{
			isUp = true;
		}

		targetAvatar.BattleBar.hpBar.BarPowerValue = skillPower;
		SysFx sysFx = SysModuleManager.Instance.GetSysModule<SysFx>();
		if (skillPower >= 100)
		{
			if (targetAvatar.Avatar.GetPfxInstIDListByUsd(GetPfxUsd(null)).Count == 0)
			{
				sysFx.PopUIObj(GameDefines.uiEffectPath, "UIFx_Nu", targetAvatar.FxPosition, true);
				var p_NuBuffObj = CreatePFX("p_Q_Nu_Buff");
				targetAvatar.Avatar.PlayPfx(p_NuBuffObj, AvatarAction.Effect._DestroyType.Buff, GetPfxUsd(null), "", "", true, Vector3.zero, Vector3.zero, false, Vector3.zero);
			}
		}
		else
		{
			if (targetAvatar.Avatar.GetPfxInstIDListByUsd(GetPfxUsd(null)).Count != 0)
			{
				targetAvatar.Avatar.StopPfxByUsd(GetPfxUsd(null));
			}
		}

		// pop ui
		if (eventTargetRecord.Value1 == 1)
		{
			if (isUp)
			{
				sysFx.PopUIObj(GameDefines.uiEffectPath, "UIFx_NuQi_Up", targetAvatar.FxPosition, true);
			}
			else
			{
				sysFx.PopUIObj(GameDefines.uiEffectPath, "UIFx_NuQi_Down", targetAvatar.FxPosition, true);
			}
		}
	}

	protected override int GetPfxUsd(AvatarAction.Effect effectCfg)
	{
		return int.MaxValue;
	}
}

// Add Buff.
public class AddBuffEvent : BattleEvent
{
	private bool newBuff = true;

	protected override void DoAI()
	{
		if ((eventTargetRecord.TestType & CombatTurn._TestType.Dodged) != 0)
		{
			//Dodged
			return;
		}

		if (targetAvatar.IsDead())
		{
			return;
		}

		int buffInstId = eventTargetRecord.Value;

		newBuff = targetAvatar.DoesBuffExist(buffInstId) == false;
		if (newBuff)
			targetAvatar.AddBuff(eventTargetRecord.Value, eventTargetRecord.Value1);

		Buff buf = ConfigDatabase.DefaultCfg.ActionConfig.GetBuffById(eventTargetRecord.Value1);

		if (buf == null)
			Debug.LogError("Missing buf: " + eventTargetRecord.Value1.ToString("X"));

		if (buf != null && buf.uiName != "")
		{
			SysModuleManager.Instance.GetSysModule<SysFx>().PopUIObj(GameDefines.uiEffectPath, buf.uiName, targetAvatar.FxPosition, true);
		}
	}

	protected override void DoEffect()
	{
		if ((eventTargetRecord.TestType & CombatTurn._TestType.Dodged) != 0)
		{
			//Dodged
			return;
		}

		if (targetAvatar.IsDead())
		{
			return;
		}

		base.DoEffect();

		Buff buf = ConfigDatabase.DefaultCfg.ActionConfig.GetBuffById(eventTargetRecord.Value1);
		if (buf == null)
			return;

		if (newBuff == false)
			return;

		// Do buff effectCfg.
		for (int i = 0; i < buf.effects.Count; i++)
		{
			DoEffect(buf.effects[i]);
		}
	}

	protected override int GetPfxUsd(AvatarAction.Effect effectCfg)
	{
		return eventTargetRecord.Value;
	}
}

// Remove Buff.
public class RemoveBuffEvent : BattleEvent
{
	protected override void DoAI()
	{
		targetAvatar.DelBuff(eventTargetRecord.Value);

		// Del buff FXController.
		targetAvatar.Avatar.StopPfxByUsd(eventTargetRecord.Value);
	}
}

// Buff Time Up.
public class BuffTimeUpEvent : BattleEvent
{
	protected override void DoAI()
	{
		targetAvatar.DelBuff(eventTargetRecord.Value);

		// Del buff FXController.
		targetAvatar.Avatar.StopPfxByUsd(eventTargetRecord.Value);
	}
}

// Buff be removed.
public class DeleteBuffEvent : BattleEvent
{
	protected override void DoAI()
	{
		int buffId = eventTargetRecord.Value;
		int buffType = eventTargetRecord.Value1;

		var deletingBuffs = new List<BattleRole.Buff>();
		foreach (KeyValuePair<int, BattleRole.Buff> buff in targetAvatar.GetBuffs())
		{
			Buff buffConfig = ConfigDatabase.DefaultCfg.ActionConfig.GetBuffById(buff.Value.bufID);
			int type = buffConfig.buffType;
			if (buff.Value.bufID == buffId)
				deletingBuffs.Add(buff.Value);

			if (type == buffType)
				deletingBuffs.Add(buff.Value);
		}

		foreach (var buff in deletingBuffs)
		{
			targetAvatar.DelBuff(buff.instID);
			targetAvatar.Avatar.StopPfxByUsd(buff.instID);
		}
	}
}

//普通角色暴走技
public class SkillStartEvent : BattleEvent
{
	protected override void DoAI()
	{
		int id = this.eventTargetRecord.Value;

		SkillConfig.Skill skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(id);
		if (skillCfg == null)
			return;

		//屏幕中间弹扇板
		//UIPnlBattleBar uiBattleBar = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlBattleBar>();
		//var skillFX = SysModuleManager.Instance.GetSysModule<SysFx>().PopSkill(uiBattleBar.skillPos.transform.position, actAvatar, skillCfg.id);

		//在人物头上弹UI
		var skillFX = SysFx.Instance.PopSuperSkillForNormalAvatar(actAvatar.BattleBar.CachedTransform.position, actAvatar, skillCfg.id);

		if (skillFX == null)
			return;

		UIAvatarSuperSkillBar skillEffectBar = skillFX.gameObject.GetComponent<UIAvatarSuperSkillBar>();
		if (skillEffectBar == null)
			skillFX.gameObject.AddComponent<UIAvatarSuperSkillBar>().battleBar = actAvatar.BattleBar;
		else
			skillEffectBar.battleBar = actAvatar.BattleBar;

		//播放技能配音
		if (skillFX == null || string.IsNullOrEmpty(skillCfg.roleVoiceName))
			return;

		var eventHandler = skillFX.GetComponentInChildren<AnimationEventHandler>();

		if (eventHandler == null)
		{
			Debug.LogError("SkillStartEffect lose AnimationEventHandler");
			return;
		}

		eventHandler.userEventDelegate = (eventname, eventdata) =>
		{
			//释放小弹板的角色播放技能配音
			if (eventname == "PlayNormalRoleSuperSkillVoice")
				AudioManager.Instance.PlaySound(skillCfg.roleVoiceName, 1, 0);
			//防止缓存的UI在此播放配音
			eventHandler.userEventDelegate = null;
		};
	}
}

//五星角色暴走技
public class SuperSkillEffectEvent : BattleEvent
{
	protected override void DoAI()
	{
		int id = this.eventTargetRecord.Value;

		SkillConfig.Skill skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(id);
		if (skillCfg == null)
			return;

		var skillFX = SysModuleManager.Instance.GetSysModule<SysFx>().PopSuperSkill(actAvatar, skillCfg.id, actAvatar.TeamIndex == battleRecordPlayer.SponsorTeamIndex);

		if (skillFX == null || string.IsNullOrEmpty(skillCfg.roleVoiceName))
			return;

		var eventHandler = skillFX.GetComponent<UIEffectSkillStart>().card.GetComponent<AnimationEventHandler>();

		if (eventHandler == null)
		{
			Debug.LogError("SuperSkillStartEffect lose AnimationEventHandler");
			return;
		}

		eventHandler.userEventDelegate = (eventname, eventdata) =>
		{
			//释放大弹板的角色播放技能配音
			if (eventname == "PlayRoleSuperSkillVoice")
				AudioManager.Instance.PlaySound(skillCfg.roleVoiceName, 1, 0);
			//防止缓存的UI再次播放配音
			eventHandler.userEventDelegate = null;
		};
	}
}

public class CounterAttackStartEvent : BattleEvent
{
	protected override void DoAI()
	{
		SysFx sysFx = SysModuleManager.Instance.GetSysModule<SysFx>();
		if (sysFx != null)
		{
			var uiObj = sysFx.PopUIObj(GameDefines.uiEffectPath, GameDefines.uiFXCounterAttackStart, actAvatar.FxPosition, true);
			actAvatar.AddDelayObject(new DelayFX(uiObj, null, null, null));
		}
	}
}

// Heal event.
public class HealEvent : BattleEvent
{
	protected override void DoAI()
	{
		PrcHeal();
	}

	// When player is dead, make the die action can not be terminated by result action.
	public override bool PsvActRcdBlock()
	{
		return targetAvatar.AvatarHP <= 0;
	}

	protected void PrcHeal()
	{
		// Process HP.
		targetAvatar.AvatarHP += eventTargetRecord.Value;
		targetAvatar.BattleBar.UpdateHP();

		if (eventTargetRecord.Value1 > 0)
		{
			SysFx sysFx = SysModuleManager.Instance.GetSysModule<SysFx>();
			sysFx.PopHurtText(targetAvatar.FxPosition, eventTargetRecord.Value, HURT_TEXT_TP.HEAL);
		}
	}
}

public class DmgHealEvent : HealEvent
{
	public override void Process()
	{
		if (!hasBeenDelayed)
		{
			//开始时间
			float startDuration = ConfigDatabase.DefaultCfg.GameConfig.combatSetting.dmgHealStartDuration;
			//两次弹板之间的事件间隔
			float delta = ConfigDatabase.DefaultCfg.GameConfig.combatSetting.dmgHealEventDelta;
			switch (eventTargetRecord.Value1)
			{
				case 1://第1次弹板
					actAvatar.AddDelayObject(new DelayTime(startDuration, DelayProcess, null, null));
					break;

				case 2://第2次弹板
					actAvatar.AddDelayObject(new DelayTime(startDuration + delta, DelayProcess, null, null));
					break;

				case 3://第3次弹板
					actAvatar.AddDelayObject(new DelayTime(startDuration + 2 * delta, DelayProcess, null, null));
					break;

				default:
					Debug.LogError("DmgHealEvent Invalid Value1:" + eventTargetRecord.Value1);
					break;
			}

			hasBeenDelayed = true;
		}
		else
		{
			DoAI();

			DoEffect();

			//用于内丹吸血弹板。内丹吸血播放Action：CombatIdle。可能弹1、2、3次板，使用循环动画，但是在循环动画下角色会永远处于Busy状态，需要在最后一次弹板设为Idle
			//使用TestType标记是否是最后一个弹板，这里TestType仅像Value一样用来存储数据。
			if (eventTargetRecord.TestType != 0)
				actAvatar.ChangeState(BattleRole._ActionState.Idle);
		}
	}
}

// Hide weapon event.
public class HideWeaponEvent : BattleEvent
{
	protected override void DoAI()
	{
		actAvatar.HideWeapon(true);
	}
}

// Show weapon event.
public class ShowWeaponEvent : BattleEvent
{
	protected override void DoAI()
	{
		actAvatar.HideWeapon(false);
	}
}

// Replace weapon event.
public class ReplaceWeaponEvent : BattleEvent
{
	protected override void DoAI()
	{
		actAvatar.ReplaceWeapon(actionEventCfg.weaponId, actionEventCfg.boneName);
	}
}

// Recover weapon event.
public class RecoverWeaponEvent : BattleEvent
{
	protected override void DoAI()
	{
		actAvatar.RecoverWeapon(actionEventCfg.weaponId);
	}
}

//手动配置战斗++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

//战斗中显示对话
public class ShowDialogueEvent : BattleEvent
{
	bool dialogueClosed = false;

	//对话前后固定延迟，写死。
	float beforeDialogueDelay = 0.3f;
	float afterDialogueDelay = 0.5f;

	protected override void DoAI()
	{
		base.DoAI();

		int dialogueSetId = eventTargetRecord.Value;
		bool postDelayTimeAdded = false;

		if (actAvatar == null)
		{
			Debug.LogError("[ShowDialogueEvent] actAvatar can not be null");
			return;
		}

		//临时解决battleRound不能被Delay的Bug
		actAvatar.ChangeState(BattleRole._ActionState.Busy);

		actAvatar.AddDelayObject(new DelayTime(beforeDialogueDelay,
		(m, n) =>
		{//开始前延迟固定时间

			//延迟完成后显示对话
			SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
			uiEnv.GetUIModule<UITipAdviser>().ShowDialogue(dialogueSetId,
				() =>
				{
					//对话关闭后通知BattleEvent
					dialogueClosed = true;
				});

			//添加对话延迟对象
			actAvatar.AddDelayObject(new DialogueDelayObject(
				() =>
				{
					if (dialogueClosed && !postDelayTimeAdded)
					{
						postDelayTimeAdded = true;
						//对话完成后的延迟
						actAvatar.AddDelayObject(new DelayTime(afterDialogueDelay,
						(a, b) =>
						{
							//临时解决battleRound不能被Delay的Bug
							actAvatar.ChangeState(BattleRole._ActionState.Idle);
						}, null, null));
					}
					return dialogueClosed;
				}
				, null, null, null));
		}
		, null, null));

	}
}

//剧情战斗，改变角色站位
public class ChangeBattlePositionEvent : BattleEvent
{
	protected override void DoAI()
	{
		base.DoAI();

		actAvatar.BattlePosition = eventTargetRecord.Value;
	}
}

//剧情战斗，角色可见前设置其物理位置和物理朝向
public class SetBattleTransEvent : BattleEvent
{
	protected override void DoAI()
	{
		base.DoAI();

		bool needRun = eventTargetRecord.Value == 1 ? true : false;
		Vector3 position, forward;

		//角色之后是否需要向前跑一步（需要配置EnterBattleGround Action）
		if (needRun)
		{
			position = battleRecordPlayer.BattleScene.GetStartPosition(battleRecordPlayer.BattleIndex, actAvatar.TeamIndex, actAvatar.GetBattlePositionRow(), actAvatar.GetBattlePositionColumn());
		}
		else
		{
			position = battleRecordPlayer.BattleScene.GetBattlePosition(battleRecordPlayer.BattleIndex, actAvatar.TeamIndex, actAvatar.GetBattlePositionRow(), actAvatar.GetBattlePositionColumn());
		}

		forward = battleRecordPlayer.BattleScene.GetTeamForward(battleRecordPlayer.BattleIndex, actAvatar.TeamIndex);

		actAvatar.Avatar.CachedTransform.position = position;
		actAvatar.Avatar.CachedTransform.forward = forward;
		//战斗中TurnBack时会回到Footthold的位置
		actAvatar.Foothold = position;

		//事件执行不需要时间，让battleRole可以继续执行其他Action
		actAvatar.ChangeState(BattleRole._ActionState.Idle);
	}
}

//显示指定角色的血条
public class ShowBattleBarEvent : BattleEvent
{
	protected override void DoAI()
	{
		base.DoAI();

		if (eventTargetRecord.Value == 1)//显示血条
			actAvatar.BattleBar.Hide(false);
		else if (eventTargetRecord.Value == 0)//隐藏血条
			actAvatar.BattleBar.Hide(true);

		//事件执行不需要时间，让battleRole可以继续执行其他Action
		actAvatar.ChangeState(BattleRole._ActionState.Idle);
	}
}

public class AttributeChangeEvent : BattleEvent
{
	protected override void DoAI()
	{
		base.DoAI();
		targetAvatar.AvatarData.GetAttributeByType(eventTargetRecord.Value).Value = eventTargetRecord.Value1;
	}
}