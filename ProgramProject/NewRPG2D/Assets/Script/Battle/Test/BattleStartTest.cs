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
        //ReadJsonNewMgr.instance.LoadJsonByMono(this);
    }

    // Use this for initialization
    void Start()
    {
        StartButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    public void StartGame()
    {
        RoleDetailData role1 = new RoleDetailData();//GameCardData.Instance.GetItem(Int32.Parse(Role1Field.text));
        role1.InitData();
        role1.Id = Int32.Parse(Role1Field.text);

        RoleDetailData role2 = new RoleDetailData(); //GameCardData.Instance.GetItem(Int32.Parse(Role2Field.text));
        role2.InitData();
        role2.Id = Int32.Parse(Role2Field.text);

        role1.TeamPos = 0;
        role2.TeamPos = 1;
        BattleDetailDataMgr.instance.RoleDatas = new[]
        {
            role1, role2
        };
        BattleStaticAndEnum.isGod = IsGod.isOn;
        // GoFightMgr.instance.PlayerLevel = 1;
        SceneManager.LoadScene("SceneLoad");

    }
}
