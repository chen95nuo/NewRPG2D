using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Invoke("UpdateWin", 1.0f);
    }

    void UpdateWin()
    {
        Debug.Log("运行了");
        UIEventManager.instance.SendEvent(UIEventDefineEnum.MissionComplete);
        GoFightMgr.instance.MissionComplete();
    }
}
