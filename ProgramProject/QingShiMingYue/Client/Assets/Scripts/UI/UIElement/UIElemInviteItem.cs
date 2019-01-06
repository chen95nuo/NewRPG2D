using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemInviteItem : MonoBehaviour
{
	public SpriteText leftCenter;
	public SpriteText leftDown;

	public List<UIElemAssetIcon> rewards;
	public List<SpriteText> rewardCounts;

	public UIButton getRewardBtn;
	public UIBox getRewardBox;
	public UIBox getRewardNotify;
}
