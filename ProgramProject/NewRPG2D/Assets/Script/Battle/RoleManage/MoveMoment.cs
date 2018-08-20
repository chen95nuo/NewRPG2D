using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class MoveMoment
    {
        private Transform roleTransform, targetTransform;
        private RoleBase mCurrentRole;
        private Vector3 addOffesetVector3, currentTargetPosition;
        private SwitchRoleActionParam switchActionParam;
        private float minMoveDistance = 1;
        private bool finishMove = false;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            roleTransform = mRole.RoleTransform;
            switchActionParam = new SwitchRoleActionParam(mCurrentRole, RoleActionEnum.Idle);
            minMoveDistance = 1;
        }

        private Vector3 nextPosition = Vector3.zero;
        public void Update(float deltaTime)
        {
            if (targetTransform == null)
            {
                if ((roleTransform.position - currentTargetPosition).magnitude < StaticAndConstParamter.MOVE_SPEED_MIN_THRESHOLD)
                {
                    mCurrentRole.FinishMoveToPoint = true;
                    return;
                }
                SetTargetPosition(currentTargetPosition);
            }
            else
            {
                if ((roleTransform.position - targetTransform.position).magnitude < minMoveDistance)
                {
                    return;
                }
                SetTargetTranformDir();
            }

            nextPosition = roleTransform.position + addOffesetVector3 * (deltaTime * mCurrentRole.RolePropertyValue.MoveSpeed * 0.8f);
            //if (MapColliderMgr.instance.CheckCollider(nextPosition))
            //{
                roleTransform.position = nextPosition;
            //}
        }

        public void Dispose()
        {

        }

        public void SetTargetPosition(Vector3 targetPosition)
        {
            mCurrentRole.FinishMoveToPoint = false;
            targetTransform = null;
            currentTargetPosition = targetPosition;
            Vector3 dir = (currentTargetPosition - roleTransform.position).normalized;
            SetOffesetVector3(dir);
        }

        public void SetTargetPositionAndChangeActionState(Vector3 targetPosition)
        {
            SetTargetPosition(targetPosition);
            mCurrentRole.SetRoleActionState(ActorStateEnum.Run);
        }

        public void SetTargetTranform(Transform target)
        {
            targetTransform = target;
            SetTargetTranformDir();
            mCurrentRole.SetRoleActionState(ActorStateEnum.Run);
        }

        public void SetTargetTranformDir()
        {
            Vector3 dir = (targetTransform.position - roleTransform.position).normalized;
            SetOffesetVector3(dir);
        }

        public void SetTargetMinDistance(float minDistance)
        {
            minMoveDistance = minDistance;
        }

        public void SetOffesetVector3(Vector3 OffesetVec)
        {
            if (OffesetVec.x < -StaticAndConstParamter.JOYSTICK_TIME)
            {
                roleTransform.right = Vector3.left;
            }
            else if (OffesetVec.x > StaticAndConstParamter.JOYSTICK_TIME)
            {
                roleTransform.right = Vector3.right;
            }

            addOffesetVector3 = OffesetVec;

            //if (addOffesetVector3.magnitude < StaticAndConstParamter.MOVE_SPEED_MIN_THRESHOLD)
            //{
            //    switchActionParam.NextAction = RoleActionEnum.Idle;
            //}
            //else
            //{
            //    switchActionParam.NextAction = RoleActionEnum.Run;
            //}

            //EventManager.instance.SendEvent(EventDefineEnum.SwitchRoleAction, switchActionParam);
        }

        private void CheckOffesetVail(ref Vector3 OffesetVec)
        {
            if ((OffesetVec.x < 0 && roleTransform.position.x < -StaticAndConstParamter.MAX_MOVE_X) || 
                (OffesetVec.x > 0 && roleTransform.position.x > StaticAndConstParamter.MAX_MOVE_X))
            {
                OffesetVec.x = 0.0f;
            }
            if ((OffesetVec.y < 0 && roleTransform.position.y < 0) ||
                (OffesetVec.y > 0 && roleTransform.position.y > StaticAndConstParamter.MAX_MOVE_Y))
            {
                OffesetVec.y = 0.0f;
            }
            if ((OffesetVec.z < 0 && roleTransform.position.z < -StaticAndConstParamter.MAX_MOVE_Z) || 
                (OffesetVec.z > 0 && roleTransform.position.z > 0))
            {
                OffesetVec.z = 0.0f;
            }
        }

    }
}
