using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public string testValue = "att(4, 6)";
    string[] str;

    private void Start()
    {
        char[] ch = new char[] { '(', ',', ')' };
        str = testValue.Split(ch);
        for (int i = 0; i < str.Length - 1; i++)
        {
            Debug.Log(int.Parse(str[1]) + "  " + (int.Parse(str[2]) + 1));
        }
    }
}
