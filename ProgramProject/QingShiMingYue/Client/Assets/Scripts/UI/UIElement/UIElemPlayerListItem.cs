using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemPlayerListItem : MonoBehaviour
{
	public UIElemAssetIcon playerIcon;
	public SpriteText playerName;
	public SpriteText playerLevel;
	public UIBox playerSelect;
	public UIProgressBar hpProgress;//显示血量
	public UIBox playerDieAll;
	public UIBox playerDieAllBg;
}
