using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActiveWroldUIManager :  MonoBehaviour  , ProcessResponse {
	
	
	/*
	 * 活动副本主界面控制类
	 * 
	 */
//	public static ActiveWroldUIManager mInstance;
	public UIScrollBar ScrollBar;
	public GameObject ScrollView;
	public GameObject GridList;
	public GameObject Arrow_L;
	public GameObject Arrow_R;
	
	//1, 进入某一副本（即进入关卡选择界面） 2, //
	private int requestType;
	private bool receiveData;
	private int errorCode;
	private List<CopyItem> eventList;
	//服务器返回的数据//
	private List<string> strData;
	private int selCDTime;
	
	private GameObject loadPrefab;
	private string loadPath = "Prefabs/UI/ActiveWrold/ActiveCopyItem";
	private List<string> activeCopyDatas;
	private float scrollBarValue;
	
	private Transform _myTransform;

    private MapResultJson mapRJ;


    CardGroupResultJson cardGroupRJ;

    public int curSelEventId;


    int num = 0;

	
	void Awake(){
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		_myTransform = transform ;
		init();
//		hide();
	}
	
	public void init ()
	{
//		base.init ();
		activeCopyDatas = new List<string>();
		eventList = new List<CopyItem>();
		strData = new List<string>();
		
	}

	// Use this for initialization
	void Start () {
        if (PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_EVENT)
        {
            if (PlayerInfo.getInstance().isEvent)
            {
                PlayerInfo.getInstance().BattleOverBackType = 0;
                //			MainMenuManager.mInstance.hide();
                //隐藏主城//
                if (UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
                {
                    UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                }
                requestType = 16;
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_EVENT), this);
            }
            else
            {

            }
        }
        else
        {
            //			hide();

        }
	}
	
	// Update is called once per frame
	void Update () {
		//显示箭头//
		if( activeCopyDatas.Count<=3){
			Arrow_L.SetActive(false);
			Arrow_R.SetActive(false);
		}
		else
		{
			if(ScrollBar.value <=0){
				Arrow_L.SetActive(false);
				Arrow_R.SetActive(true);
			}
			else if(ScrollBar.value >=1){
				Arrow_L.SetActive(true);
				Arrow_R.SetActive(false);
			}
			else{
				Arrow_L.SetActive(true);
				Arrow_R.SetActive(true);
			}
		}
		
		if(receiveData){
			receiveData = false;
			if(errorCode == -3)
				return;
			switch(requestType){
			case 1:
                    if (type == 1)
                    {
                        type = 0;
                        string[] str = strData[0].Split('-');
                        curSelEventId = StringUtil.getInt(str[0]);
                        requestType = 15;
                        //PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0), this);
                        PlayerInfo.getInstance().sendRequest(new EventBattleJson(curSelEventId, PlayerInfo.getInstance().copyId), this);
                    }
                    else
                    {
                        //				ActiveWroldSelManager.mInstance.setData(strData, selCDTime);
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COPYSELECT);
                        ActiveWroldSelManager activeSel = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COPYSELECT,
                            "ActiveWroldSelManager") as ActiveWroldSelManager;
                        activeSel.setData(strData, selCDTime);
                        hide();
                    }
				break;
            case 10:
                UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
                MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
                    "MissionUI2") as MissionUI2;
                MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI") as MissionUI;
                mission.mrj = mapRJ;
                mission2.show();
                mission.show();
				mission2.koAwardBtn.SetActive(false);
                hide();
                break;
            case 5:
                UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
                CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager") as CombinationInterManager;

                combination.curCardGroup = cardGroupRJ.transformCardGroup();

                combination.SetData(5, new List<string>(), new List<string>(), curSelEventId);

                //				//关闭主菜单选项卡//
                //				MainMenuManager.mInstance.hide();
                //隐藏主城//
                if (UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
                {
                    UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                }
                hide();
                break;
           case 15:
                if(errorCode == 0){
                    requestType = 5;
                    PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0), this);
				}
				else if(errorCode == 24)		//进入次数到达上限，更高的vip等级可以增加进入次数//
				{
					string str = TextsData.getData(277).chinese;
					ToastWindow.mInstance.showText(str);
					//提示去充值//
					//UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else if(errorCode == 27)		//体力不足//
				{
//					string str = TextsData.getData(149).chinese;
//					ToastWindow.mInstance.showText(str);
					int buyType = 2;
					int jsonType = 1;
					int costType = 1;
					ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_ACTIVESEL);
				}
				else if(errorCode == 57)		//等级不足//
				{
					string str = TextsData.getData(148).chinese;
					ToastWindow.mInstance.showText(str);
				}
				else if(errorCode == 86)			//冷却时间未到，无法进入//
				{
					int buyType = 5;
					int jsonType = 1;
					int costType = 1;
					int cdType = 3;			//活动副本//
					ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, 
						BuyTipManager.UI_TYPE.UI_ACTIVESEL, cdType, PlayerInfo.getInstance().copyId);
				}
				break;
             case 16:
                setData(strData,num);
                break;
			}
		}
	}
	
	
	
	public void show ()
	{
//		base.show ();
	}
	//list格式：id-mark(0未开启，1开启)-time(剩余时间)//
	public void setData(List<string> strList,int num = 0){
		activeCopyDatas = strList;
        this.num = num;
		initCopyData();
		show();
	}
	
	//初始化副本选项数据//
	public void initCopyData(){
		CleanList();
		CleanScrollView();
		for(int i = 0;i < activeCopyDatas.Count;i++){
			string[] str = activeCopyDatas[i].Split('-');
            Debug.Log("========================================== :" + str[0]);
			int copyId = StringUtil.getInt( str[0]);
			int mark = StringUtil.getInt( str[1]);
			int time = StringUtil.getInt(str[2]);
			//EventData ed = EventData.getData(copyId);
			
			if(loadPrefab==null)
			{
				loadPrefab = Resources.Load(loadPath) as GameObject;
			}
			GameObject item = Instantiate(loadPrefab) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(item,GridList);
			
			//设置副本信息//
			CopyItem ci = item.GetComponent<CopyItem>();
			ci.copyId = copyId;
			ci.markId = mark;
			//像链表中添加数据//
			eventList.Add(ci);
			
			UISprite icon = item.transform.FindChild("Icon").GetComponent<UISprite>();
			UISprite race = item.transform.FindChild("Race").GetComponent<UISprite>();
			UILabel name = item.transform.FindChild("Name").GetComponent<UILabel>();
			UILabel timeLabel = item.transform.FindChild("Time").GetComponent<UILabel>();

            UILabel enterNums = item.transform.FindChild("Num").GetComponent<UILabel>();

            UILabel power = item.transform.FindChild("Power").GetComponent<UILabel>();

			GameObject lockIcon = item.transform.FindChild("Locked").gameObject;
			
			//修改icon和名字//
			EventData ed = EventData.getEventData(copyId);
			icon.atlas = LoadAtlasOrFont.LoadHeroAtlasByName(ed.atlas);
			icon.spriteName = ed.image;
			//种族//
			int raceId = ed.raceimage;
			if(raceId > 0){
				race.spriteName = "race_" + raceId;
			}
			else {
				race.gameObject.SetActive(false);
			}
			//名字//
			name.text = ed.name;
			
			//判断开启，如果已开启则将lockIcon隐藏//
			if(mark == 0){			//未开启//
				lockIcon.SetActive(true);
				timeLabel.gameObject.SetActive(false);
			}
			else if(mark == 1){		//开启//

                icon.gameObject.SetActive(true);
				lockIcon.SetActive(false);
				//显示剩余时间//
				string s1 = TextsData.getData(143).chinese;
				string s2 = "";
				int timeNum = 0;
				if(time >= 1440){		//天//
					s2 = TextsData.getData(144).chinese;
					timeNum = time/1440;
					if(time % 1440 > 0){
						timeNum += 1;
					}
				}
				else if(time >= 60 && time < 1440){				//小时//
					s2 = TextsData.getData(145).chinese;
					timeNum = time/60;
					if(time%60>0){
						timeNum += 1;
					}
				}
				else if(time > 0 && time < 60){					//分钟//
					s2 = TextsData.getData(152).chinese;
					timeNum = time;
				}
				
				string ss = s1 + timeNum + s2;
				timeLabel.text = ss;
			}

			UIButtonMessage ubm = item.GetComponent<UIButtonMessage>();
			ubm.target = _myTransform.gameObject;
			ubm.param = copyId;
            if (ed.nametype == 1)
            {
                ubm.functionName = "OnClickToCardBtn";

                timeLabel.transform.localPosition = new Vector3(timeLabel.transform.localPosition.x, -40, timeLabel.transform.localPosition.z);
               
                FBeventData fbed = FBeventData.getData(ed.fbids[0]);
                //进入次数//
                string str1 = TextsData.getData(147).chinese;
                enterNums.gameObject.SetActive(true);
                power.gameObject.SetActive(true);
                if (PlayerInfo.getInstance().player.vipLevel > 0)
                {
                    enterNums.text = str1 + num + "/" + VipData.getData(PlayerInfo.getInstance().player.vipLevel).activity;
                }
                else
                    enterNums.text = str1 + num + "/" + fbed.entry;

                power.text = TextsData.getData(13).chinese + fbed.cost;
            }
            else
                ubm.functionName = "OnClickBtn";
			//为scrollView复制//
			UIDragPanelContents udpc = item.GetComponent<UIDragPanelContents>();
			udpc.draggablePanel = ScrollView.GetComponent<UIDraggablePanel>();
//			udpc.draggablePanel.disableDragIfFits
		}

	}
	
	
	public void hide ()
	{
//		base.hide ();
		CleanList();
		CleanScrollView();
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY);
	}
	
	private void gc()
	{
		strData.Clear();
		if(activeCopyDatas!=null)
		{
			activeCopyDatas.Clear();
		}
		activeCopyDatas=null;
		loadPrefab=null;
		Resources.UnloadUnusedAssets();
	}
	
	public void CleanScrollView(){
		GridList.GetComponent<UIGrid>().repositionNow = true;
		ScrollBar.value = 0;
		ScrollView.transform.localPosition = Vector3.zero;
		ScrollView.GetComponent<UIPanel>().clipRange = new Vector4(0,0,760,330);
	}
	
	public void CleanList(){
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
		eventList.Clear();
	}



	public void receiveResponse (string json)
	{
		if(json != null){
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			Debug.Log("ActiveWroldUIManager : " + json);
			switch(requestType){
			case 1:				//进入某一副本选关界面//
				strData.Clear();
				EventResultJson erj = JsonMapper.ToObject<EventResultJson>(json);
				errorCode = erj.errorCode;
				if(errorCode == 0)
				{
					strData = erj.s;
					selCDTime = erj.cdtime;
                    num = erj.num;
				}
				receiveData = true;
				break;
            case 10:
                MapResultJson mj = JsonMapper.ToObject<MapResultJson>(json);
                errorCode = mj.errorCode;
                if (errorCode == 0)
                    mapRJ = mj;
                receiveData = true;
                break;
            case 5:
                CardGroupResultJson cgrj = JsonMapper.ToObject<CardGroupResultJson>(json);
                errorCode = cgrj.errorCode;
                if (errorCode == 0)
                {
                    cardGroupRJ = cgrj;
                }
                receiveData = true;
                break;
                    
            case 15:
                EventBattleResultJson ebrj = JsonMapper.ToObject<EventBattleResultJson>(json);
                errorCode = ebrj.errorCode;
                if (errorCode == 0)
                {
                    //设置战斗数据//
                    //				PlayerInfo.getInstance().bNum=ebrj.bNum;
                    PlayerInfo.getInstance().ebrj = ebrj;
                    errorCode = ebrj.errorCode;
                }
                receiveData = true;
                break;

            case 16:		//活动副本（异世界）//
                //				strData.Clear();
                EventResultJson er = JsonMapper.ToObject<EventResultJson>(json);
                errorCode = er.errorCode;

               
                if (errorCode == 0)
                {
                    strData = er.s;
                    num = er.num;
                }
                receiveData = true;
                break;
			}
		}
	}

    int type = 0;
    //死亡洞窟去卡组界面//
    public void OnClickToCardBtn(int id)
    {
        type = 1;
        PlayerInfo.getInstance().copyId = id;
        requestType = 1;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SELECT_EVENT, id), this);


    }


	//按键响应 id 为副本在表中的id//
	public void OnClickBtn(int id){
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		CopyItem ci = null;
		for(int i =0;i < eventList.Count;i++){
			CopyItem item = eventList[i];
			if(item.copyId == id){
				ci = item;
			}
		}
		if(ci.markId == 1){
			//进入关卡选择界面//
			PlayerInfo.getInstance().copyId = id;
			requestType = 1;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SELECT_EVENT, id),this);
		}
	}
	
	public void OnBackBtn(){
        requestType = 10;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP), this);
	}
}
