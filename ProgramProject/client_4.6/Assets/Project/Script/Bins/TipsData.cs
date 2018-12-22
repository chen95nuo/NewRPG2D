using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipsData : PropertyReader {
    
    public int id { get; set; }
    public string chinese { get; set; }
    
    public static List<int> idList = new List<int>();

    private static Hashtable data = new Hashtable();

    public void addData()
    {
        data.Add(id, this);
        idList.Add(id);
    }
    public void resetData()
    {
        data.Clear();
    }
    public void parse(string[] ss) { }

    public static TipsData getData(int id)
    {
        return (TipsData)data[id];
    }
}
