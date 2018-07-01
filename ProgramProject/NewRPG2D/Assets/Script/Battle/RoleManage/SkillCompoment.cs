using System;
using System.Collections.Generic;
using Assets.Script.Battle.BattleData;
using Assets.Script.Utility;

namespace Assets.Script.Battle
{
    public class SkillCompoment
    {
        private RoleBase mCurrentRole;
        private Dictionary<int, SkillLevelComponent> skillDataDic;
        private Dictionary<int, int> skillLevelDic;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            skillDataDic = new Dictionary<int, SkillLevelComponent>(4);
            skillLevelDic = new Dictionary<int, int>(4);
        }

        public void InitSkill(SkillSlotTypeEnum skillSlotType, int skillId)
        {
            SkillLevelComponent data = SkillDataMgr.instance.GetSkilLevelBySkillId(skillId);
            skillDataDic.Add((int)skillSlotType, data);
            SetSkillLevel(skillSlotType, 1);
        }

        public void SetSkillLevel(SkillSlotTypeEnum skillSlotType, int skillLevel)
        {
            skillLevelDic[(int) skillSlotType] = skillLevel;
        }

        public SkillData GetSkillDataBySkilSlot(SkillSlotTypeEnum skillSlotType)
        {
            int skillSlotIndex = (int) skillSlotType;
            int skillLevel = skillLevelDic[skillSlotIndex];
            SkillLevelComponent data = skillDataDic[skillSlotIndex];
            return data.GetSkillDataByLevel(skillLevel);
        }
    }
}
