using UnityEngine;
using System.Collections;

public class RegistJson
{
	public string uid;
	public string psd;
	public string nickname;
	public string platform;
	
	public RegistJson(string uid,string psd,string nickname,string platform)
	{
		this.uid=uid;
		this.psd=psd;
		this.nickname=nickname;
		this.platform=platform;
	}
}
