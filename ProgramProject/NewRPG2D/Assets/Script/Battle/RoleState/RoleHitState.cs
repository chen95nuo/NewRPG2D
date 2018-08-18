using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleHitState : FsmState<RoleBase>
    {
        private float addTime;
        private Vector3 originalPosition;
        public override void Enter(RoleBase mRoleBase)
        {
            base.Enter(mRoleBase);
            addTime = 0;
            mRoleBase.RoleAnimation.SetCurrentAniamtionByName(RoleAnimationName.Hit);
            mRoleBase.IsCanControl = false;
            originalPosition = mRoleBase.RoleTransform.position;
            //DebugHelper.LogError("  Hit  " + mRoleBase.ToString());
        }

        public override void Update(RoleBase mRoleBase, float deltaTime)
        {
            base.Update(mRoleBase, deltaTime);
         
            if (addTime > 0.1f)
            {
                mRoleBase.IsCanControl = true;
                mRoleBase.RestartRoleLastActorState();
            }
            else
            {
                addTime += Time.deltaTime;
                mRoleBase.RoleTransform.position += mRoleBase.RoleTransform.right * (-2f * Time.deltaTime);
            }

        }

        public override void Exit(RoleBase mRoleBase)
        {
            base.Exit(mRoleBase);
            mRoleBase.RoleTransform.position = originalPosition;
        }

    }
}
