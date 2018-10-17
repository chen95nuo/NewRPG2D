using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallChildren : MonoBehaviour
{
    public RoleBabyData childData;

    private void UpdateInfo(RoleBabyData data)
    {
        childData = data;
    }
}
