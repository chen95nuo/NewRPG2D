using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILevelUpTip : TTUIPage
{
    public Transform tipGridPoint;
    public GameObject tip;
    private Canvas canvas;

    private Dictionary<int, UILevelUpTipGrid> levelUpGrids = new Dictionary<int, UILevelUpTipGrid>();
    private List<UILevelUpTipGrid> outGrids = new List<UILevelUpTipGrid>();

    private void Awake()
    {
        canvas = TTUIRoot.Instance.GetComponent<Canvas>();
    }
    public override void Show(object mData)
    {
        base.Show(mData);
        LocalBuildingData data = mData as LocalBuildingData;
        UpdateTime(data);
    }

    private void UpdateTime(LocalBuildingData data)
    {
        if (levelUpGrids.ContainsKey(data.id))
        {
            if (data.leftTime == -1)//如果是训练结束发来的消息 删除当前
            {
                RemoveLister(data);
                return;
            }
            levelUpGrids[data.id].GetInfo(data, canvas);
        }
        else
        {
            if (data.leftTime == -1)
            {
                Debug.LogError("错误消息");
            }
            AddLister(data);
        }
    }

    private void AddLister(LocalBuildingData buildingData)
    {
        UILevelUpTipGrid data;
        if (outGrids.Count <= 0)
        {
            GameObject go = Instantiate(tip, tipGridPoint) as GameObject;
            data = go.GetComponent<UILevelUpTipGrid>();
            data.GetInfo(buildingData, canvas);
        }
        else
        {
            data = outGrids[0];
            data.gameObject.SetActive(true);
            data.GetInfo(buildingData, canvas);
            outGrids.RemoveAt(0);
        }
        levelUpGrids.Add(buildingData.id, data);

    }

    public void RemoveLister(LocalBuildingData buildingData)
    {
        if (levelUpGrids.ContainsKey(buildingData.id))
        {
            outGrids.Add(levelUpGrids[buildingData.id]);
            levelUpGrids[buildingData.id].gameObject.SetActive(false);
            levelUpGrids.Remove(buildingData.id);
        }
    }
}
