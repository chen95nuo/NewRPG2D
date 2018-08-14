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
                        EventManager.instance.SendEvent(EventDefineEnum.ClickMyRole, cacheRole.CurrentRole);
                    }
                    else
                    {
                        EventManager.instance.SendEvent(EventDefineEnum.ClickEnemyRole, cacheRole.CurrentRole);
                    }
                    DebugHelper.Log(" cacheRole " + cacheRole.name);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                RoleRender role = GetTouchRoleRender();
               
                if (role == null && cacheRole != null)
                {
                    cacheRole.Move(ScreenToWorldPoint(Input.mousePosition));
                }
                else if(role!=null && cacheRole != null)
                {
                    DebugHelper.Log("role = " + role.name);
                    cacheRole.ChangeTarget(role.CurrentRole);
                }

                cacheRole = null;
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
