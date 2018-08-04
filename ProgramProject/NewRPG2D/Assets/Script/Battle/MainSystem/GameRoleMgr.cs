using System.Collections.Generic;
using Assets.Script.Tools;
using Assets.Script.Utility;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class GameRoleMgr : TSingleton<GameRoleMgr>
    {
        public List<RoleBase> RolesList = new List<RoleBase>(10);
        public List<RoleBase> RolesEnemyList = new List<RoleBase>(10);
        public List<RoleBase> RolesHeroList = new List<RoleBase>(10);
        public Dictionary<ushort, RoleBase> RoleDic = new Dictionary<ushort, RoleBase>();

        public void ClearAllRole()
        {
            RolesList.Clear();
            RolesEnemyList.Clear();
            RolesHeroList.Clear();
            RoleDic.Clear();
        }

        public void AddRole(GameObject perfab, bool isMaster = false)
        {
            if (perfab == null)
                return;
        }

        public void AddMonsterRole(string perfabpath, string indexName, Transform transform, Vector3 mPosition,
            ushort instanceId, int roleId, float angle)
        {
            string itemModelName = StaticAndConstParamter.ITEM_PATH_NAME + "BattleRole/Monster";
            RoleRender roleMono = SetRoleTransform(itemModelName, indexName, transform, mPosition, angle);
            roleMono.name = StringHelper.instance.GetPerfabName(indexName, instanceId);
            RoleInfo info = new RoleInfo();
            EnemyHeroRole role = new EnemyHeroRole();
            info.TeamId = TeamTypeEnum.Monster;
            info.InstanceId = instanceId;
            info.RoleId = roleId;
            info.RoleType = RoleTypeEnum.Monster;
            role.SetRoleInfo(info, roleMono);
            RolesList.Add(role);
            RolesEnemyList.Add(role);
            RoleDic[instanceId] = role;
        }

        public void AddHeroRole(string perfabpath, string indexName, Transform transform, Vector3 mPosition, ushort instanceId, int roleId, float angle)
        {
            string itemModelName = StaticAndConstParamter.ITEM_PATH_NAME + "BattleRole/Monster";
            RoleRender roleMono = SetRoleTransform(itemModelName, indexName, transform, mPosition, angle);
            roleMono.name = StringHelper.instance.GetPerfabName(indexName, instanceId);
            Debug.Log("AddHeroRole addrole:" + instanceId);
            RoleInfo info = new RoleInfo();
            MainHeroRole role = new MainHeroRole();
            info.TeamId = TeamTypeEnum.Hero;
            info.InstanceId = instanceId;
            info.RoleId = roleId;
            info.RoleType = RoleTypeEnum.Hero;
            role.SetRoleInfo(info, roleMono);
            RolesList.Add(role);
            RolesHeroList.Add(role);
            RoleDic[instanceId] = role;
        }

        public RoleBase GetRole(ushort inatancdid)
        {
            if (RoleDic.ContainsKey(inatancdid) == false)
            {
                Debug.LogError("GetRole =instanceId= " + inatancdid);
                return null;
            }
            return RoleDic[inatancdid];
        }

        private RoleRender SetRoleTransform(string perfabpath, string indexName, Transform transform, Vector3 mPosition, float angle)
        {
            GameObject perfab = ResourcesLoadMgr.instance.PopObjFromPool(perfabpath, indexName);
            perfab.transform.parent = transform;
            perfab.transform.position = mPosition;
            perfab.transform.localEulerAngles = new Vector3(angle, 0);
            return perfab.GetComponent<RoleRender>();
        }

    }
}
