using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILevelUpTip : TTUIPage
{
    public static UILevelUpTip instance;

    public Transform tipGridPoint;
    public GameObject tip;
    private Dictionary<int, UILevelUpTipGrid> timeTextGrids = new Dictionary<int, UILevelUpTipGrid>();
    private List<UILevelUpTipGrid> outGrids = new List<UILevelUpTipGrid>();
    private int dicIndex = 0;
    private Canvas canvas;

    public int DicIndex
    {
        get
        {
            return dicIndex++;
        }
    }

    private void Awake()
    {
        instance = this;
        canvas = TTUIRoot.Instance.GetComponent<Canvas>();
    }
    public override void Show(object mData)
    {
        base.Show(mData);
    }

    public int AddLister(int roomID)
    {
        LocalBuildingData roomData = ChickPlayerInfo.instance.GetBuilding(roomID);
        if (roomData.currentRoom == null)
        {
            return -1;
        }
        int index = 0;
        index = DicIndex;
        if (outGrids.Count <= 0)
        {
            GameObject go = Instantiate(tip, tipGridPoint) as GameObject;
            UILevelUpTipGrid data = go.GetComponent<UILevelUpTipGrid>();
            Transform ts = roomData.currentRoom.disTip.transform;
            data.GetInfo(ts, canvas);
            timeTextGrids.Add(index, data);
        }
        else
        {
            Transform ts = roomData.currentRoom.disTip.transform;
            outGrids[0].GetInfo(ts, canvas);
            outGrids[0].gameObject.SetActive(true);
            timeTextGrids.Add(index, outGrids[0]);
            outGrids.RemoveAt(0);
        }
        return index;
    }

    public void RemoveLister(int index)
    {
        if (timeTextGrids.ContainsKey(index))
        {
            outGrids.Add(timeTextGrids[index]);
            timeTextGrids[index].gameObject.SetActive(false);
            timeTextGrids.Remove(index);
        }
    }

    public void UpdateTime(LevelUPHelper data)
    {
        if (timeTextGrids.ContainsKey(data.tipID))
        {
            timeTextGrids[data.tipID].UpdateTime(data);
            return;
        }
        int newTip = AddLister(data.roomID);
        ChickPlayerInfo.instance.ChangeBuildNumber(data, newTip);
        timeTextGrids[newTip].UpdateTime(data);
    }
}
