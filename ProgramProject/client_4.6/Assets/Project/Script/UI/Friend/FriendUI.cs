using UnityEngine;
using System.Collections.Generic;

public class FriendUI : MonoBehaviour ,BWWarnUI,ProcessResponse{
	
//	public static FriendUI mInstance;
	
	public UILabel friendNum;
	public UILabel title;
	public GameObject itemsGo;
	public GameObject itemsScrollBar;
	public GameObject itemParent;
	public GameObject unitDetail;
	
	public GameObject friend_list;
	public GameObject friend_delete;
	public GameObject friend_request;
	public GameObject friend_add;
	public UIInput search;
	public UILabel itemLabel;
	public UILabel refreshTime;
	public UISprite refresh;
	public GameObject refreshLabelObj;
	
	private GameObject initItem;
	private List<GameObject> items;
	//==当前状态:0好友列表,1删除,2新的申请,3添加,4我的申请==//
	private int curState;
	//==要删除的item索引==//
	private int index;
	//==要申请为好友的玩家是否是搜索出的==//
	private bool isSearch;
	//==上次刷新时间==//
	private float lastRefreshTime;
	//==需要刷新好友列表==//
	private bool needRefreshFriendList=true; 
	
	bool needResetRefreshBtnInfo = false;
	
	//==好友列表==//
	public FriendResultJson frj;
	
	private int errorCode;
	
	//==1删除好友,2添加好友==//
	private int requestType;
	private bool receiveData;
	private Transform _myTransform;

    public UILabel rule;
	
	private int buyFriendUseCrystal;

    public int mark;
	void Awake()
	{
//		_MyObj=gameObject;
//		mInstance=this;
		_myTransform = transform;
	}
	
	// Use this for initialization
	void Start () {
//		init();
//		close();
        
	}
	
	// Update is called once per frame
	void Update () {
		if(receiveData)
		{
			receiveData=false;
			if(errorCode == -3)
				return;
			
			switch(requestType)
			{
			case 0://==查看好友列表==//
				if(frj.errorCode==47)
				{
					//每天最多能接受15个好友的赠送！//
					ToastWindow.mInstance.showText(TextsData.getData(117).chinese);
				}
				else if(frj.errorCode==48)
				{
					//体力已满,不能接受赠送！//
					ToastWindow.mInstance.showText(TextsData.getData(118).chinese);
				}
				else if(frj.errorCode==49)
				{
					//体力不足,不能赠送！//
					ToastWindow.mInstance.showText(TextsData.getData(119).chinese);
				}
				else
				{
					curState=0;
					PlayerInfo.getInstance().player.power=frj.power;
					HeadUI.mInstance.refreshPlayerInfo();
					if(needRefreshFriendList)
					{
						drawUI();
					}
					else
					{
						needRefreshFriendList=true;
						updateItems();
					}
				}
				break;
			case 1://==删除好友==//
				curState=1;
				drawUI();
				break;
			case 2://==刷新玩家==//
				if(frj.errorCode==0)    
				{
					isSearch=false;
					curState=3;
					drawUI();
				}
				break;
			case 3://==申请好友==//
				if(frj.errorCode==36)
				{
					//最多只能同时申请10个好友！//
					ToastWindow.mInstance.showText(TextsData.getData(103).chinese);
					return;
				}
				curState=3;
				updateItems();
				break;
			case 4://==我的申请==//
			case 5://==取消申请==//
				curState=4;
				drawUI();
				break;
			case 6://==申请我==//
				curState=2;
				drawUI();
				break;
			case 7://==处理申请==//
				if(frj.errorCode==39)
				{
					//好友已达上限，无法继续添加好友！//
					ToastWindow.mInstance.showText(TextsData.getData(107).chinese);
				}
				else if(frj.errorCode==40)
				{
					//对方好友已达上限，无法继续添加好友！//
					ToastWindow.mInstance.showText(TextsData.getData(108).chinese);
					curState=2;
					drawUI();
				}
				else
				{
					curState=2;
					drawUI();
				}
				break;
			case 8://==搜索==//
				if(frj.errorCode==43)
				{
					//不能搜索自己！//
					ToastWindow.mInstance.showText(TextsData.getData(116).chinese);
				}
				else
				{
					isSearch=true;
					curState=3;
					drawUI();
				}
				break;
			case 9://==购买好友上限==//
				if(errorCode==97)
				{
					//好友数量已达上限！//
					ToastWindow.mInstance.showText(TextsData.getData(390).chinese);
					return;
				}
				else if(errorCode==71)
				{
					//钻石不足//
					string str = TextsData.getData(49).chinese+TextsData.getData(51).chinese;
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
					return;
				}
				else if(errorCode==89)
				{
					//金币不足//
					ToastWindow.mInstance.showText(TextsData.getData(46).chinese);
					return;
				}
				else
				{
					drawFriendNum();
					HeadUI.mInstance.refreshPlayerInfo();
					FriendCostData fd=FriendCostData.getData(frj.buyNum);
					if(!TalkingDataManager.isTDPC)
					{
						//记录购买好友上线//
						TDGAItem.OnPurchase("BuyFriendGrid-"+fd.number1,1,buyFriendUseCrystal);
					}
				}
				break;
			}
		}
		
		if(lastRefreshTime>0)
		{
			lastRefreshTime-=Time.deltaTime;
			refreshTime.text="[A0A0A0]"+((int)(lastRefreshTime)+1)+"";
			refreshLabelObj.SetActive(false);
		}
		else 
		{
			if(needResetRefreshBtnInfo)
			{
				needResetRefreshBtnInfo = false;
				lastRefreshTime=0;
				if(refreshTime != null)
				{
					refreshTime.text="";
				}
				if(refresh != null)
				{
					refreshLabelObj.SetActive(true);
				}
			}
			
		}
		
		string text=search.value;
		if(text!=null && text.Length>10)
		{
			search.value=text.Substring(0,10);
		}
	}
	
	public void show()
	{
//		base.show();
		search.value="";
		isSearch=false;
		drawUI();
        if (mark == 1)
        {
            friend_list.transform.FindChild("request/Mark").gameObject.SetActive(true);
        }
        else
            friend_list.transform.FindChild("request/Mark").gameObject.SetActive(false);
		//好友说明//
        rule.text = TextsData.getData(570).chinese;
		
		needResetRefreshBtnInfo = true;

	}
	
	public void hide()
	{
//		base.hide();
		_myTransform.gameObject.SetActive(false);
		gc();
	}
	
	public void CloseUI()
	{
		hide();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_FRIEND);
	}
	
	private void gc()
	{
		initItem=null;
		if(items!=null)
		{
			items.Clear();
		}
		items=null;
		frj=null;
		Resources.UnloadUnusedAssets();
	}

    public void onClickGetBtn()
    {
		//当前好友//
        ToastWindow.mInstance.showText(TextsData.getData(561).chinese);
    }

	private void drawFriendNum()
	{
		int num=0;
		if(frj.list!=null)
		{
			num=frj.list.Count;
		}
		FriendCostData fd=FriendCostData.getData(frj.buyNum);
		//当前好友//
		friendNum.text=TextsData.getData(92).chinese+""+num+"/"+fd.number1;
	}
	
	private void drawItems()
	{
		if(frj.list==null || frj.list.Count==0)
		{
			itemLabel.gameObject.SetActive(true);
			itemsGo.SetActive(false);
			switch(curState)
			{
			case 0://==好友列表==//
				//没有好友！//
				itemLabel.text=TextsData.getData(111).chinese;
				break;
			case 1://==删除==//
				itemLabel.text=TextsData.getData(111).chinese;
				break;
			case 2://==新的申请==//
				//没有新的好友申请！//
				itemLabel.text=TextsData.getData(115).chinese;
                friend_list.transform.FindChild("request/Mark").gameObject.SetActive(false);
				break;
			case 3://==添加==//
				if(isSearch)
				{
					//没有这个玩家！//
					itemLabel.text=TextsData.getData(112).chinese;
				}
				else
				{
					//当前没有其他在线玩家！//
					itemLabel.text=TextsData.getData(113).chinese;
				}
				break;
			case 4://==我的申请==//
				//没有申请！//
				itemLabel.text=TextsData.getData(114).chinese;
				break;
			}
			return;
		}
		
		itemLabel.gameObject.SetActive(false);
		itemsGo.SetActive(true);
		itemsScrollBar.GetComponent<UIScrollBar>().value=0;
		if(items==null)
		{
			items=new List<GameObject>();
		}
		items.Clear();
		GameObjectUtil.destroyGameObjectAllChildrens(itemParent);
		if(frj.list!=null && frj.list.Count>0)
		{
			for(int k=0;k<frj.list.Count;k++)
			{
				if(initItem==null)
				{
					initItem=GameObjectUtil.LoadResourcesPrefabs("UI-friend/item",3);
				}
				GameObject item=Instantiate(initItem) as GameObject;
				item.transform.FindChild("unitskill-icon").GetComponent<FriendItem>().setData(frj.list[k]);
				GameObjectUtil.gameObjectAttachToParent(item,itemParent);
				UIButtonMessage msg=item.GetComponent<UIButtonMessage>();
//				msg.target=_MyObj;
				msg.target=_myTransform.gameObject;
				msg.functionName="onClickItem";
				msg.param=k;
				items.Add(item);
				//战力://
				item.transform.FindChild("battle-power").GetComponent<UILabel>().text=TextsData.getData(203).chinese+frj.list[k].bp;

                GameObject btnNew = item.transform.FindChild("btnNew").gameObject;
                GameObject btnNew_h = item.transform.FindChild("btnNew_h").gameObject;

				GameObject btn=item.transform.FindChild("btn").gameObject;
				GameObject btn2=item.transform.FindChild("btn2").gameObject;


				UILabel label=btn.transform.FindChild("label").GetComponent<UILabel>();

                UISprite sp = btnNew.GetComponent<UISprite>();
				UIButtonMessage btnMsg=btn.GetComponent<UIButtonMessage>();

                UIButtonMessage btnMsg1 = btnNew.GetComponent<UIButtonMessage>();

				switch(curState)
				{
				case 0:
					label.text=null;
                    btn.SetActive(false);
					btn2.SetActive(false);
					
					FriendElement fe=frj.list[k];
					switch(fe.t)
					{
					case 0://==未赠不可收==//
					case 3://==未赠已收==//
						//label.text=TextsData.getData(311).chinese;
//						btnMsg.target=_MyObj;
						sp.spriteName = "energy-give";
                        btnNew.SetActive(true);
                        btnNew_h.SetActive(false);
						btnMsg1.target=_myTransform.gameObject;
						btnMsg1.functionName="onClickSendPower";
						btnMsg1.param=k;
						break;
					case 2://==未赠可收==//
					case 5://==已赠可收==//
                       // btnNew1.SetActive(true);
						//label.text=TextsData.getData(312).chinese;
//						btnMsg.target=_MyObj;
                        sp.spriteName = "energy-get";
                        btnNew.SetActive(true);
                        btnMsg1.target = _myTransform.gameObject;
                        btnMsg1.functionName = "onClickReceivePower";
                        btnMsg1.param = k;
						break;
					case 4://==已赠不可收==//
					case 6://==已赠已收==//
                        sp.spriteName = "energy-give";
                        btnNew.SetActive(false);
                        btnNew_h.SetActive(true);
						//label.text=TextsData.getData(313).chinese;
						btnMsg1.target=null;
						break;
					}
					break;
				case 1:
					//删除//
					label.text=TextsData.getData(98).chinese;
//					btnMsg.target=_MyObj;
					btnMsg.target=_myTransform.gameObject;
					btnMsg.functionName="onClickDelete";
					btnMsg.param=k;
					btn2.SetActive(false);
					break;
				case 2:
					//同意//
					label.text=TextsData.getData(105).chinese;
//					btnMsg.target=_MyObj;
					btnMsg.target=_myTransform.gameObject;
					btnMsg.functionName="onClickAgree";
					btnMsg.param=k;
					btn2.SetActive(true);
					btn2.transform.FindChild("label").GetComponent<UILabel>().text=TextsData.getData(106).chinese;
					UIButtonMessage btn2Msg=btn2.GetComponent<UIButtonMessage>();
//					btn2Msg.target=_MyObj;
					btn2Msg.target=_myTransform.gameObject;
					btn2Msg.functionName="onClickRefuse";
					btn2Msg.param=k;
					break;
				case 3:
					switch(frj.list[k].t)
					{
					case 0://==已经是好友==//
						//已添加//
						label.text=TextsData.getData(102).chinese;
						btnMsg.target=null;
						break;
					case 1://==未申请==//
						//申请//
						label.text=TextsData.getData(100).chinese;
//						btnMsg.target=_MyObj;
						btnMsg.target=_myTransform.gameObject;
						btnMsg.functionName="onClickApply";
						btnMsg.param=k;
						break;
					case 2://==已申请==//
						//待确认//
						label.text=TextsData.getData(101).chinese;
						btnMsg.target=null;
						break;
					}
					btn2.SetActive(false);
					break;
				case 4:
					//取消//
					label.text=TextsData.getData(97).chinese;
//					btnMsg.target=_MyObj;
					btnMsg.target=_myTransform.gameObject;
					btnMsg.functionName="onClickCancel";
					btnMsg.param=k;
					btn2.SetActive(false);
					break;
				}
			}
		}
		itemParent.GetComponent<UIGrid>().repositionNow=true;
	}
	
	//==未申请变为已申请,赠送体力收取体力==//
	private void updateItems()
	{
		if(items==null)
		{
			return;
		}
		if(curState==3)
		{
			for(int k=0;k<items.Count;k++)
			{
				if(k==index)
				{
					GameObject item=items[k];
					GameObject btn=item.transform.FindChild("btn").gameObject;
					GameObject btn2=item.transform.FindChild("btn2").gameObject;
					UILabel label=btn.transform.FindChild("label").GetComponent<UILabel>();
					UIButtonMessage btnMsg=btn.GetComponent<UIButtonMessage>();
					//待确认//
					label.text=TextsData.getData(101).chinese;
					btnMsg.target=null;
					btn2.SetActive(false);
					break;
				}
			}
		}
		else if(curState==0)
		{
			for(int k=0;k<items.Count;k++)
			{
				if(k==index)
				{
					GameObject item=items[k];




                    GameObject btnNew = item.transform.FindChild("btnNew").gameObject;
                    GameObject btnNew_h = item.transform.FindChild("btnNew_h").gameObject;




					GameObject btn=item.transform.FindChild("btn").gameObject;
					GameObject btn2=item.transform.FindChild("btn2").gameObject;
					//UILabel btnText=btn.transform.FindChild("label").GetComponent<UILabel>();
					UILabel label=btn.transform.FindChild("label").GetComponent<UILabel>();
                    UISprite sp = btnNew.GetComponent<UISprite>();
					UIButtonMessage btnMsg=btnNew.GetComponent<UIButtonMessage>();
					label.text=null;



                    btn.SetActive(false);
					btn2.SetActive(false);
					
					FriendElement fe=frj.list[k];
					switch(fe.t)
					{
					case 0://==未赠不可收==//
					case 3://==未赠已收==//
						//label.text=TextsData.getData(311).chinese;
//						btnMsg.target=_MyObj;
						sp.spriteName = "energy-give";
                        btnNew.SetActive(true);
                        btnNew_h.SetActive(false);
						btnMsg.target=_myTransform.gameObject;
						btnMsg.functionName="onClickSendPower";
						btnMsg.param=k;
						break;
					case 2://==未赠可收==//
					case 5://==已赠可收==//
                       // btnNew1.SetActive(true);
						//label.text=TextsData.getData(312).chinese;
//						btnMsg.target=_MyObj;
                        sp.spriteName = "energy-get";
                        btnNew.SetActive(true);
                        btnMsg.target = _myTransform.gameObject;
                        btnMsg.functionName = "onClickReceivePower";
                        btnMsg.param = k;
						break;
					case 4://==已赠不可收==//
					case 6://==已赠已收==//
                        sp.spriteName = "energy-give";
                        btnNew.SetActive(false);
                        btnNew_h.SetActive(true);
						//label.text=TextsData.getData(313).chinese;
						btnMsg.target=null;
						break;
					}
					break;
				}
			}
		}
	}
	
	
	public void onClickItem(int param)
	{
		if(items==null)
		{
			return;
		}
//		for(int k=0;k<items.Count;k++)
//		{
//			GameObject item=items[k];
//			if(k==param)
//			{
//				item.transform.FindChild("unitskill-icon").GetComponent<FriendItem>().highLight();
//			}
//			else
//			{
//				item.transform.FindChild("unitskill-icon").GetComponent<FriendItem>().lowLight();
//			}
//		}
	}
	
	public void onClickBtn(int param)
	{
		switch(param)
		{
		case 0://==后退==//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
			if(curState==4)
			{
				onClickBtn(3);
			}
			else if(curState>0)
			{
				requestType=0;
				PlayerInfo.getInstance().sendRequest(new FriendJson(0,0),this);
			}
			else
			{

//				MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
				UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
				GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
				if(obj!=null)
				{
					MainMenuManager main = obj.GetComponent<MainMenuManager>();
					main.SetData(STATE.ENTER_MAINMENU_BACK);
				}
				GameObjectUtil.destroyGameObjectAllChildrens(itemParent);
				if(items!=null)
				{
					items.Clear();
				}
				frj=null;
				hide();
			}
			break;
		case 1://==删除==//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			curState=1;
			drawUI();
			break;
		case 2://==请求==//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			requestType=6;
			PlayerInfo.getInstance().sendRequest(new FriendMyApplyJson(1),this);
			break;
		case 3://==添加==//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			requestType=2;
			PlayerInfo.getInstance().sendRequest(new FriendRefreshJson(0),this);
			break;
		case 4://==已申请==//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			requestType=4;
			PlayerInfo.getInstance().sendRequest(new FriendMyApplyJson(0),this);
			break;
		case 5://==刷新==//
			//播放音效//
			MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
			if(lastRefreshTime==0)
			{
				lastRefreshTime=15f;
				needResetRefreshBtnInfo = true;
				requestType=2;
				PlayerInfo.getInstance().sendRequest(new FriendRefreshJson(1),this);
			}
			break;
		}
	}
	
	//==type:1为删除,0为正常==//
	private void drawUI()
	{
		switch(curState)
		{
		case 0://==好友列表==//
			//当前好友//
			title.text=TextsData.getData(87).chinese;
			friend_list.SetActive(true);
			friend_delete.SetActive(false);
			friend_add.SetActive(false);
			friend_request.SetActive(false);
			drawFriendNum();
			drawItems();
			
			friendNum.gameObject.SetActive(true);
			break;
		case 1://==删除==//
			title.text=TextsData.getData(88).chinese;
			friend_delete.SetActive(true);
			friend_list.SetActive(false);
			friend_add.SetActive(false);
			friend_request.SetActive(false);
			drawFriendNum();
			drawItems();
			onClickItem(-1);
			
			friendNum.gameObject.SetActive(true);
			break;
		case 2://==新的申请==//
			title.text=TextsData.getData(91).chinese;
			friend_delete.SetActive(false);
			friend_list.SetActive(false);
			friend_add.SetActive(false);
			friend_request.SetActive(false);
			drawItems();
			
			friendNum.gameObject.SetActive(true);
			break;
		case 3://==添加==//
			title.text=TextsData.getData(89).chinese;
			friend_delete.SetActive(false);
			friend_list.SetActive(false);
			friend_add.SetActive(true);
			friend_request.SetActive(false);
			drawItems();
			
			friendNum.gameObject.SetActive(false);
			break;
		case 4://==我的申请==//
			title.text=TextsData.getData(90).chinese;
			friend_delete.SetActive(false);
			friend_list.SetActive(false);
			friend_add.SetActive(false);
			friend_request.SetActive(false);
			drawItems();
			
			friendNum.gameObject.SetActive(true);
			break;
		}
		
	}
	
	public void onClickReceivePower(int param)
	{
//		if(PlayerInfo.getInstance().player.power>=Constant.MaxPower)
//		{
//			ToastWindow.mInstance.showText(TextsData.getData(118).chinese);
//			return;
//		}
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		index=param;
		requestType=0;
		needRefreshFriendList=false;
		PlayerInfo.getInstance().sendRequest(new FriendJson(1,param),this);
	}
	
	public void onClickSendPower(int param)
	{
//		if(PlayerInfo.getInstance().player.power<2)
//		{
//			ToastWindow.mInstance.showText(TextsData.getData(119).chinese);
//			return;
//		}
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		index=param;
		requestType=0;
		needRefreshFriendList=false;
		PlayerInfo.getInstance().sendRequest(new FriendJson(2,param),this);
	}
	
	public void onClickDelete(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		requestType=1;
		index=param;
		FriendElement fe=frj.list[param];
		//确定删除好友[ name ]？//
		string msg=TextsData.getData(99).chinese.Replace("name",fe.name);
		ToastWarnUI.mInstance.showWarn(msg,this);
	}
	
	public void onClickApply(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		requestType=3;
		index=param;
		PlayerInfo.getInstance().sendRequest(new FriendApplyJson(param,isSearch?1:0),this);
	}
	
	public void onClickCancel(int param)
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		requestType=5;
		index=param;
		FriendElement fe=frj.list[param];
		//要取消对[ name ]的好友申请吗？//
		string msg=TextsData.getData(104).chinese.Replace("name",fe.name);
		ToastWarnUI.mInstance.showWarn(msg,this);
	}
	
	public void onClickAgree(int param)
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		requestType=7;
		PlayerInfo.getInstance().sendRequest(new FriendProcessApplyJson(1,param),this);
	}
	
	public void onClickRefuse(int param)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		requestType=7;
		index=param;
		FriendElement fe=frj.list[param];
		//要删除[ name ]的好友申请吗？//
		string msg=TextsData.getData(109).chinese.Replace("name",fe.name);
		ToastWarnUI.mInstance.showWarn(msg,this);
	}
	
	public void onClickSearch()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		string userName=search.value;
		if(string.IsNullOrEmpty(userName))
		{
			//请先输入对方名字！//
			ToastWindow.mInstance.showText(TextsData.getData(110).chinese);
			return;
		}
		if(userName.Equals(PlayerInfo.getInstance().player.name))
		{
			//不能搜索自己！//
			ToastWindow.mInstance.showText(TextsData.getData(116).chinese);
			return;
		}
		requestType=8;
		PlayerInfo.getInstance().sendRequest(new FriendSearchJson(userName),this);
	}
	
	public void onClickBuyFriendTimes()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		FriendCostData fd=FriendCostData.getData(frj.buyNum+1);
		if(fd==null)
		{
			//好友数量已达上限！//
			ToastWindow.mInstance.showText(TextsData.getData(390).chinese);
			return;
		}
		string msg="";
		if(fd.type==1)
		{
			//您是否花费钻石num//
			msg=TextsData.getData(391).chinese.Replace("num",fd.cost+"");
		}
		else
		{
			//您是否花费金币num//
			msg=TextsData.getData(445).chinese.Replace("num",fd.cost+"");
		}
		//扩充好友上限至num？//
		msg+="\n"+TextsData.getData(393).chinese.Replace("num",fd.number1+"");
		requestType=9;
		ToastWarnUI.mInstance.showWarn(msg,this);
	}
	
	//==确定,无视警告==//
	public void warnningSure()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		if(requestType==1)
		{
			PlayerInfo.getInstance().sendRequest(new FriendRemoveJson(index),this);
		}
		else if(requestType==5)
		{
			PlayerInfo.getInstance().sendRequest(new FriendCancelJson(index),this);
		}
		else if(requestType==7)
		{
			PlayerInfo.getInstance().sendRequest(new FriendProcessApplyJson(2,index),this);
		}
		else if(requestType==9)
		{
			PlayerInfo.getInstance().sendRequest(new BuyFriendJson(),this);
		}
	}
	
	public void warnningCancel(){}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 0://==查看好友列表==//
				frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				receiveData=true;
				break;
			case 1://==删除好友==//
				frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				receiveData=true;
				break;
			case 2://==刷新玩家==//
				frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				receiveData=true;
				break;
			case 3://==申请好友==//
				frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				receiveData=true;
				break;
			case 4://==我的申请==//
				frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				receiveData=true;
				break;
			case 5://==取消申请==//
				frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				receiveData=true;
				break;
			case 6://==申请我==//
				frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				receiveData=true;
				break;
			case 7://==处理申请==//
				frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				receiveData=true;
				break;
			case 8://==搜索==//
				frj=JsonMapper.ToObject<FriendResultJson>(json);
				errorCode = frj.errorCode;
				receiveData=true;
				break;
			case 9://==购买好友上限==//
				BuyResultJson brj=JsonMapper.ToObject<BuyResultJson>(json);
				errorCode=brj.errorCode;
				if(errorCode==0)
				{
					frj.buyNum=brj.buyTimes;
					buyFriendUseCrystal = PlayerInfo.getInstance().player.crystal-brj.crystal;
					PlayerInfo.getInstance().player.gold=brj.gold;
					PlayerInfo.getInstance().player.crystal=brj.crystal;
				}
				receiveData=true;
				break;
			}
		}
	}
	
}
