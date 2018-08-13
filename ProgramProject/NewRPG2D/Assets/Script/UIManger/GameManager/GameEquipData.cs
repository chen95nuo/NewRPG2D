using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class GameEquipData
{

    private static GameEquipData instance;
    public static GameEquipData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.EquipData];
                instance = JsonUtility.FromJson<GameEquipData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<EquipData>();
                }

            }
            return instance;
        }
    }

    public List<EquipData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EquipData GetItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                string affix_1 = GitAffix(items[i].Affix_1);
                string affix_2 = GitAffix(items[i].Affix_2);
                string affix_3 = GitAffix(items[i].Affix_3);
                string affix_4 = GitAffix(items[i].Affix_4);


                return new EquipData(items[i], affix_1, affix_2, affix_3, affix_4);
            }
        }
        return null;
    }

    public EquipData QueryEquip(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                return items[i];
            }
        }
        return null;
    }

    public string GitAffix(string affix)
    {
        if (affix == "")
        {
            return null;
        }
        string[] str;
        char[] ch = new char[] { '(', ',', ')' };
        str = affix.Split(ch);
        int a = int.Parse(str[1]);
        int b = int.Parse(str[2]) + 1;
        int roll = UnityEngine.Random.Range(a, b);
        return str[0] + "+" + roll;
    }
}