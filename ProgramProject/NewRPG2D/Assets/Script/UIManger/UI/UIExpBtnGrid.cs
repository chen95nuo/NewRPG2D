using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExpBtnGrid : MonoBehaviour
{
    public Button expBtn;
    public Text mapName;
    public Text needTime;
    public Image map;
    public UIBagGrid bagGrid;
    [System.NonSerialized]
    public ExploreData expData;
    private UIBagGrid[] bagGrids;

    public void UpdateExpBtnGrid(ExploreData data)
    {
        expData = data;
        mapName.text = data.Name;
        SystemTime.insatnce.TimeNormalized(data.NeedTime, needTime);
        //地图图片 无
        bagGrids = new UIBagGrid[data.DroppingId.Length];
        bagGrid.gameObject.SetActive(false);
        Transform ts = bagGrid.transform.parent.transform;
        for (int i = 0; i < bagGrids.Length; i++)
        {
            GameObject go = Instantiate(bagGrid.gameObject, ts) as GameObject;
            bagGrids[i] = go.GetComponent<UIBagGrid>();
            bagGrids[i].gameObject.SetActive(true);
            ItemData item = GamePropData.Instance.GetItem(data.DroppingId[i]);
            bagGrids[i].UpdateItem(item);
        }
    }
}
