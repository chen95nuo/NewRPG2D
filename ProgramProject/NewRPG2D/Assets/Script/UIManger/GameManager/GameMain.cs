using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;

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
        TTUIPage.ShowPage<UIMain>();
    }

    private void Update()
    {

    }
}
