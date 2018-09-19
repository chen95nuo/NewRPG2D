using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{

    public class ItemDataBaseMgr<TK> : TSingleton<TK>
    {
        protected ItemBaseData[] CurrentItemData;
        private int CurrentXmlIndex;

        protected virtual XmlName CurrentXmlName
        {
            get { return XmlName.RoleData; }
        }

        public virtual T GetXmlDataByItemId<T>(int itemId) where T : ItemBaseData
        {
            for (int i = 0; i < CurrentItemData.Length; i++)
            {
                if (CurrentItemData[i].ItemId == itemId)
                {
                    return CurrentItemData[i] as T;
                }
            }
            DebugHelper.LogError(typeof(T) + " don't have the item is the ItemId = " + itemId);
            return null;
        }

        public override void Init()
        {
            base.Init();
            CurrentXmlIndex = (int)CurrentXmlName;
            // DebugHelper.LogError("CurrentXmlName  " + CurrentXmlName);
            XmlData[] tempData = ReadXmlNewMgr.instance.AllXmlDataDic[CurrentXmlIndex];
            CurrentItemData = new ItemBaseData[tempData.Length];
            for (int i = 0; i < tempData.Length; i++)
            {
                if (tempData[i] is ItemBaseData == false)
                {
                    DebugHelper.LogError("tempData[i] = " + (tempData[i].ItemXmlName));
                }
                CurrentItemData[i] = (ItemBaseData)tempData[i];
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }


        public Dictionary<RoomType, List<BuildingData>> GetBuildingType()
        {
            Dictionary<RoomType, List<BuildingData>> dic = new Dictionary<RoomType, List<BuildingData>>();

            for (int i = 0; i < CurrentItemData.Length; i++)
            {
                BuildingData data = CurrentItemData[i] as BuildingData;
                if (data.UnlockLevel != null)
                {
                    if (dic.ContainsKey(data.RoomType) == false)
                    {
                        dic.Add(data.RoomType, new List<BuildingData>());
                    }
                    dic[data.RoomType].Add(data);
                }
            }
            return dic;
        }
    }
}
