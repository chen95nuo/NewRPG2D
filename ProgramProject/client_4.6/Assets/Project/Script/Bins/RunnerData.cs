using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunnerData : PropertyReader {

	public int id;
	public int state;		//活动状态//
	public int type;		//活动类型//
	public string text;	//档位文字//
	public string icon;	//档位图标//
	public int condition;	//达成条件//
	public int time;			//抽奖次数//
	
	//转盘物品数量列表, 注:类型为1-5的格式是“id,num”,6以上是“num”(是金币等物品，无id).所以6以上的物品id为0//
	public List<int> goodsType = new List<int>();		//转盘物品类型列表//
    public List<int> goodsIds = new List<int>();		//转盘物品id列表//
    public List<int> goodsNum = new List<int>();		//转盘物品数量列表//
	
	private static Hashtable data = new Hashtable();
	
	public void addData()
    {
        data.Add(id, this);
    }

    public void resetData()
    {
        data.Clear();
    }

    public void parse(string[] ss)
    {
		int location = 0;
		id = StringUtil.getInt(ss[location]);
		state = StringUtil.getInt(ss[location+1]);
		type = StringUtil.getInt(ss[location+2]);
		text = ss[location+3];
		icon = ss[location+4];
		condition = StringUtil.getInt(ss[location+5]);
		time = StringUtil.getInt(ss[location+6]);
		
		for(int i=0;i<8;i++)
		{
			location = 7 + i*5;
			int iType = StringUtil.getInt(ss[location]);
			if(iType==0)	break;
			goodsType.Add(iType);
			
			string[] temp = ss[location+1].Split(',');
			if(temp.Length == 1)
			{
				goodsIds.Add(0);
				goodsNum.Add(StringUtil.getInt(temp[0]));
			}
			else if(temp.Length == 2)
			{
				goodsIds.Add(StringUtil.getInt(temp[0]));
                goodsNum.Add(StringUtil.getInt(temp[1]));
			}
		}
		
		addData();
	}
	
	public static RunnerData getData(int id)
    {
        return (RunnerData)data[id];
    }
	
	public static int GetValidRunDataNum()
	{
		int num = 0;
		foreach(DictionaryEntry item in data)
		{
			RunnerData temp = (RunnerData)item.Value;
			if(temp.type != 0)
			{
				num++;
			}
		}
		return num;
	}
	
	public static int GetCurValidRunDataNum()
	{
		int num = 0;
		foreach(DictionaryEntry item in data)
		{
			RunnerData temp = (RunnerData)item.Value;
			if(temp.state == 1 && temp.type != 0)
			{
				num++;
			}
		}
		return num;
	}
	
	//获取当前激活的活动类型//
	public static int GetCurActiveActType()
	{
		int activeType = 0;
		foreach(DictionaryEntry item in data)
		{
			RunnerData temp = (RunnerData)item.Value;
			if(temp.state == 1)
			{
				activeType = temp.type;
				break;
			}
		}
		return activeType;
	}
}
