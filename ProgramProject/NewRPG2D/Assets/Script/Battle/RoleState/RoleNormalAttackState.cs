using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleNormalAttackState  : RoleBaseSkillState
    {
        public override string AnimationName
        {
            get { return RoleAnimationName.NormalAttack; }
        }

        public override void Enter(RoleBase mRoleBase)
        {
            CurrentSkillData = mRoleBase.RoleSkill.GetSkillUseDataBySkilSlot(SkillSlotTypeEnum.NormalAttack);
            base.Enter(mRoleBase);
        }

        public override void Update(RoleBase mRoleBase, float deltaTime)
        {
            base.Update(mRoleBase, deltaTime);
        }

        public override void Exit(RoleBase mRoleBase)
        {
            base.Exit(mRoleBase);
        }

        protected override void OnceAttack(RoleBase mRoleBase)
        {
            base.OnceAttack(mRoleBase);
            HurtInfo info = new HurtInfo();
            info.HurtValue = 10;
            info.AttackRole = mRoleBase;
            info.TargeRole = TargetRole;
            info.HurtType = HurtTypeEnum.Physic;
            if (mRoleBase.AttackType == AttackTypeEnum.ShortRange)
            {
                mRoleBase.RoleDamageMoment.HurtDamage(ref info);
            }
            else
            {
                string aiObjectName = mRoleBase.RoleTransform.name + "_aiObjcet";
                AIObjectRenderer aiObject = GameRoleMgr.instance.SetRoleTransform<AIObjectRenderer>("BattleAIObject/AIObjectCommon", aiObjectName, 1,
                    mRoleBase.RoleTransform.parent, mRoleBase.RoleTransform.position, 0);
                aiObject.SetAIObjectInfo(ref info, new AIObjectInfo {MoveSpeed = 10});
            }
            //扣血
        }
    }
}
