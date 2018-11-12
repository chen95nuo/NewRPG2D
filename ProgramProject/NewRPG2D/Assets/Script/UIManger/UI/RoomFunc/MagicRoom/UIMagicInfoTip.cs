using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMagicInfoTip : MonoBehaviour
{
    public Text txt_tip_1;
    public Text txt_tip_2;
    public Text txt_tip_3;
    public Text txt_tip_4;
    public Text txt_tip_5;
    public Text txt_tip_6;
    public Text txt_Diamonds;

    public Button btn_Message;
    public Button btn_Add;
    public Button btn_Remove;
    public Button btn_Change;
    public Button btn_Cancel;
    public Button btn_SpeedUp;

    public UIMagicMessage magicMessage;

    private RealMagic currentMagic;
    [System.NonSerialized]
    public RectTransform rt;
    private int currentType = 0;
    private int needDiamonds;
    private bool isFull = false;
    private MagicGridType messageType;

    private void Awake()
    {
        rt = transform.transform as RectTransform;

        txt_tip_1.text = "讯息";
        txt_tip_2.text = "添加";
        txt_tip_3.text = "移除";
        txt_tip_4.text = "替换";
        txt_tip_5.text = "取消";
        txt_tip_6.text = "加速";

        btn_Message.onClick.AddListener(ChickMessage);
        btn_Add.onClick.AddListener(ChickAddMagic);
        btn_Remove.onClick.AddListener(ChickRemoveMagic);
        btn_Change.onClick.AddListener(ChickChange);
        btn_Cancel.onClick.AddListener(ChickCancel);
        btn_SpeedUp.onClick.AddListener(ChickSpeedUp);
    }
    private void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (go != null && go.transform.parent != transform)
            {
                gameObject.SetActive(false);
            }
        }
    }
    private void ChickSpeedUp()
    {
        //MagicDataMgr.instance.
    }

    private void ChickCancel()
    {
        MagicDataMgr.instance.CancelNewMagic(currentMagic.magicID);
        UIMagicWorkShop.instance.UpdateWorkMagic();
        gameObject.SetActive(false);
    }

    private void ChickChange()
    {

    }

    private void ChickRemoveMagic()
    {
        MagicDataMgr.instance.UnloadMagic(currentMagic.magicID);
        UIMagicWorkShop.instance.UpdateReadMagic();
        UIMagicWorkShop.instance.UpdateUseMagic();
        gameObject.SetActive(false);
    }

    private void ChickAddMagic()
    {
        if (isFull)
        {
            UIMagicWorkShop.instance.ChangeMagic(currentMagic);
        }
        else
        {
            MagicDataMgr.instance.LoadMagic(currentMagic.magicID);
            UIMagicWorkShop.instance.UpdateReadMagic();
            UIMagicWorkShop.instance.UpdateUseMagic();
            UIMagicWorkShop.instance.ChangeUseType();
        }
        gameObject.SetActive(false);
    }

    private void ChickMessage()
    {
        UIPanelManager.instance.ShowPage<UIMagicMessage>();
        switch (messageType)
        {
            case MagicGridType.Use:
                string st = "在战斗中使用";
                UIMagicMessage.instance.UpdateInfo(currentMagic.magic, st);
                break;
            case MagicGridType.Read:
                UIMagicMessage.instance.UpdateInfo(currentMagic.magic);
                break;
            default:
                break;
        }
        gameObject.SetActive(false);
    }

    public void UpdateInfo(RealMagic data, int type = 0)
    {
        currentMagic = data;
        currentType = type;
        switch (type)
        {
            case 0:
                btn_Remove.gameObject.SetActive(true);
                break;
            case 1:
                btn_Add.gameObject.SetActive(true);
                break;
            case 2:
                btn_Change.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void UpdateInfo(RealMagic data)
    {
        currentMagic = data;
        btn_Cancel.gameObject.SetActive(true);
        btn_SpeedUp.gameObject.SetActive(true);
        txt_Diamonds.text = ((int)Mathf.Round(data.time * 0.01f)).ToString();
    }
    private void OnEnable()
    {
        CloseAllUI();
    }
    public void CloseAllUI()
    {
        btn_Add.gameObject.SetActive(false);
        btn_Remove.gameObject.SetActive(false);
        btn_Change.gameObject.SetActive(false);
        btn_Cancel.gameObject.SetActive(false);
        btn_SpeedUp.gameObject.SetActive(false);
    }

    public void ShowUse(RealMagic data, bool isFull)
    {
        currentMagic = data;
        btn_Change.gameObject.SetActive(isFull);
        btn_Remove.gameObject.SetActive(!isFull);
        messageType = MagicGridType.Use;
    }

    public void ShowRead(RealMagic data, bool useIsFull)
    {
        isFull = useIsFull;
        currentMagic = data;
        btn_Add.gameObject.SetActive(true);
        messageType = MagicGridType.Read;
    }

    public void ShowWork(RealMagic data)
    {
        currentMagic = data;
        btn_Cancel.gameObject.SetActive(true);
        btn_SpeedUp.gameObject.SetActive(true);
        txt_Diamonds.text = "999";
        messageType = MagicGridType.Read;
    }

    public void ChickPoint(Vector2 point)
    {
        transform.position = point;
        point = rt.anchoredPosition;
        point.y += 50;
        point.y = Mathf.Clamp(point.y, 0, Screen.height);
        rt.anchoredPosition = point;
    }
}
