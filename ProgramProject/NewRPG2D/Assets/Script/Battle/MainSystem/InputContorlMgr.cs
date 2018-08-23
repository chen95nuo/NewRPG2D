using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class InputContorlMgr : TSingleton<InputContorlMgr>
    {
        private Ray2D ray;
        private RaycastHit2D hitObj;
        private Camera mainCamera;
        private LayerMask touchMask;
        private RoleRender cacheRole;

        public override void Init()
        {
            base.Init();
        }

        public void SetMainCamera(Camera mCamera)
        {
            mainCamera = mCamera;
        }

        public void SetLayMask(LayerMask mask)
        {
            touchMask = mask;
        }

        private SelectTargetParam selectTargetData =new SelectTargetParam();
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (Input.GetMouseButtonDown(0))
            {
                cacheRole = GetTouchRoleRender();
                if (cacheRole != null)
                {
                    if (cacheRole.CurrentRole.TeamId == TeamTypeEnum.Hero)
                    {
                        EventManager.instance.SendEvent(EventDefineEnum.ClickMyRole, cacheRole.CurrentRole.InstanceId);
                        EventManager.instance.SendEvent(EventDefineEnum.DragStart, cacheRole);
                    }
                    else
                    {
                        EventManager.instance.SendEvent(EventDefineEnum.ClickEnemyRole, cacheRole.CurrentRole);
                    }
                    DebugHelper.Log(" cacheRole " + cacheRole.name);
                }
                else
                {
                    EventManager.instance.SendEvent<RoleBase>(EventDefineEnum.ClickEnemyRole, null);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                EventManager.instance.SendEvent<RoleRender>(EventDefineEnum.DragEnd, cacheRole);
                if (cacheRole != null && cacheRole.CurrentRole.TeamId == TeamTypeEnum.Hero)
                {
                    RoleRender role = GetTouchRoleRender();
                    if (role != null && cacheRole.CurrentRole.TeamId != TeamTypeEnum.Hero)
                    {
                        DebugHelper.Log("role = " + role.name);
                        cacheRole.ChangeTarget(role.CurrentRole);
                    }
                    else
                    {
                        cacheRole.Move(ScreenToWorldPoint(Input.mousePosition));
                    }

                    cacheRole = null;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (cacheRole != null && cacheRole.CurrentRole.TeamId == TeamTypeEnum.Hero)
                {
                    RoleRender role = GetTouchRoleRender();
                    selectTargetData.OriginalTransform = cacheRole.transform;
                    selectTargetData.TargetTransform = role != null && role.CurrentRole.TeamId != TeamTypeEnum.Hero ? role.transform : null;
                    EventManager.instance.SendEvent(EventDefineEnum.Draging, selectTargetData);
                }
            }
        }

        public RoleRender GetTouchRoleRender()
        {
            hitObj = Physics2D.Raycast(ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, touchMask);
            if (hitObj.transform != null)
            {
                RoleRender role = hitObj.transform.GetComponent<RoleRender>();
                if (role != null)
                {
                    return role;
                }
            }
            return null;
        }

        public Vector2 ScreenToWorldPoint(Vector2 pos)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return mousePos;
        }
    }
}
