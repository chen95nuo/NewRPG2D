using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (j == 5)
                {
                    break;
                }
                Debug.Log(j);
            }
        }

    }

}
