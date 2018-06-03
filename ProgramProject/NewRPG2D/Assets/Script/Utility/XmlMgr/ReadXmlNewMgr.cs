
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
        public virtual ReadXmlDataMgr.XmlName ItemXmlName
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
            int maxEnum = (int)ReadXmlDataMgr.XmlName.Max;
            AllXmlDataDic = new Dictionary<int, XmlData[]>(maxEnum);
            for (int i = 0; i < maxEnum; i++)
            {
                GameLogic.Instance.StartCoroutine(LoadConfig((ReadXmlDataMgr.XmlName)i));
            }
         
        }

        private XmlNode LoadConfigNode;
        private IEnumerator LoadConfig(ReadXmlDataMgr.XmlName name)
        {
            XmlNode node = null;
            yield return GameLogic.Instance.StartCoroutine(ReadTxt(name));
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
                XmlData data = ReadXmlDataMgr.GetXmlData(name);
                data.GetXmlDataAttribute(childrenNodeList[i]);
                xmlDataArray[i] = data;
            }
            AllXmlDataDic.Add((int)name, xmlDataArray);
            DebugHelper.Log("=======" + AllXmlDataDic.Count);
        }

        public override void Dispose()
        {
            base.Dispose();
            AllXmlDataDic.Clear();
            AllXmlDataDic = null;
        }

        public MemoryStream LoadFile(ReadXmlDataMgr.XmlName name)
        {
            string mPath = ReadXmlDataMgr.GetXmlPath(name);
            StreamReader reader = new StreamReader(mPath);
            UTF8Encoding encode = new System.Text.UTF8Encoding();
            byte[] binary = encode.GetBytes(reader.ReadToEnd());
            reader.Close();
            return new MemoryStream(binary);
        }

        IEnumerator ReadTxt(ReadXmlDataMgr.XmlName name)
        {
            string mPath = ReadXmlDataMgr.GetXmlPath(name);
            string content;
            WWW www = new WWW(mPath);
            yield return www;
            if (www.isDone)
            {
                content = www.text;
                UTF8Encoding encode = new System.Text.UTF8Encoding();
                byte[] binary = encode.GetBytes(content);
                MemoryStream fileMemoryStream = new MemoryStream(binary);
                LoadConfigNode = LoadXmlFile(fileMemoryStream);
            }
            else
            {
                LoadConfigNode = null;
            }
        }

        public XmlDocument GetXmlDocByMemory(MemoryStream mStream)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReader reader = XmlReader.Create(mStream);
            xmlDoc.Load(reader);
            reader.Close();
            return xmlDoc;
        }

        public XmlNodeList GetXmlNodeList(XmlDocument xmlDoc)
        {
            return xmlDoc.GetElementsByTagName("RECORDS");
        }

        public XmlNode LoadXmlFile(MemoryStream mStream)
        {
            XmlNodeList nodeList = GetXmlNodeList(GetXmlDocByMemory(mStream));
            if (nodeList == null || nodeList.Count <= 0) return null;
            return nodeList[0];
        }
    }
}
