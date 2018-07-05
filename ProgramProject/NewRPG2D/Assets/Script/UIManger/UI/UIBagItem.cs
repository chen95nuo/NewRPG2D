using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagItem : MonoBehaviour
{

    public List<UIBagGrid> grids;
    public GameObject bagGrid;

    void Awake()
    {
        bagGrid = GetComponentInChildren<UIBagGrid>().gameObject;

        grids.Add(bagGrid.GetComponent<UIBagGrid>());

        Debug.Log(transform.parent.parent.GetComponent<RectTransform>().sizeDelta.y);
        Debug.Log(transform.GetComponent<RectTransform>().sizeDelta.y);
        float index = transform.GetComponent<GridLayoutGroup>().cellSize.y + transform.GetComponent<GridLayoutGroup>().spacing.y;


        //预载足够数量的空格等待使用
        float hight = 0;
        while (hight < transform.parent.parent.GetComponent<RectTransform>().sizeDelta.y)
        {
            GameObject go = Instantiate(bagGrid, transform) as GameObject;
            grids.Add(go.GetComponent<UIBagGrid>());
            if (grids.Count % 4 == 0)
            {
                hight += index;
            }
        }


        //BagEggData.Instance.updateEggsEvent += UpdateEgg;//更新蛋的界面

    }

    void Start()
    {
        UpdateProp();
    }

    void OnDestroy()
    {
        //BagEggData.Instance.updateEggsEvent -= UpdateEgg;

    }

    /// <summary>
    /// 更新蛋
    /// </summary>
    void UpdateEgg()
    {

    }

    /// <summary>
    /// 更新道具
    /// </summary>
    void UpdateProp()
    {

    }

    /// <summary>
    /// 更新装备
    /// </summary>
    void UpdateEquip()
    {

    }

}

