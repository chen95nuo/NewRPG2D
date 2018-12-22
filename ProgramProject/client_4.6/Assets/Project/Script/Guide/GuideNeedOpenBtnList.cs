using UnityEngine;
using System.Collections;

public class GuideNeedOpenBtnList : BWUIPanel
{
	public static GuideNeedOpenBtnList mInstance = null;
	
	public UILabel label0;
	bool isClick = false;
	
	int _guideID = -1;
	int _needRunStep = -1;
	
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
	
	public void clear()
	{
		isClick = false;
		_guideID = -1;
		_needRunStep = -1;	
	}
	
	public override void init()
	{
		base.init();
		clear();
		label0.text = TextsData.getData(589).chinese;
	}
	
	public void showPanel(int guideID,int needRunStep)
	{
		base.show();
		clear();
		_guideID = guideID;
		_needRunStep = needRunStep;
	}
	
	public override void hide()
	{
		base.hide();
		clear();
	}
	
	public void onClickOpenBtnList()
	{
		if(isClick)
		{
			return;
		}
		isClick = true;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openIcons();
		}
		switch(_guideID)
		{
		case (int)GuideManager.GuideType.E_IntensifyEquip:
		{
			GuideUI_IntensifyEquip.mInstance.showStep(_needRunStep);
		}break;
		case (int)GuideManager.GuideType.E_Compose:
		{
			GuideUI14_Compose.mInstance.showStep(_needRunStep);
		}break;
		case (int)GuideManager.GuideType.E_Break:
		{
			GuideUI22_Break.mInstance.showStep(_needRunStep);
		}break;
		case (int)GuideManager.GuideType.E_Rune:
		{
			GuideUI20_Rune.mInstance.showStep(_needRunStep);
		}break;
		case (int)GuideManager.GuideType.E_Spirit:
		{
			GuideUI23_Spirit.mInstance.showStep(_needRunStep);
		}break;
		}
		
		hide ();
	}

}
