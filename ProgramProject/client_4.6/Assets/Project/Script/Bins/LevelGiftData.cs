using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGiftData : PropertyReader
{
    public int id;
    public int level;

    public List<int> goodsType = new List<int>();
    public List<int> goodsIds = new List<int>();
    public List<int> goodsNum = new List<int>();

    private static Dictionary<int, LevelGiftData> data = new Dictionary<int, LevelGiftData>();

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
        level = StringUtil.getInt(ss[index++]);
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

    public static LevelGiftData getData(int id)
    {
        return data[id];
    }

    public static Dictionary<int, LevelGiftData> getAllData()
    {
        return data;
    }

    public static int getNum()
    {
        return data.Count;
    }
}
