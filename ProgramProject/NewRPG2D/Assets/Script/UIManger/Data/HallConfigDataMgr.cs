using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class HallConfigDataMgr : ItemCsvDataBaseMgr<HallConfigDataMgr>
{
    protected override CsvEChartsType CurrentCsvName
    {
        get { return CsvEChartsType.HallConfigData; }
    }

    private Dictionary<HallConfigEnum, string> dic;
    private int[] maxHappiness;
    private Dictionary<int, int[]> happiness;

    public Dictionary<int, int[]> Happiness
    {
        get
        {
            if (happiness == null)
            {
                happiness = new Dictionary<int, int[]>();
                string st = Dic[HallConfigEnum.happiness];
                string realDateString = st.Substring(2, st.Length - 4);
                string[] temp = realDateString.Split('|');
                for (int i = 0; i < temp.Length; i++)
                {
                    string[] temp1 = temp[i].Split(',');
                    happiness.Add(int.Parse(temp1[0]), new int[2] { int.Parse(temp1[1]), int.Parse(temp1[2]) });
                }
            }
            return happiness;
        }
    }

    public Dictionary<HallConfigEnum, string> Dic
    {
        get
        {
            if (dic == null)
            {
                dic = new Dictionary<HallConfigEnum, string>();
                GetDic();
            }
            return dic;
        }
    }

    private void GetDic()
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            HallConfigData data = CurrentItemData[i] as HallConfigData;
            dic.Add(data.key, data.value);
        }
    }

    public float GetValue(HallConfigEnum name)
    {
        return float.Parse(Dic[name]);
    }

    public int GetNowHappiness(int amount)
    {
        foreach (var h in Happiness)
        {
            if (h.Value[0] < amount && h.Value[1] > amount)
            {
                return h.Key;
            }
        }
        return 0;
    }

    public int GetMaxHappiness(bool isTrue = false)
    {
        if (maxHappiness == null)
        {
            string st = Dic[HallConfigEnum.happinessMax];
            string realDateString = st.Substring(1, st.Length - 2);
            string[] sp = realDateString.Split(',');
            maxHappiness = new int[2];
            maxHappiness[0] = int.Parse(sp[0]);
            maxHappiness[1] = int.Parse(sp[1]);
        }

        return isTrue ? maxHappiness[1] : maxHappiness[0];
    }



}
