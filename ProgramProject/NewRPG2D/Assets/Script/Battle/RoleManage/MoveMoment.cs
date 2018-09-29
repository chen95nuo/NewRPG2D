using System.Collections.Generic;
using Assets.Script.Utility;
using Pathfinding;
using Pathfinding.RVO;
using Pathfinding.Util;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class MoveMoment
    {
        public bool reachedEndOfPath { get; protected set; }
        private Transform roleTransform, targetTransform;
        private RoleBase mCurrentRole;
        private Vector3 currentTargetPosition;
        private SwitchRoleActionParam switchActionParam;
        private float pickNextWaypointDist = 2;
        private float endReachedDistance = 0.2F;
        private float slowdownDistance = 0.6F;

        private RVOController moveController;
        private Seeker moveSeeker;
        private AIPath aiPath;

        protected PathInterpolator interpolator = new PathInterpolator();
        private IMovementPlane movementPlane = GraphTransform.identityTransform;
        private float remainingDistance
        {
            get
            {
                return interpolator.valid ? interpolator.remainingDistance + movementPlane.ToPlane(interpolator.position - roleTransform.position).magnitude : float.PositiveInfinity;
            }
        }

        private Vector3 nextPosition = Vector3.zero;
        private Vector3 steeringTarget
        {
            get
            {
                return interpolator.valid ? interpolator.position : roleTransform.position;
            }
        }

        private float maxSpeed = 10;
        private Vector2 velocity2D;
        private float repathRate = 0.5f;
        private float moveNextDist = 1;
        private float nextRepath = 0;
        private Vector3 _target;
        private bool canSearchAgain = true;
        private Path path = null;
        private bool finishMove = false;
        protected Vector2 lastDeltaPosition;
        protected float lastDeltaTime, attackDistance;

        public void SetCurrentRole(RoleBase mRole, RVOController controller, Seeker seeker)
        {
            mCurrentRole = mRole;
            roleTransform = mRole.RoleTransform;
          //  switchActionParam = new SwitchRoleActionParam(mCurrentRole, RoleActionEnum.Idle);
            moveController = controller;
            moveSeeker = seeker;

        }

        public void InitData()
        {
            attackDistance = mCurrentRole.RoleSkill.GetSkillUseDataBySkilSlot(SkillSlotTypeEnum.NormalAttack).AttackRange;
        }

        public void Update(float deltaTime)
        {
            if (Time.time >= nextRepath && canSearchAgain)
            {
                RecalculatePath();
            }
            var currentPosition = roleTransform.position;
            maxSpeed = mCurrentRole.RolePropertyValue.MoveSpeed;
            interpolator.MoveToCircleIntersection2D(currentPosition, pickNextWaypointDist, movementPlane);
            var dir = movementPlane.ToPlane(steeringTarget - currentPosition);
            float distanceToEnd = dir.magnitude + Mathf.Max(0, interpolator.remainingDistance);

            var prevTargetReached = reachedEndOfPath;
            reachedEndOfPath = distanceToEnd <= endReachedDistance && interpolator.valid;
            if (!prevTargetReached && reachedEndOfPath) OnTargetReached();

            if (moveController != null && moveController.enabled)
            {
                moveController.SetTarget(_target, maxSpeed, maxSpeed);
            }
            var delta2D = lastDeltaPosition = CalculateDeltaToMoveThisFrame(movementPlane.ToPlane(currentPosition), distanceToEnd, deltaTime);
            nextPosition = currentPosition + movementPlane.ToWorld(delta2D, 0);
            roleTransform.position = nextPosition;
        }

        public void Dispose()
        {

        }

        public void CancelCurrentPathRequest()
        {
            if (moveSeeker != null) moveSeeker.CancelCurrentPathRequest();
        }

        private void RecalculatePath()
        {
            canSearchAgain = false;
            nextRepath = Time.time + repathRate * (Random.value + 0.5f);
            if (targetTransform == null)
            {
                _target = currentTargetPosition;
            }
            else
            {
                var dir = (targetTransform.position - roleTransform.position).normalized;
                _target = targetTransform.position;

            }
            moveSeeker.StartPath(roleTransform.position, _target, OnPathComplete);
        }

        private void OnPathComplete(Path _p)
        {
            ABPath p = _p as ABPath;

            canSearchAgain = targetTransform != null;

            p.Claim(this);

            if (p.error)
            {
                if (p.error)
                    p.Release(this);
                return;
            }
            if (path != null) path.Release(this);
            path = p;
            if (path.vectorPath.Count == 1) path.vectorPath.Add(path.vectorPath[0]);
            interpolator.SetPath(path.vectorPath);

            var graph = AstarData.GetGraph(path.path[0]) as ITransformedGraph;
            movementPlane = graph != null ? graph.transform : new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 270, 90), Vector3.one));

            reachedEndOfPath = false;
            interpolator.MoveToLocallyClosestPoint((GetFeetPosition() + p.originalStartPoint) * 0.5f);
            interpolator.MoveToLocallyClosestPoint(GetFeetPosition());
            interpolator.MoveToCircleIntersection2D(roleTransform.position, pickNextWaypointDist, movementPlane);

            var distanceToEnd = remainingDistance;
            if (distanceToEnd <= endReachedDistance)
            {
                reachedEndOfPath = true;
                OnTargetReached();
            }
        }

        private Vector2 CalculateDeltaToMoveThisFrame(Vector2 position, float distanceToEndOfPath, float deltaTime)
        {
            if (moveController != null && moveController.enabled)
            {
                return movementPlane.ToPlane(moveController.CalculateMovementDelta(movementPlane.ToWorld(position, 0), deltaTime));
            }
            return Vector2.ClampMagnitude(velocity2D * deltaTime, distanceToEndOfPath);
        }

        private Vector3 GetFeetPosition()
        {
            return roleTransform.position + (roleTransform.rotation * Vector3.up) * (moveController.center - moveController.height * 0.5f);
        }


        private void OnTargetReached()
        {
            mCurrentRole.FinishMoveToPoint = true;
            //if(targetTransform == null)
            //mCurrentRole.RoleSearchTarget.SetTarget(null);
        }

        public void SetTargetPosition(Vector3 targetPosition)
        {
            mCurrentRole.FinishMoveToPoint = false;
            targetTransform = null;
            currentTargetPosition = targetPosition;
            Vector3 dir = (currentTargetPosition - roleTransform.position).normalized;
            finishMove = false;
            SetOffesetVector3(dir);
            RecalculatePath();
        }

        public void SetTargetPositionAndChangeActionState(Vector3 targetPosition)
        {
            SetTargetPosition(targetPosition);
            mCurrentRole.IsCanInterrput = true;
            mCurrentRole.SetRoleActionState(ActorStateEnum.Run);
        }

        public void SetTargetTranform(Transform target)
        {
            targetTransform = target;
            SetTargetTranformDir();
            mCurrentRole.IsCanInterrput = true;
            finishMove = false;
            RecalculatePath();
            mCurrentRole.SetRoleActionState(ActorStateEnum.Run);
        }

        public void SetTargetTranformDir()
        {
            Vector3 dir = (targetTransform.position - roleTransform.position).normalized;
            SetOffesetVector3(dir);
        }

        public void SetTargetMinDistance(float minDistance)
        {
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
