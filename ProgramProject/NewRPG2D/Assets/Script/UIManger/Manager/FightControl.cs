using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightControl : MonoBehaviour
{

    private void Awake()
    {
        GoFightMgr.CreateInstance();
        DontDestroyOnLoad(this.gameObject);
        if (GoFightMgr.instance.isRound)
        {
            TinyTeam.UI.TTUIPage.ShowPage<UIRoundPage>();
            GoFightMgr.instance.isRound = false;
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
