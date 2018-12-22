using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WarpSpaceUIManager : MonoBehaviour , ProcessResponse,BWWarnUI {
	
//	public static WarpSpaceUIManager mInstance;
	public GameObject ScrollView;
	public GameObject GridList;
	public GameObject ScrollBar;
	public GameObject BlackBg;
	public GameObject BuyTipObj;
	public GameObject InfoBoxObj;
	public GameObject rewardInfo;//奖励详细信息界面//
	public GameObject rewardInfoParet;
	public UILabel CostTipLb;
	public UILabel MazeCDTimeLb;
	
	private GameObject rewardInfoItem;
	
	private Transform _myTransform;
	private List<GameObject> mazeList;
	//1, 进入迷宫 2, 额外购买进入迷宫的权限 , 3 获取扭曲空间各个迷宫信息, 4 重置信息//
	private int requestType;
	private bool receiveData;
	//进入迷宫的类型，1 免费进入， 2 付费进入//
	private int intoMazeType = -1;
	//错误序列//
	private int errorCode;
	Vector3 scrollStartLocalPos;
	GameObject loadPrefab;
	string loadPath = "Prefabs/Item/MazeItem";
	//重置后服务器返回的数据，string, 迷宫id-位置-次数-付费次数//
	string receiveStr;
	
	bool needShowGuideByFinishBattle = false;
	bool finishRequestData = false;
	
	int curSelMazeId = -1;
	//购买类型：1 购买付费进入迷宫，2 购买重置数据//
	int buyType;
	//进入冷却时间//
	int cdTime;
	//重置迷宫花费的水晶数//
	int resetCost;
	//计时器//
	float timeCount;

    private MapResultJson mapRJ;
	
	//int useCrystal;
	
	MazeResultJson mrj;
	
	public static string mazeWish;//已经许愿的物品//
	
	public static string mazeBossDrop;//可以许愿的物品//
	
	private Hashtable mazeWishHash = new Hashtable();
	
	private int curWishId;
	
	private string[] curMazeResetInfo;
	void Awake(){
//		mInstance = this;
//		_MyObj = mInstance.gameObject;
		_myTransform = transform;
		init();
	}

	// Use this for initialization
	void Start () {
		
		
		if(PlayerInfo.getInstance().mbrj != null){
			
			Debug.Log("state : " + PlayerInfo.getInstance().mbrj.state);
		}
		if(PlayerInfo.getInstance().BattleOverBackType == STATE.BATTLE_BACK_WRAPSPACE  ){
			PlayerInfo.getInstance().BattleOverBackType = 0; 
//			MainMenuManager.mInstance.hide();
			//隐藏主城//
			if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
			{
				UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
			}
			HeadUI.mInstance.hide();
			//获取界面数据//
			requestType = 3;
			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_WARPSPACE),this);
			//播放声音//
			string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MAZE).music;
			MusicManager.playBgMusic(musicName);
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
			{
				UISceneDialogPanel.mInstance.showDialogID(33);
			}
			needShowGuideByFinishBattle = true;
		}
		else {
//			hide();
			
		}
	}
	
	public void init ()
	{
//		base.init ();
		mazeList = new List<GameObject>();
		string s1 = TextsData.getData(61).chinese;
		string s2 = TextsData.getData(62).chinese;
		MazeData md = MazeData.getData(1);
		CostTipLb.text = s1 + md.energy + s2;
		scrollStartLocalPos = ScrollView.transform.localPosition;
		rewardInfo.SetActive(false);
	}
	
	// Update is called once per frame
    void Update()
    {
        if (finishRequestData)
        {
            if (needShowGuideByFinishBattle)
            {
                needShowGuideByFinishBattle = false;
                checkIsCanRunGuide();
            }
        }
        if (receiveData)
        {
            receiveData = false;
			if(errorCode == -3)
				return;
			
            switch (requestType)
            {
                case 1:			//进入迷宫//
                    receiveData = false;
                    if (errorCode == 0)
                    {

                        //付费进入时，扣除水晶//
                        MazeData md = MazeData.getData(curSelMazeId);
                        if (intoMazeType == 2)
                        {
                            PlayerInfo.getInstance().player.crystal -= md.expense;
                        }

                        if (PlayerInfo.getInstance().costPowerMark == 1)
                        {

                            //每次进入扣除一定体力//
                            PlayerInfo.getInstance().player.power -= md.energy;
                        }
                        /**更新UI头**/
                        //					HeadUI.mInstance.refreshPlayerInfo();
                        //					PlayerInfo.getInstance().battleOverBackMaze = true;
                        PlayerInfo.getInstance().BattleOverBackType = STATE.BATTLE_BACK_MAZE;
                        //创建迷宫界面//
                        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE);
                        NewMazeUIManager maze = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAZE, "NewMazeUIManager") as NewMazeUIManager;
                        //maze.setData(PlayerInfo.getInstance().curMazeId, PlayerInfo.getInstance().curPosId);
						NewMazeUIManager.isFirstComeIn = true;
						maze.SetData(mrj.number,mrj.td,mrj.cb,mrj.mcb);
					
						if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_WarpSpace))
						{
							GuideUI17_WarpSpace.mInstance.hideAllStep();
							UISceneDialogPanel.mInstance.showDialogID(21);
						}
						
                        HeadUI.mInstance.hide();
                        hide();
                       
                    }
                    else if (errorCode == 19)
                    {			//水晶不足//
                        string ss = TextsData.getData(73).chinese;
                        //					ToastWindow.mInstance.showText(ss);
                        //提示去充值//
                        UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, ss);
                    }
                    else if (errorCode == 27)
                    {			//体力不足//
                        //					string ss = TextsData.getData(72).chinese;
                        //					ToastWindow.mInstance.showText(ss);
                        int buyType = 2;
                        int jsonType = 1;
                        int costType = 1;
                        ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_MAZE);
                    }
                    else if (errorCode == 86)			//冷却时间未到，无法进入//
                    {
                        int buyType = 5;
                        int jsonType = 1;
                        int costType = 1;
                        int cdType = 2;			//迷宫//
                        ShowBuyTipControl.mInstance.SendToGetUIData(jsonType, buyType, costType, 0, 0, BuyTipManager.UI_TYPE.UI_MAZE, cdType);
                    }
				    else if (errorCode == 70)			//vip等级不足//			
                    {
                        string ss = TextsData.getData(243).chinese;
                        //					ToastWindow.mInstance.showText(ss);
                        //提示去充值//
                        UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, ss);
                    }
					else if(errorCode == 53)		//背包满了，无法进入迷宫//
					{
						//@add by liuqing//
						string str = TextsData.getData(78).chinese;
						UIJumpTipManager.mInstance.SetPackageTypeData(UIJumpTipManager.UI_JUMP_TYPE.UI_EXTENDPACKAGE, str);
					}
                    break;
                case 2:

                    break;
                case 3:			//获取扭曲空间中的数据//
                    receiveData = false;
                    SetData(PlayerInfo.getInstance().curOpenMazeId, PlayerInfo.getInstance().curIntoMaze, curSelMazeId, cdTime);
                    finishRequestData = true;
                    break;
                case 4:			//重置数据//
                    receiveData = false;
                    /**更新UI头**/
                    //				HeadUI.mInstance.refreshPlayerInfo();
                    if (errorCode == 0)
                    {			//重置成功//

                        //修改迷宫数据//
                        for (int i = 0; i < mazeList.Count; i++)
                        {
                            MazeItem item = mazeList[i].GetComponent<MazeItem>();
                            if (item.id == curSelMazeId)
                            {
                                //迷宫id-位置-次数-付费次数//
                                string[] str = receiveStr.Split('-');
                                int id = StringUtil.getInt(str[0]);
                                int state = StringUtil.getInt(str[1]);
                                int num = StringUtil.getInt(str[2]);
                                int paynum = StringUtil.getInt(str[3]);
                                item.id = id;
                                item.state = state;
                                item.num = num;
                                item.payNum = paynum;
                            }
                        }
						if(!TalkingDataManager.isTDPC)
						{
							TDGAItem.OnPurchase("mazerefresh", 1, resetCost);//购买重置迷宫//
						}
                        setMazeShow();
                    }
                    else if (errorCode == 61)
                    {			//进入次数未达上限//
                        string s = TextsData.getData(162).chinese;
                        ToastWindow.mInstance.showText(s);


                    }
                    else if (errorCode == 60)
                    {			//进入该迷宫次数和充值次数都达到上限//
                        string s = TextsData.getData(163).chinese;
                        ToastWindow.mInstance.showText(s);
                    }
                    else if (errorCode == 19)
                    {			//水晶不足//
                        string s = TextsData.getData(164).chinese;
                        //					ToastWindow.mInstance.showText(s);
                        //提示去充值//
                        UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, s);
                    }
                    else if (errorCode == 70)			//vip等级不足//			
                    {
                        string ss = TextsData.getData(243).chinese;
                        //					ToastWindow.mInstance.showText(ss);
                        //提示去充值//
                        UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, ss);
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
				case 11:
					if(errorCode == 0)
					{
						//操作许愿图片显示//
						ChangeWishSpirt();
					}
					else if(errorCode == 122)
					{
						ToastWindow.mInstance.showText(TextsData.getData(686).chinese);
					}
					break;
            }
        }

        ChangeCDTimeLb();
    }
	
	void checkIsCanRunGuide()
	{
		// unlock guide
		int curGuideID = GuideManager.getInstance().getCurrentGuideID();
		switch(curGuideID)
		{
		case (int)GuideManager.GuideType.E_ActiveCopy:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_ActiveCopy))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		case (int)GuideManager.GuideType.E_PVP:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_PVP))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
		case (int)GuideManager.GuideType.E_Rune:
		{
			if(RequestUnlockManager.mInstance.isCanSendUnlockRequestMsg((int)RequestUnlockManager.MODELID.E_Rune))
			{
				RequestUnlockManager.mInstance.requestUnlockMsg();
			}
		}break;
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
		BlackBg.SetActive(false);
		BuyTipObj.SetActive(false);
		//播放声音//
		string musicName = MusicData.getDataByType(STATE.MUSIC_TYPE_MAZE).music;
		MusicManager.playBgMusic(musicName);
		
		setMazeShow();
		
	}
	/// <summary>
	/// Sets the data. 获取扭曲空间的信息
	/// </summary>
	/// <param name='ids'>
	/// Identifiers.	已经解锁的迷宫的id的list 格式id-type(0未解锁， 1解锁)-cost(重置花费的钻石数)
	/// </param>
	/// <param name='maze'>
	/// Maze.		今天进入过的迷宫，格式：迷宫id-位置-次数，迷宫id-位置-次数
	/// </param>
	/// <param name='time'>
	/// time  进入冷却时间
	/// </param>
	public void SetData(List<string> ids, string maze, int selMazeId, int time){
	
		mazeList.Clear();
		CleanScrollView();
		
		curSelMazeId = selMazeId;
		cdTime = time;
		
		ChangeCDTimeLb();
		
		if(ids != null){
			//初始化迷宫数据//
			for(int i =0 ;i < ids.Count;i++){
				string[] str0 = ids[i].Split('-');
				int mId = StringUtil.getInt(str0[0]);
				int mark = StringUtil.getInt(str0[1]);
				int cost = StringUtil.getInt(str0[2]);
				if(loadPrefab==null)
				{
					loadPrefab = Resources.Load(loadPath) as GameObject;
				}
				GameObject item = Instantiate(loadPrefab) as GameObject;
				GameObjectUtil.gameObjectAttachToParent(item,GridList);
				//设置每个item中的信息//
				MazeItem mi = item.GetComponent<MazeItem>();
				mi.id = mId;
				mi.mark = mark;
				mi.resetCost = cost;
				mi.state = 100;
				mi.num = 0;
				mi.rewardInfo.functionName = "ShowRewardInfo";
				mi.rewardInfo.target = this.gameObject;
				mi.rewardInfo.param = mId;
				MazeData md = MazeData.getData(mi.id);

				
				//根据迷宫的id获得每个迷宫的icon的图片//
				mi.Icon.spriteName = md.icon;
				mi.IconBlack.spriteName = md.icon;
				mi.Name.text = md.name;
				if(mark > 0){
					mi.Locked.gameObject.SetActive(false);
				}
				else{
					string locktext = TextsData.getData(159).chinese;
					mi.Locked.text = md.condition + locktext;
				}
				
				mazeList.Add(item);
				
				//为点击时间赋值//
				UIButtonMessage ubm = item.GetComponent<UIButtonMessage>();
				ubm.target = _myTransform.gameObject;
				ubm.param = mi.id;
				//为scrollView复制//
				UIDragPanelContents udpc = item.GetComponent<UIDragPanelContents>();
				udpc.draggablePanel = ScrollView.GetComponent<UIDraggablePanel>();
			}
			
			//修改当前进入的迷宫的数据//
		
			string[] str = maze.Split(',');
			curMazeResetInfo = str;
			for(int i=0; i < str.Length;i++){
				string[] ss = str[i].Split('-');
				if(ss!= null && ss.Length>0 && ss[0] != ""){
					
					int id = StringUtil.getInt(ss[0]);
					int pos = 0;
					int times = 0;
					int payNum = 0;
					if(ss.Length >1){
						
						pos = StringUtil.getInt(ss[1]);
					}
					if(ss.Length>2){
						
						times = StringUtil.getInt(ss[2]);
					}
					if(ss.Length > 3){
						payNum = StringUtil.getInt(ss[3]);
					}
					foreach(GameObject go in mazeList)
					{
						MazeItem mi = go.GetComponent<MazeItem>();
						if(mi.id == id){
							mi.state = pos;
							mi.num = times;
							mi.payNum = payNum;
							PlayerInfo.getInstance().prePosId = PlayerInfo.getInstance().curPosId;
							PlayerInfo.getInstance().curPosId = mi.state;
							//MazeData md = MazeData.getData(id);

						}
					}
				}
			}
			
			if(ids.Count <= 0){
				//您还未解锁任何迷宫，继续努力吧！//
				string ss = TextsData.getData(79).chinese;
				ToastWindow.mInstance.showText(ss);
			}
			
			
		}
			show();
	}
	
	public void ChangeCDTimeLb()
	{
		//修改显示时间//
		if(cdTime  > 0 && MazeCDTimeLb != null && MazeCDTimeLb.gameObject.activeSelf)
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
				MazeCDTimeLb.text = TextsData.getData(310).chinese + min + TextsData.getData(335).chinese;
			}
		}
		
		else if(cdTime <= 0 && MazeCDTimeLb.gameObject.activeSelf)
		{
			timeCount = 0;
			MazeCDTimeLb.gameObject.SetActive(false);
		}
	}
	
	
	public void setMazeShow(){
		//将选中的迷宫的图片变亮//
		MazeItem mi = null;
		for(int i = 0;i < mazeList.Count;i ++){
			MazeItem item =  mazeList[i].GetComponent<MazeItem>();
			if(curSelMazeId == item.id && item.mark == 1){
				mi = item;
				item.IconBlack.gameObject.SetActive(false);
				item.Icon.gameObject.SetActive(true);
				//显示奖励详细按钮//
				item.rewardInfo.gameObject.SetActive(true);
			}
			else {
				item.IconBlack.gameObject.SetActive(true);
				item.Icon.gameObject.SetActive(false);
				//隐藏奖励详细按钮//
				item.rewardInfo.gameObject.SetActive(false);
			}
		}
		//修改详细信息中的内容//
		if(curSelMazeId < 0){
			InfoBoxObj.SetActive(false);
		}
		else 
		{
			if(mi != null){
				InfoBoxObj.SetActive(true);
				UILabel Name = InfoBoxObj.transform.FindChild("Name").GetComponent<UILabel>();
				UILabel Des = InfoBoxObj.transform.FindChild("Des").GetComponent<UILabel>();
				UILabel Reward = InfoBoxObj.transform.FindChild("Reward").GetComponent<UILabel>();
				UILabel EnterTimes = InfoBoxObj.transform.FindChild("EnterTimes").GetComponent<UILabel>();
				MazeData md = MazeData.getData(mi.id);
				string s1 = TextsData.getData(157).chinese;	//级//
				Name.text = md.name + "(" + md.condition + s1 + ")";
				Des.text = md.description;
				Reward.text = md.output;
				s1 = TextsData.getData(156).chinese;		//进入次数//
				string s2 = TextsData.getData(158).chinese;	//体力//
				EnterTimes.text = s1 + mi.num + "/" + md.freeentry + "(" + md.energy + s2 + ")";
				
			}
			
		}
	}
	

	
	public void CleanScrollView(){
		GridList.GetComponent<UIGrid>().repositionNow = true;
		ScrollBar.GetComponent<UIScrollBar>().value = 0;
		ScrollView.transform.localPosition = scrollStartLocalPos;
		ScrollView.GetComponent<UIPanel>().clipRange = new Vector4(0,0,700,400);
	}
	
	public void hide ()
	{
		
//		base.hide ();
		CleanData();
		gc();
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE);
	}
	
	private void gc()
	{
		loadPrefab=null;
		Resources.UnloadUnusedAssets();
	}
	
	public void CleanData(){
		mazeList.Clear();
		GameObjectUtil.destroyGameObjectAllChildrens(GridList);
	}
	
	//点击迷宫item，则改迷宫item变亮，其他的是显示black，并修改旁边提示框中的内容//
	public void OnClickBtn(int id){
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		for(int i = 0;i < mazeList.Count; i++){
			MazeItem item =  mazeList[i].GetComponent<MazeItem>();
			if(item.mark > 0 && item.id == id){
				
				curSelMazeId = id;
				//修改迷宫的显示//
				setMazeShow();
			}
		}
		
	}
	
	//点击进入按钮，发送数据//
	public void OnEnterBtn()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		SendToIntoMaze();
	}
	
	public void SendToIntoMaze()
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
		{
			return;
		}
		MazeItem mi = null;
		for(int i = 0;i < mazeList.Count; i++){
			
			MazeItem item =  mazeList[i].GetComponent<MazeItem>();
			if(curSelMazeId == item.id){
				mi = item;
			}
		}
		if(mi != null){
			
			MazeData md = MazeData.getData(curSelMazeId);
			//没有付费进入了//
			/**
			//可以付费进入//
			if(mi.num >= md.freeentry && mi.num < md.freeentry + md.payentry && mi.payNum < 1){
				BlackBg.SetActive(true);
				BuyTipObj.SetActive(true);
				buyType = 1;
				UILabel tipLb = BuyTipObj.transform.FindChild("TipLabel").GetComponent<UILabel>();
				string s1 = TextsData.getData(63).chinese;
				string s2 = TextsData.getData(64).chinese;
				tipLb.text = s1 + md.expense + s2;
			}
			//免费次数和付费次数都用尽//
			else if(mi.num == md.freeentry + md.payentry){
				string ss = TextsData.getData(65).chinese;
				ToastWindow.mInstance.showText(ss);
			}
			//免费进入//
			else if(mi.num < md.freeentry || (mi.num >= md.freeentry && mi.num < md.freeentry + md.payentry && mi.payNum == 1)){
				
				requestType = 1;
				//免费进入//
				intoMazeType = 1;
				Debug.Log("WarpSpaceUIManager : " + curSelMazeId);
				//请求进入迷宫信息//
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAZE_INTO, curSelMazeId, 0),this);
			}
			**/
			
			//免费次数用尽//
			if(mi.num == md.freeentry ){
				//亲，今天的进入次数已用尽！//
				string ss = TextsData.getData(65).chinese;
				ToastWindow.mInstance.showText(ss);
			}
			//免费进入//
			else if(mi.num < md.freeentry ){
				
				requestType = 1;
				//免费进入//
				intoMazeType = 1;
				Debug.Log("WarpSpaceUIManager : " + curSelMazeId);
				//请求进入迷宫信息//
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAZE_INTO, curSelMazeId, 0),this);
			}
		}
	}
	
	
	//重置按钮//
	public void OnResetBtn(){
		
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
		{
			return;
		}
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		MazeItem mi = null;
		for(int i = 0;i < mazeList.Count;i++){
			MazeItem item = mazeList[i].GetComponent<MazeItem>();
			if(item.id == curSelMazeId){
				mi = item;
			}
		}
		MazeData md = MazeData.getData(mi.id);
		if(mi.num < md.freeentry){
			//次数尚未用尽，无法重置！//
			string s = TextsData.getData(162).chinese;
			ToastWindow.mInstance.showText(s);
		}
		else {
			int vipLevel = PlayerInfo.getInstance().player.vipLevel;
			VipData vd = VipData.getData(vipLevel);
			int curMazeResetNum = 0;
			for(int i = 0;i<curMazeResetInfo.Length;i++)
			{
				string[] ss = curMazeResetInfo[i].Split('-'); 
				if(curSelMazeId == StringUtil.getInt(ss[0]))
				{
					curMazeResetNum = StringUtil.getInt(ss[3]);
				}
			}

				if(vd.maze>curMazeResetNum)
				{
					BlackBg.SetActive(true);
					BuyTipObj.SetActive(true);
					buyType = 2;
					UILabel tipLb = BuyTipObj.transform.FindChild("TipLabel").GetComponent<UILabel>();
					string s1 = TextsData.getData(160).chinese;
					string s2 = TextsData.getData(161).chinese;
					tipLb.text = s1 + mi.resetCost + s2;
					resetCost = mi.resetCost;
				}
				else
				{
					if(vd.level<12)
					{
					 	string ss = TextsData.getData(243).chinese;
	                        //提示去充值//
	                 	UIJumpTipManager.mInstance.SetData(UIJumpTipManager.UI_JUMP_TYPE.UI_CHARGE, ss);
					}
					else
					{
						//vip已达到最大，重置次数用尽//
						string s = TextsData.getData(163).chinese;
						ToastWindow.mInstance.showText(s);
					}
				}
			
		}
	}
	
	
	
	//back click down//
	public void OnBackClick(){
		
//        //播放音效//
//            MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		
////		MainUI.mInstance.show();
//        //返回主城界面//
//        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
//        MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU,
//            "MainMenuManager") as MainMenuManager;
//        main.SetData(STATE.ENTER_MAINMENU_BACK);
//        HeadUI.mInstance.show();
////		MainMenuManager.mInstance.show();
////		MainMenuManager.mInstance.CloseSpr();
////		MainMenuManager.mInstance.OnClickBtn(4);
//        hide();

        //MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
        //UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CHALLENGE);
        //MainUI mainUI = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CHALLENGE, "MainUI") as MainUI;
        //mainUI.show();
        //hide();

        requestType = 10;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP), this);
	}
	
	//点击付费提示框中的按钮 id = 0 购买， 1 取消//
	public void OnClickBuyBtn(int id){
		switch(id){
		case 0:		//确认购买//
			if(buyType == 1)		//付费进入//
			{
				requestType = 1;
				//付费进入//
				intoMazeType = 2;
				Debug.Log("WarpSpaceUIManager : " + id);
				//请求进入迷宫信息//
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAZE_INTO, curSelMazeId, 0),this);
			}
			else if(buyType == 2)	//重置信息//
			{
				requestType = 4;
				PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAZE_RESET, curSelMazeId),this);
			}
			break;
		case 1:		//取消购买//
			//关闭提示框//
			break;
		}
		BlackBg.SetActive(false);
		BuyTipObj.SetActive(false);
	}
	
	//显示当前关卡奖励信息//
	public void ShowRewardInfo(int param)
	{
		if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
		{
			return;
		}
		List<MazeprobabilityData> mpbd = MazeprobabilityData.getData(param);
		List<int> rewardDropId = new List<int>();
		//List<int> rewardSkillId = new List<int>();
		List<int> rewardSkillDebris = new List<int>();
		List<int> mazeBoss = new List<int>();
		if(mazeWish!=null)
		{
			string[] sss = mazeWish.Split('&');
			int curWishId = 0;
			for(int i =0;i<sss.Length;i++)
			{
				string[] ss = sss[i].Split('-');
				int mazeid = StringUtil.getInt(ss[0]);
				if(mazeid == param)
				{
					curWishId = StringUtil.getInt(ss[1]);
					this.curWishId = curWishId;
					rewardSkillDebris.Add(curWishId);
				}
			}
		}
		if(mazeBossDrop!=null)
		{
			string[] sss2 = mazeBossDrop.Split('&');
			for(int i = 0;i<sss2.Length;i++)
			{
				string[] ss = sss2[i].Split('-');
				int mazeid = StringUtil.getInt(ss[0]);
				if(mazeid == param)
				{
					for(int j = 1;j<ss.Length;j++)
					{
						if(StringUtil.getInt(ss[j])!=curWishId)
							mazeBoss.Add(StringUtil.getInt(ss[j]));
					}
				}
			}
			for(int i =0;i<mazeBoss.Count;i++)
			{
				rewardSkillDebris.Add(mazeBoss[i]);
			}
		}
		for(int i = 0;i<mpbd.Count;i++)
		{
			List<string> dropId_pro_isLook = mpbd[i].dropId_pro_isLook;
			for(int j = 0;j<dropId_pro_isLook.Count;j++)
			{
				string dpl = dropId_pro_isLook[j];
				string[] ss = dpl.Split('-');
				if(StringUtil.getInt(ss[2]) == 1)
				{
					rewardDropId.Add(StringUtil.getInt(ss[0]));
				}
			}
		}
		for(int i =0;i<rewardDropId.Count;i++)
		{
			List<MazeskilldropData> msdd = MazeskilldropData.GetAllAwardSkillId(rewardDropId[i]);
			for(int j = 0;j<msdd.Count;j++)
			{
				if(msdd[j].probability!=0)
				{
					switch(msdd[j].type)
					{
					//case 4 :				//主动技能//
					//	rewardSkillId.Add(StringUtil.getInt(msdd[j].skillID.Split(',')[0]));
					//	break;
					case 1 :				//技能碎片//
						int id = StringUtil.getInt(msdd[j].skillID.Split(',')[0]);
						if((!(id == curWishId))&&(!mazeBoss.Contains(id))&&(!rewardSkillDebris.Contains(id)))
						{
							rewardSkillDebris.Add(id);	
						}
						break;
					}
				}
			}
		}
		
		if(rewardInfoItem==null)
		{
			rewardInfoItem = GameObjectUtil.LoadResourcesPrefabs("UI-Maze/RewardInfoItem",3);
		}
		
		GameObjectUtil.destroyGameObjectAllChildrens(rewardInfoParet);
		mazeWishHash.Clear();
//		for(int i =0;i<rewardSkillId.Count;i++)//主动技能//
//		{
//
//			GameObject item = Instantiate(rewardInfoItem) as GameObject;
//			GameObjectUtil.gameObjectAttachToParent(item,rewardInfoParet);	
//			RewardInfoItem rii = item.GetComponent<RewardInfoItem>();
//			SkillData sd = SkillData.getData(rewardSkillId[i]);
//			rii.name.text = sd.name;
//			rii.sci2.setSimpleCardInfo(rewardSkillId[i],GameHelper.E_CardType.E_Skill);
//			rii.debris.gameObject.SetActive(false);
//			int spd = SkillPropertyData.getProperty(sd.type,sd.level,sd.star);
//			rii.des.text = sd.description + spd;
//			if(rewardSkillId[i] == curWishId)
//			{
//				rii.BG.gameObject.SetActive(false);
//				rii.wish.gameObject.SetActive(true);
//				rii.SetIsCanWish(false);
//			}
//			else
//			{
//				for(int j = 0;j<mazeBoss.Count;j++)
//				{
//					if(rewardSkillId[i] == mazeBoss[j])
//					{
//						rii.BG.gameObject.SetActive(false);
//						rii.wish.gameObject.SetActive(false);
//						rii.SetIsCanWish(true);
//					}
//				}
//			}
//			string ss = rewardSkillId[i]+"-4";
//			mazeWishHash.Add(ss,rri);
//		}
		
		for(int i =0;i<rewardSkillDebris.Count;i++)//技能碎片//
		{
			GameObject item = Instantiate(rewardInfoItem) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(item,rewardInfoParet);	
			RewardInfoItem rii = item.GetComponent<RewardInfoItem>();
			ItemsData itd = ItemsData.getData(rewardSkillDebris[i]);
			rii.nameLabel.text = itd.name;
			rii.des.text = itd.discription;
			switch(itd.goodztype)//1.skill 2.item 3.equip 4.card 5.passiveskill//
			{
			case 1:
				rii.sci2.setSimpleCardInfo(itd.goodsid,GameHelper.E_CardType.E_Skill);
				break;
			case 2:
				rii.sci2.setSimpleCardInfo(itd.goodsid,GameHelper.E_CardType.E_Item);
				break;
			case 3:
				rii.sci2.setSimpleCardInfo(itd.goodsid,GameHelper.E_CardType.E_Equip);
				break;
			case 4:
				rii.sci2.setSimpleCardInfo(itd.goodsid,GameHelper.E_CardType.E_Hero);
				break;
			case 5:
				rii.sci2.setSimpleCardInfo(itd.goodsid,GameHelper.E_CardType.E_PassiveSkill);
				break;
			}
			
			if(rewardSkillDebris[i] == curWishId)
			{
				rii.BG.gameObject.SetActive(false);
				rii.wish.gameObject.SetActive(true);
				rii.SetIsCanWish(false);
			}
			else
			{
				for(int j = 0;j<mazeBoss.Count;j++)
				{
					if(rewardSkillDebris[i] == mazeBoss[j])
					{
						rii.BG.gameObject.SetActive(false);
						rii.wish.gameObject.SetActive(false);
						rii.SetIsCanWish(true);
					}
				}
			}
			rii.wishBtn.target = gameObject;
			rii.wishBtn.functionName = "OnClickWish";
			rii.wishBtn.param = rewardSkillDebris[i];
			rii.debris.gameObject.SetActive(true);
			rii.SetId(rewardSkillDebris[i]);
			
			mazeWishHash.Add(rewardSkillDebris[i],rii);
		}
		
		rewardDropId.Clear();
		rewardSkillDebris.Clear();
		rewardInfoParet.GetComponent<UIGrid>().repositionNow = true;
		rewardInfoParet.transform.parent.GetComponent<UIDraggablePanel>().verticalScrollBar.value = 0;
		rewardInfo.SetActive(true);
	}
	
	public void OnClickWish(int param)
	{
		curWishId = param;
		RewardInfoItem ri = (RewardInfoItem)mazeWishHash[param];
		if(ri.GetIsCanWish())
		{
			requestType = 11;
			UIJson json = new UIJson();
			json.UIJsonForMazeBossReward(STATE.UI_MAZE_WISH,param,curSelMazeId);
			PlayerInfo.getInstance().sendRequest(json,this);
		}
		else
		{
			if(param == ri.GetId())
			{return;}
			ToastWindow.mInstance.showText(TextsData.getData(692).chinese);
		}
	}
	
	public void ChangeWishSpirt()
	{
		foreach(DictionaryEntry de in mazeWishHash)
		{
			if((int)de.Key == curWishId)
			{
				((RewardInfoItem)de.Value).wish.gameObject.SetActive(true);
				((RewardInfoItem)de.Value).SetIsCanWish(false);
			}
			else
			{
				((RewardInfoItem)de.Value).wish.gameObject.SetActive(false);
				if(((RewardInfoItem)de.Value).BG.gameObject.activeSelf)
				{
					((RewardInfoItem)de.Value).SetIsCanWish(false);
				}
				else
				{
					((RewardInfoItem)de.Value).SetIsCanWish(true);
				}
			}
		}
	}
	
	public void SetMazeWish(string mazeWish,string mazeBossDrop)
	{
		WarpSpaceUIManager.mazeWish = mazeWish;
		WarpSpaceUIManager.mazeBossDrop = mazeBossDrop;
	}
	
	public void OnClickBackMaze()
	{
		rewardInfo.SetActive(false);
		GameObjectUtil.destroyGameObjectAllChildrens(rewardInfoParet);
	}
	
	public void OnClickShowTips()
	{
	 	ToastWindow.mInstance.showText(TextsData.getData(674).chinese,this);
	}
	
	public void warnningCancel(){}
	
	public void warnningSure(){}
	
	public void receiveResponse (string json)
	{
		
		Debug.Log("warpSpaceUIManager : " + json);
		if(json != null){
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;


            switch (requestType)
            {
                case 1:			//进入迷宫//
                    receiveData = true;
                    mrj = JsonMapper.ToObject<MazeResultJson>(json);
                    errorCode = mrj.errorCode;
                    if (errorCode == 0)
                    {
                        PlayerInfo.getInstance().curMazeId = mrj.td;
                        PlayerInfo.getInstance().curPosId = mrj.state;
                        PlayerInfo.getInstance().curMazeNum = mrj.num;
                        PlayerInfo.getInstance().costPowerMark = mrj.t;
                    }
                    break;
                case 2:			//额外购买次数//
                    mrj = JsonMapper.ToObject<MazeResultJson>(json);
                    errorCode = mrj.errorCode;
                    receiveData = true;
                    break;
                case 3:			//获取扭曲空间数据//
                    mrj = JsonMapper.ToObject<MazeResultJson>(json);
                    errorCode = mrj.errorCode;
                    if (errorCode == 0)
                    {
                        PlayerInfo.getInstance().curOpenMazeId.Clear();
                        PlayerInfo.getInstance().curOpenMazeId = mrj.s;
                        PlayerInfo.getInstance().curIntoMaze = mrj.maze;
                        curSelMazeId = mrj.mId;
                        cdTime = mrj.cdtime;
                    }
                    receiveData = true;
                    break;
                case 4:			//重置数据//
                    MazeClearResultJson mcrj = JsonMapper.ToObject<MazeClearResultJson>(json);
                    errorCode = mcrj.errorCode;
                    if (errorCode == 0)
                    {
                        receiveStr = mcrj.maze;
                        //int oldCrystal = PlayerInfo.getInstance().player.crystal;
                        PlayerInfo.getInstance().player.crystal = mcrj.crystal;
                        //useCrystal = oldCrystal - mcrj.crystal;
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
				case 11:
					WishResultJson wrj = JsonMapper.ToObject<WishResultJson>(json);
					errorCode = wrj.errorCode;
					WarpSpaceUIManager.mazeWish = wrj.mazeWish;
					WarpSpaceUIManager.mazeBossDrop = wrj.mazeBossDrop;
					receiveData = true;
					break;
            }
			
			
		}	
	}
}
