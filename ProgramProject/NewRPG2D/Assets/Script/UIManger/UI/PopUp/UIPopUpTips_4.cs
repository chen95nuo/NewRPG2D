//主城建筑工人

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUpTips_4 : TTUIPage
{
    public Transform gridPoint;
    public GameObject grid;
    private List<UIPopUpTips_4Grid> grids = new List<UIPopUpTips_4Grid>();

    public Transform tipTs;



    public override void Show(object mData)
    {
        base.Show(mData);
        UpdateInfo();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (go == null || (go != tipTs.gameObject && go.tag != "Tip"))
            {
                ClosePage();
            }
        }
    }

    private void UpdateInfo()
    {
        List<LocalBuildingData> rooms = BuildingManager.instance.SearchCanUpgradedRoom();

        for (int i = 0; i < rooms.Count; i++)
        {
            if (grids.Count <= i)
            {
                InstanceNewGrid();
            }
            grids[i].gameObject.SetActive(true);
            grids[i].UpdateInfo(rooms[i]);
        }
        for (int i = rooms.Count; i < grids.Count; i++)
        {
            grids[i].gameObject.SetActive(false);
        }
    }

    private void InstanceNewGrid()
    {
        GameObject go = Instantiate(grid, gridPoint) as GameObject;
        UIPopUpTips_4Grid temp = go.GetComponent<UIPopUpTips_4Grid>();
        grids.Add(temp);
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }

}
