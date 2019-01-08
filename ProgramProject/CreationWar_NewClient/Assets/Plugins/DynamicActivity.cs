/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicActivity : MonoBehaviour 
{
    public static DynamicActivity instance;

    public UIGrid grid;
    public GameObject Growup;//成长福利
    public GameObject Continuous;//连续充值奖励
    public GameObject Leveling;//冲级奖励
    public GameObject Recharge;//充值奖励
    public GameObject Consumption;//消费返额
    public GameObject Returnmoney;//充值反额

    private Dictionary<string, GameObject> btns = new Dictionary<string, GameObject>();
    void Awake()
    {
        instance = this;

        if(Growup)
        {
            btns.Add("Growup", Growup);
        }

        if (Continuous)
        {
            btns.Add("Continuous", Continuous);
        }

        if (Leveling)
        {
            btns.Add("Leveling", Leveling);
        }

        if (Recharge)
        {
            btns.Add("Recharge", Recharge);
        }

        if (Consumption)
        {
            btns.Add("Consumption", Consumption);
        }

        if (Returnmoney)
        {
            btns.Add("Returnmoney", Returnmoney);
        }
    }

    void OnEnable()
    {
        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().AskDynamicActivity());
    }

	// Use this for initialization
	void Start () 
	{
	    
	}

    private Dictionary<string, Dictionary<string, object>> noticeInfo = new Dictionary<string, Dictionary<string, object>>();
    private Dictionary<string, string> panelActiveDic = new Dictionary<string, string>();
    public void GetActivityInfo(Dictionary<string, object> activity)
    {
        noticeInfo.Clear();
        panelActiveDic.Clear();
        foreach(KeyValuePair<string, object> kvp in activity)
        {
            Dictionary<string, object> mDic = ((Dictionary<object, object>)kvp.Value).DicObjTo<string, object>();
            noticeInfo.Add(kvp.Key, mDic);

            string label = (string)mDic["label"];
            string onoff = (string)mDic["onoff"];
            panelActiveDic.Add(label, onoff);
        }

        if (gameObject.activeSelf)
        {
            StartCoroutine(SetPanelState());
        }
    }

    IEnumerator SetPanelState()
    {
        foreach(KeyValuePair<string, string> kvp in panelActiveDic)
        {
            if(kvp.Value.Equals("0"))
            {
                if (btns.ContainsKey(kvp.Key))
                {
                    btns[kvp.Key].SetActive(false);
                }
            }
            else
            {
                if (btns.ContainsKey(kvp.Key))
                {
                    int num = btns[kvp.Key].GetComponent<BtnSelect>().num;
                    //DailyBenefitsPanelSelect.My.listPanel[num].SendMessage("SetInfo", SendMessageOptions.DontRequireReceiver);

                    DailyBenefitsPanelSelect.My.listPanel[num].GetComponent<DynamicActivityPanel>().SetInfo();
                }
            }
        }

        yield return new WaitForFixedUpdate();
        grid.Reposition();
    }

    public Dictionary<string, Dictionary<string, object>> GetNoticeInfo()
    {
        return noticeInfo;
    }

    public void OpenTargetPanel(string id)
    {
        GameObject go = null;

        if (btns.ContainsKey(id))
        {
            go = btns[id];
            DailyBenefitsPanelSelect.My.OpenTargetActivity(go);
        }  
    }
}
