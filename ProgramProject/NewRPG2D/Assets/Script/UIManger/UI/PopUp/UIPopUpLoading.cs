using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUpLoading : TTUIPage
{
    public Image icon;

    public override void Show(object mData)
    {
        base.Show(mData);
    }

    private void Update()
    {
        icon.transform.Rotate(Vector3.back);
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
