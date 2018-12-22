using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SevenDaysData : PropertyReader
{
    public int days;
    public List<int> goodsType = new List<int>();
    public List<int> goodsIds = new List<int>();
    public List<int> goodsNum = new List<int>();

    private static Dictionary<int, SevenDaysData> data = new Dictionary<int, SevenDaysData>();

    public void addData()
    {
        data.Add(days, this);
    }

    public void resetData()
    {
        data.Clear();
    }

    public void parse(string[] ss)
    {
        int index = 0;
        days = StringUtil.getInt(ss[index++]);
        for (; index < ss.Length; index += 2)
        {
            int tempType = StringUtil.getInt(ss[index]);
            if (tempType == 0) break;
            goodsType.Add(tempType);

            string[] temp = ss[index + 1].Split(',');
            if (temp.Length == 1)
            {
                goodsIds.Add(0);
                goodsNum.Add(StringUtil.getInt(temp[0]));
            }
            else if (temp.Length == 2)
            {
                goodsIds.Add(StringUtil.getInt(temp[0]));
                goodsNum.Add(StringUtil.getInt(temp[1]));
            }
        }
        addData();
    }

    public static SevenDaysData getData(int day)
    {
        return data[day];
    }

    public static Dictionary<int, SevenDaysData> getAllData()
    {
        return data;
    }

    public static int getDayNum()
    {
        return data.Count;
    }
}
