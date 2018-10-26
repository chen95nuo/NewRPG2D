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
    private Transform viewGridPoint_1;
    [SerializeField]
    private Transform viewGridPoint_2;

    private UIEquipViewGrid equipGrid_1;
    private UIEquipViewGrid equipGrid_2;

    public Button btn_Close;

    private void Awake()
    {
        instance = this;

        btn_Close.onClick.AddListener(ClosePage);

        GameObject go = Instantiate(viewGrid, viewGridPoint_1) as GameObject;
        equipGrid_1 = go.GetComponent<UIEquipViewGrid>();
        go = Instantiate(viewGrid, viewGridPoint_2);
        equipGrid_2 = go.GetComponent<UIEquipViewGrid>();
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
        viewGridPoint_2.gameObject.SetActive(false);

        if (equipData.roleData == null)
        {
            equipGrid_1.UpdateInfo(equipData.equipData, 1, equipData.roleData);
        }
        else
        {
            equipGrid_1.UpdateInfo(equipData.equipData, 3, equipData.roleData);
        }

    }

    private void UpdateInfo(UIEquipInfoHelper_1 equipInfoData)
    {
        viewGridPoint_2.gameObject.SetActive(true);
        if (equipInfoData.bagEquipData == null)
        {
            viewGridPoint_2.gameObject.SetActive(false);
            equipGrid_1.UpdateInfo(equipInfoData.roleEquipData, 2, equipInfoData.roleData);
        }
        else
        {
            equipGrid_2.UpdateInfo(equipInfoData.roleEquipData, 0, null);
            equipGrid_1.UpdateInfo(equipInfoData.bagEquipData, 3, equipInfoData.roleData);
        }
    }

    public override void ClosePage()
    {
        viewGridPoint_2.gameObject.SetActive(false);
        base.ClosePage();
    }
}