using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCastleBg : MonoBehaviour
{

    public Transform leftUp;
    public Transform rightUp;
    public Transform right;

    public GameObject floor;
    public Transform floorGroup;
    public List<GameObject> floors;

    public GameObject rWall;
    public Transform rWallGroup;
    public List<GameObject> rWalls;

    public GameObject lWall;
    public Transform lWallGroup;
    public List<GameObject> lWalls;

    public GameObject tWall;
    public Transform tWallGroup;
    public List<GameObject> tWalls;

    public int floorsNumber;

    public void UpdateInfo(int x, int y)
    {
        int indexX = x - 17 > 0 ? x - 17 : 0;
        int indexY = y - 5 > 0 ? y - 5 : 0;
        leftUp.localPosition = new Vector3(0, 2.7f * indexY, 0);
        rightUp.localPosition = new Vector3(1.2f * indexX, 2.7f * indexY, 0);
        right.localPosition = new Vector3(1.2f * indexX, 0, 0);

        ChickNumber(floor, floors, x + 6, floorGroup);
        for (int i = 0; i < x + 6; i++)
        {
            floors[i].transform.localPosition = new Vector3(1.2f * i, 0, 0);
        }
        ChickNumber(rWall, rWalls, y, rWallGroup);
        for (int i = 0; i < y; i++)
        {
            rWalls[i].transform.localPosition = new Vector3(0, 2.7f * i, 0);
        }
        ChickNumber(lWall, lWalls, indexY, lWallGroup);
        for (int i = 0; i < indexY; i++)
        {
            lWalls[i].transform.localPosition = new Vector3(0, 2.7f * i, 0);
        }
        ChickNumber(tWall, tWalls, indexX / 3, tWallGroup);
        for (int i = 0; i < indexX / 3; i++)
        {
            tWalls[i].transform.localPosition = new Vector3(3.6f * i, 0, 0);
        }
    }

    public void ChickNumber(GameObject obj, List<GameObject> Grids, int index, Transform ts)
    {
        if (Grids.Count < index)
        {
            int temp = Grids.Count - 1;
            for (int i = Grids.Count; i < index; i++)
            {
                GameObject go = Instantiate(obj, ts) as GameObject;
                Grids.Add(go);
            }
        }
        else if (Grids.Count > index)
        {
            for (int i = index - 1; i < Grids.Count; i++)
            {
                Grids[i].transform.position = new Vector3(0, 0, 1000);
            }
        }
    }
}
