using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgWolfStart : UIModule
{
	private enum StartType
	{
		Rest_Own,		//拥有重置次数
		Rest_Not,		//没有重置次数
		Rest_Success,	//重置成功
		Rest_NULL
	};

	public UIChildLayoutControl layoutCoutrol;//按钮组

	public SpriteText message;//描述
	public SpriteText startNumberLabel;//数据显示
	public UIBox startBox;//数据显示上层

	private StartType type;//界面控制
	private int restetCount;//当前重置次数
	private int alreadyResetTimes;//已经用过的次数

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		type = StartType.Rest_NULL;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		this.alreadyResetTimes = (int)userDatas[0];

		SetType();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	//刚进来，计算type
	private void SetType()
	{
		//判断可以重置次数是否足够
		restetCount = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount) - alreadyResetTimes;
		Debug.Log(ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount));
		if (restetCount > 0)
			type = StartType.Rest_Own;
		else
			type = StartType.Rest_Not;

		SetShowUI();
	}

	//根据Type进行渲染界面
	private void SetShowUI()
	{
		//渲染之前，把所有界面上的东西清空一次
		ClearUI();

		switch (type)
		{
			//拥有重置次数
			case StartType.Rest_Own:
				startBox.Hide(false);
				message.Text = GameUtility.GetUIString("UIDlgWolfStart_StartMessage");
				startNumberLabel.Text = GameUtility.FormatUIString("UIDlgWolfStart_StartLabel", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(),
										restetCount,
										ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount));

				SetLYC(false, false, true, true);
				break;
			//没有重置次数
			case StartType.Rest_Not:
				startBox.Hide(false);
				message.Text = GameUtility.GetUIString("UIDlgWolfStart_NoStartMessage");
				startNumberLabel.Text = GameUtility.FormatUIString("UIDlgWolfStart_StartLabel", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(),
										restetCount,
										ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount));

				SetLYC(false, true, true, false);
				break;
			//重置成功
			case StartType.Rest_Success:

				message.Text = GameUtility.GetUIString("UIDlgWolfStart_SucceedMessage");

				SetLYC(true, true, false, true);
				break;
		}
	}

	private void ClearUI()
	{
		message.Text = "";
		startNumberLabel.Text = "";
		startBox.Hide(true);
	}

	private void SetLYC(params bool[] hideF)
	{
		for (int index = 0; index < Mathf.Min(hideF.Length, layoutCoutrol.childLayoutControls.Length); index++)
			layoutCoutrol.HideChildObj(layoutCoutrol.childLayoutControls[index].gameObject, hideF[index]);
	}

	public void ResetWolfSmoke()
	{
		type = StartType.Rest_Success;
		SetShowUI();
	}

	#region OnClick

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		//如果是重置成功的界面关闭，那么回到主城
		if (type == StartType.Rest_Success)
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>();

		HideSelf();
	}

	//点击重置 发送协议
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnRestart(UIButton btn)
	{
		//发送重置协议
		RequestMgr.Inst.Request(new QueryResetWolfSmoke());
	}

	//点击军需堂
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnGotoShop(UIButton btn)
	{
		if (SysGameStateMachine.Instance.CurrentState is GameState_Battle)
			RequestMgr.Inst.Request(new QueryWolfSmoke());
		else
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfNormalShop));
			HideSelf();
		}
	}

	#endregion
}
