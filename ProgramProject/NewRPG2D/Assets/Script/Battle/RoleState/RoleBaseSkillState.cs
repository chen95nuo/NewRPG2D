using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleBaseSkillState : FsmState<RoleBase>
    {
        public virtual string AnimationName
        {
            get { return ""; }
        }

        public override void Enter(RoleBase mRoleBase)
        {
            base.Enter(mRoleBase);
            mRoleBase.MonoRoleRender.PlayAnimation(AnimationName);
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
