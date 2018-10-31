using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject info;
    public Image[] Icons;
    public Image slider;

    private HallRoleData roleData;

    private int maxTime = 0;

    private void Awake()
    {
        btn_LevelUp.onClick.AddListener(ChickLevelUp);
        btn_SpeedUp.onClick.AddListener(ChickSpeedUp);
        txt_Tip.text = "+1";
        txt_Tip_1.text = "已达当前上限";
        txt_Tip_2.text = "加速";

        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.ChickRoleTrain, UpdateTime);
        HallEventManager.instance.AddListener<HallRoleData>(HallEventDefineEnum.ChickRoleTrain, ChickInfo);
    }

    private void OnDisable()
    {
        ClearType();
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickRoleTrain, UpdateTime);
        HallEventManager.instance.RemoveListener<HallRoleData>(HallEventDefineEnum.ChickRoleTrain, ChickInfo);
    }

    public void ChickInfo(HallRoleData data)
    {
        if (data == roleData)
        {
            UpdateInfo(data);
        }
    }

    public void UpdateInfo()
    {
        info.SetActive(false);
    }
    public void UpdateInfo(HallRoleData data, Sprite sp)
    {
        for (int i = 0; i < Icons.Length; i++)
        {
            Icons[i].sprite = sp;
        }
        UpdateInfo(data);
    }
    public void UpdateInfo(HallRoleData data)
    {
        roleData = data;
        info.SetActive(true);
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
            string time = SystemTime.instance.TimeNormalizedOfSecond(roleTrainData.time);
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
        HallRoleMgr.instance.LevelComplete(roleData, false);

    }
}
