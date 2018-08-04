using System;
using System.Collections.Generic;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class SkillDataMgr : ItemDataBaseMgr<SkillDataMgr>
    {

        Dictionary<int, SkillLevelComponent> skillLevelDic = new Dictionary<int, SkillLevelComponent>();

        protected override XmlName CurrentXmlName
        {
            get {return XmlName.SkillData; }
        }

        public SkillLevelComponent GetSkilLevelBySkillId(int skillId)
        {
            SkillLevelComponent skillLevelData = null;
            if (skillLevelDic.TryGetValue(skillId, out skillLevelData))
            {
                return skillLevelData;
            }
            else
            {
                SkillData data;
                skillLevelData = new SkillLevelComponent(skillId);
                skillLevelDic[skillId] = skillLevelData;
                for (int i = 0; i < CurrentItemData.Length; i++)
                {
                    data = CurrentItemData[i] as SkillData;
                    if (data != null && data.SkillId == skillId)
                    {
                        skillLevelData.AddSkillData(data);
                    }
                }
                return skillLevelData;
            }
        }
    }
}
