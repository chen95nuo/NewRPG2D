using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGainTip : MonoBehaviour
{

    public UIGainGrid grid;
    public Text addGoid;
    public Button btn_back;

    private List<UIGainGrid> grids;
    private bool isNext = false;
    private CardGainData[] cardGainData;

    private void Awake()
    {
        grid.gameObject.SetActive(false);

        btn_back.onClick.AddListener(ChickBack);

        grids = new List<UIGainGrid>();
        UIEventManager.instance.AddListener<GainData[]>(UIEventDefineEnum.UpdateGainTipEvent, UpdateGrids);
        UIEventManager.instance.AddListener<CardGainData[]>(UIEventDefineEnum.UpdateGainTipEvent, UpdateCardExp);
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<GainData[]>(UIEventDefineEnum.UpdateGainTipEvent, UpdateGrids);
        UIEventManager.instance.RemoveListener<CardGainData[]>(UIEventDefineEnum.UpdateGainTipEvent, UpdateCardExp);
    }

    public void UpdateGrids(GainData[] gainData)
    {
        GridsControl(gainData.Length);
        addGoid.text = "金币 + " + gainData[0].addGoin;

        for (int i = 0; i < gainData.Length; i++)
        {
            switch (gainData[i].itemtype)
            {
                case ItemType.Nothing:
                    grids[i].gameObject.SetActive(false);
                    break;
                case ItemType.Egg:
                    EggData eggData = GameEggData.Instance.GetItem(gainData[i].itemId);
                    grids[i].UpdateItem(eggData);
                    break;
                case ItemType.Prop:
                    ItemData itemData = GamePropData.Instance.GetItem(gainData[i].itemId);
                    itemData.Number = gainData[i].itemNumber;
                    grids[i].UpdateItem(itemData);
                    break;
                case ItemType.Equip:
                    EquipData equipData = GameEquipData.Instance.GetItem(gainData[i].itemId);
                    grids[i].UpdateItem(equipData);
                    break;
                default:
                    break;
            }
        }
    }

    public void UpdateCardExp(CardGainData[] datas)
    {
        cardGainData = datas;
        isNext = true;
    }

    public void ChickBack()
    {
        if (isNext)
        {
            //打开角色经验面板
            TinyTeam.UI.TTUIPage.ShowPage<UICardAddExpPage>();
            UIEventManager.instance.SendEvent<CardGainData[]>(UIEventDefineEnum.UpdateGainTipEvent, cardGainData);
            isNext = false;
        }
        else
        {
            //关闭当前面板
            TinyTeam.UI.TTUIPage.ClosePage<UIGainTipPage>();
        }
    }

    private void GridsControl(int number)
    {
        if (number > grids.Count)
        {
            for (int i = 0; i < grids.Count; i++)
            {
                grids[i].gameObject.SetActive(true);
            }
            int index = grids.Count;
            for (int i = index; i < number; i++)
            {
                GameObject go = Instantiate(grid.gameObject, grid.transform.parent.transform) as GameObject;
                go.SetActive(true);
                grids.Add(go.GetComponent<UIGainGrid>());
            }
        }
        else if (number < grids.Count)
        {
            for (int i = number; i < grids.Count; i++)
            {
                grids[i].gameObject.SetActive(false);
            }
        }

    }

}
