using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Script.UIManger;
using DG.Tweening;

public class UIMagicGrid : MonoBehaviour
{
    public Text txt_Tip_1;
    public Text txt_Empty;
    public Text txt_MagicLevel;
    public Text txt_Time;
    public GameObject SliderObj;
    public Image LevelObj;
    public Image slider;
    public Image Icon;
    public Image lockIcon;
    public Button btn_Click;
    public Material gray;

    public MagicGridType ClickType = MagicGridType.Empty;
    private MagicData currentMagic;
    public RealMagic currentRealMagic;

    private bool isRun = false;
    private Tweener tween;

    private void Awake()
    {
        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.CryNewMagic, TimeCallBack);

        txt_Empty.text = "空";
        SliderObj.SetActive(false);
        LevelObj.gameObject.SetActive(false);
        txt_Tip_1.gameObject.SetActive(false);
        lockIcon.gameObject.SetActive(false);
        btn_Click.onClick.AddListener(ChickClick);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.CryNewMagic, TimeCallBack);
    }

    private void ChickClick()
    {
        if (isRun)
        {
            UIMagicWorkShop.instance.ChangeMagicData(this);
            return;
        }

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
                ClickCanLevelUp();
                break;
            case MagicGridType.NeedLevelUp:
                ClickNeedLevelUp();
                break;
            case MagicGridType.NeedLevel:
                object st = "请先解锁该技能";
                UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
                break;
            case MagicGridType.Nothing:
                ClickShowMessage();
                break;
            default:
                break;
        }
    }

    private void ClickShowMessage()
    {
        UIPanelManager.instance.ShowPage<UIMagicMessage>();
        UIMagicMessage.instance.UpdateInfo(currentMagic);
    }

    private void ClickNeedLevelUp()
    {
        UIPanelManager.instance.ShowPage<UIMagicMessage>();
        string st = string.Format("需要{0}级房间", currentMagic.levelUpNeed);
        UIMagicMessage.instance.UpdateInfo(currentMagic, st);
    }

    private void ClickCanLevelUp()
    {
        UIPanelManager.instance.ShowPage<UIMagicMessage>();
        UIMagicMessage.instance.UpdateInfo(currentMagic, "", true);

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
        UIMagicWorkShop.instance.GridAnim();
    }
    private void ClickLockType()
    {
        UIPanelManager.instance.ShowPage<UIMagicMessage>();
        string st = string.Format("需要{0}级房间", currentMagic.needWorkLevel);
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
        Icon.sprite = GetSpriteAtlas.insatnce.GetSkilIcon(data.magic.magicName.ToString());
        this.ClickType = ClickType;
    }

    public void TimeCallBack(int magicID)
    {
        if (ClickType != MagicGridType.Work)
        {
            return;
        }

        if (magicID == currentRealMagic.magicID)
        {
            SliderObj.SetActive(true);
            txt_Time.text = SystemTime.instance.TimeNormalizedOf(currentRealMagic.time, false);
            slider.fillAmount = ((float)currentRealMagic.magic.produceTime - (float)currentRealMagic.time) / (float)currentRealMagic.magic.produceTime;
            return;
        }
        SliderObj.SetActive(false);
    }
    /// <summary>
    /// 选择一个替换的法术
    /// </summary>
    /// <param name="currentMagic"></param>
    /// <param name="v"></param>
    public void UpdateInfo(RealMagic realMagic, bool isDrag)
    {
        currentRealMagic = realMagic;
        Icon.enabled = true;
        Icon.sprite = GetSpriteAtlas.insatnce.GetSkilIcon(realMagic.magic.magicName.ToString());
    }

    public void GridAnim(bool isRun)
    {
        this.isRun = isRun;
        if (isRun)
        {
            tween = transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            tween.SetAutoKill(false);
        }
        else
        {
            tween.Kill(true);
            transform.DOScale(Vector3.one, 0.5f);
        }
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
        txt_Tip_1.gameObject.SetActive(type == MagicGridType.CanLevelUp);
        Icon.enabled = true;
        Icon.sprite = GetSpriteAtlas.insatnce.GetSkilIcon(data.magicName.ToString());
        Icon.material = null;
        btn_Click.image.material = null;

        if (type == MagicGridType.CanLevelUp)
        {
            txt_Tip_1.text = LanguageDataMgr.instance.GetString(data.magicName.ToString());
        }
    }
    public void UpdateLock(MagicData data)
    {
        currentMagic = data;
        ClickType = MagicGridType.Lock;
        lockIcon.gameObject.SetActive(true);
        Icon.enabled = true;
        Icon.sprite = GetSpriteAtlas.insatnce.GetSkilIcon(data.magicName.ToString());
        Icon.material = gray;
        btn_Click.image.material = gray;
    }

    /// <summary>
    /// 技能升级系列
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <param name="isLevelUp">是否有正在升级的技能占用</param>
    public void UpdateLevelUpInfo(MagicData data, MagicGridType type = MagicGridType.CanLevelUp)
    {
        Debug.Log(data.magicName.ToString());
        currentMagic = data;
        ClickType = type;
        txt_Empty.text = "";
        Icon.enabled = true;
        Icon.sprite = GetSpriteAtlas.insatnce.GetSkilIcon(data.magicName.ToString());
        LevelObj.gameObject.SetActive(true);
        txt_MagicLevel.text = data.level.ToString();
        Color color = new Color(1, 1, 1, 1);
        txt_Tip_1.gameObject.SetActive(false);

        if (type == MagicGridType.NeedLevel)
        {
            Icon.material = gray;
            LevelObj.gameObject.SetActive(false);
            btn_Click.image.material = gray;
            color.a = 0.5f;
        }
        else if (type == MagicGridType.NeedLevelUp)
        {
            Icon.material = gray;
            LevelObj.material = gray;
            btn_Click.image.material = gray;
            txt_Tip_1.gameObject.SetActive(true);
            txt_Tip_1.text = string.Format("需要<color=#e3f760>{0}</color>级房间", data.needLevel);
        }
        else
        {
            Icon.material = null;
            LevelObj.material = null;
            btn_Click.image.material = null;
        }

        Icon.color = color;
        LevelObj.color = color;
        btn_Click.image.color = color;
    }
}
