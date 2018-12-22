using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnnounceResultJson : ErrorJson {
	
	public List<Announce> announces;
	
}

public class Announce
{
	public int id{get;set;}
	public string announce{get;set;}	
	public int frequency{get;set;}		//时间间隔：分//
	public int num{get;set;}				//显示次数//
	public int type{get;set;}				//类型：2即时型，1事件型//
	public long time{get;set;}
	public long curTime{get;set;}
	public int aType{get;set;}				//1游戏内公告   2转盘公告//
}
