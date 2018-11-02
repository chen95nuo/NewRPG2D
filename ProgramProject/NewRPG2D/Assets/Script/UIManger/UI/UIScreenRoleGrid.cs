using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIScreenRoleGrid : MonoBehaviour
{
    public UIChickHold btn_Photo;
    public Image image_TypeIcon;
    public Image roleIcon;
    public Text txt_Point;
    public Text txt_Name;
    public Text txt_Level;
    public Text txt_NeedDiamonds;
    public Text txt_Tip;
    public Text txt_Time;
    public GameObject TrainType;
    public HallRoleData currentRole;

    private void Awake()
    {
        btn_Photo.click += ChickPhoto;
    }

    public void ChickPhoto()
    {
        UIPanelManager.instance.ShowPage<UIDraggingRolePhoto>(currentRole);
        UIScreenRole.instance.sr.horizontal = false;
    }

    public void UpdateInfo(HallRoleData data, RoleAttribute needAtr)
    {
        if (data != currentRole)
        {
            HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);
        }
        currentRole = data;
        TrainType.SetActive(false);
        txt_Name.text = data.Name;
        txt_Level.text = data.GetAtrProduce(needAtr).ToString();
        roleIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.sexType.ToString());
        if (needAtr == RoleAttribute.Max)
        {
            image_TypeIcon.sprite = GetSpriteAtlas.insatnce.GetLevelIconToAtr(data.RoleLevel[6].atr);
        }
        else
        {
            image_TypeIcon.sprite = GetSpriteAtlas.insatnce.GetLevelIconToAtr(needAtr);
        }
        if (data.currentRoom != null)
        {
            txt_Point.text = data.currentRoom.RoomName.ToString();
            if (data.currentRoom.BuildingData.RoomType == RoomType.Training)
            {
                //如果是训练类的房间
                TrainType.SetActive(true);
                HallEventManager.instance.AddListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);
            }
        }
        else
        {
            txt_Point.text = "漫游";
        }
    }

    private void OnDisable()
    {
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);
    }

    public void ChickTrainTime(int RoleIndex)
    {
        RoleTrainHelper role = HallRoleMgr.instance.GetTrainRole(RoleIndex);
        if (role.roleID != currentRole.id)
        {
            return;
        }
        string time = SystemTime.instance.TimeNormalizedOfSecond(role.time);
        txt_Time.text = time;
    }
}
