using UnityEngine;
using System.Collections;

public class LoginJson
{
	public string uid;
	public string psd;
	public string nickname;
	public int other;//==0无信息,1断线重连==//
	public string platform;
	
	public LoginJson(string uid,string psd,int other,string platform,string nickname)
	{
		this.uid=uid;
		this.psd=psd;
		this.other=other;
		this.platform=platform;
		this.nickname=nickname;
	}
}
