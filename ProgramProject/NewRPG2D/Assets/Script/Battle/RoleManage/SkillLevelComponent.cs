using System;
using System.Collections.Generic;
using Assets.Script.Battle.BattleData;

namespace Assets.Script.Battle
{
    public class SkillLevelComponent
    {
        private List<SkillData> skillDataList;
        public int CurrentSkillId;

        public SkillLevelComponent(int skillId)
        {
            CurrentSkillId = skillId;
            skillDataList = new List<SkillData>();
        }

        public void AddSkillData(SkillData data)
        {
            skillDataList.Add(data);
        }

        public SkillData GetSkillDataByLevel(int skillLevel)
        {
            foreach (var data in skillDataList)
            {
                if (data.SkillLevel == skillLevel)
                {
                    return data;
                }
            }

            DebugHelper.LogError(" don't find the skill level data " + skillLevel);

            return null;
        }

    }
}
