using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleSkill1State : RoleBaseSkillState
    {
        public override string AnimationName
        {
            get { return RoleAnimationName.Skill1; }
        }

        public override void Enter(RoleBase mRoleBase)
        {
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
