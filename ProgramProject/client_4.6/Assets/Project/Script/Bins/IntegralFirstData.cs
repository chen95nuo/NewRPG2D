using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntegralFirstData : PropertyReader
{
    public int id;
    public int rank;
	
	//type-id-num//
    public static List<string> rewardInfo = new List<string>();

    private static Dictionary<int, IntegralFirstData> data = new Dictionary<int, IntegralFirstData>();

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
        int index = 0;
        id = StringUtil.getInt(ss[index++]);
        rank = StringUtil.getInt(ss[index++]);
        for (; index < ss.Length; index += 2)
        {
            int tempType = StringUtil.getInt(ss[index]);
            if (tempType == 0) break;
            string[] temp = ss[index + 1].Split(',');
			string info = tempType+"-"+temp[0]+"-"+temp[1];
			rewardInfo.Add(info);
        }
        addData();
    }
	
	public static IntegralFirstData getData(int id)
	{
		return (IntegralFirstData)data[id];
	}
}
