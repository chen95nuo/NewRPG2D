using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCardSetPanel : BWUIPanel,ProcessResponse
{
	
	public static MainCardSetPanel mInstance;
	public enum E_ParamType : int
	{
		E_Back = 0,
		E_Combination = 1,
		E_Bag = 2,
		E_Intensify = 3,
		E_Compose = 4,
		E_Talent = 5,
		E_Icon = 6,
	}
	
	public GameObject BasicLayer;		//要移动的基础界面//
	public List<GameObject> BtnPosList;	//基础界面各个按钮移动的位置//
	public GameObject[] unLockObj;			//模块的解锁描述//
	public GameObject[] models;			//各个模块//
	private bool receiveData;
	private int errorCode;
	//1背包,2军团,3吞噬
	private int requestType;
	
	float ScaleTime = 0.3f;				//基础界面放大的时间//
	//float MoveTime = 0.5f;				//基础界面移动的时间//
	float StayTime = 0.5f;				//动画播放完等待跳转的时间//
	float ScaleNum = 1.5f;				//基础界面放大的倍数//
	float frameCount = 0;
	bool StartFrameCount = false;		//是否开始等待跳转的计时器//
	int curSelectBtn = -1;
	void Awake()
	{
		mInstance = this;
		_MyObj = mInstance.gameObject;
		init ();
		close();
//		show();
	}
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			switch(requestType)
			{
			case 1:				//背包//
//				PackUI.mInstance.show();
				//关闭主菜单选项卡//
//				MainMenuManager.mInstance.hide();
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				hide();
				break;
			case 2:				//军团//
				Debug.Log("requestType ========== " + requestType);
//				CombinationInterManager.mInstance.show();
//				CombinationInterManager.mInstance.SetData(0, 1);
				//关闭主菜单选项卡//
//				MainMenuManager.mInstance.hide();
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				hide();
				break;
			case 3:				//吞噬//
//				IntensifyPanel.mInstance.show();
				//关闭主菜单选项卡//
//				MainMenuManager.mInstance.hide();
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				hide();
				break;
			case 4:				//合成//
//				if(ComposePanel.mInstance != null)
//				{
//					ComposePanel.mInstance.setComposePage(ComposePanel.PageType.E_ComposePackPage);
//					ComposePanel.mInstance.setComposeType(ComposePanel.ComposeType.E_Equip);
//					ComposePanel.mInstance.show();
//				}
				//关闭主菜单选项卡//
				hide();
//				MainMenuManager.mInstance.hide();
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				break;
			case 5://符文//
//				RuneUI.mInstance.show();
				hide();
				HeadUI.mInstance.hide();
//				MainMenuManager.mInstance.hide();
				//隐藏主城//
				if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
				{
					UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				}
				break;
			}
		}
		
		if(StartFrameCount){
			frameCount+= Time.deltaTime;
			if(frameCount >= StayTime){
				StartFrameCount = false;
				frameCount=0;
				SendMessageToServer(curSelectBtn + 1);
			}
		}
	}
	//判断是否开启模块//
	public void ShowCardSetUnLockLabel(){
		for(int i =0;i< unLockObj.Length;i++){
			int mark = PlayerInfo.getInstance().getUnLockData(i + 1);
			if(mark == 0){		//未解锁//
				string des = UnlockData.getData(i + 1).description;
				unLockObj[i].gameObject.SetActive(true);
				UILabel label = unLockObj[i].transform.FindChild("Label").GetComponent<UILabel>();
				label.text = des;
			}
			else if(mark == 1){	//已解锁//
				unLockObj[i].gameObject.SetActive(false);
			}
		}
	}
	
	//id在数组中的索引//
	public void ManagerBtnShow(int id){
		for(int i = 0;i < models.Length;i++){
			GameObject light = models[i].transform.FindChild("SelectSpr").gameObject;
			GameObject black = models[i].transform.FindChild("Background").gameObject;
			if(i == id)
			{
				light.SetActive(true);
				black.SetActive(false);
				if(i ==0)
				Debug.Log(light.name + light.GetComponent<UISprite>().color.a);
			}
			else 
			{
				light.SetActive(false);
				black.SetActive(true);
				if(i ==0)
				Debug.Log(light.name + light.GetComponent<UISprite>().color.a);
			}
		}
	}
	
	public override void init ()
	{
		base.init ();
		CleanData();
	}
	
	public override void show()
	{
		base.show();	
		CleanData();
		ShowCardSetUnLockLabel();
		ManagerBtnShow(-1);
	}
	
	public override void hide()
	{
		CleanData();
		base.hide();
	}
	//军团界面：1.军团    2 背包   3.吞噬   4.合成   5.符文   6.图鉴, 在表中的id为1-6//
	public void onClickBtn(int param)
	{
		ManagerBtnShow(param - 1);
		//移动及缩放背景图//
		if(curSelectBtn < 0){
			int unlockMark = PlayerInfo.getInstance().getUnLockData(param);
			//1 军团， 2 背包， 3 吞噬 , 4 compose //
//			if(param > 0 && param <= 5){
			if(unlockMark == 1){
				
				curSelectBtn = param- 1;
				BasicLayerPlayAnim(curSelectBtn);
			}
			else if(unlockMark == 0){
//				UIToastTipManager.mInstance.show();
				string str = TextsData.getData(142).chinese;
				string s2 = UnlockData.getData(param).description;
				string ss = str + "\r\n" + s2;
				ToastWindow.mInstance.showText(ss);
			}
		}
		
	}
	
	//返回按钮//
	public void OnBackBtn(){
//		MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
		if(main!= null)
		{
			main.SetData(STATE.ENTER_MAINMENU_BACK);
		}
		hide();
	}
	
	public void BasicLayerPlayAnim(int posId){
		Vector3 desPos = BtnPosList[posId].transform.position;
		Vector3 scale = new Vector3(ScaleNum, ScaleNum, 1);
		iTween.ScaleTo(BasicLayer,iTween.Hash("scale",scale ,"time",ScaleTime, "easetype",iTween.EaseType.linear));
		iTween.MoveTo(BasicLayer,iTween.Hash("position", desPos,"time", ScaleTime, "oncomplete", "BasicLayerMoveCallBack", "oncompletetarget", _MyObj, "easetype",iTween.EaseType.linear));
	}
	
	public void BasicLayerMoveCallBack(){
		StartFrameCount = true;
//		SendMessageToServer(curSelectBtn + 1);
	}
	
	
	public void SendMessageToServer(int param){
		switch(param)
		{
		case (int)E_ParamType.E_Back:/**后退**/
			UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
			MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
			if(main!= null)
			{
				main.SetData(STATE.ENTER_MAINMENU_BACK);
			}
			
			break;
		case (int)E_ParamType.E_Combination:
		{
			requestType=2;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0),this);
		}break;
		case (int)E_ParamType.E_Bag:
		{
			requestType=1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_PACK,1),this);
		}break;
		case (int)E_ParamType.E_Intensify:
		{
			requestType=3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Intensify,1),this);
		}break;
		case (int)E_ParamType.E_Compose:
		{
			requestType = 4;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_Compose,(int)ComposePanel.ComposeType.E_Equip),this);
			
		}break;
		case (int)E_ParamType.E_Talent://符文//
		{
			requestType = 5;
			PlayerInfo.getInstance().sendRequest(new RuneJson(0),this);
		}break;
		case (int)E_ParamType.E_Icon:
		{
			//return;
			// TODO
		}break;
		}
//		hide();
	}
	
		
	public void receiveResponse(string json)
	{
		Debug.Log("MainCardSetPanel : " + json);
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
			{
				PackResultJson prj=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = prj.errorCode;
				if(errorCode == 0)
				{
//					PackUI.mInstance.pejs=prj.pejs;
				}
				receiveData=true;	
			}break;
			case 2:
			{
				CardGroupResultJson cgrj=JsonMapper.ToObject<CardGroupResultJson>(json);
				errorCode = cgrj.errorCode;
				if(errorCode == 0)
				{
//					CombinationInterManager.mInstance.curCardGroup=cgrj.transformCardGroup();
//					CombinationInterManager.mInstance.curPage=0;
//					CombinationInterManager.mInstance.isUsed=true;
				}
				receiveData=true;
			}break;
			case 3:
			{
				PackResultJson prj=JsonMapper.ToObject<PackResultJson>(json);
				errorCode = prj.errorCode;
				if(errorCode == 0)
				{
//					IntensifyPanel.mInstance.allCells=prj.list;
				}
				receiveData=true;
			}break;
			case 4:
			{
				ComposeResultJson crj = JsonMapper.ToObject<ComposeResultJson>(json);
				errorCode = crj.errorCode;
				if(errorCode == 0)
				{
					
//					ComposePanel.mInstance.packItemInfoList = crj.cs;
				}
				receiveData = true;
			}break;
			case 5:
				RuneResultJson rrj=JsonMapper.ToObject<RuneResultJson>(json);
				errorCode = rrj.errorCode;
				if(errorCode == 0)
				{
					
//					RuneUI.mInstance.rrj=rrj;
				}
				receiveData = true;
				break;
			}
		}
	}
	
	public void CleanData(){
		frameCount = 0;
		StartFrameCount = false;
		BasicLayer.transform.localScale	= Vector3.one;
		BasicLayer.transform.localPosition = Vector3.zero;
		curSelectBtn= -1;
		
	}
	
}
