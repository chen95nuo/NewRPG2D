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
    public static UIEquipView instance;

    [SerializeField]
    private GameObject viewGrid;
    [SerializeField]
    private Transform LeftPoint;
    [SerializeField]
    private Transform RightPoint;

    public UIEquipViewGrid leftEquipGrid;
    public UIEquipViewGrid rightEquipGrid;

    public Button btn_Close;

    private void Awake()
    {
        instance = this;

        btn_Close.onClick.AddListener(ClosePage);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        transform.SetSiblingIndex(transform.parent.childCount - 1);
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
        LeftPoint.gameObject.SetActive(false);
        RightPoint.gameObject.SetActive(true);

        if (equipData.roleData == null)
        {
            rightEquipGrid.UpdateInfo(equipData.equipData, 1, equipData.roleData);
        }
        else
        {
            rightEquipGrid.UpdateInfo(equipData.equipData, 3, equipData.roleData);
        }

    }

    private void UpdateInfo(UIEquipInfoHelper_1 equipInfoData)
    {
        LeftPoint.gameObject.SetActive(true);
        RightPoint.gameObject.SetActive(true);

        if (equipInfoData.bagEquipData == null)
        {
            LeftPoint.gameObject.SetActive(false);
            rightEquipGrid.UpdateInfo(equipInfoData.roleEquipData, 2, equipInfoData.roleData);
        }
        else
        {
            leftEquipGrid.UpdateInfo(equipInfoData.roleEquipData, 0, null);
            rightEquipGrid.UpdateInfo(equipInfoData.bagEquipData, 3, equipInfoData.roleData);
        }
    }

    public override void ClosePage()
    {
        LeftPoint.gameObject.SetActive(false);
        RightPoint.gameObject.SetActive(false);
        rightEquipGrid.Close();
        leftEquipGrid.Close();
        base.ClosePage();
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim);
    }
}