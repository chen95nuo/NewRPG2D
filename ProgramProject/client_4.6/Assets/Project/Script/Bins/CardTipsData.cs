using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardTipsData : PropertyReader {
    public int id { get; set; }
    public string name { get; set; }
    public string model { get; set; }
    public string description { get; set; }
	public float yPos{get;set;}
	public float zoom{get;set;}
	public float modelrotation{get;set;}
	
	public string atlas;
	public string icon;
	
    private static Hashtable data = new Hashtable();
    public static List<int> idList = new List<int>();

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

    public static CardTipsData getData(int id)
    {
        return (CardTipsData)data[id];
    }

    public static List<int> getCardTipsIDArray(int guideID)
    {
        List<int> idList = new List<int>();
        foreach (DictionaryEntry de in data)
        {
            CardTipsData td = (CardTipsData)de.Value;
            if (td.id == guideID)
            {
                idList.Add(td.id);
            }
        }
        if (idList.Count > 0)
        {
            idList.Sort();
        }
        return idList;
    }
}
