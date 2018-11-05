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
        HallEventManager.instance.AddListener(HallEventDefineEnum.CameraMove, ClosePage);
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CameraMove, ClosePage);
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
            ClosePage();
        }
    }

    public override void Hide(bool needAnim = true)
    {
        BG.fillAmount = 1;
        CameraControl.instance.IsShowEdit = false;
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }
}
