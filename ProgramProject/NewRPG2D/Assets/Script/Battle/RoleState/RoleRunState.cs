using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleRunState : FsmState<RoleBase>
    {
    
        public override void Enter(RoleBase mRoleBase)
        {
            base.Enter(mRoleBase);
            mRoleBase.RoleAnimation.SetCurrentAniamtionByName(RoleAnimationName.Move, true);
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
