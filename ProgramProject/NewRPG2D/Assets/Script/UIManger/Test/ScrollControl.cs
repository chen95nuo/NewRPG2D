using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{

    public InfinityGridLayoutGroup infinity;

    int amount = 6;

    // Use this for initialization
    public void UpdateInfo(int amount)
    {
        ////初始化数据列表;
        //infinity = transform.Find("Panel_Scroll/Panel_Grid").GetComponent<InfinityGridLayoutGroup>();
        infinity.SetAmount(amount);
        infinity.updateChildrenCallback = UpdateChildrenCallback;
    }

    void UpdateChildrenCallback(int index, Transform trans)
    {
        Debug.Log(index);

        //Text text = trans.Find("Text").GetComponent<Text>();
        //text.text = index.ToString();
    }
}