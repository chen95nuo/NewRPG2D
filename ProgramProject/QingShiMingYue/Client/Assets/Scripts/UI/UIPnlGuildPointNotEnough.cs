using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildPointNotEnough : UIModule
{
	public SpriteText messageLabel;
	public UIElemAssetIcon gotoIcon;
	public UIElemAssetIcon gotoIcon2;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		gotoIcon.SetData(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.GuildBuildIconId);
		gotoIcon2.SetData(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.AdventureIconId);
		
		messageLabel.Text = GameUtility.GetUIString("UIPnlGuildPointMain_NotMoveMessage");

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGotoClickOne(UIButton btn)
	{
		SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlGuildConstruct));	
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGotoClickTwo(UIButton btn)
	{
		SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlAdventureMain));	
	}
}

