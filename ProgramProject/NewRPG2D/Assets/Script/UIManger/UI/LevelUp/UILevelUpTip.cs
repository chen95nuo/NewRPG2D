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
        if (outGrids.Count <= 0)
        {
            GameObject go = Instantiate(tip, tipGridPoint) as GameObject;
            UILevelUpTipGrid data = go.GetComponent<UILevelUpTipGrid>();
            Transform ts = roomData.currentRoom.disTip.transform;
            data.GetInfo(ts, canvas);
            timeTextGrids.Add(dicIndex, data);
        }
        else
        {
            Transform ts = roomData.currentRoom.disTip.transform;
            outGrids[0].GetInfo(ts, canvas);
            timeTextGrids.Add(dicIndex, outGrids[0]);
            outGrids.RemoveAt(0);
        }
        dicIndex++;
        return dicIndex - 1;
    }

    public void RemoveLister(int index)
    {
        if (timeTextGrids.ContainsKey(index))
        {
            outGrids.Add(timeTextGrids[index]);
            timeTextGrids[index].transform.position = Vector3.back * 1000;
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
    }
}
