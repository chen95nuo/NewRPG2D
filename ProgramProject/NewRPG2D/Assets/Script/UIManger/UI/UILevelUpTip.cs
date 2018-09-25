using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILevelUpTip : TTUIPage
{

    public Transform tipGridPoint;
    public GameObject tip;
    private RoomMgr roomData;
    private Camera cam;
    private List<UILevelUpTipGrid> timeTextGrids = new List<UILevelUpTipGrid>();
    private List<UILevelUpTipGrid> outGrids = new List<UILevelUpTipGrid>();
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
            timeTextGrids.Add(data);
            return timeTextGrids.IndexOf(data);
        }
        else
        {
            outGrids[0].transform.position = Vector3.zero;
            outGrids[0].ts = roomData.disTip.transform;
            outGrids[0].UpdatePos();
            timeTextGrids.Add(outGrids[0]);
            int index = timeTextGrids.IndexOf(outGrids[0]);
            outGrids.RemoveAt(0);
            return index;
        }
    }

    public void RemoveLister(int index)
    {
        outGrids.Add(timeTextGrids[index]);
        timeTextGrids[index].transform.position = Vector3.back * 1000;
        timeTextGrids.RemoveAt(index);
    }

    public void SetCamMove()
    {


        for (int i = 0; i < timeTextGrids.Count; i++)
        {

        }
    }

    public void UpdateTime(int time, int index)
    {
        timeTextGrids[index].UpdateTime(time);
    }
}
