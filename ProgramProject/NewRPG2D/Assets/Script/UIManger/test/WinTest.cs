using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTest : MonoBehaviour
{
    private void Awake()
    {
        //GoFightMgr.CreateInstance();
        Invoke("Win", 0.5f);
    }

    private void Win()
    {
        GoFightMgr.instance.MissionComplete();
        //UIEventManager.instance.SendEvent(UIEventDefineEnum.MissionComplete);
        
    }
    private void Lost()
    {
        GoFightMgr.instance.MissionFailed();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.MissionFailed);
    }
}
