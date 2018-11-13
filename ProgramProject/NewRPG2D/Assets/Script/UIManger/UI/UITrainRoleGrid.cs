using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UITrainRoleGrid : MonoBehaviour
{

    public GameObject[] LevelUpType;

    public Text txt_name;
    public Text txt_level;
    public Text txt_Damionds;
    public Text txt_Time;
    public Text txt_Tip;
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Button btn_LevelUp;
    public Button btn_SpeedUp;
    public Button btn_Click;

    public GameObject info;
    public Image[] Icons;
    public Image slider;
    public Image AddRoleIcon;
    public Image roleIcon;

    private HallRoleData roleData;

    private int maxTime = 0;
    private bool isShow = false;
    private int index = 0;
    private UIRoomInfo room;

    private void Awake()
    {
        btn_LevelUp.onClick.AddListener(ChickLevelUp);
        btn_SpeedUp.onClick.AddListener(ChickSpeedUp);
        btn_Click.onClick.AddListener(ChickClick);
        txt_Tip.text = "+1";
        txt_Tip_1.text = "已达当前上限";
        txt_Tip_2.text = "加速";

        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.ChickRoleTrain, UpdateTime);
        HallEventManager.instance.AddListener<HallRoleData>(HallEventDefineEnum.ChickRoleTrain, ChickInfo);
    }

    private void ChickClick()
    {
        UIPanelManager.instance.ShowPage<UIScreenRole>();
        UIScreenRole.instance.ShowPage();
        isShow = true;
    }

    private void OnDisable()
    {
        if (isShow == true)
        {
            UIPanelManager.instance.ClosePage<UIScreenRole>();
        }
        ClearType();
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickRoleTrain, UpdateTime);
        HallEventManager.instance.RemoveListener<HallRoleData>(HallEventDefineEnum.ChickRoleTrain, ChickInfo);
    }

    public void ChickInfo(HallRoleData data)
    {
        index = data.id;
        if (data == roleData)
        {
            UpdateInfo(data, this.room);
        }
    }

    public void UpdateInfo(UIRoomInfo room)
    {
        index = -1;
        this.room = room;
        info.SetActive(false);
        AddRoleIcon.enabled = true;
        roleIcon.enabled = false;
    }
    public void UpdateInfo(HallRoleData data, Sprite sp, UIRoomInfo room)
    {
        index = data.id;
        for (int i = 0; i < Icons.Length; i++)
        {
            Icons[i].sprite = sp;
        }
        UpdateInfo(data, room);
    }
    public void UpdateInfo(HallRoleData data, UIRoomInfo room)
    {
        index = data.id;
        this.room = room;
        roleData = data;
        info.SetActive(true);
        AddRoleIcon.enabled = false;
        roleIcon.enabled = true;
        roleIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.sexType.ToString());
        txt_name.text = data.Name;
        switch (data.TrainType)
        {
            case RoleTrainType.Nothing:
                info.SetActive(false);
                break;
            case RoleTrainType.LevelUp:
                info.SetActive(true);
                TypeLevelUp(data);
                break;
            case RoleTrainType.Complete:
                info.SetActive(true);
                TypeComplete(data);
                break;
            case RoleTrainType.MaxLevel:
                info.SetActive(true);
                TypeMaxLevel(data);
                break;
            default:
                break;
        }
    }

    public void UpdateTime(int index)
    {
        if (roleData == null)
        {
            return;
        }
        RoleTrainHelper roleTrainData = HallRoleMgr.instance.FindTrainRole(index);
        if (roleTrainData.roleID == this.roleData.id)
        {
            slider.fillAmount = (roleTrainData.maxTime - roleTrainData.time) / roleTrainData.maxTime;
            string time = SystemTime.instance.TimeNormalizedOf(roleTrainData.time,false);
            txt_Time.text = time;
        }
        txt_Damionds.text = (roleTrainData.time * 0.01f).ToString("#0");
    }

    private void TypeLevelUp(HallRoleData data)
    {
        ClearType();
        LevelUpType[0].SetActive(true);
        txt_name.text = data.Name;
        int level = data.GetAtrLevel(data.currentRoom.RoomName);
        txt_level.text = level.ToString() + "+1";
        btn_SpeedUp.gameObject.SetActive(true);
    }
    private void TypeComplete(HallRoleData data)
    {
        ClearType();
        LevelUpType[1].SetActive(true);
        txt_name.text = data.Name;
        int level = data.GetAtrLevel(data.currentRoom.RoomName);
        txt_level.text = level.ToString();
        btn_SpeedUp.gameObject.SetActive(false);
    }
    private void TypeMaxLevel(HallRoleData data)
    {
        ClearType();
        LevelUpType[2].SetActive(true);
        int level = data.GetAtrLevel(data.currentRoom.RoomName);
        txt_level.text = level.ToString();
        btn_SpeedUp.gameObject.SetActive(false);
    }

    private void ClearType()
    {
        for (int i = 0; i < LevelUpType.Length; i++)
        {
            LevelUpType[i].SetActive(false);
        }
    }

    public void ChickLevelUp()
    {
        HallRoleMgr.instance.LevelComplete(roleData, true);
    }
    public void ChickSpeedUp()
    {
        PlayerData data = GetPlayerData.Instance.GetData();
        Debug.Log("扣钻石");
        if (true)
        {

        }
        HallRoleMgr.instance.LevelComplete(roleData, false);
    }

    public void UIAddRole(HallRole role)
    {
        if (role.RoleData.id == index)
        {
            object st = "请将角色移动到其他位置";
            UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
            return;
        }
        room.RoomAddRole(role, index);
        UIScreenRole.instance.ShowPage();
    }
}
