
//功能： 读取的Xml数据管理
//创建者: 胡海辉
//创建时间：


using System;
using UnityEngine;
using System.Xml;
using Assets.Script.Battle.BattleData;

namespace Assets.Script.Utility
{
    public class ReadBattleXmlDataMgr
    {

        public static XmlData GetXmlData(XmlName name)
        {
            switch (name)
            {
                case XmlName.RoleData:
                    return new RoleData();
                case XmlName.RolePropertyData:
                    return new RolePropertyData();
                case XmlName.SkillData:
                    return new SkillData();
                case XmlName.BufferData:
                    return new RolePropertyData();
                case XmlName.MapSceneLevel:
                    return new MapSceneLevelData();
                case XmlName.CreateEnemyData:
                    return new CreateEnemyData();
                default: return new XmlData();
            }
        }
    }
}
