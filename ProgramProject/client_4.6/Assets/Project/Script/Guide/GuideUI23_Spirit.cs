using UnityEngine;
using System.Collections;

public class GuideUI23_Spirit : GuideBase
{
	public static GuideUI23_Spirit mInstance = null;
	
	public UILabel label0;
	public UILabel label1;
	
	
	void Awake()
	{
		mInstance = this;
		_MyObj = gameObject;
		init ();
	}
	
	// Use this for initialization
	void Start ()
	{
		close();
	}
	
	public override void init()
	{
		base.init();
		label0.text = TextsData.getData(229).chinese;
		label1.text = TextsData.getData(229).chinese;
		setClickBtnCount(2);
		
	}
	
	public void onClickSpirit()
	{
		if(btnClickList[0])
		{
			return;
		}
		btnClickList[0] = true;
//		MainMenuManager.mInstance.openChallenge();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.OpenSpriteWorldPanel();
		}
	}

	public void onClickMuse()
	{
		if(btnClickList[1])
		{
			return;
		}
		btnClickList[1] = true;
		hideAllStep();
//		SpriteWroldUIManager.mInstance.doMuse();
		SpriteWroldUIManager spriteWorld = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD, "SpriteWroldUIManager")as SpriteWroldUIManager;
		spriteWorld.doMuse();
	}
}
