using UnityEngine;
using System.Collections;

public class BlackBgUI : BWUIPanel {

	public static BlackBgUI mInstance;
	public UIButtonMessage blackBg;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
		hide();
	}
	
	
	//type 黑框按键反应的类型//
	public void SetData(int type)
	{
		show();
		if(blackBg != null)
		{
			blackBg.param = type;
		}
	}
	
	//type 1 只关闭本界面，2 关闭本界面，也关闭scrollView界面//
	public void OnClickBtn(int type)
	{
		switch(type)
		{
		case 1:
			hide();
			break;
			
		case 2:
//			if(ScrollViewPanel.mInstance != null)
//			{
//				ScrollViewPanel.mInstance.close();
//			}
			ScrollViewPanel scrollView = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SCROLLVIEW, 
				"ScrollViewPanel" ) as ScrollViewPanel;
			if(scrollView!=null)
			{
				scrollView.close();
			}
			
			hide();
			break;
		}
	}
}
