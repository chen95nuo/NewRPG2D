using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class GameRoleMgr : TSingleton<GameRoleMgr>
    {
        public List<RoleBase> RolesList = new List<RoleBase>(10);
        public Dictionary<ushort, RoleBase> RoleDic = new Dictionary<ushort, RoleBase>();

        public void AddRole(GameObject perfab, bool isMaster = false)
        {
            if (perfab == null)
                return;
        }

        public void AddMonsterRole(string perfabpath, string indexName, Transform transform, Vector3 mPosition,
            ushort instanceId, uint playerId, float angle)
        {

        }

        public void AddHeroRole(string perfabpath, string indexName, Transform transform, Vector3 mPosition, ushort instanceId, uint playerId, float angle, bool isMaster = false)
        {
            string itemModelName = StaticAndConstParamter.ITEM_PATH_NAME + "Role/Hero/HeroRole";// + perfabpath ;
            GameObject perfab = ResourcesLoadSys.instance.PopObjFromPool(itemModelName, indexName);
            Debug.Log("PlayerController addrole:" + instanceId);
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
    }
}
