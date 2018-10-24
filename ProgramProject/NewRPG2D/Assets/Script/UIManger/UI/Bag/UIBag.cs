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
        btn_back.onClick.AddListener(ClosePage);
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            btn_AllType[i].onClick.AddListener(ChickBagType);
        }
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
        Debug.LogError("没有找到对应按钮");
    }
}
