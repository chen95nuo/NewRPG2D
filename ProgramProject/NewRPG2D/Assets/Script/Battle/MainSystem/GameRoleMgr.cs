﻿using System.Collections.Generic;
using Assets.Script.Battle.BattleData;
using Assets.Script.Battle.BattleData.ReadData;
using Assets.Script.EventMgr;
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
        public TSimpleNotifier<int> RemianEnemyCount = new TSimpleNotifier<int>();
        public TSimpleNotifier<int> CurrentPlayerMp = new TSimpleNotifier<int>();

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

        public bool AddMonsterRole(string indexName, Transform transform, Vector3 mPosition, ushort instanceId, int roleId, float angle)
        {
            RoleRender roleMono = SetRoleTransform(roleId, indexName, instanceId, transform, mPosition, angle);
            if (roleMono == null)
            {
                return false;
            }

            RoleInfo info = new RoleInfo();
            EnemyHeroRole role = new EnemyHeroRole();
            info.TeamId = TeamTypeEnum.Monster;
            info.InstanceId = instanceId;
            info.RoleId = roleId;
            info.RoleType = RoleTypeEnum.Monster;
            role.SetRoleInfo(info, roleMono);
            RoleDetailData roleData = new RoleDetailData();
            roleData.InitData();
            roleData.Id = 100001;
            role.InitRoleBaseProperty(default(PropertyData), roleData);
            RolePropertyData currentRoleData = RolePropertyDataMgr.instance.GetXmlDataByItemId<RolePropertyData>(roleData.Id);
            roleData.BattleIconSpriteName = currentRoleData.SpriteName;
            if (currentRoleData.Profession == WeaponProfessionEnum.Fighter)
            {
                role.InitSkill(100100100, 100100100, 100100100);
            }
            else
            {
                role.InitSkill(100200100, 100100100, 100100100);
            }
            EventManager.instance.SendEvent<RoleBase>(EventDefineEnum.CreateRole, role);
            RolesList.Add(role);
            RolesEnemyList.Add(role);
            RoleDic[instanceId] = role;
            return true;
        }

        public bool AddHeroRole(string indexName, Transform transform, Vector3 mPosition, ushort instanceId, RoleDetailData roleData, float angle)
        {
            RoleRender roleMono = SetRoleTransform(roleData.Id, indexName, instanceId, transform, mPosition, angle);
            if (roleMono == null)
            {
                return false;
            }

            Debug.Log("AddHeroRole addrole:" + instanceId);
            RoleInfo info = new RoleInfo();
            MainHeroRole role = new MainHeroRole();
            info.TeamId = TeamTypeEnum.Hero;
            info.InstanceId = instanceId;
            info.RoleId = roleData.Id;
            info.RoleType = RoleTypeEnum.Hero;
            role.SetRoleInfo(info, roleMono);
            PropertyData data = default(PropertyData);
            int level = 1;
            data.RoleHp = 200 + level * 40;
            data.Damage = 10 + level * 1.1f;
            role.InitRoleBaseProperty(data, roleData);
            RolePropertyData currentRoleData = RolePropertyDataMgr.instance.GetXmlDataByItemId<RolePropertyData>(roleData.Id);
            roleData.BattleIconSpriteName = currentRoleData.SpriteName;
            if (currentRoleData.Profession == WeaponProfessionEnum.Fighter)
            {
                role.InitSkill(100100100, 100100100, 100100100);
            }
            else
            {
                role.InitSkill(100200100, 100100100, 100100100);
            }
            RolesList.Add(role);
            RolesHeroList.Add(role);
            RoleDic[instanceId] = role;
            //Debug.LogError(" AddHeroRole ");
            EventManager.instance.SendEvent<RoleBase>(EventDefineEnum.CreateRole, role);
            return true;
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

        private RoleRender SetRoleTransform(int roleId, string indexName, int instanceId, Transform transform, Vector3 mPosition, float angle)
        {
            RolePropertyData currentRoleData = RolePropertyDataMgr.instance.GetXmlDataByItemId<RolePropertyData>(roleId);
            if (currentRoleData == null)
            {
                return null;
            }
            string itemModelName = currentRoleData.PrefabName;
            return SetRoleTransform<RoleRender>(itemModelName, indexName, instanceId, transform, mPosition, angle);
        }

        public T SetRoleTransform<T>(string itemModelName, string indexName, int instanceId, Transform transform, Vector3 mPosition, float angle) where T : Component
        {
            indexName = StringHelper.instance.GetPerfabName(indexName, instanceId);
            itemModelName = StaticAndConstParamter.ITEM_PATH_NAME + itemModelName;
            GameObject perfab = ResourcesLoadMgr.instance.PopObjFromPool(itemModelName, indexName);
            perfab.name = indexName;
            perfab.transform.parent = transform;
            perfab.transform.position = mPosition;
            perfab.transform.localEulerAngles = new Vector3(angle, 0);
            perfab.transform.localScale = Vector3.one;
            return perfab.GetComponent<T>();
        }

        //private PropertyBaseData GetPropertyBaseData(RoleDetailData data)
        //{
        //    if (data == null)
        //    {
        //        DebugHelper.LogError("dont find role info");
        //        return default(PropertyBaseData);
        //    }
        //    PropertyBaseData propertyBaseData = new PropertyBaseData();
        //    // propertyBaseData.RoleProperty = data.AgileGrow
        //    float growValueMin = 0;
        //    float growValueMax = 0;
        //    float baseValue = data.Health;
        //    propertyBaseData.Hp = new PropertyAddtion(baseValue, growValueMin, growValueMax);

        //     baseValue = data.Defense;
        //    propertyBaseData.Defense = new PropertyAddtion(baseValue, growValueMin, growValueMax);

        //     baseValue = data.Agile;
        //    propertyBaseData.Prompt = new PropertyAddtion(baseValue, growValueMin, growValueMax);

        //     baseValue = data.Attack;
        //    propertyBaseData.Attack = new PropertyAddtion(baseValue, growValueMin, growValueMax);
        //    return propertyBaseData;
        //}
    }
}
