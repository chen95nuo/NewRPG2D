using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Utility
{
    public abstract class CSVAnalysis
    {
        public virtual CsvEChartsType ItemCsvName
        {
            get { return 0; }
        }

        public abstract bool AnalySis(string[] data);

        protected int IntParse(string[] data, int index)
        {

            if (data.Length > index)
            {
                try
                {
                    return int.Parse(data[index]);
                }
                catch
                {
                    Debug.LogError(" IntParse  data is error " + data[index]);
                }
            }
            else
            {
                Debug.LogError(" IntParse index is error " + index);
            }
            return 0;
        }

        protected float FloatParse(string[] data, int index)
        {

            if (data.Length > index)
            {
                try
                {
                    return float.Parse(data[index]);
                }
                catch
                {
                    Debug.LogError(" FloatParse  data is error " + data[index]);
                }
            }
            else
            {
                Debug.LogError(" FloatParse index is error " + index);
            }
            return 0.0f;
        }

        protected string StrParse(string[] data, int index)
        {

            if (data.Length > index)
            {
                try
                {
                    return data[index];
                }
                catch
                {
                    Debug.LogError(" StrParse  data is error " + data[index]);
                }
            }
            else
            {
                Debug.LogError(" StrParse index is error " + index);
            }
            return "";
        }

        protected List<float> ListParse(string[] data, int index)
        {
            List<float> realData = null;
            if (data.Length > index)
            {
                try
                {
                    realData = new List<float>();
                    string realDateString = data[index].Substring(1, data[index].Length - 2);
                    string[] realDateStringArray = realDateString.Split(',');
                    for (int i = 0; i < realDateStringArray.Length; i++)
                    {
                        realData.Add(FloatParse(realDateStringArray, i));
                    }
                    return realData;
                }
                catch
                {
                    Debug.LogError(" StrParse  data is error " + data[index]);
                }
            }
            else
            {
                Debug.LogError(" StrParse index is error " + index);
            }
            return realData;
        }

    }

    public class ReadTextAssetMgr : TSingleton<ReadTextAssetMgr>
    {
        public Dictionary<Enum, List<CSVAnalysis>> dicCsvMode = new Dictionary<Enum, List<CSVAnalysis>>();
        public bool isLoading = true;
        int loadIndex = 0;
        public override void Init()
        {
            base.Init();
            Debug.Log("开始下载");
            for (int i = 0; i < (int)CsvEChartsType.Max; i++)
            {
                ConfigCSVFileName((CsvEChartsType)i);
            }
        }

        /// <summary>
        /// 配置CSV文件名,并配置对应的解析CSV文件的类
        /// </summary>
        private void ConfigCSVFileName(CsvEChartsType filEChartsType)
        {
            GameLogic.Instance.StartCoroutine(DownLoadData(filEChartsType));
        }

        private IEnumerator DownLoadData(CsvEChartsType filEChartsType)
        {
            isLoading = true;

            //下载ECharts.csv
            List<string[]> model = new List<string[]>();

            yield return model = ReadTextDataMgr.ReadCSV(filEChartsType.ToString());

            List<CSVAnalysis> listCsv = new List<CSVAnalysis>(model.Count);
            int tabelHead = 3;
            for (int i = tabelHead; i < model.Count; i++)
            {
                CSVAnalysis data = ReadTextDataMgr.GetXmlData(filEChartsType);
                data.AnalySis(model[i]);
                listCsv.Add(data);
            }
            dicCsvMode[filEChartsType] = listCsv;
            isLoading = false;
        }

        public void DownLoad(string fileName, Action<string, List<string[]>> callback)
        {
            if (callback != null)
            {
                callback(fileName, ReadTextDataMgr.ReadCSV(fileName));
            }
        }
    }
}
