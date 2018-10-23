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

        protected override void HitTarget(RoleBase mRoleBase)
        {
            HurtInfo info = new HurtInfo();
            info.HurtValue = mRoleBase.RolePropertyValue.Damage;
            info.AttackRole = mRoleBase;
            info.TargeRole = TargetRole;
            info.HurtType = mRoleBase.RolePropertyValue.HurtType;
            mRoleBase.RoleWeapon.TriggerBuff(TirggerTypeEnum.Attack, ref info);
            if (mRoleBase.AttackType == AttackTypeEnum.ShortRange)
            {
                mRoleBase.RoleDamageMoment.HurtDamage(ref info);
                FxManger.instance.PlayFx("SwordSlashBlue", mRoleBase.RoleTransform);
                FxManger.instance.PlayFx("MagicSwordHitRed", TargetRole.RoleTransform);
            }
            else
            {
                string aiObjectName = mRoleBase.RoleTransform.name + "_aiObjcet";
                AIObjectRenderer aiObject = GameRoleMgr.instance.SetRoleTransform<AIObjectRenderer>("BattleAIObject/AIObjectCommon", aiObjectName, 1,
                    mRoleBase.RoleTransform.parent, mRoleBase.RoleTransform.position + mRoleBase.RoleTransform.up, 0);
                aiObject.SetAIObjectInfo(ref info, new AIObjectInfo { MoveSpeed = 10 });
            }
            base.HitTarget(mRoleBase);
        }
    }
}
