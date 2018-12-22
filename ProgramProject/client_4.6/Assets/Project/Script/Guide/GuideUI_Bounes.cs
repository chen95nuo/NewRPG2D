using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//第2次战斗&合体技&BONUS //
public class GuideUI_Bounes : GuideBase
{
	public static GuideUI_Bounes mInstance = null;
	public int step = 0;
	
	public UILabel label0;
	public UILabel label1;
	public UILabel label2;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = gameObject;
		init();
	}
	
	// Use this for initialization
	void Start ()
	{
		close();
	}
	
	public override void init()
	{
		base.init();
		label0.text = TextsData.getData(170).chinese;
		label1.text = TextsData.getData(171).chinese;
		label2.text = TextsData.getData(172).chinese;
		
		setClickBtnCount(3);
	}

	public void onClickBattleBtn()
	{
		if(btnClickList[0])
		{
			return;	
		}
		btnClickList[0] = true;
//		MainMenuManager.mInstance.openMap();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openMap();
		}
	}
	
	public void onClickMissonOne()
	{
		if(btnClickList[1])
		{
			return;	
		}
		btnClickList[1] = true;
//		MissionUI.mInstance.onClickZone("1-1-1");
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.onClickZone("1-1-1");
		
		
		UISceneDialogPanel.mInstance.showDialogID(38);
		hideAllStep();
		//showStep(2);
	}

	public void onClickBattle2()
	{
		if(btnClickList[2])
		{
			return;	
		}
		btnClickList[2] = true;
//		MissionUI.mInstance.requestBattleByMissionID(110102);
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.requestBattleByMissionID(110102);
	}

}
