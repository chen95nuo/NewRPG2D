using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VipData : PropertyReader {
	
	public int level { get; set; }
	public int costios{get;set;}
	public int costandroid{get;set;}
	public string description{get;set;}
	public int gold{get;set;}
	public int energy{get;set;}
	public int maze{get;set;}
	public int normal{get;set;}
	public int hero{get;set;}
	public int activity{get;set;}
	public int mission{get;set;}
	public int missiontimes{get;set;}
	public int energystart{get;set;}
	public int maxenergy{get;set;}
	public int pvptime{get;set;}
	public int autoupgrade{get;set;}
	public int giftid{get;set;}
	public int fakeprice{get;set;}
	public int realprice{get;set;}
	public int time{get;set;}
	
	public static List<int> vipList = new List<int>();

    private static Hashtable data = new Hashtable();
	
	public void addData()
    {
        data.Add(level, this);
        vipList.Add(level);
    }
    public void resetData()
    {
        data.Clear();
    }
    public void parse(string[] ss) { }
	
	public static VipData getData(int level)
    {
        return (VipData)data[level];
    }
	
	//根据要扫荡的次数来获得可以进行该扫荡行为的vip等级//
	public static int getLevelForSweep(int times)
	{
		int level = 0;
		for(int i =0 ;i < vipList.Count;i++)
		{
			int key = vipList[i];
			VipData vip = (VipData)data[level];
			if((times > 1 && vip.missiontimes > 0)||(times == 1 && vip.mission > 0))
			{
				level = key;
				break;
			}
			
		}
		return level;
	}

    //获得可以进行强化行为的vip等级//
    public static int getLevelForIntensify()
    {
        int level = 0;
        for (int i = 0; i < vipList.Count; i++)
        {
            VipData vip = (VipData)data[i];
            if (vip.autoupgrade > 0)
            {
                return vip.level;
            }
        }
        return level;
    }
}
