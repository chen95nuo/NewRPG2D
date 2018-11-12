using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Script.UIManger;

public class UIMagicGrid : MonoBehaviour
{
    public Text txt_Tip_1;
    public Text txt_Empty;
    public Text txt_MagicLevel;
    public GameObject SliderObj;
    public GameObject LevelObj;
    public Image slider;
    public Image Icon;
    public Image lockIcon;
    public Button btn_Click;
    public Material gray;

    private MagicGridType ClickType = MagicGridType.Empty;
    private MagicData currentMagic;
    private RealMagic currentRealMagic;

    private void Awake()
    {
        txt_Empty.text = "空";
        SliderObj.SetActive(false);
        LevelObj.SetActive(false);
        txt_Tip_1.gameObject.SetActive(false);
        lockIcon.gameObject.SetActive(false);
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
                ClickUse();
                break;
            case MagicGridType.Read:
                ChickRead();
                break;
            case MagicGridType.Work:
                ChickWork();
                break;
            case MagicGridType.Lock:
                ClickLockType();
                break;
            case MagicGridType.Cry:
                ClickCryType();
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

    private void ChickWork()
    {
        UIMagicWorkShop.instance.ChickGridEvent(transform.position, ClickType, currentRealMagic);
    }

    private void ChickRead()
    {
        UIMagicWorkShop.instance.ChickGridEvent(transform.position, ClickType, currentRealMagic);
    }

    private void ClickUse()
    {
        UIMagicWorkShop.instance.ChickGridEvent(transform.position, ClickType, currentRealMagic);
    }
    public void ClickType_0()
    {
        //所有已存在的技能颤抖
        UIMagicWorkShop.instance.GridAnim(true);
    }
    private void ClickLockType()
    {
        UIPanelManager.instance.ShowPage<UIMagicMessage>();
        string st = string.Format("需要{0}级房间", currentRealMagic.magic.needWorkLevel);
        UIMagicMessage.instance.UpdateInfo(currentMagic, st, false, true, false);
    }
    public void ClickCryType()
    {
        UIPanelManager.instance.ShowPage<UIMagicMessage>();
        UIMagicMessage.instance.UpdateInfo(currentMagic, "", true, false, true);
    }

    //使用中的空技能
    public void UpdateInfo(MagicGridType ClickType = MagicGridType.Empty)
    {
        Icon.enabled = false;
        this.ClickType = ClickType;
    }

    //使用中的技能
    public void UpdateInfo(RealMagic data, MagicGridType ClickType = MagicGridType.Use)
    {
        currentRealMagic = data;
        Icon.enabled = true;
        //Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.magic.magicName.ToString());
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon("技能1");
        this.ClickType = ClickType;
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

    /// <summary>
    /// 选择一个替换的法术
    /// </summary>
    /// <param name="currentMagic"></param>
    /// <param name="v"></param>
    public void UpdateInfo(RealMagic currentMagic, bool isDrag)
    {

    }

    public void GridAnim(bool isRun)
    {

    }

    /// <summary>
    /// 刷新标准格子
    /// </summary>
    /// <param name="data"></param>
    public void UpdateInfo(MagicData data, MagicGridType type = MagicGridType.Read)
    {
        currentMagic = data;
        ClickType = type;
        lockIcon.gameObject.SetActive(false);
        txt_Tip_1.gameObject.SetActive(true);
        txt_Tip_1.text = data.magicName.ToString();
        Icon.enabled = true;
        //Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.magicName.ToString());
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon("技能1");
        Icon.material = null;
        btn_Click.image.material = null;
    }
    public void UpdateLock(MagicData data)
    {
        currentMagic = data;
        ClickType = MagicGridType.Lock;
        lockIcon.gameObject.SetActive(true);
        Icon.enabled = true;
        //Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.magicName.ToString());
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon("技能1");
        Icon.material = gray;
        btn_Click.image.material = gray;
    }
}
