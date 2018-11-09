using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMagicGrid : MonoBehaviour
{
    public Text txt_Tip_1;
    public Text txt_Empty;
    public Text txt_MagicLevel;
    public GameObject SliderObj;
    public GameObject LevelObj;
    public Image slider;
    public Image Icon;
    public Button btn_Click;

    private MagicGridType ClickType = MagicGridType.Empty;

    private void Awake()
    {
        txt_Empty.text = "空";
        SliderObj.SetActive(false);
        LevelObj.SetActive(false);
        txt_Tip_1.gameObject.SetActive(false);
        btn_Click.onClick.AddListener(ChickClick);
    }

    private void ChickClick()
    {
        switch (ClickType)
        {
            case MagicGridType.Empty:
                ClickType_0();
                break;
            case MagicGridType.Use:
                break;
            case MagicGridType.Read:
                break;
            case MagicGridType.Work:
                break;
            case MagicGridType.Lock:
                break;
            case MagicGridType.CanLevelUp:
                break;
            case MagicGridType.NeedLevelUp:
                break;
            case MagicGridType.NeedLevel:
                break;
            default:
                break;
        }
    }
    public void ClickType_0()
    {
        //所有已存在的技能颤抖
        UIMagicWorkShop.instance.GridAnim(true, true);
    }
    public void ClickType_1()
    {
        //直接弹出信息框 只有提示没有其他
        UIMagicWorkShop.instance.
    }
    public void ClickType_2()
    {
        //返回技能数据 类型数据
    }
    public void ClickType_3()
    {
        //返回技能数据 类型数据
    }
    public void UpdateInfo(MagicGridType ClickType = MagicGridType.Empty)
    {
        Icon.enabled = false;
        this.ClickType = ClickType;
    }
    public void UpdateInfo(RealMagic data, MagicGridType ClickType = MagicGridType.Use)
    {
        Icon.enabled = true;
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.magic.magicName.ToString());
        this.ClickType = ClickType;
    }

    public void UpdateInfo(MagicData data)
    {
        txt_Tip_1.gameObject.SetActive(true);
        txt_Tip_1.text = data.magicName.ToString();
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.magicName.ToString());
    }

    public void UpdateInfo(MagicData data, int roomLevel)
    {
        if (roomLevel == 0)
        {
            txt_MagicLevel.gameObject.SetActive(false);
            btn_Click.interactable = false;
            txt_Tip_1.gameObject.SetActive(false);
        }
        txt_MagicLevel.gameObject.SetActive(true);
        txt_MagicLevel.text = (data.level - 1).ToString();
        int needLevel = data.needLevel;
        if (roomLevel >= needLevel)
        {
            btn_Click.interactable = true;
        }
        else
        {
            btn_Click.interactable = false;
        }
        if (needLevel > 0)
        {
            txt_Tip_1.gameObject.SetActive(true);
            string tip = string.Format("需要{0}级房间", needLevel);
            txt_Tip_1.text = tip;
            return;
        }
        txt_Tip_1.gameObject.SetActive(false);
    }

    public void GridShake(bool isRun)
    {

    }
}
