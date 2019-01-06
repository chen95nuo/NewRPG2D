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


	//����ѯ����Э��ɹ�����
	public void OnQuerySuccess()
	{
		SetCloseActivity(false);
		eastSeaNumLable.Text = SysLocalDataBase.Inst.LocalPlayer.Zentia.ToString();
	}

	//�ܿ��عرշ��غ���
	public void OnQueryEastSeaZentiaError()
	{
		SetCloseActivity(true);
	}

	private void SetCloseActivity(bool isClose)
	{
		successObject.SetActive(!isClose);
		errorObject.SetActive(isClose);
	}

	//��Ե�һ���ť
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEastSeaExchangeBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlEastSeaElementItem));
		OnHide();
	}
}


