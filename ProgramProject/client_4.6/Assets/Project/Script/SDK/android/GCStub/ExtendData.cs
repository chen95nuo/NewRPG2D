using UnityEngine;
using System.Collections;

public class ExtendData
{
	public string roleId; //必填//
	public string roleName;//必填//
	public string roleLevel;//必填//
	public int zoneId;//必填//
	public string zoneName;//区名//
	public string balance;//必填(用户游戏余额)//
	public string partyName;//可选(公会帮派名称)//
	public string gamerVip;//可选(vip等级)//
	
	public ExtendData(string roleId,string roleName,string roleLevel,int zoneId,string zoneName,string balance,string partyName,string gamerVip)
	{
		this.roleId=roleId;
		this.roleName=roleName;
		this.roleLevel=roleLevel;
		this.zoneId=zoneId;
		this.zoneName=zoneName;
		this.balance=balance;
		this.partyName=partyName;
		this.gamerVip=gamerVip;
	}
}
