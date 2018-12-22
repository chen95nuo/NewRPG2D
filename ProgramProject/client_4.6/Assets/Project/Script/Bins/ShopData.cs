using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopData : PropertyReader
{
    public int id { get; set; }
    public int goodstype { get; set; }
    public int itemId { get; set; }
    public string name { get; set; }
    public int viplevel { get; set; }
    public int dailynumber { get; set; }
    public int totalnumber { get; set; }
    public int costtype1 { get; set; }
    public int cost { get; set; }

    private static Dictionary<int, ShopData> data = new Dictionary<int, ShopData>();

    public void addData()
    {
        data.Add(id, this);
    }

    public void resetData()
    {
        data.Clear();
    }

    public void parse(string[] ss) { }

    public static ShopData getData(int id)
    {
        return data[id];
    }

    public static Dictionary<int, ShopData> getAllData()
    {
        return data;
    }
}
