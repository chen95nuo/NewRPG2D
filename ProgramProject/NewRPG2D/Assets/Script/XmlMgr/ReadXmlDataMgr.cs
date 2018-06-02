
//功能： 读取的Xml数据管理
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using Assets.Script.Tools;
using System;
using UnityEngine;
using System.Xml;

namespace Assets.Script
{
    public class ReadXmlDataMgr : TSingleton<ReadXmlDataMgr>, IDisposable
    {
        /// <summary>
        /// 配置表名
        /// </summary>

        public enum XmlName
        {
            EnemyAI,
            PlayerData,
            AudioData,
            Max,
        }

        public XmlData GetXmlData(XmlName name) 
        {
            switch (name) 
            {
                case XmlName.AudioData: return new AudioData();
                case XmlName.PlayerData: return new PlayerData();
                default: return new XmlData();
            }
        }

        public string GetXmlPath(XmlName name)
        {
            string path;
            path = PlatformTools.m_FileRealPath + "/Config/" + name + ".xml";
            return path;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public int IntParse(XmlNode node, string name)
        {
            return (int)Parse(node, name, 0);
        }

        public string StrParse(XmlNode node, string name)
        {
            return (string)Parse(node, name, "");
        }

        public float FloatParse(XmlNode node, string name)
        {
            return (float)Parse(node, name, 0.0f);
        }

        public object Parse(XmlNode node, string name, object defaultValue)
        {
            return Convert.ChangeType(node.Attributes[name].Value, defaultValue.GetType());
        }
    }

    /// <summary>
    /// 敌人AI
    /// </summary>
    [Serializable]
    public class EnemyAIData
    {
        public int ID;
        public Vector3 startPos;
        public Vector3 endPos;
        public float moveSpeed = 3;
        public float stayTime = 2;
        public float aroundTime = 1;
        public float attackSpeed = 1;
        public float followSpeed = 4;
        public float runSpeed = 5;
        public float viewRange = 6;
        public float attackRange = 2;
        public float feelRange = 3;
        public bool isForceAttack = true;
        public float colliderRange = 1;
    }

    /// <summary>
    /// 玩家基本数据
    /// </summary>
    [Serializable]
    public class PlayerData : XmlData
    {
        public float moveSpeed = 0.08f;
        public float runSpeed = 0.08f;
        public float colliderRange = 0.5f;
        public override bool GetXmlDataAttribute(XmlNode node)
        {
            moveSpeed = ReadXmlDataMgr.GetInstance().IntParse(node, "moveSpeed");
            runSpeed = ReadXmlDataMgr.GetInstance().FloatParse(node, "runSpeed");
            colliderRange = ReadXmlDataMgr.GetInstance().FloatParse(node, "colliderRange");
            return base.GetXmlDataAttribute(node);
        }
    }

    /// <summary>
    /// 音频数据
    /// </summary>
    [Serializable]
    public class AudioData : XmlData
    {
        public int ID;
        public string Name;
        public string AudioName;

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            ID = ReadXmlDataMgr.GetInstance().IntParse(node, "ID");
            Name = ReadXmlDataMgr.GetInstance().StrParse(node, "Name");
            AudioName = ReadXmlDataMgr.GetInstance().StrParse(node, "AudioName");
            return base.GetXmlDataAttribute(node);
        }

    }

}
