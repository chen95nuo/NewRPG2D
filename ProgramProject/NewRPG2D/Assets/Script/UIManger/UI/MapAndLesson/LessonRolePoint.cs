using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonRolePoint : MonoBehaviour
{

    public int Type_1 = 0;
    public int Type_2 = 0;
    public int Type_3 = 0;
    [System.NonSerialized]
    public int[] Types = new int[3];

    private void Awake()
    {
        Types[0] = Type_1;
        Types[1] = Type_2;
        Types[2] = Type_3;
    }

}
