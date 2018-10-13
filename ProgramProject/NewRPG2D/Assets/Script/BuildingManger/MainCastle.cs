using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCastle : Castle
{
    public static MainCastle instance;

    public Transform NewRolePoint;

    private void Awake()
    {
        instance = this;

        Init();
    }

    public void AddNewRole(GameObject role)
    {
        role.transform.parent = NewRolePoint;
        NewRolePoint.GetChild(0).localPosition = Vector3.zero;
        for (int i = 1; i < NewRolePoint.childCount; i++)
        {
            NewRolePoint.GetChild(i).localPosition = Vector3.left * 1.85f;
        }
    }
    /// <summary>
    /// 刷新建筑 在编辑模式保存后常规模式需要刷新建筑的位置
    /// </summary>
    /// <param name="allbuilding"></param>
    public void RefreshBuilding(List<LocalBuildingData> allbuilding, List<LocalBuildingData> newRoom)
    {
        ResetWall();
        for (int i = 0; i < allroom.Count; i++)
        {
            for (int j = 0; j < allbuilding.Count; j++)
            {
                if (allroom[i].currentBuildData.id == allbuilding[j].id)
                {
                    allroom[i].BuildingMove(allbuilding[j], this);
                    allroom[i].ChickLeftOrRight(buildPoint);
                }
            }
        }
        for (int i = 0; i < newRoom.Count; i++)
        {
            ChickPlayerInfo.instance.AddBuilding(newRoom[i]);
        }
    }
}
