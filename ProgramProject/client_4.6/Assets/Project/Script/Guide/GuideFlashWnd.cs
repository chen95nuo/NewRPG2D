using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideFlashWnd : BWUIPanel {
	
	public static GuideFlashWnd mInstance = null;
	
	int _guideID = -1;
	int _needRunStep = -1;
	int _needRunDialogID = -1;
	
	public enum FlashWndType : int
	{
		E_UseCombo3 = 0,
		E_ShowKOTips = 1,
	}
	
	public List<GameObject> flashWndList ;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init();
	}
	
	// Use this for initialization
	void Start ()
	{
		close();
	}
	
	public void clear()
	{
		_guideID = -1;
		_needRunStep = -1;
		_needRunDialogID = -1;
		hideAllFlashWnd();
	}
	
	public override void init()
	{
		base.init();
		clear();
	}
	
	public override void hide()
	{
		base.hide();
		clear();
	}
	
	public void hideAllFlashWnd()
	{
		for(int i = 0; i < flashWndList.Count;++i)
		{
			flashWndList[i].SetActive(false);
		}
	}
	
	public void showFlashWnd(int guideID,int needRunStep,int needRunDialogID,FlashWndType fwt)
	{
		show();
		clear();
		_guideID = guideID;
		_needRunStep = needRunStep;
		_needRunDialogID = needRunDialogID;
		flashWndList[(int)fwt].SetActive(true);
		if(fwt == FlashWndType.E_ShowKOTips)
		{
			Invoke("showGuideNextStep",2f);
		}
		else if(fwt == FlashWndType.E_UseCombo3)
		{
			Invoke("showGuideNextStep",1.2f);
		}
		
	}
	
	void showGuideNextStep()
	{
		switch(_guideID)
		{
		case (int)GuideManager.GuideType.E_UseCombo3:
		{
			if(_needRunStep != -1)
			{
				GuideUI_UseCombo3.mInstance.showStep(_needRunStep);
			}
			else	if(_needRunDialogID != -1)
			{
				UISceneDialogPanel.mInstance.showDialogID(_needRunDialogID);
			}
		}break;
		case (int)GuideManager.GuideType.E_Battle2_Bounes:
		{
			if(_needRunStep != -1)
			{
				GuideUI_Bounes.mInstance.showStep(_needRunStep);
			}
			else	if(_needRunDialogID != -1)
			{
				UISceneDialogPanel.mInstance.showDialogID(_needRunDialogID);
			}
		}break;
		}
		hide();
	}
	
	
}
