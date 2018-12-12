using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTop : MonoBehaviour {

    void Start()
    {
        if (ClickPoint.Instance.isShowTop)
        {
            GameObject.Find("HandTap").SetActive(true);

        }
        else {
            GameObject.Find("HandTap").SetActive(false);
        }
    }
}
