using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle;
using Assets.Script.Battle.BattleData;
using Assets.Script.Utility;
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
        ReadXmlNewMgr.instance.ReadXmlByType(XmlName.RoleData, XmlName.Battle, XmlTypeEnum.Battle);
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

        role1.BornPositionType = BornPositionTypeEnum.Point01;
        role2.BornPositionType = BornPositionTypeEnum.Point02;
        role1.EquipIdList = new int[2];
        role2.EquipIdList = new int[2];
        for (int i = 0; i < 2; i++)
        {
            EquipmentRealProperty temp = EquipmentMgr.instance.CreateNewEquipment(10001 + i * 2);
          
            role1.EquipIdList[i] = temp.EquipId;
        }
        role1.sexType = SexTypeEnum.Man;
        role1.Profession = WeaponProfessionEnum.Shooter;

        for (int i = 0; i < 2; i++)
        {
            EquipmentRealProperty temp = EquipmentMgr.instance.CreateNewEquipment(10006 + i);
        
            role2.EquipIdList[i] = temp.EquipId;
        }
        role2.sexType = SexTypeEnum.Woman;
        role2.Profession = WeaponProfessionEnum.Fighter;

        List<RoleDetailData> roles = new List<RoleDetailData>
        {
            role1,
            role2,
        };
        LocalStartFight.instance.UpdateInfo(roles, MapLevelDataMgr.instance.GetXmlDataByItemId<MapLevelData>(10001));
        BattleStaticAndEnum.isGod = IsGod.isOn;
        // GoFightMgr.instance.PlayerLevel = 1;
        SceneManager.LoadScene("SceneLoad");

    }
}
