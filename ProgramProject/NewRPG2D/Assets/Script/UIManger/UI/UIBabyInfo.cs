using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIBabyInfo : TTUIPage
{
    public Image slider;
    public Text txt_time;
    public Text txt_Tip;
    public Button btn_Close;
    public Button btn_Click;
    private RoleBabyData currentData;

    private void Awake()
    {
        btn_Click.onClick.AddListener(ClosePage);
        btn_Close.onClick.AddListener(ClosePage);
    }

    public void UpdateInfo(RoleBabyData babyData)
    {
        currentData = babyData;
        txt_time.text = babyData.time.ToString("#0");
        slider.fillAmount = (600 - babyData.time) / 600;
    }

    public void Update()
    {
        txt_time.text = SystemTime.instance.TimeNormalizedOf((float)currentData.time,false);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        RoleBabyData data = mData as RoleBabyData;
        UpdateInfo(data);
    }
}
