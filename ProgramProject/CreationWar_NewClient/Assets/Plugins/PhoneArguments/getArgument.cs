using UnityEngine;
using System.Collections;

public class getArgument : MonoBehaviour {

	/// <summary>
	/// 接受安卓传过来的所有参数，是用；隔开的,{所有参数的值= 手機的型號；cpu的核數；cpu的名字；cpu最大赫茲；cpu最小赫茲；手機總內存；手機可利用內存；手機MAC地址；手機IP；手机Imei；网络类型（3g,2g.wifi）}；
	/// 当cpu最大赫兹，最小赫兹，和MAC地址为null,手机Imei为15个0的时候，来判断这款手机为虚拟机（这四个判断取其一即可）;各个参数是用“；”隔开
	/// </summary>
	/// <param name="message">Message.</param>
	public static void getArguments(string message) 
	{
		Debug.Log(message);

		string[] str;
		str = message.Split(';');
		BtnGameManager.yt[0]["DeviceType"].YuanColumnText = str[0];
		BtnGameManager.yt[0]["NetType"].YuanColumnText = str[10];
		Debug.Log("DeviceType == " + str[0] + " NetType == " + str[10]);
	}

	public GameObject qm;
	/// <summary>
	/// 接受安卓传过来的品质分级（如：messagee=1,则品质为1级）；
	/// </summary>
	/// <param name='message'>
	/// Message.
	/// </param>
	public void getGAMEQuality(string message){
		qm.SendMessage("returnAndroid" , message ,SendMessageOptions.DontRequireReceiver);
	}
	

}
