using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle.BattleData;

public class UIEquipInfoHelper
{
    public EquipmentRealProperty roleEquipData;
    public EquipmentRealProperty bagEquipData;
    public UIEquipInfoHelper(EquipmentRealProperty roleEquipData, EquipmentRealProperty bagEquipData)
    {
        this.roleEquipData = roleEquipData;
        this.bagEquipData = bagEquipData;
    }
}

public class UIEquipInfo : TTUIPage
{

    public GameObject atrGrid;

    public override void Show(object mData)
    {
        base.Show(mData);
        if (mData is EquipmentRealProperty)
        {

        }
        else if (mData is UIEquipInfoHelper)
        {

        }
    }

    private void UpdateInfo(EquipmentRealProperty equipData)
    {

    }

    private void UpdateInfo(UIEquipInfoHelper equipInfoData)
    {


    }


}