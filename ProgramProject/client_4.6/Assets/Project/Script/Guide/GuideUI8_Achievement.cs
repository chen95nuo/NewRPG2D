using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideUI8_Achievement : GuideBase
{
	public static GuideUI8_Achievement mInstance = null;
	public UILabel label0;
	public UILabel label1;
	int guideDIalogACMID = 10201;
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
		label0.text = TextsData.getData(237).chinese;
		label1.text = TextsData.getData(453).chinese;
		
		setClickBtnCount(2);
	}
	
	public void onClickAchievementBtn()
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
			main.openAchievement();
		}
	}
	
	public void onClickFinishGuideAchievement()
	{
		if(btnClickList[1])
		{
			return;	
		}
		btnClickList[1] = true;
		
		AchievementPanel achieve = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACHIVEMENT, "AchievementPanel")as AchievementPanel;
		achieve.finishACM(guideDIalogACMID);
	}
	
}
