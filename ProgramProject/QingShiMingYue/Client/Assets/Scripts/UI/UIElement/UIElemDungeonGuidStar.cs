using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDungeonGuidStar : MonoBehaviour
{
	public List<SpriteText> starDescLables;

	public void SetData(CampaignConfig.Dungeon dungeonCfg)
	{
		for (int index = 0; index < starDescLables.Count; index++)
			starDescLables[index].Text = string.Empty;

		for (int index = 0; index < starDescLables.Count && index < dungeonCfg.startCondition.Count; index++)
		{
			var condition = dungeonCfg.startCondition[index];

			string desc = _StarRewardEvaType.GetDisplayNameByType(condition.type, ConfigDatabase.DefaultCfg);

			if (condition.compareIntValue > 0 || condition.compareFloatValue > 0)
			{
				string valueStr = condition.compareIntValue > condition.compareFloatValue ? condition.compareIntValue.ToString() : condition.compareFloatValue.ToString("P");
				desc += string.Format("{0}{1}", _ConditionValueCompareType.GetDisplayNameByType(condition.compareType, ConfigDatabase.DefaultCfg), valueStr);
			}

			starDescLables[index].Text += desc;
		}
	}
}
