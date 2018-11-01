using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class EquipmentDataMgr : ItemDataBaseMgr<EquipmentDataMgr>
    {
        protected override XmlName CurrentXmlName
        {
            get { return XmlName.Equipment; }
        }

        private List<EquipmentData> battleEquipmentDataList;
        public List<EquipmentData> BattleEquipment
        {
            get
            {
                if (battleEquipmentDataList == null)
                {
                    battleEquipmentDataList = new List<EquipmentData>();
                    for (int i = 0; i < CurrentItemData.Length; i++)
                    {
                        EquipmentData data = CurrentItemData[i] as EquipmentData;
                        if (data == null || data.WeaponType == WeaponTypeEnum.Life)
                        {
                            continue;
                        }

                        battleEquipmentDataList.Add(data);
                    }

                }
                return battleEquipmentDataList;

            }
        }

        private List<EquipmentData> lifeEquipmentDataList;
        public List<EquipmentData> LifeEquipment
        {
            get
            {
                if (lifeEquipmentDataList == null)
                {
                    lifeEquipmentDataList = new List<EquipmentData>();
                    for (int i = 0; i < CurrentItemData.Length; i++)
                    {
                        EquipmentData data = CurrentItemData[i] as EquipmentData;
                        if (data == null || data.WeaponType != WeaponTypeEnum.Life)
                        {
                            continue;
                        }

                        lifeEquipmentDataList.Add(data);
                    }

                }
                return lifeEquipmentDataList;

            }
        }

        public List<int> GetBattleEquipmentByLevelAndQuality(int currentLevel, QualityTypeEnum qualityType)
        {
           return  GetEquipmentByLevelAndQuality(BattleEquipment, currentLevel, qualityType);
        }

        public List<int> GetLifeEquipmentByLevelAndQuality(int currentLevel, QualityTypeEnum qualityType)
        {
            return GetEquipmentByLevelAndQuality(LifeEquipment, currentLevel, qualityType);
        }

        public List<int> GetEquipmentByLevelAndQuality(List<EquipmentData> currentItemData, int currentLevel, QualityTypeEnum qualityType)
        {
            List<int> equipItemIdList= new List<int>();
            for (int i = 0; i < currentItemData.Count; i++)
            {
                if (currentLevel >= currentItemData[i].LevelRange.Min && currentLevel <= currentItemData[i].LevelRange.Max)
                {
                    if (currentItemData[i].QualityType == qualityType)
                    {
                        equipItemIdList.Add(currentItemData[i].ItemId);
                    }
                }
            }

            return equipItemIdList;
        }

    }
}
