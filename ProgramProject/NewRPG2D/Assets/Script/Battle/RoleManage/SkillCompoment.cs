using System;
using System.Collections.Generic;
using Assets.Script.Battle.BattleData;
using Assets.Script.Utility;
using UnityEngine.Video;

namespace Assets.Script.Battle
{
    public class SkillCompoment
    {
        private RoleBase mCurrentRole;
        //private Dictionary<int, SkillLevelComponent> skillDataDic;
        //private Dictionary<int, int> skillLevelDic;
        private Dictionary<int, SkillUseData> SkillUseDataDic;
        //private Dictionary<int, SkillData> SkillDataDic;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            //skillDataDic = new Dictionary<int, SkillLevelComponent>(4);
            //skillLevelDic = new Dictionary<int, int>(4);
            SkillUseDataDic = new Dictionary<int, SkillUseData>(4);
        }

        public void InitSkill(SkillSlotTypeEnum skillSlotType, int skillId)
        {
            SkillData data = SkillDataMgr.instance.GetXmlDataByItemId<SkillData>(skillId);
            SkillUseData useData = new SkillUseData();
            useData.CurrentCD = 0;
            useData.SkillType = data.SkillType;
            useData.EffectId1 = data.EffectId1;
            useData.EffectId2 = data.EffectId2;
            useData.EffectId3 = data.EffectId3;
            useData.EffectId4 = data.EffectId4;
            useData.MP = data.MP;
            useData.SkillCDTime = data.CD;
            useData.TargetCount = data.TargetCount;
            useData.SkillLevel = data.SkillLevel;
            useData.AttackRange = data.AttackRange;
            useData.Init();
            SkillUseDataDic.Add((int)skillSlotType, useData);
           // SetSkillLevel(skillSlotType, 1);
        }

        public void UpdateLogic(float deltaTime)
        {
            foreach (var data in SkillUseDataDic)
            {
                data.Value.UpdateLogic(deltaTime);
            }
        }

        //public void SetSkillLevel(SkillSlotTypeEnum skillSlotType, int skillLevel)
        //{
        //    int skillSlotIndex = (int)skillSlotType;
        //    SkillLevelComponent data = skillDataDic[skillSlotIndex];
        //    SkillUseData currentSkillUseData = data.GetSkillUseDataByLevel(skillLevel);
        //    int lastSkillLevel = 0;
        //    if (skillLevelDic.TryGetValue(skillSlotIndex, out lastSkillLevel))
        //    {
        //        SkillUseData lastSkillUseData = data.GetSkillUseDataByLevel(lastSkillLevel);
        //        currentSkillUseData.SetCurrentCD(lastSkillUseData.CurrentCD);
        //    }
        //    skillLevelDic[(int) skillSlotType] = skillLevel;
        //    SkillUseDataDic[(int) skillSlotType] = currentSkillUseData;
        //}

        //public SkillData GetSkillDataBySkilSlot(SkillSlotTypeEnum skillSlotType)
        //{
        //    int skillSlotIndex = (int) skillSlotType;
        //    int skillLevel = skillLevelDic[skillSlotIndex];
        //    SkillLevelComponent data = skillDataDic[skillSlotIndex];
        //    return data.GetSkillDataByLevel(skillLevel);
        //}

        public SkillUseData GetSkillUseDataBySkilSlot(SkillSlotTypeEnum skillSlotType)
        {
            return SkillUseDataDic[(int) skillSlotType];
        }
    }
}
