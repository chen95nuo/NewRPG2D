using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIRoleGrid : MonoBehaviour
{

    public Text txt_Name;
    public Text txt_Level;
    public Button btn_ShowAllRole;

    private bool isShow = false;
    private void Awake()
    {
        btn_ShowAllRole = GetComponent<Button>();
        btn_ShowAllRole.onClick.AddListener(ShowAllRole);
    }

    public void UpdateInfo(HallRoleData data, BuildRoomName name)
    {
        txt_Name.text = data.Name;
        txt_Level.text = data.GetArtProduce(name).ToString();
    }

    public void ShowAllRole()
    {
        UIPanelManager.instance.ShowPage<UIScreenRole>();
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
