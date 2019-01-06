using System;
using System.Collections.Generic;
using UnityEngine;

public class UIAvatarSuperSkillBar : MonoBehaviour
{
	public UIAvatarBattleBar battleBar;

	//相对于血条的偏移，默认没有偏移，即覆盖在血条正上方
	public Vector3 offsetToBattleBar = new Vector3(0, -45, -0.01f);

	void Update()
	{
		Transform uiCamTrans = SysUIEnv.Instance.UICam.transform;

		CachedTransform.position = battleBar.WorldPos + KodGames.Math.RelativeOffset(uiCamTrans, offsetToBattleBar, false);
	}
}