using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlAdventureQuestionAnswer : UIModule
{
	public List<UIElemAdventureQuestion> questions;
	public UIBox answerBG;
	public UIBox delaySign;

	private MarvellousAdventureConfig.SelectionEvent selectionEvent;

	private List<MarvellousAdventureConfig.SelectionItem> selectionItems;

	private float[] sizes = new float[] { 0, 62, 108, 154, 200 };

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas)) return false;

		ResetUIAndData();

		selectionEvent = userDatas[0] as MarvellousAdventureConfig.SelectionEvent;

		if (selectionEvent == null) return false;

		selectionItems = selectionEvent.SelectionItem;

		answerBG.SetSize(answerBG.width, sizes[selectionItems.Count]);

		for (int i = 0; i < selectionItems.Count; i++)
		{
			if (i < questions.Count)
			{
				var selectionItem = selectionItems[i];
				questions[i].gameObject.SetActive(true);
				questions[i].SetData(selectionItem.Content);
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

	private void ResetUIAndData()
	{
		foreach (var question in questions)
			question.gameObject.SetActive(false);
		
	}

	public override void OnHide()
	{
		if (SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().IsShown)
			SysUIEnv.Instance.GetUIModule<UIPnlAdventureMessage>().OnHide();
		SysUIEnv.Instance.HideUIModule<UIPnlAdventureScene>();
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectedButtonA(UIButton btn)
	{
		
		RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._MultiSelectType.A, -1, null));
		OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectedButtonB(UIButton btn)
	{
		
		  
		RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._MultiSelectType.B, -1, null));
		OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectedButtonC(UIButton btn)
	{

		RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._MultiSelectType.C, -1, null));
		    OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectedButtonD(UIButton btn)
	{

		RequestMgr.Inst.Request(new MarvellousNextMarvellousReq(ClientServerCommon._MultiSelectType.D, -1, null));
		    OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlAdventureMain));
		OnHide();
	}
}