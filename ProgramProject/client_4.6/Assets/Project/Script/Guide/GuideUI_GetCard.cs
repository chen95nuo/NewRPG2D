using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//抽卡 //
public class GuideUI_GetCard : GuideBase
{
	public static GuideUI_GetCard mInstance = null;
	
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
		label0.text = TextsData.getData(174).chinese;
		label1.text = TextsData.getData(190).chinese;
		label2.text = TextsData.getData(191).chinese;
		label3.text = TextsData.getData(173).chinese;
		setClickBtnCount(4);
	}
		
	public void onClickOpenLotPanel()
	{
		if(btnClickList[0])
		{
			return;	
		}
		btnClickList[0] = true;
//		MainMenuManager.mInstance.openLotPanel();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openLotPanel();
		}
	}
	
	public void onClickLotOneCard()
	{
		if(btnClickList[1])
		{
			return;	
		}
		btnClickList[1] = true;
//		LotCardUI.mInstance.onClickBtn(4);
		LotCardUI lotCard = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT, "LotCardUI")as LotCardUI;
		lotCard.onClickBtn(2);
	}

	public void clickLotOkBtn()
	{
		if(btnClickList[2])
		{
			return;	
		}
		btnClickList[2] = true;
//		LotCardUI.mInstance.clickOKBtn();
		LotCardUI lotCard = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT, "LotCardUI")as LotCardUI;
		lotCard.clickOKBtn();
		showStep(3);
	}
	
	public void onClickLotBackBtn()
	{
		if(btnClickList[3])
		{
			return;	
		}
		btnClickList[3] = true;
//		LotCardUI.mInstance.onClickBack();
		LotCardUI lotCard = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT, "LotCardUI")as LotCardUI;
		lotCard.onClickBack();
		GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_GetCard);
		hide();
	}
	
}
