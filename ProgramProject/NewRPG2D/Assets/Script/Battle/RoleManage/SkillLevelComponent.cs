using System;
using System.Collections.Generic;
using Assets.Script.Battle.BattleData;

namespace Assets.Script.Battle
{

    public class SkillUseData
    {
        public float CurrentCD;
        public int SkillLevel;
        public float SkillCDTime;
        public int MP;
        public SkillTypeEnum SkillType;
        public int TargetCount;
        public int EffectId1;
        public int EffectId2;
        public int EffectId3;
        public int EffectId4;

        public void Init()
        {
            CurrentCD = 0;
        }

        public void UpdateLogic(float deltaTime)
        {
            CurrentCD += deltaTime;
        }

        public void UseSkill()
        {
            CurrentCD = 0;
        }

        public void SetCurrentCD(float time)
        {
            CurrentCD = time;
        }

    }

    public class SkillLevelComponent
    {
        private List<SkillData> skillDataList;
        private List<SkillUseData> SkillUseDataList;
        public int CurrentSkillId;

        public SkillLevelComponent(int skillId)
        {
            CurrentSkillId = skillId;
            skillDataList = new List<SkillData>();
            SkillUseDataList = new List<SkillUseData>();
        }

        public void AddSkillData(SkillData data)
        {
            skillDataList.Add(data);
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
            useData.Init();
            SkillUseDataList.Add(useData);
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

        public SkillUseData GetSkillUseDataByLevel(int skillLevel)
        {
            foreach (var data in SkillUseDataList)
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
