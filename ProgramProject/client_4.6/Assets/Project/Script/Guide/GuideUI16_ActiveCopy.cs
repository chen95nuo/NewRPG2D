using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideUI16_ActiveCopy : GuideBase
{
	public static GuideUI16_ActiveCopy mInstance = null;
	
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
		label1.text = TextsData.getData(589).chinese;
		label2.text = TextsData.getData(294).chinese;
		
		setClickBtnCount(3);
	}
	
	public void onClickBattleBtn()
	{
		if(btnClickList[0])
		{
			return;
		}
		btnClickList[0] = true;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openMap();
		}
		showStep(1);
	}
	
	public void onClickBattleActivityBtn()
	{
		if(btnClickList[1])
		{
			return;
		}
		btnClickList[0] = true;
		MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI2")as MissionUI2;
		mission2.onClickModelBtn(3);
		//showStep(2);
		hideAllStep();
		
	}
	
	public void onClickActiveCopy()
	{
		if(btnClickList[2])
		{
			return;
		}
		btnClickList[2] = true;		
		//MainUI mainUI = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CHALLENGE, "MainUI")as MainUI;
		//mainUI.openAcitveCopy();
		MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI2")as MissionUI2;
		MainUI mainUI = mission2.activityPanel.GetComponent<MainUI>();
		mainUI.openAcitveCopy();
		hideAllStep();
	}

}
