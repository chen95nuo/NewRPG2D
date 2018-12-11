using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class MapLevelDataMgr : ItemCsvDataBaseMgr<MapLevelDataMgr>
    {
        protected override CsvEChartsType CurrentCsvName
        {
            get { return CsvEChartsType.CreateEnemyData; }
        }
        
        /// <summary>
        /// 获取全部关卡信息
        /// </summary>
        /// <returns></returns>
        public MapLevelData[] GetAllLisson()
        {
            MapLevelData[] data = new MapLevelData[CurrentItemData.Length];
            for (int i = 0; i < CurrentItemData.Length; i++)
            {
                data[i] = CurrentItemData[i] as MapLevelData;
            }
            return data;
        }
    }
}
