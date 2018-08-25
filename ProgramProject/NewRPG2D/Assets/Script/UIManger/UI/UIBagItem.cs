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
        objctPool.gameObject.SetActive(false);

        bagGrid.SetActive(false);

        if (itemType != ItemType.Role)
        {
            PreloadingGrids(h_gridNumber);
        }

        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateEggsEvent, UpdateEggs);//更新鸡蛋
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdatePropsEvent, UpdateProp);//更新道具
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateEquipsEvent, UpdateEquip);//更新装备
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateRolesEvent, UpdateRole);//更新角色

        UIEventManager.instance.AddListener<GainData[]>(UIEventDefineEnum.UpdateRewardMessageEvent, UpdateReward);//探险奖励面板更新
    }

    void OnDestroy()
    {
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateEggsEvent, UpdateEggs);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdatePropsEvent, UpdateProp);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateEquipsEvent, UpdateEquip);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateRolesEvent, UpdateRole);

        UIEventManager.instance.RemoveListener<GainData[]>(UIEventDefineEnum.UpdateRewardMessageEvent, UpdateReward);

    }

    /// <summary>
    /// 预载足够数量的空格等待使用
    /// </summary>
    private void PreloadingGrids(int temp)
    {
        if (transform.GetComponent<GridLayoutGroup>() == null)
        {
            return;
        }
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
        Debug.Log(BagEggData.Instance.eggs.Count);
        GridsControl(BagEggData.Instance.eggs.Count);

        for (int i = 0; i < BagEggData.Instance.eggs.Count; i++)
        {
            if (BagEggData.Instance.eggs[i].ItemNumber == 0)
            {
                BagEggData.Instance.Remove(BagEggData.Instance.eggs[i]);
            }
            if (BagEggData.Instance.eggs.Count > i)
            {
                grids[i].UpdateItem(BagEggData.Instance.eggs[i]);
            }
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
                BagItemData.Instance.Remove(BagItemData.Instance.items[i]);
            //虽然该道具归零了 但是Remove掉 该位置则变为下一个道具 所以下一个道具还是需要添加的
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
            //虽然该道具归零了 但是Remove掉 该位置则变为下一个道具 所以下一个道具还是需要添加的
            if (BagItemData.Instance.items[i].Number == 0)
                BagItemData.Instance.Remove(BagItemData.Instance.items[i]);

            if (BagItemData.Instance.items[i].Melting == PropMeltingType.isTrue)
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
        Debug.Log("更新所有");
        if (itemType != ItemType.Role)
            return;

        GridsControl(BagRoleData.Instance.roles.Count, GridType.Nothing);
        for (int i = 0; i < BagRoleData.Instance.roles.Count; i++)
        {
            grids[i].UpdateItem(BagRoleData.Instance.roles[i]);
        }
    }

    public void UpdateRole(CardData[] datas, GridType type)
    {
        Debug.Log("排除类型");

        if (itemType != ItemType.Role)
        {
            return;
        }

        List<CardData> roles = BagRoleData.Instance.roles;
        GridsControl(roles.Count, type);
        int index = 0;
        for (int i = 0; i < roles.Count; i++)
        {
            int temp = 0;
            if (roles[i].Fighting)
            {
                continue;
            }
            for (int j = 0; j < datas.Length; j++)
            {
                if (roles[i] != datas[j])
                {
                    temp++;
                }
            }
            if (temp >= datas.Length)
            {
                grids[index].UpdateItem(BagRoleData.Instance.roles[i]);
                index++;
            }
        }
        GridsControl(index, type);
    }
    public void UpdateRole(CardData[] datas, int level, bool isHight)
    {
        Debug.Log("排除等级");

        if (itemType != ItemType.Role)
        {
            return;
        }
        List<CardData> roles = BagRoleData.Instance.roles;
        GridsControl(roles.Count, GridType.Explore);
        int index = 0;
        for (int i = 0; i < roles.Count; i++)
        {
            int temp = 0;
            if (roles[i].Fighting)
            {
                continue;
            }
            if (isHight && roles[i].Level < level)
            {
                continue;
            }
            else if (!isHight && roles[i].Level > level)
            {
                continue;
            }
            for (int j = 0; j < datas.Length; j++)
            {
                if (roles[i] != datas[j])
                {
                    temp++;
                }
            }
            if (temp >= datas.Length)
            {
                grids[index].UpdateItem(BagRoleData.Instance.roles[i]);
                index++;
            }
        }
        GridsControl(index, GridType.Explore);
    }
    public void UpdateStore(ItemData[] data)
    {
        if (itemType != ItemType.Prop)
        {
            return;
        }
        GridsControl(data.Length);
        for (int i = 0; i < data.Length; i++)
        {
            grids[i].UpdateItem(data[i]);

            grids[i].itemType = ItemType.Prop;
            grids[i].gridType = GridType.Store;
        }
    }

    public void UpdateReward(GainData[] datas)
    {
        if (itemType != ItemType.Reward)
        {
            return;
        }
        GridsControl(datas.Length);
        for (int i = 0; i < datas.Length; i++)
        {

            switch (datas[i].itemtype)
            {
                case ItemType.Nothing:
                    break;
                case ItemType.Egg:
                    EggData eggData = GameEggData.Instance.GetItem(datas[i].itemId);
                    grids[i].UpdateItem(eggData);
                    break;
                case ItemType.Prop:
                    ItemData itemData = GamePropData.Instance.GetItem(datas[i].itemId);
                    itemData.Number = datas[i].itemNumber;
                    Debug.Log(itemData.Name);
                    grids[i].UpdateItem(itemData);
                    break;
                case ItemType.Equip:
                    EquipData equipData = GameEquipData.Instance.GetItem(datas[i].itemId);
                    grids[i].UpdateItem(equipData);
                    break;
                case ItemType.Role:
                    break;
                default:
                    break;
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
                grids[i].propData = new ItemData();
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
                grids[i].propData = new ItemData();
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
                grids[i].propData = new ItemData();
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
                grids[i].propData = new ItemData();
                grids[i].UpdateItem(-1, ItemType.Nothing);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="number"></param>
    /// <param name="type"></param>
    void GridsControl(int number, GridType type)
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
        for (int i = 0; i < grids.Count; i++)
        {
            grids[i].gridType = type;
        }
    }


}

