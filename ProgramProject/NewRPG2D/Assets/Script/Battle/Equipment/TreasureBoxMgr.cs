
using System.Collections.Generic;
using Assets.Script.Battle.BattleData;
using UnityEngine;

namespace Assets.Script.Battle.Equipment
{
    public class ItemDataInTreasure
    {
        public List<EquipmentRealProperty> EquipmentList;
        public List<PropData> PropDataList;

        public ItemDataInTreasure()
        {
            EquipmentList = new List<EquipmentRealProperty>(5);
            PropDataList = new List<PropData>(5);
        }
    }

    public class TreasureBoxMgr : TSingleton<TreasureBoxMgr>
    {
        public ItemDataInTreasure OpenTreasureBox(int itemId, int currentBattleLevel, int currentLifeLevel)
        {
            ItemDataInTreasure data = new ItemDataInTreasure();
            TreasureBox boxItem = TreasureBoxDataMgr.instance.GetXmlDataByItemId<TreasureBox>(itemId);
            int realBattleLevel = Random.Range(currentBattleLevel - 5, currentBattleLevel + 2);
            int realLifeLevel = Random.Range(currentLifeLevel - 10, currentLifeLevel);
            for (int i = 0; i < boxItem.TreasureBoxItems.Length; i++)
            {
                int randomCount = Random.Range(boxItem.TreasureBoxItems[i].ItemMinCount,boxItem.TreasureBoxItems[i].ItemMaxCount);
                GetItemDetail(data, boxItem.TreasureBoxItems[i].ItemId, realBattleLevel, realLifeLevel, boxItem.DependLevel, randomCount);

            }

            for (int i = 0; i < boxItem.RandomTreasureBoxItems.Length; i++)
            {
                if (Random.Range(0, 1.0f) < boxItem.RandomTreasureBoxItems[i].CreateChange)
                {
                    int randomCount = Random.Range(boxItem.TreasureBoxItems[i].ItemMinCount, boxItem.TreasureBoxItems[i].ItemMaxCount);
                    GetItemDetail(data, boxItem.RandomTreasureBoxItems[i].ItemData.ItemId, realBattleLevel, realLifeLevel,
                        boxItem.DependLevel, randomCount);

                }
            }

            return data;
        }

        private void GetItemDetail(ItemDataInTreasure data, int itemId, int currentBattleLevel, int currentLifeLevel, bool dependLevel, int count)
        {
            PropData mData = PropDataMgr.instance.GetXmlDataByItemId<PropData>(itemId);
            if (mData.propType == PropType.Equipment)
            {
                List<int> equipmentId = new List<int>();
                if (Random.Range(0, 100) < 80 || (QualityTypeEnum)mData.quality == QualityTypeEnum.Orange ||
                    (QualityTypeEnum)mData.quality == QualityTypeEnum.Purple)
                {
                    equipmentId = EquipmentDataMgr.instance.GetBattleEquipmentByLevelAndQuality(currentBattleLevel,
                        (QualityTypeEnum)mData.quality);
                }
                else
                {
                    equipmentId = EquipmentDataMgr.instance.GetLifeEquipmentByLevelAndQuality(currentLifeLevel,
                        (QualityTypeEnum)mData.quality);
                }

                if (equipmentId.Count > 0)
                {
                    int randomIndex = Random.Range(0, equipmentId.Count);
                    EquipmentRealProperty equipmentRealData =
                        EquipmentMgr.instance.CreateNewEquipment(equipmentId[randomIndex], dependLevel);

                    for (int i = 0; i < count; i++)
                    {
                        data.EquipmentList.Add(equipmentRealData);
                    }
                }
            }
            else if (mData.propType == PropType.Fragment)
            {
                List<PropData> propDataList = PropDataMgr.instance.GetPropDataByType(mData.propType, mData.quality);
                propDataList.Remove(mData);
                PropData propData = propDataList[Random.Range(0, propDataList.Count)];
                for (int i = 0; i < count; i++)
                {
                    data.PropDataList.Add(propData);
                }

            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    data.PropDataList.Add(mData);
                }
            }

        }
    }
}
