using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleDeathState : FsmState<RoleBase>
    {
    
        public override void Enter(RoleBase mRoleBase)
        {
            base.Enter(mRoleBase);
            mRoleBase.Death();
            mRoleBase.RoleAnimation.SetCurrentAniamtionByName(RoleAnimationName.Death, false);
            //ResourcesLoadMgr.instance.PushObjIntoPool(mRoleBase.RoleTransform.name, mRoleBase.RoleTransform.gameObject);
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
