using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainUI : MonoBehaviour , ProcessResponse{
	public GameObject BasicLayer;		//要移动的基础界面//
	public List<GameObject> BtnPosList;	//基础界面各个按钮移动的位置//
	public GameObject[] unLockObj;		//显示解锁条件//
	public GameObject[] models;			//各个模块//
	
	private Transform _myTransform;
	
	float ScaleTime = 0.3f;				//基础界面放大的时间//
	//float MoveTime = 0.5f;				//基础界面移动的时间//
	float StayTime = 0.5f;				//动画播放完等待跳转的时间//
	float ScaleNum = 1.5f;				//基础界面放大的倍数//
	float frameCount = 0;
	bool StartFrameCount = false;		//是否开始等待跳转的计时器//
	int curSelectBtn = -1;
	//1 世界boss， 2 异世界, 3 竞技场, 4 灵界, 5 扭曲空间//
	private int requestType;
	private int errorCode;
	private bool receiveData;
	
	//进入灵界界面获得的数据//
	int nullPackNum;					//背包空余格子数//
	int curNpcId;						//当前激活的npc的id//



    int mid; //当前玩家激活的领奖id编号//

    int mnum; //当前玩家的冥想次数//
	
	//进入竞技场界面信息--玩家信息//
	string playerArenaData;
	//requestType 3 排位赛界面pk对手信息, 2 异世界中副本的信息（id-mark-time）//
	List<string> strData;

    RankResultJson rrj;
	
	MazeResultJson mrj;
	
	int selMazeId;
	//总的挑战次数//
	int totalDekaronNum;
	//总的符文值//
	int totalPVPReward;
	//request == 3 竞技场的冷却时间 ， 2 活动副本， 5 扭曲空间 pk剩余时间//
	int pkTime;

    private MapResultJson mapRJ;

    public GameObject fogs;

    int num;
	
	void Awake()
	{

		_myTransform = transform ;
		init ();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(StartFrameCount){
			frameCount+= Time.deltaTime;
			if(frameCount >= StayTime){
				StartFrameCount = false;
				frameCount=0;
				ChangeWorld(curSelectBtn + 1);
			}
		}
		
		if(receiveData){
			receiveData = false;
			if(errorCode == -3)
				return;
            switch (requestType)
            {
                case 1:		//世界boss//
                    //				MissionUI2.mInstance.show();
                    //				MissionUI.mInstance.show();
                    //				HeadUI.mInstance.hide();
                    //				hide();
                    break;
                case 2:
                    PlayerInfo.getInstance().bBackActivity = true;  //返回活动界面//

                    //打开活动副本界面//
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY);
                    ActiveWroldUIManager activeCopy = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACTIVECOPY,
                        "ActiveWroldUIManager") as ActiveWroldUIManager;
                    activeCopy.setData(strData,num);
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_ActiveCopy))
                    {
                        UISceneDialogPanel.mInstance.showDialogID(22);
                    }
                    break;
                case 3:		//竞技场//
                    //				ArenaUIManager.mInstance.SetData(0, playerArenaData, strData, totalPVPReward, totalDekaronNum, pkTime);

                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA);
                    ArenaUIManager arena = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ARENA,
                        "ArenaUIManager") as ArenaUIManager;
                    arena.SetData(0, playerArenaData, strData, totalPVPReward, totalDekaronNum, pkTime,rrj.cardIds,1);
                    break;
                case 4:		//灵界//
                    //				SpriteWroldUIManager.mInstance.SetData(nullPackNum, curNpcId);

                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD);
                    SpriteWroldUIManager spriteWorld = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SPRITEWORLD, "SpriteWroldUIManager") as SpriteWroldUIManager;
                    spriteWorld.SetData(nullPackNum, curNpcId, mid, mnum);

                    HeadUI.mInstance.hide();
                    break;

                case 5:		//扭曲空间//	
                    PlayerInfo.getInstance().bBackActivity = true;  //返回活动界面//

                    //打开扭曲空间界面//
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE);
                    WarpSpaceUIManager warpSpace = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_WARPSPACE, "WarpSpaceUIManager") as WarpSpaceUIManager;
                    warpSpace.SetData(PlayerInfo.getInstance().curOpenMazeId, PlayerInfo.getInstance().curIntoMaze, selMazeId, pkTime);
					warpSpace.SetMazeWish(mrj.mazeWish,mrj.mazeBossDrop);
                    HeadUI.mInstance.hide();
                    if (GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_WarpSpace))
                    {
                        UISceneDialogPanel.mInstance.showDialogID(20);
                    }
                    break;
                case 6:
                    UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
                    MissionUI2 mission2 = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
                        "MissionUI2") as MissionUI2;
                    MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP, "MissionUI") as MissionUI;
                    mission.mrj = mapRJ;
                    mission2.show();
                    mission.show();
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
					{
						GuideUI18_Skill.mInstance.showStep(4);	
					}
                    break;
            }
			hide();
			
		}
	}
	
	//判断是否开启模块 模块id 7 - 11//
	public void ShowBattleUnLockLabel(){
		for(int i =0;i< unLockObj.Length;i++){
			Debug.Log("i ================= " + i);
			int unlockFormId = models[i].transform.FindChild("SelectSpr").GetComponent<UIButtonMessage>().param;
			
			int mark = PlayerInfo.getInstance().getUnLockData(unlockFormId);
			
			if(mark == 0){		//未解锁//
				string des = UnlockData.getData(unlockFormId).description;
				unLockObj[i].gameObject.SetActive(true);
				UILabel label = unLockObj[i].transform.FindChild("Label").GetComponent<UILabel>();
				label.text = des;
			}
			else if(mark == 1){	//已解锁//
				unLockObj[i].gameObject.SetActive(false);
			}
		}
	}
	
	//id 在解锁表中的id//
	public void ManagerBtnShow(int id){
		for(int i = 0;i < models.Length;i++){
			GameObject light = models[i].transform.FindChild("SelectSpr").gameObject;
			GameObject black = models[i].transform.FindChild("Background").gameObject;
			UIButtonMessage ubm = models[i].GetComponent<UIButtonMessage>();
			if(ubm.param == id){
				light.SetActive(true);
				black.SetActive(false);
			}
			else {
				light.SetActive(false);
				black.SetActive(true);
			}
		}
	}
	
	public void init ()
	{
//		base.init();
		CleanData();
		strData = new List<string>();
	}
	
	public void show()
	{
//		base.show();	
		CleanData();
		ShowBattleUnLockLabel();
//		ManagerBtnShow(-1);
	}
	
	public void hide()
	{
		CleanData();
//		base.hide();
		gc();
		
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
	}
	
	private void gc()
	{
		Resources.UnloadUnusedAssets();
	}
	
	//战斗界面： 7.世界boss    8 （异世界）   9.竞技场    10. 灵界   11扭曲空间, 在表中的id为7-11//
	public void onClickBtn(int param)
	{
        TweenPosition tw0 = fogs.GetComponent<TweenPosition>();
        if (tw0.enabled == true)
        {
            return;
        }
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		int unlockMark = PlayerInfo.getInstance().getUnLockData(param);
//		ManagerBtnShow(param);
		
//		if(param == 1 || param == 3 || param == 4 || param == 5){			//当前只开放基础世界, 灵界和扭曲空间//
		if(unlockMark == 1){			//以开放//
			curSelectBtn = param - 7;
//			BasicLayerPlayAnim(curSelectBtn);
			ChangeWorld(curSelectBtn + 1);
		}
		else {							//未解锁//
//			UIToastTipManager.mInstance.show();
			//string str = TextsData.getData(142).chinese;
			string s2 = UnlockData.getData(param).description;
//			string ss = str + "\r\n" + s2;
			string ss = s2;
			ToastWindow.mInstance.showText(ss);
		}
	}

	public void OnBackBtn(){
//        //播放音效//
//        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
////		MainMenuManager.mInstance.SetData(STATE.ENTER_MAINMENU_BACK);
//        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
//        MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU, "MainMenuManager") as MainMenuManager;
//        if(main!= null)
//        {
//            main.SetData(STATE.ENTER_MAINMENU_BACK);
//        }
//        hide();

        requestType = 6;
        PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP), this);
	}
	
	public void BasicLayerPlayAnim(int posId){
		Vector3 desPos = BtnPosList[posId].transform.position;
		Vector3 scale = new Vector3(ScaleNum, ScaleNum, 1);
		iTween.ScaleTo(BasicLayer,iTween.Hash("scale",scale ,"time",ScaleTime, "easetype",iTween.EaseType.linear));
		iTween.MoveTo(BasicLayer,iTween.Hash("position", desPos,"time", ScaleTime, "oncomplete", "BasicLayerMoveCallBack", "oncompletetarget", _myTransform.gameObject, "easetype",iTween.EaseType.linear));
	}
	
	public void BasicLayerMoveCallBack(){
//		ChangeWorld(curSelectBtn + 1);
		StartFrameCount = true;
	}
	
	public void ChangeWorld(int param){
		switch(param)
		{
		case 1:/**世界boss**/
//			requestType=1;
//			PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_MAP),this);
			break;
		case 2:/**异世界**/
			openAcitveCopy();
			return;
		case 3:/**竞技场**/
			openPVP();
			break;
		case 4:/**灵界**/
			openSpirit();
			break;
		case 5:/**扭曲空间**/
			openWarpSpace();
			break;
		}
		
		//关闭主菜单选项卡//
//		MainMenuManager.mInstance.hide();
		//隐藏主城//
		if(UISceneStateControl.mInstace.stateHash.ContainsKey(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU))
		{
			UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		}
		
	}
	
	public void openAcitveCopy()
	{
		requestType = 2;
		//发送进入异世界（活动副本）请求信息//
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_EVENT),this);
	}
	
	public void openPVP()
	{
		requestType = 3;
		//发送进入竞技场请求信息//
		PlayerInfo.getInstance().sendRequest(new RankJson(0),this);
	}
	
	public void openSpirit()
	{
		requestType = 4;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_SPRITEWROLD_INTO),this);
	}
	
	public void openWarpSpace()
	{
		requestType = 5;
		PlayerInfo.getInstance().sendRequest(new UIJson(STATE.UI_WARPSPACE),this);
	}
	
//	public void OnClickBack(){
//	
//		MainMenuManager.mInstance.show();
//		hide();
//	}
	
	public void CleanData(){
		frameCount = 0;
		StartFrameCount = false;
		BasicLayer.transform.localScale	= Vector3.one;
		BasicLayer.transform.localPosition = Vector3.zero;
		curSelectBtn= -1;
		
	}
	
	public void receiveResponse (string json)
	{
		if(json!= null){
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			Debug.Log("MainUI ： " + json);
            switch (requestType)
            {
                case 1:
                    //				MapResultJson mj=JsonMapper.ToObject<MapResultJson>(json);
                    //				MissionUI.mInstance.mrj=mj;
                    //				receiveData=true;
                    break;
                case 2:		//活动副本（异世界）//
                    //				strData.Clear();
                    EventResultJson erj = JsonMapper.ToObject<EventResultJson>(json);
                    errorCode = erj.errorCode;
                    if (errorCode == 0)
                    {
                        strData = erj.s;

                        num = erj.num;
                    }
                    receiveData = true;
                    break;
                case 3:		//竞技场//
                    //				strData.Clear();
                    RankResultJson rrj = JsonMapper.ToObject<RankResultJson>(json);
                    errorCode = rrj.errorCode;
                    if (errorCode == 0)
                    {
                        this.rrj = rrj;
                        playerArenaData = rrj.s;
                        strData = rrj.ss;
                        totalDekaronNum = rrj.sPknum;
                        totalPVPReward = rrj.sAward;
                        pkTime = (int)(rrj.cdtime);
                    }
                    receiveData = true;
                    break;
                case 4:			//冥想//
                    ImaginationResultJson irj = JsonMapper.ToObject<ImaginationResultJson>(json);
                    errorCode = irj.errorCode;
                    if (errorCode == 0)
                    {
                        curNpcId = irj.id;
                        nullPackNum = irj.i;

                        mid = irj.mid;
                        mnum = irj.mnum;
                        PlayerInfo.getInstance().player.gold = irj.g;
                    }
                    receiveData = true;
                    break;
                case 5:			//扭曲空间//

                    mrj = JsonMapper.ToObject<MazeResultJson>(json);
                    errorCode = mrj.errorCode;
                    if (errorCode == 0)
                    {
                        PlayerInfo.getInstance().curOpenMazeId.Clear();
                        PlayerInfo.getInstance().curOpenMazeId = mrj.s;
                        PlayerInfo.getInstance().curIntoMaze = mrj.maze;
                        selMazeId = mrj.mId;
                        pkTime = mrj.cdtime;
                    }
                    receiveData = true;
                    break;
                case 6:
                    MapResultJson mj = JsonMapper.ToObject<MapResultJson>(json);
                    errorCode = mj.errorCode;
                    if (errorCode == 0)
                    {
                        mapRJ = mj;
                    }
                    receiveData = true;
                    break;
            }
		}
	}
	
}
