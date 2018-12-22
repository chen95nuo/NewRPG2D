using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//强化卡牌 //
public class GuideUI_Intesnify: GuideBase
{
	public static GuideUI_Intesnify mInstance = null;
	public UILabel label0;
	public UILabel label1;
	public UILabel label2;
	public UILabel label3;
	public UILabel label4;
	public UILabel label5;
	public UILabel label6;
	
	public SimpleCardInfo2 cardInfo;

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
		CardData cd = CardData.getData(21002);
		if(cd != null)
		{
			cardInfo.clear();
			cardInfo.setSimpleCardInfo(21002,GameHelper.E_CardType.E_Hero);
			label0.text = cd.name + " x 1";
		}
		
		label1.text = TextsData.getData(183).chinese;
		label2.text = TextsData.getData(178).chinese;
		label3.text = TextsData.getData(184).chinese;
		label4.text = TextsData.getData(183).chinese;
		label5.text = TextsData.getData(173).chinese;
		label6.text = TextsData.getData(173).chinese;
		
		setClickBtnCount(7);
		
	}
	// close pop window
	public void onClickClosePopWindow()
	{
		showStep(1);
		cardInfo.clear();
		Resources.UnloadUnusedAssets();
	}
	

	public void onClickOpenIntensifyPanel()
	{
		if(btnClickList[0])
		{
			return;	
		}
		btnClickList[0] = true;
//		MainMenuManager.mInstance.openIntensify();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openIntensify();
		}
	}
	
	public void onClickTargetCard()
	{
		if(btnClickList[1])
		{
			return;	
		}
		btnClickList[1] = true;
//		IntensifyPanel.mInstance.selectGuideTargetCard();
		IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
			"IntensifyPanel")as IntensifyPanel;
		intensify.selectGuideTargetCard();
	}
	
	public void onClickConsumeCard()
	{
		if(btnClickList[2])
		{
			return;	
		}
		btnClickList[2] = true;
//		IntensifyPanel.mInstance.guideSelectConsumeCard();
		IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
			"IntensifyPanel")as IntensifyPanel;
		intensify.guideSelectConsumeCard();
		showStep(4);
	}
	
	public void onClickIntensify()
	{
		if(btnClickList[3])
		{
			return;	
		}
		btnClickList[3] = true;
//		IntensifyPanel.mInstance.guideClickIntensify();
		IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
			"IntensifyPanel")as IntensifyPanel;
		intensify.guideClickIntensify();
	}
	
	public void onClickIntensifyBack1()
	{
		if(btnClickList[4])
		{
			return;	
		}
		btnClickList[4] = true;
//		IntensifyPanel.mInstance.guideClickIntensifyBackToBagBtn();
		IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
			"IntensifyPanel")as IntensifyPanel;
		intensify.guideClickIntensifyBackToBagBtn();
	}
	
	public void onClickIntensifyBack2()
	{
		if(btnClickList[5])
		{
			return;	
		}
		btnClickList[5] = true;
//		IntensifyPanel.mInstance.closeIntensifyPanel();	
		IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
			"IntensifyPanel")as IntensifyPanel;
		intensify.closeIntensifyPanel();	
		hide();
	}
	

	
}
