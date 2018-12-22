using System;
using System.Collections;
using System.Collections.Generic;

public class NewPlayerBattleResultJson : ErrorJson
{
	public CardJson[] cs0;
	public CardJson[] cs1;
	
	//uniteskillId-怒气值//
	public List<string> us0;
	public List<string> us1;
	public int[] mes;//==双方最大怒气上限==//
	public int initE;//==初始怒气==//
}
