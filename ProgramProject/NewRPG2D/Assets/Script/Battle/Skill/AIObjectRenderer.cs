using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Timer;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class AIObjectRenderer : MonoBehaviour
    {

        private float moveSpeed;
        private RoleBase originalRole;
        private Transform targeTransform;
        private HurtInfo AIObjectHurtInfo;

        private bool canMove;

        public void Awake()
        {
            canMove = false;
        }

        public void SetAIObjectInfo(ref HurtInfo hurtInfo, AIObjectInfo moveInfo)
        {
            originalRole = hurtInfo.AttackRole;
            targeTransform = hurtInfo.TargeRole.RoleTransform;
            moveSpeed = moveInfo.MoveSpeed;
            AIObjectHurtInfo = hurtInfo;
            canMove = true;
        }

        public void Update()
        {
            if (canMove == false)
            {
                return;
            }

            Vector3 moveDir = (targeTransform.position - originalRole.RoleTransform.position).normalized;
            transform.right = moveDir;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            if (Mathf.Abs(transform.position.x - targeTransform.position.x) < 1)
            {
                HitTarget();
            }
            else if (Mathf.Abs(transform.position.x - targeTransform.position.x) > 100)
            {
                canMove = false;
                ResourcesLoadMgr.instance.PushObjIntoPool(transform.name, gameObject);
            }
        }

        private Transform hitObject;
        private void HitTarget()
        {
            originalRole.RoleDamageMoment.HurtDamage(ref AIObjectHurtInfo);
            canMove = false;
            ResourcesLoadMgr.instance.PushObjIntoPool(transform.name, gameObject);
            FxManger.instance.PlayFx("MagicSwordHitRed", targeTransform);
        }

    }
}
