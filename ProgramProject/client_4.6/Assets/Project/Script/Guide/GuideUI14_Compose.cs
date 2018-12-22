using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideUI14_Compose : GuideBase
{
	public static GuideUI14_Compose mInstance = null;
	
	public UILabel label0;
	public UILabel label00;
	
	public UILabel label1;
	public UILabel label2;
	public UILabel label3;
	
	int itemID = 10101;
	int itemID0 = 10301;
	int composeTargetID = 1101;
	
	public SimpleCardInfo2 cardInfo;
	public SimpleCardInfo2 cardInfo0;
	
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
		ItemsData itemData = ItemsData.getData(itemID);
		if(itemData != null)
		{
			label0.text = TextsData.getData(296).chinese + itemData.name + " x 3";
			cardInfo.clear();
			cardInfo.setSimpleCardInfo(itemID,GameHelper.E_CardType.E_Item);
		}
		
		ItemsData itemData0 = ItemsData.getData(itemID0);
		if(itemData0 != null)
		{
			label00.text = TextsData.getData(296).chinese + itemData0.name + " x 1";
			cardInfo0.clear();
			cardInfo0.setSimpleCardInfo(itemID0,GameHelper.E_CardType.E_Item);
		}
		
		label1.text = TextsData.getData(224).chinese;
		label2.text = TextsData.getData(224).chinese;
		label3.text = TextsData.getData(224).chinese;
		setClickBtnCount(4);
	}
	
	public void onClickClosePopWindow()
	{
		if(btnClickList[0])
		{
			return;
		}
		btnClickList[0] = true;
		hideAllStep();
		UISceneDialogPanel.mInstance.showDialogID(30);
		cardInfo.clear();
		cardInfo0.clear();
		Resources.UnloadUnusedAssets();
	}
	
	public void onClickOpenComposeBtn()
	{
		if(btnClickList[1])
		{
			return;
		}
		btnClickList[1] = true;
//		MainMenuManager.mInstance.openCompose();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			main.openCompose();
		}
		
	}
	
	public void onClickComposeTarget()
	{
		if(btnClickList[2])
		{
			return;
		}
		btnClickList[2] = true;
//		ComposePanel.mInstance.onSelectPackCardItem(composeTargetID);
		if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE))
		{
			ComposePanel compose = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE, "ComposePanel" )as ComposePanel;
			compose.onSelectPackCardItem(composeTargetID);
		}
		showStep(3);
	}
	
	public void onClickComposeBtn()
	{
		if(btnClickList[3])
		{
			return;
		}
		btnClickList[3] = true;
//		ComposePanel.mInstance.doCompose();
		if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE))
		{
			ComposePanel compose = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COMPOSE, "ComposePanel" )as ComposePanel;
			compose.doCompose();
		}
		hideAllStep();
	}
}
