using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackRefreshData : PropertyReader
{
    public int number { get; set; }
    public int costtype { get; set; }
    public int costnumber { get; set; }

    private static Dictionary<int, BlackRefreshData> data = new Dictionary<int, BlackRefreshData>();

    public void addData()
    {
        data.Add(number, this);
    }

    public void resetData()
    {
        data.Clear();
    }

    public void parse(string[] ss) { }

    public static BlackRefreshData getData(int id)
    {
        return data[id];
    }

    public static int getTotalNum()
    {
        return data.Count;
    }
}
