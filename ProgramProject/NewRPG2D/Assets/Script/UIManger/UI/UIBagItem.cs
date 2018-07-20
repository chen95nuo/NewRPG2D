using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagItem : MonoBehaviour
{

    public List<UIBagGrid> grids;
    public List<UIBagGrid> gridsSpare;//备用的格子
    public Transform objctPool;
    private int gridsFirstCount;
    public GameObject bagGrid;
    public ItemType itemType;
    public int h_gridNumber = 4;//每行几个

    void Awake()
    {
        bagGrid = GetComponentInChildren<UIBagGrid>().gameObject;

        bagGrid.SetActive(false);

        if (itemType != ItemType.Role)
        {
            PreloadingGrids(h_gridNumber);
        }
        else if (itemType == ItemType.Role)
        {
            UpdateRole();
        }


        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateEggsEvent, UpdateEggs);//更新鸡蛋
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdatePropsEvent, UpdateProp);//更新道具
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateEquipsEvent, UpdateEquip);//更新装备
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateRolesEvent, UpdateRole);//更新角色
    }

    void Start()
    {

    }

    void OnDestroy()
    {
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateEggsEvent, UpdateEggs);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdatePropsEvent, UpdateProp);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateEquipsEvent, UpdateEquip);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateRolesEvent, UpdateRole);
    }

    /// <summary>
    /// 预载足够数量的空格等待使用
    /// </summary>
    private void PreloadingGrids(int temp)
    {
        float index = transform.GetComponent<GridLayoutGroup>().cellSize.y + transform.GetComponent<GridLayoutGroup>().spacing.y;

        float hight = 0;
        while (hight < transform.parent.parent.GetComponent<RectTransform>().sizeDelta.y)
        {
            GameObject go = Instantiate(bagGrid, transform) as GameObject;
            go.SetActive(true);
            go.name = bagGrid.name;
            grids.Add(go.GetComponent<UIBagGrid>());
            if (grids.Count % temp == 0)
            {
                hight += index;
            }
        }
        gridsFirstCount = grids.Count;
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
            if (BagItemData.Instance.items[i].Number == 0)
            {
                BagItemData.Instance.Remove(BagItemData.Instance.items[i]);
                //虽然该道具归零了 但是Remove掉 该位置则变为下一个道具 所以下一个道具还是需要添加的
                grids[i].UpdateItem(BagItemData.Instance.items[i]);
            }
            else
                grids[i].UpdateItem(BagItemData.Instance.items[i]);
        }
    }
    public void UpdateProp(PropMeltingType melting)
    {
        if (itemType != ItemType.Prop)
            return;

        GridsControl(BagItemData.Instance.items.Count);
        int index = 0;
        for (int i = 0; i < BagItemData.Instance.items.Count; i++)
        {
            if (BagItemData.Instance.items[i].Number == 0)
            {
                BagItemData.Instance.Remove(BagItemData.Instance.items[i]);
                //虽然该道具归零了 但是Remove掉 该位置则变为下一个道具 所以下一个道具还是需要添加的
                if (BagItemData.Instance.items[i].Melting == melting)
                {
                    grids[index].UpdateItem(BagItemData.Instance.items[i]);
                    index++;
                }
            }
            else if (BagItemData.Instance.items[i].Melting == melting)
            {
                grids[index].UpdateItem(BagItemData.Instance.items[i]);
                index++;
            }
        }
        GridsControl(index);
    }
    public void UpdateProp(PropType propType, PropType propType1)
    {
        if (itemType != ItemType.Prop)
            return;

        GridsControl(BagItemData.Instance.items.Count);
        int index = 0;
        for (int i = 0; i < BagItemData.Instance.items.Count; i++)
        {
            if (BagItemData.Instance.items[i].Number == 0)
            {
                BagItemData.Instance.Remove(BagItemData.Instance.items[i]);
                //虽然该道具归零了 但是Remove掉 该位置则变为下一个道具 所以下一个道具还是需要添加的
                if (BagItemData.Instance.items[i].PropType == propType || BagItemData.Instance.items[i].PropType == propType1)
                {
                    grids[index].UpdateItem(BagItemData.Instance.items[i]);
                    index++;
                }
            }
            else if (BagItemData.Instance.items[i].PropType == propType || BagItemData.Instance.items[i].PropType == propType1)
            {
                grids[index].UpdateItem(BagItemData.Instance.items[i]);
                index++;
            }
        }
        GridsControl(index);
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
    public void UpdateEquip(EquipType type)
    {
        if (itemType != ItemType.Equip)
            return;

        GridsControl(BagEquipData.Instance.equips.Count);
        int index = 0;
        for (int i = 0; i < BagEquipData.Instance.equips.Count; i++)
        {
            if (BagEquipData.Instance.equips[i].EquipType == type)
            {
                grids[index].UpdateItem(BagEquipData.Instance.equips[i]);
                index++;
            }
        }
        GridsControl(index);
    }
    public void UpdateRole()
    {
        if (itemType != ItemType.Role)
            return;

        GridsControl(BagRoleData.Instance.roles.Count, ItemType.Role);

        for (int i = 0; i < BagRoleData.Instance.roles.Count; i++)
        {
            grids[i].UpdateItem(BagRoleData.Instance.roles[i]);
        }
    }

    public void UpdateRole(CardData data, CardData[] datas)
    {
        if (itemType != ItemType.Role)
        {
            return;
        }

        GridsControl(BagRoleData.Instance.roles.Count, ItemType.Role);
        int index = 0;
        for (int i = 0; i < BagRoleData.Instance.roles.Count; i++)
        {
            if (BagRoleData.Instance.roles[i] != data
                && BagRoleData.Instance.roles[i] != datas[0]
                && BagRoleData.Instance.roles[i] != datas[1]
                && BagRoleData.Instance.roles[i] != datas[2])
            {
                grids[index].UpdateItem(BagRoleData.Instance.roles[i]);
                index++;
            }
        }
        GridsControl(index, ItemType.Role);
    }
    public void UpdateStore(ItemData[] data)
    {
        if (itemType != ItemType.Store)
        {
            return;
        }
        GridsControl(data.Length);
        for (int i = 0; i < data.Length; i++)
        {
            grids[i].UpdateItem(data[i]);

            if (i % 3 == 0)
            {
                grids[i].storeGrid.border.gameObject.SetActive(true);
            }
            else
            {
                grids[i].storeGrid.border.gameObject.SetActive(false);
            }
            grids[i].itemType = ItemType.Store;
            grids[i].gridType = GridType.Store;
        }
    }

    void GridsControl(int number, ItemType type)
    {
        if (number > grids.Count + gridsSpare.Count)
        {
            if (gridsSpare.Count > 0)
            {
                for (int i = 0; i < gridsSpare.Count; i++)
                {
                    gridsSpare[0].transform.SetParent(transform);
                    grids.Add(gridsSpare[0]);
                    gridsSpare.Remove(gridsSpare[0]);
                }
            }
            while (grids.Count < number)
            {
                GameObject go = Instantiate(bagGrid, transform) as GameObject;
                go.name = bagGrid.name;
                go.SetActive(true);
                grids.Add(go.GetComponent<UIBagGrid>());

            }
        }
        else if (number < grids.Count + gridsSpare.Count)
        {
            if (number < grids.Count)
            {
                for (int i = grids.Count; i > number; i--)
                {
                    grids[i - 1].transform.SetParent(objctPool);
                    gridsSpare.Add(grids[i - 1]);
                    grids.Remove(grids[i - 1]);
                }
            }
            else
            {
                int data = number - grids.Count;
                for (int i = 0; i < data; i++)
                {
                    gridsSpare[0].transform.SetParent(transform);
                    grids.Add(gridsSpare[0]);
                    gridsSpare.Remove(gridsSpare[0]);
                }
            }
        }
        else
        {
            if (number > grids.Count)
            {
                int data = number - grids.Count;
                for (int i = 0; i < data; i++)
                {
                    gridsSpare[0].transform.SetParent(transform);
                    grids.Add(gridsSpare[0]);
                    gridsSpare.Remove(gridsSpare[0]);
                }
            }
            else
            {

            }
        }
    }


    /// <summary>
    /// 控制背包格子数量
    /// </summary>
    /// <param name="number"></param>
    /// <param name="isReset"></param>
    void GridsControl(int number)
    {
        int index = 0;
        if (number % h_gridNumber == 0)
        {
            index = number;
        }
        else
        {
            index = number + (h_gridNumber - (number % h_gridNumber)); //计算整行
        }
        if (index > gridsFirstCount && index > grids.Count + gridsSpare.Count)
        {
            //需求超过默认，需求超过总数量，需要生成
            if (gridsSpare.Count > 0)
            {
                for (int i = gridsSpare.Count; i > 0; i--)
                {
                    gridsSpare[0].transform.SetParent(transform);
                    grids.Add(gridsSpare[0]);
                    gridsSpare.Remove(gridsSpare[0]);
                }
            }
            while (grids.Count < index)
            {
                GameObject go = Instantiate(bagGrid, transform) as GameObject;
                go.name = bagGrid.name;
                go.SetActive(true);
                grids.Add(go.GetComponent<UIBagGrid>());
            }

            for (int i = number; i < grids.Count; i++)
            {
                grids[i].UpdateItem(-1, ItemType.Nothing);
            }
        }
        else if (grids.Count + gridsSpare.Count >= index && grids.Count < index)
        {
            //总数大于需求，但是需求中有一部分被隐藏，需要隐藏超过需求的，显示需求以内的

            int data = index - grids.Count;
            for (int i = 0; i < data; i++)
            {
                gridsSpare[0].transform.SetParent(transform);
                grids.Add(gridsSpare[0]);
                gridsSpare.Remove(gridsSpare[0]);
            }
            for (int i = number; i < grids.Count; i++)
            {
                grids[i].UpdateItem(-1, ItemType.Nothing);
            }
        }
        else if (grids.Count + gridsSpare.Count >= index && grids.Count > index && index > gridsFirstCount)
        {
            for (int i = grids.Count; i > index; i--)
            {
                grids[i - 1].transform.SetParent(objctPool);
                gridsSpare.Add(grids[i - 1]);
                grids.Remove(grids[i - 1]);
            }
            for (int i = number; i < grids.Count; i++)
            {
                grids[i].UpdateItem(-1, ItemType.Nothing);
            }
        }
        else if (index <= gridsFirstCount)
        {

            //需求没有超过默认
            if (grids.Count > gridsFirstCount)
            {
                for (int i = grids.Count; i > gridsFirstCount; i--)
                {
                    grids[i - 1].transform.SetParent(objctPool);
                    gridsSpare.Add(grids[i - 1]);
                    grids.Remove(grids[i - 1]);
                }
            }
            for (int i = number; i < grids.Count; i++)
            {
                grids[i].UpdateItem(-1, ItemType.Nothing);
            }
        }
    }
}

