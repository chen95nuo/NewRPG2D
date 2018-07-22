﻿using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleSkill2State : RoleBaseSkillState
    {
        public override string AnimationName
        {
            get { return RoleAnimationName.Skill2; }
        }

        public override void Enter(RoleBase mRoleBase)
        {
            CurrentSkillData = mRoleBase.RoleSkill.GetSkillDataBySkilSlot(SkillSlotTypeEnum.Skill2);
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

    }
}
