using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideUI12_Equip : GuideBase
{
	public static GuideUI12_Equip mInstance = null;
	
	public GuideTeamPos guideTeamPos;
	
	public UILabel label0;
	public UILabel label1;
	public UILabel label2;
	//public UILabel label3;
	public UILabel label4;
	public UILabel label5;
	public UILabel label6;
	
	public int needRunStep = 0;
	public int targetEquipID = 1101;
	
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
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public override void init()
	{
		base.init();
		
		label0.text = TextsData.getData(173).chinese;
		label1.text = TextsData.getData(173).chinese;
		label2.text = TextsData.getData(176).chinese;
		//label3.text = TextsData.getData(290).chinese;
		label4.text = TextsData.getData(246).chinese;
		label5.text = TextsData.getData(247).chinese;
		label6.text = TextsData.getData(248).chinese;
		setClickBtnCount(7);
	}
	
	public override void showStep(int step)
	{
		base.showStep(step);
		if(step == 3)
		{
			CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
			int pos = combination.findCardGroupFirstExistCardPos();
			guideTeamPos.showPosCtrl(pos,TextsData.getData(290).chinese);
		}
	}
	
	public void onClickMissionBack1()
	{
		if(btnClickList[0])
		{
			return;	
		}
		btnClickList[0] = true;
//		MissionUI.mInstance.closeMissionList();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,"MissionUI")as MissionUI;
		mission.closePop();
		showStep(1);
	}
	
	public void onClickMissionBack2()
	{
		if(btnClickList[1])
		{
			return;	
		}
		btnClickList[1] = true;
//		MissionUI.mInstance.backToMainMenu();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.backToMainMenu();
		hideAllStep();
	}
	
	public void onClickOpenCombinationPanel()
	{
		if(btnClickList[2])
		{
			return;	
		}
		btnClickList[2] = true;
//		MainMenuManager.mInstance.openCardGroup();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openCardGroup();
		}
	}

	public void onClickEmptyEquipPos()
	{
		if(btnClickList[4])
		{
			return;	
		}
		btnClickList[4] = true;
		hideAllStep();
//		CardInfoPanelManager.mInstance.openEquipList(3);
		CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
		cardInfo.openEquipList(3);
	}
	
	public void onClickEquipItem()
	{
		if(btnClickList[5])
		{
			return;	
		}
		btnClickList[5] = true;
//		ScrollViewPanel.mInstance.clickScrollViewItem(ScrollViewPanel.mInstance.finidGuideUITargetIndex(targetEquipID));
		
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		scrollView.clickScrollViewItem(scrollView.finidGuideUITargetIndex(targetEquipID));
		
		showStep(6);
	}
	
	public void onClickUseEquipBtn()
	{
		if(btnClickList[6])
		{
			return;	
		}
		btnClickList[6] = true;
		hideAllStep();
//		ScrollViewPanel.mInstance.onClickListItemUseBtn(ScrollViewPanel.mInstance.finidGuideUITargetIndex(targetEquipID));
		
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		scrollView.onClickListItemUseBtn(scrollView.finidGuideUITargetIndex(targetEquipID));
		
		//GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Equip);
	}
}
