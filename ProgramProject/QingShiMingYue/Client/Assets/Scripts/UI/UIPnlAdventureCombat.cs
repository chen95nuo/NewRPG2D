using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlAdventureCombat : UIModule
{
	public GameObject combatObject;
	public GameObject combatOrEscapeObject;

	public UIBox delaySign;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas)) return false;

		if (userDatas != null && userDatas.Length > 0)
		{
			var combatEvent = userDatas[0] as MarvellousAdventureConfig.CombatEvent;
			if (combatEvent.FleeGoToEventId == -1)
			{
				SetShowCombatBtn(true);
			}
			else
			{
				SetShowCombatBtn(false);
			}
		}

		return true;
	}

	private void Update()
	{
		if (AdventureSceneData.Instance.HadDelayReward > 0)
			delaySign.Hide(false);
		else delaySign.Hide(true);
	}

	private void SetShowCombatBtn(bool isShow)
	{
		combatObject.SetActive(isShow);
		combatOrEscapeObject.SetActive(!isShow);
	}

	public override void OnHide()
	{
		if (SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().IsShown)
			SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().OnHide();
		SysUIEnv.Instance.HideUIModule<UIPnlAdventureScene>();
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnHitButton(UIButton btn)
	{
		//需进入战斗场景进行战斗
		//SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.Tower, true);
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp),_CombatType.Adventure);
		//进行战斗，请求下一步
		//RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._DoubleSelectType.Yes, null));
		//HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnNoHitButton(UIButton btn)
	{
		if (SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().IsShown)
			SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().OnHide();
		//好言相劝，请求下一步
		RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._DoubleSelectType.No, -1, null));
		OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlAdventureMain));
		OnHide();
	}
}