
//功能：读取xml数据
//创建者: 胡海辉
//创建时间：


using Assets.Script.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Script
{
    public static class ReadXmlMgr<T> where T : class
    {

        private static string path;

        private static T target;
        private static XElement allXml;
        static ReadXmlMgr()
        {

        }

        #region author:Kuribayashi
        public static List<AudioData> GetAllAudioData()
        {
            string filepath =PlatformTools.m_FileRealPath+ "/Config/AudioData.xml";
            List<AudioData> audioDataList = new List<AudioData>();
            if (File.Exists(filepath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                using (XmlReader reader = XmlReader.Create(filepath, settings))
                {
                    xmlDoc.Load(reader);
                    XmlNode root = xmlDoc.SelectSingleNode("AudioData");
                    int count = root.ChildNodes.Count;
                    for (int i = 1; i <= count; i++)
                    {
                        AudioData audioData = new AudioData();
                        XmlNode node = root.SelectSingleNode("AudioData" + i);
                       
                        audioData.ID = int.Parse(node.SelectSingleNode("ID").InnerText);
                        audioData.Name = node.SelectSingleNode("Name").InnerText;
                        audioData.AudioName = node.SelectSingleNode("AudioName").InnerText;
                        audioDataList.Add(audioData);
                    }
                    return audioDataList;
                }
            }
            else return null;

        }



        public static void ChanageAudioData(AudioData audiodata)
        {
            ChanageNode("AudioData", string.Format("AudioData/AudioData{0}/Name", audiodata.ID), audiodata.Name);
            ChanageNode("AudioData", string.Format("AudioData/AudioData{0}/AudioName", audiodata.ID), audiodata.AudioName);

        }

        static bool ChanageNode(string xmlName, string nodePathName, string value)
        {
            string filepath = PlatformTools.m_FileRealPath + "/Config/AudioData.xml"; ;
            if (File.Exists(filepath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filepath);
                XmlNode node = xmlDoc.SelectSingleNode(nodePathName);
                node.InnerText = value;
                xmlDoc.Save(filepath);
                return true;

            }
            return false;
        }

        #endregion

        /// <summary>
        /// Sets the xml path.
        /// </summary>
        public static void SetXmlPath(string p)
        {
           path = p;
        }

        /// <summary>
        /// Loads the XML Files.
        /// </summary>
        private static XElement LoadXML(string XName)
        {
            if (path == null)
            {
                return null;
            }
            if (allXml == null)
            {
                XmlReaderSettings set = new XmlReaderSettings();
                set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
                //XmlDocument doc = new XmlDocument();
                allXml = XElement.Load(path);
            }
            XElement xmlNode = allXml.Element(XName);
            return xmlNode;

        }



        /// <summary>
        /// Creates the class initiate.
        /// </summary>
        private static void CreateInitiate()
        {

            Type t = typeof(T);

            ConstructorInfo ct = t.GetConstructor(System.Type.EmptyTypes);

            target = (T)ct.Invoke(null);

        }

        /// <summary>
        /// attribute assignment,
        /// 由于反射中设置字段值的方法会涉及到赋值的目标类型和当前类型的转化，
        /// 所以需要使用Convert.ChangeType进行类型转化
        /// </summary>
        public static T ReadXmlElement(string XName)
        {

            if (target != null)
            {
                target = null;
            }

            CreateInitiate();

            XElement xml = LoadXML(XName);
            if (xml == null) 
            {
                UnityEngine.Debug.LogError("XName is null===" + XName);
                return null;
            }
            Type t = target.GetType();

            FieldInfo[] fields = t.GetFields();

            string fieldName = string.Empty;

            foreach (FieldInfo f in fields)
            {

                fieldName = f.Name;

                if (xml.Element(fieldName) != null)
                {

                    f.SetValue(target, Convert.ChangeType(xml.Element(fieldName).Value, f.FieldType));
                }
            }

            return target;

        }

    }
}
