using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeditationData : PropertyReader {

    public int id; //任务编号//

    public int timeNumber; //需求次数//

    public int type; //奖励类型//

    public int itemID; //奖励内容//
	
	public int random;//随即机制//


    private static Hashtable data = new Hashtable();

    private static List<MeditationData> dataList = new List<MeditationData>();

    public List<string> reward;
    public void addData()
    {

        data.Add(id, this);
        dataList.Add(this);
    }

    public void resetData() { }

    public void parse(string[] ss)
    {
        int location = 0;
        id = StringUtil.getInt(ss[location++]);
        timeNumber = StringUtil.getInt(ss[location++]);
        type = StringUtil.getInt(ss[location++]);
        itemID = StringUtil.getInt(ss[location++]);
		random = StringUtil.getInt(ss[location++]);

        reward = new List<string>();
        for (int i = 0; i < 2; i++)
        {
            location = 5 + i * 1;
            int rewardType = StringUtil.getInt(ss[location]);

            int rewardDetail = StringUtil.getInt(ss[location]);
            if (rewardType != 0)
            {
                reward.Add(rewardType + "-" + rewardDetail);
            }
        }
        addData();
    }

    public static MeditationData GetData(int id)
    {
        return (MeditationData)data[id];
    }
}   
