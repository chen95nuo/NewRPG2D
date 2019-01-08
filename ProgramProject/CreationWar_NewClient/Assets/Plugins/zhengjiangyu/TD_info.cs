using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TD_info : MonoBehaviour {
	public static bool isTDStartGame = false;
	
	
	// Use this for initialization

	static bool insTD = false;
	public GameObject obj;
	public static bool isStart = false;
	public static bool isNotice = false;
	public static bool isLogin = false;
	public static bool isCreatRole = false;
	public static bool isTraining =false;
//    void Start () {
//#if UNITY_IOS
//        if(!insTD)
//        {
//            insTD = true;
//            GameObject.Instantiate(obj);
//        }

//        TalkingDataGA.OnStart( "3DF063BEDBD648B74D7C7FFBE11F59A7", TableRead.strPageName ); 
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo001"));
//        isNotice = true;
//#endif
//    }
	
//    // Update is called once per frame
//    void Update () {
		
//    }
	
//    /// <summary>
//    /// Sets the TD user init.
//    /// //设置唯一帐户  用户的帐号+角色id
//    ///	accountId
//    ///	//设置帐户的显性名 用户名
//    ///	accountName
//    ///	//设置级别
//    ///	level
//    ///	//设置区服
//    ///	gameServerName
//    /// 字符串拼接
//    /// String userInfo = accountId+";"+accountName+";"+level+";"+gameServerName
//    /// 
//    /// </summary>
//    /// <param name='userInfo'>
//    /// User info.
//    /// </param>
//    public static void setTDUserInit(string userInfo){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setUserInfo",userInfo);
//        #elif UNITY_IOS
//        string[] info = new string[4];
//        info = userInfo.Split(';');
//        int Level = int.Parse(info[2]);
//        TDGAAccount account = TDGAAccount.SetAccount(info[0]);
//        account.SetAccountType(AccountType.REGISTERED);
//        account.SetAccountName(info[1]);
//        account.SetLevel(Level);
//        account.SetGameServer(info[3]);
//        // 点击开始游戏
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo002"));
//        #endif
//    }
	
//    /// <summary>
//    /// Sets up user level.
//    /// 	//设置唯一帐户  用户的帐号+角色id
//    ///	accountId	
//    ///	//用户级别  升级之前
//    ///	oldLevel
//    ///	//用户区服
//    ///	gameServerName
//    ///	//设置新级别 升级之后
//    ///	newLevel
//    /// 字符串拼接
//    /// String  upLevelInfo = accountId+";"+oldLevel+";"+gameServerName+";"+newLevel
//    /// </summary>
//    /// <param name='upLevelInfo'>
//    /// Up level info.
//    /// </param>
//    public static void setUpUserLevel(string upLevelInfo){
//        #if UNITY_ANDROID		
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setUserUpLevel",upLevelInfo);
//        #elif UNITY_IOS
//        string[] info = new string[4];
//        info = upLevelInfo.Split(';');
//        int oldLevel = int.Parse(info[1]);
//        int newLevel = int.Parse(info[3]);
//        TDGAAccount account = TDGAAccount.SetAccount(info[0]);
//        account.SetAccountType(AccountType.ANONYMOUS);
//        account.SetLevel(oldLevel);
//        account.SetGameServer(info[2]);
//        account.SetLevel(newLevel);
//        #endif


//    }
	
//    /// <summary>
//    /// Sets the in game screen.
//    /// 成功进入游戏场景
//    /// </summary>
//    public static void setInGameScreen(){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setInGameScreen");
//        #elif UNITY_IOS
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo002"));
//        #endif

//    }
	
//    /// <summary>
//    /// Sets the in game instance.
//    /// 进入副本 
//    /// mapName 副本名字
//    /// </summary>
//    /// <param name='mapName'>
//    /// Map name. 
//    /// </param>
//    public static void setInGameInstance(string mapName){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setInGameInstance", mapName);	
//        #elif UNITY_IOS
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo003") + mapName);
//        #endif

//    }
	
//    /// <summary>
//    /// Sets the over instance.
//    /// 完成副本 
//    /// mapName 副本名字
//    /// </summary>
//    /// <param name='mapName'>
//    /// Map name.
//    /// </param>
//    public static void setOverInstance(string mapName){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setOverInstance", mapName);	
//        #elif UNITY_IOS
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo003") + mapName);
//        #endif

//    }
	
//    /// <summary>
//    /// Sets the leave instance.
//    /// 强制离开副本 
//    /// mapName 副本名字
//    /// </summary>
//    /// <param name='mapName'>
//    /// Map name.
//    /// </param>
//    public static void setLeaveInstance(string mapName){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setLeaveInstance", mapName);	
//        #elif UNITY_IOS
//        TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo003") + mapName, StaticLoc.Loc.Get("tdinfo004"));
//        #endif

//    }
//    /// <summary>
//    /// Sets the start game.
//    /// 启动游戏统计
//    /// </summary>
//    public static void setStartGame(){ 
//        #if UNITY_ANDROID
//        isTDStartGame = true;
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setgameStart");
//        #elif UNITY_IOS
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo005"));
//        isStart = true;
//        #endif
	
//    }
	
//    /// <summary>
//    /// Starts the succcess.
//    /// 启动统计成功
//    /// </summary>
//    public static void startSucccess(){
//        #if UNITY_ANDROID
//        isTDStartGame = false;
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("startSucccess");	
//        #elif UNITY_IOS
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo005"));
//        isStart = false;
//        #endif
//    }
//    /// <summary>
//    /// Starts the fail.
//    /// 游戏启动失败
//    /// </summary>
//    public static void startFail(){//读条失败调用
//        #if UNITY_ANDROID
//        isTDStartGame = false;
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("startFail");	
//        #elif UNITY_IOS
//        TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo005"), StaticLoc.Loc.Get("tdinfo006"));
//        isStart = false;
//        #endif

//    }
//    /// <summary>
//    /// Notices the success.
//    /// 关闭通知栏
//    /// </summary>
//    public static void NoticeSuccess(){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("NoticeSuccess");	
//        #elif UNITY_IOS
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo001"));
//        isNotice = false;
//        #endif
//    }
//    /// <summary>
//    /// Sets the login.
//    /// 统计进入游戏按钮
//    /// </summary>
//    public static void setLogin(){
//        #if UNITY_ANDROID
//        BtnManager.isTDlogin = true;
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setLogin");	
//        #elif UNITY_IOS
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo007"));
//        isLogin = true;
//        #endif

//    }
//    /// <summary>
//    /// Logins the success.
//    /// 成功进入游戏
//    /// </summary>
//    public static void loginSuccess(){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("loginSuccess");	
//        #elif UNITY_IOS
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo007"));
//        isLogin = false;
//        #endif
//    }
//    /// <summary>
//    /// Logins the fail.
//    /// 进入游戏失败
//    /// </summary>
//    public static void loginFail(){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("loginFail");	
//        #elif UNITY_IOS
//        TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo007"), StaticLoc.Loc.Get("tdinfo006"));
//        isLogin = false;
//        #endif
//    }
//    /// <summary>
//    /// Sets the select server.
//    /// 统计选择区服
//    /// </summary>
//    /// <param name="serverName">Server name.</param>
//    /// serverName 区服名字
//    public static void setSelectServer(string serverName){
//        #if UNITY_ANDROID
//        BtnManager.isTDselet = true;
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setSelectServer", serverName);	
//        #elif UNITY_IOS
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo008") + serverName);
//        #endif
//    }
//    /// <summary>
//    /// Selects the success.
//    /// 成功进入区服
//    /// </summary>
//    /// <param name="serverName">Server name.</param>
//    /// serverName区服名称
//    public static void selectSuccess(string serverName){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("selectSuccess", serverName);	
//        #elif UNITY_IOS
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo008") + serverName);
//        #endif

//    }
//    /// <summary>
//    /// Selects the fail.
//    /// 进入区服失败
//    /// </summary>
//    /// <param name="serverName">Server name.</param>
//    /// serverName区服名称
//    public static void selectFail(string serverName){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("selectFail", serverName);	
//        #elif UNITY_IOS
//        TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo008") + serverName, StaticLoc.Loc.Get("tdinfo006"));
//        #endif

//    }
//    /// <summary>
//    /// Sets the creat role.
//    /// 创建角色
//    /// </summary>
//    public static void setCreatRole(){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setCreatRole");	
//        #elif UNITY_IOS
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo009"));
//        isCreatRole = true;
//        #endif
//    }
//    /// <summary>
//    /// Creats the success.
//    /// 成功创建角色
//    /// </summary>
//    /// <param name="roleInfo">Role info.</param>
//    /// 新建角色信息  roleInfo="所选职业"+"所在区服";
//    public static void creatSuccess(string roleInfo){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("creatSuccess",roleInfo);	
//        #elif UNITY_IOS 
//        string[] roleInfos = roleInfo.Split(';');
//        string profession = roleInfos [0];
//        string serverName = roleInfos [1];
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo009"));
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo010")+","+profession+","+serverName);
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo010")+","+profession+","+serverName);
//        isCreatRole = false;
//        #endif

//    }
//    /// <summary>
//    /// Creats the fail.
//    /// 创建角色失败
//    /// </summary>
//    /// <param name="failReason">Fail reason.</param>
//    /// failReason 失败原因
//    public static void creatFail(string failReason){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("creatFail",failReason);
//        #elif UNITY_IOS 
//        TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo009"), failReason);
//        isCreatRole = false;
//        #endif

//    }
//    /// <summary>
//    /// Sets the task.
//    /// 接受任务
//    /// </summary>
//    /// <param name="taskName">Task name.</param>
//    /// taskName任务名称
//    public static void setTask(string taskName){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setTask",taskName);	
//        #elif UNITY_IOS 
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo011") + taskName);
//        #endif
//    }
	
//    /// <summary>
//    /// Tasks the success.
//    /// 任务成功
//    /// </summary>
//    /// <param name="taskName">Task name.</param>
//    /// taskname任务名称
//    public static void taskSuccess(string taskName){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("taskSuccess",taskName);
//        #elif UNITY_IOS 
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo011") + taskName);
//        #endif
//    }
//    /// <summary>
//    /// Tasks the fail.
//    /// 任务失败
//    /// </summary>
//    /// <param name="taskName">Task name.</param>
//    /// taskName任务名称
//    public static void taskFail(string taskName){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("taskFail",taskName);	
//        #elif UNITY_IOS 
//        TDGAMission.OnFailed(StaticLoc.Loc.Get("tdinfo011") + taskName, StaticLoc.Loc.Get("tdinfo012"));
//        #endif

//    }
//    /// <summary>
//    /// Sets the skip screen.
//    /// 训练场景跳过统计
//    /// </summary>
//    public static void setSkipScreen(){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setSkipScreen");	
//        #elif UNITY_IOS 
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo013"));
//        #endif

//    }
	
//    /// <summary>
//    /// Skips the success.
//    /// 成功跳过训练场景
//    /// </summary>
//    public static void skipSuccess(){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("skipSuccess");	
//        #elif UNITY_IOS 
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo013"));
//        if (isTraining) {
//            successTraining();
//        }
//        #endif

//    }
	
//    /// <summary>
//    /// Trainings the screeen.
//    /// 训练场景
//    /// </summary>
//    public static void trainingScreeen() {
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("trainingScreeen");	
//        #elif UNITY_IOS 
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo014"));
//        isTraining = true;
//        #endif

//    }
	
//    /// <summary>
//    /// Successes the training.
//    /// 通过训练场景
//    /// </summary>
//    public static void successTraining() {
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("successTraining");	
//        #elif UNITY_IOS 
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo014"));
//        isTraining = false;
//        #endif

//    }
	
//    /// <summary>
//    /// Changes the equi.
//    /// 换装统计
//    /// </summary>
//    /// <param name="profession">职业.</param>
//    public static void changeEqui(string profession) {
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("changeEqui",profession);	
//        #elif UNITY_IOS 
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo015")+profession);
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo015")+profession);
//        #endif
//    }
	
//    /// <summary>
//    /// Panels the statistics.
//    /// 面板统计
//    /// </summary>
//    /// <param name="panelName">面板名称</param>
//    public static void panelStatistics(string panelName) {
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("panelStatistics",panelName);	
//        #elif UNITY_IOS 
//        TDGAMission.OnBegin(StaticLoc.Loc.Get("tdinfo016")+panelName);
//        TDGAMission.OnCompleted(StaticLoc.Loc.Get("tdinfo016")+panelName);
//        #endif 

//    }
	
	
//    /// <summary>
//    /// Guides the player.
//    /// 新手引导
//    /// </summary>
//    /// <param name="guideStep">引导步骤</param>
//    public static void guidePlayer(string guideStep) {
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("guidePlayer",guideStep);	
//        #elif UNITY_IOS 
//        Dictionary<string, object> guide = new Dictionary<string, object>();
//        guide.Add(StaticLoc.Loc.Get("tdinfo017"), guideStep);
//        TalkingDataGA.OnEvent(StaticLoc.Loc.Get("tdinfo018"), guide);
//        #endif 

//    }
//    /// <summary>
//    /// Sets the pay request.
//    /// 发起支付请求
//    /// </summary>
//    /// payInfo = orderId+";"+currencyAmount+";"+currencyType+";"+paymentType;
//    /// orderId 订单ID，最多64 个字符。用于唯一标识一次交易。
//    /// currencyAmount 充值的金额
//    /// currencyType ISO 4217 中规范的3 位字母代码标记货币类型 人民币CNY；美元USD；欧元EUR
//    /// paymentType 支付的途径，最多16 个字符
//    /// <param name="payInfo">Pay info.</param>
//    public static void setPayRequest(string payInfo) {
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setPayRequest",payInfo);	
//        #elif UNITY_IOS 
//        string[] payInfos = payInfo.Split(';');
//        // 订单ID，最多64 个字符。用于唯一标识一次交易。
//        string orderId = payInfos[0];
//        // 充值包ID，最多32 个字符。唯一标识一类充值包。
//        string iapId = "";
//        // 充值的金额
//        double currencyAmount = Double.Parse(payInfos[1]);
//        // ISO 4217 中规范的3 位字母代码标记货币类型 人民币CNY；美元USD；欧元EUR
//        string currencyType = "CNY";
//        // 虚拟币金额
//        double virtualCurrencyAmount =Double.Parse(payInfos[2]);
//        // 支付的途径，最多16 个字符。
//        string paymentType = payInfos[3];
		
//        int price = (int) currencyAmount;
		
//        switch (price) {
//        case 6:
//            iapId = StaticLoc.Loc.Get("tdinfo019");

//            break;
//        case 30:
//            iapId = StaticLoc.Loc.Get("tdinfo020");

//            break;
//        case 418:
//            iapId = StaticLoc.Loc.Get("tdinfo021");

//            break;
//        case 998:
//            iapId = StaticLoc.Loc.Get("tdinfo022");

//            break;
//        case 25:
//            iapId = StaticLoc.Loc.Get("tdinfo023");

//            break;
//        case 60:
//            iapId = StaticLoc.Loc.Get("tdinfo029");
//            break;
//        default:
//            break;
//        }
//        TDGAVirtualCurrency.OnChargeRequest(orderId, iapId, currencyAmount,currencyType, virtualCurrencyAmount, paymentType);
//        #endif 
	

//    }
//    /// <summary>
//    /// Paies the success.
//    /// 支付成功
//    /// </summary>
//    /// orderId 定义的订单号
//    /// <param name="orderId">Order identifier.</param>
//    public static void paySuccess(string orderId) {
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("paySuccess",orderId);	
//        #elif UNITY_IOS 
//        TDGAVirtualCurrency.OnChargeSuccess(orderId);
//        #endif 

//    }

//    /// <summary>
//    /// Sets the gift currency.
//    /// 追踪系统赠送玩家的血石
//    /// 该方法需要传入获得的血石的数量、赠送血石的原因（例如：任务、副本、礼包等）
//    /// giftInfo = 血石数量 + ";" + 赠送血石原因；
//    /// </summary>
//    /// <param name='giftInfo'>
//    /// Gift info.
//    /// </param>
//    public static void setGiftCurrency(string giftInfo){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setGiftCurrency", giftInfo);	
//        #elif UNITY_IOS 
//        string[] info = new String[2];
//        info = giftInfo.Split(';');
//        double virtualCurrencyAmount = Double.Parse(info[0]);
//        TDGAVirtualCurrency.OnReward(virtualCurrencyAmount, info[1]);
//        #endif
//    }
//    /// <summary>
//    /// Sets the user purchase.
//    /// 购买道具
//    /// </summary>
//    /// purchaseInfo =道具编号+;+购买数量+;+道具单价
//    /// <param name="purchaseInfo">Purchase info.</param>
//    public static void setUserPurchase(string purchaseInfo){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setUserPurchase", purchaseInfo);	
//        #elif UNITY_IOS 
//        string[] purchaseInfos = purchaseInfo.Split(';');
//        //某个消费点的编号
//        string item = purchaseInfos[0];
//        //消费数量
//        int itemNumber = int.Parse(purchaseInfos[1]);
//        //虚拟币单价
//        double priceInVirtualCurrency = Double.Parse(purchaseInfos[2]);
//        TDGAItem.OnPurchase(item, itemNumber, priceInVirtualCurrency);
//        #endif

//    }
//    /// <summary>
//    /// Sets the on property use.
//    /// 道具消耗
//    /// </summary>
//    /// propUse =道具编号+;+消耗数量
//    /// <param name="propUse">Property use.</param>
//    public static void setOnPropUse(string propUse){
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        jo.Call("setOnPropUse", propUse);	
//        #elif UNITY_IOS 
//        string[] propUses = propUse.Split(';');
//        //某个消费点的编号
//        string item = propUses[0];
//        //消费数量
//        int itemNumber = int.Parse(propUses[1]);
//        TDGAItem.OnUse(item, itemNumber);
//        #endif
//    }

//    /// <summary>
//    /// 设备剔除统计
//    /// </summary>
//    /// <param name="guideStep">Guide step.</param>
//    public static void deviceReject(string phoneType, string phoneID, string chanel) {
//        #if UNITY_ANDROID
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
//        //jo.Call("guidePlayer",guideStep);	
//        #elif UNITY_IOS 
//        Dictionary<string, object> guide = new Dictionary<string, object>();
//        guide.Add(StaticLoc.Loc.Get("tdinfo026"), phoneType);
//        guide.Add(StaticLoc.Loc.Get("tdinfo027"), phoneID);
//        guide.Add(StaticLoc.Loc.Get("tdinfo028"), chanel);
//        TalkingDataGA.OnEvent(StaticLoc.Loc.Get("tdinfo025"), guide);
//        #endif 
		
//    }

}
