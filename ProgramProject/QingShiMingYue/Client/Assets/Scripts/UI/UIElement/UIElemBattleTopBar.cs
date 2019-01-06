using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemBattleTopBar : MonoBehaviour
{
    public GameObject parentObj;
    public SpriteText sponorName;
    public SpriteText opponentName;

	public void SetData(KodGames.ClientClass.BattleRecord battleRecord)
	{
		if (battleRecord.TeamRecords.Count < 2)
		{
			return;
		}

		// Set name
		this.sponorName.Text = battleRecord.TeamRecords[0].DisplayName;
		this.opponentName.Text = battleRecord.TeamRecords[1].DisplayName;
	}

    public void Hide(bool tf)
    {
        parentObj.SetActive(!tf);
    }
}
