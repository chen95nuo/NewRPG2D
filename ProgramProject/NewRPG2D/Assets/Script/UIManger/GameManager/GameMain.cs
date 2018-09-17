using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;

public class GameMain : MonoBehaviour
{

    public static GameMain Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UIPanelManager.instance.ShowPage<UIMain>();
    }

    private void Update()
    {

    }
}
