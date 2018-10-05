using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIEditModeTip : TTUIPage
{
    public Image BG;

    private void OnEnable()
    {
        BG.fillAmount = 1;
    }
    // Update is called once per frame
    void Update()
    {
        BG.fillAmount -= Time.deltaTime;
        if (BG.fillAmount <= 0)
        {
            BG.fillAmount = 0;
            MapControl.instance.ShowEditMap();
            UIPanelManager.instance.ClosePage(this);
        }
    }
}
