
//功能： 读取的Xml数据管理
//创建者: 胡海辉
//创建时间：


using System;
using UnityEngine;
using System.Xml;
using Assets.Script.Battle.BattleData;

namespace Assets.Script.Utility
{

    public class ReadCommonlXmlDataMgr
    {

        public static XmlData GetXmlData(XmlName name)
        {
            switch (name)
            {
                case XmlName.Equipment:
                    return new EquipmentData();
                case XmlName.EquipProperty:
                    return new EquipPropertyData();
                case XmlName.EquipBaseProperty:
                    return new EquipBasePropertyData();
                default: return new XmlData();
            }
        }
    }
}
