using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleGrid : MonoBehaviour
{
    public Text txt_Type;
    public Text txt_Name;
    public Text txt_Level;
    public Text txt_LevelTip;
    public Image Image_Icon;
    public Image potoBg;
    public Image headIcon;
    public Button btn_ShowAllRole;
    public GameObject lockOBJ;

    public Sprite[] sp;
    public Sprite addRole;

    private bool isShow = false;

    private UIRoomInfo room;
    private int index = 0;

    private void Awake()
    {
        btn_ShowAllRole.onClick.AddListener(ShowAllRole);

    }

    public void UpdateInfo(UIRoomInfo roomInfo, int index)
    {
        LockType(false);
        txt_Type.text = "";
        txt_Name.gameObject.SetActive(false);
        txt_Level.gameObject.SetActive(false);
        potoBg.sprite = sp[0];
        headIcon.gameObject.SetActive(false);
    }
    public void UpdateInfo(HallRoleData data, BuildRoomName name, UIRoomInfo roomInfo, int index)
    {
        LockType(false);
        potoBg.sprite = sp[0];
        Sprite icon = GetSpriteAtlas.insatnce.GetLevelIconToRoom(name);
        Image_Icon.sprite = icon;
        txt_Type.text = "";
        txt_Name.text = data.Name;
        txt_Name.gameObject.SetActive(true);
        txt_Level.gameObject.SetActive(true);
        txt_Level.text = "+" + data.GetArtProduce(name).ToString();
        headIcon.gameObject.SetActive(true);
    }
    public void UpdateLockInfo(UIRoomInfo roomInfo, int index)
    {

        LockType(true, index + 1);
        potoBg.sprite = sp[1];
        headIcon.gameObject.SetActive(false);

    }

    public void UIAddRole(HallRole role)
    {
        room.RoomAddRole(role, index);
        UIScreenRole.instance.ShowPage();

    }

    /// <summary>
    /// 卧室
    /// </summary>
    /// <param name="data"></param>
    public void UpdateLivineRoom(HallRoleData data, UIRoomInfo roomInfo, int index)
    {
        potoBg.sprite = sp[0];
        txt_Name.text = data.Name;

        Image_Icon.enabled = true;

        if (data == null)
        {
            headIcon.sprite = addRole;
        }
        else
        {
            headIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.IconName);
        }
        if (data.LoveType == RoleLoveType.WaitFor)
        {
            txt_Type.text = "等待伴侣中";
        }
        else txt_Type.text = "";
    }
    public void UpdateLivineRoom(UIRoomInfo roomInfo, int index)
    {
        Image_Icon.enabled = false;
        txt_Name.text = "";
        txt_Type.text = "";
    }

    public void ShowAllRole()
    {
        UIPanelManager.instance.ShowPage<UIScreenRole>();
        UIScreenRole.instance.ShowPage();
        isShow = true;
    }

    private void LockType(bool isTrue, int index = 0)
    {
        lockOBJ.SetActive(isTrue);
        txt_Level.gameObject.SetActive(!isTrue);
        txt_LevelTip.gameObject.SetActive(isTrue);
        txt_LevelTip.text = string.Format("需要{0}级军营", index);
    }

    private void OnDisable()
    {
        if (isShow == true)
        {
            UIPanelManager.instance.ClosePage<UIScreenRole>();
        }
    }
}
