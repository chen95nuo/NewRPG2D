using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : PropertyReader {

	public int level{get;set;}
	public int exp{get;set;}
	public int maxHp{get;set;}
	public int maxFight{get;set;}
	public int maxCard{get;set;}
	public int maxFriend{get;set;}
	public int recover{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(level, this);
	}

	public void resetData()
	{
		data.Clear();
	}

	public static PlayerData getData(int level)
	{
		return (PlayerData)data[level];
	}

	public void parse(string[] ss)
	{
	}
}
