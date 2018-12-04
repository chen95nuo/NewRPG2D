//主城幸福度系统

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUpTips_5 : TTUIPage
{
    public Slider slider;
    public Text txt_Tip;
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;

    public Text txt_NowHappiness;
    public Text txt_NowUp;

    public Text[] txt_num;

    public Transform tipTs;

    private void Awake()
    {
        txt_Tip.text = LanguageDataMgr.instance.GetUIString("xinfudu");
        txt_Tip_1.text = LanguageDataMgr.instance.GetUIString("dangqianxinfuzhi");
        txt_Tip_2.text = LanguageDataMgr.instance.GetUIString("shouyijiacheng");
        txt_Tip_3.text = LanguageDataMgr.instance.GetUIString("xinfuduHelp");
        slider.maxValue = HallConfigDataMgr.instance.GetMaxHappiness(false);

        int index = 0;
        foreach (var v in HallConfigDataMgr.instance.Happiness)
        {
            txt_num[index].text = v.Key.ToString();
            index++;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (go == null || go != tipTs.gameObject)
            {
                ClosePage();
            }
        }
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        Transform ts = mData as Transform;
        UpdateInfo(ts);
    }

    public void UpdateInfo(Transform ts)
    {
        PlayerData data = GetPlayerData.Instance.GetData();
        int max = HallConfigDataMgr.instance.GetMaxHappiness(false);
        int amount = HallConfigDataMgr.instance.GetNowHappiness(data.Happiness);
        slider.value = amount;
        txt_NowHappiness.text = data.Happiness + "/" + max;
        txt_NowUp.text = amount+"%";
        tipTs.position = ts.position;
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
    }

}
