using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleStartTest : MonoBehaviour
{

    public InputField Role1Field;
    public InputField Role2Field;
    public Toggle IsGod;
    public Button StartButton;

    public void Awake()
    {
        ReadJsonNewMgr.instance.LoadJsonByMono(this);
    }

    // Use this for initialization
    void Start()
    {
        StartButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    public void StartGame()
    {
        CardData role1 = GameCardData.Instance.GetItem(Int32.Parse(Role1Field.text));
        CardData role2 = GameCardData.Instance.GetItem(Int32.Parse(Role2Field.text));
        if(role1 != null) role1.TeamPos = 0;
        if(role2 != null) role2.TeamPos = 1;
        GoFightMgr.instance.cardData = new[]
       {
            role1, role2
        };
        BattleStaticAndEnum.isGod = IsGod.isOn;
        GoFightMgr.instance.PlayerLevel = 1;
        SceneManager.LoadScene("SceneLoad");

    }
}
