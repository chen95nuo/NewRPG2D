using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideUI9_GetCard2 : GuideBase 
{
	public static GuideUI9_GetCard2 mInstance = null;
	
	public UILabel label0;
	public UILabel label1;
	
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
		label1.text = TextsData.getData(457).chinese;
		setClickBtnCount(2);
	}
	
	public void onClickOpenLotPanel()
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
		LotCardUI lotCard = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_LOT, "LotCardUI")as LotCardUI;
		lotCard.onClickBtn(4);
		// to do 
		
	}
	
}
