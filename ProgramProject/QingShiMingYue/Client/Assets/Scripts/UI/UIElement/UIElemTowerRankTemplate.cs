using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIElemTowerRankTemplate : MonoBehaviour
{
	public AutoSpriteControlBase playerBg1;
	public AutoSpriteControlBase playerBg2;

	public void SetTowerRankBg(AutoSpriteControlBase border, bool isMyRank)
	{
		UIUtility.CopyIcon(border, isMyRank ? playerBg2 : playerBg1);
	}
}
