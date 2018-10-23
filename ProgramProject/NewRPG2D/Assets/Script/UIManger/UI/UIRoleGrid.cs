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
    public Image Image_Icon;
    public Image potoBg;
    public Button btn_ShowAllRole;
    public GameObject Lock;

    public Sprite[] sp;

    private bool isShow = false;
    private void Awake()
    {
        btn_ShowAllRole.onClick.AddListener(ShowAllRole);

    }

    public void UpdateInfo()
    {
        txt_Type.text = "";
        txt_Name.gameObject.SetActive(false);
        txt_Level.gameObject.SetActive(false);
        potoBg.sprite = sp[1];
    }
    public void UpdateInfo(HallRoleData data, BuildRoomName name)
    {
        potoBg.sprite = sp[0];
        Sprite icon = GetSpriteAtlas.insatnce.ChickRoomIcon(name);
        Image_Icon.sprite = icon;
        txt_Type.text = "";
        txt_Name.text = data.Name;
        txt_Name.gameObject.SetActive(true);
        txt_Level.gameObject.SetActive(true);
        txt_Level.text = data.GetArtProduce(name).ToString();
    }



    /// <summary>
    /// 卧室
    /// </summary>
    /// <param name="data"></param>
    public void UpdateInfo(HallRoleData data)
    {
        txt_Level.text = data.Name;
        txt_Level.gameObject.SetActive(true);
        txt_Name.gameObject.SetActive(false);
        if (data.LoveType == RoleLoveType.WaitFor)
        {
            txt_Type.text = "等待伴侣中";
        }
        else txt_Type.text = "";
    }

    public void ShowAllRole()
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
    }
}
