using UnityEngine;
using System.Collections;

public class IDKData : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/// <summary>
	/// IDKs the set user register.
	/// 玩家注册
	/// </summary>
	/// <param name="userId">User identifier.</param>
	public static void IDKSetUserRegister(string userId){
	#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetUserRegister",userId);
	#endif
	}
	/// <summary>
	/// IDKs the set creat role.
	/// 创建角色
	/// </summary>
	/// <param name="userInfo">User info.</param>
	public static void IDKSetCreatRole(string userInfo){
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetCreatRole",userInfo);
		#endif
	}
	/// <summary>
	/// IDKs the set account login.
	/// 账号登陆
	/// </summary>
	/// <param name="userid">Userid.</param>
	public static void IDKSetAccountLogin(string userid){
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetAccountLogin",userid);
		#endif
	}

	/// <summary>
	/// IDKs the set role login.
	/// 角色登陆
	/// </summary>
	/// <param name="roleInfo">Role info.</param>
	public static void IDKSetRoleLogin(string roleInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleLogin",roleInfo);
		#endif
	}
	/// <summary>
	/// IDKs the set role lev up.
	/// 角色升级
	/// </summary>
	/// <param name="levInfo">Lev info.</param>
	public void IDKSetRoleLevUp(string levInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleLevUp",levInfo);
		#endif
	}
	/// <summary>
	/// IDKs the set user pay.
	/// 玩家充值
	/// </summary>
	/// <param name="payInfo">Pay info.</param>
	public void IDKSetUserPay(string payInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetUserPay",payInfo);
		#endif
	}
	/// <summary>
	/// IDKs the set role get property.
	/// 角色购买道具
	/// </summary>
	/// <param name="propInfo">Property info.</param>
	public void IDKSetRoleGetProp(string propInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleGetProp",propInfo);
		#endif
	}

	/// <summary>
	/// IDKs the set role use property.
	/// 角色消耗道具
	/// </summary>
	/// <param name="propInfo">Property info.</param>
	public void IDKSetRoleUseProp(string propInfo){
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleUseProp",propInfo);
		#endif
	}
	
	/// <summary>
	/// IDKs the set start task.
	/// 角色开始任务
	/// </summary>
	/// <param name="taskInfo">Task info.</param>
	public void IDKSetStartTask(string taskInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetStartTask",taskInfo);
		#endif
	}
	
	/// <summary>
	/// IDKs the set over task.
	/// 角色结束任务
	/// </summary>
	/// <param name="taskInfo">Task info.</param>
	public void IDKSetOverTask(string taskInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetOverTask",taskInfo);
		#endif
	}
	
	/// <summary>
	/// IDKs the set role coin cost.
	/// 虚拟币消耗
	/// </summary>
	/// <param name="costCoinInfo">Cost coin info.</param>
	public void IDKSetRoleCoinCost(string costCoinInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleCoinCost",costCoinInfo);
		#endif
	}
	
	/// <summary>
	/// IDKs the set role get coin.
	/// 虚拟币获得
	/// </summary>
	/// <param name="getCoinInfo">Get coin info.</param>
	public void IDKSetRoleGetCoin(string getCoinInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleGetCoin",getCoinInfo);
		#endif
	}
	
	/// <summary>
	/// IDKs the set role start map.
	/// 关卡开始
	/// </summary>
	/// <param name="startMapInfo">Start map info.</param>
	public void IDKSetRoleStartMap(string startMapInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleStartMap",startMapInfo);
		#endif
	}
	/// <summary>
	/// IDKs the set role end map.
	/// 关卡结束
	/// </summary>
	/// <param name="endMapInfo">End map info.</param>
	public void IDKSetRoleEndMap(string endMapInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleEndMap",endMapInfo);
		#endif
	}
	/// <summary>
	/// IDKs the set role exit.
	/// 角色退出
	/// </summary>
	/// <param name="roleInfo">Role info.</param>
	public void IDKSetRoleExit(string roleInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleExit",roleInfo);
		#endif
	}
	/// <summary>
	/// IDKs the set role break line. 
	/// 角色断线
	/// </summary>
	/// <param name="roleInfo">Role info.</param>
	public void IDKSetRoleBreakLine(string roleInfo) {
		#if UNITY_ANDROID
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("IDKSetRoleBreakLine",roleInfo);
		#endif
	}



}
