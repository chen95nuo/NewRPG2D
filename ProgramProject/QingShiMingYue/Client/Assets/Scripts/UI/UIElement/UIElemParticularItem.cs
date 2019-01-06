using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemParticularItem : MonoBehaviour
{
	public SpriteText titleLabel;
	public SpriteText descLabel;

	public void SetData(int StargId, int GainId)
	{
		titleLabel.Text = GameUtility.FormatUIString("UIDlgWolfParticular_Title",
			ItemInfoUtility.GetLevelCN(ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetStageSequenceById(StargId)));

		string strDesc = string.Empty;
		var config = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetAdditionById(GainId);
		if (config != null)
		{
			switch (config.EmBattleAttribute.type)
			{
				case PositionConfig._EmBattleType.Back:
					strDesc = GameUtility.FormatUIString("UIDlgWolfParticular_Back") + config.EmBattleAttribute.desc;
					break;
				case PositionConfig._EmBattleType.Front:
					strDesc = GameUtility.FormatUIString("UIDlgWolfParticular_Fron") + config.EmBattleAttribute.desc;
					break;
				case PositionConfig._EmBattleType.All:
					strDesc = config.EmBattleAttribute.desc;
					break;
			}
		}

		descLabel.Text = strDesc;
	}
}
