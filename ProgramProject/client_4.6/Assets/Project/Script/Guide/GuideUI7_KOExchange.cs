using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GuideUI7_KOExchange : GuideBase
{
	public static GuideUI7_KOExchange mInstance = null;
	
	public bool finishExchange = false;
	
	public UILabel lable0;
	public UILabel lable1;
	public UILabel label2;
	public UILabel label3;
	public UILabel label4;
	//public UILabel label5;
	
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
		lable0.text = TextsData.getData(170).chinese;
		lable1.text = TextsData.getData(173).chinese;
		label2.text = TextsData.getData(453).chinese;
		label3.text = TextsData.getData(453).chinese;
		//label4.text = TextsData.getData(454).chinese;
		label4.text = TextsData.getData(455).chinese;
		setClickBtnCount(5);
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
	}
	
	public void onClickCloseZoneBtn()
	{
		if(btnClickList[1])
		{
			return;	
		}
		btnClickList[1] = true;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.closePop();
		hideAllStep();
		UISceneDialogPanel.mInstance.showDialogID(40);
	}
	
	public void onClickOpenKOExchangeBtn()
	{
		if(btnClickList[2])
		{
			return;	
		}
		btnClickList[2] = true;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.onClickKOAwardBtn();
	}
	
	public void onClickExchange()
	{
		if(btnClickList[3])
		{
			return;	
		}
		btnClickList[3] = true;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.koaward.OnClickKoExchange(1); 
	}
	
	public void onClickGoTeamBtn()
	{
		if(btnClickList[4])
		{
			return;	
		}
		btnClickList[4] = true;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.koaward.warnningSure();
	}
	
}
