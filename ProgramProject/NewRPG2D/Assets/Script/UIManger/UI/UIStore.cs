﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStore : MonoBehaviour
{

    public int maxItem = 9;

    public Text time;

    public UIBagItem bagItem;


    private ItemData currentData;
    private float currentPrice;
    private ItemData[] toDayItem;
    private PlayerData playerData;


    void Awake()
    {
        UIEventManager.instance.AddListener<bool>(UIEventDefineEnum.UpdateBuyItem, IsTrue);

        Init();
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<bool>(UIEventDefineEnum.UpdateBuyItem, IsTrue);

    }

    private void Init()
    {
        playerData = GetPlayData.Instance.player[0];
    }

    private void Update()
    {
        System.DateTime nextDay = SystemTime.insatnce.GetTime().AddDays(1).Date;
        System.TimeSpan ts = nextDay.Subtract(SystemTime.insatnce.GetTime());
        string nowTime = "刷新时间:" + string.Format("{0:D2}", (int)ts.TotalHours) + ":" + string.Format("{0:D2}", ts.Minutes);
        time.text = nowTime;
    }

    private void Start()
    {
        RefreshStore();
    }


    private void RefreshStore()
    {
        List<StoreData> items = GameStoreData.Instance.items;
        int index = items.Count;
        List<StoreData> newData = new List<StoreData>();
        do
        {
            int roll = Random.Range(0, index);
            newData.Add(items[roll]);
        } while (newData.Count < maxItem);

        toDayItem = new ItemData[newData.Count];

        for (int i = 0; i < newData.Count; i++)
        {
            int roll = Random.Range(1, newData[i].PropNumber);
            ItemData temp = GamePropData.Instance.GetItem(newData[i].PropId, roll);
            toDayItem[i] = temp;
        }

        bagItem.UpdateStore(toDayItem);//刷新今日道具
    }

    public void IsTrue(bool isTrue)
    {
        bagItem.UpdateStore(toDayItem);//刷新道具
    }
}
