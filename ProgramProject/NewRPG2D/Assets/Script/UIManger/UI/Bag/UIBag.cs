using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using UnityEngine.EventSystems;

public class ItemHelper
{
    public int instanceId;
    public ItemType itemType;
    public ItemHelper(int instanceId, ItemType itemType)
    {
        this.instanceId = instanceId;
        this.itemType = itemType;
    }
}
public class UIBag : TTUIPage
{
    #region GetButton
    public Button btn_back;
    public Button[] btn_AllType;
    #endregion
    public ScrollControl sc;
    public List<ItemHelper> items = new List<ItemHelper>();

    private int currentBtnNumb = 0;

    private void Awake()
    {
        HallEventManager.instance.AddListener<EquipmentRealProperty>(HallEventDefineEnum.ShowEquipInfo, BagChickRoleEquip);
        HallEventManager.instance.AddListener(HallEventDefineEnum.RefreshBagUI, ChickBagType);

        btn_back.onClick.AddListener(ClosePage);
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            btn_AllType[i].onClick.AddListener(ChickBagType);
        }
        btn_AllType[currentBtnNumb].interactable = false;
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<EquipmentRealProperty>(HallEventDefineEnum.ShowEquipInfo, BagChickRoleEquip);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.RefreshBagUI, ChickBagType);
    }



    private void ChickBagType()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            if (btn_AllType[i].gameObject == go)
            {
                btn_AllType[currentBtnNumb].interactable = true;
                btn_AllType[i].interactable = false;
                sc.UpdateInfo((BagType)i);
                currentBtnNumb = i;
                return;
            }
        }
        sc.UpdateInfo((BagType)currentBtnNumb);
    }

    private void BagChickRoleEquip(EquipmentRealProperty equipData)
    {
        UIEquipInfoHelper data_1 = new UIEquipInfoHelper(equipData, null);
        UIPanelManager.instance.ShowPage<UIEquipView>(data_1);
    }
}
