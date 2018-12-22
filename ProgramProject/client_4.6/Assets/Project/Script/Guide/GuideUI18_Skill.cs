using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GuideUI18_Skill : GuideBase
{
	public static GuideUI18_Skill mInstance = null;
	
	public bool isOffLine = false;
	
	public UILabel label0;
	
	public UILabel label1;
	public UILabel label2;
	public UILabel label3;
	public UILabel label4;
	public UILabel label5;
	public UILabel label6;
	public UILabel label7;
	public UILabel label8;
	
	public UILabel label9;
	
		
	int guideSkillID = 13021;
	
	public int  needRunGuideByFinishBattle =0; // 0 lose battel , 1 win battle
	
	public SimpleCardInfo2 cardInfo;
	
	public GuideTeamPos guideTeamPos;
	
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
		
		SkillData sd = SkillData.getData(guideSkillID);
		if(sd != null)
		{
			cardInfo.clear();
			cardInfo.setSimpleCardInfo(guideSkillID,GameHelper.E_CardType.E_Skill);
			label0.text = TextsData.getData(296).chinese + sd.name;
		}
		label1.text = TextsData.getData(453).chinese;
		label2.text = TextsData.getData(173).chinese;
		label3.text = TextsData.getData(173).chinese;
		label4.text = TextsData.getData(173).chinese;
		label5.text = TextsData.getData(176).chinese;
		label6.text = TextsData.getData(291).chinese;
		label7.text = TextsData.getData(292).chinese;
		label8.text = TextsData.getData(293).chinese;
		label9.text = TextsData.getData(453).chinese;
		
		setClickBtnCount(11);
	}
	
	public override void showStep(int step)
	{
		base.showStep(step);
		if(step == 6)
		{
			CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
			int pos = combination.findCardGroupFirstExistCardPos();
			guideTeamPos.showPosCtrl(pos,TextsData.getData(290).chinese);
		}
	}
	
	public void onClickClosePopWindow()
	{
		if(btnClickList[0])
		{
			return;
		}
		btnClickList[0] = true;
		UISceneDialogPanel.mInstance.showDialogID(34);
		hideAllStep();
		cardInfo.clear();
		Resources.UnloadUnusedAssets();
	}
	
	public void onClickBox()
	{
		if(btnClickList[1])
		{
			return;
		}
		btnClickList[1] = true;
		hideAllStep();
		NewMazeUIManager maze = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE, "NewMazeUIManager")as NewMazeUIManager;
		maze.OnclickBossReward(0);
	}
	
	public void onClickBackToWarpSpace()
	{
		if(btnClickList[10])
		{
			return;
		}
		btnClickList[10] = true;
		hideAllStep();
		NewMazeUIManager maze = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE, "NewMazeUIManager")as NewMazeUIManager;
		maze.OnClickBack(1);
		
	}
	
	public void onClickBackToMainUI()
	{
		if(btnClickList[2])
		{
			return;
		}
		btnClickList[2] = true;
		showStep(3);
//		WarpSpaceUIManager.mInstance.OnBackClick();
		WarpSpaceUIManager warpSpace = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE, "WarpSpaceUIManager") as WarpSpaceUIManager;
		warpSpace.OnBackClick();
		
	}
	
	public void onClickBackToMission()
	{
		if(btnClickList[3])
		{
			return;
		}
		btnClickList[3] = true;
		hideAllStep();
//		MainUI.mInstance.OnBackBtn();
		//MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI2")as MissionUI2;
		//MainUI mainUI = mission2.activityPanel.GetComponent<MainUI>();
	//	mainUI.OnBackBtn();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.backToMainMenu();
		hideAllStep();
	}
	
	public void onClickBackToMainMenu()
	{
		if(btnClickList[4])
		{
			return;
		}
		btnClickList[4] = true;
		hideAllStep();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.backToMainMenu();
	}
	
	public void onClickOpenCombinationPanel()
	{
		if(btnClickList[5])
		{
			return;
		}
		btnClickList[5] = true;
//		MainMenuManager.mInstance.openCardGroup();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openCardGroup();
		}
	}
	
	
	public void onClickSkill()
	{
		if(btnClickList[7])
		{
			return;
		}
		btnClickList[7] = true;
//		CardInfoPanelManager.mInstance.openSkillList(1);
		CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
		cardInfo.openSkillList(1);
	}
	
	public void onClickSkillItem()
	{
		if(btnClickList[8])
		{
			return;
		}
		btnClickList[8] = true;
//		ScrollViewPanel.mInstance.clickScrollViewItem(ScrollViewPanel.mInstance.finidGuideUITargetIndex(guideSkillID));
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		scrollView.clickScrollViewItem(scrollView.finidGuideUITargetIndex(guideSkillID));
		showStep(9);
	}
	
	public void onClickUseSkillBtn()
	{
		if(btnClickList[9])
		{
			return;
		}
		btnClickList[9] = true;
		hideAllStep();
//		ScrollViewPanel.mInstance.onClickListItemUseBtn(ScrollViewPanel.mInstance.finidGuideUITargetIndex(guideSkillID));
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		scrollView.onClickListItemUseBtn(scrollView.finidGuideUITargetIndex(guideSkillID));
	}
	
}
