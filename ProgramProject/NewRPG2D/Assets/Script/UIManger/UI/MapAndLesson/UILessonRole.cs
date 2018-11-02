using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILessonRole : MonoBehaviour
{
    public Text txt_Tip;
    public Text txt_Name;
    public Text txt_Icon;

    public Image backGround;
    public Image sliderFill;
    public Image roleIcon;

    public GameObject LockType;

    public void UpdateInfo()
    {
        LockGrid(false);
        Empty(true);
        roleIcon.enabled = false;
        txt_Tip.gameObject.SetActive(false);
        txt_Name.gameObject.SetActive(false);
    }
    public void UpdateInfo(HallRoleData roleData)
    {
        roleIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(roleData.sexType.ToString());
        LockGrid(false);
        Empty(false);
        txt_Name.text = roleData.Name;
        sliderFill.fillAmount = roleData.NowHp / roleData.HP;
    }
    public void UpdateLocked(int index)
    {
        LockGrid(true);
        Empty(false);
        txt_Tip.text = string.Format("需要{0}级军营", index);
    }

    private void Empty(bool isShow)
    {
        backGround.enabled = !isShow;
        txt_Icon.gameObject.SetActive(isShow);
    }
    private void LockGrid(bool isShow)
    {
        roleIcon.enabled = !isShow;
        backGround.enabled = !isShow;
        txt_Name.gameObject.SetActive(!isShow);
        LockType.SetActive(isShow);
        txt_Tip.gameObject.SetActive(isShow);
    }
}
