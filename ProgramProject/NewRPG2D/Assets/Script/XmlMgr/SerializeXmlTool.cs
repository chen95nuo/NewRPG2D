
//功能：
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Assets.Script
{
    public class SerializeXmlTool : TSingleton<SerializeXmlTool>,IDisposable
    {
        public override void Init()
        {
            base.Init();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="filePath"></param>
        public static void Serialize<T>(T o, string filePath)
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                StreamWriter sw = new StreamWriter(filePath, false);
                formatter.Serialize(sw, o);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e) 
            {
                DebugHelper.DebugLogError(e.Message);
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string filePath) 
        {
            try
            {
                XmlSerializer mSerializeFile = new XmlSerializer(typeof(T));
                StreamReader mReader = new StreamReader(filePath);
                T data = (T)mSerializeFile.Deserialize(mReader);
                mReader.Close();
                return data;
            }
            catch (Exception e)
            {
                DebugHelper.DebugLogError(e.Message);   
            }
            return default(T);
        } 
    }
}
