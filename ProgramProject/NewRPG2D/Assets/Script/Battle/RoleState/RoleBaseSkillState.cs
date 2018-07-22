using Assets.Script.Battle.BattleData;
using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleBaseSkillState : FsmState<RoleBase>
    {
        public virtual string AnimationName
        {
            get { return ""; }
        }

        public RoleBase TargetRole;

        public SkillData CurrentSkillData;
        public float skillCDTime;
        public int MP;
        public int CD;
        public SkillTypeEnum SkillType;
        public int TargetCount;
        public int EffectId1;
        public int EffectId2;
        public int EffectId3;
        public int EffectId4;

        protected float addTime;

        public override void Enter(RoleBase mRoleBase)
        {
            base.Enter(mRoleBase);
            OnceAttack(mRoleBase);
            skillCDTime = CurrentSkillData.CD;
            MP = CurrentSkillData.MP;
            SkillType = CurrentSkillData.SkillType;
            TargetCount = CurrentSkillData.TargetCount;
            EffectId1 = CurrentSkillData.EffectId1;
            EffectId2 = CurrentSkillData.EffectId2;
            EffectId3 = CurrentSkillData.EffectId3;
            EffectId4 = CurrentSkillData.EffectId4;
            addTime = 0;
        }

        public override void Update(RoleBase mRoleBase, float deltaTime)
        {
            base.Update(mRoleBase, deltaTime);
            addTime += deltaTime;
            if (addTime > CD && mRoleBase.RoleAnimation.IsComplete() && mRoleBase.RolePropertyValue.RoleMp > MP)
            {
                OnceAttack(mRoleBase);
            }
        }

        public override void Exit(RoleBase mRoleBase)
        {
            base.Exit(mRoleBase);
        }

        protected virtual void OnceAttack(RoleBase mRoleBase)
        {
            TargetRole = mRoleBase.RoleSearchTarget.Target;
            addTime = 0;
            if (TargetRole == null)
            {
                mRoleBase.SetRoleActionState(ActorStateEnum.Idle);
                return;
            }
          
            mRoleBase.RolePropertyValue.SetMp(MP);
            mRoleBase.RoleAnimation.SetCurrentAniamtionByName(AnimationName);
        }
    }
}
