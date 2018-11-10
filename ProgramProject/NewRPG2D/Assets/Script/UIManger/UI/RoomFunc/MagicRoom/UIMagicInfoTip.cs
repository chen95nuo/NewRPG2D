using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int currentType = 0;

    private void Awake()
    {
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

    private void ChickSpeedUp()
    {
        //MagicDataMgr.instance.
    }

    private void ChickCancel()
    {
        MagicDataMgr.instance.CancelNewMagic(currentMagic.magicID);
        UIMagicWorkShop.instance.UpdateWorkMagic();
    }

    private void ChickChange()
    {

    }

    private void ChickRemoveMagic()
    {
        MagicDataMgr.instance.UnloadMagic(currentMagic.magicID);
        UIMagicWorkShop.instance.UpdateReadMagic();
        UIMagicWorkShop.instance.UpdateUseMagic();
    }

    private void ChickAddMagic()
    {

    }

    private void ChickMessage()
    {
        switch (currentType)
        {

            default:
                break;
        }
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

    public void CloseAllUI()
    {
        btn_Add.gameObject.SetActive(false);
        btn_Remove.gameObject.SetActive(false);
        btn_Change.gameObject.SetActive(false);
        btn_Cancel.gameObject.SetActive(false);
        btn_SpeedUp.gameObject.SetActive(false);
    }
}
