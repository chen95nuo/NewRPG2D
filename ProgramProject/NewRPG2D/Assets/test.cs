using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        RoleGrade(1, 10, 50);
    }

    public string RoleGrade(float grade, float min, float max)
    {
        float Amin = 0;
        float Amax = 0;
        float Bmin = 0;
        float Bmax = 0;
        float Cmin = 0;
        float Cmax = 0;
        float Smin = 0;
        float Smax = 0;
        Cmin = min;
        Cmax = min + (max - min) / 10.0f * 3.0f;
        Bmin = Cmax + 0.1f;
        Bmax = Cmax + (max - min) / 10.0f * 3.0f;
        Amin = Bmax + 0.1f;
        Amax = Bmax + (max - min) / 10.0f * 3.0f;
        Smin = Amax + 0.1f;
        Smax = Amax + (max - min) / 10.0f * 1.0f;

        Debug.Log("Cmin = " + Cmin);
        Debug.Log("Cmax = " + Cmax);
        Debug.Log("Bmin = " + Bmin);
        Debug.Log("Bmax = " + Bmax);
        Debug.Log("Amin = " + Amin);
        Debug.Log("Amax = " + Amax);
        Debug.Log("Smin = " + Smin);
        Debug.Log("Smax = " + Smax);

        return null;
    }

}
