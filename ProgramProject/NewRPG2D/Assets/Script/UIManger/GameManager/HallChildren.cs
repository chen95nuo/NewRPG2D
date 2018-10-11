using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallChildren : MonoBehaviour
{
    public RoleChildrenData childData;

    private void UpdateInfo(RoleChildrenData data)
    {
        childData = data;
    }
}
