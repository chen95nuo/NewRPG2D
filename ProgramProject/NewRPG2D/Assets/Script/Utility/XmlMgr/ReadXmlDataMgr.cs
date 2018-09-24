
//功能： 读取的Xml数据管理
//创建者: 胡海辉
//创建时间：


using System;
using UnityEngine;
using System.Xml;
using Assets.Script.Battle.BattleData;

namespace Assets.Script.Utility
{
    /// <summary>
    /// 配置表名
    /// </summary>
    public enum XmlName
    {
        BuildingData,
        Hall,

        RoleData,
        RolePropertyData,
        SkillData,
        CreateEnemyData,
        Battle,

        Max,
        MapSceneLevel,
        BufferData,
    }

    public enum XmlTypeEnum
    {
        Hall,
        Battle,
    }


    public class ReadXmlDataMgr
    {
        public static string GetXmlPath(string name)
        {
            string path = "";
#if UNITY_EDITOR
            path = string.Format("{0}/Config/{1}.xml", Application.streamingAssetsPath, name);

#elif UNITY_IPHONE
	   path = string.Format("{0}/Raw/Config/{1}.xml", Application.dataPath, name); 
 
#elif UNITY_ANDROID
	    path = string.Format("{0}/Config/{1}.xml", Application.streamingAssetsPath, name);
#else
        path = string.Format("{0}/Config/{1}.xml", Application.streamingAssetsPath, name);
#endif
            return path;
        }

        public static int IntParse(XmlNode node, string name)
        {
            return (int)Parse(node, name, 0);
        }

        public static string StrParse(XmlNode node, string name)
        {
            return (string)Parse(node, name, "");
        }

        public static float FloatParse(XmlNode node, string name)
        {
            return (float)Parse(node, name, 0.0f);
        }

        public static object Parse(XmlNode node, string name, object defaultValue)
        {
            try
            {
                return Convert.ChangeType(node.Attributes[name].Value, defaultValue.GetType());
            }
            catch (System.Exception ex)
            {
                DebugHelper.LogError(ex.Message + " name ===" + name);
                return defaultValue;
            }
        }

        public static float[] FloatArray(XmlNode node, string name)
        {
            string a = StrParse(node, name);
            string[] astr = a.Split(',');
            float[] f = new float[astr.Length];
            for (int i = 0; i < astr.Length; i++)
            {
                f[i] = float.Parse(astr[i]);
                if (f[i] == 0)
                {
                    return null;
                }
            }
            return f;
        }
    }
}
