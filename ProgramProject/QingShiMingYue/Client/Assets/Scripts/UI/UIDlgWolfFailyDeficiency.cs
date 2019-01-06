using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgWolfFailyDeficiency : UIModule
{
	private enum StartType
	{
		Rest_Own,		//用重置次数
		Rest_Not,		//没有重置次数
		Rest_Two,		//重置第二次确认面板
		Rest_Success,	//重置成功
		Rest_NULL
	};

	public UIChildLayoutControl layoutCoutrol;

	private StartType type;//渲染控制
	private KodGames.ClientClass.WolfInfo wolfInfo;

	//共同的标题显示
	public SpriteText titleLabel;

	//可以重置方面的界面显示
	public SpriteText restartMessageLabel;
	public SpriteText restartNumberLabel;
	public UIBox restartBox;

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

		this.wolfInfo = userDatas[0] as KodGames.ClientClass.WolfInfo;

		CalCulatetion();
		ShowType();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	private int GetResetResidueCount()
	{
		return ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount) - wolfInfo.AlreadyResetTimes;
	}

	private int GetResetAllCount()
	{
		return ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount);
	}

	//计算渲染类型
	private void CalCulatetion()
	{
		if (GetResetResidueCount() > 0)
			type = StartType.Rest_Own;
		else
			type = StartType.Rest_Not;
	}

	//根据类型进行按钮文字渲染
	private void ShowType()
	{
		switch (type)
		{
			//可以重置
			case StartType.Rest_Own:
				{
					SetShow();
					restartBox.Hide(false);
					titleLabel.Text = GameUtility.GetUIString("UIDlgWolfFailyDeficiency_Restart_Title");

					restartMessageLabel.Text = GameUtility.GetUIString("UIDlgWolfFailyDeficiency_Restart_Message");
					restartNumberLabel.Text = GameUtility.FormatUIString("UIDlgWolfFailyDeficiency_Restart_NumberMessage",
												GameDefines.textColorBtnYellow.ToString(),
												GameDefines.textColorWhite.ToString(),
												GetResetResidueCount(),
												GetResetAllCount());

					SetLayoutCoutrol(false, false, true, true);
				}
				break;
			//重置次数不足
			case StartType.Rest_Not:
				SetShow();
				titleLabel.Text = GameUtility.GetUIString("UIDlgWolfFailyDeficiency_Restart_Title");

				restartMessageLabel.Text = GameUtility.GetUIString("UIDlgWolfFailyDeficiency_Restart_ficiency");
				restartBox.Hide(false);
				restartNumberLabel.Text = GameUtility.FormatUIString("UIDlgWolfFailyDeficiency_Restart_NumberMessage",
												GameDefines.textColorBtnYellow,
												GameDefines.textColorWhite,
												GetResetResidueCount(),
												GetResetAllCount());
				SetLayoutCoutrol(false, true, true, true);
				break;
			//点击重置出现二次确认面板
			case StartType.Rest_Two:
				SetShow();
				restartBox.Hide(false);
				titleLabel.Text = GameUtility.GetUIString("UIDlgWolfFailyDeficiency_Succeed_Title");

				restartMessageLabel.Text = GameUtility.GetUIString("UIDlgWolfFailyDeficiency_Succeed_MessageLabel");
				restartNumberLabel.Text = GameUtility.FormatUIString("UIDlgWolfFailyDeficiency_Restart_NumberMessage",
												GameDefines.textColorBtnYellow.ToString(),
												GameDefines.textColorWhite.ToString(),
												GetResetResidueCount(),
												GetResetAllCount());

				SetLayoutCoutrol(true, true, false, false);
				break;
			//重置成功
			case StartType.Rest_Success:
				SetShow();
				titleLabel.Text = GameUtility.GetUIString("UIDlgWolfFailyDeficiency_Succeed_Title");

				restartMessageLabel.Text = GameUtility.GetUIString("UIDlgWolfFailyDeficiency_Succeed_ResMessageLabel");
				SetLayoutCoutrol(false, true, true, true);
				break;
		}
	}

	private void SetLayoutCoutrol(params bool[] hideF)
	{
		//关闭 重置 【点击重置后】 取消 保存
		for (int index = 0; index < Mathf.Min(layoutCoutrol.childLayoutControls.Length, hideF.Length); index++)
			layoutCoutrol.HideChildObj(layoutCoutrol.childLayoutControls[index].gameObject, hideF[index]);
	}

	private void SetShow()
	{
		restartBox.Hide(true);
		restartMessageLabel.Text = "";
		restartNumberLabel.Text = "";
		titleLabel.Text = "";
	}

	public void ResetWolfSmoke()
	{
		type = StartType.Rest_Success;
		ShowType();
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

	//点击重新开始
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnRestart(UIButton btn)
	{
		type = StartType.Rest_Two;
		ShowType();
	}

	//点击军需堂
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnGotoShop(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlWolfNormalShop));

		HideSelf();
	}

	//点击重置
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnRestartSucceed(UIButton btn)
	{
		//发送重置协议
		RequestMgr.Inst.Request(new QueryResetWolfSmoke());
	}

	#endregion
}
