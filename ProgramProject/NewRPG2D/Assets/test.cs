using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        long a = DateTime.Now.ToFileTime();
        Debug.Log(DateTime.FromFileTime(a));

    }

}
