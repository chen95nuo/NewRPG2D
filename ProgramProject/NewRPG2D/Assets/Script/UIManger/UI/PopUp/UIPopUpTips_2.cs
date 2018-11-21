using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUpTips_2 : TTUIPage
{
    public static UIPopUpTips_2 instance;
    public Text txt_Name;
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Num_1;
    public Text txt_Num_2;

    public Transform tipTs;

    private void Awake()
    {
        instance = this;
        tipTs.position = Vector3.one * 20000;
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
    }

    public void UpdateInfo(BuildRoomName name, Transform ts)
    {
        int allyield = ChickPlayerInfo.instance.GetAllYield(name);
        int allStock = ChickPlayerInfo.instance.GetAllStockSpace(name);
        txt_Num_1.text = allyield.ToString();
        txt_Num_2.text = allStock.ToString();
        tipTs.position = ts.position;
    }






    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
        tipTs.position = Vector3.one * 20000;
    }
}
