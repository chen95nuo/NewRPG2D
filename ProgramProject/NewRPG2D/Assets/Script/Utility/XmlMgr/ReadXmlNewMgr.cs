
//功能：
//创建者: 胡海辉
//创建时间：


using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using UnityEngine;
using System.Collections;
using Assets.Script;

namespace Assets.Script.Utility
{
    public class XmlData
    {
        public virtual XmlName ItemXmlName
        {
            get { return 0; }
        }

        public virtual bool GetXmlDataAttribute(XmlNode node)
        {
            return true;
        }
    }

    public class ReadXmlNewMgr : TSingleton<ReadXmlNewMgr>, IDisposable
    {
        public Dictionary<int, XmlData[]> AllXmlDataDic;
        public override void Init()
        {
            base.Init();
            AllXmlDataDic = new Dictionary<int, XmlData[]>(10);
            ReadXmlByType(XmlName.Battle + 1, XmlName.Max, XmlTypeEnum.Common);
            ReadXmlByType(XmlName.RoleData, XmlName.Battle, XmlTypeEnum.Battle);
            ReadXmlByType(XmlName.BuildingData, XmlName.Hall);
        }

        public void ReadXmlByType(XmlName startXmlName, XmlName maXmlName, XmlTypeEnum xmlType = XmlTypeEnum.Hall)
        {
            int maxEnum = (int)maXmlName;
            int startEnum = (int)startXmlName;
            for (int i = startEnum; i < maxEnum; i++)
            {
                XmlName name = (XmlName)i;
                GameLogic.Instance.StartCoroutine(LoadConfig(name, name.ToString(), xmlType));
            }
        }

        public void LoadSpecialXML(XmlName xmlName, string pathName, XmlTypeEnum xmlType)
        {
            GameLogic.Instance.StartCoroutine(LoadConfig(xmlName, pathName, xmlType));
        }

        private XmlNode LoadConfigNode;
        private IEnumerator LoadConfig(XmlName name, string xmlPathName, XmlTypeEnum xmlType = XmlTypeEnum.Hall)
        {
            XmlNode node = null;
            yield return GameLogic.Instance.StartCoroutine(ReadTxt(xmlPathName, xmlType));
            node = LoadConfigNode;
            if (node == null)
            {
                DebugHelper.LogError("node====null");
                yield break;
            }
            XmlNodeList childrenNodeList = node.ChildNodes;
            XmlData[] xmlDataArray = new XmlData[childrenNodeList.Count];
            for (int i = 0; i < childrenNodeList.Count; i++)
            {
                XmlData data = null;
                if (xmlType == XmlTypeEnum.Hall)
                {
                    data = ReadHallXmlDataMgr.GetXmlData(name);
                }
                else if (xmlType == XmlTypeEnum.Battle)
                {
                    data = ReadBattleXmlDataMgr.GetXmlData(name);
                }
                else if (xmlType == XmlTypeEnum.Common)
                {
                    data = ReadCommonlXmlDataMgr.GetXmlData(name);
                }
                data.GetXmlDataAttribute(childrenNodeList[i]);
                xmlDataArray[i] = data;
            }
            AllXmlDataDic[(int)name] = xmlDataArray;
            DebugHelper.Log("=======" + AllXmlDataDic.Count);
        }

        public override void Dispose()
        {
            base.Dispose();
            AllXmlDataDic.Clear();
            AllXmlDataDic = null;
        }

        /*       private MemoryStream LoadFile(string name)
               {
                   string mPath = ReadXmlDataMgr.GetXmlPath(name);
                   StreamReader reader = new StreamReader(mPath);
                   UTF8Encoding encode = new System.Text.UTF8Encoding();
                   byte[] binary = encode.GetBytes(reader.ReadToEnd());
                   reader.Close();
                   return new MemoryStream(binary);
               }*/

        private IEnumerator ReadTxt(string name, XmlTypeEnum xmlType = XmlTypeEnum.Hall)
        {
            string mPath = ReadXmlDataMgr.GetXmlPath(name, xmlType);
            string content;
            WWW www = new WWW(mPath);
            yield return www;
            if (www.isDone)
            {
                content = www.text;
                UTF8Encoding encode = new System.Text.UTF8Encoding();
                byte[] binary = encode.GetBytes(content);
                MemoryStream fileMemoryStream = new MemoryStream(binary);

                try
                {
                    LoadConfigNode = LoadXmlFile(fileMemoryStream);
                }
                catch (Exception e)
                {

                    DebugHelper.LogError(" error " + e.Message + "   " + name.ToString());
                }
            }
            else
            {
                LoadConfigNode = null;
            }
        }

        private XmlDocument GetXmlDocByMemory(MemoryStream mStream)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReader reader = XmlReader.Create(mStream);
            xmlDoc.Load(reader);
            reader.Close();
            return xmlDoc;
        }

        private XmlNodeList GetXmlNodeList(XmlDocument xmlDoc)
        {
            return xmlDoc.GetElementsByTagName("RECORDS");
        }

        private XmlNode LoadXmlFile(MemoryStream mStream)
        {
            XmlNodeList nodeList = GetXmlNodeList(GetXmlDocByMemory(mStream));
            if (nodeList == null || nodeList.Count <= 0) return null;
            return nodeList[0];
        }
    }
}
