using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomIdControl : MonoBehaviour
{

    public static int AddID(List<LocalBuildingData> allID)
    {
        int newID = 1;
        for (int i = 0; i < allID.Count; i++)
        {
            if (allID[i].id == newID)
            {
                newID = newID << 1;
                i = -1;
            }
        }
        if (newID == 3 || newID > 4)
        {
            Debug.Log("ID出错");
        }
        Debug.Log("NewID = " + newID);
        return newID;
    }

    public static int MergeID(LocalBuildingData id1, LocalBuildingData id2)
    {
        return id1.id + id2.id;
    }

    public static int[] SplitID(List<LocalBuildingData> allID, int index)
    {
        int[] newID = new int[index];
        for (int i = 0; i < newID.Length; i++)
        {
            newID[i] = AddID(allID);
        }
        return newID;
    }
}
