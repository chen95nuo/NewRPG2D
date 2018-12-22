using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActiveWroldSelManager : MonoBehaviour , ProcessResponse {

//	public static ActiveWroldSelManager mInstance;
	public UIScrollBar ScrollBar;
	public GameObject ScrollView;
	public GameObject GridList;
	public GameObject Arrow_L;
	public GameObject Arrow_R;
	public UILabel CDTimeLabel;
	//当前选择的副本的关卡id//
	public int curSelEventId;
	
	
	private Transform _myTransform;
	
	//1, 返回活动副本界面 2,进入战斗 , 3 获得选关界面信息//
	private int requestType;
	private bool receiveData;
	private List<string> strData;
	private List<GameObject> eventList;
	private GameObject loadPrefab;
	private string loadPath = "Prefabs/UI/ActiveWrold/EventSelItem";

	private List<string> curEventDatas;
	private int errorCode;
	
	private bool needShowGuideByFinishBattle = false;
	private bool finishRequestData = false;
	
	private int cdTime;
	private float timeCount;

    CardGroupResultJson cardGroupRJ;
	
	/*
	 * 活动副本主界面控制类
	 * 
	 */
	void Awake(){
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		_myTransform = transform;
		init();
//		hide();
	}
	
	public void init ()
	{
//		base.init ();
		strData = new List<string>();
		curEventDatas = new List<string>();
		eventList = new List<GameObject>();
	}

	// Use this for initialization
	void Start () {
        if (PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_EVENT)
        {
            if (PlayerInfo.getInstance().isEvent)
            {

            }
            else
            {
                PlayerInfo.getInstance().BattleOverBackType = 0;
                //			MainMenuManager.mInstance.hide();
                //隐藏主城//
                if (UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
                {
                    UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                }
                //			WarpSpaceUIManager.mInstance.hide();
                //			ArenaUIManager.mInstance.hide();
                requestType = 3;
                PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SELECT_EVENT, PlayerInfo.getInstance().copyId), this);
                //播放声音//
                string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MAZE).music;
                MusicManager.playBgMusic(musicName);
                needShowGuideByFinishBattle = true;
            }
        }
        else
        {
            //			hide();

        }
	}
	
	// Update is called once per frame
	void Update () {
		if(finishRequestData)
		{
			if(needShowGuideByFinishBattle)
			{
				needShowGuideByFinishBattle = false;
				checkIsCanRunGuide();
			}
		}
		if(receiveData){
			receiveData = false;
			if(errorCode == -3)
				return;
			if(errorCode==53)			//背包空间不足且没有达到上限//
			{
//				ToastWindow.mInstance.showText(TextsData.getData(78).chinese);
				string str = TextsData.getData(78).chinese;
				UIJumpTipManager.mInstance.SetPackageTypeData(UIJumpTipManager.UI_JUMP_TYPE.UI_EXTENDPACKAGE, str);
				
				return;
			}
			else if(errorCode==131)			//背包空间不足且达到上限//
			{
//				ToastWindow.mInstance.showText(TextsData.getData(78).chinese);
				string str = TextsData.getData(78).chinese;
				UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_PACKAGE, str);
				
				return;
			}
			
			switch(requestType){
			case 1:			//返回活动副本界面//
				if(errorCode == 0){
					
//					ActiveWroldUIManager.mInstance.setData(strData);
					//打开活动副本界面//
					UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY);
					ActiveWroldUIManager activeCopy = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY, 
						"ActiveWroldUIManager")as ActiveWroldUIManager;
					activeCopy.setData(strData);
					hide();
				}
				break;
			case 2:			//进入战斗//
				if(errorCode == 0){
					PlayerInfo.getInstance().battleType = STATE.BATTLE_TYPE_EVENT;
					PlayerInfo.getInstance().BattleOverBackType = STATE.BATTLE_BACK_EVENT;
					
					GameObjectUtil.LoadLevelByName(STATE.GAME_SCENE_NAME_GAME);
				}
				else if(errorCode == 24)		//进入次数到达上限，更高的vip等级可以增加进入次数//
				{
					string str = TextsData.getData(579).chinese;
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
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
			case 3:				//获取界面信息//
				setData(strData, cdTime);
				finishRequestData = true;
				break;
            case 5:
                UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
                CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager") as CombinationInterManager;

                combination.curCardGroup = cardGroupRJ.transformCardGroup();

                combination.SetData(5, new List<string>(),new List<string>(),curSelEventId);

                //				//关闭主菜单选项卡//
                //				MainMenuManager.mInstance.hide();
                //隐藏主城//
                if (UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
                {
                    UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
                }
                hide();
                break;
			}
			
		}
		
		//显示箭头//
		if( curEventDatas.Count<=4){
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
		
		//修改冷却时间//
		ChangeCDTimeLb();
	}
	
	void checkIsCanRunGuide()
	{
		// unlock guide
		int curGuideID = GuideManager.getInstance().getCurrentGuideID();
		switch(curGuideID)
		{
		case (int)GuideManager.GuideType.E_UnlockBreak:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Break))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		case (int)GuideManager.GuideType.E_Spirit:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Spirit))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		}
	}
	
	public void show ()
	{
//		base.show ();
	}
	
	//list格式：id-num(完成次数)//
	//time 进入冷却时间//
	public void setData( List<string> strList, int time){
		show();
		curEventDatas = strList;
		this.cdTime = time;
		initCopyData();
		if(time > 0)
		{
			CDTimeLabel.gameObject.SetActive(true);
			int min = cdTime / 60;		//分钟//
			int sec = cdTime % 60;		//秒//
			if(sec > 0)
			{
				min +=1;
			}
			CDTimeLabel.text = TextsData.getData(310).chinese + min + TextsData.getData(335).chinese;
		}
		else
		{
			CDTimeLabel.gameObject.SetActive(false);
		}
	}
	
	//修改冷却时间//
	public void ChangeCDTimeLb()
	{
		
		
		//修改显示时间//
		if(cdTime  > 0 && CDTimeLabel != null && CDTimeLabel.gameObject.activeSelf)
		{
			timeCount += Time.deltaTime;
			if(timeCount > 1)				//每过一秒钟修改一次//
			{
				cdTime --;
				timeCount = 0;
				
				int min = cdTime / 60;		//分钟//
				int sec = cdTime % 60;		//秒//
				
				if(sec > 0)
				{
					min +=1;
				}
				CDTimeLabel.text = TextsData.getData(310).chinese + min + TextsData.getData(335).chinese;
			}
		}
		
		else if(cdTime <= 0 && CDTimeLabel.gameObject.activeSelf)
		{
			timeCount = 0;
			CDTimeLabel.gameObject.SetActive(false);
		}
	}
	
	
	
	//初始化副本选项数据//
	public void initCopyData(){
		CleanList();
		CleanScrollView();
		int selNum = curEventDatas.Count;
		if(selNum < 4){
			selNum = 4;
		}
		for(int i = 0;i < selNum;i++){
			if(loadPrefab==null)
			{
				loadPrefab = Resources.Load(loadPath) as GameObject;
			}
			GameObject item = Instantiate(loadPrefab) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(item,GridList);
			
			//像链表中添加数据//
			eventList.Add(item);
			
			UISprite icon = item.transform.FindChild("Icon").GetComponent<UISprite>();
			icon.gameObject.SetActive(false);
			UISprite skillIcon = item.transform.FindChild("SkillIcon").GetComponent<UISprite>();
			skillIcon.gameObject.SetActive(false);
			UILabel name = item.transform.FindChild("Name").GetComponent<UILabel>();
			UILabel num = item.transform.FindChild("Num").GetComponent<UILabel>();
			UILabel level = item.transform.FindChild("Level").GetComponent<UILabel>();
			UILabel power = item.transform.FindChild("Power").GetComponent<UILabel>();
//			GameObject lockIcon = item.transform.FindChild("Locked").gameObject;
			
			UIButtonMessage ubm = item.GetComponent<UIButtonMessage>();
			ubm.target = _myTransform.gameObject;
			ubm.functionName = "OnClickBtn";
			
			
				
			
			if(i < curEventDatas.Count){
				//icon.gameObject.SetActive(true);
				name.gameObject.SetActive(true);
				num.gameObject.SetActive(true);
				level.gameObject.SetActive(true);
				power.gameObject.SetActive(true);
				
				string[] str = curEventDatas[i].Split('-');
				int eventId = StringUtil.getInt( str[0]);
				int n = StringUtil.getInt(str[1]);		//进进入次数//
//				EventData ed = EventData.getData(copyId);
				
				EventItem ei = item.GetComponent<EventItem>();
				ei.eventId = eventId;
				ei.nums = n;
				
				FBeventData fbed = FBeventData.getData(eventId);
				//修改icon和名字//
				string atlasName = fbed.atlas;
				if(string.Equals(atlasName,"SkillCircularIcom"))
				{
					skillIcon.gameObject.SetActive(true);
					skillIcon.spriteName = fbed.bossicon;
					UIAtlas iconAtlas = LoadAtlasOrFont.LoadAtlasByName(fbed.atlas);
					skillIcon.atlas = iconAtlas;
				}
				else
				{
					icon.gameObject.SetActive(true);
					icon.spriteName = fbed.bossicon;
					UIAtlas iconAtlas = LoadAtlasOrFont.LoadAtlasByName(fbed.atlas);
					icon.atlas = iconAtlas;
				}
				
				//名字//
				name.text = fbed.name;
                if(PlayerInfo.getInstance().player.level < fbed.unlocklevel)
                {
                    name.text = TextsData.getData(138).chinese.Remove(0,1).Remove(3);
                    n = 0;
                }
				//进入次数//
				string s1 = TextsData.getData(147).chinese;

                if (PlayerInfo.getInstance().player.vipLevel > 0)
                {
                    num.text = s1 + n + "/" + VipData.getData(PlayerInfo.getInstance().player.vipLevel).activity;
                }
                else
                    num.text = s1 + n + "/" + fbed.entry;
//                level.gameObject.SetActive(false);
				//需要等级//
                //if(fbed.unlocklevel > 0){
					
                //    s1 = TextsData.getData(146).chinese;
                //    level.text = "LV." + fbed.unlocklevel;
                //}
                //else {
                //    level.gameObject.SetActive(false);
                //}
				//需求等级//
                s1 = TextsData.getData(146).chinese;
                level.text = s1 + fbed.unlocklevel;
				//需要体力//
                s1 = TextsData.getData(13).chinese;
                power.text= s1 + fbed.cost;
	
				ubm.param = eventId;
			}
			else
			{
				
				icon.spriteName = "empty";
				
				name.gameObject.SetActive(false);
				num.gameObject.SetActive(false);
				level.gameObject.SetActive(false);
				power.gameObject.SetActive(false);
				
				ubm.param = -1;
				
			}
          
			
			//为scrollView复制//
			UIDragPanelContents udpc = item.GetComponent<UIDragPanelContents>();
			udpc.draggablePanel = ScrollView.GetComponent<UIDraggablePanel>();
		}
		
	}
	
	
	public void hide ()
	{
//		base.hide ();
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_COPYSELECT);
	}
	
	private void gc()
	{
		strData.Clear();
		eventList.Clear();
		loadPrefab=null;
		Resources.UnloadUnusedAssets();
	}
	
	public void CleanScrollView(){
		GridList.GetComponent<UIGrid>().repositionNow = true;
		ScrollBar.GetComponent<UIScrollBar>().value = 0;
		ScrollView.transform.localPosition = Vector3.zero;
		ScrollView.GetComponent<UIPanel>().clipRange = new Vector4(0,0,760,260);
	}
	
	public void CleanList(){
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
		eventList.Clear();
	}
	
	//选择关卡进行战斗请求//
	public void SendToBattle()
	{
		//选择关卡进行战斗//
		requestType = 2;
		//Debug.Log("ActiveWroldSelManager :  copyId =========== " + PlayerInfo.getInstance().copyId);
		PlayerInfo.getInstance().sendRequest(new EventBattleJson(curSelEventId, PlayerInfo.getInstance().copyId),this);
	}
	
	
	//按键响应 id 为该副本在表中关卡的id//
	public void OnClickBtn(int id){
		curSelEventId = id;
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		if(id > 0){

			int num = 0;
			//进入次数跟VIP等级有关//
			int totalNum = VipData.getData(PlayerInfo.getInstance().player.vipLevel).activity;
			//int power = 0;
			int level = 0;
			for(int i = 0;i < eventList.Count;i++){
				GameObject item = eventList[i];
				EventItem ei = item.GetComponent<EventItem>();
				if(ei.eventId == id){
					num = ei.nums;
					FBeventData fed = FBeventData.getData(ei.eventId);
//					totalNum = fed.entry;
					//power = fed.cost;
					level = fed.unlocklevel;
				}
			}
            
//            else if(PlayerInfo.getInstance().player.power < power)		//体力不足//
//            {
////				string str = TextsData.getData(149).chinese;
////				ToastWindow.mInstance.showText(str);
//                int buyType = 2;
//                int jsonType = 1;
//                int costType = 1;
//                ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_ACTIVESEL);
//            }
            if(PlayerInfo.getInstance().player.level < level)
            {
                //string str = TextsData.getData(372).chinese;
                string two = TextsData.getData(367).chinese.Replace("num",""+ level);
                ToastWindow.mInstance.showText(two);
            }
			else 
			{
				
				//选择关卡进行战斗//
//				requestType = 2;
//				PlayerInfo.getInstance().sendRequest(new EventBattleJson(id, PlayerInfo.getInstance().copyId),this);
				if(num >= totalNum){			//进入次数达到上限，够买vip等级可以增加进入次数//
					string str = TextsData.getData(579).chinese;
//					ToastWindow.mInstance.showText(str);
					//提示去充值//
					UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, str);
				}
				else
				{

                    FBeventData ed = FBeventData.getData(curSelEventId);

                    if (ed.leveltype == 1)
                    {
                        requestType = 5;
                        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_CARDGROUP, 0), this);
                    }
                    else
                        SendToBattle();
				}
			}
			
		}
	}
	
	public void OnBackBtn(){
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		
		requestType = 1;
		//发送进入异世界（活动副本）请求信息//
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_EVENT),this);
	}
	
	public void receiveResponse (string json)
	{
		if(json != null){
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			Debug.Log("ActiveWroldSelManager : " + json);
			switch(requestType){
			case 1:				//返回副本界面//
			case 3:				//获得本界面信息//
				strData.Clear();
				EventResultJson erj = JsonMapper.ToObject<EventResultJson>(json);
				errorCode = erj.errorCode;
				if(errorCode == 0)
				{
					strData = erj.s;
					errorCode = erj.errorCode;
					cdTime = erj.cdtime;
				}
				receiveData = true;
				break;
			case 2:
				EventBattleResultJson ebrj=JsonMapper.ToObject<EventBattleResultJson>(json);
				errorCode = ebrj.errorCode;
				if(errorCode == 0)
				{
					//设置战斗数据//
					//				PlayerInfo.getInstance().bNum=ebrj.bNum;
					PlayerInfo.getInstance().ebrj = ebrj;
					errorCode = ebrj.errorCode;
				}
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
			}
		}
	}
}
