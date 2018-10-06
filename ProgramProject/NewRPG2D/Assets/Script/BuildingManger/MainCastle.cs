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

}
