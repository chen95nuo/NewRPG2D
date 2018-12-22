using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunData : PropertyReader {

	public int id;
	public int runtype;
	public int run;
	public int turntime;		//转动次数//
	public int rewardtype;		//奖励类型//
	public string timereward;	//奖励内容, 注:类型为1-5的格式是“id,num”,6以上是“num”//
	
	//转盘奖励物品数量列表, 注:类型为1-5的格式是“id,num”,6以上是“num”(是金币等物品，无id).所以6以上的物品id为0//
	public int rewardId;
	public int rewardNum;
	
	private static Hashtable data = new Hashtable();
	
	//有效的id列表//
	public static List<int> runDataIDsList = new List<int>();
	
	public void addData()
    {
        data.Add(id, this);
    }

    public void resetData()
    {
        data.Clear();
		runDataIDsList.Clear();
    }

    public void parse(string[] ss)
    {
		id = StringUtil.getInt(ss[0]);
		runtype = StringUtil.getInt(ss[1]);
		run = StringUtil.getInt(ss[2]);
		turntime = StringUtil.getInt(ss[3]);
		rewardtype = StringUtil.getInt(ss[4]);
		timereward = ss[5];
		
		if(rewardtype != 0)
		{
			string[] temp = timereward.Split(',');
			if(temp.Length == 1)
			{
				rewardId = 0;
				rewardNum = StringUtil.getInt(temp[0]);
			}
			else if(temp.Length == 2)
			{
				rewardId = StringUtil.getInt(temp[0]);
                rewardNum = StringUtil.getInt(temp[1]);
			}
			
			runDataIDsList.Add(id);
		}
		addData();
	}
	
	public static RunData getData(int id)
    {
        return (RunData)data[id];
    }
	
	public static int getTotalRewardNum()
    {
        return data.Count;
    }
	
	public static int getValidRewardNum()
    {
		int num = 0;
		foreach(DictionaryEntry item in data)
		{
			RunData temp = (RunData)item.Value;
			if(temp.rewardtype != 0)
			{
				num++;	
			}
		}
		return num;
    }
	
	//得到当前档位的RunData list//
	public static List<RunData> getCurRunList(int curActiveType,int curRun)
    {
		List<RunData> tempRunDataList = new List<RunData>();
		foreach(DictionaryEntry item in data)
		{
			RunData temp = (RunData)item.Value;
			if((temp.rewardtype != 0) && (temp.runtype == curActiveType) && (temp.run == curRun))
			{
				tempRunDataList.Add(temp);
			}
		}
		return tempRunDataList;
    }
}
