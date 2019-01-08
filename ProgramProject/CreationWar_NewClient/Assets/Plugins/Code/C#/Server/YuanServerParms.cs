using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum YuanerServerParmsStatus:int
{
	Update=0,
	Get,
}
public class YuanServerParms  {

	private Dictionary<short,object> prams;
	public Dictionary<short, object> Prams {
		get {
			return this.prams;
		}
		set {
			prams = value;
		}
	}

	public YuanServerParms(YuanerServerParmsStatus mStatus,yuan.YuanMemoryDB.YuanTable mYt,Dictionary<short,object> mPrams)
	{
		OutTime=0;
		Status=mStatus;
		Yt=mYt;
		Prams=mPrams;
	}
	

	
	private yuan.YuanMemoryDB.YuanTable yt;
	public yuan.YuanMemoryDB.YuanTable Yt {
		get {
			return this.yt;
		}
		set {
			yt = value;
		}
	}
	
	private int outTime;
	public int OutTime {
		get {
			return this.outTime;
		}
		set {
			outTime = value;
		}
	}
	
	public int times=0;
	
	private YuanerServerParmsStatus status;
	public YuanerServerParmsStatus Status {
		get {
			return this.status;
		}
		set {
			status = value;
		}
	}	
	
	
	public void Dispose()
	{
		OutTime=0;
		Prams=null;
		yt=null;
	}
	
}
