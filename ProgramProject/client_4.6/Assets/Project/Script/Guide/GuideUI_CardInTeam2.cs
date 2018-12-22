using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//齐格飞上阵 //
public class GuideUI_CardInTeam2 : GuideBase
{
	public static GuideUI_CardInTeam2 mInstance = null;
	
	public GuideTeamPos guideTeamPos; 
	
	public int step = 0;
	
	public UILabel label0;
	public UILabel label2;
	public UILabel label3;
	public UILabel label4;
	public UILabel label5;
	public UILabel label6;
	public UILabel label7;

	
	int targetCardFormID = 14003;
	//int targetCardIndexInList = 3;
	
	int newUnitSkillID = 50002;
	
	public bool needShowDialog = false;

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
		label0.text = TextsData.getData(176).chinese;
		//label1.text = TextsData.getData(177).chinese;
		//label2.text = TextsData.getData(193).chinese;
		label3.text = TextsData.getData(586).chinese;
		label4.text = TextsData.getData(179).chinese;
		label5.text = TextsData.getData(173).chinese;
		label6.text = TextsData.getData(453).chinese;
		label7.text = TextsData.getData(453).chinese;
			
		setClickBtnCount(8);
	}
	
	public override void showStep(int step)
	{
		base.showStep(step);
		if(step == 1)
		{
			CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
			int pos = combination.findCardGroupFirstEmptyPos();
			guideTeamPos.showPosCtrl(pos,TextsData.getData(177).chinese);
		}
	}
	
	public void onClickOpenCombinationPanel()
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
			main.openCardGroup();
		}
	}
	
	public void onClickEmptyCard()
	{
		
		if(btnClickList[2])
		{
			return;	
		}
		btnClickList[2] = true;
		CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
		cardInfo.openCardList(0);
	}
	
	public void onSelectTargetCard()
	{
		if(btnClickList[3])
		{
			return;	
		}
		btnClickList[3] = true;
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		scrollView.clickScrollViewItem(scrollView.finidGuideUITargetIndex(targetCardFormID));
		showStep(4);
	}
	
	public void onClickUseCardBtn()
	{
		needShowDialog = true;
		if(btnClickList[4])
		{
			return;	
		}
		btnClickList[4] = true;
		hideAllStep();
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		scrollView.onClickListItemUseBtn(scrollView.finidGuideUITargetIndex(targetCardFormID));
		
	}
	
	public void onClickBackToCombinationPanel()
	{
		if(btnClickList[5])
		{
			return;	
		}
		btnClickList[5] = true;
	
		CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
		cardInfo.CardInfo_Back();
	
		hideAllStep();
		UISceneDialogPanel.mInstance.showDialogID(51);
	}
	
	public void onClickUseNewUnitskill()
	{
		if(btnClickList[6])
		{
			return;
		}
		btnClickList[6] = true;
		CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
		//combination.cancelAllSelectUnitSkill();
		combination.OnClickUniteListItemUseBtn(newUnitSkillID);
		hideAllStep();

	}
	
	public void onClickOpenUnitSkillPanel()
	{
		if(btnClickList[7])
		{
			return;
		}
		btnClickList[7] = true;
		
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.OpenUniteSkillPanel();
		}
	}
	
	/*public void onClickBackToMainBtn()
	{
		if(btnClickList[7])
		{
			return;	
		}
		btnClickList[7] = true;
//		CombinationInterManager.mInstance.onClickBackBtn();
		CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
		combination.onClickBackBtn();
		
		GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_CardInTeam);
		hide();
	}
	*/

}
