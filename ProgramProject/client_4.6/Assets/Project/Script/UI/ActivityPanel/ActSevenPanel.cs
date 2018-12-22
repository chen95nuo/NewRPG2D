using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActSevenPanel : MonoBehaviour {
	
	public GameObject rewardPanel;
    public GameObject objSevenPanel;
    public GameObject objGrid;
    public UILabel labDesc;

    LoginSevenDayResJson sevenResJson;
    //int activityId;
    ActTopIcon topIcon;

    List<ActSevenCell> panelCells = new List<ActSevenCell>();

    Vector4 dragPanelClip;  //draggable panel clip

    const string ActSevenCellPath = "Prefabs/UI/ActivityPanel/actSevenCell";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(LoginSevenDayResJson sevenResJson, int activityId, ActTopIcon topIcon)
    {
		this.rewardPanel.SetActive(false);
        this.sevenResJson = sevenResJson;
        //this.activityId = activityId;
        this.topIcon = topIcon;

        setDragPanelPos();

        labDesc.text = TextsData.getData(633).chinese;

        //旧数据服务器会发id=0,过滤一下//
        for (int i = 0; i < sevenResJson.s.Count; )
        {
            if (!SevenDaysData.getAllData().ContainsKey(sevenResJson.s[i].id))
                sevenResJson.s.RemoveAt(i);
            else
                i++;
        }

        panelCells.Clear();
        Object objRes = Resources.Load(ActSevenCellPath);
        for (int i = 0; i < SevenDaysData.getDayNum(); i++)
        {
            GameObject obj = Instantiate(objRes) as GameObject;
            obj.transform.parent = objGrid.transform;
            obj.transform.localScale = Vector3.one;

            obj.name = objRes.name + (1000 + i);

            ActSevenCell cell = obj.GetComponent<ActSevenCell>();
            if (i < sevenResJson.s.Count)
            {
                cell.Init(sevenResJson.s[i].id, sevenResJson.s[i].num, activityId, this,rewardPanel);
            }
            else
            {
                cell.Init(i + 1, 2, activityId, this,rewardPanel);
            }
            panelCells.Add(cell);
        }
        objGrid.GetComponent<UIGrid>().repositionNow = true;
		
		Resources.UnloadUnusedAssets();
    }

    public void RefreshTopMark()
    {
        bool flag = false;
        foreach (ActSevenCell cell in panelCells)
        {
            if (cell.State == 0)    //0可以领取//
            {
                flag = true;
                break;
            }
        }
        topIcon.setMark(flag);
    }

    void setDragPanelPos()
    {
        dragPanelClip = objSevenPanel.GetComponent<UIPanel>().clipRange;

        float cellHeight = objGrid.GetComponent<UIGrid>().cellHeight;

        int temp = 0;
        for (; temp < sevenResJson.s.Count; temp++)
        {
            if (sevenResJson.s[temp].num == 0)
                break;
        }
        if (temp > SevenDaysData.getDayNum() - 3)
            temp = SevenDaysData.getDayNum() - 3;

        objSevenPanel.transform.localPosition += new Vector3(0, temp * cellHeight, 0);
        dragPanelClip.y -= temp * cellHeight;
        objSevenPanel.GetComponent<UIPanel>().clipRange = dragPanelClip;
    }
}
