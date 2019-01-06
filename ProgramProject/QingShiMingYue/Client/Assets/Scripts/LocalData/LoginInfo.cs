using System;
using System.Collections.Generic;
using KodGames;
using KodGames.ClientClass;

public class LoginInfo
{
	private int serverTimeZone = 0;
	public int ServerTimeZone
	{
		get { return serverTimeZone; }
		set { serverTimeZone = value; }
	}

	private long loginTimeSinceStartup = 0;
	private long loginTime;
	public long LoginTime
	{
		set
		{
			loginTime = value;
			loginTimeSinceStartup = (int)(TimeEx.realtimeSinceStartup * 1000);
		}
	}

	public long NowTime
	{
		get { return loginTime + (int)(TimeEx.realtimeSinceStartup * 1000) - loginTimeSinceStartup; }
	}

	public System.DateTime NowDateTime
	{
		get { return TimeEx.ToUTCDateTime(NowTime).AddHours(serverTimeZone); }
	}

	public System.DateTime ToServerDateTime(long time)
	{
		return TimeEx.ToUTCDateTime(time).AddHours(serverTimeZone);
	}

	public System.DateTime ServerTimeToLocalTime(System.DateTime dateTime)
	{
		return dateTime.AddHours(-serverTimeZone).AddHours(TimeEx.LocalZoneOffset);
	}

	private bool quickLogin = false;
	public bool QuickLogin
	{
		get { return quickLogin; }
		set { quickLogin = value; }
	}

	private string account = "";
	public string Account
	{
		get { return account; }
		set { account = value; }
	}

	private string password = "";
	public string Password
	{
		get { return password; }
		set { password = value; }
	}

	private int accountId;
	public int AccountId
	{
		get { return accountId; }
		set { accountId = value; }
	}

	private string loginToken = "";
	public string LoginToken
	{
		get { return loginToken; }
		set { loginToken = value; }
	}

	private List<Area> serverAreas = new List<Area>();
	public List<Area> ServerAreas
	{
		get { return serverAreas; }
		set { serverAreas = value != null ? value : new List<Area>(); }
	}

	private int lastAreaId;
	public int LastAreaId
	{
		get { return lastAreaId; }
		set { lastAreaId = value; }
	}

	private KodGames.ClientClass.Area loginArea;
	public KodGames.ClientClass.Area LoginArea
	{
		get { return loginArea; }
		set { loginArea = value; }
	}

	public KodGames.ClientClass.Area SearchArea(int areaId)
	{
		foreach (var area in serverAreas)
			if (area.AreaID == areaId)
				return area;

		return null;
	}
}
