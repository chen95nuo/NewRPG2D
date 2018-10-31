﻿using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleWinState : FsmState<RoleBase>
    {
    
        public override void Enter(RoleBase mRoleBase)
        {
            base.Enter(mRoleBase);
            mRoleBase.RoleAnimation.SetCurrentAniamtionByName(RoleAnimationName.Win, true);
            mRoleBase.IsCanControl = false;
            // mRoleBase.RoleMoveMoment.SetTargetPosition(mRoleBase.RoleTransform.position);
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
