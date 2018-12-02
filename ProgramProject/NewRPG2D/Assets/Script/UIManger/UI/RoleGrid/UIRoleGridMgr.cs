using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public abstract class UIRoleGridMgr : MonoBehaviour
{

    public Text txt_Name;
    public Text txt_Level;
    public Image Icon_Level;
    public Image Icon_AddRole;
    public Image Icon_RoleFace;
    public Button btn_ScreenRole;
    private bool isShow = false;
    private UIRoomInfo room;
    private int roleID = -1;

    public virtual void UpdateInfo(UIRoomInfo info)
    {
        roleID = -1;
        room = info;
        Icon_RoleFace.enabled = false;
        Icon_AddRole.enabled = true;
        ShowLevelUp(false);
        txt_Name.text = "";
    }
    public virtual void UpdateInfo(HallRoleData role, UIRoomInfo info)
    {
        roleID = role.id;
        room = info;
        Icon_AddRole.enabled = false;
        Icon_RoleFace.enabled = true;
        Icon_RoleFace.sprite = GetSpriteAtlas.insatnce.GetRoleIcon(role);
        ShowLevelUp(true, role);
        txt_Name.text = role.Name;
    }

    protected virtual void ShowLevelUp(bool isShow, HallRoleData role = null)
    {
        if (isShow)
        {
            txt_Level.gameObject.SetActive(true);
            Icon_Level.sprite = GetSpriteAtlas.insatnce.GetLevelIconToRoom(room.roomData.buildingData.RoomName);
            txt_Level.text = "+" + role.GetArtProduce(room.roomData.buildingData.RoomName).ToString();
        }
        else
        {
            txt_Level.gameObject.SetActive(false);
        }
    }

    public virtual void UIAddRole(HallRole role)
    {
        if (role.RoleData.id == roleID)
        {
            object st = "请将角色移动到其他位置";
            UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
            return;
        }
        room.RoomAddRole(role, roleID);
        UIScreenRole.instance.ShowPage();
    }

    public virtual void ShowAllRole()
    {
        UIPanelManager.instance.ShowPage<UIScreenRole>();
        UIScreenRole.instance.ShowPage();
        isShow = true;
    }
}
