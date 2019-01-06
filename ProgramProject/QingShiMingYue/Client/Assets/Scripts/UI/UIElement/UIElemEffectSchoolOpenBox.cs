using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;
using com.kodgames.corgi.protocol;

public class UIElemEffectSchoolOpenBox : MonoBehaviour
{
	public UIElemAssetIcon rewardIcon;

	public void SetData(ShowReward showReward)
	{
		rewardIcon.SetData(showReward.id, showReward.count);
	}
}
