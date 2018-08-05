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
        private Dictionary<int, SkillLevelComponent> skillDataDic;
        private Dictionary<int, int> skillLevelDic;
        private Dictionary<int, SkillUseData> SkillUseDataDic;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            skillDataDic = new Dictionary<int, SkillLevelComponent>(4);
            skillLevelDic = new Dictionary<int, int>(4);
            SkillUseDataDic = new Dictionary<int, SkillUseData>(4);
        }

        public void InitSkill(SkillSlotTypeEnum skillSlotType, int skillId)
        {
            SkillLevelComponent data = SkillDataMgr.instance.GetSkilLevelBySkillId(skillId);
            skillDataDic.Add((int)skillSlotType, data);
            SetSkillLevel(skillSlotType, 1);
        }

        public void UpdateLogic(float deltaTime)
        {
            foreach (var data in SkillUseDataDic)
            {
                data.Value.UpdateLogic(deltaTime);
            }
        }

        public void SetSkillLevel(SkillSlotTypeEnum skillSlotType, int skillLevel)
        {
            int skillSlotIndex = (int)skillSlotType;
            SkillLevelComponent data = skillDataDic[skillSlotIndex];
            SkillUseData currentSkillUseData = data.GetSkillUseDataByLevel(skillLevel);
            int lastSkillLevel = 0;
            if (skillLevelDic.TryGetValue(skillSlotIndex, out lastSkillLevel))
            {
                SkillUseData lastSkillUseData = data.GetSkillUseDataByLevel(lastSkillLevel);
                currentSkillUseData.SetCurrentCD(lastSkillUseData.CurrentCD);
            }
            skillLevelDic[(int) skillSlotType] = skillLevel;
            SkillUseDataDic[(int) skillSlotType] = currentSkillUseData;
        }

        public SkillData GetSkillDataBySkilSlot(SkillSlotTypeEnum skillSlotType)
        {
            int skillSlotIndex = (int) skillSlotType;
            int skillLevel = skillLevelDic[skillSlotIndex];
            SkillLevelComponent data = skillDataDic[skillSlotIndex];
            return data.GetSkillDataByLevel(skillLevel);
        }

        public SkillUseData GetSkillUseDataBySkilSlot(SkillSlotTypeEnum skillSlotType)
        {
            return SkillUseDataDic[(int) skillSlotType];
        }
    }
}
