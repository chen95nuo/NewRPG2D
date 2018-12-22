using UnityEngine;
using System.Collections;

public class VersionJson
{
	public string versionId;
	public string channelId;
	public string platform;
	
	public VersionJson(string versionId,string channelId,string platform)
	{
		this.versionId=versionId;
		this.channelId=channelId;
		this.platform=platform;
	}
	
}
