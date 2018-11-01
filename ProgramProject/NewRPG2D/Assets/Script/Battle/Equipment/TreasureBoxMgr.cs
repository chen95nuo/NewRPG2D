
using System.Collections.Generic;
using Assets.Script.Battle.BattleData;
using UnityEngine;

namespace Assets.Script.Battle.Equipment
{
    public class ItemDataInTreasure
    {
        public List<EquipmentRealProperty> EquipmentList;
        public List<RealPropData> PropDataList;

        public ItemDataInTreasure()
        {
            EquipmentList = new List<EquipmentRealProperty>(5);
            PropDataList = new List<RealPropData>(5);
        }
    }

    public class TreasureBoxMgr : TSingleton<TreasureBoxMgr>
    {
        public ItemDataInTreasure OpenTreasureBox(int itemId, int currentBattleLevel, int currentLifeLevel)
        {
            ItemDataInTreasure data = new ItemDataInTreasure();
            TreasureBox boxItem = TreasureBoxDataMgr.instance.GetXmlDataByItemId<TreasureBox>(itemId);
            int realBattleLevel = Mathf.Max(1, Random.Range(currentBattleLevel - 5, currentBattleLevel + 2));
            int realLifeLevel = Mathf.Max(1, Random.Range(currentLifeLevel - 10, currentLifeLevel));
            for (int i = 0; i < boxItem.TreasureBoxItems.Length; i++)
            {
                if (boxItem.TreasureBoxItems[i].ItemMaxCount == 0)
                {
                    continue;
                }
                int randomCount = Random.Range(boxItem.TreasureBoxItems[i].ItemMinCount, boxItem.TreasureBoxItems[i].ItemMaxCount);
                GetItemDetail(data, boxItem.TreasureBoxItems[i].ItemId, realBattleLevel, realLifeLevel, boxItem.DependLevel, randomCount);

            }

            for (int i = 0; i < boxItem.RandomTreasureBoxItems.Length; i++)
            {
                if (boxItem.RandomTreasureBoxItems[i].ItemData.ItemMaxCount == 0)
                {
                    continue;
                }
                if (Random.Range(0, 1.0f) < boxItem.RandomTreasureBoxItems[i].CreateChange)
                {
                    int randomCount = Random.Range(boxItem.RandomTreasureBoxItems[i].ItemData.ItemMinCount, boxItem.RandomTreasureBoxItems[i].ItemData.ItemMaxCount);
                    GetItemDetail(data, boxItem.RandomTreasureBoxItems[i].ItemData.ItemId, realBattleLevel, realLifeLevel,
                        boxItem.DependLevel, randomCount);

                }
            }

            return data;
        }

        private void GetItemDetail(ItemDataInTreasure data, int itemId, int currentBattleLevel, int currentLifeLevel, bool dependLevel, int count)
        {
            if (itemId == 0)
            {
                return;
            }
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
                        EquipmentMgr.instance.CreateNewEquipment(equipmentId[randomIndex]);

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
                RealPropData mRealPropData = ChickItemInfo.instance.CreateNewProp(propData.ItemId, count);
                for (int i = 0; i < count; i++)
                {
                    if (data.PropDataList.Contains(mRealPropData))
                    {
                        int index = data.PropDataList.IndexOf(mRealPropData);
                        data.PropDataList[index].number++;
                    }
                    else
                    {
                        data.PropDataList.Add(mRealPropData);
                    }
                }
            }
            else
            {
                RealPropData mRealPropData = ChickItemInfo.instance.CreateNewProp(itemId, count);
                data.PropDataList.Add(mRealPropData);
            }

        }
    }
}
