using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;
using Assets.Script.Battle.BattleData;
using Assets.Script.UIManger;

public class UIItemGrid : MonoBehaviour
{
    public Image Icon;
    public Image BG;
    public Text txt_Num;
    public Button btn_Grid;


    private ItemType gridType;
    private TreasureBox boxData;
    private PropData propData;
    private EquipmentRealProperty equipData;

    private void Awake()
    {
        btn_Grid.onClick.AddListener(ChickClick);
    }

    public void UpdateInfo(int itemId, int minCount, int MaxCount = 0)
    {
        gridType = ItemType.All;
        PropData propData = PropDataMgr.instance.GetDataByItemId<PropData>(itemId);
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(propData.SpriteName);
        Debug.Log("MinCount" + minCount);
        Debug.Log("MaxCount" + MaxCount);
        if (minCount != MaxCount && MaxCount > 0)
        {
            BG.gameObject.SetActive(true);
            txt_Num.text = minCount.ToString("#0") + "~" + MaxCount.ToString("#0");
        }
        else if (minCount > 0)
        {
            BG.gameObject.SetActive(true);
            txt_Num.text = minCount.ToString("#0");
        }
        else
        {
            BG.gameObject.SetActive(false);
        }
    }

    public void UpdateInfo(ItemGridHelp data)
    {
        BG.gameObject.SetActive(false);
        gridType = data.type;
        switch (data.type)
        {
            case ItemType.All:
                break;
            case ItemType.Box:
                boxData = TreasureBoxDataMgr.instance.GetXmlDataByItemId<TreasureBox>(data.itemId);
                Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(boxData.SpriteName);
                break;
            case ItemType.Prop:
                propData = PropDataMgr.instance.GetDataByItemId<PropData>(data.itemId);
                Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(propData.SpriteName);
                break;
            case ItemType.Equip:
                equipData = EquipmentMgr.instance.GetEquipmentByEquipId(data.itemId);
                Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(equipData.SpriteName);
                break;
            default:
                break;
        }
    }

    private void ChickClick()
    {
        Debug.Log("");
        switch (gridType)
        {
            case ItemType.All:
                return;
            case ItemType.Box:
                UIPanelManager.instance.ShowPage<UIBoxInfo>(boxData);
                break;
            case ItemType.Prop:
                UIPanelManager.instance.ShowPage<UIEquipView>(propData);
                break;
            case ItemType.Equip:
                UIPanelManager.instance.ShowPage<UIEquipView>(equipData);
                break;
            default:
                break;
        }
    }
}
