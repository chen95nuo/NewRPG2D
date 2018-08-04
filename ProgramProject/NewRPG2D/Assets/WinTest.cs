using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTest : MonoBehaviour
{
    private void Awake()
    {
        GoFightMgr.CreateInstance();
    }
    // Use this for initialization
    void Start()
    {
        Invoke("UpdateWin", 1.0f);
    }

    void UpdateWin()
    {
        //UIEventManager.instance.SendEvent(UIEventDefineEnum.MissionComplete);
        GoFightMgr.instance.MissionComplete();
    }
    private void OnDestroy()
    {
        GoFightMgr.DestroyInstance();
    }
}
