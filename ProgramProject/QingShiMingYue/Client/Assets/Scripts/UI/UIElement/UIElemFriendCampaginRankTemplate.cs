using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIElemFriendCampaginRankTemplate : MonoBehaviour
{
	public AutoSpriteControlBase playerBg1;
	public AutoSpriteControlBase playerBg2;

	public void SetFriendCampaginRankBg(AutoSpriteControlBase border, bool isMyRank)
	{
		UIUtility.CopyIcon(border, isMyRank ? playerBg2 : playerBg1);
	}
}
