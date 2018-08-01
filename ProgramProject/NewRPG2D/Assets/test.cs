using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GainData[] data = new GainData[4];
    // Use this for initialization
    void Start()
    {
        data[1].itemtype = ItemType.Prop;
        data[1].itemNumber = 10;
        data[1].itemId = 1;
        for (int i = 0; i < data.Length; i++)
        {
            data[i].itemtype = ItemType.Prop;
            data[i].itemNumber = 10;
            data[i].itemId = 1;

        }
        Invoke("UpdateTest", 1);
    }

    private void UpdateTest()
    {
        TinyTeam.UI.TTUIPage.ShowPage<UIGainTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateGainTipEvent, data);
    }


}
