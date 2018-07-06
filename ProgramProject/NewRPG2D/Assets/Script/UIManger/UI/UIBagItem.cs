using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagItem : MonoBehaviour
{

    public List<UIBagGrid> grids;
    private int gridsFirstCount;
    public GameObject bagGrid;
    public ItemType itemType;

    void Awake()
    {
        bagGrid = GetComponentInChildren<UIBagGrid>().gameObject;

        bagGrid.SetActive(false);

        float index = transform.GetComponent<GridLayoutGroup>().cellSize.y + transform.GetComponent<GridLayoutGroup>().spacing.y;


        //预载足够数量的空格等待使用
        float hight = 0;
        while (hight < transform.parent.parent.GetComponent<RectTransform>().sizeDelta.y)
        {
            GameObject go = Instantiate(bagGrid, transform) as GameObject;
            go.SetActive(true);
            grids.Add(go.GetComponent<UIBagGrid>());
            if (grids.Count % 4 == 0)
            {
                hight += index;
            }
        }
        gridsFirstCount = grids.Count;


        BagEggData.Instance.updateEggsEvent += UpdateEggs;//更新蛋的界面
        BagItemData.Instance.updateItemsEvent += UpdateProp;//更新道具
        BagEquipData.Instance.updateEquipsEvent += UpdateEquip;//更新装备
    }

    void Start()
    {

    }

    void OnDestroy()
    {
        BagEggData.Instance.updateEggsEvent -= UpdateEggs;
        BagItemData.Instance.updateItemsEvent -= UpdateProp;
        BagEquipData.Instance.updateEquipsEvent -= UpdateEquip;
    }



    public void UpdateEggs()
    {
        if (itemType != ItemType.Egg)
            return;

        GridsControl(BagEggData.Instance.eggs.Count);

        for (int i = 0; i < BagEggData.Instance.eggs.Count; i++)
        {
            grids[i].UpdateItem(BagEggData.Instance.eggs[i]);
        }
    }
    public void UpdateProp()
    {
        if (itemType != ItemType.Prop)
            return;

        GridsControl(BagItemData.Instance.items.Count);

        for (int i = 0; i < BagItemData.Instance.items.Count; i++)
        {
            grids[i].UpdateItem(BagItemData.Instance.items[i]);
        }
    }
    public void UpdateEquip()
    {
        if (itemType != ItemType.Equip)
            return;

        GridsControl(BagEquipData.Instance.equips.Count);

        for (int i = 0; i < BagEquipData.Instance.equips.Count; i++)
        {
            grids[i].UpdateItem(BagEquipData.Instance.equips[i]);
        }
    }


    /// <summary>
    /// 控制背包格子数量
    /// </summary>
    /// <param name="number">道具数量</param>
    void GridsControl(int number)
    {
        if (number > gridsFirstCount && number > grids.Count)
        {
            while (grids.Count < number + (4 - (number % 4)))
            {
                GameObject go = Instantiate(bagGrid, transform) as GameObject;
                go.SetActive(true);
                grids.Add(go.GetComponent<UIBagGrid>());
            }
            for (int i = number; i < grids.Count; i++)
            {
                grids[i].UpdateItem(-1, ItemType.Nothing);
            }
        }
        else if (number <= gridsFirstCount)
        {
            if (grids.Count > gridsFirstCount)
            {
                for (int i = grids.Count; i > gridsFirstCount; i--)
                {
                    Destroy(grids[i - 1].gameObject);
                    grids.RemoveAt(i - 1);
                }
            }
            for (int i = number; i < grids.Count; i++)
            {
                grids[i].UpdateItem(-1, ItemType.Nothing);
            }
        }
        else if (number > gridsFirstCount && number < grids.Count)
        {
            for (int i = grids.Count; i > number; i--)
            {
                Destroy(grids[i - 1].gameObject);
                grids.RemoveAt(i - 1);
            }
            while (grids.Count < number + (4 - (number % 4)))
            {
                GameObject go = Instantiate(bagGrid, transform) as GameObject;
                go.SetActive(true);
                grids.Add(go.GetComponent<UIBagGrid>());
            }
        }
    }
}

