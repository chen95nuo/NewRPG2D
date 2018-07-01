using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{

    public class ItemDataBaseMgr<TK>: TSingleton<TK> 
    {
        protected ItemBaseData[] CurrentItemData;
        private int CurrentXmlIndex;

        public virtual XmlName CurrentXmlName
        {
            get {return XmlName.RoleData;}
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
            XmlData[] tempData = ReadXmlNewMgr.instance.AllXmlDataDic[CurrentXmlIndex];
            CurrentItemData = new ItemBaseData[tempData.Length];
            for (int i = 0; i < tempData.Length; i++)
            {
                CurrentItemData[i] = (ItemBaseData)tempData[i];
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
