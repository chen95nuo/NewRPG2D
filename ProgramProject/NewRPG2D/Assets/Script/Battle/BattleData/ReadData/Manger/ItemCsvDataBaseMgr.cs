using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{

    public class ItemCsvDataBaseMgr<TK> : TSingleton<TK>
    {
        protected ItemBaseCsvData[] CurrentItemData;

        protected virtual CsvEChartsType CurrentCsvName
        {
            get { return CsvEChartsType.RoleData; }
        }

        public virtual T GetDataByItemId<T>(int itemId) where T : ItemBaseCsvData
        {
            for (int i = 0; i < CurrentItemData.Length; i++)
            {
                if (CurrentItemData[i].ItemId == itemId)
                {
                    return CurrentItemData[i] as T;
                }
            }
            DebugHelper.Log(typeof(T) + " don't have the item is the ItemId = " + itemId);
            return null;
        }

        public override void Init()
        {
            base.Init();
            // DebugHelper.LogError("CurrentXmlName  " + CurrentXmlName);
            List<CSVAnalysis> tempData = ReadTextAssetMgr.instance.dicCsvMode[CurrentCsvName];
            CurrentItemData = new ItemBaseCsvData[tempData.Count];
            for (int i = 0; i < tempData.Count; i++)
            {
                if (tempData[i] is ItemBaseCsvData == false)
                {
                    DebugHelper.LogError("tempData[i] = " + (tempData[i].ItemCsvName));
                }
                CurrentItemData[i] = (ItemBaseCsvData)tempData[i];
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
