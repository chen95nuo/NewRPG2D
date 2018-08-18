using Assets.Script.Battle.BattleData;
using Spine;
using UnityEngine;
using Event = Spine.Event;

namespace Assets.Script.Battle.RoleState
{
    public class RoleBaseSkillState : FsmState<RoleBase>
    {
        public virtual string AnimationName
        {
            get { return ""; }
        }

        public RoleBase TargetRole, CurrentRole;

        public SkillUseData CurrentSkillData;
        public float skillCDTime;
        public int MP;
        public SkillTypeEnum SkillType;
        public int TargetCount;
        public int EffectId1;
        public int EffectId2;
        public int EffectId3;
        public int EffectId4;
        public TrackEntry AnimationEntry;

        protected float addTime = 0;

        public override void Enter(RoleBase mRoleBase)
        {
            base.Enter(mRoleBase);
            // OnceAttack(mRoleBase);
            skillCDTime = CurrentSkillData.SkillCDTime;
            MP = CurrentSkillData.MP;
            SkillType = CurrentSkillData.SkillType;
            TargetCount = CurrentSkillData.TargetCount;
            EffectId1 = CurrentSkillData.EffectId1;
            EffectId2 = CurrentSkillData.EffectId2;
            EffectId3 = CurrentSkillData.EffectId3;
            EffectId4 = CurrentSkillData.EffectId4;
            addTime = CurrentSkillData.CurrentCD;
            CurrentRole = mRoleBase;
            CurrentRole.RoleAnimation.SetCurrentAniamtionByName(RoleAnimationName.Idle, true);
            mRoleBase.RoleAnimation.AddCompleteListener(OnCompleteAnimation);
            mRoleBase.RoleAnimation.AddEventListener(OnAnimationEvent);
        }

        public override void Update(RoleBase mRoleBase, float deltaTime)
        {
            base.Update(mRoleBase, deltaTime);
            addTime += deltaTime;
            if (addTime > skillCDTime)
            {
                if (mRoleBase.TeamId == TeamTypeEnum.Hero)
                {
                    if (GameRoleMgr.instance.CurrentPlayerMp.Value > MP)
                    {
                        GameRoleMgr.instance.CurrentPlayerMp.Value -= MP;
                        OnceAttack(mRoleBase);
                    }
                }
                else
                {
                    OnceAttack(mRoleBase);
                }
              
            }
        }

        public override void Exit(RoleBase mRoleBase)
        {
            base.Exit(mRoleBase);
            mRoleBase.RoleAnimation.RemoveCompleteListener(OnCompleteAnimation);
            mRoleBase.RoleAnimation.RemoveEventListener(OnAnimationEvent);
        }

        protected virtual void OnceAttack(RoleBase mRoleBase)
        {
            TargetRole = mRoleBase.RoleSearchTarget.Target;
            Vector3 dir = (TargetRole.RoleTransform.position - mRoleBase.RoleTransform.position).normalized;
            mRoleBase.RoleMoveMoment.SetOffesetVector3(dir);
            addTime = 0;
            if (TargetRole == null)
            {
                mRoleBase.SetRoleActionState(ActorStateEnum.Idle);
                return;
            }
            // mRoleBase.RolePropertyValue.SetMp(MP);
            AnimationEntry = mRoleBase.RoleAnimation.SetCurrentAniamtionByName(AnimationName);
            CurrentSkillData.UseSkill();
        }

        protected virtual void HitTarget(RoleBase mRoleBase)
        {
        }

        protected virtual void OnCompleteAnimation(TrackEntry animationEntry)
        {
            if (animationEntry.animation.name == AnimationName)
            {
                CurrentRole.RoleAnimation.SetCurrentAniamtionByName(RoleAnimationName.Idle, true);
            }
        }

        protected virtual void OnAnimationEvent(TrackEntry animationEntry, Event e)
        {
            Debug.LogError("OnAnimationEvent  " + e.data.name);
            if (animationEntry.animation.name == AnimationName)
            {
                HitTarget(CurrentRole);
            }
        }
    }
}
