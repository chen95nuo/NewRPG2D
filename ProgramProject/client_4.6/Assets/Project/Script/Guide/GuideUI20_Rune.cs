using UnityEngine;
using System.Collections;

public class GuideUI20_Rune : GuideBase
{
	public static GuideUI20_Rune mInstance = null;
	
	public UILabel label0;
	
	
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
		label0.text = TextsData.getData(235).chinese;
		setClickBtnCount(1);
	}
	
	public void onClickRuneBtn()
	{
		if(btnClickList[0])
		{
			return;
		}
		btnClickList[0] = true;
//		MainMenuManager.mInstance.openRune();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openRune();
		}
	}

}
