using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GainData[] testData;
    // Use this for initialization
    private ExploreData currentExplore;
    void Start()
    {

        //Invoke("ExploreTest", 1);



        //data[1].itemtype = ItemType.Prop;
        //data[1].itemNumber = 10;
        //data[1].itemId = 1;
        //for (int i = 0; i < data.Length; i++)
        //{
        //    data[i].itemtype = ItemType.Prop;
        //    data[i].itemNumber = 10;
        //    data[i].itemId = 1;

        //}

    }

    private void ExploreTest()
    {
        currentExplore = GameExploreData.Instance.GetItem(1);
        DropBagData dropData = GameDropBagData.Instance.GetItem(currentExplore.DroppingBoxId);
        int roll_1 = UnityEngine.Random.Range(0, dropData.MaxDrop + 1); //随机多少个道具
        int index = 0;
        List<GainData> gainData = new List<GainData>();
        for (int i = 0; i < dropData.Items.Length; i++)
        {
            int roll = UnityEngine.Random.Range(0, 100);
            if (roll <= dropData.Items[i].dropOdds)
            {
                index++;
                gainData.Add(new GainData(dropData.Items[i].itemID, dropData.Items[i].itemType, UnityEngine.Random.Range(1, dropData.Items[i].maxNumber), dropData.AddGold));
            }
            if (index >= dropData.MaxDrop)
            {
                break;
            }
        }
        testData = new GainData[gainData.Count];
        for (int i = 0; i < gainData.Count; i++)
        {
            Debug.Log(gainData[i].itemId);
            testData[i] = gainData[i];
        }
        TinyTeam.UI.TTUIPage.ShowPage<UIGainTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateGainTipEvent, testData);
    }

    private void UpdateTest()
    {

    }


}
