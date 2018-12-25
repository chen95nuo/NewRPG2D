using UnityEngine;
using System.Collections;

public class ExtendDataForUC
{
	public string roleId; //必填//
	public string roleName;//必填//
	public string roleLevel;//必填//
	public int zoneId;//必填//
	public string zoneName;//区名//
	
	public ExtendDataForUC(string roleId,string roleName,string roleLevel,int zoneId,string zoneName)
	{
		this.roleId=roleId;
		this.roleName=roleName;
		this.roleLevel=roleLevel;
		this.zoneId=zoneId;
		this.zoneName=zoneName;
	}
}
