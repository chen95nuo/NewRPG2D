using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class MoveMoment
    {
        private Transform roleTransform;
        private RoleBase mCurrentRole;
        private Vector3 addOffesetVector3;
        private SwitchRoleActionParam switchActionParam;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            roleTransform = mRole.RoleTransform;
            switchActionParam=new SwitchRoleActionParam(mCurrentRole, RoleActionEnum.Idle);
        }

        private Vector3 nextPosition = Vector3.zero;
        public void Update(float deltaTime)
        {
            if (addOffesetVector3.magnitude < StaticAndConstParamter.MOVE_SPEED_MIN_THRESHOLD)
            {
                return;
            }
            nextPosition = roleTransform.position + addOffesetVector3 * (deltaTime * mCurrentRole.RolePropertyValue.MoveSpeed);
            if (MapColliderMgr.instance.CheckCollider(nextPosition))
            {
                roleTransform.position = nextPosition;
            }
        }

        public void Dispose()
        {

        }

        public void SetOffesetVector3(Vector3 OffesetVec)
        {
            if (OffesetVec.x < -StaticAndConstParamter.JOYSTICK_TIME)
            {
                roleTransform.right = Vector3.right;
            }
            else if (OffesetVec.x > StaticAndConstParamter.JOYSTICK_TIME)
            {
                roleTransform.right = Vector3.left;
            }
            //CheckOffesetVail(ref OffesetVec);
            addOffesetVector3 = OffesetVec;

            if (addOffesetVector3.magnitude < StaticAndConstParamter.MOVE_SPEED_MIN_THRESHOLD)
            {
                switchActionParam.NextAction = RoleActionEnum.Idle;
            }
            else
            {
                switchActionParam.NextAction = RoleActionEnum.Run;
            }

            EventManager.instance.SendEvent(EventDefineEnum.SwitchRoleAction, switchActionParam);
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
