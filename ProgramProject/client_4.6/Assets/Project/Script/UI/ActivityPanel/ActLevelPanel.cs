using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActLevelPanel : MonoBehaviour {

    public GameObject objLevelPrize;
    public GameObject objGrid;
	public GameObject rewardPanel;

    LoginSevenDayResJson sevenResJson;
    //int activityId;
    ActTopIcon topIcon;

    List<ActLevelCell> panelCells = new List<ActLevelCell>();

    Vector4 dragPanelClip;  //draggable panel clip

    const string ActLevelCellPath = "Prefabs/UI/ActivityPanel/actLevelCell";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(LoginSevenDayResJson sevenResJson, int activityId, ActTopIcon topIcon)
    {
        this.sevenResJson = sevenResJson;
        //this.activityId = activityId;
        this.topIcon = topIcon;
		this.rewardPanel.SetActive(false);

        setDragPanelPos();

        panelCells.Clear();
        Object objRes = Resources.Load(ActLevelCellPath);
        for (int i = 0; i < LevelGiftData.getNum(); i++)
        {
            GameObject obj = Instantiate(objRes) as GameObject;
            obj.transform.parent = objGrid.transform;
            obj.transform.localScale = Vector3.one;

            obj.name = objRes.name + (1000 + i);

            ActLevelCell cell = obj.GetComponent<ActLevelCell>();
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
		
		objRes = null;
		Resources.UnloadUnusedAssets();
    }

    public void RefreshTopMark()
    {
        bool flag = false;
        foreach (ActLevelCell cell in panelCells)
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
        dragPanelClip = objLevelPrize.GetComponent<UIPanel>().clipRange;

        float cellHeight = objGrid.GetComponent<UIGrid>().cellHeight;

        int temp = 0;
        for (; temp < sevenResJson.s.Count; temp++)
        {
            if (sevenResJson.s[temp].num == 0)
                break;
        }
        if (temp > LevelGiftData.getNum() - 3)
            temp = LevelGiftData.getNum() - 3;

        objLevelPrize.transform.localPosition += new Vector3(0, temp * cellHeight, 0);
        dragPanelClip.y -= temp * cellHeight;
        objLevelPrize.GetComponent<UIPanel>().clipRange = dragPanelClip;
    }
}
