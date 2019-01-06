using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlEastSeaCloseActivity : UIPnlEastSeaFindFairyTimer
{
	public SpriteText eastSeaNumLable;
	public GameObject successObject;
	public GameObject errorObject;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		successObject.SetActive(false);
		errorObject.SetActive(false);

		return true;
	}


	//主查询请求协议成功返回
	public void OnQuerySuccess()
	{
		SetCloseActivity(false);
		eastSeaNumLable.Text = SysLocalDataBase.Inst.LocalPlayer.Zentia.ToString();
	}

	//总开关关闭返回函数
	public void OnQueryEastSeaZentiaError()
	{
		SetCloseActivity(true);
	}

	private void SetCloseActivity(bool isClose)
	{
		successObject.SetActive(!isClose);
		errorObject.SetActive(isClose);
	}

	//仙缘兑换按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEastSeaExchangeBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlEastSeaElementItem));
		OnHide();
	}
}


