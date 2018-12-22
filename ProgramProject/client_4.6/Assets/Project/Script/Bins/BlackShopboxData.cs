using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackShopboxData : PropertyReader
{
    public int number { get; set; }
    public int viplevel { get; set; }
    public int costtype { get; set; }
    public int cost { get; set; }

    private static Dictionary<int, BlackShopboxData> data = new Dictionary<int, BlackShopboxData>();

    public void addData()
    {
        data.Add(number, this);
    }

    public void resetData()
    {
        data.Clear();
    }

    public void parse(string[] ss) { }

    public static BlackShopboxData getData(int id)
    {
        return data[id];
    }

    public static int getTotalNum()
    {
        return data.Count;
    }
}
