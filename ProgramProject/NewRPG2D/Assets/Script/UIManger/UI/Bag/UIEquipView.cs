using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle.BattleData;

public class UIEquipInfoHelper
{
    public HallRoleData roleData;
    public EquipmentRealProperty equipData;
    public UIEquipInfoHelper(EquipmentRealProperty equipData, HallRoleData roleData)
    {
        this.equipData = equipData;
        this.roleData = roleData;
    }
}
public class UIEquipInfoHelper_1
{
    public HallRoleData roleData;
    public EquipmentRealProperty roleEquipData;
    public EquipmentRealProperty bagEquipData;
    public UIEquipInfoHelper_1(EquipmentRealProperty roleEquipData, EquipmentRealProperty bagEquipData, HallRoleData roleData)
    {
        this.roleEquipData = roleEquipData;
        this.bagEquipData = bagEquipData;
        this.roleData = roleData;
    }
}

public class UIEquipView : TTUIPage
{

    public GameObject atrGrid;

    public UIEquipViewGrid equipGrid_1;
    public UIEquipViewGrid equipGrid_2;

    public Button btn_Close;

    private void Awake()
    {
        btn_Close.onClick.AddListener(ClosePage);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        if (mData is UIEquipInfoHelper)
        {
            UIEquipInfoHelper equipData = mData as UIEquipInfoHelper;
            UpdateInfo(equipData);
        }
        else if (mData is UIEquipInfoHelper_1)
        {
            UIEquipInfoHelper_1 equipData = mData as UIEquipInfoHelper_1;
            UpdateInfo(equipData);
        }
    }

    private void UpdateInfo(UIEquipInfoHelper equipData)
    {
        equipGrid_2.transform.parent.gameObject.SetActive(false);

        if (equipData.roleData == null)
        {
            equipGrid_1.UpdateInfo(equipData.equipData, 1, equipData.roleData);
        }
        else
        {
            equipGrid_1.UpdateInfo(equipData.equipData, 2, equipData.roleData);
        }

    }

    private void UpdateInfo(UIEquipInfoHelper_1 equipInfoData)
    {
        equipGrid_2.transform.parent.gameObject.SetActive(true);
        equipGrid_1.UpdateInfo(equipInfoData.roleEquipData, 2, equipInfoData.roleData);
        equipGrid_2.UpdateInfo(equipInfoData.bagEquipData, 0, null);
    }

    public override void ClosePage()
    {
        equipGrid_2.transform.parent.gameObject.SetActive(false);
        base.ClosePage();
    }
}