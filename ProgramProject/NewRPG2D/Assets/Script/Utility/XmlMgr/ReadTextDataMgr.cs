
//功能： 读取的Xml数据管理
//创建者: 胡海辉
//创建时间：


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Xml;
using Assets.Script.Battle.BattleData;

namespace Assets.Script.Utility
{

    /// <summary>
    /// 关键索引，所有CSV文件关键字段集合
    /// </summary>
    public enum CsvEChartsType
    {
        #region ECharts

        //战斗内的
        RolePropertyData,
        Max,

        //城堡大厅的
        BuildingData,
        TrainData,
        MagicData,
        ChildData,
        PropData,
        WorldMapData,
        WorkShopData,


        SkillData,
        RoleData,
        //公共的
        TreasureBox,
        Equipment,
        EquipBaseProperty,
        LanguageData,
        CreateEnemyData,



        //动态读取
        MapSceneLevel,
        BufferData,
        #endregion
    }

    public class ReadTextDataMgr
    {

        public static CSVAnalysis GetXmlData(CsvEChartsType name)
        {
            switch (name)
            {
                //case CsvEChartsType.RoleData:
                //    return new RoleData();
                case CsvEChartsType.RolePropertyData:
                    return new RolePropertyData();
                //case CsvEChartsType.SkillData:
                //    return new SkillData();
                //case CsvEChartsType.BufferData:
                //    return new RolePropertyData();
                //case CsvEChartsType.MapSceneLevel:
                //    return new MapSceneLevelData();
                case CsvEChartsType.BuildingData:
                    return new BuildingData();
                case CsvEChartsType.MagicData:
                    return new MagicData();
                case CsvEChartsType.ChildData:
                    return new ChildData();
                case CsvEChartsType.PropData:
                    return new PropData();
                case CsvEChartsType.WorldMapData:
                    return new WorldMapData();


                default: return new ItemBaseCsvData();
            }
        }


        //文件名返回路径
        public static string FileCSV(string _fileName)
        {
            string path = "";
#if UNITY_STANDALONE
        path = Application.dataPath + "/StreamingAssets/Config/CSV/" + _fileName + ".csv";
#elif UNITY_EDITOR
            path = Application.dataPath + "/StreamingAssets/Config/CSV/" + _fileName + ".csv";
#elif UNITY_IPHONE
    path = Application.dataPath + "/Raw/Config/Csv/" + _str + ".csv";
#elif UNITY_ANDROID
    path = "jar:file://" + Application.dataPath + "!/assets/Config/CSV/"+ _str +".csv";
#endif
            Debug.Log(path);
            return path;
        }

        //解析文件
        public static List<string[]> ReadCSV(string _fileName)
        {
            //  Debug.Log("解析");
            List<string[]> data = new List<string[]>();
            string path = FileCSV(_fileName);
            if (File.Exists(path))
            {
                try
                {
                    StreamReader srReadFile = new StreamReader(path, Encoding.UTF8);
                    while (!srReadFile.EndOfStream)
                    {
                        //检索出行
                        UTF8Encoding utf8 = new UTF8Encoding();
                        string value = utf8.GetString(utf8.GetBytes(srReadFile.ReadLine()));
                        data.Add(GetItemList(value));
                        // Debug.Log(value);
                    }
                    // 关闭读取流文件
                    srReadFile.Close();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message + "请检查本地文件是否打开");
                }

            }
            else
            {
                WWW www = new WWW(path);
                while (!www.isDone) { }

                MemoryStream stream = new MemoryStream(www.bytes);
                StreamReader reader = new StreamReader(stream);
                while (!reader.EndOfStream)
                {
                    //检索出行
                    UTF8Encoding utf8 = new UTF8Encoding();
                    string value = utf8.GetString(utf8.GetBytes(reader.ReadLine()));
                    data.Add(GetItemList(value));
                    Debug.Log(value);
                }
                stream.Close();
                reader.Close();

                Debug.Log(www.text);
            }
            return data;
        }

        private static string[] GetItemList(string line)
        {
            string[] values = line.Split(',');
            List<string> valueList = new List<string>(values.Length);
            int startIndex = values.Length, endIndex = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].StartsWith("\"") && values[i].EndsWith("\"") == false)
                {
                    startIndex = i;
                    endIndex = values.Length;
                }

                if (i > startIndex && i < endIndex)
                {
                    if (values[i].EndsWith("\""))
                    {
                        endIndex = i;
                    }
                    values[startIndex] += "," + values[i];
                    values[i] = string.Empty;
                }

                if (String.IsNullOrEmpty(values[i]) == false)
                {
                    valueList.Add(values[i]);
                }
                else
                {
                    if (valueList.Count >= startIndex)
                    {
                        valueList[startIndex] = values[startIndex];
                    }
                }
            }
            return valueList.ToArray();
        }

    }
}
