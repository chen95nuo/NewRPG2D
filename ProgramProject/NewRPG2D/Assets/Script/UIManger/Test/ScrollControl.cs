using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{

    public InfinityGridLayoutGroup infinity;

    //int amount = 6;
    public List<ItemHelper> allItem = new List<ItemHelper>();
    // Use this for initialization
    public void UpdateInfo(List<ItemHelper> items)
    {
        int amount = items.Count;
        ////初始化数据列表;
        infinity.SetAmount(amount);
        infinity.updateChildrenCallback = UpdateChildrenCallback;
    }

    void UpdateChildrenCallback(int index, Transform trans)
    {
        Debug.Log(index);

        UIItemGrid grid = trans.GetComponent<UIItemGrid>();
        if (grid == null)
        {
            return;
        }
        grid.UpdateInfo(allItem[index]);
    }
}