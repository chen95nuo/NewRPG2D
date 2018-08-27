using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTest : MonoBehaviour
{
    private void Awake()
    {
        Invoke("Lost", 0.5f);
    }

    private void Win()
    {
        GoFightMgr.instance.MissionComplete();
    }
    private void Lost()
    {
        GoFightMgr.instance.MissionFailed();
    }
}
