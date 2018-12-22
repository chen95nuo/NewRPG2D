using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CornucopiaData : PropertyReader
{

    public int id;//编号//

    public int cost;//钻石金额//

    public List<string> dayawardstype_itemId;

    private static Hashtable data = new Hashtable();



    public void addData()
    {
        data.Add(id, this);
    }

    public static CornucopiaData GetData(int index)
    {
        return (CornucopiaData)data[index];
    }

    public void parse(string[] ss)
    {
        int location = 0;
        id = StringUtil.getInt(ss[location]);
        cost = StringUtil.getInt(ss[location + 1]);
        dayawardstype_itemId = new List<string>();
        for (int i = 0; i < 8; i++)
        {
            location = 2 + i * 2;
            int dayawardstype = StringUtil.getInt(ss[location]);
            if (dayawardstype == 0)
            {
                continue;
            }
            string itemId = ss[location + 1];
            string material_numStr = dayawardstype + "-" + itemId;
            dayawardstype_itemId.Add(material_numStr);
        }
        addData();
    }

    public void resetData()
    {
        data.Clear();
    }

}
