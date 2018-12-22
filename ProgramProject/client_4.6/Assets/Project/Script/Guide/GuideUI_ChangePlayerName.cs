using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideUI_ChangePlayerName : GuideBase
{
	public static GuideUI_ChangePlayerName mInstance = null;
	public int step = 0;
	
	public UILabel label0;
	public UILabel label1;
	public UILabel label2;
	public UILabel label3;
	

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
		label0.text = TextsData.getData(483).chinese;
		label1.text = TextsData.getData(484).chinese;
		label2.text = TextsData.getData(173).chinese;
		label3.text = TextsData.getData(170).chinese;
		setClickBtnCount(4);
	}
	
	public void onClickHead()
	{
		if(btnClickList[0])
		{
			return;	
		}
		btnClickList[0] = true;
		HeadUI.mInstance.OnClickHeadBtn();
	}
	
	public void onClickChangeName()
	{
		if(btnClickList[1])
		{
			return;	
		}
		btnClickList[1] = true;
		HeadSettingManager hsm = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_HEADSETTING,"HeadSettingManager") as HeadSettingManager;
		hsm.ChangeState(2);
		hideAllStep();
		UISceneDialogPanel.mInstance.showDialogID(49);
	}
	
	public void onClickBackBtn()
	{
		if(btnClickList[2])
		{
			return;	
		}
		btnClickList[2] = true;
		HeadSettingManager hsm = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_HEADSETTING,"HeadSettingManager") as HeadSettingManager;
		hsm.OnClickBackBtn();
	}
	
	public void onClickBattleBtn()
	{
		if(btnClickList[3])
		{
			return;	
		}
		btnClickList[3] = true;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openMap();
		}
		hideAllStep();
		GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_ChangePlayerName);
	}
	
}
