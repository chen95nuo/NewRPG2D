using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Utility
{
    public class ReadTextAssetMgr : TSingleton<ReadTextAssetMgr>
    {
        public Dictionary<int, XmlData[]> AllTxtDataDic;
        public override void Init()
        {
            base.Init();
            AllTxtDataDic = new Dictionary<int, XmlData[]>(10);

            for (int i = 0; i < (int)XmlName.Max; i++)
            {
                if (i == (int) XmlName.Hall || i == (int) XmlName.Battle)
                {
                    continue;;
                }

                XmlName name = (XmlName)i;
                ReadTxt(name);
            }
        }

        private void ReadTxt(XmlName name)
        {
            string mPath = ReadXmlDataMgr.GetXmlPath(name.ToString());
            TextAsset binAsset = Resources.Load(mPath, typeof(TextAsset)) as TextAsset;
            string[] lineArray = binAsset.text.Split("\r"[0]);
            XmlData[] xmlDataArray = new XmlData[lineArray.Length];
            for (int i = 0; i < lineArray.Length; i++)
            {
                XmlData data = ReadTextDataMgr.GetXmlData(name);
                data.GetXmlDataAttribute(lineArray[i]);
                xmlDataArray[i] = data;
            }
            AllTxtDataDic[(int)name] = xmlDataArray;
        }
    }
}
