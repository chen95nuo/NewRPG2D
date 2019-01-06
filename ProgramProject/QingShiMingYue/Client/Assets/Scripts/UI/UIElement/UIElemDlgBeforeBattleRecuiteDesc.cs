using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;


public class UIElemDlgBeforeBattleRecuiteDesc : MonoBehaviour
{
	public SpriteText desc;

	public void SetData(int dungeonId)
	{
		var config = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		if (config != null)
			desc.Text = config.recruiteDesc;
	}
}
