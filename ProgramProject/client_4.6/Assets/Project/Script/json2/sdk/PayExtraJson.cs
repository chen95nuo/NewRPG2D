using UnityEngine;
using System.Collections;

public class PayExtraJson
{
	public string username;
	public int playerId;
	public int rechargeId;
	public string os;
	public string merchantName;
	
	public PayExtraJson()
	{}
	
	public PayExtraJson(string username,int playerId,int rechargeId,string os,string merchantName)
	{
		this.username=username;
		this.playerId=playerId;
		this.rechargeId=rechargeId;
		this.os=os;
		this.merchantName = merchantName;
	}
}
