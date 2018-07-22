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
            CurrentSkillData = mRoleBase.RoleSkill.GetSkillDataBySkilSlot(SkillSlotTypeEnum.NormalAttack);
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
            mRoleBase.RoleDamageMoment.HurtDamage(ref info);
            //扣血
        }
    }
}
