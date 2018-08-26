using UnityEngine;

namespace Assets.Script.Battle.RoleState
{
    public class RoleHitState : FsmState<RoleBase>
    {
        private float addTime;
        private Vector3 originalPosition;
        private bool changedColor;
        private Vector3 hitDir;
        private float hitTime = 0.15f;

        public override void Enter(RoleBase mRoleBase)
        {
            base.Enter(mRoleBase);
            addTime = 0;
            mRoleBase.RoleAnimation.SetCurrentAniamtionByName(RoleAnimationName.Hit);
            mRoleBase.IsCanControl = false;
            originalPosition = mRoleBase.RoleTransform.position;
            //DebugHelper.LogError("  Hit  " + mRoleBase.ToString());
            //  mRoleBase.RoleAnimation.ChangeAniamtionName(RoleAnimationName.Hit);
            mRoleBase.ChangeRoleColor(Color.red);
            changedColor = true;
            RoleBase attackRole = mRoleBase.RolePropertyValue.AttackRole;

            if (attackRole != null)
            {
                hitDir = (attackRole.RoleTransform.position - originalPosition).normalized;
            }
            else
            {
                hitDir = mRoleBase.RoleTransform.right;
            }
          //  mRoleBase.RoleMoveMoment.SetTargetPosition(originalPosition + hitDir * (-1 * hitTime));
        }


        public override void Update(RoleBase mRoleBase, float deltaTime)
        {
            base.Update(mRoleBase, deltaTime);

            if (addTime > hitTime)
            {
                mRoleBase.ChangeRoleColor(Color.white);
                mRoleBase.IsCanControl = true;
                mRoleBase.RestartRoleLastActorState();
            }
            else
            {
                addTime += Time.deltaTime;
                mRoleBase.RoleTransform.position += hitDir * (-1f * Time.deltaTime);

            }
            //if (changedColor && addTime > 0.1f)
            //{
            //    changedColor = false;
            //    mRoleBase.ChangeRoleColor(Color.white);
            //}
        }

        public override void Exit(RoleBase mRoleBase)
        {
            base.Exit(mRoleBase);
            //  mRoleBase.RoleTransform.position = originalPosition;
        }

    }
}
