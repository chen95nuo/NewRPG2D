using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIEditModeTip : TTUIPage
{
    public Image BG;

    private void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.CameraMove, closePage);

    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CameraMove, closePage);
    }

    private void OnEnable()
    {
        BG.fillAmount = 1;
    }
    public override void Show(object mData)
    {
        base.Show(mData);
    }

    // Update is called once per frame
    void Update()
    {
        BG.fillAmount -= Time.deltaTime;
        if (BG.fillAmount <= 0)
        {
            BG.fillAmount = 0;
            MapControl.instance.ShowEditMap();
            closePage();
        }
    }

    public void closePage()
    {
        CameraControl.instance.isShowEdit = false;
        UIPanelManager.instance.ClosePage(this);
    }

}
