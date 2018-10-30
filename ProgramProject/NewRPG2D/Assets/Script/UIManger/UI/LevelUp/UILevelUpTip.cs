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
    private RoomMgr roomData;
    private Dictionary<int, UILevelUpTipGrid> timeTextGrids = new Dictionary<int, UILevelUpTipGrid>();
    private List<UILevelUpTipGrid> outGrids = new List<UILevelUpTipGrid>();
    private int dicIndex = 0;

    private void Awake()
    {
        instance = this;
    }
    public override void Show(object mData)
    {
        base.Show(mData);
        roomData = mData as RoomMgr;
    }

    public int AddLister()
    {
        if (outGrids.Count <= 0)
        {
            GameObject go = Instantiate(tip, tipGridPoint) as GameObject;
            UILevelUpTipGrid data = go.GetComponent<UILevelUpTipGrid>();
            Canvas canvas = TTUIRoot.Instance.GetComponent<Canvas>();
            Transform ts = roomData.disTip.transform;
            data.GetInfo(ts, canvas);
            timeTextGrids.Add(dicIndex, data);
        }
        else
        {
            outGrids[0].transform.position = Vector3.zero;
            outGrids[0].ts = roomData.disTip.transform;
            outGrids[0].UpdatePos();
            timeTextGrids.Add(dicIndex, outGrids[0]);
            outGrids.RemoveAt(0);
        }
        dicIndex++;
        return dicIndex - 1;
    }

    public void RemoveLister(int index)
    {
        outGrids.Add(timeTextGrids[index]);
        timeTextGrids[index].transform.position = Vector3.back * 1000;
        timeTextGrids.Remove(index);
    }

    public bool UpdateTime(int time, int index)
    {
        if (timeTextGrids.ContainsKey(index))
        {
            timeTextGrids[index].UpdateTime(time);
            return true;
        }
        return false;
    }
}
