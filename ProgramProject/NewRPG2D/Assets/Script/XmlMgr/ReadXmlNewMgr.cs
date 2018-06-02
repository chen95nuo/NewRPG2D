
//功能：
//创建者: 胡海辉
//创建时间：


using System;
using System.Text;
using System.Xml;
using System.IO;
using Assets.Script.Base;

namespace Assets.Script
{
    public class XmlData
    {
        public virtual bool GetXmlDataAttribute(XmlNode node)
        {
            return true;
        }
    }

    public class ReadXmlNewMgr : TSingleton<ReadXmlNewMgr>,IDisposable
    {

        public override void Init()
        {
            base.Init();
            int maxEnum = Enum.GetNames(typeof(ReadXmlDataMgr.XmlName)).Length; //(int)ReadXmlDataMgr.XmlName.Max;
            for (int i = 0; i < maxEnum; i++)
            {
                LoadCofig((ReadXmlDataMgr.XmlName)i);
            }
        }

        private void LoadCofig(ReadXmlDataMgr.XmlName name)
        {
            XmlData data = ReadXmlDataMgr.GetInstance().GetXmlData(name);
            XmlNode node = LoadXmlFile(name);
            if (node == null)
            {
                DebugHelper.DebugLog("node====null");
                return;
            }
            XmlNodeList childrenNodeList = node.ChildNodes;
            for (int i = 0; i < childrenNodeList.Count; i++)
            {
                data.GetXmlDataAttribute(childrenNodeList[i]);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }


        public MemoryStream LoadFile(ReadXmlDataMgr.XmlName name)
        {
            string mPath = ReadXmlDataMgr.GetInstance().GetXmlPath(name);
            StreamReader reader = new StreamReader(mPath);
            UTF8Encoding encode = new System.Text.UTF8Encoding();
            byte[] binary = encode.GetBytes(reader.ReadToEnd());
            reader.Close();
            return new MemoryStream(binary);
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
            return xmlDoc.GetElementsByTagName("ReNodes");
        }

        public XmlNode LoadXmlFile(ReadXmlDataMgr.XmlName name)
        {
            MemoryStream stream = LoadFile(name);
            XmlNodeList nodeList = GetXmlNodeList(GetXmlDocByMemory(stream));
            if (nodeList == null || nodeList.Count <= 0) return null;
            return nodeList[0];
        }
    }
}
