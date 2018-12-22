using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//使用三连强袭 //
public class GuideUI_UseCombo3 : GuideBase
{
	public static GuideUI_UseCombo3 mInstance = null;

	public int step = 0;
	
	public UILabel label0;
	public UILabel label1;
	public UILabel label2;
	
	// singleGuide 为false时是在卡牌上阵指引结束后继续的新手指引，为true则是登陆后 卡牌上阵完成但未完成使用合体技 //
	public bool singleGuide = false;
	
	int combo3ID = 50001;

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
		label1.text = TextsData.getData(453).chinese;
		label2.text = TextsData.getData(173).chinese;
		setClickBtnCount(3);
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
	
	public void onClickUseCombo3()
	{
		if(btnClickList[1])
		{
			return;
		}
		btnClickList[1] = true;
		CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
		combination.OnClickUniteListItemUseBtn(combo3ID);
		showStep(2);
	}
	
	
	public void onClickBackToMainBtn()
	{
		if(btnClickList[2])
		{
			return;	
		}
		btnClickList[2] = true;
		CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager")as CombinationInterManager;
		combination.onClickBackBtn();
			
		GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_UseCombo3);
		hide();
	}
	

}
