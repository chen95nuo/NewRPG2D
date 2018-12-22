using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideUI17_WarpSpace : GuideBase 
{
	public static GuideUI17_WarpSpace mInstance = null;
	
	public UILabel label0;
	public UILabel label1;
	public UILabel label2;
	public UILabel label3;
	public UILabel label4;
	public UILabel label5;
	

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
		label2.text = TextsData.getData(232).chinese;
		label3.text = TextsData.getData(233).chinese;
		label4.text = TextsData.getData(234).chinese;
		label5.text = TextsData.getData(688).chinese;
		setClickBtnCount(6);
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
	
	public void onClickBattleActivityBtn()
	{
		if(btnClickList[1])
		{
			return;
		}
		btnClickList[1] = true;
		MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI2")as MissionUI2;
		mission2.onClickModelBtn(3);
		hideAllStep();
	}
	
	public void onClickOpenWarpSpace()
	{
		if(btnClickList[2])
		{
			return;
		}
		btnClickList[2] = true;
		hideAllStep();
//		MainUI.mInstance.openWarpSpace();
		MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI2")as MissionUI2;
		MainUI mainUI = mission2.activityPanel.GetComponent<MainUI>();
		mainUI.openWarpSpace();
		
		//MainUI mainUI = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CHALLENGE, "MainUI")as MainUI;
		//mainUI.openWarpSpace();
	}
	
	public void onClickGoBtn()
	{
		if(btnClickList[3])
		{
			return;
		}
		btnClickList[3] = true;
//		WarpSpaceUIManager.mInstance.OnEnterBtn();
		WarpSpaceUIManager warpSpace = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE, "WarpSpaceUIManager") as WarpSpaceUIManager;
		warpSpace.OnEnterBtn();
	}
	
	public void onClickMazePos1()
	{
		if(btnClickList[4])
		{
			return;
		}
		btnClickList[4] = true;
		hideAllStep();

		NewMazeUIManager maze = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE, "NewMazeUIManager")as NewMazeUIManager;
		maze.OnClickSelectCard(0);
		
	}
	
	public void onClickUseHP()
	{
		if(btnClickList[5])
		{
			return;
		}
		btnClickList[5] = true;
		
		hideAllStep();
		NewMazeUIManager maze = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE, "NewMazeUIManager")as NewMazeUIManager;
		maze.OnClickUseMedicine();
	}
	
}
