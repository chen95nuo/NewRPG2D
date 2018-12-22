using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackMarketData : PropertyReader
{
    public int id { get; set; }
    public int goodstype { get; set; }
    public int itemId { get; set; }
    public string name { get; set; }
    public int costtype { get; set; }
    public int cost { get; set; }
    public int number { get; set; }
    public int level { get; set; }
    public int showup { get; set; }
    public int probability1 { get; set; }
    public int probability2 { get; set; }
    public int probability3 { get; set; }

    private static Dictionary<int, BlackMarketData> data = new Dictionary<int, BlackMarketData>();

    public void addData()
    {
        data.Add(id, this);
    }

    public void resetData()
    {
        data.Clear();
    }

    public void parse(string[] ss) { }

    public static BlackMarketData getData(int id)
    {
        return data[id];
    }

    public static List<BlackMarketData> getBlackTop()
    {
        List<BlackMarketData> list = new List<BlackMarketData>();
        foreach (KeyValuePair<int, BlackMarketData> temp in data)
        {
            if (temp.Value.showup == 1)
                list.Add(temp.Value);
        }
        return list;
    }
}
