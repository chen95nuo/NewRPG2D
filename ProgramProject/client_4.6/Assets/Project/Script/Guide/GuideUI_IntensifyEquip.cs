using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideUI_IntensifyEquip : GuideBase
{
	public static GuideUI_IntensifyEquip mInstance = null;
	
	public UILabel label0;
	public UILabel label1;
	public UILabel label2;
	public UILabel label3;
	public UILabel label4;
	public UILabel label5;
	
	//true为正常强化流程,false为通过列表进入强化的流程//
	public bool normalType = false;
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
	
	public override void init()
	{
		base.init();
		label0.text = TextsData.getData(246).chinese;
		label1.text = TextsData.getData(183).chinese;
		label2.text = TextsData.getData(183).chinese;
		label3.text = TextsData.getData(496).chinese;
		label4.text = TextsData.getData(247).chinese;
		label5.text = TextsData.getData(183).chinese;
		
		setClickBtnCount(7);
	}
	
	// unnormal  *****
	public void onClickTargetEquipPos()
	{
		if(btnClickList[0])
		{
			return;	
		}
		btnClickList[0] = true;
		CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager" ) as CardInfoPanelManager;
		cardInfo.openEquipList(3);
	}
	

	public void onClickIntensifyEquipBtn()
	{
		if(btnClickList[1])
		{
			return;	
		}
		btnClickList[1] = true;
		ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
			"ScrollViewPanel" ) as ScrollViewPanel;
		scrollView.onClickListItemIntensifyBtn(scrollView.finidGuideUITargetIndex(targetEquipID));
	}
	// *****
	
	// normal *****
	public void onClickOpenIntensifyPanel()
	{
		if(btnClickList[2])
		{
			return;	
		}
		btnClickList[2] = true;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openIntensify();
		}
	}
	
	public void onClickEquipType()
	{
		if(btnClickList[3])
		{
			return;	
		}
		btnClickList[3] = true;	
		IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
			"IntensifyPanel")as IntensifyPanel;
		intensify.onClickIntensifyPackCtrlsBtn(2);
	}
	
	public void onClickTargetEquip()
	{
		if(btnClickList[4])
		{
			return;	
		}
		btnClickList[4] = true;
		IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
			"IntensifyPanel")as IntensifyPanel;
		intensify.selectGuideTargetEquipCard();
		showStep(5);
	}
	// *****
	
	public void onClickIntensifyBtn()
	{
		if(btnClickList[5])
		{
			return;	
		}
		btnClickList[5] = true;
		
		IntensifyPanel intensify = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_INTENSIFY, 
			"IntensifyPanel")as IntensifyPanel;
		intensify.guideClickEquipIntensify();
	}
}
